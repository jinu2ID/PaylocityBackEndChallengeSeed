﻿CREATE TABLE [dbo].[Employees]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[FirstName] NVARCHAR(100) NOT NULL,
	[LastName] NVARCHAR(100) NOT NULL,
	[Salary] DECIMAL(10,2) NOT NULL,
	[DateOfBirth] DATETIME2 NOT NULL
)
GO
