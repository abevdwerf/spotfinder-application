using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SpotFinder.Classes;
using SpotFinder.UserControls;
using System.Net.Http;
using System.Threading.Tasks;
using System.Globalization;

namespace SpotFinder.Pages
{
    /// <summary>
    /// Interaction logic for Reservations.xaml
    /// </summary>
    public partial class Reservations : Page
    {
        public Reservations()
        {
            InitializeComponent();
            LoadReservations();
        }

        private List<Reservation> reservationsList;
        private List<Room> roomList;
        private List<RoomType> roomTypeList;
        private List<Floor> floorList;
        private List<Location> locationList;
        private List<User> userList;

        private async Task<List<Reservation>> GetReservations()
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

        private async Task<List<Location>> GetLocations()
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

        private async Task<List<Floor>> GetFloors()
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

        private async Task<List<RoomType>> GetRoomTypes()
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
            reservationsList = await GetReservations();
            roomList = await GetRooms();
            roomTypeList = await GetRoomTypes();
            floorList = await GetFloors();
            locationList = await GetLocations();
            userList = await GetUsers();

            DateTime today = DateTime.Today;
            DateTime oneDayLater = today.AddDays(1);
            DateTime twoDaysLater = today.AddDays(2);

            tbDate1.Text = today.ToString("dddd, dd MMMM", CultureInfo.InvariantCulture);
            tbDate2.Text = oneDayLater.ToString("dddd, dd MMMM", CultureInfo.InvariantCulture);
            tbDate3.Text = twoDaysLater.ToString("dddd, dd MMMM", CultureInfo.InvariantCulture);

            foreach (Reservation reservation in reservationsList)
            {
                BlockReservation blockReservation = new BlockReservation();

                blockReservation.BeginTime = reservation.ReservationStart.ToShortTimeString();
                blockReservation.EndTime = reservation.ReservationEnd.ToShortTimeString();

                foreach (Room room in roomList)
                {
                    if (room.Id == reservation.RoomId)
                    {
                        blockReservation.Room = room.RoomName;
                        
                        foreach (RoomType roomType in roomTypeList)
                        {
                            if (room.RoomTypeId == roomType.Id)
                            {
                                blockReservation.RoomType = roomType.TypeName;
                                break;
                            }
                        }

                        foreach (Floor floor in floorList)
                        {
                            if (floor.Id == room.FloorId)
                            {
                                foreach (Location location in locationList)
                                {
                                    if (location.Id == floor.LocationId)
                                    {
                                        blockReservation.Building = location.LocationName;
                                        break;
                                    }
                                }
                            }
                        }
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

                if (reservation.ReservationStart.Date == today.Date)
                {
                    wpReservations1.Children.Add(blockReservation);
                }
                else if (reservation.ReservationStart.Date == oneDayLater.Date)
                {
                    wpReservations2.Children.Add(blockReservation);
                }
                else if (reservation.ReservationStart.Date == twoDaysLater.Date)
                {
                    wpReservations3.Children.Add(blockReservation);
                }
            } 
        }
    }
}
