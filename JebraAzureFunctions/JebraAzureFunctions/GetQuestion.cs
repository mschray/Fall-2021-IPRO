using System.IO;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace JebraAzureFunctions
{
    public static class GetQuestion
    {
        [FunctionName("GetQuestion")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "id" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **id** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string id = req.Query["id"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            //id = data?.id;
            id = id ?? data?.id;

            log.LogInformation("C# HTTP trigger function processed a request.");

            string responseMessage = "Response: \n";

            var str = Environment.GetEnvironmentVariable("SqlConnectionString");
            //System.Diagnostics.Debug.WriteLine("Conn String: " + str);
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                System.Diagnostics.Debug.WriteLine($"id from body = {id}");
                var command = $"SELECT * FROM question WHERE id={id}";

                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    // Execute the command and log the # rows affected.
                    SqlDataReader rows = await cmd.ExecuteReaderAsync();
                    //while (rows.Read())
                    //{
                    //    responseMessage += $"{rows.GetValue(1)}, {rows.GetValue(3)} \n";
                    //}

                    responseMessage = tools.sqlDatoToJson(rows);
                }
            }
            return new OkObjectResult(responseMessage);
        }
    }
}

