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
    public static class DropPlayer
    {
        [FunctionName("DropPlayer")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Game Requests" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **userId** parameter")]
        [OpenApiParameter(name: "courseId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **course_id** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = null)] HttpRequest req,
            ILogger log)
        {
            string userId = req.Query["userId"];
            string courseId = req.Query["courseId"];

            var command = $"DELETE FROM course_assignment WHERE user_id = {userId} AND course_id = {courseId}";
            string requestBody = Tools.ExecuteQueryAsync(command).GetAwaiter().GetResult();

            return new OkObjectResult("OK"); //Convert to json object
        }
    }
}

