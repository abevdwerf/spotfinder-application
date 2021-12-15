using System;
using System.Collections.Generic;
using System.Text;

namespace SpotFinder.Classes
{
    public class Floor
    {
        public Floor()
        {

        }

        public Floor(string floorName, int floorLevel)
        {
            floor_name = floorName;
            FloorLevel = floorLevel;

        }

        public string floor_name { get; set; }//is gelijk aan de response parameter
        public int FloorLevel { get; set; }
    }
}
