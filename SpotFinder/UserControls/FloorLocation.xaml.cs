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
using SpotFinder.Pages;
using SpotFinder.Classes;

namespace SpotFinder.UserControls
{
    /// <summary>
    /// Interaction logic for Floor.xaml
    /// </summary>
    public partial class FloorLocation : UserControl
    {
        public FloorLocation()
        {
            InitializeComponent();
        }

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

        public Floor ClickedfFloor
        {
            get;
            set;
        }

        private void FloorLocation_Click(object sender, MouseButtonEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).ChangeMenuContent(new AddFloor() { ChosenFloor = ClickedfFloor});
            //MainWindow main = Application.Current.MainWindow as MainWindow;
            //main.ChangeMenuContent(new AddFloor() { ChosenFloor = ClickedfFloor });
        }
    }
}
