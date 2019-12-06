
/*
 * Author: Ken Alparslan
 * Date: 03-12-2019
 * Description: This page has the logic needed for creating a Sales Record
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
using System.Windows.Shapes;

namespace WallyWorld
{
    /// <summary>
    /// Interaction logic for Sales_Record.xaml
    /// </summary>
    public partial class Sales_Record : Window
    {
       
        public Sales_Record(string orderid, string custName, string branchName, Dictionary<string, int> ProductInfo, int saleOrRefund)
        {
            InitializeComponent();
            InitializeForm(orderid, custName, branchName, ProductInfo, saleOrRefund);
            
        }

        /*
         *  Function    : InitializeForm
         *  Description : This function is used to initialize the form with appropriate values related to the Order/Refund
         *      
         *
         *  Parameters  : object sender, RoutedEventArgs e
         *      
         *  Returns     : void
         *      
         */
        public void InitializeForm(string orderid, string custName, string branchName, Dictionary<string, int> ProductInfo, int saleRefund)
        {
            BranchName.Content = "Wally's World " + branchName;
            DBMS dbms = new DBMS();
            string date = dbms.GetOrderDate(orderid);
            CustomerNameDate.Content = date + ", " + custName + "!";
            if(saleRefund == 1)
            {
                OrderID.Content = "Order ID: " + orderid+ ",  Status: PAID";
            }
            else
            {
                OrderID.Content = "Order ID: " + orderid + ",  Status: REFUND";
            }
          
            string unitPrice = "";
            decimal totalProductCost;
            decimal salesTotal = 0;
            decimal tax;
            int status = saleRefund;
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
            if(status == 1)
            {
                Thanks.Text = "Paid - Thank You!";
            }
            else
            {
                Thanks.Text = "Refund - Thank You!";
            }
        }
        
    }
}
