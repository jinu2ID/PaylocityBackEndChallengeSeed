CREATE TABLE [dbo].[Dependents]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[FirstName] NVARCHAR(100) NOT NULL,
	[LastName] NVARCHAR(100) NOT NULL,
	[DateOfBirth] DATETIME2 NOT NULL,
	[Relationship] INT NOT NULL, -- Enum: 0=Spouse, 1=DomesticPartner, 2=Child
	[EmployeeId] INT NOT NULL,
	CONSTRAINT [FK_Dependents_Employees] FOREIGN KEY ([EmployeeId])
		REFERENCES [dbo].[Employees]([Id])
)
GO
