#ifndef LOGGER_H
#define LOGGER_H

#include "../Broker/broker.h"
#include "../Drivers/baseConnector.h"

#include <mutex>
#include <thread>
#include <vector>
#include <memory>
#include <ctime>

#include <quill/Backend.h>
#include <quill/Frontend.h>
#include <quill/LogMacros.h>
#include <quill/Logger.h>
#include <quill/sinks/FileSink.h>

namespace LOGGER
{
	template<typename T>
	class Logger
	{
	private:
		std::mutex logger_mutex;

		std::string query;

		quill::Logger* logger;

		std::shared_ptr<quill::Sink> file_sink;
		std::vector<std::shared_ptr<BROKER::Broker>> brokers;
		std::shared_ptr<CONNECTOR::BaseConnector<T>> conn;

		bool is_network = false;

		void create_quill_logger(const std::string& logpath)
		{
			quill::BackendOptions backend_options;
			quill::Backend::start(backend_options);

			file_sink = quill::Frontend::create_or_get_sink<quill::FileSink>(
				logpath,
				[]()
				{
					quill::FileSinkConfig cfg;
					cfg.set_open_mode('w');
					cfg.set_filename_append_option(quill::FilenameAppendOption::StartDateTime);
					return cfg;
				}(),
				quill::FileEventNotifier{});

    		logger = quill::Frontend::create_or_get_logger("root", std::move(file_sink),
                                      "%(time) [%(thread_id)] %(short_source_location:<28) "
                                      "LOG_%(log_level:<9) %(logger:<12) %(message)",
                                      "%H:%M:%S.%Qns", quill::Timezone::GmtTime);

    		logger->set_log_level(quill::LogLevel::Debug);
		}

	public:
		Logger(const std::vector<std::string>& brokers, std::shared_ptr<CONNECTOR::BaseConnector<T>> conn, const std::string logpath) : 
			conn(conn)
		{
			for(auto v : brokers)
			{
				this->brokers.push_back(std::make_shared<BROKER::Broker>(v));
			}

			is_network = false;
			create_quill_logger(logpath);
		};

		Logger(std::shared_ptr<CONNECTOR::BaseConnector<T>> conn, const std::string logpath) :
			conn(conn)
		{
			is_network = true;	
			create_quill_logger(logpath);
		}

		void add_error_message(const std::string& message)
		{
			LOG_ERROR(logger, "{}", message);
		}

		void add_success_message(const std::string& message)
		{
			LOG_DEBUG(logger, "{}", message);
		}

		void add_info_message(const std::string& message)
		{
			LOG_INFO(logger, "{}", message);
		}

		void add_warning_message(const std::string& message)
		{
		    LOG_WARNING(logger, "{}", message);
		}

		void post_message(std::string message, std::string request, bool status)
		{
		    std::lock_guard<std::mutex> lock(logger_mutex);

		    if(!is_network)
		    {
		    	for(auto v : this->brokers)
		    	{
		    		v->post_message(message);
		    	}
		    }
		    
		    this->conn->post_logger_message(request, status);
		}
	};
}

#endif
