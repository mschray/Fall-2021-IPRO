using System;
using System.Collections.Generic;
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
    public static class GenerateTestingQuestions
    {
        [FunctionName("GenerateTestingQuestions")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Development Requests" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "amount", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "Number of questions to generate.")]
        [OpenApiParameter(name: "type", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "Type of questions to generate.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            string amountS = req.Query["amount"];
            string type = req.Query["type"];

            int amount = int.Parse(amountS);

            int subjectId = -1;

            /*
            var str = Environment.GetEnvironmentVariable("SqlConnectionString");
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                var command = $"SELECT id FROM subject WHERE subject_name='{type}'";

                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    SqlDataReader rows = await cmd.ExecuteReaderAsync();
                    //subjectId = int.Parse(Tools.SqlDatoToJson(rows));//Convert object to JSON.
                    //Console.WriteLine(Tools.SqlDatoToJson(rows)); //[{"id":2}]

                    subjectId = int.Parse(rows.GetValue(0).ToString());
                }
            }
            */

            string command = "";
            switch (type)
            {
                case "Simplify Exponents":
                    subjectId = 2;
                    for (int i = 0; i < amount; i++)
                    {
                        QuestionModel question = Tools.SimplifyExponents();
                        command += $"INSERT INTO question VALUES({question.answer_a}, null, '{question.question}', {subjectId}) \n";
                    }
                    break;
                case "Simplify Square Roots":
                    subjectId = 4;
                    for (int i = 0; i < amount; i++)
                    {
                        QuestionModel question = Tools.SimplifySquareRoots();
                        command += $"INSERT INTO question VALUES({question.answer_a}, {question.answer_b}, '{question.question}', {subjectId}) \n";
                    }
                    break;
                default:
                    // code block
                    break;
            }

            Tools.ExecuteNonQueryAsync(command);

            return new OkObjectResult("Request to add questions sent.");
        }
            
    }

}


