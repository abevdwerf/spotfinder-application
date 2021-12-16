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
        public RoomUC()
        {
            InitializeComponent();
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
    }
}
