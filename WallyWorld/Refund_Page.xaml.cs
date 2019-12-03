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
        public Refund_Page()
        {
            InitializeComponent();
        }

        private void Show_Orders_Click(object sender, RoutedEventArgs e)
        {
            DBMS dbms = new DBMS();
            DataTable dt = new DataTable();
            if (Orders.Columns.Count == 7 || Orders.Columns.Count == 0)
            {
                DataGridTemplateColumn col1 = new DataGridTemplateColumn();
                col1.Header = "Refund";

                FrameworkElementFactory factory1 = new FrameworkElementFactory(typeof(Button));
                factory1.SetValue(Button.ContentProperty, "Refund");
                factory1.AddHandler(Button.ClickEvent, new RoutedEventHandler(RefundBtn_Click));
                DataTemplate cellTemplate1 = new DataTemplate();
                cellTemplate1.VisualTree = factory1;
                col1.CellTemplate = cellTemplate1;
                Orders.Columns.Add(col1);
            }
            dt = dbms.DisplayOrders();
            Orders.ItemsSource = dt.DefaultView;
        }

        private void RefundBtn_Click(object sender, RoutedEventArgs e)
        {
            DataRowView r;
            DBMS dbms = new DBMS();
            string quant;
            int quantity;
            string sku;
            r = (DataRowView)((Button)e.Source).DataContext;
            int id = int.Parse(r["orderID"].ToString());
            if (r["status"].ToString() == "PAID")
            {

                if (dbms.RefundOrder(id) == 1)
                {
                    quant = dbms.ReturnQuantity(id);
                    quantity = int.Parse(quant.ToString());
                    sku = dbms.ReturnSKUFromOL(id);
                    dbms.UpdateDatabaseQuantity(sku, quantity, 1);
                    if (dbms.RefundOrderLine(id) == 1)
                    {
                        MessageBox.Show("Order Refunded Successfully");
                        this.NavigationService.Navigate(new Refund_Page());
                    }
                }
            }
            else
            {
                MessageBox.Show("This Order have been Refunded");
            }
            
        }
    }
}
