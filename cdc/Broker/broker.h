#ifndef BROKER_H
#define BROKER_H

#include "../parser.h"

#include <iostream>

#include <cppkafka/cppkafka.h>
#include <nlohmann/json.hpp>

#include <memory>
#include <atomic>
#include <mutex>
#include <thread>

#include <future>

using json = nlohmann::json;

namespace BROKER
{
	class Broker
	{
	private:
		cppkafka::Configuration config;
		cppkafka::Producer producer;

		std::string topic;
		std::mutex broker_mutex;

	public:
		explicit Broker(std::string topic)
			: config{
				{"metadata.broker.list", "localhost:9092"}
			},
			producer(config),
			topic(topic)
			{}

		void post_message(const std::string& message);
		void post_message(const json& message);
	};
}

#endif
