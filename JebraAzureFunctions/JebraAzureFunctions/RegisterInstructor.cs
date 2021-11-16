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
    public static class RegisterInstructor
    {
        [FunctionName("RegisterInstructor")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Instructor Requests" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBodyAttribute(contentType: "string", bodyType: typeof(InstructorModel))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            string username = req.Query["username"];
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            username = username ?? data?.username;

            string password = req.Query["pass"];
            data = JsonConvert.DeserializeObject(requestBody);
            password = password ?? data?.pass;

            string fname = req.Query["fname"];
            data = JsonConvert.DeserializeObject(requestBody);
            fname = fname ?? data?.fname;

            string lname = req.Query["lname"];
            data = JsonConvert.DeserializeObject(requestBody);
            lname = lname ?? data?.lname;

            string email = req.Query["email"];
            data = JsonConvert.DeserializeObject(requestBody);
            email = email ?? data?.email;

            string responseMessage = Tools.ExecuteQueryAsync($@"
            INSERT INTO instructor (fname, lname, username, pass, email)
            OUTPUT INSERTED.id
            VALUES
            ('{fname}','{lname}','{username}','{password}','{email}')
            ").GetAwaiter().GetResult();

            return new OkObjectResult(responseMessage);
        }
    }
}

