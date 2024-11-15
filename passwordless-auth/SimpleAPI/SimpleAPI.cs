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

        private readonly string? _connectionString = Environment.GetEnvironmentVariable("MSSQL");

        private readonly ILogger<SimpleAPI> _logger;

        public SimpleAPI(ILogger<SimpleAPI> logger)
        {
            _logger = logger;
        }
        [Function("SimpleAPI")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("Querying database...");

            if (_connectionString is null)
            {
                return new BadRequestObjectResult("Please set the 'MSSQL' environment variable to a valid connection string.");
            }

            using SqlConnection conn = new SqlConnection(_connectionString);
            var result = JsonDocument.Parse(conn.QueryFirst<string>(_sql));
            return new OkObjectResult(result);
        }
    }
}