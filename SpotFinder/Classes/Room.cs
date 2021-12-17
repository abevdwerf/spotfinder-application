using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;

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

        //public async Task<List<Floor>> GetFloors()
        //{
        //    List<Floor> floors = null;
        //    HttpResponseMessage response = await ApiHelper.Get($"api/floors");

        //    if (response.IsSuccessStatusCode)
        //    {
        //        floors = await response.Content.ReadAsAsync<List<Floor>>();
        //    }
        //    else
        //    {
        //        throw new Exception(response.ReasonPhrase);
        //    }

        //    return floors;
        //}
    }
}
