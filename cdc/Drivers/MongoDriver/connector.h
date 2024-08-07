#ifndef MONGOCOMMECTION_H
#define MONGOCOMMECTION_H

#include <cstdint>
#include <iostream>
#include <vector>
#include <mutex>

// #include <bsoncxx/builder/basic/document.hpp>
// #include <bsoncxx/json.hpp>
// #include <mongocxx/client.hpp>
// #include <mongocxx/instance.hpp>
// #include <mongocxx/stdx.hpp>
// #include <mongocxx/uri.hpp>

// #include <nlohmann/json.hpp>

// using json = nlohmann::json;

// using bsoncxx::builder::basic::kvp;
// using bsoncxx::builder::basic::make_array;
// using bsoncxx::builder::basic::make_document;

namespace CONNECTION
{
    class MongoConn
    {
    // private:
    //     mongocxx::instance instance;
    //     mongocxx::uri uri;
    //     mongocxx::client client;

    //     mongocxx::v_noabi::database db;

    // public:
    //     MongoConn(const std::string url, const std::string dbname);

    //     bool insert(const std::string& data);
    //     bool update(const std::string& update_query);
    //     bool remove(const std::string& remove_query);
    //     bool check_query(const std::string& query);

    //     void post_logger_message(const std::string& request, bool status);

    //     std::vector<std::string> select(const std::string& select_query);
    };
}

#endif
