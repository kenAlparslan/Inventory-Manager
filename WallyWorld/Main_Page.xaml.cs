/*
 * Author: Ken Alparslan
 * Date: 03-12-2019
 * Description: This page has the logic needed for navingating to different pages in the program
 */

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

        /*
         *  Function    : New_Customer_Click
         *  Description : This function is used to navigate to the Page for adding Customer
         *      
         *
         *  Parameters  : object sender, RoutedEventArgs e
         *      
         *  Returns     : void
         *      
         */
        private void New_Customer_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Add_Customer_Page(branch));
        }

        /*
         *  Function    : New_Customer_Click
         *  Description : This function is used to navigate to the Page for making Orders
         *      
         *
         *  Parameters  : object sender, RoutedEventArgs e
         *      
         *  Returns     : void
         *      
         */
        private void New_Order_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Add_Order_Page(branch));
        }

        /*
         *  Function    : Refund_Click
         *  Description : This function is used to navigate to the Page for making Refunds
         *      
         *
         *  Parameters  : object sender, RoutedEventArgs e
         *      
         *  Returns     : void
         *      
         */
        private void Refund_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Refund_Page(branch));
        }

        /*
         *  Function    : Order_Lookup_Click
         *  Description : This function is used to navigate to the Page for Looking up Orders
         *      
         *
         *  Parameters  : object sender, RoutedEventArgs e
         *      
         *  Returns     : void
         *      
         */
        private void Order_Lookup_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Order_LookUp_Page(branch));
        }

        /*
         *  Function    : Inventory_Lookup_Click
         *  Description : This function is used to navigate to the Page for Looking up Inventory
         *      
         *
         *  Parameters  : object sender, RoutedEventArgs e
         *      
         *  Returns     : void
         *      
         */
        private void Inventory_Lookup_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Inventory_LookUp_Page(branch));
        }

        /*
         *  Function    : Inventory_Lookup_Click
         *  Description : This function is used to navigate to the Page for Selecting a branch
         *      
         *
         *  Parameters  : object sender, RoutedEventArgs e
         *      
         *  Returns     : void
         *      
         */
        private void Select_Branch_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Branch_Page());
        }

        private void Add_Product_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Manage_Products());
        }
    }
}
