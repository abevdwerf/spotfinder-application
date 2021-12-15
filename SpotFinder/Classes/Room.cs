using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SpotFinder.Classes
{
    public class Room
    {
        public Room()
        {

        }

        public Room(int floorId, string roomName, int maxPersons)
        {
            FloorId = floorId;
            RoomName = roomName;
            MaxPersons = maxPersons;
        }

        [JsonProperty("floor_id")]
        public int FloorId { get; set; }
        [JsonProperty("room_name")]
        public string RoomName { get; set; }
        [JsonProperty("max_persons")]
        public int MaxPersons { get; set; }

    }
}
