using SpotFinder.Classes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SpotFinder.UserControls;

namespace SpotFinder.Pages
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();
            DataContext = this;
            // LoadDashboard(0)
            LoadReservations();
        }

        // Get a list of reservation objects
        public async Task<List<Reservation>> GetReservations()
        {
            List<Reservation> reservations = null;
            HttpResponseMessage response = await ApiHelper.Get("api/reservations");

            if (response.IsSuccessStatusCode)
            {
                reservations = await response.Content.ReadAsAsync<List<Reservation>>();
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }

            return reservations;
        }

        public async Task<List<Location>> GetLocations()
        {
            List<Location> locations = null;
            HttpResponseMessage response = await ApiHelper.Get("api/locations");

            if (response.IsSuccessStatusCode)
            {
                locations = await response.Content.ReadAsAsync<List<Location>>();
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }

            return locations;
        }

        public async Task<List<Floor>> GetFloors()
        {
            List<Floor> floors = null;
            HttpResponseMessage response = await ApiHelper.Get("api/floors");

            if (response.IsSuccessStatusCode)
            {
                floors = await response.Content.ReadAsAsync<List<Floor>>();
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }

            return floors;
        }

        private async Task<List<Room>> GetRooms()
        {
            List<Room> rooms = null;
            HttpResponseMessage response = await ApiHelper.Get("api/rooms");

            if (response.IsSuccessStatusCode)
            {
                rooms = await response.Content.ReadAsAsync<List<Room>>();
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }

            return rooms;
        }

        public async Task<List<RoomType>> GetRoomTypes()
        {
            List<RoomType> roomTypes = null;
            HttpResponseMessage response = await ApiHelper.Get("api/roomtypes");

            if (response.IsSuccessStatusCode)
            {
                roomTypes = await response.Content.ReadAsAsync<List<RoomType>>();
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }

            return roomTypes;
        }

        private async Task<List<User>> GetUsers()
        {
            List<User> users = null;
            HttpResponseMessage response = await ApiHelper.Get("api/users");

            if (response.IsSuccessStatusCode)
            {
                users = await response.Content.ReadAsAsync<List<User>>();
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }

            return users;
        }

        private async void LoadReservations()
        {
            DateTime today = DateTime.Today;
            DateTime oneDayLater = today.AddDays(1);
            DateTime twoDaysLater = today.AddDays(2);
            int counter = 0;    // Counts amount of blockreservations added
            int reservationCounter = 0;
            int userCounter = 0;

            foreach (Reservation reservation in await GetReservations())
            {

                BlockReservation blockReservation = new BlockReservation();
                reservationCounter++;

                blockReservation.BeginTime = reservation.ReservationStart.ToShortTimeString();
                blockReservation.EndTime = reservation.ReservationEnd.ToShortTimeString();

                foreach (Room room in await GetRooms())
                {
                    if (room.Id == reservation.RoomId)
                    {
                        blockReservation.Room = room.RoomName;

                        foreach (RoomType roomType in await GetRoomTypes())
                        {
                            if (room.RoomTypeId == roomType.Id)
                                blockReservation.RoomType = roomType.TypeName;
                        }

                        foreach (Floor floor in await GetFloors())
                        {
                            if (floor.Id == room.FloorId)
                            {
                                foreach (Location location in await GetLocations())
                                {
                                    if (location.Id == floor.LocationId)
                                        blockReservation.Building = location.LocationName;
                                }
                            }
                        }
                    }
                }

                foreach (User user in await GetUsers())
                {
                    userCounter++;
                }

                if(counter < 5)
                {
                    wpReservations.Children.Add(blockReservation);
                    counter++;
                }
            }

            ReservationSum.Text = reservationCounter.ToString();
            UsersSum.Text = userCounter.ToString();

            // Amount of spots available
            foreach(Location location in await GetLocations())
            {
                AvailableWorkSpots availableWorkSpots = new AvailableWorkSpots();
                availableWorkSpots.Building = location.LocationName;
                int capacityCounter = 0;
                
                foreach(Room room in await GetRooms())
                {
                    foreach(Floor floor in await GetFloors())
                    {
                        if(room.Id == floor.Id && floor.Id == location.Id)
                        {
                            capacityCounter += room.MaxPersons;
                        }
                    }
                }

                availableWorkSpots.Available = capacityCounter.ToString();
                availableList.Children.Add(availableWorkSpots);
            }
        }


        // For each object, create a user control
        //private async void LoadReservations()
        //{
        //    foreach (Reservation reservation in await GetReservations())
        //    {
        //        UserControl reservationUc = new UserControls.BlockReservation() { Building="building", Room=reservation.RoomId, RoomType="RoomType", Time=reservation.reservationStart, User="user" };
        //        spRoomContent.Children.Add(reservationUc);
        //    }
        //}
    }
}