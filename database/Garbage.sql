INSERT INTO company (company_id, inn, kpp, ogrn, fullname, shortname, email, password, legal_address, postal_address,
					 director, logo, is_deleted)
	VALUES ('42E136F9-C325-4310-AA62-962A0B0E1ED5', 'a', 'a', 'a', 'Slava', 'Kuntsou', '1@gmail.com', '1', 'minsk',
			'minsk', 'director', NULL, FALSE);
INSERT INTO "user" (user_id, firstname, secondname, thirdname, phone, email, password, logo, is_deleted)
	VALUES ('FB945E5F-49B5-4629-8292-C6C783F25D1B', 'Misha', 'Ржавый', 'haha', '+375292345417',
			'q@gmail.com', 'q', NULL, FALSE);

SELECT * FROM "user";