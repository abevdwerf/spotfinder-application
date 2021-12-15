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
using System.Windows.Shapes;
using SpotFinder.Classes;
using System.Net.Http;
using System.Threading.Tasks;

namespace SpotFinder.Pages
{
    /// <summary>
    /// Interaction logic for AddFloor.xaml
    /// </summary>
    public partial class AddFloor : Page
    {
        public AddFloor()
        {
            InitializeComponent();
        }

        private async Task<List<Floor>> GetFloorsAsync()
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

        private async void btnSaveFloor_Click(object sender, RoutedEventArgs e)
        {
            List<Floor> floors = await GetFloorsAsync();

            foreach (Floor floor in floors)
            {
                MessageBox.Show(floor.floor_name);
            }
        }
    }
}
