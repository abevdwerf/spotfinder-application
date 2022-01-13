using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpotFinder.Classes
{
    class Module
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("desk_id")]
        public int DeskId { get; set; }
        [JsonProperty("module_name")]
        public string ModuleName { get; set; }

        public Module()
        {

        }
    }
}
