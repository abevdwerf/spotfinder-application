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