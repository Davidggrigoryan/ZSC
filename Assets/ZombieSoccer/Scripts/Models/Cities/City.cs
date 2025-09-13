using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ZombieSoccer.Models
{
    [Serializable]
    public class City
    {
        [JsonProperty("Matches")]
        public List<Match> matches { get; set; } = new List<Match>();
        [JsonProperty("Locations")]
        public List<LocationData> locations { get; set; } = new List<LocationData>();
        [JsonProperty("Name")]
        public string name { get; set; }
        [JsonProperty("Order")]
        public int order   { get; set; }        
        
    }
}
