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
        public Cart_Page()
        {
            InitializeComponent();
        }

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
                factory1.AddHandler(Button.ClickEvent, new RoutedEventHandler(SelectBtn_Click));
                DataTemplate cellTemplate1 = new DataTemplate();
                cellTemplate1.VisualTree = factory1;
                col1.CellTemplate = cellTemplate1;
                Cart.Columns.Add(col1);
            }

            dt = dbms.DisplayCart();
            Cart.ItemsSource = dt.DefaultView;
        }

        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
