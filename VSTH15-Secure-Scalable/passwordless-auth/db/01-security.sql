CREATE USER [<managed-identity-name>] FROM EXTERNAL PROVIDER
GO

ALTER ROLE db_datareader ADD MEMBER [<managed-identity-name>]
GO

SELECT * FROM sys.database_principals WHERE [name] = '<managed-identity-name>'
go