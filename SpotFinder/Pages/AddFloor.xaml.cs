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
using SpotFinder.UserControls;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Win32;
using System.Linq;

namespace SpotFinder.Pages
{
    /// <summary>
    /// Interaction logic for AddFloor.xaml
    /// </summary>
    public partial class AddFloor : Page
    {
        private double heightCanvas;
        private double widthCanvas;
        private const int size = 12;
        private Random rnd = new Random();
        private Floor currentFloor;
        private List<RoomType> roomTypeList;

        public AddFloor(Floor currentFloor)
        {   
            InitializeComponent();
            this.currentFloor = currentFloor;
            this.Height = System.Windows.SystemParameters.VirtualScreenHeight - 125;
            LoadFloorNames();
            LoadRooms();
        }

        //methods
        private async void LoadRooms()
        {
            roomTypeList = await GetRoomTypes();
            foreach (Room room in await GetRooms())
            {
                if (room.FloorId == currentFloor.Id)
                {
                    SolidColorBrush randomColor = new SolidColorBrush(Color.FromRgb((byte)rnd.Next(255), (byte)rnd.Next(255), (byte)rnd.Next(255)));

                    List<ButtonLocation> root = null;
                    if (room.GridLocation != null && room.GridLocation != "[]")
                    {
                        root = JsonConvert.DeserializeObject<List<ButtonLocation>>(room.GridLocation);

                        foreach (Button btn in MyCanvas.Children)
                        {
                            foreach (ButtonLocation btnLoc in root)
                            {
                                Point pt1 = new Point();
                                pt1.X = btnLoc.X;
                                pt1.Y = btnLoc.Y;

                                Point pt2 = new Point();
                                pt2.X = btnLoc.X + size;
                                pt2.Y = btnLoc.Y + size;
                                Rect SelectedRect = new Rect(pt1, pt2);


                                Rect rectBounds = VisualTreeHelper.GetDescendantBounds(btn);
                                Vector vector = VisualTreeHelper.GetOffset(btn);
                                rectBounds.Offset(vector);
                                if (rectBounds.IntersectsWith(SelectedRect))
                                {
                                    btn.Background = randomColor;
                                }
                            }
                        }
                    }
                    foreach (RoomType roomtype in roomTypeList)
                    {
                        if (room.RoomTypeId == roomtype.Id)
                        {
                            RoomUC roomUc = new RoomUC(currentFloor, room, roomtype) { RoomName = room.RoomName, RoomType = roomtype.TypeName, MaxPersons = room.MaxPersons, ClickedRoom = room };

                            if(room.GridLocation != null && room.GridLocation != "[]")
                            {
                                roomUc.Color = randomColor;
                            }

                            spRoomContent.Children.Add(roomUc);
                            break;
                        } 
                    }
                }
            }
        }

        private void LoadFloorNames()
        {
            tbFloorName.Text = currentFloor.FloorName;
            tbName.Text = currentFloor.FloorName;
        }
        
        public void DrawButtons()
        {
            for (int j = 0; j < heightCanvas; j++)
            {
                for (int i = 0; i < widthCanvas; i++)
                {
                    Button button = new Button
                    {
                        Height = size,
                        Width = size,
                    };

                    //button.Click += Button_Click;
                    button.Background = null;
                    MyCanvas.Children.Add(button);

                    System.Windows.Controls.Canvas.SetLeft(button, i * size);
                    System.Windows.Controls.Canvas.SetTop(button, j * size);
                }
            }
        }

        //events
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            var mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            mw.ShowDropdown = Visibility.Visible;
            mw.Main.Navigate(new Locations(mw.Location));
        }

        private void btnAddRoom_Click(object sender, RoutedEventArgs e)
        {
            var mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            mw.Main.Navigate(new AddRoom(this.currentFloor));
        }

        private void btnAddMap_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";

            if (op.ShowDialog() == true)
            {
                ImageBrush ib = new ImageBrush();
                ib.ImageSource = new BitmapImage(new Uri(op.FileName, UriKind.Absolute));
                MyCanvas.Background = ib;
            }
        }
        
        private async void btnSaveFloor_Click(object sender, RoutedEventArgs e)
        {
            currentFloor.FloorName = tbName.Text;
            await UpdateFloor(currentFloor);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            heightCanvas = MyCanvas.ActualHeight / size;
            widthCanvas = MyCanvas.ActualWidth / size;
            DrawButtons();
        }

        //api requests
        private async Task<string> UpdateFloor(Floor currentFloor)
        {
            string json = JsonConvert.SerializeObject(currentFloor);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await ApiHelper.Put("api/floor/update/" + currentFloor.Id, content);

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    tbFloorName.Text = currentFloor.FloorName;
                    MessageBox.Show("floor updated");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return response.ToString();
        }

        public async Task<string> PutButtonGrid(string jsonObject)
        {
           StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
           HttpResponseMessage response = await ApiHelper.Put("api/room/update/1", content);

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("floor updated");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return response.ToString();
        }

        public async Task<List<Room>> GetRooms()
        {
            List<Room> rooms = null;
            HttpResponseMessage response = await ApiHelper.Get("api/rooms");

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    rooms = await response.Content.ReadAsAsync<List<Room>>();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return rooms;
        }

        public async Task<List<RoomType>> GetRoomTypes()
        {
            List<RoomType> roomType = null;
            HttpResponseMessage response = await ApiHelper.Get("api/roomtypes");

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    roomType = await response.Content.ReadAsAsync<List<RoomType>>();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return roomType;
        }
    }
}
