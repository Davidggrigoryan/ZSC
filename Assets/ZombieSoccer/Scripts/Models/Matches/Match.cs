using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieSoccer.Models
{
    [System.Serializable]
    public class Match
    {
        [JsonProperty("Id")]
        public string id { get; set; }
        [JsonProperty("Order")]
        public int order { get; set; }
        [JsonProperty("PowerScore")]
        public int powerScore { get; set; }
    }
}
