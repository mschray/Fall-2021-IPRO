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
using JebraAzureFunctions.Models;
using System.Collections.Generic;

namespace JebraAzureFunctions
{
    /// <summary>
    /// Will return new stage id if monster is defeated.
    /// </summary>
    public static class GetEvents
    {
        [FunctionName("GetEvents")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Event Requests" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "stage", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **stage id** parameter")]
        [OpenApiParameter(name: "course_code", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **course** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            /*
             * If monster health = 0, reply with the new stage ID instead.
             * 
             */

            //log.LogInformation("C# HTTP trigger function processed a request.");

            string courseCode = req.Query["course_code"];
            string stage = req.Query["stage"];

            ///string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ///dynamic data = JsonConvert.DeserializeObject(requestBody);
            ///name = name ?? data?.name;
            ///   

            // Fetch info about stage and subject
            string currentStageCommand = $@"
            SELECT TOP 1 stage.id, stage.max_hp, stage.name, stage.subject_id, subject.subject_name
            FROM course
            INNER JOIN stage ON course.stage_id = stage.id
            INNER JOIN subject ON stage.subject_id = subject.id
            WHERE course.code={courseCode};
            ";
            string currentStageString = Tools.ExecuteQueryAsync(currentStageCommand).GetAwaiter().GetResult();
            currentStageString = currentStageString.Substring(1, currentStageString.Length - 2);
            dynamic currentStageData = JsonConvert.DeserializeObject(currentStageString);

            int currentStageId = currentStageData?.id;
            int maxHp = currentStageData?.max_hp;
            string stageName = currentStageData?.name;
            int subjectId = currentStageData?.subject_id;
            string subjectName = currentStageData?.subject_name;

            // If the currentStageId does not match that of the the incoming request, that means that this client is stuck on an old stage.
            // In this case, we respond with an EndOfStage message to let the client know to move to this new stage.
            if (currentStageId.ToString() != stage)
            {
                return new OkObjectResult(ConstructEndOfStageMessage(currentStageId, maxHp, stageName, subjectId, subjectName));
            }

            string command = @$"
            SELECT stage_event.id, stage_event.inflicted_hp, stage_event.was_correct, stage_event.event_time FROM stage_event
            INNER JOIN stage_event_join ON stage_event.id = stage_event_join.stage_event_id
            INNER JOIN course ON course.code = {courseCode}
            WHERE stage_event_join.course_id = course.id AND stage_event_join.stage_id = {stage}
            ";

            dynamic responseMessage = JsonConvert.DeserializeObject(Tools.ExecuteQueryAsync(command).GetAwaiter().GetResult());

            List<StageEventModel> events = Tools.JsonEventsToModelArray(responseMessage);

            int currentHp = maxHp;
            foreach(StageEventModel e in events)
            {
                currentHp -= e.inflicted_hp;
            }

            if(currentHp <= 0)
            {
                /*
                 * - Create new stage
                 * - Update course with new stage_id
                 * - Delete old stage
                 * - Ret new stage_id, new max hp, new stage name, new subject id, new subject name
                 */
                string newStageName = stageName + " - again!"; // Placeholder
                string newStageIdString = Tools.ExecuteQueryAsync($@"INSERT INTO stage OUTPUT INSERTED.id VALUES({maxHp}, '{newStageName}', {subjectId})").GetAwaiter().GetResult();
                //[{"id":2}]
                newStageIdString = newStageIdString.Substring(1, newStageIdString.Length - 2);
                //{"id":2}
                dynamic data = JsonConvert.DeserializeObject(newStageIdString);
                int newStageId = data?.id;

                Tools.ExecuteNonQueryAsync($"UPDATE course SET stage_id={newStageId} WHERE code={courseCode}").GetAwaiter().GetResult();

                Tools.ExecuteNonQueryAsync($"DELETE FROM stage WHERE id={stage}").GetAwaiter().GetResult();

                responseMessage = ConstructEndOfStageMessage(newStageId, maxHp, newStageName, subjectId, subjectName);
            }
            return new OkObjectResult(responseMessage);
        }

        // Note: we should use a model for EndOfStage messages. For now, just making a static function for the sake of time
        private static string ConstructEndOfStageMessage(int newStageId, int newMaxHp, string newStageName, int newSubjectId, string newSubjectName)
        {
            return $@"
            {{
                ""end_of_stage"": true,
                ""new_stage_id"": {newStageId},
                ""new_max_hp"": {newMaxHp},
                ""new_stage_name"": ""{newStageName}"",
                ""new_subject_id"": {newSubjectId},
                ""new_subject_name"": ""{newSubjectName}""
            }}
            ";
        }
    }
}

