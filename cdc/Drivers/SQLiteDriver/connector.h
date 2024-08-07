#ifndef SQLITEDRIVER_H
#define SQLITEDRIVER_H

#include <iostream>
#include <sqlite3.h>
#include <vector>

namespace SQLITECONN
{
	class Sqliteconn
	{
	private:
		std::string dbpath;
		sqlite3* db;
		char* errorMsg = nullptr;
		int rc;

	public:
		Sqliteconn(std::string dbpath);

		bool insert(const std::string data);
		bool update(const std::string update_query);
		bool remove(const std::string remove_query);
		bool check_query(const std::string query);

		void post_logger_message(const std::string& request, bool status);

		std::vector<std::string> select(const std::string select_query);
	};
}

#endif
