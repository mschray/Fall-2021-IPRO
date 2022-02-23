using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using JebraAzureFunctions.Models;
using JebraAzureFunctions.Questions;
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
            int amountAdded = -1;
            switch (type)
            {
                case "Simplify Exponents":
                    amountAdded = Tools.InsertQuestionsAsync(Tools.GenerateUniqueQuestions(Tools.SimplifyExponents, amount, subjectId)).GetAwaiter().GetResult();
                    break;
                case "Simplify Square Roots":
                    amountAdded = Tools.InsertQuestionsAsync(Tools.GenerateUniqueQuestions(Tools.SimplifySquareRoots, amount, subjectId)).GetAwaiter().GetResult();
                    break;
                case "Simplify Exponents 2":
                    amountAdded = Tools.InsertQuestionsAsync(Tools.GenerateUniqueQuestions(Tools.SimplifyExponents2, amount, subjectId)).GetAwaiter().GetResult();
                    break;
                case "Factorials":
                    amountAdded = Tools.InsertQuestionsAsync(Tools.GenerateUniqueQuestions(Tools.Factorials, amount, subjectId)).GetAwaiter().GetResult();
                    break;
                case "Cartesian Coordinates":
                    amountAdded = Tools.InsertQuestionsAsync(Tools.GenerateUniqueQuestions(Tools.CartesianCoordinates, amount, subjectId)).GetAwaiter().GetResult();
                    break;
                case "Single Variable":
                    amountAdded = Tools.InsertQuestionsAsync(Tools.GenerateUniqueQuestions(Tools.SingleVariable, amount, subjectId)).GetAwaiter().GetResult();
                    break;
                case "System Of Equations":
                    amountAdded = Tools.InsertQuestionsAsync(Tools.GenerateUniqueQuestions(Tools.SystemOfEquations, amount, subjectId)).GetAwaiter().GetResult();
                    break;
                case "Quadratic Roots":
                    amountAdded = Tools.InsertQuestionsAsync(Tools.GenerateUniqueQuestions(Tools.QuadraticRoots, amount, subjectId)).GetAwaiter().GetResult();
                    break;
                case "Trig Functions":
                    amountAdded = Tools.InsertQuestionsAsync(Tools.GenerateUniqueQuestions(Trigonometry.TrigFunctions, amount, subjectId)).GetAwaiter().GetResult();
                    break;
                case "Inverse Trig Functions":
                    amountAdded = Tools.InsertQuestionsAsync(Tools.GenerateUniqueQuestions(Trigonometry.InverseTrigFunctions, amount, subjectId)).GetAwaiter().GetResult();
                    break;
                default:
                    amountAdded = -1;
                    break;
            }

            if(amountAdded > 0)
                response = $"Successfully inserted {amountAdded} questions. :)";
            else
                response = $"Failed to insert {amount} {type} questions. :(";

            return new OkObjectResult(response);
        }

    }

}


