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
using System.Linq;

namespace SpotFinder.Pages
{
    /// <summary>
    /// Interaction logic for Locations.xaml
    /// </summary>
    public partial class Locations : Page
    {
        private bool LoadDropdown { get; set; } = false;

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
            int counter = 0;

            //unregister event to change combobox
            cbLocations.SelectionChanged -= cbLocations_SelectionChanged;
            
            if (wpLocations.Children.Count > 0)
            {
                wpLocations.Children.Clear();
            }

            foreach (Location location in locations)
            {
                //if dropdown already loaded then don't add new Items
                if (!LoadDropdown)
                {
                    counter++;

                    ComboBoxItem itemLocation = new ComboBoxItem();
                    itemLocation.Content = location.LocationName;
                    itemLocation.Tag = location.Id.ToString();

                    cbLocations.Items.Add(itemLocation);

                    if (locations.Count() == counter)
                    {
                        LoadDropdown = true;
                    }
                }
                
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

                if (locationId == location.Id)
                {
                    foreach (Floor floor in await GetFloors())
                    {
                        if (locationId == floor.LocationId)
                        {
                            UserControl floorLocation = new FloorLocation() { Building = location.LocationName, Level = floor.FloorName, ClickedfFloor = floor };
                            wpLocations.Children.Add(floorLocation);
                        }
                    }
                }
            }

            //register event
            cbLocations.SelectionChanged += cbLocations_SelectionChanged;
        }

        private void cbLocations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedtag = ((ComboBoxItem)cbLocations.SelectedItem).Tag.ToString();
            int locationId = int.Parse(selectedtag);
            LoadLocations(locationId);
        }
    }
}
