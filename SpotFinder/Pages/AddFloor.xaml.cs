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

namespace SpotFinder.Pages
{
    /// <summary>
    /// Interaction logic for AddFloor.xaml
    /// </summary>
    public partial class AddFloor : Page
    {
        private double heightCanvas;
        private double widthCanvas;
        private Point startPoint;
        private Point pointWhereMouseIs;

        // private Canvas startPoint;
        Rectangle selectionRectangle = new Rectangle();

        private const int size = 12;
        //private const int space = 0;

        private List<ButtonLocation> lstButtons = new List<ButtonLocation>();

        private Random rnd = new Random();

        public AddFloor()
        {
            InitializeComponent();
            LoadRooms();

            this.Height = System.Windows.SystemParameters.VirtualScreenHeight - 125;
        }

        public Floor ChosenFloor { get; set; }

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
                            UserControl roomUc = new RoomUC(this, room) { RoomName = room.RoomName, RoomType = roomtype.TypeName, MaxPersons = room.MaxPersons, ClickedRoom = room };
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

        private void btnSaveFloor_Click(object sender, RoutedEventArgs e)
        {
            //TODO opslaan

            string json = JsonConvert.SerializeObject(lstButtons);

            dynamic jsonObject = new JObject();
            jsonObject.grid_location = json;

            //txtbl.Text = jsonObject.ToString().Replace(@"\", "");
            //MessageBox.Show(json);

            string testjson = JsonConvert.SerializeObject(jsonObject);

            //await PutButtonGrid(testjson);

            //var arrayOfObjects = JsonConvert.SerializeObject(
            //    new[] { JsonConvert.DeserializeObject(json1), JsonConvert.DeserializeObject(json2) }
            //)
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
 
            ((MainWindow)Application.Current.MainWindow).ChangeMenuContent(new Locations());
        }

        private void btnAddRoom_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).ChangeMenuContent(new AddRoom(this));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            Button btn2 = new Button();
            btn2.Click += Button_Click;
            btn2.Background = Brushes.Red;

            //MyCanvas.Children.Remove(10);
            //MyCanvas.Children.Insert(10, button2);
            //MessageBox.Show(lstButtons.Count.ToString());
            if (btn.Background == Brushes.Purple)
            {
                btn.Background = Brushes.Transparent;

                foreach (ButtonLocation btnLocation in lstButtons.ToArray())
                {
                    if (btnLocation.X == System.Windows.Controls.Canvas.GetLeft(btn) && btnLocation.Y == System.Windows.Controls.Canvas.GetTop(btn))
                    {
                        lstButtons.Remove(btnLocation);
                    }
                }
            }
            else
            {
                btn.Background = Brushes.Purple;

                ButtonLocation btnLocation = new ButtonLocation();
                btnLocation.Y = System.Windows.Controls.Canvas.GetTop(btn);
                btnLocation.X = System.Windows.Controls.Canvas.GetLeft(btn);
                lstButtons.Add(btnLocation);
            }
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

                    button.Click += Button_Click;
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

        private void EditedItems_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition((IInputElement)sender);
        }

        // '' <summary>
        // '' When mouse move, update the highlight of the selected items.
        // '' </summary>
        private void EditedItems_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (startPoint == default(Point))
            {
                return;
            }
            pointWhereMouseIs = e.GetPosition((IInputElement)sender);
            Rect SelectedRect = new Rect(startPoint, pointWhereMouseIs);
            if ((SelectedRect.Width < 20)
                        && (SelectedRect.Height < 20))
            {
                return;
            }

            Rectangle selectionRectangle = new Rectangle();

            //  show the rectangle again
            System.Windows.Controls.Canvas.SetLeft(selectionRectangle, Math.Min(startPoint.X, pointWhereMouseIs.X));
            System.Windows.Controls.Canvas.SetTop(selectionRectangle, Math.Min(startPoint.Y, pointWhereMouseIs.Y));
            selectionRectangle.Width = Math.Abs(pointWhereMouseIs.X - startPoint.X);
            selectionRectangle.Height = Math.Abs(pointWhereMouseIs.Y - startPoint.Y);
            foreach (Button btn in MyCanvas.Children)
            {
                Rect rectBounds = VisualTreeHelper.GetDescendantBounds(btn);
                Vector vector = VisualTreeHelper.GetOffset(btn);
                rectBounds.Offset(vector);
                if (rectBounds.IntersectsWith(SelectedRect))
                {
                    btn.Background = Brushes.LightGreen;

                    //double test1 = System.Windows.Controls.Canvas.GetTop(btn);
                    //double test2 = System.Windows.Controls.Canvas.GetTop(btn);
                    //lstTest.Add(test1);
                }
                else
                {
                    if (btn.Background != Brushes.Purple)
                    {
                        btn.Background = Brushes.Transparent;
                    }
                }
            }
        }

        // '' <summary>
        // '' When Left Mouse button is released, change all CheckBoxes values. (Or do nothing if it is a small move -->
        // '' click will be handled in a standard way.)
        // '' </summary>
        private void EditedItems_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            pointWhereMouseIs = e.GetPosition((IInputElement)sender);
            Rect SelectedRect = new Rect(startPoint, pointWhereMouseIs);
            startPoint = default(Point);
            selectionRectangle.Opacity = 0;
            //  hide the rectangle again
            if (((SelectedRect.Width < 20)
                        && (SelectedRect.Height < 20)))
            {
                return;
            }
            foreach (Button btn in MyCanvas.Children)
            {
                Rect rectBounds = VisualTreeHelper.GetDescendantBounds(btn);
                Vector vector = VisualTreeHelper.GetOffset(btn);
                rectBounds.Offset(vector);
                if (rectBounds.IntersectsWith(SelectedRect))
                {
                    if (btn.Background == Brushes.Purple)
                        btn.Background = null;
                    else
                    {
                        btn.Background = Brushes.Purple;

                        ButtonLocation btnLocation = new ButtonLocation();
                        btnLocation.Y = System.Windows.Controls.Canvas.GetTop(btn);
                        btnLocation.X = System.Windows.Controls.Canvas.GetLeft(btn);
                        lstButtons.Add(btnLocation);
                    }
                }
                //btn.Background = Brushes.Transparent;
            }
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
