import psycopg2
import requests

db_params = {
    "dbname": "parserDataBase",
    "user": "postgres",  # ПОМЕНЯТЬ ПОТОМ
    "password": "222120111m",
    "host": "localhost",  # ИЛИ АДРЕС СЕРВЕРА
    "port": "5432"
}

search_url = "https://egrul.nalog.ru/"
search_data = {
    "query": "7723431957"  # ИНН ДЛЯ ВСТАВКИ
}

def insert_data_to_db(data):
    try:
        conn = psycopg2.connect(**db_params)
        cursor = conn.cursor()

        insert_query = """
        INSERT INTO PARSER (INN, KPP, OGRN, ShortName, FullName, Director)
        VALUES (%s, %s, %s, %s, %s, %s)
        """

        record_to_insert = (
            data['i'],
            data['p'],
            data['o'],
            data['c'],
            data['n'],
            data['g'].replace("ГЕНЕРАЛЬНЫЙ ДИРЕКТОР:", "").strip()
        )

        cursor.execute(insert_query, record_to_insert)
        conn.commit()

        print("Данные были успешно вставлены")

    except Exception as error:
        print(f"Ошибка при вставке данных: {error}")

    finally:
        if conn:
            cursor.close()
            conn.close()

response = requests.post(search_url, data=search_data)

if response.status_code == 200:
    search_id = response.json()["t"]

    result_url = f"https://egrul.nalog.ru/search-result/{search_id}"

    result_response = requests.get(result_url)

    if result_response.status_code == 200:
        result_data = result_response.json()

        if "rows" in result_data:
            for row in result_data["rows"]:
                filtered_data = {key: row[key] for key in ['c', 'g', 'i', 'n', 'o', 'p']}
                insert_data_to_db(filtered_data)
        else:
            print("Никаких результатов найдено не было.")
    else:
        print(f"Ошибка при получении результатов: {result_response.status_code}")
else:
    print(f"Ошибка во время поиска: {response.status_code}")
