CREATE TABLE [dbo].[Task]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Sid] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(MAX) NOT NULL, 
    [IsCompleted] BIT NOT NULL, 
    [DateCreated] DATETIME NOT NULL, 
    [DateDue] DATETIME NULL, 
    [DateCompleted] DATETIME NULL
)
