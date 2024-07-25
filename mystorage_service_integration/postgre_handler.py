import uuid
import asyncpg
import asyncio
import logging

class PostgreHandler():
    def __init__(self, conn, logger: logging.Logger):
        self.conn = conn
        self.logger = logger

    async def get_auth_data(self, id: str):
        try:
            row = await self.conn.fetchrow(
                'SELECT mystorage_username, mystorage_login FROM "user" WHERE id = $1', id
            )
            return row
        except Exception as e:
            self.logger.error('get_auth_data: ' + str(e))