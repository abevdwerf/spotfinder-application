using SpotFinder.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SpotFinder.Classes;

namespace SpotFinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //ApiHelper.InitializeClient();
            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Main.Navigate(new Dashboard());
        }

        public void ChangeMenuContent(Page page)
        {
            Main.Navigate(page);
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            SetActiveStyleMenuItem("Dashboard");
            Main.Navigate(new Dashboard());
        }

        private void Reservation_click(object sender, RoutedEventArgs e)
        {
            SetActiveStyleMenuItem("Reservations");
            Main.Navigate(new Reservations());
        }

        private void Locations_Click(object sender, RoutedEventArgs e)
        {
            SetActiveStyleMenuItem("Locations");
            Main.Navigate(new Locations());
        }

        private void SetActiveStyleMenuItem(string activeMenu)
        {
            Style activeStyle = FindResource("ActiveMenuItem") as Style;
            Style defaultStyle = FindResource("DefaultMenuItem") as Style;

            if (activeMenu == "Dashboard")
            {
                Dashboard.Style = activeStyle;
                Reservations.Style = defaultStyle;
                Locations.Style = defaultStyle;
            }
            else if (activeMenu == "Reservations")
            {
                Reservations.Style = activeStyle;
                Dashboard.Style = defaultStyle;
                Locations.Style = defaultStyle;
            }
            else
            {
                Locations.Style = activeStyle;
                Dashboard.Style = defaultStyle;
                Reservations.Style = defaultStyle;
            }
        }
    }
}
