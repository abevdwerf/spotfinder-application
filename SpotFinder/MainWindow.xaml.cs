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
using System.Net.Http;

namespace SpotFinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Location> locationsList;
        private List<Floor> floorList;
        private Location lct;
        //public Dashboard dashboard = new Dashboard();
        //public Reservations reservations = new Reservations();
        //public Locations locations = new Locations();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += MainWindow_Loaded;
        }

        public Visibility ShowDropdown
        {
            set { dpLocationDropwdown.Visibility = value; }
        }

        public Location Location
        {
            get { return lct; }
            set { lct = value; }
        }

        //events
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAllLocations();
            Main.Navigate(new Dashboard(lct));
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            SetActiveStyleMenuItem("Dashboard");
            ShowDropdown = Visibility.Visible;
            Main.Navigate(new Dashboard(lct));
        }

        private void Reservation_click(object sender, RoutedEventArgs e)
        {
            SetActiveStyleMenuItem("Reservations");
            ShowDropdown = Visibility.Visible;
            Main.Navigate(new Reservations(lct));
        }

        private void Locations_Click(object sender, RoutedEventArgs e)
        {
            SetActiveStyleMenuItem("Locations");
            ShowDropdown = Visibility.Visible;
            Main.Navigate(new Locations(lct));
        }

        private async void LogOut_Click(object sender, RoutedEventArgs e)
        {
            await LogOut();
        }

        private void cbLocations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Location = (Location)cbLocations.SelectedItem;

            switch (Main.Content.GetType().Name)
            {
                case "Dashboard":
                    Main.Navigate(new Dashboard(lct));
                    break;
                case "Reservations":
                    Main.Navigate(new Reservations(lct));
                    break;
                case "Locations":
                    Main.Navigate(new Locations(lct));
                    break;
                default:

                    break;
            }
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

        private async void LoadAllLocations()
        {
            locationsList = await GetLocations();
            floorList = await GetFloors();

            foreach (Location location in locationsList)
            {
                foreach (Floor floor in floorList)
                {
                    if (floor.LocationId == location.Id)
                    {
                        location.Floors.Add(floor);
                    }
                }
            }

            cbLocations.ItemsSource = locationsList;
        }

        private async Task LogOut()
        {

            HttpResponseMessage response = await ApiHelper.Post("api/logout", null);

            if (response.IsSuccessStatusCode)
            {
                Login login = new Login();
                login.Show();

                //sluit de Mainwindow af
                Close();
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        private async Task<List<Location>> GetLocations()
        {
            List<Location> locationsList = null;
            HttpResponseMessage response = await ApiHelper.Get("api/locations");

            if (response.IsSuccessStatusCode)
            {
                locationsList = await response.Content.ReadAsAsync<List<Location>>();
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }

            return locationsList;
        }

        private async Task<List<Floor>> GetFloors()
        {
            List<Floor> floors = null;
            HttpResponseMessage response = await ApiHelper.Get("api/floors");

            if (response.IsSuccessStatusCode)
            {
                floors = await response.Content.ReadAsAsync<List<Floor>>();
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }

            return floors;
        }
    }
}
