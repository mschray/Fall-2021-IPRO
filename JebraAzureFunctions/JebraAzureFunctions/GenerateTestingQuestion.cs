using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using JebraAzureFunctions.Models;
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

            
            string subjectIdString = Tools.ExecuteQueryAsync($"SELECT id FROM subject WHERE subject_name='{type}'").GetAwaiter().GetResult();
            //[{"id":2}]
            subjectIdString = subjectIdString.Substring(1, subjectIdString.Length-2);
            //{"id":2}
            dynamic data = JsonConvert.DeserializeObject(subjectIdString);
            int subjectId = -1;
            subjectId = data?.id;

            string questionsS = Tools.ExecuteQueryAsync($"SELECT * FROM question WHERE subject_id='{subjectId}'").GetAwaiter().GetResult();
            dynamic questionListD = JsonConvert.DeserializeObject(questionsS);
            List<QuestionModel> questionList = Tools.JsonQuestionsToModelArray(questionListD);
            

            string command = "";
            switch (type)
            {
                case "Simplify Exponents":
                    for (int i = 0; i < amount; i++)
                    {
                        QuestionModel question = Tools.SimplifyExponents();
                        int maxPossible = 9;//Im afraid this has to be hard-coded.
                        int count = 0;
                        while (Tools.UniqueQuestion(question, questionList) == false && count < maxPossible)
                        {
                            question = Tools.SimplifyExponents();
                            count++;
                        }
                        if(count<maxPossible)
                        {
                            questionList.Add(question);
                            command += $"INSERT INTO question VALUES('{question.answer_a}', null, '{question.question}', {subjectId}) \n";
                        }
                    }
                    break;
                case "Simplify Square Roots":
                    for (int i = 0; i < amount; i++)
                    {
                        QuestionModel question = Tools.SimplifySquareRoots();
                        while (Tools.UniqueQuestion(question, questionList) == false)
                        {
                            question = Tools.SimplifySquareRoots();
                        }
                        command += $"INSERT INTO question VALUES('{question.answer_a}', '{question.answer_b}', '{question.question}', {subjectId}) \n";
                    }
                    break;
                default:
                    // code block
                    break;
            }

            //Console.WriteLine(command);

            Tools.ExecuteNonQueryAsync(command);

            return new OkObjectResult("Request to add questions sent.");
        }

    }

}


