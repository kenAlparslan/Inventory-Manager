using System;
using System.Collections.Generic;
using System.Data;
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

namespace WallyWorld
{
    /// <summary>
    /// Interaction logic for Reports_Page.xaml
    /// </summary>
    public partial class Reports_Page : Page
    {
        public Reports_Page()
        {
            InitializeComponent();
        }

        private void Show_Sales_Click(object sender, RoutedEventArgs e)
        {
            DBMS dbms = new DBMS();
            DataTable dt = new DataTable();
            ProductsReport.Visibility = Visibility.Hidden;
            SalesReport.Visibility = Visibility.Visible;
            dt = dbms.ShowSalesReport();
            SalesReport.ItemsSource = dt.DefaultView;
        }

        private void Show_Products_Click(object sender, RoutedEventArgs e)
        {
            DBMS dbms = new DBMS();
            DataTable dt = new DataTable();
            SalesReport.Visibility = Visibility.Hidden;
            ProductsReport.Visibility = Visibility.Visible;
            dt = dbms.ProductVolumeReport();
            ProductsReport.ItemsSource = dt.DefaultView;
        }
    }
}
