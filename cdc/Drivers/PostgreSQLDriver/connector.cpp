#include "connector.h"

namespace CONNECTION
{
	PGConnection::PGConnection(std::string config)
	    : conn(PQconnectdb(config.c_str()), &PQfinish)
	{
	    if (PQstatus(conn.get()) != CONNECTION_OK)
	    {
	        std::cout << "[ERROR] Connection failed" << std::endl;
	    }
	}

	bool PGConnection::check_query(const std::string query)
	{
	    PGresult* temp_res = PQexec(conn.get(), query.c_str());

	    if (PQresultStatus(temp_res) != PGRES_COMMAND_OK)
	    {
	        std::cout << "Error in query: " << PQerrorMessage(conn.get()) << std::endl;
	        PQclear(temp_res);
	        return false;
	    }

	    PQclear(temp_res);
	    return true;
	}

	bool PGConnection::insert(const std::string data)
	{
		try
		{
			PQsendQuery(this->conn.get(), data.c_str());

			while(this->res = PQgetResult(this->conn.get()))
			{
				if(PQresultStatus(this->res) == PGRES_FATAL_ERROR)
				{
					std::cout << "[ERROR] " << PQresultErrorMessage(this->res) << std::endl;
					return false;
				}
			}

			return true;
		}
	    catch (const std::exception& e) 
	    {
	        std::cerr << "Exception: " << e.what() << std::endl;
	        return false;
	    }
	}

	bool PGConnection::remove(const std::string query)
	{
		try
		{
			PQsendQuery(this->conn.get(), query.c_str());

			while(this->res = PQgetResult(this->conn.get()))
			{
				if(PQresultStatus(this->res) == PGRES_FATAL_ERROR)
				{
					std::cout << "[ERROR] " << PQresultErrorMessage(this->res) << std::endl;
					return false;
				}
			}

			return true;

		}	
		catch (const std::exception& e)
		{
	        std::cerr << "Exception: " << e.what() << std::endl;
	        return false;
		}
	}

	bool PGConnection::update(const std::string data)
	{
		try
		{
			PQsendQuery(this->conn.get(), data.c_str());

			while(this->res = PQgetResult(this->conn.get()))
			{
				if(PQresultStatus(this->res) == PGRES_FATAL_ERROR)
				{
					std::cout << "[ERROR] " << PQresultErrorMessage(this->res) << std::endl;
					return false;
				}
			}

			return true;
		}
	    catch (const std::exception& e) 
	    {
	        std::cerr << "Exception: " << e.what() << std::endl;
	        return false;
	    }
	}

	std::vector<std::string> PGConnection::select(const std::string query)
	{
		std::lock_guard<std::mutex> lock(connection_mutex);
		std::vector<std::string> result;

		try
		{
			res = PQexec(this->conn.get(), query.c_str());

			if(PQresultStatus(res) != PGRES_TUPLES_OK)
			{
				std::cout << "[ERROR] query execution failed" << std::endl;

				PQclear(res);
				PQfinish(conn.get());

				return result;
			}

			int numRows = PQntuples(res);
			int numCols = PQnfields(res);

			for(int row = 0; row < numRows; ++row)
			{
				for (int col = 0; col < numCols; ++col) 
				{
					std::string value = PQgetvalue(res, row, col);
					result.push_back(value);
				}
			}

			PQclear(res);
		}
		catch(const std::exception& e)
		{
			std::cerr << "Exception: " << e.what() << std::endl;
			result.clear();
		}

		return result;
	}

	void PGConnection::post_logger_message(const std::string& request, bool status)
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

			PQexecParams(conn.get(), query.c_str(), 3, NULL, paramValues, paramLengths, paramFormats, 0);

		    while (res = PQgetResult(conn.get()))
		    {
		        if (PQresultStatus(res) == PGRES_FATAL_ERROR)
		        {
		            std::cout << "[ERROR] " << PQresultErrorMessage(res) << std::endl;
		        }
		    }
		}
		catch(const std::exception& e)
		{
			std::cout << e.what() << std::endl;
		}
	}
}