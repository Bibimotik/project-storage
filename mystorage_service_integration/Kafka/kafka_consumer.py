import logging

from aiokafka import AIOKafkaConsumer

from env import CONSUMER_CONFIG

class KafkaConsumer:
    def __init__(self, logger: logging.Logger):
        self.config = CONSUMER_CONFIG
        self.consumer = None
        self.logger = logger

    async def start_consumer(self):
        self.consumer = AIOKafkaConsumer(
            bootstrap_servers=self.config['bootstrap_servers'],
            group_id=self.config['group_id']
        )
        await self.consumer.start()

    async def stop_consumer(self):
        await self.consumer.stop()

    async def consume_messages(self, topic):
        if self.consumer is None:
            await self.start_consumer()

        try:
            await self.consumer.subscribe([topic])

            async for message in self.consumer:
                self.logger.info(f"Received message from topic '{topic}': {message.value.decode('utf-8')}")
        except Exception as e:
            self.logger.error(f"Error consuming messages: {e}")

    async def close(self):
        if self.consumer is not None:
            await self.stop_consumer()