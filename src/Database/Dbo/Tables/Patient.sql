CREATE TABLE [dbo].[Patient]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[NHSNumber] NVARCHAR(50) NOT NULL,
	[Password] NVARCHAR(50) NOT NULL, 
    [FK_PatientDetails_Id] INT NULL, 
    CONSTRAINT [FK_Patient_ToPatientDetails] FOREIGN KEY ([FK_PatientDetails_Id]) REFERENCES [PatientDetails]([Id])
)

GO

CREATE UNIQUE INDEX [Unique_NSHNumber] ON [dbo].[Patient] ([NHSNumber])