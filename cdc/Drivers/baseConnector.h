#ifndef BASECONNECTOR_H
#define BASECONNECTOR_H

#include <iostream>
#include <memory>
#include <vector>
#include <mutex>
#include <type_traits>
#include <concepts>

#include <nlohmann/json.hpp>

#include "ClickHouseDriver/connector.h"
#include "MongoDriver/connector.h"
#include "PostgreSQLDriver/connector.h"
#include "SQLiteDriver/connector.h"

using json = nlohmann::json;

namespace CONNECTOR
{
	template <typename T, typename U>
	concept Suitable = 
	    std::is_same_v<U, std::string> &&
	    (std::is_same_v<T, CLICKHOUSECONN::ClickHouseConn> || std::is_same_v<T, CONNECTION::PGConnection> || std::is_same_v<T, SQLITECONN::Sqliteconn>) ||
	    std::is_same_v<U, json> && 
	    (std::is_same_v<T, CONNECTION::MongoConn>);

	template<typename T>
	class BaseConnector
	{
	private:
		std::shared_ptr<T> conn;

		static BaseConnector* instance;
		static std::mutex conn_mutex;		

	public:
		BaseConnector(std::shared_ptr<T> conn) : conn(conn) {}

		BaseConnector(BaseConnector& other) = delete;

		void operator=(const BaseConnector &) = delete;

		static BaseConnector* get_instance(std::shared_ptr<T> conn)
		{
			std::lock_guard<std::mutex> lock(conn_mutex);

			if(instance == nullptr)
			{
				instance = new BaseConnector(std::move(conn));
			}

			return instance;
		}

		template <typename U>
		requires Suitable<T, U>
		bool insert(const U data)
		{
			try
			{
				return conn->insert(data);
			}
			catch(const std::exception& e)
			{
				std::cout << e.what() << std::endl;
				return false;
			}
		}

		template<typename U>
		requires Suitable<T, U>
		bool update(const U data)
		{
			try
			{
				return conn->update(data);
			}
			catch(const std::exception& e)
			{
				std::cout << e.what() << std::endl;
				return false;
			}
		}

		template <typename U>
		requires Suitable<T, U>
		bool remove(const U data)
		{
			try
			{
				return conn->remove(data);
			}
			catch(const std::exception& e)
			{
				std::cout << e.what() << std::endl;
				return false;
			}
		}

		template <typename U>
		requires Suitable<T, U>
		bool check_query(const U data)
		{
			try
			{
				return conn->check_query(data);
			}
			catch(const std::exception& e)
			{
				std::cout << e.what() << std::endl;
				return false;
			}
		}

		std::vector<std::string> select(const std::string select_query)
		{
			try
			{
				return conn->select(select_query);
			}
			catch (const std::exception& e)
			{
				std::cout << e.what() << std::endl;
				return {};
			}
		}

		void post_logger_message(const std::string& request, bool status)
		{
			try
			{
				return conn->post_logger_message(request, status);
			}
			catch(const std::exception& e)
			{
				std::cout << e.what() << std::endl;
			}
		}
	};
}

#endif
