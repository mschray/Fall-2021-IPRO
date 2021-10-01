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

namespace JebraAzureFunctions
{
    public static class QueryExample
    {
        [FunctionName("QueryExample")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string responseMessage = "Response: ";

            var str = Environment.GetEnvironmentVariable("SqlConnectionString");
            System.Diagnostics.Debug.WriteLine("Conn String: " + str);
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                var text3 = "SELECT * FROM app_user";

                using (SqlCommand cmd = new SqlCommand(text3, conn))
                {
                    // Execute the command and log the # rows affected.
                    SqlDataReader rows = await cmd.ExecuteReaderAsync();
                    while (rows.Read())
                    {
                        responseMessage += $"Data: {rows.GetValue(1)}, {rows.GetValue(2)} \n";
                    }
                }
            }

            return new OkObjectResult(responseMessage);

        }

    }
}
