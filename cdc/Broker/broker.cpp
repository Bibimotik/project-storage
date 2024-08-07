#include "broker.h"

namespace BROKER 
{

    void Broker::post_message(const std::string& message) 
    {
        std::cout << message << std::endl;
        this->producer.produce(cppkafka::MessageBuilder(topic).payload(message));
        this->producer.flush();
    }

    void Broker::post_message(const json& message) 
    {
        try
        {
            std::string mes = message.dump();
            post_message(mes);
        }
        catch (const std::exception& e)
        {
            std::cout << e.what() << std::endl;
        }
    }
}