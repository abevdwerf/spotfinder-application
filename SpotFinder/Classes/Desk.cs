using System;
using System.Collections.Generic;
using System.Text;

namespace SpotFinder.Classes
{
    public class Desk
    {
        public Desk()
        {

        }
        
        public int AvailableSpaces { get; set; }
        public bool WallOutlet { get; set; }
        public int SensorId { get; set; }
        public int RoomType { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int RoomId { get; set; }
    }
}
