using System;
using System.Collections.Generic;
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
    /// Interaction logic for Main_Page.xaml
    /// </summary>
    public partial class Main_Page : Page
    {
        public Main_Page()
        {
            InitializeComponent();
        }

        private void New_Customer_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Add_Customer_Page());
        }

        private void New_Order_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Add_Order_Page());
        }

        private void Refund_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Refund_Page());
        }

        private void Order_Lookup_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Order_LookUp_Page());
        }

        private void Inventory_Lookup_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Inventory_LookUp_Page());
        }
    }
}
