#include "connector.h"

namespace CLICKHOUSECONN
{
	ClickHouseConn::ClickHouseConn(std::string host, std::string database) : client(ClientOptions().SetHost(host))
	{
		try
		{
			client.Execute("CREATE DATABASE IF NOT EXISTS " + database + ";");
		}
		catch(const std::exception& e)
		{
			std::cout << e.what() << std::endl;
		}
	}

bool ClickHouseConn::check_query(const std::string& query)
{
    try
    {
        client.Execute("SET readonly = 1");

        client.Execute(query);

        client.Execute("SET readonly = 0");

        return true;
    }
    catch (const std::exception& e)
    {
        std::cerr << "Ошибка при выполнении запроса: " << e.what() << std::endl;
        return false;
    }
}

	bool ClickHouseConn::insert(const std::string& data)
	{
		try
		{
			client.Execute(data);
			return true;
		}
		catch(const std::exception& e)
		{
			std::cout << e.what() << std::endl;
			return false;
		}
	}

	bool ClickHouseConn::update(const std::string& update_query)
	{
		try
		{
			client.Execute(update_query);
			return true;
		}
		catch(const std::exception& e)
		{
			std::cout << e.what() << std::endl;
			return false;
		}
	}

	bool ClickHouseConn::remove(const std::string& remove_query)
	{
		try
		{
			client.Execute(remove_query);
			return true;
		}
		catch(const std::exception& e)
		{
			std::cout << e.what() << std::endl;
			return false;
		}
	}

	void ClickHouseConn::post_logger_message(const std::string& request, bool status)
	{
		try
		{
			time_t now = time(NULL);
			char* nowtime = ctime(&now);
			std::string timeString(nowtime);

			std::string query = "INSERT INTO Logger (mes_time, mes_title, mes_status) VALUES ('" + timeString + "', '" + request + "', '" + (status ? "true" : "false") + "')";

			client.Execute(query);
		}
		catch(const std::exception& e)
		{
			std::cout << e.what() << std::endl;
		}
	}

	std::vector<std::string> ClickHouseConn::select(const std::string& select_query)
	{
	    std::vector<std::string> results;

	    try
	    {
	        client.Select(select_query, [&results](const Block& block)
	        {
	            size_t num_columns = block.GetColumnCount();

	            for (size_t i = 0; i < block.GetRowCount(); ++i)
	            {
	                std::string row_data;

	                for (size_t j = 0; j < num_columns; ++j)
	                {
	                    const auto& column_type = block[j]->GetType();

	                    std::string value;
	                    if (column_type.GetName() == "UInt64")
	                    {
	                        value = std::to_string(block[j]->As<ColumnUInt64>()->At(i));
	                    }
	                    else if (column_type.GetName() == "String")
	                    {
	                        value = block[j]->As<ColumnString>()->At(i);
	                    }
	                    else
	                    {

	                    }

	                    row_data += value + " ";
	                }

	                results.push_back(row_data);
	            }
	        });
	    }
	    catch(const std::exception& e)
	    {
	        std::cout << e.what() << std::endl;
	    }

	    return results;
	}
}