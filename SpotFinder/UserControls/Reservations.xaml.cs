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
    /// Interaction logic for Reservations.xaml
    /// </summary>
    public partial class Reservations : UserControl
    {
        public Brush Color
        {
            set { color.Fill = value; }
        }
        public string Building
        {
            set { building.Text = value; }
        }

        public string Quantity
        {
            set { quantity.Text = value; }
        }

        public Reservations()
        {
            InitializeComponent();
        }
    }
}
