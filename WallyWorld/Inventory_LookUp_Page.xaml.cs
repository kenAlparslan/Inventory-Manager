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
    /// Interaction logic for Inventory_LookUp_Page.xaml
    /// </summary>
    public partial class Inventory_LookUp_Page : Page
    {
        private string branch;
        public Inventory_LookUp_Page(string branchName)
        {
            InitializeComponent();
            branch = branchName;
            DisplayBranch.Content = "Branch: " + branchName;
        }

        private void Show_Inventory_Click(object sender, RoutedEventArgs e)
        {
            DBMS dbms = new DBMS();
            DataTable dt = new DataTable();

            dt = dbms.DisplayInventory();
            Inventory.ItemsSource = dt.DefaultView;
        }
        private void Back_To_Main_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Main_Page(branch));
        }
    }
}
