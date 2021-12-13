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
using Microsoft.Win32;

namespace SpotFinder.Pages
{
    /// <summary>
    /// Interaction logic for Canvas.xaml
    /// </summary>
    public partial class Canvas : Window
    {
        private int heightCanvas = 60;
        private int widthCanvas = 100;
        private Point startPoint;
        private Point pointWhereMouseIs;

        // private Canvas startPoint;
        Rectangle selectionRectangle = new Rectangle();

        private const int size = 20;
        //private const int space = 0;

        private List<double> clickedBtn;

        public Canvas()
        {
            InitializeComponent();
            DrawButtons(MyCanvas);
        }

        //change color of button
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            Button btn2 = new Button();
            btn2.Click += Button_Click;
            btn2.Background = Brushes.Red;

            //MyCanvas.Children.Remove(10);
            //MyCanvas.Children.Insert(10, button2);

            if (btn.Background == Brushes.Purple)
                btn.Background = null;
            else
            {
                btn.Background = Brushes.Purple;
            }
        }

        public void DrawButtons(System.Windows.Controls.Canvas MyCanvas)
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

        //uploaden image
        private void UploadImage_Click(object sender, RoutedEventArgs e)
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

        //inzoomen met button
        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            canvasZoom.ScaleX += 0.1;
            canvasZoom.ScaleY += 0.1;
        }

        //uitzoomen met button
        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            canvasZoom.ScaleX -= 0.1;
            canvasZoom.ScaleY -= 0.1;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
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

        //private int GetIndex(Button btn)
        //{
        //    //with the index number you can find the canvas.Children[1]
        //    double width = Canvas.GetLeft(btn) / size;
        //    double height = Canvas.GetTop(btn) / size;
        //    double index = width * height;

        //    //index has to be integer
        //    return Convert.ToInt32(index);
        //}

        // '' <summary>
        // '' When Left Mouse button is pressed, remember where the mouse move start
        // '' </summary>
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
            double top;
            double left;

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
                    top = System.Windows.Controls.Canvas.GetTop(btn);
                    left = System.Windows.Controls.Canvas.GetLeft(btn);
                }
                else
                {
                    //ThisChkBox.Background = Brushes.Transparent;
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
                    }
                }
                //btn.Background = Brushes.Transparent;
            }
        }

    }
}
