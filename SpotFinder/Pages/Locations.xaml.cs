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
using SpotFinder.Classes;
using System.Net.Http;
using System.Threading.Tasks;
using SpotFinder.UserControls;

namespace SpotFinder.Pages
{
    /// <summary>
    /// Interaction logic for Locations.xaml
    /// </summary>
    public partial class Locations : Page
    {
        public Locations()
        {
            InitializeComponent();
            LoadLocations(0);
        }

        public async Task<List<Location>> GetLocations()
        {
            List<Location> locations = null;
            HttpResponseMessage response = await ApiHelper.Get("api/locations");

            if (response.IsSuccessStatusCode)
            {
                locations = await response.Content.ReadAsAsync<List<Location>>();
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }

            return locations;
        }

        public async Task<List<Floor>> GetFloors()
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

        private async void LoadLocations(int locationId)
        {
            List<Location> locations = await GetLocations();
            bool allLocations = false;

            cbLocations.SelectionChanged -= cbLocations_SelectionChanged;

            foreach (Location location in locations)
            {
                ComboBoxItem itemLocation = new ComboBoxItem();
                itemLocation.Content = location.LocationName;
                itemLocation.Tag = location.Id;

                cbLocations.Items.Add(itemLocation);

                if (locationId == 0)
                {
                    allLocations = true;
                }

                if (allLocations)
                {
                    if (location.Id > locationId)
                    {
                        locationId++;
                    }
                }

                foreach (Floor floor in await GetFloors())
                {
                    if (locationId == floor.LocationId)
                    {
                        UserControl floorLocation = new FloorLocation() { Building = location.LocationName, Level = floor.FloorName, ClickedfFloor = floor };
                        wpLocations.Children.Add(floorLocation);
                    }
                }
            }

            cbLocations.SelectionChanged += cbLocations_SelectionChanged;
        }

        //private void LocationChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    string selectedtag = ((ComboBoxItem)cbLocations.SelectedItem).Tag.ToString();
        //    int locationid = Int32.Parse(selectedtag);
        //    int locationId = 1;
        //    wpLocations.Children.Clear();
        //    cbLocations.Items.RemoveAt(1);
        //    LoadLocations(locationId);
        //}

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //cbLocations.SelectionChanged += cbLocations_SelectionChanged;
        }

        private void cbLocations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedtag = ((ComboBoxItem)cbLocations.SelectedItem).Tag.ToString();
            int locationId = Int32.Parse(selectedtag);

            cbLocations.Items.RemoveAt(1);
            LoadLocations(locationId);
        }

        //private void Selected(object sender, RoutedEventArgs e)
        //{
        //    //string selectedTag = ((ComboBoxItem)cbLocations.SelectedItem).Tag.ToString();
        //    //int locationId = Int32.Parse(selectedTag);
        //    int locationId = 1;
        //    wpLocations.Children.Clear();
        //    cbLocations.Items.RemoveAt(1);
        //    LoadLocations(locationId);
        //}
    }
}
