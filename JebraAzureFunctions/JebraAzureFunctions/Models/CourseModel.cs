using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JebraAzureFunctions.Models
{
    class CourseModel : Attribute
    {
        [JsonProperty]
        public int id { get; set; }
        [JsonProperty]
        public string cname { get; set; }
        [JsonProperty]
        public string code { get; set; }
        [JsonProperty]
        public int stage_id { get; set; }
    }
}
