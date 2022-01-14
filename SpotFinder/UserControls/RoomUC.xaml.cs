using SpotFinder.Classes;
using SpotFinder.Pages;
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

namespace SpotFinder.UserControls
{
    /// <summary>
    /// Interaction logic for RoomUC.xaml
    /// </summary>
    public partial class RoomUC : UserControl
    {
        private AddFloor floor;
        private Room room;

        public RoomUC(AddFloor floor, Room room)
        {
            InitializeComponent();
            this.floor = floor;
            this.room = room;
        }

        public string RoomName
        {
            set { roomName.Text = value; }
        }

        public string RoomType
        {
            set { roomType.Text = value; }
        }

        public int MaxPersons
        {
            set { maxPersons.Text = value.ToString(); }
        }

        public Room ClickedRoom { get; set; }

        private void btnRoom1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).ChangeMenuContent(new AddRoom(floor, room));
        }
    }
}
