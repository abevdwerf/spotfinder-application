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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpotFinder.UserControls
{
    /// <summary>
    /// Interaction logic for TableUC.xaml
    /// </summary>
    public partial class TableUC : UserControl
    {
        public string Capacity
        {
            set { capacity.Text = value; }
        }
        public Desk Desk { get; set; }

        public TableUC()
        {
            InitializeComponent();
        }
    }
}
