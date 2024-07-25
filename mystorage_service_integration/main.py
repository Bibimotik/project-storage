import asyncio
import asyncpg
import logging
import aiohttp
import time

from StorageAPI      import StorageAPI, DataRetrieval
from mongo_handler   import MongoHandler
from postgre_handler import PostgreHandler
from Kafka           import KafkaProducer, KafkaConsumer

from env import POSTGRECONN

async def task1(auth, mongoHandler, logger):
    async with aiohttp.ClientSession() as session:
        storageAPI = StorageAPI(username=auth[0], password=auth[1], mongoHandler=mongoHandler, logger=logger, session=session)
        dataRetrieval = DataRetrieval(storageAPI)

        await dataRetrieval.get_all_products()

async def task2(auth, mongoHandler, logger):
    async with aiohttp.ClientSession() as session:
        storageAPI = StorageAPI(username=auth[0], password=auth[1], mongoHandler=mongoHandler, logger=logger, session=session)
        dataRetrieval = DataRetrieval(storageAPI)

        await dataRetrieval.get_all_notifications()

async def main():
    try:
        logger = logging.getLogger(__name__)
        logging.basicConfig(level=logging.INFO, filename="logger.log", filemode="w", format="%(asctime)s %(levelname)s %(message)s")
        logger.info('Started')

        conn = await asyncpg.connect(POSTGRECONN)

        mongoHandler = MongoHandler(logger=logger)
        postgreHandler = PostgreHandler(conn=conn, logger=logger)


        await mongoHandler.clear_product_collection()
        await mongoHandler.clear_notification_collection()

        kafkaProducer = KafkaProducer(logger=logger)
        kafkaConsumer = KafkaConsumer(logger=logger)

        await kafkaProducer.send_message('test', 'test message')

        auth = await postgreHandler.get_auth_data('1f0bb267-2750-4f50-a2fd-3a8874675b9e') #uuid пользователя в postgreSQL

        start_time = time.time()

        await asyncio.gather(
            task1(auth, mongoHandler, logger),
            task2(auth, mongoHandler, logger)
        )

        end_time = time.time()
        execution_time = end_time - start_time

        print(execution_time)

        logger.info('Finished')

    except Exception as e:
        print("Error in main: " + str(e))

if __name__ == "__main__":
    loop = asyncio.get_event_loop()
    loop.run_until_complete(main())
