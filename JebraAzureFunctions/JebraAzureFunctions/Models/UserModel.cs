using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JebraAzureFunctions.Models
{
    class UserModel : Attribute
    {
        [JsonProperty]
        public int id { get; set; }
        [JsonProperty]
        public string email { get; set; }
        [JsonProperty]
        public bool isOnline { get; set; }
    }
}
