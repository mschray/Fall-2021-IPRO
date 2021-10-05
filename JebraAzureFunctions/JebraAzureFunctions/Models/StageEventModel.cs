using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JebraAzureFunctions.Models
{
    class StageEventModel : Attribute
    {
        [JsonProperty]
        public int id { get; set; }
        [JsonProperty]
        public int inflicted_hp { get; set; }
        [JsonProperty]
        public bool was_correct { get; set; }
        [JsonProperty]
        public string event_time { get; set; }
    }
}
