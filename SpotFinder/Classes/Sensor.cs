using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;


namespace SpotFinder.Classes
{
    public class Sensor
    {
        public Sensor()
        {

        }

        public Sensor(string sensorName, bool occupied)
        {
            SensorName = sensorName;
            Occupied = occupied;
        }

        [JsonProperty("sensor_name")]
        public string SensorName { get; set; }
        [JsonProperty("occupied")]
        public bool Occupied { get; set; }
    }
}
