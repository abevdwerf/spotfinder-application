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
    /// Interaction logic for Floor.xaml
    /// </summary>
    public partial class Floor : UserControl
    {

        public Brush Color
        {
            set { color.Fill = value; }
        }

        public string Building
        {
            set { building.Text = value; }
        }

        public string Level
        {
            set { level.Text = value; }
        }

        public Floor()
        {
            InitializeComponent();
        }
    }
}
