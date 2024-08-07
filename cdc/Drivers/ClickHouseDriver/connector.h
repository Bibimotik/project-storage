#ifndef CLICKHOUSEDRIVER_H
#define CLICKHOUSEDRIVER_H

#include <iostream>
#include <vector>
#include <clickhouse/client.h>
#include <ctime>

using namespace clickhouse;

namespace CLICKHOUSECONN
{
	class ClickHouseConn
	{
	private:
		Client client;

	public:
		ClickHouseConn(std::string host, std::string database);

		bool insert(const std::string& data);
		bool update(const std::string& update_query);
		bool remove(const std::string& remove_query);
		bool check_query(const std::string& query);
		
		void post_logger_message(const std::string& request, bool status);

		std::vector<std::string> select(const std::string& select_query);
	};
}

#endif
