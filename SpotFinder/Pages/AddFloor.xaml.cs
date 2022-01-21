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
        private Floor chosenFloor;

        public AddFloor()
        {   
            InitializeComponent();
            LoadRooms();

            this.Height = System.Windows.SystemParameters.VirtualScreenHeight - 125;
        }

        public Floor ChosenFloor { get { return chosenFloor; } set { chosenFloor = value; LoadFloorName(); } }

        public async Task<List<Room>> GetRooms()
        {
            List<Room> rooms = null;
            HttpResponseMessage response = await ApiHelper.Get("api/rooms");

            if (response.IsSuccessStatusCode)
            {
                rooms = await response.Content.ReadAsAsync<List<Room>>();
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }

            return rooms;
        }

        public async Task<List<RoomType>> GetRoomTypes()
        {
            List<RoomType> roomType = null;
            HttpResponseMessage response = await ApiHelper.Get("api/roomtypes");

            if (response.IsSuccessStatusCode)
            {
                roomType = await response.Content.ReadAsAsync<List<RoomType>>();
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }

            return roomType;
        }

        private async void LoadRooms()
        {
            foreach (Room room in await GetRooms())
            {
                SolidColorBrush randomColor = new SolidColorBrush(Color.FromRgb((byte)rnd.Next(255), (byte)rnd.Next(255), (byte)rnd.Next(255)));

                List<ButtonLocation> root = null;
                //var json = File.ReadAllText("json1.json");
                if (room.GridLocation != null)
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
                        //btn.Background = Brushes.Transparent;
                    }
                }

                //foreach (var p in root.hireSchedules)
                //{
                //    ///do something
                //}
                foreach (RoomType roomtype in await GetRoomTypes())
                {
                    if (room.FloorId == ChosenFloor.Id)
                    {
                        if (room.RoomTypeId == roomtype.Id)
                        {
                            RoomUC roomUc = new RoomUC(this, room) { RoomName = room.RoomName, RoomType = roomtype.TypeName, MaxPersons = room.MaxPersons, ClickedRoom = room };
                            spRoomContent.Children.Add(roomUc);
                        }
                    }
                }
                //if (room.FloorId == ChosenFloor.Id)
                //{
                //    ChosenFloor.Rooms.Add(room);
                //}
            }
        }

        private void LoadFloorName()
        {
            tbFloorName.Text = ChosenFloor.FloorName;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            var mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            mw.ShowDropdown = Visibility.Visible;
            mw.Main.Navigate(new Locations(mw.Location));
        }

        private void btnAddRoom_Click(object sender, RoutedEventArgs e)
        {
            var mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            mw.Main.Navigate(new AddRoom(this));
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

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            heightCanvas = MyCanvas.ActualHeight / size;
            widthCanvas = MyCanvas.ActualWidth / size;
            DrawButtons();
            //txtbl.Text = MyCanvas.Width.ToString() + "  ActualHeight: " + MyCanvas.ActualHeight.ToString() + "  ActualWidth: " + MyCanvas.ActualWidth.ToString();
            //change button size with windows size...

            //if (ActualWidth < 400)
            //{
            //    heightCanvas = 100;
            //    widthCanvas = 100;
            //}
            //else if (ActualWidth < 400)
            //{
            //}
        }

        public async Task<string> PutButtonGrid(string jsonObject)
        {
            StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await ApiHelper.Put("api/room/update/1", content);

            return response.ToString();
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
    }
}
