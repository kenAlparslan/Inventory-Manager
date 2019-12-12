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
    /// Interaction logic for Order_Product_Inventory.xaml
    /// </summary>
    public partial class Order_Product_Inventory : Page
    {
        private string branchName;
        public Order_Product_Inventory()
        {
            InitializeComponent();
            FillBranches();
        }

        private void ComboBox_Selection_Changed(object sender, SelectionChangedEventArgs e)
        {
            branchName = BranchCB.SelectedItem.ToString();
        }

        private void Order_Inventory_Click(object sender, RoutedEventArgs e)
        {
            string pName = productName.Text;
            string amount = productAmount.Text;
            string bName = branchName;
            DBMS dbms = new DBMS();
            string branchID = "", sku = "";
            if(pName != "" && amount != "" && bName != "")
            {
                branchID = dbms.GetBranchID(bName);
                sku = dbms.GetProductID(pName);

                if(branchID != "" && sku != "")
                {
                    if(dbms.OrderInventory(branchID, sku, amount) == 1)
                    {
                        MessageBox.Show("Order Has Been Placed Successfully");
                    }
                }
            }
        }

        private void FillBranches()
        {
            List<string> branches = new List<string>();
            if (BranchCB.Items.Count < 3)
            {

                DBMS dbms = new DBMS();
                branches = dbms.GetBranches();
                for (int i = 0; i < branches.Count; ++i)
                {
                    BranchCB.Items.Add(branches.ElementAt(i));
                }
            }
        }
    }
}
