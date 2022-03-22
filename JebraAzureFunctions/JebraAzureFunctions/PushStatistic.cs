using System;
using System.Collections.Generic;
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
    public static class PushStatistic
    {
        [FunctionName("PushStatistic")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "course_id", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The course_id to gather input about a user with.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string course_id = req.Query["course_id"];
            /*
             * Steps:
             * - Get subject_id
             * - Grab all users associated with that course_id
             * - For Each user:
             *  - Use params to get all stage events related to that user in that course.
             *  - Count the number of correct and incorrect questions.
             *  - Sum the inflicted_hp
             *  - Insert into the collected data into the statistic table with the current date.
             */

            //Get subject
            dynamic subectRes = JsonConvert.DeserializeObject(Tools.GetSubjectIdFromCourseId(int.Parse(course_id)).GetAwaiter().GetResult());
            int subject_id = subectRes.subject_id;

            //Tested below command: Works
            //Get users
            dynamic allUsers = JsonConvert.DeserializeObject(Tools.GetAllUsersInCourse(int.Parse(course_id)).GetAwaiter().GetResult());

            List<StatisticModel> toPush = new List<StatisticModel>();

            //For each user:
            foreach (var user in allUsers)
            {
                //res = obj.ToString();

                StatisticModel stat = new StatisticModel();

                //Get all stage events for this user in that course:
                string command = @$"
                    SELECT stage_event.inflicted_hp, stage_event.was_correct FROM stage_event 
                    INNER JOIN stage_event_join ON stage_event.id = stage_event_join.stage_event_id
                    WHERE stage_event_join.course_id = {course_id} and stage_event.join.origin_user_id = {user.id});
                ";

                dynamic eventData = JsonConvert.DeserializeObject(Tools.ExecuteQueryAsync(command).GetAwaiter().GetResult());
                
                int correctAttempt = 0;
                int incorrectAttempt = 0;
                int score = 0;
                foreach(var e in eventData)
                {
                    score += int.Parse(e.inflicted_hp);
                    if(string.Equals(e.was_correct, "1"))
                    {
                        correctAttempt++;
                    }
                    else
                    {
                        incorrectAttempt--;
                    }
                }

                stat.score = score;
                stat.subject_id = subject_id;
                stat.user_id = user.id;
                stat.correct_attempt = correctAttempt;
                stat.incorrect_attempt = incorrectAttempt;
                stat.game_date = DateTime.UtcNow.ToString("MM-dd-yyyy");//May need to change to meet Azure's expectations
                toPush.Add(stat);
            }

            //Statistic: id score subject_id user_id correct_attempt incorrect_attempt game_date

            //TODO: Send toPush statistics to the cloud

            return new OkObjectResult("");
        }
    }
}

