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
    public static class InstructorLogin
    {
        /*
         * Due to time considerations, this function is incredibly insecure and cuts corners in many ways in order to make a demo possible.
         * 
         * ONLY username AND pass FIELDS ARE REQUIRED!
         * 
         * If login was correct/instructor exists, the response will be the instructors entry in the instructor table. Else, the response will be "[{"Column1":-1}]"
         */


        [FunctionName("InstructorLogin")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Instructor Requests" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBodyAttribute(contentType: "string", bodyType: typeof(InstructorModel))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            string username = req.Query["username"];
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            username = username ?? data?.username;

            string password = req.Query["pass"];
            data = JsonConvert.DeserializeObject(requestBody);
            password = password ?? data?.pass;

            string instructor = Tools.ExecuteQueryAsync($@"
            IF EXISTS(
            SELECT * FROM instructor WHERE username = '{username}' AND pass = '{password}'
            )
            BEGIN
                SELECT * FROM instructor WHERE username = '{username}' AND pass = '{password}'
            END
            ELSE
            BEGIN
                SELECT -1;
            END
            ").GetAwaiter().GetResult();

            return new OkObjectResult(instructor);
        }
    }
}

