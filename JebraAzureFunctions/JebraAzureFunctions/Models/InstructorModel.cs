using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JebraAzureFunctions.Models
{
    class InstructorModel : Attribute
    {
        [JsonProperty]
        public int id { get; set; }
        [JsonProperty]
        public string fname { get; set; }
        [JsonProperty]
        public string lname { get; set; }
        [JsonProperty]
        public string username { get; set; }
        [JsonProperty]
        public string pass { get; set; }
        [JsonProperty]
        public string email { get; set; }
    }
}
