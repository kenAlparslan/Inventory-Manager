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
        private string branch;
        public Main_Page(string branchName)
        {
            InitializeComponent();
            branch = branchName;
            DisplayBranch.Content = "Branch: " + branchName;
        }

        private void New_Customer_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Add_Customer_Page(branch));
        }

        private void New_Order_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Add_Order_Page(branch));
        }

        private void Refund_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Refund_Page(branch));
        }

        private void Order_Lookup_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Order_LookUp_Page(branch));
        }

        private void Inventory_Lookup_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Inventory_LookUp_Page(branch));
        }

        private void Select_Branch_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Branch_Page());
        }
    }
}
