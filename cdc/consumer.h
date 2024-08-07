#ifndef CONSUMER_H
#define CONSUMER_H

#include "parser.h"
#include "Parsers/JsonParser.h"
#include "Drivers/baseConnector.h"
#include "Broker/broker.h"

#include "Logger/Logger.h"

#include <iostream>
#include <cppkafka/cppkafka.h>
#include <nlohmann/json.hpp>
#include <memory>

#include <future>

#include <map>

namespace CONSUMER
{
	template<typename T, typename L>
	class Consumer
	{
	private:
		bool replicate_in_database;

		std::vector<std::thread> pool;

		cppkafka::Configuration config;
		cppkafka::Consumer consumer;

		std::string topic;

		std::mutex consumer_mutex;

		std::unique_ptr<PARSER::Parser> parser;
		std::shared_ptr<CONNECTOR::BaseConnector<T>> conn;
		std::shared_ptr<LOGGER::Logger<L>> logger;
		std::vector<std::shared_ptr<BROKER::Broker>> brokers;

        std::map<std::string, json (*)(const std::vector<std::string>, json&)> vecops {
            {"COLUMNS", [](std::vector<std::string> words, json& response) {
                response["COLUMNS"] = json::array();
                for (const auto& word : words) 
                {
                    response["COLUMNS"].push_back(word);
                }
                return response;
            }}
        };
        

		void delegate_message(int oper, const std::string& msg, const json& json_response) {
		    bool success = false;
		    std::string log_prefix;

		    std::future<void> async_task;

		    switch (oper)
		    {
		        case 0:
		        {
		            std::cout << 0 << std::endl;
		            if (replicate_in_database)
		            {
		                async_task = std::async(std::launch::async, [&](const std::string& msg, bool& success, std::string& log_prefix)
		                {
		                    auto select_future = std::async(std::launch::async, &CONNECTOR::BaseConnector<T>::select, conn, msg);
		                    std::vector<std::string> res = select_future.get();

		                    if (!res.empty())
		                    {
		                        success = true;
		                        log_prefix = "[OKEY]";
		                        json response;

		                        vecops["COLUMNS"](res, response);
		                        res.clear();

		                        std::cout << response.dump(4) << std::endl;

		                        for(auto broker : brokers)
		                        {
		                        	broker->post_message(response);
		                        }
		                    }
		                    else
		                    {
		                        log_prefix = "[ERROR]";
		                    }
		                }, msg, std::ref(success), std::ref(log_prefix));
		            }
		            else
		            {
		                async_task = std::async(std::launch::async, [&](const std::string& msg, bool& success, std::string& log_prefix)
		                {
		                    auto res = std::async(std::launch::async, &CONNECTOR::BaseConnector<T>::template check_query<std::string>, conn, msg);
		                    success  = res.get(); 
		                    log_prefix = success ? "[OKEY]" : "[ERROR]";
		                }, msg, std::ref(success), std::ref(log_prefix));
		            }
		            break;
		        }
		        case 1:
		        {
		            if (replicate_in_database)
		            {
		                async_task = std::async(std::launch::async, [&](const std::string& msg, bool& success, std::string& log_prefix)
		                {
		                    auto logger_future = std::async(std::launch::async, &LOGGER::Logger<L>::add_info_message, logger, "replicate update query");
		                    auto res           = std::async(std::launch::async, &CONNECTOR::BaseConnector<T>::template update<std::string>, conn, msg);
		                    success = res.get();
		                    logger_future.get();
		                    log_prefix = success ? "[OKEY]" : "[ERROR]";
		                }, msg, std::ref(success), std::ref(log_prefix));
		            }
		            else
		            {
		                async_task = std::async(std::launch::async, [&](const std::string& msg, bool& success, std::string& log_prefix)
		                {
		                    auto logger_future = std::async(std::launch::async, &LOGGER::Logger<L>::add_info_message, logger, "check update query");
		                    auto res           = std::async(std::launch::async, &CONNECTOR::BaseConnector<T>::template check_query<std::string>, conn, msg);
		                    success = res.get();
		                    logger_future.get();
		                    log_prefix = success ? "[OKEY]" : "[ERROR]";
		                }, msg, std::ref(success), std::ref(log_prefix));
		            }
		            break;
		        }
		        case 2:
		        {
		            if (replicate_in_database)
		            {
		                async_task = std::async(std::launch::async, [&](const std::string& msg, bool& success, std::string& log_prefix) {
		                    auto logger_future = std::async(std::launch::async, &LOGGER::Logger<L>::add_info_message, logger, "replicate insert query");
		                    auto res = std::async(std::launch::async, &CONNECTOR::BaseConnector<T>::template insert<std::string>, conn, msg);
		                    success = res.get();
		                    logger_future.get();
		                    log_prefix = success ? "[OKEY]" : "[ERROR]";
		                }, msg, std::ref(success), std::ref(log_prefix));
		            }
		            else
		            {
		                async_task = std::async(std::launch::async, [&](const std::string& msg, bool& success, std::string& log_prefix) {
		                    auto logger_future = std::async(std::launch::async, &LOGGER::Logger<L>::add_info_message, logger, "check insert query");
		                    auto res           = std::async(std::launch::async, &CONNECTOR::BaseConnector<T>::template check_query<std::string>, conn, msg);
		                    success = res.get();
		                    logger_future.get();
		                    log_prefix = success ? "[OKEY]" : "[ERROR]";
		                }, msg, std::ref(success), std::ref(log_prefix));
		            }
		            break;
		        }
		        case 3:
		        {
		            if (replicate_in_database)
		            {
		                async_task = std::async(std::launch::async, [&](const std::string& msg, bool& success, std::string& log_prefix) {
		                    auto logger_future = std::async(std::launch::async, &LOGGER::Logger<L>::add_info_message, logger, "replicate delete query");
		                    auto res           = std::async(std::launch::async, &CONNECTOR::BaseConnector<T>::template remove<std::string>, conn, msg);
		                    success = res.get();
		                    logger_future.get();
		                    log_prefix = success ? "[OKEY]" : "[ERROR]";
		                }, msg, std::ref(success), std::ref(log_prefix));
		            }
		            else
		            {
		                async_task = std::async(std::launch::async, [&](const std::string& msg, bool& success, std::string& log_prefix) {
		                    auto logger_future = std::async(std::launch::async, &LOGGER::Logger<L>::add_info_message, logger, "check delete query");
		                    auto res           = std::async(std::launch::async, &CONNECTOR::BaseConnector<T>::template check_query<std::string>, conn, msg);
		                    success = res.get();
		                    logger_future.get();
		                    log_prefix = success ? "[OKEY]" : "[ERROR]";
		                }, msg, std::ref(success), std::ref(log_prefix));
		            }
		            break;
		        }
		        default:
		            log_prefix = "[ERROR]";
		            break;
		    }

		    async_task.wait();

		    auto logger_future = std::async(std::launch::async, &LOGGER::Logger<L>::post_message, logger, log_prefix + " " + msg, msg, success);
		    logger_future.get();

			if (success)
			{
			    std::vector<std::future<void>> brokerFutures;
			    for (auto broker : brokers) 
			   	{
			        brokerFutures.push_back(
			            std::async(std::launch::async, [](const std::shared_ptr<BROKER::Broker>& broker, const std::string& message) {
			                broker->post_message(message);
			            }, broker, msg)
			        );
			    }

			    for (auto& future : brokerFutures) 
			    {
			        future.get();
			    }
			}
		}

	public:
		Consumer(const std::shared_ptr<CONNECTOR::BaseConnector<T>>& conn, 
				 const std::shared_ptr<LOGGER::Logger<L>>& logger, 
				 const std::vector<std::string>& broker_topics,
				 const std::string& topic,
				 bool replicate_in_database)
			: config{
			    {"metadata.broker.list", "localhost:9092"},
			    {"enable.auto.commit", "false"},
			    {"group.id", "test_group"}
			},
			consumer(config),
			topic(topic),
			logger(logger),
			parser(std::make_unique<PARSER::Parser>()),
			conn(conn.get()),
			replicate_in_database(replicate_in_database)
		{
			for (const auto& broker_topic : broker_topics)
			{
				brokers.push_back(std::make_shared<BROKER::Broker>(broker_topic));
			}
		}

	    void parserThread(std::unique_ptr<PARSER::Parser> parser, std::string request)
		{
			parser->Request_parse(request);
		}

	    void get_message()
	    {
	    	try
	    	{
		        this->consumer.subscribe({topic});

		        while(true)
		        {
		            cppkafka::Message msg = this->consumer.poll();

		            if (msg)
		            {
		            	std::lock_guard<std::mutex> lock(consumer_mutex);
		                if (msg.get_error()) 
		                {

		                    std::cout << "[" << "\033[31m" << "Error" << "\033[0m" << "] get kafka message" << std::endl;
		                }
		                else 
		                {
		                    std::cout << msg.get_payload() << std::endl;
		                    SUB::Third<json, bool, int> result = parser->Request_parse(msg.get_payload());

							if (result.second)
							{
								auto task = [this, &result, &msg]() {
									delegate_message(result.third, msg.get_payload(), result.first);
								};
								

							    pool.push_back(std::thread(task)); 
							
			                }
			            }
			        }
	    		}
	    	}
	    	catch (const std::exception& e)
	    	{
	    		std::cout << "1: " << e.what() << std::endl;
	    		logger->add_error_message(e.what());
	    	}
	    }

	    ~Consumer()
	    {
	    	for(int i = 0; i < pool.size(); ++i)
	    	{
	    		if(pool.at(i).joinable())
	    		{
	    			pool.at(i).join();
	    		}
	    	}
	    }
	};
}

#endif