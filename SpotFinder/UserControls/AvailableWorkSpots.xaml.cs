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
    /// Interaction logic for AvailableWorkSpots.xaml
    /// </summary>
    /// 

    public partial class AvailableWorkSpots : UserControl
    {
        public string Building
        {
            set { building.Text = value; }
        }

        public string Available
        {
            set { available.Text = value; }
        }

        public AvailableWorkSpots()
        {
            InitializeComponent();
        }
    }
}
