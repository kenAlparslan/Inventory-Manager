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
using System.Windows.Shapes;

namespace WallyWorld
{
    /// <summary>
    /// Interaction logic for Sales_Record.xaml
    /// </summary>
    public partial class Sales_Record : Window
    {
       
        public Sales_Record(string orderid, string custName, string branchName, Dictionary<string, int> ProductInfo)
        {
            InitializeComponent();
            InitializeForm(orderid, custName, branchName, ProductInfo);
            
        }

        public void InitializeForm(string orderid, string custName, string branchName, Dictionary<string, int> ProductInfo)
        {
            BranchName.Content = "Wally's World " + branchName;
            DBMS dbms = new DBMS();
            string date = dbms.GetOrderDate(orderid);
            CustomerNameDate.Content = date + ", " + custName + "!";
            OrderID.Content = "Order ID: "+orderid;
            string unitPrice = "";
            decimal totalProductCost;
            decimal salesTotal = 0;
            decimal tax;
            foreach(var key in ProductInfo.Keys)
            {
                unitPrice = dbms.GetUnitPrice(key);
                totalProductCost = decimal.Parse(unitPrice, System.Globalization.NumberStyles.Currency) * decimal.Parse(ProductInfo[key].ToString());
                salesTotal += totalProductCost;
                Label label = new Label();
                label.Content = key + " " + ProductInfo[key].ToString() + " x " + unitPrice + " = " + "$"+totalProductCost.ToString();
                Products.Children.Add(label);
            }
            tax = salesTotal * (decimal)0.13;
            SubTotal.Content = "Subtotal = $" + salesTotal.ToString();
            Tax.Content = "HST (13%) = $" + tax;
            SaleTotal.Content = "Sale Total = $" + (tax + salesTotal).ToString();
        }
        
    }
}
