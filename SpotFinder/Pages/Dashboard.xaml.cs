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
        public Dashboard(Location currentLocation, string Username)
        {
            InitializeComponent();
            tbUsername.Text = Username;
            LoadReservations(currentLocation);
            
        }

        private List<Reservation> reservationsList;
        private List<Room> roomList;
        private List<RoomType> roomTypeList;
        private List<User> userList;

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

        private async void LoadReservations(Location location )
        {
            int counter = 0;    // Counts amount of blockreservations added
            int reservationCounter = 0;

            reservationsList = await GetReservations();
            roomList = await GetRooms();
            roomTypeList = await GetRoomTypes();
            userList = await GetUsers();

            foreach (Reservation reservation in reservationsList)
            {
                if (counter < 5)
                {
                    foreach (Room room in roomList)
                    {
                        if (room.Id == reservation.RoomId)
                        {
                            foreach (Floor floor in location.Floors)
                            {
                                if (floor.Id == room.FloorId)
                                {
                                    if (location.Id == floor.LocationId)
                                    {
                                        reservationCounter++;
                                        BlockReservation blockReservation = new BlockReservation();

                                        blockReservation.BeginTime = reservation.ReservationStart.ToShortTimeString();
                                        blockReservation.EndTime = reservation.ReservationEnd.ToShortTimeString();
                                        blockReservation.Room = room.RoomName;
                                        blockReservation.Building = location.LocationName;

                                        foreach (RoomType roomType in roomTypeList)
                                        {
                                            if (room.RoomTypeId == roomType.Id)
                                            {
                                                blockReservation.RoomType = roomType.TypeName;
                                                break;
                                            }
                                        }

                                        foreach (User user in userList)
                                        {
                                            if (user.Id == reservation.UserId)
                                            {
                                                blockReservation.User = user.Name;
                                                break;
                                            }
                                        }

                                        wpReservations.Children.Add(blockReservation);
                                        counter++;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            ReservationSum.Text = reservationCounter.ToString();
            UsersSum.Text = userList.Count.ToString();

            // Amount of spots available
            AvailableWorkSpots availableWorkSpots = new AvailableWorkSpots();
            availableWorkSpots.Building = location.LocationName;
            int capacityCounter = 0;

            foreach (Room room in roomList)
            {
                foreach (Floor floor in location.Floors)
                {
                    if (room.Id == floor.Id && floor.Id == location.Id)
                    {
                        capacityCounter += room.MaxPersons;
                    }
                }
            }

            availableWorkSpots.Available = capacityCounter.ToString();
            availableList.Children.Add(availableWorkSpots);
        }
    }
}