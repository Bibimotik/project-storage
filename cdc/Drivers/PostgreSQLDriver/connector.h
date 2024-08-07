#ifndef PGCONNECTION_H
#define PGCONNECTION_H

#include <memory>
#include <atomic>
#include <mutex>
#include <thread>
#include <iostream>
#include <libpq-fe.h>
#include <queue>
#include <condition_variable>
#include <vector>

namespace CONNECTION
{
	class PGConnection
	{
	private:
	    void freeConnection(std::shared_ptr<PGconn> connection);
	    std::mutex connection_mutex;

	public:
	    std::shared_ptr<PGconn> conn;
	    PGresult* res;

	    PGConnection(std::string config);

	    std::shared_ptr<PGconn> connection() const;

	    bool insert(const std::string data);
	    bool update(const std::string data);
	    bool remove(const std::string query);
	    bool check_query(const std::string query);

	    void post_logger_message(const std::string& request, bool status);

	    std::vector<std::string> select(const std::string query);
	};
}

#endif
