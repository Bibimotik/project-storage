#ifndef NETWORK_H
#define NETWORK_H

#include "../parser.h"
#include "../Sub/third.h"
#include "../Drivers/baseConnector.h"
#include "../Logger/Logger.h"

#include <boost/asio.hpp>
#include <boost/program_options.hpp>

#include <iostream>
#include <optional>

#include <nlohmann/json.hpp>

namespace opt = boost::program_options;

using json = nlohmann::json;

namespace NETWORK
{
	template<typename T, typename L>
	class Session: public std::enable_shared_from_this<Session<T, L>>
	{
	public:
		Session(boost::asio::ip::tcp::socket&& socket, std::shared_ptr<CONNECTOR::BaseConnector<T>> conn, std::shared_ptr<LOGGER::Logger<L>> logger, bool replicate_in_database) :
			socket(std::move(socket)),
			parser(std::make_unique<PARSER::Parser>()),
			conn(conn),
			logger(logger),
			replicate_in_database(replicate_in_database)
			{}

		void start()
		{
		    auto self = this->shared_from_this();
		    boost::asio::async_read_until(
		        socket,
		        streambuf,
		        '\n',
		        [self, this] (
		            boost::system::error_code error,
		            std::size_t bytes_transferred) {
		                std::string query;
		                std::istream is(&self->streambuf);
		                std::getline(is, query);

		                SUB::Third<json, bool, int> result = parser->Request_parse(query);

		                if (result.second)
		                {
		                    self->delegate_message(result.third, query, result.first);
		                }
		        });
		}

		void delegate_message(int oper, const std::string& msg, const json& json_response)
		{
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

		                        send(response);
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
		        auto broker_future = std::async(std::launch::async, [this](const std::string& msg){
		            send(msg);
		        }, msg);
		        broker_future.get();
		    }
		}

		void send(const std::string& message)
		{
			boost::asio::const_buffer buffer(message.data(), message.size());

			boost::asio::async_write(socket, buffer,
				[this](const boost::system::error_code& error, std::size_t bytes_transferred)
				{
					if (error)
					{
						std::cerr << "Error sending message: " << error.message() << std::endl;
					}
				});
		}

		void send(const json& message)
		{
			std::string msg = message.dump();

			send(msg);
		}

	private:
		boost::asio::ip::tcp::socket socket;
		boost::asio::streambuf streambuf;

		std::unique_ptr<PARSER::Parser> parser;
		std::shared_ptr<CONNECTOR::BaseConnector<T>> conn;
		std::shared_ptr<LOGGER::Logger<L>> logger;

		bool replicate_in_database;

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
	};

	template<typename T, typename L>
	class Server
	{
	public:
		Server(boost::asio::io_context& io_context,
			   std::uint16_t port,
			   std::shared_ptr<CONNECTOR::BaseConnector<T>>& conn,
			   std::shared_ptr<LOGGER::Logger<L>>& logger,
			   bool replicate_in_database) :
			io_context(io_context),
			conn(conn.get()),
			logger(logger),
			replicate_in_database(replicate_in_database),
			acceptor(
					io_context,
					boost::asio::ip::tcp::endpoint(boost::asio::ip::tcp::v4(), port)
				) {}

		void async_accept()
		{
			socket.emplace(io_context);

			acceptor.async_accept(*socket, [&](boost::system::error_code error) {
				std::make_shared<Session<T, L>>(std::move(*socket), conn, logger, replicate_in_database)->start();
				async_accept();
			});
		}

	private:
		boost::asio::io_context& io_context;
		boost::asio::ip::tcp::acceptor acceptor;
		std::optional<boost::asio::ip::tcp::socket> socket;

		bool replicate_in_database;

		std::shared_ptr<CONNECTOR::BaseConnector<T>> conn;
		std::shared_ptr<LOGGER::Logger<L>> logger;
	};
}

#endif
