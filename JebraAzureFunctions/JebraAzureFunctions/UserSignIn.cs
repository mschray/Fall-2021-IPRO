using System;
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
    public static class UserSignIn
    {
        [FunctionName("UserSignIn")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "General Request" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "courseCode", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "Course code.")]
        [OpenApiParameter(name: "userEmail", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The user's email.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(UserSignInResponseModel), Description = "The OK response")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            int courseCode = int.Parse(req.Query["courseCode"]);
            string userEmail = req.Query["userEmail"];
            /*
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;
            */

            /*
             * Inputs: An email and corse code.
             * 1. Get course id from course code. 
             * 2. Get user id from email. 
             * 3. Get instructor id from course id. 
             * 4. Get stage id.
             * 5. Insert into course_assignment.
             */

            string courseIdS = Tools.ExecuteQueryAsync($"SELECT id FROM course WHERE code={courseCode}").GetAwaiter().GetResult();

            int courseId = Tools.GetIdFromResponse(courseIdS);

            string userIdS = Tools.ExecuteQueryAsync($@"
                IF EXISTS
                (
                    SELECT id FROM app_user WHERE email='{userEmail}'
                )
                    BEGIN
                        UPDATE app_user SET is_online=1 WHERE email='{userEmail}'
                    END
                ELSE
                    INSERT INTO app_user VALUES('{userEmail}', 1)
                
                SELECT id FROM app_user WHERE email='{userEmail}'
            ").GetAwaiter().GetResult();

            int userId = Tools.GetIdFromResponse(userIdS);

            string instructorIds = Tools.ExecuteQueryAsync($"SELECT instructor_id AS id FROM course_assignment WHERE course_id={courseId} AND user_id IS NULL").GetAwaiter().GetResult();
            int instructorId = Tools.GetIdFromResponse(instructorIds);

            //Get stage.id
            int stageId = -1;
            /* //IDK what I was thinking
            string stageIdS = Tools.ExecuteQueryAsync($@"
                SELECT TOP 1 stage_event_join.stage_id AS id
                FROM stage_event_join 
                INNER JOIN course ON stage_event_join.course_id = course.id
                WHERE course.code = {courseCode};
            ").GetAwaiter().GetResult();
            */
            string stageIdS = Tools.ExecuteQueryAsync($@"
                SELECT stage_id AS id
                FROM course 
                WHERE id = {courseId};
            ").GetAwaiter().GetResult();

            stageId = Tools.GetIdFromResponse(stageIdS);
            bool status = Tools.ExecuteNonQueryAsync($"INSERT INTO course_assignment (user_id, course_id, instructor_id) VALUES({userId},{courseId},{instructorId})").GetAwaiter().GetResult();

            UserSignInResponseModel res = new UserSignInResponseModel();
            res.courseId = courseId;
            res.userId = userId;
            res.instructorId = instructorId;
            res.stageId = stageId;

            return new OkObjectResult(res);
        }
    }
}

