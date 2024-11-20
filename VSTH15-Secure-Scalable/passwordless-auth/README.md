azd env new

azd env set AZURE_SQL_SERVER dmmssqlsrv.database.windows.net

azd env set AZURE_SQL_DB lab   

azd up

--

azd env get-value AZURE_USER_ASSIGNED_IDENTITY_NAME

--

create user [id-api-oa2kevqh5u5rk] from external provider;
go

alter role db_datareader add member [id-api-oa2kevqh5u5rk];
go


---

