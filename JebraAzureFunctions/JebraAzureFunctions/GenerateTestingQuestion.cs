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

           
            int subjectId = Tools.GetSubjectIdFromString(type);

            string questionsS = Tools.ExecuteQueryAsync($"SELECT * FROM question WHERE subject_id='{subjectId}'").GetAwaiter().GetResult();
            dynamic questionListD = JsonConvert.DeserializeObject(questionsS);
            List<QuestionModel> questionList = Tools.JsonQuestionsToModelArray(questionListD);


            string response = "";
            bool status = false;
            switch (type)
            {
                case "Simplify Exponents":
                    status = Tools.InsertQuestionsAsync(Tools.GenerateUniqueQuestions(Tools.SimplifyExponents, amount, subjectId)).GetAwaiter().GetResult();
                    break;
                case "Simplify Square Roots":
                    status = Tools.InsertQuestionsAsync(Tools.GenerateUniqueQuestions(Tools.SimplifySquareRoots, amount, subjectId)).GetAwaiter().GetResult();
                    break;
                case "Simplify Exponents 2":
                    status = Tools.InsertQuestionsAsync(Tools.GenerateUniqueQuestions(Tools.SimplifyExponents2, amount, subjectId)).GetAwaiter().GetResult();
                    break;
                case "Factorials":
                    status = Tools.InsertQuestionsAsync(Tools.GenerateUniqueQuestions(Tools.Factorials, amount, subjectId)).GetAwaiter().GetResult();
                    break;
                case "Cartesian Coordinates":
                    status = Tools.InsertQuestionsAsync(Tools.GenerateUniqueQuestions(Tools.CartesianCoordinates, amount, subjectId)).GetAwaiter().GetResult();
                    break;
                case "Single Variable":
                    status = Tools.InsertQuestionsAsync(Tools.GenerateUniqueQuestions(Tools.SingleVariable, amount, subjectId)).GetAwaiter().GetResult();
                    break;
                case "System Of Equations":
                    status = Tools.InsertQuestionsAsync(Tools.GenerateUniqueQuestions(Tools.SystemOfEquations, amount, subjectId)).GetAwaiter().GetResult();
                    break;
                case "Quadratic Roots":
                    status = Tools.InsertQuestionsAsync(Tools.GenerateUniqueQuestions(Tools.QuadraticRoots, amount, subjectId)).GetAwaiter().GetResult();
                    break;
                case "Trig Functions":
                    status = Tools.InsertQuestionsAsync(Tools.GenerateUniqueQuestions(Tools.TrigFunctions, amount, subjectId)).GetAwaiter().GetResult();
                    break;
                default:
                    status = false;
                    break;
            }

            if(status)
                response = $"Successfully requested to insert {amount} {type} questions. :)";
            else
                response = $"Failed to insert {amount} {type} questions. :(";

            return new OkObjectResult(response);
        }

    }

}


