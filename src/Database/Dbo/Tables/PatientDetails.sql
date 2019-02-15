CREATE TABLE [dbo].[PatientDetails]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[DateOfBirth] DATE NOT NULL,
	[Gender] BIT NOT NULL,
	[Ethnicity] BIT NOT NULL 
)
