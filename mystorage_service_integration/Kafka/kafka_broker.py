import logging

from aiokafka import AIOKafkaProducer

from env import PRODUCER_CONFIG

class KafkaProducer:
    def __init__(self, logger: logging.Logger):
        self.config   = PRODUCER_CONFIG
        self.producer = None
        self.logger   = logger

    async def start_producer(self):
        self.producer = AIOKafkaProducer(bootstrap_servers=self.config['bootstrap_servers'])
        self.logger.info("Starting Kafka producer")
        await self.producer.start()

    async def stop_producer(self):
        await self.producer.stop()

    async def send_message(self, topic: str, message: str):
        if self.producer is None:
            await self.start_producer()

        try:
            encoded_message = message.encode('utf-8')
            await self.producer.send_and_wait(topic, value=encoded_message)
            self.logger.info(f"Message delivered to {topic}")
            await self.close()
        except Exception as e:
            self.logger.error(f"Message delivery failed: {e}")

    async def close(self):
        if self.producer is not None:
            self.logger.info("Closing Kafka producer")
            await self.stop_producer()