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

        private const int size = 20;
        //private const int space = 0;

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
    }
}
