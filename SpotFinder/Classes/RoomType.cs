using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SpotFinder.Classes
{
    public class RoomType
    {
        RoomType()
        {

        }

        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("type_name")]
        public string TypeName { get; set; }

    }
}