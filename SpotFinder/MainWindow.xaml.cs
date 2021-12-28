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
            ApiHelper.InitializeClient();
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
            Main.Navigate(new Dashboard());
        }

        private void Reservation_click(object sender, RoutedEventArgs e)
        {
            Main.Navigate(new Reservations());
        }

        private void Locations_Click(object sender, RoutedEventArgs e)
        {
            Main.Navigate(new Locations());
        }
    }
}
