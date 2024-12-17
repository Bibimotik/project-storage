import os
import sys
from docxtpl import DocxTemplate
from datetime import datetime

docx_path = sys.argv[1]
context = {
    'date': sys.argv[2],
    'fromname': sys.argv[3],
    'gendir': sys.argv[4],
    'toname': sys.argv[5],
    'fromaddress': sys.argv[6],
    'frominn': sys.argv[7],
    'fromogrn': sys.argv[8],
    'fromrasch': sys.argv[9],
    'fromkor': sys.argv[10],
    'frombik': sys.argv[11],
    'frombank': sys.argv[12],
    'toaddress': sys.argv[13],
    'toinn': sys.argv[14],
    'toogrn': sys.argv[15],
    'torasch': sys.argv[16],
    'tokor': sys.argv[17],
    'tobik': sys.argv[18],
    'tobank': sys.argv[19],
    'kppfrom': sys.argv[20],
    'tokpp': sys.argv[21],
    'tostorageaddress': sys.argv[22],
}

doc = DocxTemplate(docx_path)

doc.render(context)

downloads_path = os.path.join(os.path.expanduser('~'), 'Downloads')

timestamp = datetime.now().strftime("%Y-%m-%d_%H-%M-%S")
file_name = f"generated_doc_{timestamp}.docx"

doc.save(os.path.join(downloads_path, file_name))
