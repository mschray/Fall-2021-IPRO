using System;
using System.Data.SqlClient;
using System.IO;
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

namespace JebraAzureFunctions
{
    public static class AddQuestion
    {
        [FunctionName("AddQuestion")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "AddQuestion" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBodyAttribute(contentType: "string", bodyType: typeof(QuestionModel))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            //log.LogInformation("C# HTTP trigger function processed a request.");
            //Console.WriteLine("AddQuestion Called!");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);//Convert the body we received into an object.
            //name = name ?? data?.name;
            //int x = data?.answer_a;

            //Run SQL Insert
            var str = Environment.GetEnvironmentVariable("SqlConnectionString");
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();

                var command = $"INSERT INTO question(answer_a, answer_b, question) VALUES({data?.answer_a},{data?.answer_b},'{data?.question}')";
                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    int exeTask = await cmd.ExecuteNonQueryAsync();
                }
            }

            return new OkObjectResult($"Question insert requested with following parameters: \n {requestBody}");
        }
    }
}
 
