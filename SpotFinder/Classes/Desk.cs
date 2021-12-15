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

        public Desk(int availableSpaces, bool wallOutlet, int sensorId, int roomType, int x, int y, int roomId)
        {
            AvailableSpaces = availableSpaces;
            WallOutlet = wallOutlet;
            SensorId = sensorId;
            RoomType = roomType;
            X = x;
            Y = y;
            RoomId = roomId;
        }

        [JsonProperty("available_spaces")]
        public int AvailableSpaces { get; set; }
        [JsonProperty("wall_outlet")]
        public bool WallOutlet { get; set; }
        public int SensorId { get; set; }
        public int RoomType { get; set; }
        [JsonProperty("x")]
        public int X { get; set; }
        [JsonProperty("y")]
        public int Y { get; set; }
        [JsonProperty("room_id")]
        public int RoomId { get; set; }
    }
}
