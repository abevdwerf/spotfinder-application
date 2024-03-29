﻿using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotFinder.Classes;
using SpotFinder.UserControls;
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
using System.Text.RegularExpressions;

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
        Rectangle selectionRectangle = new Rectangle();
        private const int size = 12;
        private List<ButtonLocation> lstButtons = new List<ButtonLocation>();
        Floor currentFloor;
        private List<Desk> desks = new List<Desk>();
        int number = 0;
        int roomId;
        int maxPersons = 0;
        private Random rnd = new Random();
        private Room currentRoom;

        public AddRoom(Floor currentFloor)
        {
            InitializeComponent();
            this.currentFloor = currentFloor;
            this.Height = System.Windows.SystemParameters.VirtualScreenHeight - 125;
            LoadRoomTypes();
            LoadModules();
        }

        //Update room constructor
        public AddRoom(Floor currentFloor, Room currentRoom)
        {
            InitializeComponent();
            this.currentFloor = currentFloor;
            this.currentRoom = currentRoom;
            LoadRoomTypes();
            //LoadModules();
            LoadTables();
            tbRoomName.Text = currentRoom.RoomName;
            btnSaveRoom.Visibility = Visibility.Collapsed;
            btnUpdateRoom.Visibility = Visibility.Visible;

            this.Height = System.Windows.SystemParameters.VirtualScreenHeight - 125;
        }

        #region LoadData
        private async void LoadRoomTypes()
        {
            List<RoomType> roomTypes = await GetRoomTypes();
            cbRoomTypes.ItemsSource = roomTypes;

            if (currentRoom != null)
            {
                foreach (RoomType roomType in roomTypes)
                {
                    if (currentRoom.RoomTypeId == roomType.Id)
                    {
                        cbRoomTypes.SelectedItem = roomType;
                    }
                }
            }
        }

        private async void LoadTables()
        {
            List<Desk> allDesks = await GetDesks();
            List<Module> modules = await GetModules();
            cbModules.ItemsSource = modules;

            foreach (Desk desk in allDesks)
            {
                if (desk.RoomId == currentRoom.Id)
                {
                    desks.Add(desk);
                    UserControl ucTable = new TableUC() { Capacity = desk.AvailableSpaces.ToString(), Desk = desk };
                    lbTables.Items.Add(ucTable);

                    cbModules.SelectedItem = (cbModules.SelectedValue = desk.ModuleId.ToString());
                    foreach (Module module in modules)
                    {
                        if (desk.Id == module.DeskId)
                        {
                            desk.ModuleId = module.Id;
                        }
                    }
                }
            }
        }

        private void loadRoom()
        {
            SolidColorBrush randomColor = new SolidColorBrush(Color.FromRgb((byte)rnd.Next(255), (byte)rnd.Next(255), (byte)rnd.Next(255)));

            MyCanvas.Arrange(new Rect(MyCanvas.RenderSize));
            //MyCanvas.Measure(MyCanvas.RenderSize);
            //var json = File.ReadAllText("json1.json");
            if (currentRoom.GridLocation != null || currentRoom.GridLocation != "[]")
            {
                lstButtons = JsonConvert.DeserializeObject<List<ButtonLocation>>(currentRoom.GridLocation);

                foreach (Button btn in MyCanvas.Children)
                {
                    foreach (ButtonLocation btnLoc in lstButtons)
                    {

                        Point pt1 = new Point();
                        pt1.X = btnLoc.X + 1;
                        pt1.Y = btnLoc.Y + 1;

                        Point pt2 = new Point();
                        pt2.X = btnLoc.X + (size - 1);
                        pt2.Y = btnLoc.Y + (size - 1);
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
            tbName.Text = currentRoom.RoomName;
        }

        private async void LoadModules()
        {
            List<Module> modules = await GetModules();
            cbModules.ItemsSource = await GetModules();
        }

        private async void LoadModules(Desk desk)
        {
            List<Module> modules = await GetModules();

            cbModules.ItemsSource = modules;

            foreach (Module module in modules)
            {
                if (desk.Id == module.DeskId)
                {
                    cbModules.SelectedItem = module;
                }
            }
        }
        #endregion

        #region ClickHandlers
        private void btnAddTable_Click(object sender, RoutedEventArgs e)
        {
            if (currentRoom == null)
            {
                Desk desk = new Desk(int.Parse(tbPeople.Text));
                desk.ModuleId = (Int32)cbModules.SelectedValue;

                maxPersons = maxPersons + int.Parse(tbPeople.Text);

                desks.Add(desk);
                UserControl ucTable = new TableUC() { Capacity = desk.AvailableSpaces.ToString(), Desk = desk };
                lbTables.Items.Add(ucTable);
            }
            else
            {
                Desk desk = new Desk(int.Parse(tbPeople.Text), currentRoom.Id);
                desk.ModuleId = (Int32)cbModules.SelectedValue;

                maxPersons = maxPersons + int.Parse(tbPeople.Text);

                desks.Add(desk);
                UserControl ucTable = new TableUC() { Capacity = desk.AvailableSpaces.ToString(), Desk = desk };
                lbTables.Items.Add(ucTable);
            }
        }


        private async void btnUpdateRoom_Click(object sender, RoutedEventArgs e)
        {
            if (tbName.Text != "" && cbRoomTypes.SelectedValue != null)
            {
                currentRoom.RoomName = tbName.Text;
                currentRoom.RoomTypeId = (Int32)cbRoomTypes.SelectedValue;

                currentRoom.GridLocation = JsonConvert.SerializeObject(lstButtons);
                currentRoom.MaxPersons = maxPersons;

                await UpdateRoom(currentRoom);
                if (desks.Count > 0)
                {
                    await PostDesksUpdate();
                }
            }
            else
            {
                MessageBox.Show("Name is required");
            }
        }

        private async void lbTables_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("Delete Table?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if (lbTables.SelectedItem != null)
                {
                    //do Yes stuff
                    TableUC deskUc = (TableUC)lbTables.SelectedItem;
                    desks.Remove(deskUc.Desk);
                    lbTables.Items.Remove(deskUc);
                    lbTables.SelectedItem = null;
                    await DeleteDesk(deskUc.Desk);
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Delete room tiles?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                //do Yes stuff
                lstButtons.Clear();

                foreach (Button btn in MyCanvas.Children)
                {
                    btn.Background = Brushes.Transparent;
                }
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
            if (tbName.Text != "" && cbRoomTypes.SelectedValue != null)
            {
                Room room = new Room(currentFloor.Id, tbName.Text, maxPersons);
                room.RoomTypeId = (Int32)cbRoomTypes.SelectedValue;

                room.GridLocation = JsonConvert.SerializeObject(lstButtons);

                await PostRoom(room);
                if (desks.Count > 0)
                {
                    await PostDesks();
                }
            }
            else
            {
                MessageBox.Show("Please make sure all fields are filled in correctly. (name and room type)");
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            var mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            mw.Main.Navigate(new AddFloor(currentFloor));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            Button btn2 = new Button();
            btn2.Click += Button_Click;
            btn2.Background = Brushes.Red;

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
        #endregion

        #region APiRequests
        private async Task<string> DeleteDesk(Desk desk)
        {
            HttpResponseMessage response = await ApiHelper.Delete("api/desk/delete/" + desk.Id);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("desk deleted");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return response.ToString();
        }

        private async Task<string> UpdateRoom(Room currentRoom)
        {
            string json = JsonConvert.SerializeObject(currentRoom);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await ApiHelper.Put("api/room/update/" + currentRoom.Id, content);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    tbRoomName.Text = currentRoom.RoomName;
                    MessageBox.Show("Room updated");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return response.ToString();
        }

        private async Task<string> PostDesks()
        {
            string json = JsonConvert.SerializeObject(desks);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await ApiHelper.Post("api/desk/create", content);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Desk(s) saved");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return response.ToString();
        }

        private async Task<string> PostDesksUpdate()
        {
            string json = JsonConvert.SerializeObject(desks);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await ApiHelper.Post("api/desk/create/" + currentRoom.Id, content);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("desk updated");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return response.ToString();
        }

        private async Task<string> PostRoom(Room room)
        {
            string json = JsonConvert.SerializeObject(room);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await ApiHelper.Post("api/room/create", content);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    string test = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(test);
                    //MessageBox.Show(data.last_insert_id.ToString());
                    roomId = data.last_insert_id;

                    foreach (Desk desk in desks)
                    {
                        desk.RoomId = data.last_insert_id;
                    }

                    tbRoomName.Text = room.RoomName;
                    btnSaveRoom.Visibility = Visibility.Collapsed;
                    btnUpdateRoom.Visibility = Visibility.Visible;

                    MessageBox.Show("Room saved");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return response.ToString();

        }

        private async Task<List<RoomType>> GetRoomTypes()
        {
            List<RoomType> roomTypes = null;
            HttpResponseMessage response = await ApiHelper.Get("api/roomtypes");

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    roomTypes = await response.Content.ReadAsAsync<List<RoomType>>();
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }

            return roomTypes;
        }

        private async Task<List<Desk>> GetDesks()
        {
            List<Desk> desks = null;
            HttpResponseMessage response = await ApiHelper.Get("api/desks");

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    desks = await response.Content.ReadAsAsync<List<Desk>>();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return desks;
        }

        private async Task<List<Module>> GetModules()
        {
            List<Module> modules = null;
            HttpResponseMessage response = await ApiHelper.Get("api/modules");

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    modules = await response.Content.ReadAsAsync<List<Module>>();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return modules;
        }
        #endregion

        #region GridMethods
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

            if (currentRoom != null)
            {
                loadRoom();
            }
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
            }
        }
        #endregion
               
        private void cbRoomTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }



        private void lbTables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbTables.SelectedItem != null)
            {
                TableUC desk = (TableUC)lbTables.SelectedItem;
                LoadModules(desk.Desk);
                tbPeople.Text = desk.Desk.AvailableSpaces.ToString();
            }
        }

        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
