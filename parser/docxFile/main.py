import os
from docxtpl import DocxTemplate
from datetime import datetime

doc = DocxTemplate("D:/Учеба/Договор.docx")

context = {
    'date': '16 декабря 2024 года',
    'fromname': 'ООО "Поставщик"',
    'gendir': 'Иванов Иван Иванович',
    'toname': 'ООО "Покупатель"',
    'fromaddress': 'г. Москва, ул. Ленина, д. 10',
    'frominn': '1234567890',
    'fromogrn': '1027700000000',
    'fromrasch': '40702810123456789012',
    'fromkor': '30101810400000000001',
    'frombik': '044525225',
    'frombank': 'Сбербанк России',
    'toaddress': 'г. Ногинск, ул. Бетонная, д. 1',
    'toinn': '9876543210',
    'toogrn': '112774688',
    'torasch': '40702810234567890123',
    'tokor': '30101810500000000001',
    'tobik': '044525225',
    'tobank': 'ВТБ',
    'kppfrom': '772301001',
    'tokpp': '502301001',
    'tostorageaddress': 'М.О., г. Ногинск, ул. Бетонная, д. 1'
}

doc.render(context)

downloads_path = os.path.join(os.path.expanduser('~'), 'Downloads')

timestamp = datetime.now().strftime("%Y-%m-%d_%H-%M-%S")

# Создание уникального имени для файла
file_name = f"generated_doc_{timestamp}.docx"

# Сохранение файла
doc.save(os.path.join(downloads_path, file_name))