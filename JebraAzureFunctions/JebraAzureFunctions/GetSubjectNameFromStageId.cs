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
    public static class GetSubjectNameFromStageId
    {
        [FunctionName("GetSubjectNameFromStageId")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Stage Requests" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "stage_id", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "The **stage_id** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            int stageId = int.Parse(req.Query["stage_id"]);

            string res = Tools.ExecuteQueryAsync($@"
                SELECT TOP 1 subject.subject_name
                FROM subject
                INNER JOIN stage ON stage.id = {stageId}
                WHERE stage.subject_id = subject.id;
            ").GetAwaiter().GetResult();

            res = res.Substring(1, res.Length - 2);//Removes [ ] from ends. 

            return new OkObjectResult(res);
        }
    }
}

