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

namespace JebraAzureFunctions
{
    public static class GetEvents
    {
        [FunctionName("GetEvents")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Event Requests" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "stage", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **stage** parameter")]
        [OpenApiParameter(name: "course_code", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **course** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            //log.LogInformation("C# HTTP trigger function processed a request.");

            string courseCode = req.Query["course_code"];
            string stage = req.Query["stage"];

            ///string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ///dynamic data = JsonConvert.DeserializeObject(requestBody);
            ///name = name ?? data?.name;

            string command = @$"
            SELECT stage_event.inflicted_hp, stage_event.was_correct, stage_event.event_time FROM stage_event
            INNER JOIN stage_event_join ON stage_event.id = stage_event_join.stage_event_id
            INNER JOIN course ON course.code = {courseCode}
            WHERE stage_event_join.course_id = course.id AND stage_event_join.stage_id = {stage}
            ";

            string responseMessage = Tools.ExecuteQueryAsync(command).GetAwaiter().GetResult();

            return new OkObjectResult(responseMessage);
        }
    }
}

