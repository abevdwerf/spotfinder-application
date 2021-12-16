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
            LoadLocations();
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

        private async void LoadLocations()
        {
            List<Location> locations = await GetLocations();

            foreach (Location location in locations)
            {
                foreach (Floor floor in await GetFloors())
                {
                    if (location.Id == floor.LocationId)
                    {
                        UserControl floorLocation = new FloorLocation() { Building = location.LocationName, Level = floor.FloorName, ClickedfFloor = floor};
                        wpLocations.Children.Add(floorLocation);
                    }
                }
            }
        }
    }
}
