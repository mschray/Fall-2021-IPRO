using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JebraAzureFunctions.Models
{
    class StageModel : Attribute
    {
        [JsonProperty]
        public int id { get; set; }
        [JsonProperty]
        public int max_hp { get; set; }
        [JsonProperty]
        public string name { get; set; }
        [JsonProperty]
        public string subj { get; set; }
    }
}
