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
using System;

namespace JebraAzureFunctions
{
    /*
     * 	
        [{"cname":"Mr. Jebra Fun Class","code":"555444333","stage_id":5,"max_hp":500,"name":"Mega Basher","subject_id":2,"subject_name":"Simplify Exponents"}]
     */
    public static class EndGame
    {
        [FunctionName("EndGame")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Game Requests" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "courseId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "The **course.id** parameter")]
        [OpenApiParameter(name: "stageId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "The **stage.id** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = null)] HttpRequest req,
            ILogger log)
        {
            int courseId = int.Parse(req.Query["courseId"]);
            int stageId = int.Parse(req.Query["stageId"]);

            bool status1 = Tools.ExecuteNonQueryAsync($@"
                DELETE FROM course WHERE id = {courseId};").GetAwaiter().GetResult();

            bool status2 = Tools.ExecuteNonQueryAsync($"DELETE FROM stage WHERE id = {stageId};").GetAwaiter().GetResult();

            bool status3 = Tools.ExecuteNonQueryAsync($@"DELETE FROM course_assignment WHERE course_id = {courseId};").GetAwaiter().GetResult();

            return new OkObjectResult($"Request to end game response: {status1 && status2 && status3}");
        }
    }
}

