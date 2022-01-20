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
        private List<Reservation> reservationsList;
        private List<Room> roomList;
        private List<RoomType> roomTypeList;
        private List<User> userList;

        //private Location currentLocation;

        public Reservations(Location currentLocation)
        {
            InitializeComponent();
            LoadReservations(currentLocation);
        }

        //public Location CurrentLocation { get { return currentLocation; } set { currentLocation = value; LoadReservations(currentLocation); } }

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

        private async void LoadReservations(Location location)
        {
            if (wpReservations1.Children.Count > 0)
            {
                wpReservations1.Children.Clear();
            }

            if (wpReservations2.Children.Count > 0)
            {
                wpReservations2.Children.Clear();
            }

            if (wpReservations3.Children.Count > 0)
            {
                wpReservations3.Children.Clear();
            }

            reservationsList = await GetReservations();
            roomList = await GetRooms();
            roomTypeList = await GetRoomTypes();
            userList = await GetUsers();

            DateTime today = DateTime.Today;
            DateTime oneDayLater = today.AddDays(1);
            DateTime twoDaysLater = today.AddDays(2);

            tbDate1.Text = today.ToString("dddd, dd MMMM", CultureInfo.InvariantCulture);
            tbDate2.Text = oneDayLater.ToString("dddd, dd MMMM", CultureInfo.InvariantCulture);
            tbDate3.Text = twoDaysLater.ToString("dddd, dd MMMM", CultureInfo.InvariantCulture);

            string message = "No reservations have been made for this date yet";

            //controleren of location bij de reservering past anders hoeft de blockreservation niet aangemaakt te worden

            foreach (Reservation reservation in reservationsList)
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

                                    break;
                                }
                            }
                        }
                    } 
                }
            }

            if (wpReservations1.Children.Count == 0)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = message;
                wpReservations1.Children.Add(textBlock);
            }

            if (wpReservations2.Children.Count == 0)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = message;
                wpReservations2.Children.Add(textBlock);
            }
            
            if (wpReservations3.Children.Count == 0)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = message;
                wpReservations3.Children.Add(textBlock);
            }
        }
    }
}
