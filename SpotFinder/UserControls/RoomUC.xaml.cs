using SpotFinder.Classes;
using SpotFinder.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace SpotFinder.UserControls
{
    /// <summary>
    /// Interaction logic for RoomUC.xaml
    /// </summary>
    public partial class RoomUC : UserControl
    {
        private Floor floor;
        private Room room;
        private RoomType roomType;

        public RoomUC(Floor floor, Room room, RoomType roomType)
        {
            InitializeComponent();
            this.floor = floor;
            this.room = room;
            this.roomType = roomType;
            LoadRoomIcon(this.roomType.TypeName);
        }

        public string RoomName
        {
            set { roomName.Text = value; }
        }

        public string RoomType
        {
            set { tbroomType.Text = value;}
        }

        public int MaxPersons
        {
            set { maxPersons.Text = value.ToString(); }
        }

        public Brush Color
        {
            set { color.Fill = value; }
        }

        public Room ClickedRoom { get; set; }

        private void btnRoom1_Click(object sender, RoutedEventArgs e)
        {
            var mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            mw.Main.Navigate(new AddRoom(floor, room));
        }

        private void LoadRoomIcon(string name)
        {
            if (name == "Desk")
            {
                imgRoom.Source = new BitmapImage(new Uri("/Images/desk.png", UriKind.Relative));
            }
            else if (name == "Silent Room")
            {
                imgRoom.Source = new BitmapImage(new Uri("/Images/silentroom.png", UriKind.Relative));
            }
            else //meeting room
            {
                imgRoom.Source = new BitmapImage(new Uri("/Images/meetingroom.png", UriKind.Relative));
            }
        }
    }
}
