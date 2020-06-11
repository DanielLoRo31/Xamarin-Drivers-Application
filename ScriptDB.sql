CREATE DATABASE XamarinDriversTable
GO

USE XamarinDriversTable
GO

CREATE TABLE Position(
	IDPosition INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Latitude FLOAT,
	Longitude FLOAT
)
GO

CREATE TABLE Driver
(
	IDDriver INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Name VARCHAR(50),
	Status VARCHAR(50),
	Picture TEXT,
	IDActualPosition INT FOREIGN KEY REFERENCES Position(IDPosition)
)
GO

INSERT INTO Position (Latitude, Longitude) VALUES
(25.632145987, 65.326589214),
(-5.632145987, 30.326589214)
GO

INSERT INTO Driver (Name, Status, Picture, IDActualPosition) VALUES
('Daniel', 'Free', 'https://pbs.twimg.com/profile_images/1179400462950436875/qGsVforj_400x400.jpg', 1),
('Mario', 'Free', 'https://upload.wikimedia.org/wikipedia/commons/b/b2/Rappi_backgr_logo.png', 2)
GO

CREATE PROCEDURE sp_getAllDrivers AS
BEGIN
	SELECT * FROM Driver INNER JOIN Position ON Position.IDPosition = Driver.IDActualPosition
END
GO

CREATE PROCEDURE sp_getDriver 
	@IDDriver INT
AS
BEGIN
	SELECT * FROM Driver INNER JOIN Position ON Position.IDPosition = Driver.IDActualPosition WHERE Driver.IDDriver = @IDDriver
END
GO

CREATE PROCEDURE sp_CreateDriver 
	@Name VARCHAR(50),
	@Status VARCHAR(50),
	@Picture TEXT,
	@Latitude FLOAT,
	@Longitude FLOAT
AS
BEGIN
	INSERT INTO Position (Latitude, Longitude) VALUES (@Latitude, @Longitude);
	INSERT INTO Driver (Name, Status, Picture, IDActualPosition) VALUES (@Name, @Status, @Picture, @@IDENTITY);
	SELECT @@IDENTITY;
END
GO

CREATE PROCEDURE sp_UpdateDriverNewPosition 
	@IDDriver INT,
	@Name VARCHAR(50),
	@Status VARCHAR(50),
	@Picture TEXT,
	@Latitude FLOAT,
	@Longitude FLOAT
AS
BEGIN
	INSERT INTO Position (Latitude, Longitude) VALUES (@Latitude, @Longitude);

	UPDATE Driver SET
	Name = @Name,
	Status = @Status,
	Picture = @Picture,
	IDActualPosition = @@IDENTITY
	WHERE IDDriver = @IDDriver;

	SELECT @IDDriver;
END
GO

CREATE PROCEDURE sp_UpdateDriverOldPosition 
	@IDDriver INT,
	@Name VARCHAR(50),
	@Status VARCHAR(50),
	@Picture TEXT
AS
BEGIN
	UPDATE Driver SET
	Name = @Name,
	Status = @Status,
	Picture = @Picture
	WHERE IDDriver = @IDDriver;

	SELECT @IDDriver;
END
GO

EXEC sp_getAllDrivers 
GO

EXEC sp_getDriver 2
GO

EXEC sp_CreateDriver 'Fabri', 'Free', 'https://lh3.googleusercontent.com/qy_wZ92sFQccojEtscg52vtdAQmCIeQ4jsybMPmuML9Or7_SEOyrt0Jn0wyG-l2Fyw', 25.654898546, -98.632546859
GO

EXEC sp_UpdateDriverNewPosition 2, 'Pepe', 'Free', 'https://lh3.googleusercontent.com/qy_wZ92sFQccojEtscg52vtdAQmCIeQ4jsybMPmuML9Or7_SEOyrt0Jn0wyG-l2Fyw', 35.654898546, -98.632546859
GO

EXEC sp_UpdateDriverOldPosition 2, 'Juan', 'Free', 'https://lh3.googleusercontent.com/qy_wZ92sFQccojEtscg52vtdAQmCIeQ4jsybMPmuML9Or7_SEOyrt0Jn0wyG-l2Fyw'
GO