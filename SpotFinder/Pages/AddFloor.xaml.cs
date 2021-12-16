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
using System.Windows.Shapes;
using SpotFinder.Classes;
using SpotFinder.UserControls;
using System.Net.Http;
using System.Threading.Tasks;

namespace SpotFinder.Pages
{
    /// <summary>
    /// Interaction logic for AddFloor.xaml
    /// </summary>
    public partial class AddFloor : Page
    {
        public AddFloor()
        {
            InitializeComponent();
            LoadRooms();
        }

        public Floor ChosenFloor { get; set; }

        public async Task<List<Room>> GetRooms()
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

        private async void LoadRooms()
        {
            foreach (Room room in await GetRooms())
            {
                if (room.FloorId == ChosenFloor.Id)
                {
                    UserControl roomUc = new RoomUC() { RoomName = room.RoomName, RoomType = "desk", MaxPersons = room.MaxPersons };
                    spRoomContent.Children.Add(roomUc);
                }
            }
        }

        private async void btnSaveFloor_Click(object sender, RoutedEventArgs e)
        {
           //TODO opslaan
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
 
            ((MainWindow)Application.Current.MainWindow).ChangeMenuContent(new Locations());
        }

        private void btnAddRoom_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).ChangeMenuContent(new AddRoom());
        }
    }
}
