import logging

import motor.motor_asyncio as motor


class MongoHandler:
    def __init__(self, logger: logging.getLogger):
        self.client        = motor.AsyncIOMotorClient('mongodb://localhost:27017')
        self.db            = self.client['mystorage']
        self.products      = self.db['products']
        self.owners        = self.db['owners']
        self.storages      = self.db['storages']
        self.notifications = self.db['notifications']
        
        self.fs = motor.AsyncIOMotorGridFSBucket(self.db)
        self.logger = logger

    async def insert_document(self, collection, document):
        try:
            await collection.insert_one(document)
            self.logger.info(f"Added {collection.name[:-1]}")
        except Exception as e:
            self.logger.error(f"Error in MongoHandler::insert_document: {str(e)}")

    async def add_product(self, product: dict):
        await self.insert_document(self.products, product)

    async def add_storage(self, storage: dict):
        await self.insert_document(self.storages, storage)

    async def add_notification(self, notification: dict):
        await self.insert_document(self.notifications, notification)

    async def add_image(self, image_url: str, session):
        try:
            async with session.get(image_url) as response:
                if response.status == 200:
                    image_data = await response.read()
                    file_id = await self.fs.upload_from_stream(filename=image_url, source=image_data)
                    self.logger.info("Added image with file_id: " + str(file_id))
                    return file_id
                else:
                    self.logger.error("Failed to download image from URL: " + image_url)
        except Exception as e:
            self.logger.error("Error in MongoHandler::add_image: " + str(e))

    async def add_owner(self, owner: dict):
        await self.insert_document(self.owners, owner)

    async def check_document(self, collection, field, value):
        try:
            document = await collection.find_one({field: value})
            return document is not None
        except Exception as e:
            self.logger.error(f"Error in MongoHandler::check_document: {str(e)}")

    async def check_owner(self, id: str) -> bool:
        return await self.check_document(self.owners, "id", id)

    async def check_storage(self, id: str) -> bool:
        return await self.check_document(self.storages, "id", id)

    async def clear_collection(self, collection):
        try:
            result = await collection.delete_many({})
            self.logger.info(f"clear {collection.name[:-1]} collection: {str(result)}")
        except Exception as e:
            self.logger.error(f"Error in MongoHandler::clear_collection: {str(e)}")

    async def clear_storage_collection(self):
        await self.clear_collection(self.storages)

    async def clear_product_collection(self):
        await self.clear_collection(self.products)

    async def clear_notification_collection(self):
        await self.clear_collection(self.notifications)