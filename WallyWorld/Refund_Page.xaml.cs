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
    /// Interaction logic for Refund_Page.xaml
    /// </summary>
    public partial class Refund_Page : Page
    {
        private string branch;
        private int orderID;
        private int totalQuantity;
        private string sku;
        private string status;
        private int quantitySelected;
        string customerName;
        private Dictionary<string, int> ProNameQuant;
        private string productName;

        public Refund_Page(string branchName)
        {
            InitializeComponent();
            branch = branchName;
            DisplayBranch.Content = "Branch: " + branchName;
            ProNameQuant = new Dictionary<string, int>();
        }

        private void Show_Orders_Click(object sender, RoutedEventArgs e)
        {
            DBMS dbms = new DBMS();
            DataTable dt = new DataTable();
            if (Orders.Columns.Count == 7 || Orders.Columns.Count == 0)
            {
                DataGridTemplateColumn col1 = new DataGridTemplateColumn();
                col1.Header = "Select";

                FrameworkElementFactory factory1 = new FrameworkElementFactory(typeof(Button));
                factory1.SetValue(Button.ContentProperty, "Select");
                factory1.AddHandler(Button.ClickEvent, new RoutedEventHandler(SelectBtn_Click));
                DataTemplate cellTemplate1 = new DataTemplate();
                cellTemplate1.VisualTree = factory1;
                col1.CellTemplate = cellTemplate1;
                Orders.Columns.Add(col1);
            }
            dt = dbms.DisplayOrders();
            Orders.ItemsSource = dt.DefaultView;
        }

        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            DataRowView r;
            r = (DataRowView)((Button)e.Source).DataContext;
            if (r["status"].ToString() != "RFND" && int.Parse(r["quantity"].ToString()) != 0)
            {
                orderID = int.Parse(r["orderID"].ToString());
                totalQuantity = int.Parse(r["quantity"].ToString());
                sku = r["sku"].ToString();
                status = r["status"].ToString();
                customerName = r["Customer_Name"].ToString();
                productName = r["name"].ToString();

                for (int i = 1; i <= totalQuantity; ++i)
                {
                    QuantityForRefund.Items.Add(i);
                }
                QuantityForRefund.SelectedIndex = 0;
                SelectQuantity.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Cannot Refund This Item");
            }
        }

        private void RefundBtn_Click(object sender, RoutedEventArgs e)
        {
            
            DBMS dbms = new DBMS();
            ProNameQuant.Clear();
           
            if (status == "PAID")
            {
                ProNameQuant.Add(productName, quantitySelected);
                if (dbms.RefundOrder(orderID) == 1)
                {
                    //quant = dbms.ReturnQuantity(id);
                    dbms.UpdateDatabaseQuantity(sku, quantitySelected, 1);
                    if (dbms.RefundOrderLine(orderID, quantitySelected) >= 1)
                    {
                        MessageBox.Show("Order Refunded Successfully");
                        this.NavigationService.Navigate(new Refund_Page(branch));
                        Show_Orders_Click(sender, e);
                        Window addCust = new Sales_Record(orderID.ToString(), customerName, branch, ProNameQuant, 0);
                        addCust.Show();
                    }
                }
            }
            else
            {
                MessageBox.Show("This Order have been Refunded");
            }
            
        }
        private void Back_To_Main_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Main_Page(branch));
        }

        private void Quantity_Selection_Changed(object sender, SelectionChangedEventArgs e)
        {
            if(QuantityForRefund.SelectedItem.ToString() != "0" || QuantityForRefund.SelectedItem != null)
            {
                quantitySelected = int.Parse(QuantityForRefund.SelectedItem.ToString());
            }
            
        }
    }
}
