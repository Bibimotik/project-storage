cmake_minimum_required(VERSION 3.12)
project(cdc)

set(CMAKE_CXX_STANDARD 20)

set(mysql-concpp_DIR "/home/bararide/mysql-connector-c++-9.0.0-linux-glibc2.28-x86-64bit")

find_package(mysql-concpp)
find_package(Boost REQUIRED COMPONENTS filesystem program_options system)
find_package(quill REQUIRED)

find_library(SQLITE3_LIBRARY sqlite3)

set(SOURCES
    Broker/broker.cpp
    Drivers/SQLiteDriver/connector.cpp
    Drivers/PostgreSQLDriver/connector.cpp
    # Drivers/MySQLDriver/connector.cpp
    Drivers/ClickHouseDriver/connector.cpp
    # Drivers/MongoDriver/connector.cpp
    main.cpp
    parser.cpp
)

set(HEADERS
    Logger/Logger.h
    Broker/broker.h
    Network/network.h
    Drivers/baseConnector.h
    Drivers/SQLiteDriver/connector.h
    Drivers/PostgreSQLDriver/connector.h
    # Drivers/MongoDriver/connector.h
    # Drivers/MySQLDriver/connector.h
    Drivers/ClickHouseDriver/connector.h
    Manager/manager.h
    Parsers/JsonParser.h
    Sub/third.h
    consumer.h
    parser.h
)

add_subdirectory(contribs/clickhouse-cpp)

add_executable(${PROJECT_NAME} ${SOURCES} ${HEADERS})

target_compile_options(${PROJECT_NAME} PRIVATE 
    -I/usr/pgsql-11/include 
    -I/usr/include 
    #-I/usr/local/include/mongocxx/v_noabi 
    #-I/usr/local/include/bsoncxx/v_noabi
)

target_link_directories(${PROJECT_NAME} PRIVATE
    /home/bararide/cppkafka/build/src/lib64
    /usr/local/include/quill
    /usr/pgsql-11/lib
    /usr/local/lib
    /usr/lib
    ${mysql-concpp_DIR}
)

target_include_directories(${PROJECT_NAME} PRIVATE 
    contribs/clickhouse-cpp/ 
    contribs/clickhouse-cpp/contrib/absl
)

target_link_libraries(${PROJECT_NAME} PRIVATE
    pq
    cppkafka
    sqlparser
    ${Boost_LIBRARIES}
    Boost::system
    quill::quill
    ${SQLITE3_LIBRARY}
    clickhouse-cpp-lib
    #mongocxx
    #bsoncxx
)

include_directories(
    /home/bararide/.pgx/11.22/pgx-install/include
    /home/bararide/mysql-connector-c++-9.0.0-linux-glibc2.28-x86-64bit/include
)

link_directories(
    /usr/lib64
    /usr/lib
    ${mysql-concpp_DIR}
)