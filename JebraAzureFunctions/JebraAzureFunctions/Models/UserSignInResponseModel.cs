using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace JebraAzureFunctions.Models
{
    class UserSignInResponseModel : Attribute
    {
        [JsonProperty]
        public int courseId { get; set; }
        [JsonProperty]
        public int userId { get; set; }
        [JsonProperty]
        public int instructorId { get; set; }
        [JsonProperty]
        public int stageId { get; set; }
    }
}
