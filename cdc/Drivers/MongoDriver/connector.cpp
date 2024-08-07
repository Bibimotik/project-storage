// #include "connector.h"

// namespace CONNECTION
// {
// 	MongoConn::MongoConn(const std::string url, const std::string dbname) : uri(url), client(uri)
// 	{
// 		db = this->client[dbname];
// 	}

// 	bool insert(const json& data)
// 	{
// 		try
// 		{

// 		}
// 		catch (const std::exception& e)
// 		{
// 	        std::cerr << "Exception: " << e.what() << std::endl;
// 	        return false;
// 		}
// 	}
// }