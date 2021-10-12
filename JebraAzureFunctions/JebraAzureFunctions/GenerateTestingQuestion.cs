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
        [OpenApiParameter(name: "number", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "Number of questions to generate.")]
        [OpenApiParameter(name: "type", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "Type of questions to generate.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            string amountS = req.Query["number"];
            string typeS = req.Query["type"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            amountS = amountS ?? data?.number;
            int amount = int.Parse(amountS);

            typeS = typeS ?? data?.number;
            string type = typeS;

            Tools.ExecuteNonQueryAsync(amount, type);

            string responseMessage = $"A request to add {amount} questions of {type} type for development testing has been sent.";

            return new OkObjectResult(responseMessage);
        }
            
    }

}


