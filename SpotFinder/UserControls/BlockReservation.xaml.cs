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
    /// Interaction logic BlockReservation.xaml
    /// </summary>
    public partial class BlockReservation : UserControl
    {

        public string Building
        {
            set { building.Text = value; }
        }

        public string Room
        {
            set { room.Text = value; }
        }

        public string RoomType
        {
            set { roomType.Text = value; }
        }

        public string Time
        {
            set { time.Text = value; }
        }

        public string User
        {
            set { user.Text = value; }
        }

        public BlockReservation()
        {
            InitializeComponent();
        }
    }
}
