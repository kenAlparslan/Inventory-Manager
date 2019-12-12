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
    /// Interaction logic for Manage_Products.xaml
    /// </summary>
    public partial class Manage_Products : Page
    {
        public Manage_Products()
        {
            InitializeComponent();
        }

        private void New_Product_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Add_Product_Page());
        }

        private void Order_Product_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Order_Product_Inventory());
        }

        private void Discontinued_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Toggle_Discontinue_Page());
        }

        private void View_Pro_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new View_Product_Page());
        }
    }
}
