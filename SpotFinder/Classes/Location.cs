using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SpotFinder.Classes
{
    public class Location
    {
        public Location()
        {

        }

        public Location(string locationName)
        {
            LocationName = locationName;
        }

        [JsonProperty("location_name")]
        public string LocationName { get; set; }
    }
}
