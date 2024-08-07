#include "parser.h"

namespace PARSER
{
    void Parser::SelectParse(std::vector<std::string>& container, json& response, const hsql::SelectStatement* select)
    {
        try
        {
            if(select->selectList != nullptr)
            {
                for(hsql::Expr* expr : *(select->selectList))
                {
                    if(expr->alias != nullptr)
                    {
                        std::string columnName = expr->alias;
                        container.push_back(columnName);
                    }
                    else if(expr->name != nullptr)
                    {
                        std::string columnName = expr->name;
                        container.push_back(columnName);
                    }
                    else
                    {
                        container.push_back({"*"});
                    }
                }

                vecops["SELECT"](container, response);
                container.clear();
            }

            if (select->fromTable != nullptr)
            {
                if(select->fromTable->list != nullptr)
                {
                    for(hsql::TableRef* tableRef : *(select->fromTable->list))
                    {
                        std::string tableName = tableRef->getName();

                        container.push_back(tableName);
                    }
                }
                else if(select->fromTable->name != nullptr)
                {
                    container.push_back(select->fromTable->name);
                }
                else if(select->fromTable->alias != nullptr)
                {
                    container.push_back(select->fromTable->alias->name);
                }
            }

            vecops["FROM"](container, response);
            container.clear();
        }
        catch (const std::exception& e)
        {
            container.clear();
        }
    }

    void Parser::InsertParse(std::vector<std::string>& container, json& response, const hsql::InsertStatement* insert)
    {
        try
        {
            if(insert->tableName != nullptr)
            {
                ops["INSERT"](std::string(insert->tableName), response);
            }
            
            if(insert->columns != nullptr)
            {
                for(const auto* col : *insert->columns)
                {
                    container.push_back(col);
                }

                vecops["COLUMNS"](container, response);
                container.clear();
            }
            
            if(insert->values != nullptr)
            {
                for(hsql::Expr* expr : *(insert->values))
                {
                    if(expr->alias != nullptr)
                    {
                        container.push_back(expr->alias);
                    }
                    else if(expr->name != nullptr)
                    {
                        container.push_back(expr->name);
                    }
                    else if(expr->fval != 0.0)
                    {
                        container.push_back(std::to_string(expr->fval));
                    }
                    else if(expr->ival != 0)
                    {
                        container.push_back(std::to_string(expr->ival));
                    }
                }

                vecops["VALUES"](container, response);
                container.clear();
            }
            
            if(insert->select != nullptr)
            {
                const hsql::SelectStatement* select = insert->select;
                SelectParse(container, response, select);
            }
        }
        catch (const std::exception& e)
        {
            container.clear();
        }
    }

    void Parser::UpdateParse(std::vector<std::string>& container, json& response, const hsql::UpdateStatement* update)
    {
        try
        {
            json sub;

            if(update->table->name != nullptr)
            {
                ops["UPDATE"](update->table->name, response);
            }

            if(update->updates != nullptr)
            {
                for(const auto* clause : *(update->updates))
                {
                    if(clause->column != nullptr)
                    {
                        container.push_back(std::string(clause->column));
                    }
                }

                vecops["COLUMNS"](container, response);
                container.clear();

                for(const auto* clause : *(update->updates))
                {
                    if(clause->value->name != nullptr)
                    {
                        container.push_back(clause->value->name);
                    }

                    if(clause->value->alias != nullptr)
                    {
                        container.push_back(clause->value->alias);
                    }

                    if(clause->value->fval != 0.0)
                    {
                        container.push_back(std::to_string(clause->value->fval));
                    }

                    if(clause->value->ival != 0)
                    {
                        container.push_back(std::to_string(clause->value->ival));
                    }
                }

                vecops["VALUES"](container, response);
                container.clear();
            }
        }
        catch (const std::exception& e)
        {
            container.clear();
        }
    }

    SUB::Third<json, bool, int> Parser::Request_parse(std::string request)
    {
        try
        {
            std::lock_guard<std::mutex> lock(response_mutex);
            json response;
            Operation oper;

            hsql::SQLParserResult result;
            hsql::SQLParser::parse(request, &result);

            std::vector<std::string> container;

            if (result.isValid() && result.size() > 0)
            {
                const hsql::SQLStatement* statement = result.getStatement(0);
                if (statement->isType(hsql::kStmtSelect))
                {
                    const hsql::SelectStatement* select = static_cast<const hsql::SelectStatement*>(statement);
                    SelectParse(container, response, select);
                    oper = Operation::select;
                }
                else if(statement->isType(hsql::kStmtInsert))
                {
                    const hsql::InsertStatement* insert = static_cast<const hsql::InsertStatement*>(statement);
                    InsertParse(container, response, insert);
                    oper = Operation::insert;
                }
                else if(statement->isType(hsql::kStmtUpdate))
                {
                    const hsql::UpdateStatement* update = static_cast<const hsql::UpdateStatement*>(statement);
                    UpdateParse(container, response, update);
                    oper = Operation::update;
                }
            }

            std::string jsonStr = response.dump(4);
            std::cout << jsonStr << std::endl;
            bool success = !jsonStr.empty();

            return SUB::Third<json, bool, int>::make_third(response, success, int(oper));
        }
        catch (const std::exception& e)
        {
            return SUB::Third<json, bool, int>::make_third("{}", false, -1);
        }
    }
}