using SpotFinder.Classes;
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
        public AddRoom(AddFloor currentFloor)
        {
            this.currentFloor = currentFloor;
            InitializeComponent();

            this.Height = System.Windows.SystemParameters.VirtualScreenHeight - 125;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).ChangeMenuContent(currentFloor);
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

        private void btnSaveRoom_Click(object sender, RoutedEventArgs e)
        {
            Room room = new Room();
        }
    }
}
