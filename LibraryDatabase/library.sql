use master
go
IF EXISTS(SELECT 1 FROM master.dbo.sysdatabases WHERE name = 'library') 
	BEGIN
		DROP DATABASE library
	END
GO

CREATE DATABASE library
GO
USE library
GO

CREATE TABLE [dbo].[Staff](
	[StaffID] 		[int] IDENTITY (1000, 1)		NOT NULL,
	[FirstName]		[varchar](50)			NOT NULL,
	[LastName]		[varchar](50)			NOT NULL,
	[Email]			[varchar](100)			NULL,
	[Phone]			[char](10)				NULL,
	[UserName]		[varchar](20)			NOT NULL,
	[Password]		[varchar](256)			NOT NULL	DEFAULT '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8',
	[Active]		[bit]					NOT NULL	DEFAULT 1

	CONSTRAINT [pk_StaffID] PRIMARY KEY([StaffID] ASC),
	CONSTRAINT [ak_StaffUserName] UNIQUE ([UserName] ASC)
)
GO

CREATE TABLE [dbo].[Book](
	[BookID] 			[varchar](20)				NOT NULL,
	[PublisherID]		[varchar](50)				NOT NULL,
	[AuthorID]			[int]						NOT NULL,
	[Title]				[varchar](50)				NOT NULL,
	[Pages]				[int]						NULL,
	[Copies]			[int]						NOT NULL,
	[Active]			[bit]						NOT NULL 	DEFAULT 1

	CONSTRAINT [pk_BookID] PRIMARY KEY([BookID] ASC)
)
GO

CREATE TABLE [dbo].[Author](
	[AuthorID] 		[int] IDENTITY (1000, 1)	NOT NULL,
	[FirstName]		[varchar](50)				NOT NULL,
	[LastName]		[varchar](50)				NOT NULL,
	[Active]		[bit]						NOT NULL 	DEFAULT 1

	CONSTRAINT [pk_AuthorID] PRIMARY KEY([AuthorID] ASC)
)
GO

CREATE TABLE [dbo].[Publisher](
	[PublisherID] 	[varchar](50)	NOT NULL,
	[Phone]			[char](10)		NOT NULL,
	[Active]		[bit]			NOT NULL 	DEFAULT 1

	CONSTRAINT [pk_PublisherID] PRIMARY KEY([PublisherID] ASC)
)
GO

CREATE TABLE [dbo].[Order](
	[OrderID] 		[int] IDENTITY (1000,1)	NOT NULL,
	[CustomerID]	[int]					NOT NULL,
	[DateOrdered]	[datetime]				NOT NULL

	CONSTRAINT [pk_OrderID] PRIMARY KEY([OrderID] ASC)
)
GO

CREATE TABLE [dbo].[OrderLine](
	[OrderID]		[int]						NOT NULL,
	[BookID]		[varchar](20)				NOT NULL,
	[Price]			[decimal](4,2)				NULL,
	[Quantity]		[int]						NOT NULL

	CONSTRAINT [pk_OrderLineID] PRIMARY KEY CLUSTERED([OrderID], [BookID]  ASC)
)
GO

CREATE TABLE [dbo].[Review](
	[BookID]		[varchar](20)		NOT NULL,
	[RentalID]		[int]				NOT NULL,
	[Content]		[varchar](2000)		NULL,
	[Active]		[bit]				NOT NULL	DEFAULT 1

	CONSTRAINT [pk_ReviewID] PRIMARY KEY CLUSTERED([BookID], [RentalID]  ASC)
)
GO

CREATE TABLE [dbo].[Rental](
	[RentalID] 		[int] IDENTITY (1000,1)		NOT NULL,
	[CustomerID]	[int]						NOT NULL,
	[DateRented]	[datetime]					NOT NULL,
	[DateDue]		[datetime]					NOT NULL,
	[LateFee]		[decimal](4,2)				NOT NULL,
	[Active]		[bit]						NOT NULL	DEFAULT 1

	CONSTRAINT [pk_RentalID] PRIMARY KEY([RentalID] ASC)
)
GO

CREATE TABLE [dbo].[RentalLine](
	[RentalID] 		[int]						NOT NULL,
	[BookID]		[varchar](20)				NOT NULL

	CONSTRAINT [pk_RentalLineID] PRIMARY KEY CLUSTERED([BookID], [RentalID]  ASC)
)
GO

CREATE TABLE [dbo].[Customer](
	[CustomerID] 	[int] IDENTITY (1000, 1)	NOT NULL,
	[FirstName]		[varchar](50)				NOT NULL,
	[LastName]		[varchar](50)				NOT NULL,
	[Email]			[varchar](100)				NULL,
	[Phone]			[char](10)					NULL,
	[UserName]		[varchar](20)				NOT NULL,
	[Password]		[varchar](256)				NOT NULL	DEFAULT '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8',
	[Active]		[bit]						NOT NULL	DEFAULT 1

	CONSTRAINT [pk_CustomerID] PRIMARY KEY([CustomerID] ASC),
	CONSTRAINT [ak_CustomerUserName] UNIQUE ([UserName] ASC)
)
GO

CREATE TABLE [dbo].[Roles](
	RoleID varchar(30) primary key not null,
	Description varchar(100) not null
);
GO

CREATE TABLE [dbo].[UserRoles](
	UserID int not null,
	RoleID varchar(30) not null

	CONSTRAINT [PK_UserRoles] PRIMARY KEY ( UserID, RoleID ASC )
);
GO


/* Order */
ALTER TABLE [dbo].[Order] WITH NOCHECK ADD CONSTRAINT [fk_Order_Customer] FOREIGN KEY(CustomerID)
REFERENCES [dbo].[Customer]([CustomerID])
ON UPDATE CASCADE 
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [fk_Order_Customer]
GO

/* OrderLine */
ALTER TABLE [dbo].[OrderLine] WITH NOCHECK ADD CONSTRAINT [fk_OrderLine_Order] FOREIGN KEY(OrderID)
REFERENCES [dbo].[Order]([OrderID])
ON UPDATE CASCADE 
GO
ALTER TABLE [dbo].[OrderLine] CHECK CONSTRAINT [fk_OrderLine_Order]
GO

ALTER TABLE [dbo].[OrderLine] WITH NOCHECK ADD CONSTRAINT [fk_OrderLine_Book] FOREIGN KEY(BookID)
REFERENCES [dbo].[Book]([BookID])
ON UPDATE CASCADE 
GO
ALTER TABLE [dbo].[OrderLine] CHECK CONSTRAINT [fk_OrderLine_Book]
GO

/* Book */
ALTER TABLE [dbo].[Book] WITH NOCHECK ADD CONSTRAINT [fk_Book_Publisher] FOREIGN KEY(PublisherID)
REFERENCES [dbo].[Publisher]([PublisherID])
ON UPDATE CASCADE 
GO
ALTER TABLE [dbo].[Book] CHECK CONSTRAINT [fk_Book_Publisher]
GO

ALTER TABLE [dbo].[Book] WITH NOCHECK ADD CONSTRAINT [fk_Book_Author] FOREIGN KEY(AuthorID)
REFERENCES [dbo].[Author]([AuthorID])
ON UPDATE CASCADE 
GO
ALTER TABLE [dbo].[Book] CHECK CONSTRAINT [fk_Book_Author]
GO

/* Review */
ALTER TABLE [dbo].[Review] WITH NOCHECK ADD CONSTRAINT [fk_Review_Book] FOREIGN KEY(BookID)
REFERENCES [dbo].[Book]([BookID])
ON UPDATE CASCADE 
GO
ALTER TABLE [dbo].[Review] CHECK CONSTRAINT [fk_Review_Book]
GO

ALTER TABLE [dbo].[Review] WITH NOCHECK ADD CONSTRAINT [fk_Review_Rental] FOREIGN KEY(RentalID)
REFERENCES [dbo].[Rental]([RentalID])
ON UPDATE CASCADE 
GO
ALTER TABLE [dbo].[Review] CHECK CONSTRAINT [fk_Review_Rental]
GO

/* Rental */
ALTER TABLE [dbo].[Rental] WITH NOCHECK ADD CONSTRAINT [fk_Rental_Customer] FOREIGN KEY(CustomerID)
REFERENCES [dbo].[Customer]([CustomerID])
ON UPDATE CASCADE 
GO
ALTER TABLE [dbo].[Rental] CHECK CONSTRAINT [fk_Rental_Customer]
GO

/* Rental */
ALTER TABLE [dbo].[RentalLine] WITH NOCHECK ADD CONSTRAINT [fk_RentalLine_Book] FOREIGN KEY(BookID)
REFERENCES [dbo].[Book]([BookID])
ON UPDATE CASCADE 
GO
ALTER TABLE [dbo].[RentalLine] CHECK CONSTRAINT [fk_RentalLine_Book]
GO

ALTER TABLE [dbo].[RentalLine] WITH NOCHECK ADD CONSTRAINT [fk_RentalLine_Rental] FOREIGN KEY(RentalID)
REFERENCES [dbo].[Rental]([RentalID])
ON UPDATE CASCADE 
GO
ALTER TABLE [dbo].[RentalLine] CHECK CONSTRAINT [fk_RentalLine_Rental]
GO

/* Roles */
ALTER TABLE [dbo].[UserRoles] WITH NOCHECK ADD  CONSTRAINT [FK_UserRoles_UserID] FOREIGN KEY(UserID)
REFERENCES [dbo].[Customer](CustomerID)
ON UPDATE CASCADE 
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_UserID]
GO



/* Staff */
INSERT INTO [dbo].[Staff] ( [FirstName], [LastName], [Email], [Phone], [UserName])
	VALUES	('Rhett', 'Allen', '14rallen@gmail.com', '3195551111', 'rhettallen'),
			('John', 'Doe', 'johndoe@gmail.com', '3195552222', 'johndoe');
GO

/* Customer */
INSERT INTO [dbo].[Customer] ( [FirstName], [LastName], [Email], [Phone], [UserName])
	VALUES	('Rhett', 'Allen', '14rallen@gmail.com', '3195551111', 'rhettallen'),
			('John', 'Doe', 'johndoe@gmail.com', '3195552222', 'johndoe'),
			('User', 'User', 'user@gmail.com', '3195553333', 'user');
GO

/* Roles */
INSERT INTO [dbo].[Roles] ( [RoleID], [Description])
	VALUES	('Admin', 'Has access to entire application'),
			('Employee', 'Maintains the content and flow of application')
GO

/* User Roles */
INSERT INTO [dbo].[UserRoles] ( [UserID], [RoleID])
	VALUES	(1000, 'Employee'),
			(1000, 'Admin')
GO

/* Publisher */
INSERT INTO [dbo].[Publisher] ( [PublisherID], [Phone])
	VALUES	("Charles Scribner's Sons", "3195551234"),
			("Sylvia Beach", "3195551234"),
			("Random House", "3195551234"),
			("Bobbs Merill", "3195551234"),
			("Macmillan", "3195551234"),
			("The Viking Press-James Lloyd", "3195551235");
GO

/* Order */
INSERT INTO [dbo].[Order] ([CustomerID], [DateOrdered])
	VALUES	(1000, getdate()),
			(1000, getdate());
GO

/* Author */
INSERT INTO [dbo].[Author] ([FirstName], [LastName])
	VALUES	("F.", "Fitzgerald"),
			("John", "Steinbeck"),
			("James", "Joyce"),
			("Ayn", "Rand"),
			("Harper", "Lee");
GO

/* Book */
INSERT INTO [dbo].[Book] ( [BookID], [PublisherID], [AuthorID], [Title], [Pages], [Copies])
	VALUES	('9781597226769', "Charles Scribner's Sons", 1000, 'The Great Gatsby', 300, 20),
			('9781743380468', "Charles Scribner's Sons", 1000, 'This Side of Paradise', 245, 13),
			('9781593082451', "Charles Scribner's Sons", 1000, 'The Beautiful and Damned', 197, 8),
			('9788944101205', "The Viking Press-James Lloyd", 1001, 'The Grapes of Wrath', 200, 17),
			('9781486143757', "Sylvia Beach", 1002, 'Ulysses', 200, 17),
			('9789872095109', "Random House", 1003, 'Atlas Shrugged', 200, 17),
			('9788879726481', "Bobbs Merill", 1003, 'The Fountainhead', 200, 17),
			('9780451173959', "Macmillan", 1003, 'We the Living', 200, 17),
			('9788931001990', "The Viking Press-James Lloyd", 1004, 'To Kill a Mockingbird', 200, 17);
GO


/* Rental */
INSERT INTO [dbo].[Rental] ( [CustomerID], [DateRented], [DateDue], [LateFee])
	VALUES	(1000, '2015-11-21 00:49:04.260', '2015-12-05 00:49:04.260', 0.25),
			(1001, '2015-11-22 00:49:04.260', '2015-12-06 00:49:04.260', 0.25);
GO

/* Review */
INSERT INTO [dbo].[Review] ( [RentalID], [BookID], [Content])
	VALUES	(1000, '9781597226769', "Having reread this book for the first time in 20 years, I can confirm that there's a reason that it's considered one " +
									"of the very best American novels. However, my reaction to the story was different than when I first read it in high school. " +
									"I recall that back then I was hoping that Daisy and Gatsby's love story would ultimately yield a happy ending. Now, I found " +
									"them both to be such shallow creatures that they inspired no pity. While I considered the characters to be emotionally stunted, " +
									"that doesn't mean I was not impressed with Fitzergerald's skillful rendering. As in most forms of art, in literature it is more " +
									"difficult to accurately and interestingly portray nothingness than to describe a richly endowed subject. At this more cynical age, " +
									"I found Daisy to be a remarkable emotional void, and Gatsby's quest to pour all of his hopes and dreams into such a shallow cauldron " +
									"only confirmed his own vapidity. One thing that hasn't changed in all these years is my amazement at Fitzgerald's ability to set a scene. " +
									"His descriptive passages are truly poetic, and his command of word choice in unparalleled. All this made for a stimulating and delightful read."),
			(1001, '9788944101205', "The Grapes of Wrath was not worth my time.");
GO

/* Rental Line */
INSERT INTO [dbo].[RentalLine] ( [RentalID], [BookID])
	VALUES	(1000, '9781597226769'),
			(1001, '9788944101205');
GO

/* Order Line */
INSERT INTO [dbo].[OrderLine] ( [BookID], [OrderID], [Price], [Quantity])
	VALUES	('9781597226769', 1000, 50.00, 2),
			('9781597226769', 1001, 70.00, 3);
GO

CREATE PROCEDURE sp_select_roles (
    @userID INT
)
AS
BEGIN
    SELECT UserRoles.RoleID, Roles.Description
    FROM [dbo].[Roles], [dbo].[UserRoles]
    WHERE UserRoles.UserID = @userID
    AND Roles.roleID = UserRoles.roleID;
END;
GO

/* sp select staff by username */
CREATE PROCEDURE sp_select_staff_by_username (
	@username varchar(20)
)
AS
BEGIN
	SELECT StaffID, FirstName, LastName, Email, Phone, UserName, Password, Active
	FROM [dbo].[Staff]
	WHERE UserName = @username
END
GO

/* sp validate staff login */
CREATE PROCEDURE sp_validate_login_for_staff (
	@username varchar(20),
	@password varchar(256)
	)
	AS
	BEGIN
		SELECT COUNT(UserName)
		FROM Staff
		WHERE
			UserName = @username
		AND Password = @password
		AND Active = 1
	END
GO

/* sp update staff password */
CREATE PROCEDURE sp_update_password_for_staff (
	@username varchar(20),
	@oldPassword nvarchar(256),
	@newPassword nvarchar(256)
	)
	AS
	BEGIN
		UPDATE Staff
			SET Password = @newPassword
		WHERE
			UserName = @username
		AND Password = @oldPassword
		AND Active = 1
		RETURN @@rowcount
	END
GO

/* sp update customer */
CREATE PROCEDURE sp_update_customer (
	@firstName varchar(50),
	@lastName varchar(50),
	@email varchar(100) = null,
	@phone varchar(10) = null,
	@username varchar(20),
	@active bit
	)
	AS
	BEGIN
		UPDATE Customer
			SET FirstName = @firstName,
			 LastName = @lastName,
			 Email = ISNULL(@email, null),
			 Phone = ISNULL(@phone, null),
			 Active = @active
		WHERE UserName = @username
		RETURN @@rowcount
	END
GO

/* sp select customer by username */
CREATE PROCEDURE sp_select_customer_by_username (
	@username varchar(20)
)
AS
BEGIN
	SELECT CustomerID, FirstName, LastName, Email, Phone, UserName, Password, Active
	FROM [dbo].[Customer]
	WHERE UserName = @username
END
GO

/* sp select customer by id */
CREATE PROCEDURE sp_select_customer_by_id (
	@customerID varchar(20)
)
AS
BEGIN
	SELECT CustomerID, FirstName, LastName, Email, Phone, UserName, Password, Active
	FROM [dbo].[Customer]
	WHERE CustomerID = @customerID
END
GO

/* sp select staff by id */
CREATE PROCEDURE sp_select_staff_by_id (
	@staffID varchar(20)
)
AS
BEGIN
	SELECT StaffID, FirstName, LastName, Email, Phone, UserName, Password, Active
	FROM [dbo].[Staff]
	WHERE StaffID = @staffID
END
GO

/* sp validate customer login */
CREATE PROCEDURE sp_validate_login_for_customer (
	@username varchar(20),
	@password varchar(256)
	)
	AS
	BEGIN
		SELECT COUNT(UserName)
		FROM Customer
		WHERE
			UserName = @username
		AND Password = @password
		AND Active = 1
	END
GO

/* sp update customer password */
CREATE PROCEDURE sp_update_password_for_customer (
	@username varchar(20),
	@oldPassword varchar(256),
	@newPassword varchar(256)
	)
	AS
	BEGIN
		UPDATE Customer
			SET Password = @newPassword
		WHERE
			UserName = @username
		AND Password = @oldPassword
		AND Active = 1
		RETURN @@rowcount
	END
GO

/* sp insert into staff */
CREATE PROCEDURE sp_insert_staff
	@firstName		[varchar](50),
	@lastName		[varchar](100),
	@email			[varchar](100),
	@phone			[char](10),
	@userName		[varchar](20),
	@password		[varchar](256)
AS  
BEGIN 
	INSERT INTO [dbo].[Staff] ( [FirstName], [LastName], [Email], [Phone], [UserName], [Password])
	VALUES	(@firstName, @lastName, @email, @phone, @userName, @password)
END
GO

/* sp insert into customer */
CREATE PROCEDURE sp_insert_customer
	@firstName		[varchar](50),
	@lastName		[varchar](100),
	@email			[varchar](100) = null,
	@phone			[char](10) = null,
	@userName		[varchar](20),
	@password		[varchar](256)
AS  
BEGIN 
	INSERT INTO [dbo].[Customer] ( [FirstName], [LastName], [Email], [Phone], [UserName], [Password])
	VALUES	(@firstName, @lastName, ISNULL(@email, null), ISNULL(@phone, null), @userName, @password)
END
GO

/* sp insert into book */
CREATE PROCEDURE sp_insert_book
	@bookID			[varchar](20),
	@publisherID	[varchar](50),
	@authorID		[int],
	@title			[varchar](50),
	@pages			[int],
	@copies			[int]
AS  
BEGIN 
	INSERT INTO [dbo].[Book] ( [BookID], [PublisherID], [AuthorID], [Title], [Pages], [Copies])
	VALUES	(@bookID, @publisherID, @authorID, @title, @pages, @copies)
END
GO

/* sp insert into author */
CREATE PROCEDURE sp_insert_author
	@firstName		[varchar](50),
	@lastName		[varchar](50)
AS  
BEGIN 
	INSERT INTO [dbo].[Author] ([FirstName], [LastName])
	VALUES	(@firstName, @lastName)
END
GO

/* sp insert into publisher */
CREATE PROCEDURE sp_insert_publisher
	@publisherID	[varchar](50),
	@phone			[char](10)
AS  
BEGIN 
	INSERT INTO [dbo].[Publisher] ([PublisherID], [Phone])
	VALUES	(@publisherID, @phone)
END
GO

/* sp insert into order */
CREATE PROCEDURE sp_insert_order
	@customerID		[int],
	@bookID			[varchar](20),
	@quantity		[int]
AS  
BEGIN 
	DECLARE @newOrderID INT
	
	INSERT INTO [dbo].[Order] ([CustomerID], [DateOrdered])
	VALUES	(@customerID, getDate())
	
	SELECT @newOrderID = SCOPE_IDENTITY();
	
	INSERT INTO [dbo].[OrderLine] ( [BookID], [OrderID], [Quantity])
	VALUES	(@bookID, @newOrderID, @quantity)
END
GO

/* sp insert into order line */
CREATE PROCEDURE sp_insert_orderline
	@bookID		[varchar](20),
	@orderID	[int],
	@price		[decimal](4,2),
	@quantity	[int]
AS  
BEGIN 
	INSERT INTO [dbo].[OrderLine] ( [BookID], [OrderID], [Price], [Quantity])
	VALUES	(@bookID, @orderID, @price, @quantity)
END
GO

/* sp insert into rental */
CREATE PROCEDURE sp_insert_rental
	@customerID		[int],
	@dateRented		[datetime],
	@dateDue		[datetime],
	@lateFee		[decimal](4,2)
AS  
BEGIN 
	INSERT INTO [dbo].[Rental] ([CustomerID], [DateRented], [DateDue], [LateFee])
	VALUES	(@customerID, @dateRented, @dateDue, @lateFee)
END
GO

/* sp insert into rental with book*/
CREATE PROCEDURE sp_insert_rental_with_book
	@customerID		[int],
	@dateRented		[datetime],
	@dateDue		[datetime],
	@lateFee		[decimal](4,2),
	@bookID			[varchar](20)
AS  
BEGIN 
	DECLARE @newRentalID INT
	
	INSERT INTO [dbo].[Rental] ([CustomerID], [DateRented], [DateDue], [LateFee])
	VALUES	(@customerID, @dateRented, @dateDue, @lateFee)
	
	SELECT @newRentalID = SCOPE_IDENTITY();
	
	INSERT INTO [dbo].[RentalLine] ([RentalID], [BookID])
	VALUES	(@newRentalID, @bookID)
END
GO

/* sp insert into rental line */
CREATE PROCEDURE sp_insert_rentalline
	@rentalID		[int],
	@bookID			[varchar](20)
AS  
BEGIN 
	INSERT INTO [dbo].[RentalLine] ([RentalID], [BookID])
	VALUES	(@rentalID, @bookID)
END
GO

/* sp insert into review */
CREATE PROCEDURE sp_insert_review
	@rentalID		[int],
	@bookID			[varchar](20),
	@content		[varchar](2000)
AS  
BEGIN 
	INSERT INTO [dbo].[Review] ([RentalID], [BookID], [Content])
	VALUES	(@rentalID, @bookID, @content)
END
GO

/* sp select book by author */
CREATE PROCEDURE sp_select_book_by_author (
	@authorID [int]
)
AS
BEGIN
	SELECT BookID, PublisherID, AuthorID, Title, Pages, Copies, Active
	FROM [dbo].[Book]
	WHERE AuthorID = @authorID AND Active = 1
END
GO

/* sp select book by customer */
CREATE PROCEDURE sp_select_book_by_customer (
	@customerID [int]
)
AS
BEGIN
	SELECT Book.BookID, PublisherID, AuthorID, Title, Pages, Copies, Book.Active
	FROM [dbo].[Book], [dbo].[Rental], [dbo].[RentalLine]
	WHERE Rental.CustomerID = @customerID
		AND Rental.RentalID = RentalLine.RentalID
		AND RentalLine.BookID = Book.BookID
		AND Rental.Active = 1
END
GO

/* sp select book by rental */
CREATE PROCEDURE sp_select_book_by_rental (
	@rentalID [int]
)
AS
BEGIN
	SELECT Book.BookID, PublisherID, AuthorID, Title, Pages, Copies, Book.Active
	FROM [dbo].[Book], [dbo].[Rental], [dbo].[RentalLine]
	WHERE RentalLine.RentalID = @rentalID
		AND RentalLine.BookID = Book.BookID
		AND Rental.Active = 1
END
GO

/* sp select book by isbn */
CREATE PROCEDURE sp_select_book_by_isbn (
	@isbn [varchar](20)
)
AS
BEGIN
	SELECT BookID, PublisherID, AuthorID, Title, Pages, Copies, Active
	FROM [dbo].[Book]
	WHERE BookID LIKE '%' + @isbn + '%' AND Active = 1
END
GO

/* sp select book by title */
CREATE PROCEDURE sp_select_book_by_title (
	@title [varchar](50)
)
AS
BEGIN
	SELECT BookID, PublisherID, AuthorID, Title, Pages, Copies, Active
	FROM [dbo].[Book]
	WHERE Title LIKE '%' + @title + '%' COLLATE SQL_Latin1_General_CP1_CI_AS AND Active = 1
END
GO

/* sp select author by authorID */
CREATE PROCEDURE sp_select_author_by_authorid (
	@authorID [int]
)
AS
BEGIN
	SELECT AuthorID, FirstName, LastName, Active
	FROM [dbo].[Author]
	WHERE AuthorID = @authorID
END
GO

/* sp select publisher by publisherID */
CREATE PROCEDURE sp_select_publisher_by_publisherid (
	@publisherID [varchar](50)
)
AS
BEGIN
	SELECT PublisherID, Phone, Active
	FROM [dbo].[Publisher]
	WHERE PublisherID = @publisherID
END
GO

/* sp select rental by customer */
CREATE PROCEDURE sp_select_rental_by_customer (
	@customerID [int]
)
AS
BEGIN
	SELECT RentalID, CustomerID, DateRented, DateDue, LateFee, Active
	FROM [dbo].[Rental]
	WHERE CustomerID = @customerID AND Active = 1
END
GO

/* sp select rental by customer book */
CREATE PROCEDURE sp_select_rental_by_customer_book (
	@customerID [int],
	@bookID 	[varchar](20)
)
AS
BEGIN
	SELECT Rental.RentalID, CustomerID, DateRented, DateDue, LateFee, Active
	FROM [dbo].[Rental], [dbo].[RentalLine]
	where Rental.RentalID = RentalLine.RentalID 
		AND RentalLine.BookID = @bookID 
		AND CustomerID = @customerID
END
GO

/* sp select orderdesc by orderid */
CREATE PROCEDURE sp_select_orderdesc_by_order (
	@orderID [int]
)
AS
BEGIN
	SELECT [Order].OrderID, [Order].CustomerID, [Order].DateOrdered, OrderLine.Quantity, Book.Title
	FROM [dbo].[Order], [dbo].[OrderLine], [dbo].[Book]
	WHERE [Order].OrderID = @orderID
		AND OrderLine.OrderID = @orderID 
		AND Book.BookID = OrderLine.BookID
END
GO

/* sp check bookID */
CREATE PROCEDURE sp_check_bookid (
	@bookID [varchar](20)
)
AS
BEGIN
	SELECT COUNT(BookID)
	FROM [dbo].[Book]
	WHERE BookID = @bookID
END
GO

/* sp check username */
CREATE PROCEDURE sp_check_username (
	@username [varchar](20)
)
AS
BEGIN
	SELECT COUNT(UserName)
	FROM [dbo].[Customer]
	WHERE UserName = @username
END
GO

/* sp check review */
CREATE PROCEDURE sp_check_review (
	@customerID [int],
	@bookID		[varchar](20)
)
AS
BEGIN
	SELECT COUNT(Review.RentalID)
	FROM [dbo].[Review], [dbo].[Rental], [dbo].[RentalLine]
	WHERE Review.RentalID = Rental.RentalID
		AND Rental.RentalID = RentalLine.RentalID
		AND RentalLine.BookID = @bookID
		AND Rental.CustomerID = @customerID
END
GO

/* sp update existing book */
CREATE PROCEDURE sp_update_book
	@bookID			[varchar](20),
	@publisherID	[varchar](50),
	@authorID		[int],
	@title			[varchar](50),
	@pages			[int],
	@copies			[int]
AS  
BEGIN 
	UPDATE [dbo].[Book]
	SET PublisherID = @publisherID, AuthorID = @authorID, Title = @title, Pages = @pages, Copies = @copies
	WHERE BookID = @bookID
END
GO

/* sp update existing author */
CREATE PROCEDURE sp_update_author
	@firstName	[varchar](50),
	@lastName	[varchar](50),
	@authorID	[int]
AS  
BEGIN 
	UPDATE [dbo].[Author]
	SET FirstName = @firstName, LastName = @lastName
	WHERE AuthorID = @authorID
END
GO

/* sp update existing publisher */
CREATE PROCEDURE sp_update_publisher
	@publisherID	[varchar](50),
	@phone			[varchar](10)
AS  
BEGIN 
	UPDATE [dbo].[Publisher]
	SET Phone = @phone
	WHERE PublisherID = @publisherID
END
GO

/* sp update existing review */
CREATE PROCEDURE sp_update_review
	@rentalID	[int],
	@bookID		[varchar](20),
	@content	[varchar](2000)
AS  
BEGIN 
	UPDATE [dbo].[Review]
	SET Content = @content
	WHERE RentalID = @rentalID
		AND BookID = @bookID
END
GO

/* sp delete active book */
CREATE PROCEDURE sp_delete_book
	@bookID			[varchar](20),
	@active			[bit]
AS  
BEGIN 
	UPDATE [dbo].[Book]
	SET Active = @active
	WHERE BookID = @bookID
END
GO

/* sp delete active author */
CREATE PROCEDURE sp_delete_author
	@authorID		[int],
	@active			[bit]
AS  
BEGIN 
	UPDATE [dbo].[Author]
	SET Active = @active
	WHERE AuthorID = @authorID
END
GO

/* sp delete active publisher */
CREATE PROCEDURE sp_delete_publisher
	@publisherID	[varchar](50),
	@active		[bit]
AS  
BEGIN 
	UPDATE [dbo].[Publisher]
	SET Active = @active
	WHERE PublisherID = @publisherID
END
GO

/* sp delete active rental */
CREATE PROCEDURE sp_delete_rental
	@bookID			[varchar](20),
	@customerID		[int]
AS  
BEGIN 
	UPDATE [dbo].[Rental]
	SET Active = 0
	FROM Rental, RentalLine
	WHERE Rental.CustomerID = @customerID
		AND Rental.RentalID = RentalLine.RentalID
		AND RentalLine.BookID = @bookID
		AND Rental.Active = 1
END
GO

/* sp delete active review */
CREATE PROCEDURE sp_delete_review
	@rentalID		[int],
	@bookID			[varchar](20),
	@active			[bit]
AS  
BEGIN 
	UPDATE [dbo].[Review]
	SET Active = @active
	WHERE RentalID = @rentalID
		AND BookID = @bookID
END
GO

/* sp validate customer is staff */
CREATE PROCEDURE sp_validate_customer_is_staff (
	@username varchar(20)
	)
	AS
	BEGIN
		SELECT COUNT(UserName)
		FROM Staff
		WHERE
			UserName = @username
	END
GO

/* sp validate customer username and password */
CREATE PROCEDURE sp_validate_customer_username_and_password (
	@username varchar(20),
	@password varchar(256)
	)
	AS
	BEGIN
		SELECT COUNT(UserName)
		FROM Customer
		WHERE
			UserName = @username
		AND Password = @password
		AND Active = 1
	END
GO

/* sp update customer password for username */
CREATE PROCEDURE sp_update_customer_password_for_username (
	@username varchar(20),
	@oldPassword varchar(256),
	@newPassword varchar(256)
	)
	AS
	BEGIN
		UPDATE Customer
			SET Password = @newPassword
		WHERE
			UserName = @username
		AND Password = @oldPassword
		AND Active = 1
		RETURN @@rowcount
	END
GO








