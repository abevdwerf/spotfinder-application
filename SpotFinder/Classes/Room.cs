using System;
using System.Collections.Generic;
using System.Text;

namespace SpotFinder.Classes
{
    public class Room
    {
        public Room()
        {

        }
        
        public string RoomName { get; set; }
        public int RoomFloor { get; set; }
        public int MaxPersons { get; set; }
    }
}
