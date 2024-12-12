CREATE TABLE ENTITY
(
	Entity_ID serial             NOT NULL PRIMARY KEY,
	Type      varchar(20) UNIQUE NOT NULL, -- TODO почему оно уникально???
	Type_ID   uuid               NOT NULL
);

-- TODO может для Is_Deleted ставить DEFAULT FALSE ?
CREATE TABLE COMPANY
(
	Company_ID     uuid            NOT NULL PRIMARY KEY,
	INN            char(12) UNIQUE NOT NULL,
	KPP            char(12) UNIQUE NOT NULL,
	OGRN           char(13) UNIQUE NOT NULL,
	FullName       varchar(100)    NOT NULL,
	ShortName      varchar(100)    NOT NULL,
	Email          varchar(100)    NOT NULL,
	Password       text            NOT NULL,
	Legal_Address  varchar(1000)   NOT NULL,
	Postal_Address varchar(1000)   NOT NULL,
	Director       text            NOT NULL,
	Logo           bytea,
	Is_Deleted     bool            NOT NULL
);

CREATE TABLE "user"
(
	User_ID    uuid         NOT NULL PRIMARY KEY,
	FirstName  varchar(100) NOT NULL,
	SecondName varchar(100) NOT NULL,
	ThirdName  varchar(100) NOT NULL,
	Phone      varchar(20)  NOT NULL,
	Email      varchar(100) NOT NULL,
	Password   text         NOT NULL,
	Logo       bytea,
	Is_Deleted bool         NOT NULL
);

CREATE TABLE MANAGER
(
	Manager_ID uuid         NOT NULL PRIMARY KEY,
	User_ID    uuid         NOT NULL REFERENCES "user" (User_ID),
	Login      varchar(100) NOT NULL,
	Password   varchar(100) NOT NULL,
	Access     varchar(100) NOT NULL, -- what type???
	Is_Deleted bool         NOT NULL
);

CREATE TABLE COMPANY_MANAGERS
(
	Company_Managers_ID serial NOT NULL UNIQUE PRIMARY KEY,
	Company_ID          uuid   NOT NULL REFERENCES COMPANY (Company_ID),
	Manager_ID          uuid   NOT NULL REFERENCES MANAGER (Manager_ID)
);

CREATE TABLE CONTRACT
(
	Contract_ID         uuid         NOT NULL PRIMARY KEY,
	Company_Managers_ID serial       NOT NULL REFERENCES COMPANY_MANAGERS (Company_Managers_ID),
	Title               varchar(100) NOT NULL,
	File                varchar(100) NOT NULL,
	Is_Deleted          bool         NOT NULL
);

CREATE TABLE COUNTERPARTY
(
	Counterparty_ID uuid            NOT NULL PRIMARY KEY,
	INN             char(12) UNIQUE NOT NULL,
	KPP             char(12) UNIQUE NOT NULL,
	OGRN            char(13) UNIQUE NOT NULL,
	FullName        varchar(100)    NOT NULL,
	ShortName       varchar(100)    NOT NULL,
	Address         varchar(100)    NOT NULL,
	Payment_Account char(20)        NOT NULL, -- 20 символов
	Cor_Account     char(20)        NOT NULL, -- 20 символов
	BIK             varchar(11)     NOT NULL, -- размер от 8 до 11
	Bank            varchar(100)    NOT NULL, -- ?
	Is_Deleted      bool            NOT NULL
);

CREATE TABLE ENTITY_COUNTERPARTY
(
	Entity_Counterparty_ID serial NOT NULL PRIMARY KEY,
	Entity_ID              serial NOT NULL REFERENCES ENTITY (Entity_ID),
	Counterparty_ID        uuid   NOT NULL REFERENCES COUNTERPARTY (Counterparty_ID)
);

CREATE TABLE CURRENCY
(
	Currency_ID serial       NOT NULL PRIMARY KEY,
	Title       varchar(100) NOT NULL,
	Country     varchar(100) NOT NULL
);

CREATE TABLE ENTITY_CURRENCY
(
	Entity_Currency_ID serial NOT NULL PRIMARY KEY,
	Company_ID         uuid   NOT NULL REFERENCES COMPANY (Company_ID),
	Currency_ID        serial NOT NULL REFERENCES CURRENCY (Currency_ID)
);

CREATE TABLE MARKING
(
	Marking_ID serial      NOT NULL PRIMARY KEY,
	Title      varchar(50) NOT NULL,
	Company_ID uuid        NOT NULL REFERENCES COMPANY (Company_ID)
);

CREATE TABLE STORAGE
(
	Storage_ID uuid         NOT NULL PRIMARY KEY,
	Point      varchar(100) NOT NULL, -- ?
	Country    varchar(100) NOT NULL,
	City       varchar(100) NOT NULL,
	Address    varchar(100) NOT NULL,
	Index      varchar(100) NOT NULL, -- ?
	Is_Deleted bool         NOT NULL
);

CREATE TABLE ENTITY_STORAGE
(
	Entity_Storage_ID serial NOT NULL PRIMARY KEY,
	Company_ID        uuid   NOT NULL REFERENCES COMPANY (Company_ID),
	Storage_ID        uuid   NOT NULL REFERENCES STORAGE (Storage_ID)
);

CREATE TABLE TRANSPORTER
(
	Transporter_ID uuid         NOT NULL PRIMARY KEY,
	FullName       varchar(100) NOT NULL,
	ShortName      varchar(100) NOT NULL,
	Country        varchar(100) NOT NULL,
	Is_Deleted     bool         NOT NULL
);

CREATE TABLE ENTITY_TRANSPORTER
(
	Entity_Transporter_ID serial NOT NULL PRIMARY KEY,
	Company_ID            uuid   NOT NULL REFERENCES COMPANY (Company_ID),
	Transporter_ID        uuid   NOT NULL REFERENCES TRANSPORTER (Transporter_ID)
);

CREATE TABLE STATUS
(
	Status_ID serial       NOT NULL PRIMARY KEY,
	Title     varchar(100) NOT NULL UNIQUE
);

CREATE TABLE PRODUCT
(
	Product_ID uuid             NOT NULL PRIMARY KEY,
	Code       varchar(100)     NOT NULL, -- просто кодовое название
	Title      varchar(100)     NOT NULL,
	Unit       varchar(20)      NOT NULL, -- литры, кг, штуки и тд
	Price      double precision NOT NULL,
	Image      bytea,
	Is_Deleted bool             NOT NULL
);

CREATE TABLE ENTITY_PRODUCT
(
	Entity_Product_ID      serial       NOT NULL PRIMARY KEY,
	Company_ID             uuid         NOT NULL REFERENCES COMPANY (Company_ID),
	Product_ID             uuid         NOT NULL REFERENCES PRODUCT (Product_ID),
	Entity_Storage_ID      serial       NOT NULL REFERENCES ENTITY_STORAGE (Entity_Storage_ID),
	Available_For_Shipment int          NOT NULL, -- наверное лучше double потому что например литров может быть не ровно
	In_Shipping_Area       int          NOT NULL, -- ?
	In_Reserve             int          NOT NULL, -- ?
	Party                  varchar(100) NOT NULL, -- ?
	Implementation_Period  date         NOT NULL, -- ?
	Expiration_Date        date         NOT NULL, -- ?
	Is_Deleted             bool         NOT NULL
);

CREATE TABLE "order"
(
	Order_ID               uuid         NOT NULL PRIMARY KEY,
	Company_Managers_ID    serial       NOT NULL REFERENCES COMPANY_MANAGERS (Company_Managers_ID),
	Entity_Counterparty_ID serial       NOT NULL REFERENCES ENTITY_COUNTERPARTY (Entity_Counterparty_ID),
	Entity_Currency_ID     serial       NOT NULL REFERENCES ENTITY_CURRENCY (Entity_Currency_ID),
	Entity_Storage_ID      serial       NOT NULL REFERENCES ENTITY_STORAGE (Entity_Storage_ID),
	Contract_ID            uuid         NOT NULL REFERENCES CONTRACT (Contract_ID),
	Plan_Date_Shipment     date         NOT NULL, -- надо чтобы время тоже было
	Shipping_Address       varchar(100) NOT NULL,
	Application_Date       date         NOT NULL, -- надо чтобы время тоже было
	Delivery_Point         varchar(100) NOT NULL,
	Delivery_Address       varchar(100) NOT NULL,
	PlanDate_Receipt       date         NOT NULL, -- надо чтобы время тоже было
	Entity_Transporter_ID  serial       NOT NULL REFERENCES ENTITY_TRANSPORTER (Entity_Transporter_ID),
	Status_ID              serial       NOT NULL REFERENCES STATUS (Status_ID),
	Marking_ID             serial       NOT NULL REFERENCES MARKING (Marking_ID),
	Comment                text         NOT NULL,
	Is_Deleted             bool         NOT NULL
);

CREATE TABLE ORDER_PRODUCT
(
	Order_Product_ID  uuid             NOT NULL PRIMARY KEY,
	Order_ID          uuid             NOT NULL REFERENCES "order" (Order_ID),
	Entity_Product_ID serial           NOT NULL REFERENCES ENTITY_PRODUCT (Entity_Product_ID),
	Count             int              NOT NULL,
	VAT               double precision NOT NULL, -- налог в числах
	Discount          double precision NOT NULL  -- может быть 0 но не null
);

CREATE TABLE PROJECT
(
	Project_ID serial       NOT NULL PRIMARY KEY,
	Title      varchar(100) NOT NULL,
	Company_ID uuid         NOT NULL REFERENCES COMPANY (Company_ID),
	Order_ID   uuid         NOT NULL REFERENCES "order" (Order_ID),
	Is_Deleted bool         NOT NULL
);

CREATE TABLE SUPPORT
(
    Support_ID SERIAL       NOT NULL PRIMARY KEY,
    Email      varchar(100) NOT NULL,
    Message    TEXT         NOT NULL
);

CREATE TABLE SUPPORT_IMAGES
(
    Support_Images_ID    SERIAL NOT NULL PRIMARY KEY,
    Support_ID  INT    NOT NULL REFERENCES SUPPORT (Support_ID),
    Image       BYTEA  NOT NULL
);
