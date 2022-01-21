using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SpotFinder.Classes
{
    public class Reservation
    {
        public Reservation(DateTime reservationStart, DateTime reservationEnd, int roomId)
        {
            ReservationStart = reservationStart;
            ReservationEnd = reservationEnd;
            RoomId = roomId;
        }

        [JsonProperty("user_id")]
        public int UserId { get; set; }
        [JsonProperty("reservation_start")]
        public DateTime ReservationStart { get; set; }
        [JsonProperty("reservation_end")]
        public DateTime ReservationEnd { get; set;}
        [JsonProperty("room_id")]
        public int RoomId { get; set; }
    }
}
