using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotFinder.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SpotFinder.Pages
{
    /// <summary>
    /// Interaction logic for AddRoom.xaml
    /// </summary>
    public partial class AddRoom : Page
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
        AddFloor currentFloor;
        private List<Desk> desks = new List<Desk>();
        int number = 0;
        int roomId;
        int maxPersons = 0;
        private Random rnd = new Random();

        private Room currentRoom;
        public AddRoom(AddFloor currentFloor)
        {
            this.currentFloor = currentFloor;
            InitializeComponent();

            this.Height = System.Windows.SystemParameters.VirtualScreenHeight - 125;
            LoadRoomTypes();
            LoadModules();
        }

        public AddRoom(AddFloor currentFloor, Room currentRoom)
        {
            InitializeComponent();
            this.currentFloor = currentFloor;
            this.currentRoom = currentRoom;
            LoadRoomTypes();
            LoadModules();

            this.Height = System.Windows.SystemParameters.VirtualScreenHeight - 125;
            loadRoom();
        }
        private void loadRoom()
        {
            SolidColorBrush randomColor = new SolidColorBrush(Color.FromRgb((byte)rnd.Next(255), (byte)rnd.Next(255), (byte)rnd.Next(255)));

            List<ButtonLocation> root = null;
            //var json = File.ReadAllText("json1.json");
            if (currentRoom.GridLocation != null)
            {
                root = JsonConvert.DeserializeObject<List<ButtonLocation>>(currentRoom.GridLocation);

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
            tbName.Text = currentRoom.RoomName;
            tbPeople.Text = currentRoom.MaxPersons.ToString();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            //((MainWindow)Application.Current.MainWindow).ChangeMenuContent(currentFloor);
            var mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            mw.ChangeMenuContent(currentFloor);
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

        private async void btnSaveRoom_Click(object sender, RoutedEventArgs e)
        {
            Room room = new Room();
            room.FloorId = currentFloor.ChosenFloor.Id;
            room.RoomName = tbName.Text;
            room.RoomTypeId = (Int32)cbRoomTypes.SelectedValue;

            room.GridLocation = JsonConvert.SerializeObject(lstButtons);

            room.MaxPersons = maxPersons;

            await PostButtonGrid(room);
            await PostDesks();
        }

        private async Task<string> PostDesks()
        {
            string json = JsonConvert.SerializeObject(desks);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await ApiHelper.Post("api/desk/create", content);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("desks geup.lad");
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
            return response.ToString();
        }

        private async Task<string> PostButtonGrid(Room room)
        {
            string json = JsonConvert.SerializeObject(room);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await ApiHelper.Post("api/room/create", content);
            if (response.IsSuccessStatusCode)
            {
                string test = await response.Content.ReadAsStringAsync();
                dynamic data = JObject.Parse(test);
                MessageBox.Show(data.last_insert_id.ToString());
                //room.Id = data.last_insert_id;
                roomId = data.last_insert_id;

                foreach (Desk desk in desks)
                {
                    desk.RoomId = data.last_insert_id;
                }
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
            return response.ToString();

        }

        private async Task<List<RoomType>> GetRoomTypes()
        {
            List<RoomType> roomTypes = null;
            HttpResponseMessage response = await ApiHelper.Get("api/roomtypes");

            if (response.IsSuccessStatusCode)
            {
                roomTypes = await response.Content.ReadAsAsync<List<RoomType>>();
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }

            //StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            //HttpResponseMessage response = await ApiHelper.Put("api/room/update/1", content);

            return roomTypes;
        }

        private async void LoadRoomTypes()
        {
            cbRoomTypes.ItemsSource = await GetRoomTypes();

        }

        private async void LoadModules()
        {
            cbModules.ItemsSource = await GetModules();
        }

        private async Task<List<Module>> GetModules()
        {
            List<Module> modules = null;
            HttpResponseMessage response = await ApiHelper.Get("api/modules");

            if (response.IsSuccessStatusCode)
            {
                modules = await response.Content.ReadAsAsync<List<Module>>();
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }

            return modules;
        }

        private void cbRoomTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnAddTable_Click(object sender, RoutedEventArgs e)
        {
            number++;
            Desk desk = new Desk();
            desk.AvailableSpaces = int.Parse(tbPeople.Text);
            desk.ModuleId = (Int32)cbModules.SelectedValue;

            maxPersons = maxPersons + int.Parse(tbPeople.Text);

            desks.Add(desk);
            lbTables.Items.Add(number.ToString());
        }
    }
}
