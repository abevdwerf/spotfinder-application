using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;

namespace SpotFinder.Classes
{
    public class Floor
    {
        public Floor(string floorName, int floorLevel)
        {
            FloorName = floorName;
            FloorLevel = floorLevel;
        }

        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("location_id")]
        public int LocationId { get; set; }
        [JsonProperty("floor_name")]
        public string FloorName { get; set; }
        public int FloorLevel { get; set; }

        public List<Room> Rooms { get; set; }
    }
}
