CREATE TABLE ENTITY
(
	ID uuid             NOT NULL PRIMARY KEY,
	Type      varchar(20) UNIQUE NOT NULL,
	Type_ID   uuid               NOT NULL
);
CREATE TABLE COMPANY
(
	ID     uuid            NOT NULL PRIMARY KEY,
	INN            char(12) UNIQUE NOT NULL,
	KPP            char(12) UNIQUE NOT NULL,
	OGRN           char(13) UNIQUE NOT NULL,
	FullName       varchar(100)    NOT NULL,
	ShortName      varchar(100)    NOT NULL,
	Email          varchar(100)    NOT NULL,
	Password       varchar(100)    NOT NULL,
	Legal_Address  varchar(1000)   NOT NULL,
	Postal_Address varchar(1000)   NOT NULL,
	Director       text            NOT NULL,
	Logo           bytea,
	Is_Deleted     bool            NOT NULL
);
CREATE TABLE "user"
(
	ID    uuid         NOT NULL PRIMARY KEY,
	FirstName  varchar(100) NOT NULL,
	SecondName varchar(100) NOT NULL,
	ThirdName  varchar(100) NOT NULL,
	Phone      varchar(20)  NOT NULL,
	Email      varchar(100) NOT NULL,
	Password   varchar(100) NOT NULL,
	Logo       bytea,
	Is_Deleted bool         NOT NULL
);
CREATE TABLE ENTITY_MANAGERS
(
	ID uuid NOT NULL UNIQUE PRIMARY KEY,
	Entity_ID          uuid    NOT NULL REFERENCES ENTITY (ID),
	User_ID            uuid   NOT NULL REFERENCES "user" (ID),
	Access		       varchar(100) NOT NULL
);
CREATE TABLE ENTITY_STORAGE
(
	ID uuid       NOT NULL PRIMARY KEY,
	Entity_ID  uuid          NOT NULL,
	Point      varchar(100) NOT NULL,
	Country    varchar(100) NOT NULL,
	City       varchar(100) NOT NULL,
	Address    varchar(100) NOT NULL,
	Index      varchar(100) NOT NULL,
	Is_Deleted bool         NOT NULL
);
CREATE TABLE PRODUCT
(
	ID uuid           NOT NULL PRIMARY KEY,
	Entity_ID  uuid              NOT NULL REFERENCES ENTITY (ID),
	Code       varchar(100)     NOT NULL,
	Title      varchar(100)     NOT NULL,
	Unit       varchar(20)      NOT NULL,
	Price      double precision NOT NULL,
	Image      bytea,
	Entity_Storage_ID      uuid          NOT NULL REFERENCES ENTITY_STORAGE (ID),
	Available_For_Shipment double precision NOT NULL,
	Party                  varchar(100) NOT NULL,
	Implementation_Period  date         NOT NULL,
	Expiration_Date        date         NOT NULL,
	Is_Deleted bool             NOT NULL
);
CREATE TABLE "order"
(
	ID               	uuid       NOT NULL PRIMARY KEY,
	Entity_ID     	uuid          NOT NULL REFERENCES ENTITY (ID),
	Entity_Managers_ID     	uuid          NOT NULL REFERENCES ENTITY_MANAGERS (ID),
	INN             char(12) 		 NOT NULL,
	KPP             char(12) 		 NOT NULL,
	OGRN            char(13) 		 NOT NULL,
	FullName        varchar(100)    NOT NULL,
	Address         varchar(100)    NOT NULL,
	Payment_Account char(20)        NOT NULL,
	toCor_Account     char(20)        NOT NULL,
	toBIK             varchar(11)     NOT NULL,
	toBank            varchar(100)    NOT NULL,
	fromCor_Account     char(20)        NOT NULL,
	fromBIK             varchar(11)     NOT NULL,
	fromBank            varchar(100)    NOT NULL,
	Plan_Date_Shipment     	date         NOT NULL,
	Shipping_Address       	varchar(100) NOT NULL,
	Application_Date       	date         NOT NULL,
	Delivery_Point         	varchar(100) NOT NULL,
	Delivery_Address       	varchar(100) NOT NULL,
	Plan_Date_Receipt       date		 NOT NULL,
	TransporterFullName     varchar(100),
	TransporterShortName    varchar(100),
	Comment                	text         NOT NULL,
	VAT			    double precision,
	Is_Deleted		bool         NOT NULL
);
CREATE TABLE ENTITY_PRODUCT_ORDER
(
	ID uuid NOT NULL PRIMARY KEY,
	Product_ID    uuid NOT NULL REFERENCES PRODUCT (ID),
	Order_ID	         uuid NOT NULL REFERENCES "order" (ID),
	Count 				 int NOT NULL
);
CREATE TABLE SUPPORT
(
	ID uuid NOT NULL PRIMARY KEY,
	Entity_ID uuid NOT NULL REFERENCES ENTITY (ID),
	MESSAGE   TEXT   NOT NULL,
	Image	  bytea
);
