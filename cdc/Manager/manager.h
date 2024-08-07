#ifndef MANAGER_H
#define MANAGER_H

#include "../Drivers/baseConnector.h"
#include "../Drivers/PostgreSQLDriver/connector.h"
#include "../Drivers/SQLiteDriver/connector.h"
#include "../Drivers/ClickHouseDriver/connector.h"

#include "../Broker/broker.h"
#include "../Network/network.h"
#include "../Logger/Logger.h"
#include "../consumer.h"

namespace MANAGER
{
    template<typename L, typename N>
    class Manager
    {
    private:
        bool replicate_in_database = true;
        std::string contopic;

        std::shared_ptr<BROKER::Broker> broker;
        std::shared_ptr<BROKER::Broker> logbroker;
        std::shared_ptr<BROKER::Broker> servbroker;

        std::shared_ptr<N> dbconn;

        std::shared_ptr<CONNECTOR::BaseConnector<L>> logdb;
        std::shared_ptr<CONNECTOR::BaseConnector<N>> conn;

        std::vector<std::unique_ptr<CONSUMER::Consumer<N, L>>> consumers;

        std::unique_ptr<CONSUMER::Consumer<N, L>> consumer;
        std::unique_ptr<NETWORK::Server<N, L>> server;

        std::vector<std::string> brokers;
        std::vector<std::string> logtopics;
        std::vector<std::string> listen;

        template<typename T, typename U>
        void consumerThread(std::unique_ptr<CONSUMER::Consumer<T, U>> consumer)
        {
            consumer->get_message();
        }

    public:
        std::shared_ptr<LOGGER::Logger<L>> logger;

        void add_brokers(const std::vector<std::string>& listen, const std::vector<std::string>& notif, const std::vector<std::string>& logtopics)
        {
            try
            {
                this->brokers = notif;
                this->logtopics = logtopics;
                this->listen = listen;
            }
            catch (const std::exception& e)
            {
                std::cout << e.what() << std::endl;
            }
        }

        void create_logger(std::string& config, const std::string& logpath, std::shared_ptr<CONNECTOR::BaseConnector<L>> logdb, bool is_network)
        {
            try
            {
                this->logdb = logdb;

                if(is_network)
                {
                    logger = std::make_shared<LOGGER::Logger<L>>(this->logdb, logpath);
                }
                else
                {
                    logger = std::make_shared<LOGGER::Logger<L>>(logtopics, this->logdb, logpath);
                }
                
                logger->add_success_message("Created logger");
            }
            catch (const std::exception& e)
            {
                std::cout << e.what() << std::endl;
                logger->add_error_message(e.what());
            }
        }

        void set_replicate_in_database(bool buf)
        {
            try
            {
                this->replicate_in_database = buf;
            }
            catch (const std::exception& e)
            {
                std::cout << e.what() << std::endl;
                logger->add_error_message(e.what());
            }
        }

        bool get_replicate_in_database()
        {
            return this->replicate_in_database;
        }

        void create_consumer()
        {
            try
            {
                std::vector<std::future<void>> consumerFutures;
                for (auto v : listen)
                {
                    consumerFutures.push_back(
                        std::async(std::launch::async, [this, v]() {
                            auto consumer = std::make_unique<CONSUMER::Consumer<N, L>>(conn, logger, brokers, v, replicate_in_database);
                            consumers.push_back(std::move(consumer));
                            logger->add_success_message("Created consumer");
                        })
                    );
                }

                for (auto& future : consumerFutures)
                {
                    future.get();
                }

                for (auto& consumer : consumers)
                {
                    auto future = std::async(std::launch::async, &Manager::consumerThread<N, L>, this, std::move(consumer));

                    try
                    {
                        future.get();
                    }
                    catch (const std::exception& e)
                    {
                        logger->add_error_message(e.what());
                    }
                }
            }
            catch (const std::exception& e)
            {
                std::cout << e.what() << std::endl;
                logger->add_error_message(e.what());
            }
        }

		void create_db_connection(std::shared_ptr<N> dbconn)
		{
		    try
		    {
		        this->dbconn = dbconn;
		        this->conn = std::make_shared<CONNECTOR::BaseConnector<N>>(this->dbconn);

		        logger->add_success_message("Created connector");
		    }
		    catch (const std::exception& e)
		    {
		        std::cout << e.what() << std::endl;
		        logger->add_error_message(e.what());
		    }
		}

        void create_network_connection(int port, const std::string& ip)
        {
            try
            {
                boost::asio::io_context io_context;

                server = std::make_unique<NETWORK::Server<N, L>>(io_context, port, std::ref(this->conn), std::ref(this->logger), replicate_in_database);
                logger->add_info_message("server created");

                server->async_accept();

                logger->add_info_message("io context created");
                io_context.run();
            }
            catch (const std::exception& e)
            {
                std::cout << e.what() << std::endl;
                logger->add_error_message(e.what());
            }
        }
    };
}

#endif
