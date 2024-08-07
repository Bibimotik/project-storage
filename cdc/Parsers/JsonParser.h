#ifndef JSONPARSER_H
#define JSONPARSER_H

#include <iostream>
#include <algorithm>
#include <iterator>
#include <map>
#include <unordered_set>
#include <vector>

#include <nlohmann/json.hpp>

#include "../Sub/third.h"
#include "../Logger/Logger.h"

using json = nlohmann::json;

namespace JSONPARSER
{
	template <typename L>
	class JsonParser
	{
	private:
		std::shared_ptr<LOGGER::Logger<L>> logger;

	public:
		JsonParser(std::shared_ptr<LOGGER::Logger<L>> logger) : logger(logger) {};

		SUB::Third<std::vector<std::string>, std::vector<json>, bool> request_parse(json request)
		{
			try
			{
				std::vector<std::string> response;
				std::vector<json> data;
				bool success;

				if(!request["collection"])
				{
					logger->add_error_message("collection not specified");
					return SUB::Third<std::vector<std::string>, std::vector<json>, bool>::make_third(response, data, false);
				}
				else if(!request["operation"])
				{
					logger->add_error_message("operation not specified");
					return SUB::Third<std::vector<std::string>, std::vector<json>, bool>::make_third(response, data, false);
				}
				else if(!request["data"])
				{
					logger->add_error_message("data not specified");
					return SUB::Third<std::vector<std::string>, std::vector<json>, bool>::make_third(response, data, false);
				}

				response.push_back(request["collection"].template get<std::string>());
				response.push_back(request["operation"].template get<std::string>());

				data = json_parse(request["data"]);
			}
			catch (const std::exception& e)
			{
				logger->add_error_message("error in json request parse: ");
				logger->add_error_message(e.what());
				return SUB::Third<std::vector<std::string>, std::vector<json>, bool>::make_third(std::vector<std::string>(), std::vector<json>(), false);
			}
		}

	    std::vector<json> json_parse(json obj)
	    {
	    	try
	    	{
		        std::vector<json> elements;

		        if (obj.is_array())
		        {
		            elements = obj.get<std::vector<json>>();
		        }
		        else
		        {
		            elements.push_back(obj);
		        }

		        return elements;
	    	}
	    	catch (const std::exception& e)
	    	{
	    		logger->add_error_message("error in json request parse: ");
	    		logger->add_error_message(e.what());
	    		return {};
	    	}
	    }
	};
}

#endif
