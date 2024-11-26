using DotNetEnv;
using Microsoft.Data.SqlClient;
using Dapper;

namespace sql_injection;

class Program
{
    static void Main(string[] args)
    {
        Env.Load();
        
        Console.WriteLine("Database Object Search Engine");
        Console.WriteLine("-----------------------------");

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Enter the search term:");
            var searchTerm = Console.ReadLine();

            using var conn = new SqlConnection(Env.GetString("MSSQL"));
            var result = conn.Query($"""
                SELECT 
                    [object_id], [name], [type_desc] 
                FROM 
                    sys.objects 
                WHERE 
                    [name] LIKE '%{searchTerm}%' 
                AND
                    [type_desc] = 'USER_TABLE'
                ORDER BY 
                    [name]
            """); // SQL Injection 
            foreach (var row in result)
            {
                Console.WriteLine($"{row.object_id} => {row.name} ({row.type_desc})");
            }
        }
    }
}
