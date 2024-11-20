using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Text.Json;
using Dapper;
using System.ComponentModel;

namespace Azure.SQLDB.Samples
{
    public class SimpleAPI(ILogger<SimpleAPI> logger)
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

        private readonly string? _connectionString = Environment.GetEnvironmentVariable("MSSQL");

        private readonly ILogger<SimpleAPI> _logger = logger;

        [Function("WhoAmI")]
        public IActionResult RunSimpleAPI([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "who-am-i")] HttpRequest req)
        {
            if (_connectionString is null)
            {
                return new BadRequestObjectResult("Please set the 'MSSQL' environment variable to a valid connection string.");
            }

            using SqlConnection conn = new SqlConnection(_connectionString);
            var result = JsonDocument.Parse(conn.QueryFirst<string>(_sql));
            return new OkObjectResult(result);
        }

        [Function("Test")]
        public IActionResult RunTest([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "test")] HttpRequest req)
        {
            _logger.LogInformation("Test API...");
          
            return new OkObjectResult(new { message = "Test API" });
        }
    }
}