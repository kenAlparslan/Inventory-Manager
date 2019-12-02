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
    /// Interaction logic for Add_Order_Page.xaml
    /// </summary>
    public partial class Add_Order_Page : Page
    {
        private int quant;
        private decimal pr;
        public Add_Order_Page()
        {
            InitializeComponent();
        }

        private void Show_Products_Click(object sender, RoutedEventArgs e)
        {
            DBMS dbms = new DBMS();
            DataTable dt = new DataTable();

            if (Products.Columns.Count == 4 || Products.Columns.Count == 0)
            {
                DataGridTemplateColumn col1 = new DataGridTemplateColumn();
                col1.Header = "Select";

                FrameworkElementFactory factory1 = new FrameworkElementFactory(typeof(Button));
                factory1.SetValue(Button.ContentProperty, "Select");
                factory1.AddHandler(Button.ClickEvent, new RoutedEventHandler(SelectBtn_Click));
                DataTemplate cellTemplate1 = new DataTemplate();
                cellTemplate1.VisualTree = factory1;
                col1.CellTemplate = cellTemplate1;
                Products.Columns.Add(col1);
            }
            
            dt = dbms.DisplayProducts();
            Products.ItemsSource = dt.DefaultView;

        }
        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            DataRowView r = (DataRowView)((Button)e.Source).DataContext;
            int id = int.Parse(r["SKU"].ToString());
            string name = r["name"].ToString();
            int totalStock = int.Parse(r["stock"].ToString());
            decimal price = decimal.Parse(r["wPrice"].ToString(), System.Globalization.NumberStyles.Currency);
            List<string> branches = new List<string>();
            pr = price;
            for(int i=1; i<=totalStock; ++i)
            {
                stockCB.Items.Add(i);
            }

            DBMS dbms = new DBMS();
            branches = dbms.GetBranches();
            for(int i=0; i<branches.Count; ++i)
            {
                BranchCB.Items.Add(branches.ElementAt(i));
            }

            NameTB.Text = name;
            CalculatedTotal.Text = "$"+(price * quant).ToString();
            Product_Border.Visibility = Visibility.Visible;
            
        }

        private void Add_To_Cart_Click(object sender, RoutedEventArgs e)
        {

        }

        private void New_Customer_Click(object sender, RoutedEventArgs e)
        {
            Window addCust = new Create_Customer();
            addCust.Show();
            
            
        }

        private void ComboBox_Selection_Changed(object sender, SelectionChangedEventArgs e)
        {
            quant = int.Parse(stockCB.SelectedItem.ToString());
            CalculatedTotal.Text = "$" + (pr * quant).ToString();
        }
    }
}
