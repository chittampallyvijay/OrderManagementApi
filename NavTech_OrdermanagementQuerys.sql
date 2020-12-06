	--Role Creation
	CREATE TABLE cSec_Roles(
	iRoleId int IDENTITY(1,1) NOT NULL Primary key,
	sRoleName nvarchar(max) NULL,
	iStatus int not null
	)

	--Product
	CREATE TABLE mCore_Products(
	iProduct int IDENTITY(1,1) NOT NULL Primary key,
	Name nvarchar(max) NOT NULL,
	Weight decimal(18,2),
	Height decimal(18,2),
	image varbinary,
	[Stoke  Keeping Units] nvarchar(max) NOT NULL,
	Barcode varbinary null,
	AvailableQty decimal(18,2)
	)

	CREATE TABLE mCore_Buyers(
	iBuyerId int IDENTITY(1,1) NOT NULL Primary key,
	iRoleId int NULL FOREIGN KEY  REFERENCES cSec_Roles(iRoleId),
	sBuyerName varchar(max) NOT NULL,
	sEmailId varchar(max) NOT NULL,
	sPhoneNo varchar(max) NOT NULL,
	)

	

	CREATE TABLE mCore_Orderstatus(
	iStatus int IDENTITY(1,1) NOT NULL Primary key,
	sStatus varchar(max) NOT NULL
	)

	CREATE TABLE mCore_Orders(
	iOrderId int IDENTITY(1,1) NOT NULL Primary key,
	iBuyerId int NULL FOREIGN KEY  REFERENCES mCore_Buyers(iBuyerId),
	iOrderStatus int NULL FOREIGN KEY  REFERENCES mCore_Orderstatus(iStatus),
	sShippingAddress nvarchar(max) NOT NULL,
	iStatus int 
	)
	CREATE TABLE mCore_OrderItem(
	iOrderId int NULL FOREIGN KEY  REFERENCES mCore_Orders(iOrderId),
	Quantity decimal(18,2),
	iProduct int NULL FOREIGN KEY  REFERENCES mCore_Products(iProduct),
	)
	
	--OrderStatus
	INSERT INTO mCore_Orderstatus
	VALUES('Placed'),('Approved'),('Cancelled'),('In Delivery'),('Completed')

	--Roles
	INSERT INTO cSec_Roles
	VALUES('Administrator',1),('Buyer',1)

	--Products
	INSERT INTO mCore_Products
	VALUES('Parker Pen',50,10,NULL,'Number',NULL,1000)
	INSERT INTO mCore_Products
	VALUES('MI 7 Pro Case-1',250.00,15.00,NULL,'Number',NULL,250)
	INSERT INTO mCore_Products
	VALUES('MI 7 Pro Case-2',250.00,15.00,NULL,'Number',NULL,250)
	INSERT INTO mCore_Products
	VALUES('MI 7 Pro Case-3',250.00,15.00,NULL,'Number',NULL,250)

	--Buyes
	INSERT INTO mCore_Buyers
	VALUES(1,'Admin','chittampally.vijay@gmail.com','09642223748')
	INSERT INTO mCore_Buyers
	VALUES(2,'Kiran Kumar','chittampally.kiran@gmail.com','09642223458')

	