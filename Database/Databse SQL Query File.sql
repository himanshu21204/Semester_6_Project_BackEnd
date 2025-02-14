-- This Database Query For my Real Estate Management System

-- User Table: Stores user details with different user types (Buyer, Seller, Admin).
-- Property Table: Stores details of properties listed on the platform.
-- Images Table: Links property images to their respective properties.
-- Favorites Table: Tracks properties favorited by users.
-- Transaction Table: Records transaction details for properties.
-- ContactUs Table: Captures messages from the contact us form.
-- Appointment Table: Tracks property viewing appointments.

-- Notes => Store Procedure Formate
-- Select All : PR_LOC_TableName_SelectAll
-- Select By ID : PR_LOC_TableName_SelectByPK
-- Insert : PR_LOC_TableName_Insert
-- Update : PR_LOC_TableName_UpdateByPK
-- Delete : PR_LOC_TableName_DeleteByPK

-- 1.User Table
CREATE TABLE [User] (
    UserID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    UserName VARCHAR(50) NOT NULL UNIQUE,
    PhoneNumber INT NOT NULL UNIQUE,
    FirstName VARCHAR(25) NOT NULL,
    LastName VARCHAR(25) NOT NULL,
    Email VARCHAR(25) NOT NULL UNIQUE,
    Password VARCHAR(25) NOT NULL,
    Description VARCHAR(200) NULL,
    UserRole VARCHAR(20) NOT NULL CHECK (UserRole IN ('Seller', 'Buyer', 'Admin', 'Agent')),
    ProfilePhoto VARCHAR(100) NULL,
    Address VARCHAR(100) NULL,
    CreatedAt DATETIME NOT NULL,
    ModifiedAt DATETIME NULL,
    isActive BIT NOT NULL DEFAULT 1
);
ALTER TABLE [User]
ALTER COLUMN PhoneNumber VARCHAR(15) NOT NULL;
ALTER TABLE [User]
ALTER COLUMN Email VARCHAR(200) NOT NULL;
ALTER TABLE [User]
ALTER COLUMN PhoneNumber VARCHAR(15) NULL;

ALTER TABLE [User]
ALTER COLUMN [Password] VARCHAR(255) NULL;


-- 2.Property
CREATE TABLE Property (
    PropertyID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL FOREIGN KEY REFERENCES [User](UserID),
    PropertyTitle VARCHAR(50) NOT NULL,
    PropertyDescription VARCHAR(250) NOT NULL,
    PropertyPrice DECIMAL(10, 2) NOT NULL,
    PropertyAddress VARCHAR(100) NOT NULL,
    PropertySize DECIMAL(10, 3) NOT NULL,
    BedroomCount INT NOT NULL,
    BathroomCount INT NOT NULL,
    BuildYear DATETIME NOT NULL,
    PropertyType VARCHAR(100) NOT NULL,
    ParkingSpaces FLOAT NULL,
    CreatedAt DATETIME NOT NULL,
    ModifiedAt DATETIME NULL
);
ALTER TABLE Property
ADD TransactionType VARCHAR(250) NULL;
UPDATE Property
SET TransactionType='Buy';
ALTER TABLE Property
ADD Status VARCHAR(20) NOT NULL DEFAULT 'Available';


-- 3.Property Images
CREATE TABLE PropertyImages (
    ImageID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    PropertyID INT NOT NULL FOREIGN KEY REFERENCES Property(PropertyID),
    ImageURL VARCHAR(200) NOT NULL,
    UploadedAt DATETIME NOT NULL
);

-- 4.Favorites
CREATE TABLE Favorites (
    FavoriteID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL FOREIGN KEY REFERENCES [User](UserID),
    PropertyID INT NOT NULL FOREIGN KEY REFERENCES Property(PropertyID),
    CreatedAt DATETIME NOT NULL 
);

-- 5.Transactions

CREATE TABLE Transactions (
    TransactionID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    TotalTransactionAmount DECIMAL(10,2) NOT NULL,
    PaidAmount DECIMAL(10,2) NOT NULL DEFAULT 0,
    RemainingAmount DECIMAL(10,2) NOT NULL,
    TransactionDate DATETIME NOT NULL,
    PaymentType VARCHAR(25) NOT NULL,
    PaymentStatus VARCHAR(20) NOT NULL DEFAULT 'Pending',
    PaymentReferenceNumber VARCHAR(100) NULL,
    CashPaymentAmount DECIMAL(10,2) NULL,
    CardNumber VARCHAR(16) NULL,
    CardHolderName VARCHAR(100) NULL,
    CardExpiryDate CHAR(5) NULL,
    UPIID VARCHAR(100) NULL,
    SellerID INT NOT NULL,
    BuyerID INT NOT NULL,
    Status VARCHAR(20) NOT NULL DEFAULT 'Pending',
    TransactionType VARCHAR(50) NULL,
    TransactionDetail TEXT NULL,
    LastTransactionDate DATETIME NULL,
    PropertyID INT NOT NULL,
    CONSTRAINT FK_Transactions_Seller FOREIGN KEY (SellerID) REFERENCES [User](UserID),
    CONSTRAINT FK_Transactions_Buyer FOREIGN KEY (BuyerID) REFERENCES [User](UserID),
    CONSTRAINT FK_Transactions_Property FOREIGN KEY (PropertyID) REFERENCES Property(PropertyID)
);

CREATE TABLE Installments (
    InstallmentID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    TransactionID INT NOT NULL,
    InstallmentAmount DECIMAL(10,2) NOT NULL,
    InstallmentDate DATETIME NOT NULL,
    PaidAmount DECIMAL(10,2) NOT NULL DEFAULT 0,
    PaymentStatus VARCHAR(20) NOT NULL DEFAULT 'Pending',
    PaymentReferenceNumber VARCHAR(100) NULL,
    PaymentType VARCHAR(25) NOT NULL,
    LastPaymentDate DATETIME NULL,
    CashPaymentAmount DECIMAL(10,2) NULL,
    CardNumber VARCHAR(16) NULL,
    CardHolderName VARCHAR(100) NULL,
    CardExpiryDate CHAR(5) NULL,
    UPIID VARCHAR(100) NULL,
    CONSTRAINT FK_Installments_Transaction FOREIGN KEY (TransactionID) REFERENCES Transactions(TransactionID)
);


INSERT INTO Transactions (TotalTransactionAmount, PaidAmount, RemainingAmount, InstallmentAmount, InstallmentsCount, PaidInstallments, TransactionDate, PaymentType, PaymentStatus, PaymentReferenceNumber, CashPaymentAmount, CardNumber, CardHolderName, CardExpiryDate, UPIID, SellerID, BuyerID, Status, TransactionType, TransactionDetail, NextInstallmentDate) VALUES (100000.00, 20000.00, 80000.00, 20000.00, 4, 1, '2025-01-30 14:00:00', 'Cash', 'Completed', 'REF123456789', 20000.00, NULL, NULL, NULL, NULL, 6, 8, 'Completed', 'Sale', 'Full payment for Property XYZ', '2025-02-15 14:00:00');

-- 6.ContactUS
CREATE TABLE ContactUS (
    ContactID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(50) NOT NULL,
    Email VARCHAR(50) NOT NULL,
    PhoneNumber VARCHAR(20) NOT NULL,
    Subject VARCHAR(50) NULL,
    Message VARCHAR(500) NOT NULL,
    SubmittedAt DATETIME NOT NULL
);
ALTER TABLE ContactUS
ADD [Status] VARCHAR(25) NULL;
UPDATE ContactUS
SET [Status] = CASE
    WHEN contactID % 5 = 0 THEN 'Pending'
    WHEN contactID % 2 <> 0 THEN 'In Progress'
    WHEN contactID % 2 = 0 THEN 'Resolved'
END;

-- 7.Appointments
CREATE TABLE Appointments (
    AppointmentID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    BookerUserID INT NOT NULL FOREIGN KEY REFERENCES [User](UserID), -- Use for Buyer ID
    AppointmentUserID INT NOT NULL FOREIGN KEY REFERENCES [User](UserID), -- Use for Seller ID
    PropertyID INT NOT NULL FOREIGN KEY REFERENCES Property(PropertyID),
    AppointmentStartDate DATETIME NOT NULL,
    AppointmentEndDate DATETIME NOT NULL,
    Status VARCHAR(50) NOT NULL,
    Notes VARCHAR(250) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedAt DATETIME NULL DEFAULT GETDATE()
);


-- Select Query
SELECT * FROM [User]
SELECT * FROM Property
SELECT * FROM PropertyImages
SELECT * FROM Favorites
SELECT * FROM Transactions
SELECT * FROM ContactUS
SELECT * FROM Appointments

DELETE FROM Transactions
WHERE TransactionID = 5
-- Store Procedure For Required in Project

-- For User Table

-- 1.Select All User
GO
ALTER PROCEDURE [dbo].[PR_LOC_User_SelectAll]
AS
BEGIN
    SELECT 
        [dbo].[User].[UserID],
        [dbo].[User].[UserName],
        [dbo].[User].[PhoneNumber],
        [dbo].[User].[FirstName],
        [dbo].[User].[LastName],
        [dbo].[User].[Email],
		[dbo].[User].[Password],
        [dbo].[User].[Description],
        [dbo].[User].[UserRole],
        [dbo].[User].[ProfilePhoto],
        [dbo].[User].[Address],
        [dbo].[User].[CreatedAt],
        [dbo].[User].[ModifiedAt],
        [dbo].[User].[isActive]
    FROM 
        [dbo].[User]
    ORDER BY 
        [dbo].[User].[UserName]
END

-- 2.Select By ID
GO
ALTER PROCEDURE [dbo].[PR_LOC_User_SelectByPK]
    @UserID INT
AS
BEGIN
    SELECT 
        [dbo].[User].[UserID],
        [dbo].[User].[UserName],
        [dbo].[User].[PhoneNumber],
        [dbo].[User].[FirstName],
        [dbo].[User].[LastName],
        [dbo].[User].[Email],
		[dbo].[User].[Password],
        [dbo].[User].[Description],
        [dbo].[User].[UserRole],
        [dbo].[User].[ProfilePhoto],
        [dbo].[User].[Address],
        [dbo].[User].[CreatedAt],
        [dbo].[User].[ModifiedAt],
        [dbo].[User].[isActive]
    FROM 
        [dbo].[User]
    WHERE 
        [dbo].[User].[UserID] = @UserID
END

-- 3.Insert User
GO
ALTER PROCEDURE [dbo].[PR_LOC_User_Insert]
    @UserName VARCHAR(50),
    @PhoneNumber VARCHAR(15),
    @FirstName VARCHAR(25),
    @LastName VARCHAR(25),
    @Email VARCHAR(25),
    @Password VARCHAR(25),
    @Description VARCHAR(200) = NULL,
    @UserRole VARCHAR(20),
    @ProfilePhoto VARCHAR(100) = NULL,
    @Address VARCHAR(100) = NULL
AS
BEGIN
    INSERT INTO [dbo].[User]
    (
        [UserName],
        [PhoneNumber],
        [FirstName],
        [LastName],
        [Email],
        [Password],
        [Description],
        [UserRole],
        [ProfilePhoto],
        [Address],
        [CreatedAt],
        [isActive]
    )
    VALUES
    (
        @UserName,
        @PhoneNumber,
        @FirstName,
        @LastName,
        @Email,
        @Password,
        @Description,
        @UserRole,
        @ProfilePhoto,
        @Address,
        GETDATE(),
        1
    )
END

-- 4.Update User
GO
ALTER PROCEDURE [dbo].[PR_LOC_User_UpdateByPK]
    @UserID INT,
    @UserName VARCHAR(50),
    @PhoneNumber VARCHAR(15),
    @FirstName VARCHAR(25),
    @LastName VARCHAR(25),
    @Email VARCHAR(25),
    @Password VARCHAR(25),
    @Description VARCHAR(200),
    @UserRole VARCHAR(20),
    @ProfilePhoto VARCHAR(100),
    @Address VARCHAR(100),
    @isActive BIT
AS
BEGIN
    UPDATE [dbo].[User]
    SET 
        [UserName] = @UserName,
        [PhoneNumber] = @PhoneNumber,
        [FirstName] = @FirstName,
        [LastName] = @LastName,
        [Email] = @Email,
        [Password] = @Password,
        [Description] = @Description,
        [UserRole] = @UserRole,
        [ProfilePhoto] = @ProfilePhoto,
        [Address] = @Address,
        [isActive] = @isActive,
        [ModifiedAt] = GETDATE()
    WHERE 
        [dbo].[User].[UserID] = @UserID
END

-- 5.Delete User
GO
CREATE PROCEDURE [dbo].[PR_LOC_User_DeleteByPK]
    @UserID INT
AS 
BEGIN
    DELETE FROM [dbo].[User]
    WHERE 
        [dbo].[User].[UserID] = @UserID
END

INSERT INTO [dbo].[User]
(
    [UserName],
    [PhoneNumber],
    [FirstName],
    [LastName],
    [Email],
	[Password],
    [Description],
    [UserRole],
    [ProfilePhoto],
    [Address],
    [CreatedAt],
    [ModifiedAt],
    [isActive]
)
VALUES
(
    'himanshu21204', 
    '1234567891', 
    'Himanshu', 
    'Parmar', 
    'hkp21204@gmail.com', 
	'21204PHK',
    'I am Himanshu Parmar', 
    'Admin', 
    NULL, 
    'Any', 
    GETDATE(), 
    NULL, 
    1 -- Assuming 1 means active
);

-- Deactivate
GO
CREATE PROCEDURE [dbo].[PR_LOC_User_DeactivateByPK]
    @UserID INT
AS
BEGIN
    UPDATE [User]
    SET isActive = 0
    WHERE UserID = @UserID;
END

-- Update Profile Photo
ALTER PROCEDURE [dbo].[PR_LOC_Property_UpdateUserProfilePhoto]
    @UserID INT,
    @ProfilePhoto NVARCHAR(MAX)
AS
BEGIN
    UPDATE [User]
    SET [User].ProfilePhoto = @ProfilePhoto
    WHERE [User].UserID = @UserID;
END
-- Get User Photo Ny ID
CREATE PROCEDURE [dbo].[PR_LOC_User_UserProfilePhotoByID]
	@UserID INT
AS
BEGIN
	SELECT [dbo].[User].[ProfilePhoto] FROM [User]
	WHERE [dbo].[User].[UserID] = @UserID
END

-- 1. Select All
GO
ALTER PROCEDURE [dbo].[PR_LOC_Property_SelectAll]
AS
BEGIN
    SELECT 
        [dbo].[Property].[PropertyID],
        [dbo].[Property].[UserID],
        [dbo].[User].[UserName],
		[dbo].[User].[ProfilePhoto],
        [dbo].[Property].[PropertyTitle],
        [dbo].[Property].[PropertyDescription],
        [dbo].[Property].[PropertyPrice],
        [dbo].[Property].[PropertyAddress],
        [dbo].[Property].[PropertySize],
        [dbo].[Property].[BedroomCount],
        [dbo].[Property].[BathroomCount],
        [dbo].[Property].[BuildYear],
        [dbo].[Property].[PropertyType],
        [dbo].[Property].[ParkingSpaces],
		[dbo].[Property].[TransactionType],
        [dbo].[Property].[AdditionalFeatures],
		[dbo].[Property].[Status],
        [dbo].[Property].[CreatedAt],
        [dbo].[Property].[ModifiedAt]
    FROM 
        [dbo].[Property]
    INNER JOIN 
        [dbo].[User] ON [dbo].[Property].[UserID] = [dbo].[User].[UserID]
    ORDER BY 
        [dbo].[Property].[PropertyTitle]
END

-- 2. Select By ID
GO
ALTER PROCEDURE [dbo].[PR_LOC_Property_SelectByPK]
    @PropertyID INT
AS
BEGIN
    SELECT 
        [dbo].[Property].[PropertyID],
        [dbo].[Property].[UserID],
        [dbo].[User].[UserName],
		[dbo].[User].[ProfilePhoto],
        [dbo].[Property].[PropertyTitle],
        [dbo].[Property].[PropertyDescription],
        [dbo].[Property].[PropertyPrice],
        [dbo].[Property].[PropertyAddress],
        [dbo].[Property].[PropertySize],
        [dbo].[Property].[BedroomCount],
        [dbo].[Property].[BathroomCount],
        [dbo].[Property].[BuildYear],
        [dbo].[Property].[PropertyType],
		[dbo].[Property].[TransactionType],
        [dbo].[Property].[ParkingSpaces],
        [dbo].[Property].[AdditionalFeatures],
		[dbo].[Property].[Status],
        [dbo].[Property].[CreatedAt],
        [dbo].[Property].[ModifiedAt]
    FROM 
        [dbo].[Property]
    INNER JOIN 
        [dbo].[User] ON [dbo].[Property].[UserID] = [dbo].[User].[UserID]
    WHERE 
        [dbo].[Property].[PropertyID] = @PropertyID
END

-- 3. Insert
GO
ALTER PROCEDURE [dbo].[PR_LOC_Property_Insert]
    @UserID INT,
    @PropertyTitle VARCHAR(50),
    @PropertyDescription VARCHAR(250),
    @PropertyPrice DECIMAL(10, 2),
    @PropertyAddress VARCHAR(100),
    @PropertySize DECIMAL(10, 3),
    @BedroomCount INT,
    @BathroomCount INT,
    @BuildYear DATETIME,
    @PropertyType VARCHAR(100),
	@TransactionType VARCHAR(100),
    @ParkingSpaces FLOAT = NULL,
    @AdditionalFeatures VARCHAR(250) = NULL,
	@Status VARCHAR(20),
    @InsertedPropertyID INT OUTPUT -- Output parameter for the newly inserted PropertyID
AS
BEGIN
    -- Insert the property data
    INSERT INTO [dbo].[Property]
    (
        [dbo].[Property].[UserID],
        [dbo].[Property].[PropertyTitle],
        [dbo].[Property].[PropertyDescription],
        [dbo].[Property].[PropertyPrice],
        [dbo].[Property].[PropertyAddress],
        [dbo].[Property].[PropertySize],
        [dbo].[Property].[BedroomCount],
        [dbo].[Property].[BathroomCount],
        [dbo].[Property].[BuildYear],
        [dbo].[Property].[PropertyType],
		[dbo].[Property].[TransactionType],
        [dbo].[Property].[ParkingSpaces],
        [dbo].[Property].[AdditionalFeatures],
		[dbo].[Property].[Status],
        [dbo].[Property].[CreatedAt]
    )
    VALUES
    (
        @UserID,
        @PropertyTitle,
        @PropertyDescription,
        @PropertyPrice,
        @PropertyAddress,
        @PropertySize,
        @BedroomCount,
        @BathroomCount,
        @BuildYear,
        @PropertyType,
		@TransactionType,
        @ParkingSpaces,
        @AdditionalFeatures,
		@Status,
        GETDATE()
    )

    -- Retrieve the newly inserted PropertyID and assign it to the output parameter
    SET @InsertedPropertyID = SCOPE_IDENTITY()
END

-- 4. Update
GO
ALTER PROCEDURE [dbo].[PR_LOC_Property_UpdateByPK]
    @PropertyID INT,
    @UserID INT,
    @PropertyTitle VARCHAR(50),
    @PropertyDescription VARCHAR(250),
    @PropertyPrice DECIMAL(10, 2),
    @PropertyAddress VARCHAR(100),
    @PropertySize DECIMAL(10, 3),
    @BedroomCount INT,
    @BathroomCount INT,
    @BuildYear DATETIME,
    @PropertyType VARCHAR(100),
	@TransactionType VARCHAR(100),
    @ParkingSpaces FLOAT = NULL,
    @AdditionalFeatures VARCHAR(250) = NULL,
	@Status VARCHAR(20)
AS
BEGIN
    UPDATE [dbo].[Property]
    SET 
        [dbo].[Property].[UserID] = @UserID,
        [dbo].[Property].[PropertyTitle] = @PropertyTitle,
        [dbo].[Property].[PropertyDescription] = @PropertyDescription,
        [dbo].[Property].[PropertyPrice] = @PropertyPrice,
        [dbo].[Property].[PropertyAddress] = @PropertyAddress,
        [dbo].[Property].[PropertySize] = @PropertySize,
        [dbo].[Property].[BedroomCount] = @BedroomCount,
        [dbo].[Property].[BathroomCount] = @BathroomCount,
        [dbo].[Property].[BuildYear] = @BuildYear,
        [dbo].[Property].[PropertyType] = @PropertyType,
		[dbo].[Property].[TransactionType] = @TransactionType,
        [dbo].[Property].[ParkingSpaces] = @ParkingSpaces,
        [dbo].[Property].[AdditionalFeatures] = @AdditionalFeatures,
		[dbo].[Property].[Status] = @Status,
        [dbo].[Property].[ModifiedAt] = GETDATE()
    WHERE 
        [dbo].[Property].[PropertyID] = @PropertyID

END

-- 5. Delete
GO
ALTER PROCEDURE [dbo].[PR_LOC_Property_DeleteByPK]
    @PropertyID INT
AS 
BEGIN
    DELETE FROM [dbo].[Property]
    WHERE 
        [dbo].[Property].[PropertyID] = @PropertyID
END

-- 6.Select By UserID
GO
ALTER PROCEDURE [dbo].[PR_LOC_Property_SelectByUserID]
    @UserID INT
AS
BEGIN
    SELECT 
        [dbo].[Property].[PropertyID],
        [dbo].[Property].[UserID],
        [dbo].[User].[UserName],
        [dbo].[Property].[PropertyTitle],
        [dbo].[Property].[PropertyDescription],
        [dbo].[Property].[PropertyPrice],
        [dbo].[Property].[PropertyAddress],
        [dbo].[Property].[PropertySize],
        [dbo].[Property].[BedroomCount],
        [dbo].[Property].[BathroomCount],
        [dbo].[Property].[BuildYear],
        [dbo].[Property].[PropertyType],
        [dbo].[Property].[ParkingSpaces],
		[dbo].[Property].[Status],
        [dbo].[Property].[CreatedAt],
        [dbo].[Property].[ModifiedAt]
    FROM 
        [dbo].[Property]
    INNER JOIN 
        [dbo].[User] ON [dbo].[Property].[UserID] = [dbo].[User].[UserID]
    WHERE 
        [dbo].[Property].[UserID] = @UserID
    ORDER BY 
        [dbo].[Property].[PropertyTitle]
END


INSERT INTO [dbo].[Property]
(
    [UserID],
    [PropertyTitle],
    [PropertyDescription],
    [PropertyPrice],
    [PropertyAddress],
    [PropertySize],
    [BedroomCount],
    [BathroomCount],
    [BuildYear],
    [PropertyType],
    [ParkingSpaces],
    [AdditionalFeatures],
    [CreatedAt]
)
VALUES
-- Dummy Record 1
(2, 
 'Luxury Apartment in Downtown', 
 'A spacious 3-bedroom apartment with a modern design located in the heart of the city.', 
 750000.00, 
 '123 Main Street, Downtown City', 
 200.50, 
 3, 
 2, 
 '2015-01-01', 
 'Apartment', 
 1.0, 
 'Swimming Pool;Terrace;Central Heating;Security System', 
 GETDATE()),

-- Dummy Record 2
(2, 
 'Cozy Family Home', 
 'A charming family home with a big garden and a cozy fireplace.', 
 450000.00, 
 '456 Elm Avenue, Suburbia', 
 150.75, 
 4, 
 3, 
 '2010-06-15', 
 'House', 
 2.0, 
 'Balcony;Roof Terrace;Cable TV;Air Conditioning', 
 GETDATE()),

-- Dummy Record 3
(2, 
 'Modern Studio Apartment', 
 'A compact yet stylish studio apartment perfect for young professionals.', 
 250000.00, 
 '789 Market Lane, Uptown', 
 50.30, 
 1, 
 1, 
 '2020-03-20', 
 'Studio Apartment', 
 NULL, 
 'Oven;Towels;Parking;Security System', 
 GETDATE());


-- For Property Images Table

-- 1.Insert
GO
CREATE PROCEDURE [dbo].[PR_LOC_PropertyImage_Insert]
    @PropertyID INT,
    @ImageURL VARCHAR(200)
AS
BEGIN
    INSERT INTO [dbo].[PropertyImages]
    (
        [PropertyID],
        [ImageURL],
        [UploadedAt]
    )
    VALUES
    (
        @PropertyID,
        @ImageURL,
        GETDATE()
    )
END

-- 2.Select By Propery ID
GO
CREATE PROCEDURE [dbo].[PR_LOC_PropertyImage_GetByPropertyID]
    @PropertyID INT
AS
BEGIN
    SELECT 
        [dbo].[PropertyImages].[ImageID],
        [dbo].[PropertyImages].[PropertyID],
        [dbo].[PropertyImages].[ImageURL],
        [dbo].[PropertyImages].[UploadedAt]
    FROM 
        [dbo].[PropertyImages]
    WHERE 
        [dbo].[PropertyImages].[PropertyID] = @PropertyID
    ORDER BY 
        [dbo].[PropertyImages].[UploadedAt] DESC
END

-- 3.Delete using ImageID
GO
CREATE PROCEDURE [dbo].[PR_LOC_PropertyImage_DeleteByPK]
    @ImageID INT
AS
BEGIN
    DELETE FROM [dbo].[PropertyImages]
    WHERE 
        [dbo].[PropertyImages].[ImageID] = @ImageID
END

-- 4.Delete using PropertyID
GO
CREATE PROCEDURE [dbo].[PR_LOC_PropertyImage_DeleteByPropertyID]
    @PropertyID INT
AS
BEGIN
    DELETE FROM [dbo].[PropertyImages]
    WHERE 
        [dbo].[PropertyImages].[PropertyID] = @PropertyID
END

-- 5.Update
GO
CREATE PROCEDURE [dbo].[PR_LOC_PropertyImage_Update]
    @PropertyID INT,
    @ImageURL VARCHAR(200),
    @ImageID INT
AS
BEGIN
    UPDATE [dbo].[PropertyImages]
    SET 
        [PropertyID] = @PropertyID,
        [ImageURL] = @ImageURL,
        [UploadedAt] = GETDATE()
    WHERE 
        [ImageID] = @ImageID;
END

-- For Favorite Table

-- 1.Add(Insert)
GO
CREATE PROCEDURE [dbo].[PR_LOC_Favorites_Add]
    @UserID INT,
    @PropertyID INT
AS
BEGIN
    INSERT INTO [dbo].[Favorites]
    (
        [UserID],
        [PropertyID],
        [CreatedAt]
    )
    VALUES
    (
        @UserID,
        @PropertyID,
        GETDATE()
    )
END

-- 2.Delete(Remove)
GO
CREATE PROCEDURE [dbo].[PR_LOC_Favorites_Remove]
    @FavoriteID INT
AS
BEGIN
    DELETE FROM [dbo].[Favorites]
    WHERE 
        [dbo].[Favorites].[FavoriteID] = @FavoriteID
END

-- 3.select by UserID
GO
CREATE PROCEDURE [dbo].[PR_LOC_Favorites_GetByUser] 6
    @UserID INT
AS
BEGIN
    SELECT 
        [dbo].[Favorites].[FavoriteID],
        [dbo].[Favorites].[UserID],
        [dbo].[Favorites].[PropertyID],
        [dbo].[Favorites].[CreatedAt]
    FROM 
        [dbo].[Favorites]
    WHERE 
        [dbo].[Favorites].[UserID] = @UserID
    ORDER BY 
        [dbo].[Favorites].[CreatedAt] DESC
END

ALTER PROCEDURE [dbo].[PR_LOC_Properties_ByUser] 
    @UserID INT
AS
BEGIN
        SELECT 
            [dbo].[Favorites].[FavoriteID],
            [dbo].[Favorites].[UserID],
            [dbo].[Favorites].[PropertyID],
            [dbo].[Favorites].[CreatedAt],
			[dbo].[User].[UserName],
			[dbo].[User].[ProfilePhoto],
            [dbo].[Property].[PropertyTitle],
            [dbo].[Property].[PropertyDescription],
            [dbo].[Property].[PropertyPrice],
            [dbo].[Property].[PropertyAddress],
            [dbo].[Property].[PropertySize],
            [dbo].[Property].[BedroomCount],
            [dbo].[Property].[BathroomCount],
            [dbo].[Property].[BuildYear],
            [dbo].[Property].[PropertyType],
            [dbo].[Property].[TransactionType],
            [dbo].[Property].[ParkingSpaces],
            [dbo].[Property].[AdditionalFeatures]
        FROM 
            [dbo].[Favorites]
        INNER JOIN 
            [dbo].[Property] ON [dbo].[Favorites].[PropertyID] = [dbo].[Property].[PropertyID]
		INNER JOIN 
			[dbo].[User] ON [dbo].[Favorites].[UserID] = [dbo].[User].[UserID]
        WHERE 
            [dbo].[Favorites].[UserID] = @UserID
        ORDER BY 
            [dbo].[Favorites].[CreatedAt] DESC
END

-- For ContactUS Table

-- 1.Insert
GO
ALTER PROCEDURE [dbo].[PR_LOC_ContactUS_Insert]
    @Name VARCHAR(50),
    @Email VARCHAR(50),
    @PhoneNumber VARCHAR(20),
    @Subject VARCHAR(50) = NULL,
    @Message VARCHAR(500),
    @SubmittedAt DATETIME = NULL
AS
BEGIN
    INSERT INTO [dbo].[ContactUS]
    (
        [Name],
        [Email],
        [PhoneNumber],
        [Subject],
        [Message],
		[Status],
        [SubmittedAt]
    )
    VALUES
    (
        @Name,
        @Email,
        @PhoneNumber,
        @Subject,
        @Message,
		'Pending',
        ISNULL(@SubmittedAt, GETDATE())
    )
END

-- 2.Get All 
GO
ALTER PROCEDURE [dbo].[PR_LOC_ContactUS_SelectAll]
AS
BEGIN
    SELECT 
        [dbo].[ContactUS].[ContactID],
        [dbo].[ContactUS].[Name],
        [dbo].[ContactUS].[Email],
        [dbo].[ContactUS].[PhoneNumber],
        [dbo].[ContactUS].[Subject],
        [dbo].[ContactUS].[Message],
		[DBO].[ContactUS].[Status],
        [dbo].[ContactUS].[SubmittedAt]
    FROM 
        [dbo].[ContactUS]
    ORDER BY 
        [dbo].[ContactUS].[SubmittedAt] DESC
END

-- 3.Get By ID
GO
ALTER PROCEDURE [dbo].[PR_LOC_ContactUS_SelectByPK]
    @ContactID INT
AS
BEGIN
    SELECT 
        [dbo].[ContactUS].[ContactID],
        [dbo].[ContactUS].[Name],
        [dbo].[ContactUS].[Email],
        [dbo].[ContactUS].[PhoneNumber],
        [dbo].[ContactUS].[Subject],
        [dbo].[ContactUS].[Message],
		[DBO].[ContactUS].[Status],
        [dbo].[ContactUS].[SubmittedAt]
    FROM 
        [dbo].[ContactUS]
    WHERE 
        [dbo].[ContactUS].[ContactID] = @ContactID
END

-- Update Contact US Status
GO
CREATE PROCEDURE [dbo].[PR_LOC_ContactUS_UpdateStatus]
	@ContactID INT,
	@Status VARCHAR(25)
AS
BEGIN
    UPDATE [dbo].[ContactUS]
	SET [Status] = @Status
	WHERE ContactID = @ContactID
END
-- For Appoinment Table

-- 1.Appoinment Schedule
GO
ALTER PROCEDURE [dbo].[PR_LOC_Appointment_Schedule]
    @BookerUserID INT,
    @AppointmentUserID INT,
    @PropertyID INT,
    @AppointmentStartDate DATETIME,
    @AppointmentEndDate DATETIME,
    @Status VARCHAR(50),
    @Notes VARCHAR(250) = NULL
AS
BEGIN
    INSERT INTO [dbo].[Appointments]
    (
        [BookerUserID],
        [AppointmentUserID],
        [PropertyID],
        [AppointmentStartDate],
        [AppointmentEndDate],
        [Status],
        [Notes],
        [CreatedAt]
    )
    VALUES
    (
        @BookerUserID,
        @AppointmentUserID,
        @PropertyID,
        @AppointmentStartDate,
        @AppointmentEndDate,
        @Status,
        @Notes,
        GETDATE()
    )
END


-- 2.Update Status
GO
ALTER PROCEDURE [dbo].[PR_LOC_Appointment_UpdateStatus]
    @AppointmentID INT,
    @Status VARCHAR(50)
AS
BEGIN
    UPDATE [dbo].[Appointments]
    SET 
        [Status] = @Status,
        [ModifiedAt] = GETDATE()
    WHERE 
        [AppointmentID] = @AppointmentID;
END

-- 3.Get by user ID
GO
ALTER PROCEDURE [dbo].[PR_LOC_Appointment_GetByUser]
    @BookerUserID INT
AS
BEGIN
    SELECT 
        [Appointments].[AppointmentID],
        [Appointments].[BookerUserID],
        Booker.[FirstName] + ' ' + Booker.[LastName] AS BookerName,
        [Appointments].[AppointmentUserID],
        AppointmentUser.[FirstName] + ' ' + AppointmentUser.[LastName] AS AppointmentUserName,
        [Appointments].[PropertyID],
        Property.[PropertyTitle],
        [Appointments].[AppointmentStartDate],
        [Appointments].[AppointmentEndDate],
        [Appointments].[Status],
        [Appointments].[Notes],
        [Appointments].[CreatedAt],
        [Appointments].[ModifiedAt]
    FROM 
        [dbo].[Appointments]
    INNER JOIN
        [dbo].[User] AS Booker ON [Appointments].[BookerUserID] = Booker.[UserID]
    INNER JOIN
        [dbo].[User] AS AppointmentUser ON [Appointments].[AppointmentUserID] = AppointmentUser.[UserID]
    INNER JOIN
        [dbo].[Property] ON [Appointments].[PropertyID] = [Property].[PropertyID]
    WHERE 
        [Appointments].[BookerUserID] = @BookerUserID
    ORDER BY 
        [Appointments].[AppointmentStartDate];
END

-- 4.Get by Property ID
GO
ALTER PROCEDURE [dbo].[PR_LOC_Appointment_GetByProperty]
    @PropertyID INT
AS
BEGIN
    SELECT 
        [Appointments].[AppointmentID],
        [Appointments].[BookerUserID],
        Booker.[FirstName] + ' ' + Booker.[LastName] AS BookerName,
        [Appointments].[AppointmentUserID],
        AppointmentUser.[FirstName] + ' ' + AppointmentUser.[LastName] AS AppointmentUserName,
        [Appointments].[PropertyID],
        [Appointments].[AppointmentStartDate],
        [Appointments].[AppointmentEndDate],
        [Appointments].[Status],
        [Appointments].[Notes],
        [Appointments].[CreatedAt],
        [Appointments].[ModifiedAt]
    FROM 
        [dbo].[Appointments]
    INNER JOIN
        [dbo].[User] AS Booker ON [Appointments].[BookerUserID] = Booker.[UserID]
    INNER JOIN
        [dbo].[User] AS AppointmentUser ON [Appointments].[AppointmentUserID] = AppointmentUser.[UserID]
    INNER JOIN
        [dbo].[Property] ON [Appointments].[PropertyID] = [Property].[PropertyID]
    WHERE 
        [Appointments].[PropertyID] = @PropertyID
    ORDER BY 
        [Appointments].[AppointmentStartDate];
END

-- 5.Get by status
GO
ALTER PROCEDURE [dbo].[PR_LOC_Appointment_GetByStatus]
    @Status VARCHAR(50)
AS
BEGIN
    SELECT 
        [Appointments].[AppointmentID],
        [Appointments].[BookerUserID],
        Booker.[FirstName] + ' ' + Booker.[LastName] AS BookerName,
        [Appointments].[AppointmentUserID],
        AppointmentUser.[FirstName] + ' ' + AppointmentUser.[LastName] AS AppointmentUserName,
        [Appointments].[PropertyID],
        [Appointments].[AppointmentStartDate],
        [Appointments].[AppointmentEndDate],
        [Appointments].[Status],
        [Appointments].[Notes],
        [Appointments].[CreatedAt],
        [Appointments].[ModifiedAt]
    FROM 
        [dbo].[Appointments]
    INNER JOIN
        [dbo].[User] AS Booker ON [Appointments].[BookerUserID] = Booker.[UserID]
    INNER JOIN
        [dbo].[User] AS AppointmentUser ON [Appointments].[AppointmentUserID] = AppointmentUser.[UserID]
    WHERE 
        [Appointments].[Status] = @Status
    ORDER BY 
        [Appointments].[AppointmentStartDate];
END

-- Agent/Seller Drop Down
ALTER PROCEDURE [dbo].[PR_LOC_AgentSeller_Dropdown]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        [UserID],
        [FirstName] + ' ' + [LastName] AS FullName
    FROM 
        [dbo].[User]
    WHERE 
        UserRole IN ('Agent', 'Seller','Admin')
    ORDER BY 
        [UserName];
END

-- Property by USer ID
CREATE PROCEDURE [dbo].[PR_LOC_PropertyDropdown_ByUserID]
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        [PropertyID],
        [PropertyTitle]
    FROM 
        [dbo].[Property]
    WHERE 
        [UserID] = @UserID
    ORDER BY 
        [PropertyTitle];
END


-- Status State : ['Scheduled', 'Completed', 'Cancelled', 'Rescheduled', 'Pending']

-- Insert image twice for PropertyID 2
INSERT INTO PropertyImages (PropertyID, ImageURL, UploadedAt)
VALUES (2, 'https://ibb.co/hg066mg', GETDATE());

INSERT INTO PropertyImages (PropertyID, ImageURL, UploadedAt)
VALUES (2, 'https://ibb.co/hg066mg', GETDATE());

-- Insert image twice for PropertyID 3
INSERT INTO PropertyImages (PropertyID, ImageURL, UploadedAt)
VALUES (3, 'https://ibb.co/hg066mg', GETDATE());

INSERT INTO PropertyImages (PropertyID, ImageURL, UploadedAt)
VALUES (3, 'https://ibb.co/hg066mg', GETDATE());

-- Insert image twice for PropertyID 4
INSERT INTO PropertyImages (PropertyID, ImageURL, UploadedAt)
VALUES (4, 'https://ibb.co/hg066mg', GETDATE());

INSERT INTO PropertyImages (PropertyID, ImageURL, UploadedAt)
VALUES (4, 'https://ibb.co/hg066mg', GETDATE());

UPDATE PropertyImages
SET ImageURL = 'https://i.ibb.co/j8tNNr8/pexels-photo-731082-jpeg-cs-srgb-dl-pexels-sebastians-731082.jpg'
WHERE ImageURL = 'https://ibb.co/hg066mg';


-- Login/Register SP

-- Stored Procedure to register a new user
GO
ALTER PROCEDURE [dbo].[PR_LOC_User_Register]
    @UserName VARCHAR(50), 
    @PhoneNumber VARCHAR(20),
    @FirstName VARCHAR(25),
    @LastName VARCHAR(25),
    @Email VARCHAR(50),
    @Password VARCHAR(255),
    @UserRole VARCHAR(20)
AS
BEGIN
    -- Check if username already exists
    IF EXISTS (SELECT 1 FROM [User] WHERE UserName = @UserName)
    BEGIN
        RAISERROR('Username already exists', 16, 1);
        RETURN;
    END

    -- Check if email already exists
    IF EXISTS (SELECT 1 FROM [User] WHERE Email = @Email)
    BEGIN
        RAISERROR('Email already exists', 16, 1);
        RETURN;
    END

    -- Insert new user into the User table
    INSERT INTO [User] 
        (UserName, PhoneNumber, FirstName, LastName, Email, Password, UserRole, CreatedAt)
    VALUES 
        (@UserName, @PhoneNumber, @FirstName, @LastName, @Email, @Password, @UserRole, GETDATE());
END

GO
CREATE PROCEDURE [dbo].[PR_LOC_User_RegisterUsingGoogle]
    @UserName VARCHAR(50),
    @FirstName VARCHAR(25),
    @LastName VARCHAR(25),
    @Email VARCHAR(50),
    @ProfilePhoto VARCHAR(255),
    @UserRole VARCHAR(20)
AS
BEGIN
    -- Check if username already exists
    IF EXISTS (SELECT 1 FROM [User] WHERE UserName = @UserName)
    BEGIN
        RAISERROR('Username already exists', 16, 1);
        RETURN;
    END

    -- Check if email already exists
    IF EXISTS (SELECT 1 FROM [User] WHERE Email = @Email)
    BEGIN
        RAISERROR('Email already exists', 16, 1);
        RETURN;
    END

    -- Insert new user into the User table
    INSERT INTO [User] 
        (UserName, ProfilePhoto, FirstName, LastName, Email, UserRole, CreatedAt)
    VALUES 
        (@UserName, @ProfilePhoto, @FirstName, @LastName, @Email, @UserRole, GETDATE());
END

-- Stored Procedure for User Login
GO
ALTER PROCEDURE [dbo].[PR_LOC_User_Login] 
    @UserName VARCHAR(50),
    @Password VARCHAR(255)
AS
BEGIN
    SELECT 
        [dbo].[User].[UserID],
        [dbo].[User].[UserName],
        [dbo].[User].[PhoneNumber],
        [dbo].[User].[FirstName],
        [dbo].[User].[LastName],
        [dbo].[User].[Email],
        [dbo].[User].[Description],
        [dbo].[User].[UserRole],
        [dbo].[User].[ProfilePhoto],
        [dbo].[User].[Address],
        [dbo].[User].[CreatedAt],
        [dbo].[User].[ModifiedAt],
        [dbo].[User].[isActive],
        [dbo].[User].[Password]
    FROM 
        [dbo].[User]
    WHERE 
        [dbo].[User].[UserName] = @UserName
		AND [dbo].[User].[Password] = @Password
        AND [dbo].[User].[isActive] = 1
END


INSERT INTO ContactUS (Name, Email, PhoneNumber, Subject, Message, SubmittedAt)
VALUES 
('John Doe', 'johndoe@example.com', '123-456-7890', 'Inquiry', 'I would like to know more about your services.', GETDATE()),
('Jane Smith', 'janesmith@example.com', '987-654-3210', 'Support', 'I need assistance with my account.', GETDATE()),
('Alice Brown', 'alicebrown@example.com', '555-123-4567', 'Feedback', 'Your website is very user-friendly.', GETDATE()),
('Bob Johnson', 'bobjohnson@example.com', '444-987-6543', 'Complaint', 'I am facing issues with the delivery.', GETDATE()),
('Emily Davis', 'emilydavis@example.com', '333-222-1111', 'Collaboration', 'I am interested in partnering with your company.', GETDATE()),
('Michael Wilson', 'michaelwilson@example.com', '222-333-4444', 'Request', 'Can I get a quote for your services?', GETDATE()),
('Sarah Taylor', 'sarahtaylor@example.com', '111-444-5555', 'Inquiry', 'What are your working hours?', GETDATE()),
('David White', 'davidwhite@example.com', '666-777-8888', 'Support', 'I forgot my account password.', GETDATE()),
('Laura Harris', 'lauraharris@example.com', '999-888-7777', 'Feedback', 'Great customer support! Keep it up.', GETDATE()),
('Chris Moore', 'chrismoore@example.com', '777-666-5555', 'Other', 'I have a question about your refund policy.', GETDATE());

SELECT * FROM Property

-- Dahsboard
GO
ALTER PROCEDURE [dbo].[usp_GetRealEstateSummaryData]
AS
BEGIN
    -- Enable NOCOUNT for better performance
    SET NOCOUNT ON;

    -- Temporary tables for organized data fetching
    CREATE TABLE #Counts (
        Metric NVARCHAR(255),
        Value INT
    );

    CREATE TABLE #RecentProperties (
        PropertyID INT,
        PropertyTitle NVARCHAR(255),
        TransactionType NVARCHAR(100),
        PropertyPrice DECIMAL(10, 2),
        CreatedAt DATETIME
    );

    CREATE TABLE #RecentAgents (
		AgentID INT,
        AgentName NVARCHAR(255),
        Email NVARCHAR(255),
        CreatedAt DATETIME,
        PropertiesListed INT,
    );

    ---- Step 1: Get Counts
    -- Count total properties
    INSERT INTO #Counts
    SELECT 'Total Properties', COUNT(*) FROM Property;

    -- Count properties for rent
    INSERT INTO #Counts
    SELECT 'Properties For Rent', COUNT(*) FROM Property WHERE TransactionType = 'Rent';

	-- Count properties for buy
    INSERT INTO #Counts
    SELECT 'Properties For Buy', COUNT(*) FROM Property WHERE TransactionType = 'BUy';

    -- Count properties for sale
    INSERT INTO #Counts
    SELECT 'Properties For Sale', COUNT(*) FROM Property WHERE TransactionType = 'Buy';

    -- Calculate total earnings from properties sold (Buy transaction type)
    INSERT INTO #Counts
    SELECT 'Total Earnings', SUM(PropertyPrice) FROM Property WHERE TransactionType = 'Buy';

    -- Count total bookmarked properties (assuming there's a Bookmarked table)
    INSERT INTO #Counts
    SELECT 'Total Bookmarked', COUNT(*) FROM Favorites WHERE UserID IS NOT NULL;

    -- Step 2: Get Recent Properties based on CreatedAt
    INSERT INTO #RecentProperties
    SELECT TOP 10
        PropertyID,
        PropertyTitle,
        TransactionType,
        PropertyPrice,
        CreatedAt
    FROM Property
    ORDER BY CreatedAt DESC;

    -- Step 3: Get Recent Agents based on CreatedAt (top 10 most recently added agents)
    INSERT INTO #RecentAgents
    SELECT TOP 10
	U.UserID AS AgentID,
    U.UserName AS AgentName,
    U.Email,
    U.CreatedAt,
    COUNT(P.PropertyID) AS PropertiesListed
FROM [User] U
LEFT JOIN Property P ON U.UserID = P.UserID
WHERE U.UserRole = 'Agent'
GROUP BY U.UserID,U.UserName, U.Email, U.CreatedAt
ORDER BY U.CreatedAt DESC;

    -- Output Results
    -- Output Counts
    SELECT * FROM #Counts;

    -- Output Recent Properties
    SELECT * FROM #RecentProperties;

    -- Output Recent Agents
    SELECT * FROM #RecentAgents;

    -- Cleanup Temporary Tables
    DROP TABLE #RecentProperties;
    DROP TABLE #RecentAgents;
    DROP TABLE #Counts;
END;


GO
ALTER PROCEDURE [dbo].[usp_GetRealEstateSummaryDataForAgent] 
    @UserID INT
AS
BEGIN
    -- Enable NOCOUNT for better performance
    SET NOCOUNT ON;

    -- Temporary tables for organized data fetching
    CREATE TABLE #Counts (
        Metric NVARCHAR(255),
        Value INT
    );

    CREATE TABLE #RecentProperties (
        PropertyID INT,
        PropertyTitle NVARCHAR(255),
        TransactionType NVARCHAR(100),
        PropertyPrice DECIMAL(10, 2),
        CreatedAt DATETIME,
        AgentID INT
    );

    CREATE TABLE #RecentAppointments (
        AppointmentID INT,
        BookerUserID INT,
        AppointmentUserID INT,
        PropertyID INT,
        AppointmentStartDate DATETIME,
        AppointmentEndDate DATETIME,
        Status VARCHAR(50),
        Notes VARCHAR(250),
        CreatedAt DATETIME
    );

    ---- Step 1: Get Counts based on Agent/Seller ID
    -- Count total properties for the user
    INSERT INTO #Counts
    SELECT 'Total Properties', COUNT(*) FROM Property WHERE UserID = @UserID;

    -- Count properties for rent
    INSERT INTO #Counts
    SELECT 'Properties For Rent', COUNT(*) FROM Property WHERE TransactionType = 'Rent' AND UserID = @USerID;

    -- Count properties for buy
    INSERT INTO #Counts
    SELECT 'Properties For Buy', COUNT(*) FROM Property WHERE TransactionType = 'Buy' AND UserID = @UserID;

    -- Count properties for sale
    INSERT INTO #Counts
    SELECT 'Properties For Sale', COUNT(*) FROM Property WHERE TransactionType = 'Sale' AND UserID = @UserID;

    -- Calculate total earnings from properties sold (Buy transaction type)
    INSERT INTO #Counts
	SELECT 'Total Earnings', ISNULL(SUM(PropertyPrice), 0)
FROM Property 
WHERE [Status] IN ('Sold', 'Rented') 
AND UserID = @UserID;


    -- Step 2: Get Recent Properties for the user
    INSERT INTO #RecentProperties
    SELECT TOP 10
        P.PropertyID,
        P.PropertyTitle,
        P.TransactionType,
        P.PropertyPrice,
        P.CreatedAt,
        P.UserID AS AgentID
    FROM Property P
    WHERE P.UserID = @UserID
    ORDER BY P.CreatedAt DESC;

    -- Step 3: Get Recent Appointments for the user
    INSERT INTO #RecentAppointments
    SELECT TOP 10
        A.AppointmentID,
        A.BookerUserID,
        A.AppointmentUserID,
        A.PropertyID,
        A.AppointmentStartDate,
        A.AppointmentEndDate,
        A.[Status],
        A.Notes,
        A.CreatedAt
    FROM Appointments A
    WHERE A.AppointmentUserID = @UserID
    ORDER BY A.CreatedAt DESC;

    -- Output Results
    -- Output Counts
    SELECT * FROM #Counts;

    -- Output Recent Properties
    SELECT * FROM #RecentProperties;

    -- Output Recent Appointments
    SELECT * FROM #RecentAppointments;

    -- Cleanup Temporary Tables
    DROP TABLE #RecentProperties;
    DROP TABLE #RecentAppointments;
    DROP TABLE #Counts;
END;


-- For Transaction Table

-- 1.Retrieve All Transactions:
GO
ALTER PROCEDURE [dbo].[PR_TRANS_Transaction_SelectAll]
AS
BEGIN
    SELECT 
        [dbo].[Transactions].[TransactionID],
        [dbo].[Transactions].[TotalTransactionAmount],
        [dbo].[Transactions].[PaidAmount],
        [dbo].[Transactions].[RemainingAmount],
        [dbo].[Transactions].[TransactionDate],
        [dbo].[Transactions].[PaymentType],
        [dbo].[Transactions].[PaymentStatus],
        [dbo].[Transactions].[PaymentReferenceNumber],
        [dbo].[Transactions].[CashPaymentAmount],
        [dbo].[Transactions].[CardNumber],
        [dbo].[Transactions].[CardHolderName],
        [dbo].[Transactions].[CardExpiryDate],
        [dbo].[Transactions].[UPIID],
        [dbo].[Transactions].[SellerID],
        Seller.[FirstName] + ' ' + Seller.[LastName] AS SellerName,
        [dbo].[Transactions].[BuyerID],
        Buyer.[FirstName] + ' ' + Buyer.[LastName] AS BuyerName,
        [dbo].[Transactions].[Status],
        [dbo].[Transactions].[TransactionType],
        [dbo].[Transactions].[TransactionDetail],
        [dbo].[Transactions].[LastTransactionDate],
        [dbo].[Transactions].[PropertyID],
        [dbo].[Property].[PropertyTitle]
    FROM 
        [dbo].[Transactions]
    INNER JOIN
        [dbo].[User] AS Buyer ON [Transactions].[BuyerID] = Buyer.[UserID]
    INNER JOIN
        [dbo].[User] AS Seller ON [Transactions].[SellerID] = Seller.[UserID]
    INNER JOIN
        [dbo].[Property] ON [Transactions].[PropertyID] = [Property].[PropertyID]
    ORDER BY 
        [dbo].[Transactions].[TransactionDate] DESC
END

-- 2.Retrieve Transactions by Seller ID
GO
CREATE PROCEDURE [dbo].[PR_TRANS_Transaction_SelectBySellerID]
    @SellerID INT
AS
BEGIN
    SELECT 
        [dbo].[Transactions].[TransactionID],
        [dbo].[Transactions].[TotalTransactionAmount],
        [dbo].[Transactions].[PaidAmount],
        [dbo].[Transactions].[RemainingAmount],
        [dbo].[Transactions].[TransactionDate],
        [dbo].[Transactions].[PaymentType],
        [dbo].[Transactions].[PaymentStatus],
        [dbo].[Transactions].[PaymentReferenceNumber],
        [dbo].[Transactions].[CashPaymentAmount],
        [dbo].[Transactions].[CardNumber],
        [dbo].[Transactions].[CardHolderName],
        [dbo].[Transactions].[CardExpiryDate],
        [dbo].[Transactions].[UPIID],
        [dbo].[Transactions].[SellerID],
		Seller.[FirstName] + ' ' + Seller.[LastName] AS SellerName,
        [dbo].[Transactions].[BuyerID],
		Buyer.[FirstName] + ' ' + Buyer.[LastName] AS BuyerName,
        [dbo].[Transactions].[Status],
        [dbo].[Transactions].[TransactionType],
        [dbo].[Transactions].[TransactionDetail],
        [dbo].[Transactions].[LastTransactionDate],
        [dbo].[Transactions].[PropertyID],
		[dbo].[Property].[PropertyTitle]
    FROM 
        [dbo].[Transactions]
	INNER JOIN
        [dbo].[User] AS Buyer ON [Transactions].[BuyerID] = Buyer.[UserID]
    INNER JOIN
        [dbo].[User] AS Seller ON [Transactions].[BuyerID] = Seller.[UserID]
    INNER JOIN
        [dbo].[Property] ON [Transactions].[BuyerID] = [Property].[PropertyID]
    WHERE 
        [dbo].[Transactions].[SellerID] = @SellerID
    ORDER BY 
        [dbo].[Transactions].[TransactionDate] DESC
END

-- 3.Retrieve Transactions by Buyer ID:
GO
CREATE PROCEDURE [dbo].[PR_TRANS_Transaction_SelectByBuyerID]
    @BuyerID INT
AS
BEGIN
    SELECT 
        [dbo].[Transactions].[TransactionID],
        [dbo].[Transactions].[TotalTransactionAmount],
        [dbo].[Transactions].[PaidAmount],
        [dbo].[Transactions].[RemainingAmount],
        [dbo].[Transactions].[TransactionDate],
        [dbo].[Transactions].[PaymentType],
        [dbo].[Transactions].[PaymentStatus],
        [dbo].[Transactions].[PaymentReferenceNumber],
        [dbo].[Transactions].[CashPaymentAmount],
        [dbo].[Transactions].[CardNumber],
        [dbo].[Transactions].[CardHolderName],
        [dbo].[Transactions].[CardExpiryDate],
        [dbo].[Transactions].[UPIID],
        [dbo].[Transactions].[SellerID],
		Seller.[FirstName] + ' ' + Seller.[LastName] AS SellerName,
        [dbo].[Transactions].[BuyerID],
		Buyer.[FirstName] + ' ' + Buyer.[LastName] AS BuyerName,
        [dbo].[Transactions].[Status],
        [dbo].[Transactions].[TransactionType],
        [dbo].[Transactions].[TransactionDetail],
        [dbo].[Transactions].[LastTransactionDate],
        [dbo].[Transactions].[PropertyID],
		[dbo].[Property].[PropertyTitle]
    FROM 
        [dbo].[Transactions]
	INNER JOIN
        [dbo].[User] AS Buyer ON [Transactions].[BuyerID] = Buyer.[UserID]
    INNER JOIN
        [dbo].[User] AS Seller ON [Transactions].[BuyerID] = Seller.[UserID]
    INNER JOIN
        [dbo].[Property] ON [Transactions].[BuyerID] = [Property].[PropertyID]
    WHERE 
        [dbo].[Transactions].[BuyerID] = @BuyerID
    ORDER BY 
        [dbo].[Transactions].[TransactionDate] DESC
END

-- 4.Retrieve Transactions by Property ID:
GO
CREATE PROCEDURE [dbo].[PR_TRANS_Transaction_SelectByPropertyID]
    @PropertyID INT
AS
BEGIN
    SELECT 
        [dbo].[Transactions].[TransactionID],
        [dbo].[Transactions].[TotalTransactionAmount],
        [dbo].[Transactions].[PaidAmount],
        [dbo].[Transactions].[RemainingAmount],
        [dbo].[Transactions].[TransactionDate],
        [dbo].[Transactions].[PaymentType],
        [dbo].[Transactions].[PaymentStatus],
        [dbo].[Transactions].[PaymentReferenceNumber],
        [dbo].[Transactions].[CashPaymentAmount],
        [dbo].[Transactions].[CardNumber],
        [dbo].[Transactions].[CardHolderName],
        [dbo].[Transactions].[CardExpiryDate],
        [dbo].[Transactions].[UPIID],
        [dbo].[Transactions].[SellerID],
		Seller.[FirstName] + ' ' + Seller.[LastName] AS SellerName,
        [dbo].[Transactions].[BuyerID],
		Seller.[FirstName] + ' ' + Seller.[LastName] AS SellerName,
        [dbo].[Transactions].[Status],
        [dbo].[Transactions].[TransactionType],
        [dbo].[Transactions].[TransactionDetail],
        [dbo].[Transactions].[LastTransactionDate],
        [dbo].[Transactions].[PropertyID],
		[dbo].[Property].[PropertyTitle]
    FROM 
        [dbo].[Transactions]
	INNER JOIN
        [dbo].[User] AS Buyer ON [Transactions].[BuyerID] = Buyer.[UserID]
    INNER JOIN
        [dbo].[User] AS Seller ON [Transactions].[BuyerID] = Seller.[UserID]
    INNER JOIN
        [dbo].[Property] ON [Transactions].[BuyerID] = [Property].[PropertyID]
    WHERE 
        [dbo].[Transactions].[PropertyID] = @PropertyID
    ORDER BY 
        [dbo].[Transactions].[TransactionDate] DESC
END

-- 5.Insert Transaction:
GO
ALTER PROCEDURE [dbo].[PR_TRANS_Transaction_Insert]
    @TotalTransactionAmount DECIMAL(10, 2),
    @PaidAmount DECIMAL(10, 2),
    @RemainingAmount DECIMAL(10, 2),
    @TransactionDate DATETIME,
    @PaymentType VARCHAR(25),
    @PaymentStatus VARCHAR(20) = 'Complete',
    @PaymentReferenceNumber VARCHAR(100) = NULL,
    @CashPaymentAmount DECIMAL(10, 2) = NULL,
    @CardNumber VARCHAR(16) = NULL,
    @CardHolderName VARCHAR(100) = NULL,
    @CardExpiryDate CHAR(5) = NULL,
    @UPIID VARCHAR(100) = NULL,
    @SellerID INT,
    @BuyerID INT,
    @Status VARCHAR(20) = 'Complete',
    @TransactionType VARCHAR(50) = NULL,
    @TransactionDetail TEXT = NULL,
    @PropertyID INT,
    @TransactionID INT OUTPUT
AS
BEGIN
    INSERT INTO [dbo].[Transactions] 
    (
        TotalTransactionAmount, PaidAmount, RemainingAmount, TransactionDate,
        PaymentType, PaymentStatus, PaymentReferenceNumber, CashPaymentAmount,
        CardNumber, CardHolderName, CardExpiryDate, UPIID, SellerID, BuyerID, 
        Status, TransactionType, TransactionDetail, PropertyID,LastTransactionDate
    )
    VALUES
    (
        @TotalTransactionAmount, @PaidAmount, @RemainingAmount, @TransactionDate,
        @PaymentType, @PaymentStatus, @PaymentReferenceNumber, @CashPaymentAmount,
        @CardNumber, @CardHolderName, @CardExpiryDate, @UPIID, @SellerID, @BuyerID, 
        @Status, @TransactionType, @TransactionDetail, @PropertyID,GETDATE()
    )

    SET @TransactionID = SCOPE_IDENTITY();
END


-- 5.Update Transaction:
GO
ALTER PROCEDURE [dbo].[PR_TRANS_Transaction_Update]
    @TransactionID INT,
    @TotalTransactionAmount DECIMAL(10, 2),
    @PaidAmount DECIMAL(10, 2),
    @RemainingAmount DECIMAL(10, 2),
    @TransactionDate DATETIME,
    @PaymentType VARCHAR(25),
    @PaymentStatus VARCHAR(20),
    @PaymentReferenceNumber VARCHAR(100) = NULL,
    @CashPaymentAmount DECIMAL(10, 2) = NULL,
    @CardNumber VARCHAR(16) = NULL,
    @CardHolderName VARCHAR(100) = NULL,
    @CardExpiryDate CHAR(5) = NULL,
    @UPIID VARCHAR(100) = NULL,
    @SellerID INT,
    @BuyerID INT,
    @Status VARCHAR(20),
    @TransactionType VARCHAR(50) = NULL,
    @TransactionDetail TEXT = NULL,
    @PropertyID INT
AS
BEGIN
    UPDATE [dbo].[Transactions]
    SET 
        TotalTransactionAmount = @TotalTransactionAmount,
        PaidAmount = @PaidAmount,
        RemainingAmount = @RemainingAmount,
        TransactionDate = @TransactionDate,
        PaymentType = @PaymentType,
        PaymentStatus = @PaymentStatus,
        PaymentReferenceNumber = @PaymentReferenceNumber,
        CashPaymentAmount = @CashPaymentAmount,
        CardNumber = @CardNumber,
        CardHolderName = @CardHolderName,
        CardExpiryDate = @CardExpiryDate,
        UPIID = @UPIID,
        SellerID = @SellerID,
        BuyerID = @BuyerID,
        Status = @Status,
        TransactionType = @TransactionType,
        TransactionDetail = @TransactionDetail,
		LastTransactionDate = GETDATE(),
        PropertyID = @PropertyID
    WHERE 
        TransactionID = @TransactionID
END


ALTER TABLE Installments
ADD PaymentType VARCHAR(25) NOT NULL
SELECT * FROM Installments
-- Installments

-- 1. Retrieve All Installments:
GO
ALTER PROCEDURE [dbo].[PR_INST_Installment_SelectAll]
AS
BEGIN
    SELECT 
        InstallmentID,
        TransactionID,
        InstallmentAmount,
        InstallmentDate,
        PaidAmount,
        PaymentStatus,
        PaymentReferenceNumber,
        LastPaymentDate,
        CashPaymentAmount,
        CardNumber,
        CardHolderName,
        CardExpiryDate,
        UPIID,
        PaymentType
    FROM 
        [dbo].[InstallmentS]
    ORDER BY 
        InstallmentDate DESC
END

-- 2. Retrieve Installment by ID:
GO
ALTER PROCEDURE [dbo].[PR_INST_Installment_SelectByID]
    @InstallmentID INT
AS
BEGIN
    SELECT 
        InstallmentID,
        TransactionID,
        InstallmentAmount,
        InstallmentDate,
        PaidAmount,
        PaymentStatus,
        PaymentReferenceNumber,
        LastPaymentDate,
        CashPaymentAmount,
        CardNumber,
        CardHolderName,
        CardExpiryDate,
        UPIID,
        PaymentType
    FROM 
        [dbo].[InstallmentS]
    WHERE 
        InstallmentID = @InstallmentID
END

-- 3. Retrieve Installments by Transaction ID:
GO
ALTER PROCEDURE [dbo].[PR_INST_Installment_SelectByTransactionID]
    @TransactionID INT
AS
BEGIN
    SELECT 
        InstallmentID,
        TransactionID,
        InstallmentAmount,
        InstallmentDate,
        PaidAmount,
        PaymentStatus,
        PaymentReferenceNumber,
        LastPaymentDate,
        CashPaymentAmount,
        CardNumber,
        CardHolderName,
        CardExpiryDate,
        UPIID,
        PaymentType
    FROM 
        [dbo].[InstallmentS]
    WHERE 
        TransactionID = @TransactionID
    ORDER BY 
        InstallmentDate DESC
END

-- 4. Insert Installment:
GO
ALTER PROCEDURE [dbo].[PR_INST_Installment_Insert]
    @TransactionID INT,
    @InstallmentAmount DECIMAL(10, 2),
    @InstallmentDate DATETIME,
    @PaidAmount DECIMAL(10, 2) = 0,
    @PaymentStatus VARCHAR(20) = 'Pending',
    @PaymentReferenceNumber VARCHAR(100) = NULL,
    @LastPaymentDate DATETIME = NULL,
    @CashPaymentAmount DECIMAL(10, 2) = NULL,
    @CardNumber VARCHAR(16) = NULL,
    @CardHolderName VARCHAR(100) = NULL,
    @CardExpiryDate CHAR(5) = NULL,
    @UPIID VARCHAR(100) = NULL,
    @PaymentType VARCHAR(20)
AS
BEGIN
    INSERT INTO [dbo].[InstallmentS] 
    (
        TransactionID, InstallmentAmount, InstallmentDate, PaidAmount, 
        PaymentStatus, PaymentReferenceNumber, LastPaymentDate,
        CashPaymentAmount, CardNumber, CardHolderName, CardExpiryDate, UPIID, PaymentType
    )
    VALUES
    (
        @TransactionID, @InstallmentAmount, @InstallmentDate, @PaidAmount, 
        @PaymentStatus, @PaymentReferenceNumber, @LastPaymentDate,
        @CashPaymentAmount, @CardNumber, @CardHolderName, @CardExpiryDate, @UPIID, @PaymentType
    )
END

-- 5. Update Installment:
GO
ALTER PROCEDURE [dbo].[PR_INST_Installment_Update]
    @InstallmentID INT,
    @TransactionID INT,
    @InstallmentAmount DECIMAL(10, 2),
    @InstallmentDate DATETIME,
    @PaidAmount DECIMAL(10, 2),
    @PaymentStatus VARCHAR(20),
    @PaymentReferenceNumber VARCHAR(100) = NULL,
    @LastPaymentDate DATETIME = NULL,
    @CashPaymentAmount DECIMAL(10, 2) = NULL,
    @CardNumber VARCHAR(16) = NULL,
    @CardHolderName VARCHAR(100) = NULL,
    @CardExpiryDate CHAR(5) = NULL,
    @UPIID VARCHAR(100) = NULL,
    @PaymentType VARCHAR(20)
AS
BEGIN
    UPDATE [dbo].[InstallmentS]
    SET 
        TransactionID = @TransactionID,
        InstallmentAmount = @InstallmentAmount,
        InstallmentDate = @InstallmentDate,
        PaidAmount = @PaidAmount,
        PaymentStatus = @PaymentStatus,
        PaymentReferenceNumber = @PaymentReferenceNumber,
        LastPaymentDate = @LastPaymentDate,
        CashPaymentAmount = @CashPaymentAmount,
        CardNumber = @CardNumber,
        CardHolderName = @CardHolderName,
        CardExpiryDate = @CardExpiryDate,
        UPIID = @UPIID,
        PaymentType = @PaymentType
    WHERE 
        InstallmentID = @InstallmentID
END


-- DropDwon For Transaction With Price
GO
CREATE PROCEDURE [dbo].[PR_LOC_Transaction_PropertyDropdown_ByUserID] 2
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        [PropertyID],
        [PropertyTitle],
		[PropertyPrice]
    FROM 
        [dbo].[Property]
    WHERE 
        [UserID] = @UserID
    ORDER BY 
        [PropertyTitle];
END


CREATE TABLE AgentReviews (
    ReviewID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    AgentID INT NOT NULL,
    UserID INT NOT NULL,
    Rating INT CHECK (Rating BETWEEN 1 AND 5) NOT NULL,
    ReviewText VARCHAR(500) NOT NULL,
    Keywords VARCHAR(255) NULL,
    SubmittedAt DATETIME DEFAULT GETDATE() NOT NULL,
    FOREIGN KEY (AgentID) REFERENCES [User](UserID),
    FOREIGN KEY (UserID) REFERENCES [User](UserID)
);


CREATE TABLE PropertyReviews (
    ReviewID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    PropertyID INT NOT NULL,
    UserID INT NOT NULL,
    Rating INT CHECK (Rating BETWEEN 1 AND 5) NOT NULL,
    ReviewText VARCHAR(500) NOT NULL,
    Keywords VARCHAR(255) NULL,
    SubmittedAt DATETIME DEFAULT GETDATE() NOT NULL,
    FOREIGN KEY (PropertyID) REFERENCES Property(PropertyID),
    FOREIGN KEY (UserID) REFERENCES [User](UserID)
);

-- Property
GO
ALTER PROCEDURE [dbo].[Insert_PropertyReview]
    @PropertyID INT,
    @UserID INT,
    @Rating INT,
    @ReviewText NVARCHAR(500),
    @Keywords NVARCHAR(255)
AS
BEGIN

    INSERT INTO [dbo].[PropertyReviews] 
        ([PropertyID], [UserID], [Rating], [ReviewText], [Keywords], [SubmittedAt])
    VALUES 
        (@PropertyID, @UserID, @Rating, @ReviewText, @Keywords, GETDATE());
END


GO
ALTER PROCEDURE [dbo].[Get_PropertyReviews]
    @PropertyID INT
AS
BEGIN

    SELECT 
        [ReviewID],
        [PropertyID],
        [PropertyReviews].[UserID],
		Buyer.[FirstName] + ' ' + Buyer.[LastName] AS FullName,
        [Rating],
        [ReviewText],
        [Keywords],
        [SubmittedAt]
    FROM 
        [dbo].[PropertyReviews]
	INNER JOIN
        [dbo].[User] AS Buyer ON [PropertyReviews].[UserID] = Buyer.[UserID]
    WHERE 
        [PropertyID] = @PropertyID
    ORDER BY 
        [SubmittedAt] DESC;
END

GO
CREATE PROCEDURE [dbo].[Edit_PropertyReview]
    @ReviewID INT,
    @Rating INT,
    @ReviewText NVARCHAR(500),
    @Keywords NVARCHAR(255)
AS
BEGIN
    UPDATE [dbo].[PropertyReviews]
    SET 
        [Rating] = @Rating,
        [ReviewText] = @ReviewText,
        [Keywords] = @Keywords,
        [SubmittedAt] = GETDATE()  -- Update timestamp
    WHERE 
        [ReviewID] = @ReviewID;
END

GO
CREATE PROCEDURE [dbo].[Delete_PropertyReview]
    @ReviewID INT
AS
BEGIN
    DELETE FROM [dbo].[PropertyReviews]
    WHERE [ReviewID] = @ReviewID;
END


-- Agent
ALTER PROCEDURE [dbo].[Insert_AgentReview]
    @AgentID INT,
    @UserID INT,
    @Rating INT,
    @ReviewText NVARCHAR(500),
    @Keywords NVARCHAR(255)
AS
BEGIN

    INSERT INTO [dbo].[AgentReviews] 
        ([AgentID], [UserID], [Rating], [ReviewText], [Keywords], [SubmittedAt])
    VALUES 
        (@AgentID, @UserID, @Rating, @ReviewText, @Keywords, GETDATE());
END

ALTER PROCEDURE [dbo].[Get_AgentReviews]
    @AgentID INT
AS
BEGIN
    SELECT 
        [ReviewID],
        [AgentID],
        [AgentReviews].[UserID],
		Buyer.[FirstName] + ' ' + Buyer.[LastName] AS FullName,
        [Rating],
        [ReviewText],
        [Keywords],
        [SubmittedAt]
    FROM 
        [dbo].[AgentReviews]
	INNER JOIN
        [dbo].[User] AS Buyer ON [AgentReviews].[UserID] = Buyer.[UserID]
    WHERE 
        [AgentID] = @AgentID
    ORDER BY 
        [SubmittedAt] DESC;
END

GO
CREATE PROCEDURE [dbo].[Edit_AgentReview]
    @ReviewID INT,
    @Rating INT,
    @ReviewText NVARCHAR(500),
    @Keywords NVARCHAR(255)
AS
BEGIN
    UPDATE [dbo].[AgentReviews]
    SET 
        [Rating] = @Rating,
        [ReviewText] = @ReviewText,
        [Keywords] = @Keywords,
        [SubmittedAt] = GETDATE()  -- Update timestamp
    WHERE 
        [ReviewID] = @ReviewID;
END

GO
CREATE PROCEDURE [dbo].[Delete_AgentReview]
    @ReviewID INT
AS
BEGIN
    DELETE FROM [dbo].[AgentReviews]
    WHERE [ReviewID] = @ReviewID;
END

SELECT * FROM AgentReviews
SELECT * FROM PropertyReviews


-- Socila Meadia Table
CREATE TABLE UserSocialMedia (
    SocialMediaID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL,
    Platform VARCHAR(50) NOT NULL,
    ProfileLink VARCHAR(255) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedAt DATETIME NULL,
    FOREIGN KEY (UserID) REFERENCES [User](UserID) ON DELETE CASCADE
);

GO
CREATE PROCEDURE [dbo].[PR_UserSocialMedia_Add]
    @UserID INT,
    @Platform VARCHAR(50),
    @ProfileLink VARCHAR(255)
AS
BEGIN
    INSERT INTO UserSocialMedia
    (
        UserID,
        Platform,
        ProfileLink,
        CreatedAt
    )
    VALUES
    (
        @UserID,
        @Platform,
        @ProfileLink,
        GETDATE()
    )
END

GO
CREATE PROCEDURE [dbo].[PR_UserSocialMedia_Update]
    @SocialMediaID INT,
    @ProfileLink VARCHAR(255)
AS
BEGIN
    UPDATE UserSocialMedia
    SET ProfileLink = @ProfileLink,
        ModifiedAt = GETDATE()
    WHERE SocialMediaID = @SocialMediaID
END

GO
CREATE PROCEDURE [dbo].[PR_UserSocialMedia_GetByUser]
    @UserID INT
AS
BEGIN
    SELECT 
        SocialMediaID,
        UserID,
        Platform,
        ProfileLink,
        CreatedAt,
        ModifiedAt
    FROM UserSocialMedia
    WHERE UserID = @UserID
    ORDER BY CreatedAt DESC
END

CREATE PROCEDURE PR_LOC_User_GetByEmail
    @Email NVARCHAR(255)
AS
BEGIN
    SELECT UserID, UserName, FirstName, LastName, Email, UserRole, ProfilePhoto
    FROM [User]
    WHERE Email = @Email;
END;

GO
CREATE PROCEDURE ChangeUserPassword
    @UserId INT,
    @OldPassword NVARCHAR(255),
    @NewPassword NVARCHAR(255)
AS
BEGIN
    DECLARE @ExistingPassword NVARCHAR(255);

    -- Fetch existing password
    SELECT @ExistingPassword = Password FROM [User] WHERE UserID = @UserId;

    -- Verify old password
    IF (@ExistingPassword IS NULL OR @ExistingPassword <> @OldPassword)
    BEGIN
        THROW 50001, 'Incorrect Old Password', 1;
    END

    -- Update new password
    UPDATE [User]
    SET Password = @NewPassword
    WHERE UserID = @UserId;
END


CREATE TABLE UserOTP (
    OTPID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL,
    OTP VARCHAR(6) NOT NULL,
    OTPExpiry DATETIME NOT NULL,
    IsUsed BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES [User](UserID) ON DELETE CASCADE
);

CREATE PROCEDURE [dbo].[PR_LOC_OTP_SaveUserOTP]
    @Email NVARCHAR(50),
    @OTP NVARCHAR(6),
    @OTPExpiry DATETIME
AS
BEGIN
    DECLARE @UserID INT;
    
    SELECT @UserID = UserID FROM [User] WHERE Email = @Email;
    IF @UserID IS NULL
        RETURN;
    
    INSERT INTO UserOTP (UserID, OTP, OTPExpiry, IsUsed)
    VALUES (@UserID, @OTP, @OTPExpiry, 0);
END;

CREATE PROCEDURE [dbo].[PR_LOC_OTP_VerifyOTPAndResetPassword]
    @Email NVARCHAR(50),
    @OTP NVARCHAR(6),
    @NewPassword NVARCHAR(255)
AS
BEGIN
    DECLARE @UserID INT;
    
    SELECT @UserID = UserID FROM [User] WHERE Email = @Email;
    IF @UserID IS NULL
        RETURN;
    
    -- Check if OTP is valid
    IF EXISTS (SELECT 1 FROM UserOTP WHERE UserID = @UserID AND OTP = @OTP AND OTPExpiry > GETUTCDATE() AND IsUsed = 0)
    BEGIN
        -- Update password
        UPDATE [User] SET Password = @NewPassword WHERE UserID = @UserID;
        
        -- Mark OTP as used
        UPDATE UserOTP SET IsUsed = 1 WHERE UserID = @UserID AND OTP = @OTP;
        
        SELECT 'Success' AS Status;
    END
    ELSE
    BEGIN
        SELECT 'Invalid or Expired OTP' AS Status;
    END
END;

SELECT * FROM [UserOTP]
SELECT * FROM [User]