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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SpotFinder.Pages
{
    /// <summary>
    /// Interaction logic for Locations.xaml
    /// </summary>
    public partial class Locations : Page
    {
        private bool LoadDropdown { get; set; } = false;

        private List<Location> locationsList;

        public List<Location> LocationList
        {
            get { return locationsList; }
            set { locationsList = value; }
        }

        private List<Floor> floorList;

        public Locations()
        {
            InitializeComponent();
            DataContext = this;
            //LoadLocations(0);
            LoadAllLocations();
        }

        private async void LoadAllLocations()
        {
            locationsList = await GetLocations();
            floorList = await GetFloors();
            cbLocations.ItemsSource = locationsList;

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
        }

        public async Task<List<Location>> GetLocations()
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

        //private async void LoadLocations(int locationId)
        //{
        //    locationsList = await GetLocations();

        //    bool allLocations = false;
        //    int counter = 0;

        //    //unregister event to change combobox
        //    cbLocations.SelectionChanged -= cbLocations_SelectionChanged;
            
        //    if (wpLocations.Children.Count > 0)
        //    {
        //        wpLocations.Children.Clear();
        //    }

        //    foreach (Location location in locationsList)
        //    {
        //        //if dropdown already loaded then don't add new Items
        //        if (!LoadDropdown)
        //        {
        //            counter++;

        //            ComboBoxItem itemLocation = new ComboBoxItem();
        //            itemLocation.Content = location.LocationName;
        //            itemLocation.Tag = location.Id.ToString();

        //            cbLocations.Items.Add(itemLocation);

        //            if (locationsList.Count() == counter)
        //            {
        //                LoadDropdown = true;
        //            }
        //        }

        //        //if (locationId == 0)
        //        //{
        //        //    allLocations = true;
        //        //}

        //        //if (allLocations)
        //        //{
        //        //    if (location.Id > locationId)
        //        //    {
        //        //        locationId++;
        //        //    }
        //        //}

        //        //if (int.Parse(((ComboBoxItem)cbLocations.SelectedItem).Tag.ToString()) == location.Id)
        //        //{
        //        //    foreach (Floor floor in await GetFloors())
        //        //    {
        //        //        if (location.Id == floor.LocationId)
        //        //        {
        //        //            UserControl floorLocation = new FloorLocation() { Building = location.LocationName, Level = floor.FloorName, ClickedfFloor = floor };
        //        //            wpLocations.Children.Add(floorLocation);
        //        //        }
        //        //    }
        //        //}
        //        //else if (int.Parse(((ComboBoxItem)cbLocations.SelectedItem).Tag.ToString()) < 1)
        //        //{
        //        //    foreach (Floor floor in await GetFloors())
        //        //    {
        //        //        UserControl floorLocation = new FloorLocation() { Building = location.LocationName, Level = floor.FloorName, ClickedfFloor = floor };
        //        //        wpLocations.Children.Add(floorLocation);
        //        //    }
        //        //}
        //    }

        //    //register event
        //    cbLocations.SelectionChanged += cbLocations_SelectionChanged;
        //}

        private void LoadFloors(Location location)
        {
            if (wpLocations.Children.Count > 0)
            {
                wpLocations.Children.Clear();
            }
            foreach (Floor floor in location.Floors)
            {
                if (floor.LocationId == location.Id)
                {
                    UserControl floorLocation = new FloorLocation() { Building = location.LocationName, Level = floor.FloorName, ClickedfFloor = floor };
                    wpLocations.Children.Add(floorLocation);
                }
            }
        }

        private void cbLocations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Location lct = (Location)cbLocations.SelectedItem;
            LoadFloors(lct);
        }
    }
}
