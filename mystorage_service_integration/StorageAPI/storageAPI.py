import aiohttp
import base64
import logging

from env import APIURL

from mongo_handler import MongoHandler

class StorageAPI:
    def __init__(self, username: str, password: str, mongoHandler: MongoHandler, logger: logging.Logger, session: aiohttp.ClientSession):
        self.username     = username
        self.password     = password
        self.url          = APIURL
        self.logger       = logger
        self.session      = session
        self.mongoHandler = mongoHandler
    
        self._get_auth_headers()

    def _get_auth_headers(self) -> None:
        try:
            credentials = base64.b64encode(f"{self.username}:{self.password}".encode("utf-8")).decode("utf-8")
            self.headers = {
                "Authorization": f"Basic {credentials}",
                "Accept-Encoding": "gzip",
                "Content-Type": "application/json"
            }
        except Exception as e:
            self.logger.error("Error auorization in api: " + str(e))