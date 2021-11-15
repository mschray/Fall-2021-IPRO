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
    public static class NewGame
    {
        [FunctionName("NewGame")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Game Requests" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "subjectName", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **subjectName** parameter")]
        [OpenApiParameter(name: "courseName", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **courseName** parameter")]
        [OpenApiParameter(name: "instructorEmail", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **instructorEmail** parameter")]
        [OpenApiParameter(name: "stageHp", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "The **stageHp** parameter")]
        [OpenApiParameter(name: "stageName", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **stageName** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string subjectName = req.Query["subjectName"];
            string courseName = req.Query["courseName"];
            string instructorEmail = req.Query["instructorEmail"];
            int stageHp = int.Parse(req.Query["stageHp"]);
            string stageName = req.Query["stageName"];

            Console.WriteLine("0");

            //Two lines for readability
            string instructorIdS = Tools.ExecuteQueryAsync($"SELECT id FROM instructor WHERE email = '{instructorEmail}'").GetAwaiter().GetResult(); 
            int instructorId = Tools.GetIdFromResponse(instructorIdS);

            Console.WriteLine("0.5");

            string subjectIdS = Tools.ExecuteQueryAsync($"SELECT id FROM subject WHERE subject_name = '{subjectName}'").GetAwaiter().GetResult();

            Console.WriteLine(subjectIdS);

            int subjectId = Tools.GetIdFromResponse(subjectIdS);

            Console.WriteLine("1");

            /*
             * TODO:
             * Make course code
             * Create course
             * Create stage + assign stage_id to course
             * Create course assignment
             * Return course
             */

            //Generate course code
            //int courseCode = Tools.GetRandomIntInRange(100000000,999999999);//9 digits long //BROKEN
            //Random didnt want to generate a 9 digit random. :(
  
            int courseCode = 555444333;

            //Insert course
            int courseId = Tools.GetIdFromResponse(Tools.ExecuteQueryAsync($"INSERT INTO course (cname, code) OUTPUT INSERTED.id VALUES ('{courseName}', {courseCode})").GetAwaiter().GetResult());


            Console.WriteLine("2");

            //Insert stage 
            int stageId = Tools.GetIdFromResponse(Tools.ExecuteQueryAsync($@"
            INSERT INTO stage 
            OUTPUT INSERTED.id 
            VALUES ({stageHp}, '{stageName}', {subjectId})").GetAwaiter().GetResult());


            Console.WriteLine("3");

            //Add stage_id to course
            bool status = Tools.ExecuteNonQueryAsync($@"UPDATE course SET stage_id = {stageId} WHERE id = {courseId}").GetAwaiter().GetResult();

            Console.WriteLine("4");

            //Course assignment
            status = Tools.ExecuteNonQueryAsync($@"INSERT INTO course_assignment(instructor_id, course_id) VALUES ({instructorId}, {courseId})").GetAwaiter().GetResult();


            Console.WriteLine("5");

            string responseMessage = Tools.ExecuteQueryAsync($@"
            SELECT course.id AS course_id, course.cname, course.code, course.stage_id, stage.max_hp, stage.name, stage.subject_id, subject.subject_name
            FROM course     
            INNER JOIN stage ON course.stage_id = stage.id
            INNER JOIN subject ON subject.id = {subjectId}
            WHERE course.id = {courseId}
            ").GetAwaiter().GetResult();

            return new OkObjectResult(responseMessage);
        }
    }
}

