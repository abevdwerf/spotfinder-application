using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SpotFinder.Classes
{
    public class Floor
    {
        public Floor()
        {

        }

        public Floor(string floorName, int floorLevel)
        {
            FloorName = floorName;
            FloorLevel = floorLevel;

        }

        [JsonProperty("floor_name")]
        public string FloorName { get; set; }
        public int FloorLevel { get; set; }
    }
}
