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

            string id = req.Query["id"];//Tbh, idk what this does.

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            id = id ?? data?.id;//Get id from url

            string responseMessage = "";

            var str = Environment.GetEnvironmentVariable("SqlConnectionString");
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                System.Diagnostics.Debug.WriteLine($"id from body = {id}");
                var command = $"SELECT * FROM question WHERE id={id}";

                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    SqlDataReader rows = await cmd.ExecuteReaderAsync();

                    responseMessage = tools.sqlDatoToJson(rows);//Convert object to JSON.
                }
            }
            return new OkObjectResult(responseMessage);
        }
    }
}

