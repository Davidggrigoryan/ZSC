using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ZombieSoccer.Models
{
    [Serializable]
    public class LocationData
    {
        [JsonProperty("ImagePath")]
        public string imagePath { get; set; }
        [JsonProperty("Points")]
        public List<LocationPoint> points { get; set; } = new List<LocationPoint>();
        [JsonProperty("Order")]
        public int order { get; set; }        

    }
    
    [Serializable]
    public class LocationPoint
    {
        [JsonProperty("X")]
        public float x   { get; set; }     
        [JsonProperty("Y")]
        public float y   { get; set; }     
        [JsonProperty("Order")]
        public int order   { get; set; }     
    }
}