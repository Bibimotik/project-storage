#include "Manager/manager.h"

using json = nlohmann::json;

int main(int argc, char** argv)
{
    try
    {
        opt::options_description desc("All options");

        desc.add_options()
        	("network,n",     "Connect to the network")
        	("port,p",        opt::value<int>(), "Port number")
        	("ip",            opt::value<std::string>(), "IP address")
        	("local,l",       "Working on the local machine")
            ("listen",        opt::value<std::vector<std::string>>(),                  "enter the topics you want to listen to")
            ("notif",         opt::value<std::vector<std::string>>(),                  "enter the topics that need to be notified")
            ("logtopic",      opt::value<std::vector<std::string>>(),                  "enter the topics which you need to log")
            ("dbtype",        opt::value<std::string>()->default_value("postgresql"),  "postgresql, clickhouse, mysql, sqlite, mongodb")
            ("dbname",        opt::value<std::string>()->default_value("test"),        "Name of you main database")
            ("logger_dbtype", opt::value<std::string>()->default_value("postgresql"),  "postgresql, clickhouse, mysql, sqlite, mongodb")
            ("logger_dbname", opt::value<std::string>()->default_value("logger"),      "Name of your logger database")
            ("user",          opt::value<std::string>()->default_value("bararide"),    "Name of database user")
            ("password",      opt::value<std::string>()->default_value("0642q"),       "Database password")
            ("hostaddr",      opt::value<std::string>()->default_value("127.0.0.1"),   "Host address of database")
            ("dbport",        opt::value<std::string>()->default_value("5432"),        "Port of database")
            ("include",       opt::value<std::string>()->default_value("yes"),         "Copy command in database")
            ("logpath",       opt::value<std::string>()->default_value("logging.log"), "Path to log file")
            ("help",          "print all commands");

        opt::variables_map vm;

        opt::store(opt::parse_command_line(argc, argv, desc), vm);
        opt::notify(vm);

        if(vm.count("help"))
        {
            std::cout << desc << std::endl;
        }
        else
        {
            if(vm.count("dbtype") && vm.count("logger_dbtype"))
            {
            	if(vm["logger_dbtype"].as<std::string>() == "postgresql" && vm["dbtype"].as<std::string>() == "postgresql")
            	{
                    std::unique_ptr<MANAGER::Manager<CONNECTION::PGConnection, CONNECTION::PGConnection>> manager = 
                        std::make_unique<MANAGER::Manager<CONNECTION::PGConnection, CONNECTION::PGConnection>>();

                    if(vm.count("listen") && vm.count("notif") && vm.count("logtopic") && !vm.count("network"))
                    {
                        manager->add_brokers(
                                vm["listen"].as<std::vector<std::string>>(), 
                                vm["notif"].as<std::vector<std::string>>(), 
                                vm["logtopic"].as<std::vector<std::string>>()
                            );
                    }
                    else if((vm.count("listen") && vm.count("notif") && vm.count("logtopic")) && !vm.count("network"))
                    {
                        std::cout << "No topics specified" << std::endl;
                        return 1;
                    }

                    if(vm["include"].as<std::string>() == "yes")
                    {
                        manager->set_replicate_in_database(true);
                    }
                    else
                    {
                        manager->set_replicate_in_database(false);
                    }

                    std::string config = "dbname=" + vm["logger_dbname"].as<std::string>() +
                                            " user=" + vm["user"].as<std::string>() +
                                            " password=" + vm["password"].as<std::string>() + 
                                            " hostaddr=" + vm["hostaddr"].as<std::string>() + 
                                            " port=" + vm["dbport"].as<std::string>();

                    std::shared_ptr<CONNECTOR::BaseConnector<CONNECTION::PGConnection>> logdb = std::make_shared<CONNECTOR::BaseConnector<CONNECTION::PGConnection>>(std::make_shared<CONNECTION::PGConnection>(config));

                    if(vm.count("network"))
                    {
                        manager->create_logger(config, vm["logger_dbname"].as<std::string>(), logdb, true);
                    }
                    else
                    {
                        manager->create_logger(config, vm["logger_dbname"].as<std::string>(), logdb, false);
                    }
            	
                    if(vm.count("dbname") && vm.count("user") && vm.count("password") && vm.count("hostaddr") && vm.count("dbport"))
                    {
                        config = "dbname=" + vm["dbname"].as<std::string>() +
                                    " user=" + vm["user"].as<std::string>() +
                                    " password=" + vm["password"].as<std::string>() + 
                                    " hostaddr=" + vm["hostaddr"].as<std::string>() + 
                                    " port=" + vm["dbport"].as<std::string>();

                        manager->create_db_connection(std::make_shared<CONNECTION::PGConnection>(config));
              		}

              		if(vm.count("network"))
		        	{
		        		if(vm.count("port") && vm.count("ip"))
		        		{
		        			manager->create_network_connection(
		        				vm["port"].as<int>(),
		        				vm["ip"].as<std::string>()
		        			);
		        		}
		        		else
		        		{
		        			manager->logger->add_error_message("Both port and IP address are required for network connection.");
			                std::cout << "Error: Both port and IP address are required for network connection." << std::endl;
			                return 1;
		        		}
		        	}
		        	else
		        	{
		        		manager->create_consumer();
		        	}
                }
                else if(vm["logger_dbtype"].as<std::string>() == "postgresql" && vm["dbtype"].as<std::string>() == "sqlite")
                {
        			std::unique_ptr<MANAGER::Manager<CONNECTION::PGConnection, SQLITECONN::Sqliteconn>> manager = 
                        std::make_unique<MANAGER::Manager<CONNECTION::PGConnection, SQLITECONN::Sqliteconn>>();

                    if(vm.count("listen") && vm.count("notif") && vm.count("logtopic") && !vm.count("network"))
                    {
                        manager->add_brokers(
                                vm["listen"].as<std::vector<std::string>>(), 
                                vm["notif"].as<std::vector<std::string>>(), 
                                vm["logtopic"].as<std::vector<std::string>>()
                            );
                    }
                    else if((vm.count("listen") && vm.count("notif") && vm.count("logtopic")) && !vm.count("network"))
                    {
                        std::cout << "No topics specified" << std::endl;
                        return 1;
                    }

                    if(vm["include"].as<std::string>() == "yes")
                    {
                        manager->set_replicate_in_database(true);
                    }
                    else
                    {
                        manager->set_replicate_in_database(false);
                    }

                    std::string config = "dbname=" + vm["logger_dbname"].as<std::string>() +
                                            " user=" + vm["user"].as<std::string>() +
                                            " password=" + vm["password"].as<std::string>() + 
                                            " hostaddr=" + vm["hostaddr"].as<std::string>() + 
                                            " port=" + vm["dbport"].as<std::string>();

                    std::shared_ptr<CONNECTOR::BaseConnector<CONNECTION::PGConnection>> logdb = std::make_shared<CONNECTOR::BaseConnector<CONNECTION::PGConnection>>(std::make_shared<CONNECTION::PGConnection>(config));

                    if(vm.count("network"))
                    {
                        manager->create_logger(config, vm["logger_dbname"].as<std::string>(), logdb, true);
                    }
                    else
                    {
                        manager->create_logger(config, vm["logger_dbname"].as<std::string>(), logdb, false);
                    }

                    manager->create_db_connection(std::make_shared<SQLITECONN::Sqliteconn>(vm["dbname"].as<std::string>()));
                    
              		if(vm.count("network"))
		        	{
		        		if(vm.count("port") && vm.count("ip"))
		        		{
		        			manager->create_network_connection(
		        				vm["port"].as<int>(),
		        				vm["ip"].as<std::string>()
		        			);
		        		}
		        		else
		        		{
		        			manager->logger->add_error_message("Both port and IP address are required for network connection.");
			                std::cout << "Error: Both port and IP address are required for network connection." << std::endl;
			                return 1;
		        		}
		        	}
		        	else
		        	{
		        		manager->create_consumer();
		        	}
                }
	            else if(vm["logger_dbtype"].as<std::string>() == "postgresql" && vm["dbtype"].as<std::string>() == "clickhouse")
	            {
	    			std::unique_ptr<MANAGER::Manager<CONNECTION::PGConnection, CLICKHOUSECONN::ClickHouseConn>> manager = 
                        std::make_unique<MANAGER::Manager<CONNECTION::PGConnection, CLICKHOUSECONN::ClickHouseConn>>();

                    if(vm.count("listen") && vm.count("notif") && vm.count("logtopic"))
                    {
                        manager->add_brokers(
                                vm["listen"].as<std::vector<std::string>>(), 
                                vm["notif"].as<std::vector<std::string>>(), 
                                vm["logtopic"].as<std::vector<std::string>>()
                            );
                    }
                    else if((vm.count("listen") && vm.count("notif") && vm.count("logtopic")) && !vm.count("network"))
                    {
                        std::cout << "No topics specified" << std::endl;
                        return 1;
                    }

                    if(vm["include"].as<std::string>() == "yes")
                    {
                        manager->set_replicate_in_database(true);
                    }
                    else
                    {
                        manager->set_replicate_in_database(false);
                    }

	                std::string config = "dbname=" + vm["logger_dbname"].as<std::string>() +
	                                        " user=" + vm["user"].as<std::string>() +
	                                        " password=" + vm["password"].as<std::string>() + 
	                                        " hostaddr=" + vm["hostaddr"].as<std::string>() + 
	                                        " port=" + vm["dbport"].as<std::string>();

	                std::shared_ptr<CONNECTOR::BaseConnector<CONNECTION::PGConnection>> logdb = std::make_shared<CONNECTOR::BaseConnector<CONNECTION::PGConnection>>(std::make_shared<CONNECTION::PGConnection>(config));
                    
                    if(vm.count("network"))
                    {
                        manager->create_logger(config, vm["logger_dbname"].as<std::string>(), logdb, true);
                    }
                    else
                    {
                        manager->create_logger(config, vm["logger_dbname"].as<std::string>(), logdb, false);
                    }
	        	
	                if(vm.count("dbname") && vm.count("hostaddr"))
	                {
	                    manager->create_db_connection(std::make_shared<CLICKHOUSECONN::ClickHouseConn>(vm["hostaddr"].as<std::string>(), vm["dbname"].as<std::string>()));
	                }

              		if(vm.count("network"))
		        	{
		        		if(vm.count("port") && vm.count("ip"))
		        		{
		        			manager->create_network_connection(
		        				vm["port"].as<int>(),
		        				vm["ip"].as<std::string>()
		        			);
		        		}
		        		else
		        		{
		        			manager->logger->add_error_message("Both port and IP address are required for network connection.");
			                std::cout << "Error: Both port and IP address are required for network connection." << std::endl;
			                return 1;
		        		}
		        	}
		        	else
		        	{
		        		manager->create_consumer();
		        	}
	            }
                // else if(vm["dbtype"].as<std::string>() == "mysql")
                // {

                // }
            }
        }
    } 
    catch (const std::exception& e) 
    {
        std::cerr << "Exception: " << e.what() << std::endl;
        return 1;
    }

    return 0;
}
