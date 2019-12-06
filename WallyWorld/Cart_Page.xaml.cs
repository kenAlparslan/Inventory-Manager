/*
 * Author: Ken Alparslan
 * Date: 03-12-2019
 * Description: This page has the logic need for the performing operations on the Cart
 */

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
    /// Interaction logic for Cart.xaml
    /// </summary>
    public partial class Cart_Page : Page
    {
        private string customerName;
        //private List<string> ProductName;
        private Dictionary<string, int> ProNameQuant;
        private int quantity;
        private int totalQuantity;
        private string branchName;
        private decimal cost;
        private List<string> sessionID;
        

        public Cart_Page(string bName)
        {
            InitializeComponent();
            //ProductName = new List<string>();
            sessionID = new List<string>();
            ProNameQuant = new Dictionary<string, int>();
            branchName = bName;
            DisplayBranch.Content = "Branch: " + branchName;
        }
        /*
         *  Function    : Show_Cart_Click
         *  Description : Display the Cart Data Grid
         *      
         *
         *  Parameters  : object sender, RoutedEventArgs e
         *      
         *  Returns     : void
         *      
         */
        private void Show_Cart_Click(object sender, RoutedEventArgs e)
        {
            DBMS dbms = new DBMS();
            DataTable dt = new DataTable();

            if (Cart.Columns.Count == 7 || Cart.Columns.Count == 0)
            {
                DataGridTemplateColumn col1 = new DataGridTemplateColumn();
                col1.Header = "Checkout";

                FrameworkElementFactory factory1 = new FrameworkElementFactory(typeof(Button));
                factory1.SetValue(Button.ContentProperty, "Checkout");
                factory1.AddHandler(Button.ClickEvent, new RoutedEventHandler(CheckoutBtn_Click));
                DataTemplate cellTemplate1 = new DataTemplate();
                cellTemplate1.VisualTree = factory1;
                col1.CellTemplate = cellTemplate1;
                Cart.Columns.Add(col1);
            }

            dt = dbms.DisplayCart();
            Cart.ItemsSource = dt.DefaultView;
        }
        /*
         *  Function    : CheckoutBtn_Click
         *  Description : Display the Cart Data Grid
         *      
         *
         *  Parameters  : object sender, RoutedEventArgs e
         *      
         *  Returns     : void
         *      
         */
        private void CheckoutBtn_Click(object sender, RoutedEventArgs e)
        {
            DataRowView r;
            if (totalP.IsVisible == false)
            {
                r = (DataRowView)((Button)e.Source).DataContext;
                int id = int.Parse(r["SKU"].ToString());
                string cName = r["customerName"].ToString();
                string pName = r["productName"].ToString();
                string bName = r["branchName"].ToString();
                int totalStock = int.Parse(r["quantity"].ToString());
                decimal price = decimal.Parse(r["price"].ToString(), System.Globalization.NumberStyles.Currency);

                sessionID.Add(r["sessionID"].ToString());
                customerName = cName;
                //ProductName.Add(pName);
                //branchName = bName;
                quantity = totalStock;
                cost = price;
                totalP.Text = "Total: $" + (cost + cost * (decimal)0.13).ToString();
                totalP.Visibility = Visibility.Visible;
                ProNameQuant.Add(pName, totalStock);
                
            }
            else
            {
                r = (DataRowView)((Button)e.Source).DataContext;
                if (sessionID.Contains(r["sessionID"].ToString()) == false)
                {

                    if (customerName == r["customerName"].ToString())
                    {

                        if (branchName == r["branchName"].ToString())
                        {
                            sessionID.Add(r["sessionID"].ToString());
                            //ProductName.Add(r["productName"].ToString());
                            if (ProNameQuant.ContainsKey(r["productName"].ToString()))
                            {
                                ProNameQuant[r["productName"].ToString()] += int.Parse(r["quantity"].ToString());
                            }
                            else
                            {
                                ProNameQuant.Add(r["productName"].ToString(), int.Parse(r["quantity"].ToString()));
                            }
                            cost += decimal.Parse(r["price"].ToString(), System.Globalization.NumberStyles.Currency);
                            quantity += int.Parse(r["quantity"].ToString());
                            totalP.Text = "Total: $" + (cost + cost * (decimal)0.13).ToString();
                        }
                        else
                        {
                            MessageBox.Show("Different Branch");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Different Customer");
                    }
                }
                else
                {
                    MessageBox.Show("This Item is already added");
                }
            }

        }

        /*
         *  Function    : OrderBtn_Click
         *  Description : Display the Cart Data Grid
         *      
         *
         *  Parameters  : object sender, RoutedEventArgs e
         *      
         *  Returns     : void
         *      
         */
        private void OrderBtn_Click(object sender, RoutedEventArgs e)
        {
            DBMS dbms = new DBMS();
            string orderID = "";
            int status = 0;
            int retCode;
            if (sessionID.Count != 0)
            {
                
                string custID = dbms.GetCustomerID(customerName);
                string bID = dbms.GetBranchID(branchName);
                int retCode1 = dbms.AddOrder(custID, bID);
                if (retCode1 == 1)
                {
                    foreach (var key in ProNameQuant.Keys)
                    {
                        string proID = dbms.GetProductID(key.ToString());

                        orderID = dbms.GetOrderID();
                        retCode = dbms.AddOrderLine(orderID, proID, ProNameQuant[key].ToString());
                        if (retCode != 1)
                        {
                            status = 1;
                            break;
                        }
                    }
                }
                else
                {
                    status = 1;   
                }

                if (status == 0)
                {
                    foreach (var i in sessionID)
                    {
                        dbms.RemoveFromCart(i);
                    }
                    Show_Cart_Click(sender, e);
                    MessageBox.Show("Order Created Successfully");
                    Window addCust = new Sales_Record(orderID, customerName, branchName, ProNameQuant, 1);
                    addCust.Show();
                    totalP.Text = "";
                }
                else
                {
                    MessageBox.Show("Error Occured, Please try again");
                }
                customerName = "";
                ProNameQuant.Clear();
                quantity = 0;
                //branchName = "";
                cost = 0; ;
                sessionID.Clear();
            }
            else
            {
                MessageBox.Show("No product is selected");
            }
        }

        /*
         *  Function    : CancelBtn_Click
         *  Description : Cancels the order
         *      
         *
         *  Parameters  : object sender, RoutedEventArgs e
         *      
         *  Returns     : void
         *      
         */
        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Cart_Page(branchName));
            Show_Cart_Click(sender, e);
        }

        /*
         *  Function    : RemoveBtn_Click
         *  Description : Removes the Order from the cart
         *      
         *
         *  Parameters  : object sender, RoutedEventArgs e
         *      
         *  Returns     : void
         *      
         */
        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            DBMS dbms = new DBMS();
            if (sessionID.Count != 0)
            {

                foreach (var key in ProNameQuant.Keys)
                {
                    string proID = dbms.GetProductID(key.ToString());
                    dbms.UpdateDatabaseQuantity(proID, ProNameQuant[key], 1);
                }
                foreach (var i in sessionID)
                {
                    dbms.RemoveFromCart(i);
                }
                this.NavigationService.Navigate(new Cart_Page(branchName));
                Show_Cart_Click(sender, e);
                MessageBox.Show("Product removed from the Cart");
            }
            else
            {
                MessageBox.Show("No Product is selected");
            }
            
        }
        /*
         *  Function    : Back_To_Main_Click
         *  Description : This function is used to navigate to the main page
         *      
         *
         *  Parameters  : object sender, RoutedEventArgs e
         *      
         *  Returns     : void
         *      
         */
        private void Back_To_Main_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Main_Page(branchName));
        }
    }
}
