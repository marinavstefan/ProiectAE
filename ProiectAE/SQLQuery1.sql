IF (DB_ID(N'Proiect') IS NULL)
	CREATE DATABASE Proiect
GO

USE Proiect
GO

IF OBJECT_ID ('Products') IS NULL
	CREATE TABLE Products
	(
	Id INT NOT NULL IDENTITY(1, 1) CONSTRAINT PK_Product PRIMARY KEY,
	[Name] NVARCHAR(256) NOT NULL,
	[Description] NVARCHAR(2000) NOT NULL, 
	Price NUMERIC(20, 2) NOT NULL,
	Review NUMERIC(20,2) NOT NULL,
	IsAvailable BIT NOT NULL,
	ImagePath NVARCHAR(1000) NULL
	)
GO