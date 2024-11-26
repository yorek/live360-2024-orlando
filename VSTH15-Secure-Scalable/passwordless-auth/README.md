# Passwordless Authentication Sample

This deploys a passwordless authentication solution using Azure Functions, Azure SQL Database, Azure Entra ID, and Azure Managed Identity.

## Prepare Azure SQL database

Make sure you have an Azure SQL database that you can connect to using your Entra ID account.


## Run sample locally

Make sure you have [Azure Functions Core Tools](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local) installed.

Move into the `SimpleAPI` folder, then create a `local.settings.json` using `local.settings.json.sample` as a starting point. Replace the placeholders 

- `<your-server>`
- `<your-database>`

with your own values. Then run the following commands:

```bash
func start
```

to run the function locally.

You can the use the `test.http` file to test the function, using the [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) extension for Visual Studio Code.

The function endpoint is `http://localhost:7071/api/who-am-i`.

When you connect to the endpoint, the Azure Function will connect to the database using your credential, and so your account name will be returned as a result of the HTTP GET request.

## Deploy to Azure

Make sure that you have installed:

- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)
- [Azure Developer CLI](https://learn.microsoft.com/en-us/azure/developer/azure-developer-cli/overview?tabs=windows

Then initialize a new Azure Developer CLI environment:

```bash
azd env new
```

and configure it with your Azure SQL database details:

```bash
azd env set AZURE_SQL_SERVER <your-server>.database.windows.net
azd env set AZURE_SQL_DB <your-database>   
```

Then deploy the function to Azure:

```bash
azd up
```

Once the deployment is complete, you need to get the name of the created user-assigned managed identity:

```bash
azd env get-value AZURE_USER_ASSIGNED_IDENTITY_NAME
```

Then connect to Azure SQL and run the following commands to allow the managed identity to connect to the database:

```sql
create user [<managed-identity-name>] from external provider;
go

alter role db_datareader add member [<managed-identity-name>];
go
```

You can now test the function by connecting to the endpoint provided by the Azure Function. You can the use the `test.http` file to test the function, using the [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) extension for Visual Studio Code.

The function endpoint is `https://<azure-function-endpoint>/api/who-am-i`.

The result of the HTTP GET request will be the managed identity account name, as the Azure Function will connect to the database using the managed identity.

