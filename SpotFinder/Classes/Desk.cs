using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SpotFinder.Classes
{
    public class Desk
    {
        public Desk()
        {

        }

        public Desk(int availableSpaces, int sensorId, int roomId)
        {
            AvailableSpaces = availableSpaces;
            RoomId = roomId;
        }

        [JsonProperty("available_spaces")]
        public int AvailableSpaces { get; set; }
        [JsonProperty("room_id")]
        public int RoomId { get; set; }
        [JsonProperty("module_id")]
        public int ModuleId { get; set; }

    }
}
