import asyncio

from cachetools import TTLCache
from typing import List

from StorageAPI import StorageAPI

class DataRetrieval:
    def __init__(self, storage_api: StorageAPI):
        self.storage_api          = storage_api
        self.owner_cache          = TTLCache(maxsize=100, ttl=360)
        self.storage_cache        = TTLCache(maxsize=100, ttl=360)
        self.images_cache         = TTLCache(maxsize=100, ttl=360)
        self.files_cache          = TTLCache(maxsize=100, ttl=360)
        self.download_files_cache = TTLCache(maxsize=100, ttl=360)

    async def get_all_notifications(self):
        try:
            self.storage_api.logger.info("Getting all notifications")
            async with self.storage_api.session.get(self.storage_api.url + '/notification', headers=self.storage_api.headers) as response:
                if response.status == 200:
                    content = await response.json()
                    for row in content['rows']:
                        notification_obj = {
                            'id':      row['id'],
                            'created': row['created'],
                            'title': row['title'],
                            'description': row['description'],
                            'productId': row['good']['id']
                        }

                        notification_obj.update(await self._get_optional_fields(row, ['actualBalance', 'minimumBalance']))
                        await self.storage_api.mongoHandler.add_notification(notification_obj)
        except Exception as e:
            self.storage_api.logger.error("Getting notifications from API: " + str(e))

    async def get_all_turnover(self):
        try:
            pass
        except Exception as e:
            self.storage_api.logger.error("Getting turnover from API: " + str(e))

    async def get_all_products(self):
        try:
            self.storage_api.logger.info("Getting all products")
            async with self.storage_api.session.get(self.storage_api.url + '/entity/product', headers=self.storage_api.headers) as response:
                if response.status == 200:
                    content = await response.json()
                    tasks = []
                    for row in content['rows']:
                        owner_task   = asyncio.create_task(self._get_owner_info(row['owner']['meta']['href']))
                        storage_task = asyncio.create_task(self._get_storage_info(row['group']['meta']['href']))
                        images_task  = asyncio.create_task(self._get_images_info(row['images']['meta']['href']))
                        files        = asyncio.create_task(self._get_files_info(row['files']['meta']['href']))

                        tasks.append((owner_task, storage_task, images_task, files))

                    results = await asyncio.gather(*[asyncio.gather(*task) for task in tasks])

                    for i, row in enumerate(content['rows']):
                        owner_id, storage_id, images, files = results[i]
                        product_obj = await self._create_product_obj(row, owner_id, storage_id, images, files)
                        await self.storage_api.mongoHandler.add_product(product_obj)
                else:
                    self.storage_api.logger.error("Product object not found in API: " + str(response.status))

        except Exception as e:
            self.storage_api.logger.error("Getting products from API: " + str(e))

    async def _create_product_obj(self, row, owner_id, storage_id, images, files):
        product_obj = {
            "name": row['name'],
            "code": row['code'],
            "archived": row['archived'],
            "group": storage_id,
            "owner": owner_id,
            "images": images,
            "files": files,
            "minPrice": row['minPrice']['value'],
            "buyPrice": row['buyPrice']['value'],
            "salePrices": await self._get_sale_prices(row['salePrices']),
            "quantity": await self._get_quantity_of_product(row['code']),
            "supplierId": ''
        }

        product_obj.update(await self._get_optional_fields(row, ['pathName', 'vat', 'country', 'supplier', 'weight', 'volume', 'variantsCount', 'description', 'article', 'minimumBalance']))

        if 'barcodes' in row:
            product_obj['barcodes'] = await self._get_barcodes(row.get('barcodes', []))

        if 'packs' in row:
            product_obj['packs'] = await self._get_product_packs(row.get('packs', []))

        return product_obj
    
    async def _get_product_packs(self, packs):
        try:
            result_packs = []
            for pack_type in packs:
                pack = {
                    'id':       pack_type['id'],
                    'quantity': pack_type['quantity'],
                    'uom':      await self._get_pack_uom(pack_type['uom']['meta']['href']),
                    'barcodes': await self._get_barcodes(pack_type.get('barcodes', []))
                }
                result_packs.append(pack)

            return result_packs
        except Exception as e:
            self.storage_api.logger.error("Getting product packs: " + str(e))

    async def _get_optional_fields(self, row, fields):
        optional_fields = {}
        for field in fields:
            if field in row:
                optional_fields[field] = row[field]
        return optional_fields

    async def _get_barcodes(self, barcodes):
        barcode_types = ['code128', 'ean13', 'ean8', 'gtin', 'upc']
        result_barcodes = []
        for barcode in barcodes:
            for barcode_type in barcode_types:
                if barcode_type in barcode:
                    result_barcodes.append({barcode_type: barcode[barcode_type]})
                    break
        return result_barcodes

    async def _get_sale_prices(self, salePrices) -> List:
        try:
            self.storage_api.logger.info("Getting product sale prices")
            result = []
            for salePrice in salePrices:
                salePrice_obj = {
                    'value': salePrice['value'],
                    'id':    salePrice['priceType']['id'],
                    'name':  salePrice['priceType']['name']
                }
                result.append(salePrice_obj)

            return result
        except Exception as e:
            self.storage_api.logger.error("Getting product sale prices: " + str(e))

    async def _get_product_info(self, url: str):
        pass

    async def _get_product_supplier(self, url: str):
        try:
            self.storage_api.logger.info("Getting product supplier from API: " + str(url))
            async with self.storage_api.session.get(url, headers=self.storage_api.headers) as response:
                if response.status == 200:
                    content = await response.json()
                    return content['id']
        except Exception as e:
            self.storage_api.logger.error("Getting product supplier from API: " + str(e))

    async def _get_product_country(self, url: str):
        try:
            self.storage_api.logger.info("Getting product country from API: " + str(url))
            async with self.storage_api.session.get(url, headers=self.storage_api.headers) as response:
                if response.status == 200:
                    content = await response.json()
                    return content['name']
        except Exception as e:
            self.storage_api.logger.error("Getting product country from API: " + str(e))

    async def _get_pack_uom(self, url: str) -> dict:
        try:
            self.storage_api.logger.info("Getting product pack uom from API: " + str(url))
            async with self.storage_api.session.get(url, headers=self.storage_api.headers) as response:
                if response.status == 200:
                    content = await response.json()
                    result = {
                        'id': content['id'],
                        'name': content['name'],
                        'description': content['description']
                    }
                    return result
        except Exception as e:
            self.storage_api.logger.error("Getting uom of packs: " + str(e))

    async def _get_files_info(self, url: str) -> List:
        try:
            if url in self.files_cache:
                return self.files_cache[url]
            
            self.storage_api.logger.info("Getting files info from API: " + str(url))
            async with self.storage_api.session.get(url, headers=self.storage_api.headers) as response:
                if response.status == 200:
                    content = await response.json()
                    files = []

                    for row in content['rows']:
                        if url in self.owner_cache:
                            created_by_id = self.owner_cache[url]
                        else:
                            created_by_id = await self._get_owner_id(row['createdBy']['meta']['href'])

                        if row['meta']['href'] in self.download_files_cache:
                            download_href = self.download_files_cache[row['meta']['href']]
                        else:
                            download_href = await self._get_dowload_href_for_file(row['meta']['href'])

                        file_obj = {
                            'title': row['title'],
                            'filename': row['filename'],
                            'created': row['created'],
                            'createdById': created_by_id,
                            'downloadHref': download_href
                        }
                        
                        files.append(file_obj)

                    self.files_cache[url] = files
                    return files
        except Exception as e:
            self.storage_api.logger.error("Getting product files from API: " + str(e))

    async def _get_dowload_href_for_file(self, url: str) -> str:
        try:
            self.storage_api.logger.info("Getting download href for file from API: " + str(url))
            async with self.storage_api.session.get(url, headers=self.storage_api.headers) as response:
                if response.status == 200:
                    content = await response.json()
                    self.download_files_cache[url] = content['meta']['downloadHref']
                    return content['meta']['downloadHref']
        except Exception as e:
            self.storage_api.logger.error("Getting download href for file from API: " + str(e))

    async def _get_owner_id(self, url: str) -> str:
        try:
            self.storage_api.logger.info("Get owner id for file from API: " + str(url))
            async with self.storage_api.session.get(url, headers=self.storage_api.headers) as response:
                if response.status == 200:
                    content = await response.json()
                    self.owner_cache[url] = content['id']
                    return content['id']
        except Exception as e:
            self.storage_api.logger.error("Getting owner id for file from API: " + str(e))

    async def _get_quantity_of_product(self, code) -> List:
        try:
            self.storage_api.logger.info("Get quantity of product with code: " + str(code))
            async with self.storage_api.session.get(self.storage_api.url + '/report/stock/bystore', headers=self.storage_api.headers) as response:
                if response.status == 200:
                    content = await response.json()
                    stories = []
                    for row in content['rows']:
                        if await self._check_product_code(row['meta']['href'], code):
                            for store in row['stockByStore']:
                                store_obj = {
                                    "storageId": await self._get_storage_id(store['meta']['href']),
                                    "quantity":  store['stock'],
                                    "reserve":   store['reserve'],
                                    "inTransit": store['inTransit']
                                }
                                stories.append(store_obj)
                            return stories
                return []

        except Exception as e:
            self.storage_api.logger.error("Getting product quantity from API: " + str(e))

    async def _get_images_info(self, url: str) -> str:
        try:
            if url in self.images_cache:
                return self.images_cache[url]

            self.storage_api.logger.info("Getting images from API: " + str(url))
            async with self.storage_api.session.get(url, headers=self.storage_api.headers) as response:
                if response.status == 200:
                    images = []
                    content = await response.json()
                    for row in content['rows']:
                        image = {
                            "id":                await self.storage_api.mongoHandler.add_image(row['meta']['downloadHref'], self.storage_api.session),
                            "title":             row['title'],
                            "filename":          row['filename'],
                            "updated":           row['updated'],
                            "downloadHref":      row['meta']['downloadHref'],
                            "spareDownloadHref": row['miniature']['href']
                        }
                        images.append(image)

                    self.images_cache[url] = images
                    return images
        except Exception as e:
            self.storage_api.logger.error("Getting images info from API: " + str(e))

    async def _get_owner_info(self, url: str) -> str:
        try:
            if url in self.owner_cache:
                return self.owner_cache[url]

            self.storage_api.logger.info("Getting owner info from API: " + str(url))
            async with self.storage_api.session.get(url, headers=self.storage_api.headers) as response:
                if response.status == 200:
                    content = await response.json()
                    owner = {
                        "id":        content['id'],
                        "accountId": content['accountId'],
                        "uid":       content['uid'],
                        "name":      content['name'],
                        "email":     content['email'],
                        "phone":     content['phone'],
                        "lastName":  content['lastName']
                    }

                    if not await self.storage_api.mongoHandler.check_owner(content['id']):
                        await self.storage_api.mongoHandler.add_owner(owner)

                    self.owner_cache[url] = content['id']
                    return content['id']
        except Exception as e:
            self.storage_api.logger.error("Getting owner info from API: " + str(e))

    async def _get_storage_id(self, url) -> str:
        try:
            self.storage_api.logger.info("Getting storage id from API: " + str(url))
            async with self.storage_api.session.get(url, headers=self.storage_api.headers) as response:
                if response.status == 200:
                    content = await response.json()

                    return content['id']
        except Exception as e:
            self.storage_api.logger.error("Getting storage id from API: " + str(e))

    async def _get_storage_info(self, url) -> str:
        try:
            if url in self.storage_cache:
                return self.storage_cache[url]

            self.storage_api.logger.info("Getting storage info from API: " + str(url))
            async with self.storage_api.session.get(url, headers=self.storage_api.headers) as response:
                if response.status == 200:
                    content = await response.json()
                    storage = {
                        "id":        content['id'],
                        "accountId": content['accountId'],
                        "name":      content['name'],
                        "index":     content['index']
                    }

                    if not await self.storage_api.mongoHandler.check_storage(content['id']):
                        await self.storage_api.mongoHandler.add_storage(storage)

                    self.storage_cache[url] = content['id']
                    return content['id']
        except Exception as e:
            self.storage_api.logger.error("Getting storage info from API: " + str(e))

    async def _check_product_code(self, url: str, code) -> bool:
        try:
            self.storage_api.logger.info("Check product code from API: " + str(url))
            async with self.storage_api.session.get(url, headers=self.storage_api.headers) as response:
                if response.status == 200:
                    content = await response.json()
                    if content['code'] == code:
                        return True

            return False

        except Exception as e:
            self.storage_api.logger.error("Getting storage info from API: " + str(e))