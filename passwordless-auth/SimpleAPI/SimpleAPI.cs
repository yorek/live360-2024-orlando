using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Text.Json;
using Dapper;

namespace Azure.SQLDB.Samples
{
    public class SimpleAPI
    {
        private readonly string _sql = @"           
            select json_object(
                'session_user': session_user,
                'user_name': user_name(),
                'user_sname': user_name(),
                'suser_name': suser_name(),
                'suser_sname': suser_sname()
                ) as userData
        ";

        private readonly string _connectionString = "Server=tcp:dmmssqlsrv.database.windows.net,1433;Initial Catalog=lab;Authentication=Active Directory Default;";
 
        [Function("SimpleAPI")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous , "get", "post")] HttpRequest req)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            var result = JsonDocument.Parse(conn.QueryFirst<string>(_sql));
            return new OkObjectResult(result);
        }
    }
}