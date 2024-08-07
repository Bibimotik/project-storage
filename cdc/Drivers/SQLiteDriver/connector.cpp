#include "connector.h"

namespace SQLITECONN
{
	Sqliteconn::Sqliteconn(std::string dbpath)
	{
		try
		{
			this->dbpath = dbpath;
			rc = sqlite3_open(this->dbpath.c_str(), &db);

			if(rc != SQLITE_OK)
			{
				std::cout << "Ошибка при открытии базы данных: " << sqlite3_errmsg(db) << '\n';
			}
		}
		catch (const std::exception& e)
		{
			std::cout << e.what();
		}
	}
	
	bool Sqliteconn::insert(const std::string data)
	{
	    sqlite3_exec(db, "BEGIN TRANSACTION", nullptr, nullptr, &errorMsg);

	    try
	    {
	        rc = sqlite3_exec(db, data.c_str(), nullptr, nullptr, &errorMsg);

	        if(rc != SQLITE_OK)
	        {
	            std::cout << "Error executing insert query: " << errorMsg << std::endl;
	            sqlite3_exec(db, "ROLLBACK", nullptr, nullptr, &errorMsg);
	            return false;
	        }

	        sqlite3_exec(db, "COMMIT", nullptr, nullptr, &errorMsg);
	        return true;
	    }
	    catch (const std::exception& e)
	    {
	        std::cout << e.what() << std::endl;
	        sqlite3_exec(db, "ROLLBACK", nullptr, nullptr, &errorMsg);
	        return false;
	    }
	}

	bool Sqliteconn::check_query(const std::string query)
	{
	    sqlite3_stmt* stmt;
	    const char* tail;

	    rc = sqlite3_prepare_v2(db, query.c_str(), -1, &stmt, &tail);

	    if (rc != SQLITE_OK)
	    {
	        std::cout << "Error preparing query: " << sqlite3_errmsg(db) << std::endl;
	        return false;
	    }

	    sqlite3_finalize(stmt);
	    return true;
	}

	bool Sqliteconn::update(const std::string update_query)
	{
	    sqlite3_exec(db, "BEGIN TRANSACTION", nullptr, nullptr, &errorMsg);

	    try
	    {
	        rc = sqlite3_exec(db, update_query.c_str(), nullptr, nullptr, &errorMsg);

	        if(rc != SQLITE_OK)
	        {
	            std::cout << "Error executing update query: " << errorMsg << std::endl;
	            sqlite3_exec(db, "ROLLBACK", nullptr, nullptr, &errorMsg);
	            return false;
	        }

	        sqlite3_exec(db, "COMMIT", nullptr, nullptr, &errorMsg);
	        return true;
	    }
	    catch (const std::exception& e)
	    {
	        std::cout << e.what() << std::endl;
	        sqlite3_exec(db, "ROLLBACK", nullptr, nullptr, &errorMsg);
	        return false;
	    }
	}

	bool Sqliteconn::remove(const std::string remove_query)
	{
	    sqlite3_exec(db, "BEGIN TRANSACTION", nullptr, nullptr, &errorMsg);

	    try
	    {
	        rc = sqlite3_exec(db, remove_query.c_str(), nullptr, nullptr, &errorMsg);

	        if(rc != SQLITE_OK)
	        {
	            std::cout << "Error executing remove query: " << errorMsg << std::endl;
	            sqlite3_exec(db, "ROLLBACK", nullptr, nullptr, &errorMsg);
	            return false;
	        }

	        sqlite3_exec(db, "COMMIT", nullptr, nullptr, &errorMsg);
	        return true;
	    }
	    catch (const std::exception& e)
	    {
	        std::cout << e.what() << std::endl;
	        sqlite3_exec(db, "ROLLBACK", nullptr, nullptr, &errorMsg);
	        return false;
	    }
	}

	void Sqliteconn::post_logger_message(const std::string& request, bool status)
	{
		try
		{
		    time_t now = time(NULL);
		    char* nowtime = ctime(&now);
		    std::string timeString(nowtime);

			std::string query = "INSERT INTO Logger (mes_time, mes_title, mes_status) VALUES ($1, $2, $3)";
			const char* paramValues[3];
			paramValues[0] = timeString.c_str();
			paramValues[1] = request.c_str();
			paramValues[2] = status ? "true" : "false";

			const int paramLengths[3] = { static_cast<int>(timeString.length()), static_cast<int>(request.length()), 5 };
			const int paramFormats[3] = { 0, 0, 0 };

			rc = sqlite3_exec(db, query.c_str(), nullptr,  nullptr, &errorMsg);

			if(rc != SQLITE_OK)
			{
				std::cout << "Error: " << errorMsg << std::endl;
			}
		}
		catch(const std::exception& e)
		{
			std::cout << e.what() << std::endl;
		}
	}

	std::vector<std::string> Sqliteconn::select(const std::string select_query)
	{
	    std::vector<std::string> results;

	    try
	    {
	        rc = sqlite3_exec(db, select_query.c_str(),
	            [](void* data, int argc, char** argv, char** azColName) -> int {
	                std::vector<std::string>& results = *static_cast<std::vector<std::string>*>(data);

	                for (int i = 0; i < argc; i++)
	                {
	                    results.push_back(argv[i]);
	                }

	                return 0;
	            },
	            &results, &errorMsg);

	        if (rc != SQLITE_OK)
	        {
	            std::cout << "Error: " << errorMsg << std::endl;
	        }
	    }
	    catch (const std::exception& e)
	    {
	        std::cout << e.what() << std::endl;
	    }

	    return results;
	}
}