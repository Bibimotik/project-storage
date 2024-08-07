#ifndef PARSER_H
#define PARSER_H

#include <algorithm>
#include <iterator>
#include <map>
#include <unordered_set>
#include <vector>
#include <sstream>

#include <mutex>

#include <iostream>

#include <nlohmann/json.hpp>

#include "hsql/SQLParser.h"
#include "hsql/util/sqlhelper.h"

#include "Sub/third.h"

using json = nlohmann::json;

namespace PARSER
{
	enum Operation
	{
		select,
		insert,
		update
	};

    class Parser
    {
    private:
        std::map<std::string, json (*)(std::string, json&)> ops {
            {"SELECT", [](std::string value, json& response) {
                response["SELECT"] = value;
                return response;
            }},
            {"INSERT", [](std::string value, json& response) {
                response["INSERT"] = value;
                return response;
            }},
            {"UPDATE", [](std::string value, json& response) {
                response["UPDATE"] = value;
                return response;
            }},
            {"INTO", [](std::string value, json& response) {
                response["INTO"] = value;
                return response;
            }},
            {"SET", [](std::string value, json& response) {
                response["SET"] = value;
                return response;
            }},
            {"VALUE", [](std::string value, json& response) {
                response["VALUE"] = value;
                return response;
            }},
            {"COLUMN", [](std::string value, json& response) {
                response["COLUMN"] = value;
                return response;
            }}
        };

        std::map<std::string, json (*)(json&, json&)> conops {
            {"WHERE", [](json& value, json& response) {
                response["WHERE"] = value;
                return response;
            }}
        };

        std::map<std::string, json (*)(std::vector<std::string>, json&)> vecops {
            {"SELECT", [](std::vector<std::string> words, json& response) {
                response["SELECT"] = json::array();
                for (const auto& word : words) 
                {
                    response["SELECT"].push_back(word);
                }
                return response;
            }},
            {"INSERT", [](std::vector<std::string> words, json& response) {
                response["INSERT"] = json::array();
                for(const auto& word : words)
                {
                    response["INSERT"].push_back(word);
                }
                return response;
            }},
            {"UPDATE", [](std::vector<std::string> words, json& response) {
                response["UPDATE"] = json::array();
                for(const auto& word : words)
                {
                    response["UPDATE"].push_back(word);
                }
                return response;
            }},
            {"COLUMNS", [](std::vector<std::string> words, json& response){
                response["COLUMNS"] = json::array();
                for(const auto& word : words)
                {
                    response["COLUMNS"].push_back(word);
                }
                return response;
            }},
            {"FROM", [](std::vector<std::string> words, json& response) {
                response["FROM"] = json::array();
                for(const auto& word : words)
                {
                    response["FROM"].push_back(word);
                }
                return response;
            }},
            {"VALUES", [](std::vector<std::string> words, json& response) {
                response["VALUES"] = json::array();
                for(const auto& word : words)
                {
                    response["VALUES"].push_back(word);
                }
                return response;
            }},
            {"WHERE", [](std::vector<std::string> words, json& response) {
                response["WHERE"] = json::array();
                for(const auto& word : words)
                {
                    response["WHERE"].push_back(word);
                }
                return response;
            }}
        };

        std::mutex response_mutex;

        void SelectParse(std::vector<std::string>& container, json& response, const hsql::SelectStatement* select);
        void InsertParse(std::vector<std::string>& container, json& response, const hsql::InsertStatement* insert);
        void UpdateParse(std::vector<std::string>& container, json& response, const hsql::UpdateStatement* update);

    public:
        Parser(){};

        SUB::Third<json, bool, int> Request_parse(std::string request);
    };
}

#endif