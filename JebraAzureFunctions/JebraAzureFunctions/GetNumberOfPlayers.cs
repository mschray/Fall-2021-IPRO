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
    /// <summary>
    /// Used to collect the number of players signed into a course.
    /// Will not update in real time :(
    /// 
    /// We need to make sure users are removed from course_assignment when they close the tab.
    /// </summary>
    public static class GetNumberOfPlayers
    {

        [FunctionName("GetNumberOfPlayers")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Game Requests" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "course_id", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **course_id** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            string courseId = req.Query["course_id"];

            var command = $"SELECT user_id FROM course_assignment WHERE course_id = {courseId} AND user_id != ''";
            string requestBody = Tools.ExecuteQueryAsync(command).GetAwaiter().GetResult();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            int count = 0;
            foreach(var o in data)//Get count
            {
                count++;
            }

            NumberOfPlayersResponse res = new NumberOfPlayersResponse();
            res.NumberOfPlayers = count;

            return new OkObjectResult(JsonConvert.SerializeObject(res)); //Convert to json object
        }
    }
}

//New way to represent responses that I'm trying out.
class NumberOfPlayersResponse
{
    public int NumberOfPlayers;
}

