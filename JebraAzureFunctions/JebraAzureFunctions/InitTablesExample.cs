using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace JebraCreateDBTables
{
    public static class InitTablesExample
    {
        [FunctionName("InitTablesExample")]
        public static async Task Run([TimerTrigger("*/15 * * * * *")] TimerInfo myTimer, ILogger log)
        {
            // Get the connection string from app settings and use it to create a connection.
            var str = Environment.GetEnvironmentVariable("SqlConnectionString");
            using (SqlConnection conn = new SqlConnection(str))
            {
                System.Diagnostics.Debug.WriteLine("Conn String: " + str);
                conn.Open();
                var text = @"CREATE TABLE client (
                    clientId INT IDENTITY PRIMARY KEY,
                    fname varchar(255) NOT NULL,
                    lname varchar(255) NOT NULL,
                    age int NOT NULL,
                );";

                var text2 = "INSERT INTO client (fname, lname, age) values ('Turd', 'Ferguson', 39)";
                var text3 = "SELECT * FROM client";

                using (SqlCommand cmd = new SqlCommand(text, conn))
                {
                    // Execute the command and log the # rows affected.
                    var rows = await cmd.ExecuteNonQueryAsync();
                    log.LogInformation($"{rows} rows were updated");
                }

                using (SqlCommand cmd = new SqlCommand(text2, conn))
                {
                    // Execute the command and log the # rows affected.
                    var rows = await cmd.ExecuteNonQueryAsync();
                    log.LogInformation($"{rows} rows were updated");
                }

                using (SqlCommand cmd = new SqlCommand(text3, conn))
                {
                    // Execute the command and log the # rows affected.
                    var rows = await cmd.ExecuteNonQueryAsync();
                    log.LogInformation($"{rows} rows were updated");
                }
            }
        }
    }
}
