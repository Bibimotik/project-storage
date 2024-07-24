-- uuid or serial?

CREATE TABLE COMPANY
(
	CompanyID     uuid            NOT NULL PRIMARY KEY,
	INN           char(12) UNIQUE NOT NULL,
	KPP           char(12) UNIQUE NOT NULL,
	OGRN          char(13)        NOT NULL,
	FullName      varchar(100)    NOT NULL,
	ShortName     varchar(100)    NOT NULL,
	LegalAddress  varchar(100)    NOT NULL,
	PostalAddress varchar(100)    NOT NULL,
	Director      text            NOT NULL,
	IsDeleted     bool            NOT NULL
);

CREATE TABLE "user"
(
	UserID     uuid         NOT NULL PRIMARY KEY,
	NickName   varchar(100) NOT NULL,
	FirstName  varchar(100) NOT NULL,
	SecondName varchar(100) NOT NULL,
	ThirdName  varchar(100) NOT NULL,
	Phone      varchar(20)  NOT NULL,
	Mail       varchar(100) NOT NULL,
	Password   varchar(100) NOT NULL,
	IsDeleted  bool         NOT NULL
);

CREATE TABLE MANAGER
(
	ManagerID uuid         NOT NULL PRIMARY KEY,
	UserID    uuid         NOT NULL REFERENCES "user" (UserID),
	Login     varchar(100) NOT NULL,
	Password  varchar(100) NOT NULL,
	Access    varchar(100) NOT NULL, -- what type???
	IsDeleted bool         NOT NULL
);

CREATE TABLE COMPANY_MANAGERS
(
	CompanyManagersID serial NOT NULL UNIQUE PRIMARY KEY, -- serial?
	CompanyID         uuid   NOT NULL REFERENCES COMPANY (CompanyID),
	ManagerID         uuid   NOT NULL REFERENCES MANAGER (ManagerID)
);

CREATE TABLE CONTRACT
(
	ContractID        uuid         NOT NULL PRIMARY KEY,
	CompanyManagersID serial       NOT NULL REFERENCES COMPANY_MANAGERS (CompanyManagersID),
	Title             varchar(100) NOT NULL,
	File              varchar(100) NOT NULL,
	IsDeleted         bool         NOT NULL
);

CREATE TABLE COUNTERPARTY
(
	CounterpartyID uuid            NOT NULL PRIMARY KEY,
	INN            char(12) UNIQUE NOT NULL,
	KPP            char(12) UNIQUE NOT NULL,
	OGRN           char(13)        NOT NULL,
	FullName       varchar(100)    NOT NULL,
	ShortName      varchar(100)    NOT NULL,
	Address        varchar(100)    NOT NULL,
	PaymentAccount varchar(100)    NOT NULL, -- ?
	CorAccount     varchar(100)    NOT NULL, -- ?
	BIK            varchar(100)    NOT NULL, -- ?
	Bank           varchar(100)    NOT NULL, -- ?
	IsDeleted      bool            NOT NULL
);

CREATE TABLE COMPANY_COUNTERPARTY
(

	CompanyCounterpartyID serial NOT NULL UNIQUE PRIMARY KEY, -- serial?
	CompanyID             uuid   NOT NULL REFERENCES COMPANY (CompanyID),
	CounterpartyID        uuid   NOT NULL REFERENCES COUNTERPARTY (CounterpartyID)
);

CREATE TABLE CURRENCY
(
	CurrencyID serial       NOT NULL PRIMARY KEY,
	Title      varchar(100) NOT NULL,
	Country    varchar(100) NOT NULL
);

CREATE TABLE COMPANY_CURRENCY
(
	CompanyCurrencyID serial       NOT NULL PRIMARY KEY,
	CompanyID         uuid         NOT NULL REFERENCES COMPANY (CompanyID),
	CurrencyID        varchar(100) NOT NULL REFERENCES CURRENCY (CurrencyID)
);

CREATE TABLE MARKING
(
	MarkingID serial       NOT NULL PRIMARY KEY,
	Title     varchar(100) NOT NULL,
	CompanyID uuid         NOT NULL REFERENCES COMPANY (CompanyID)
);

CREATE TABLE PROJECT
(
	ProjectID uuid         NOT NULL PRIMARY KEY, -- what type???
	Title     varchar(100) NOT NULL,
	CompanyID uuid         NOT NULL REFERENCES COMPANY (CompanyID),
	OrderID   uuid         NOT NULL REFERENCES "order" (OrderID),
	IsDeleted bool         NOT NULL
);

CREATE TABLE "order"
(
	OrderID               uuid         NOT NULL PRIMARY KEY,
	CompanyManagersID     serial       NOT NULL REFERENCES COMPANY_MANAGERS (CompanyManagersID),
	CompanyCounterpartyID serial       NOT NULL REFERENCES COMPANY_COUNTERPARTY (CompanyCounterpartyID),
	CompanyCurrencyID     serial       NOT NULL REFERENCES COMPANY_CURRENCY (CompanyCurrencyID),
	CompanyStorageID      uuid         NOT NULL REFERENCES COMPANY_STORAGE (CompanyStorageID),
	ContractID            uuid         NOT NULL REFERENCES CONTRACT (ContractID),
	PlanDateShipment      date         NOT NULL, -- ?
	ShippingAddress       varchar(100) NOT NULL, -- ?
	ApplicationDate       date         NOT NULL, -- ?
	DeliveryPoint         varchar(100) NOT NULL, -- ?
	DeliveryAddress       varchar(100) NOT NULL, -- ?
	PlanDateReceipt       date         NOT NULL, -- ?
	CompanyTransporterID  uuid         NOT NULL REFERENCES COMPANY_TRANSPORTER (CompanyTransporterID),
	StatusID              uuid         NOT NULL REFERENCES STATUS (StatusID),
	MarkingID             uuid         NOT NULL REFERENCES MARKING (MarkingID),
	Comment               text         NOT NULL,
	IsDeleted             bool         NOT NULL
);

CREATE TABLE ORDER_PRODUCT
(
	OrderProductID   uuid         NOT NULL PRIMARY KEY,
	OrderID          uuid         NOT NULL REFERENCES "order" (OrderID),
	CompanyProductID uuid         NOT NULL REFERENCES COMPANY_PRODUCT (CompanyProductID),
	Count            int          NOT NULL,
	VAT              varchar(100) NOT NULL, -- ?
	Discount         double precision       -- not null? what type???
);

CREATE TABLE STATUS
(
	StatusID serial       NOT NULL PRIMARY KEY,
	Title    varchar(100) NOT NULL UNIQUE
);

CREATE TABLE PRODUCT
(
	ProductID uuid             NOT NULL PRIMARY KEY,
	Code      varchar(20)      NOT NULL, -- ?
	Title     varchar(100)     NOT NULL,
	Unit      varchar(20)      NOT NULL, -- ?
	Price     double precision NOT NULL,
	IsDeleted bool             NOT NULL
);

CREATE TABLE COMPANY_PRODUCT
(
	CompanyProductID     uuid         NOT NULL PRIMARY KEY, -- serial?
	CompanyID            uuid         NOT NULL REFERENCES COMPANY (CompanyID),
	ProductID            uuid         NOT NULL REFERENCES PRODUCT (ProductID),
	CompanyStorageID     uuid         NOT NULL REFERENCES COMPANY_STORAGE (CompanyStorageID),
	AvailableForShipment int          NOT NULL,             -- ?
	InShippingArea       int          NOT NULL,             -- ?
	InReserve            int          NOT NULL,             -- ?
	Party                varchar(100) NOT NULL,             -- ?
	ImplementationPeriod date         NOT NULL,             -- ?
	ExpirationDate       date         NOT NULL,             -- ?
	IsDeleted            bool         NOT NULL
);

CREATE TABLE TRANSPORTER
(
	TransporterID uuid         NOT NULL PRIMARY KEY,
	FullName      varchar(100) NOT NULL,
	ShortName     varchar(100) NOT NULL,
	Country       varchar(100) NOT NULL,
	IsDeleted     bool         NOT NULL
);

CREATE TABLE COMPANY_TRANSPORTER
(
	CompanyTransporterID serial NOT NULL PRIMARY KEY,
	CompanyID            uuid   NOT NULL REFERENCES COMPANY (CompanyID),
	TransporterID        uuid   NOT NULL REFERENCES TRANSPORTER (TransporterID)
);

CREATE TABLE STORAGE
(
	StorageID uuid         NOT NULL PRIMARY KEY,
	Point     varchar(100) NOT NULL, -- ?
	Country   varchar(100) NOT NULL,
	City      varchar(100) NOT NULL,
	Address   varchar(100) NOT NULL,
	Index     varchar(100) NOT NULL, -- ?
	IsDeleted bool         NOT NULL
);

CREATE TABLE COMPANY_STORAGE
(
	CompanyStorageID serial NOT NULL PRIMARY KEY,
	CompanyID        uuid   NOT NULL REFERENCES COMPANY (CompanyID),
	StorageID        uuid   NOT NULL REFERENCES STORAGE (StorageID)
);

