using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JebraAzureFunctions.Models
{
    class StatisticModel : Attribute
    {
        [JsonProperty]
        public int id { get; set; }
        [JsonProperty]
        public int first_time_correct { get; set; }
        [JsonProperty]
        public int total_retries { get; set; }
        [JsonProperty]
        public float score { get; set; }
    }
}
