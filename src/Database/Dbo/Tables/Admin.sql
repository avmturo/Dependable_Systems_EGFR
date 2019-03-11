CREATE TABLE [dbo].[Admin]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Username] NVARCHAR(50) NOT NULL, 
    [Password] NVARCHAR(50) NOT NULL
)


GO


CREATE UNIQUE INDEX [Unique_Username] ON [dbo].[Admin] ([Username])
