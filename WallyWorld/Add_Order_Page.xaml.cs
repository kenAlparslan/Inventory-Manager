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
        public Add_Order_Page()
        {
            InitializeComponent();
        }

        private void Show_Products_Click(object sender, RoutedEventArgs e)
        {
            DBMS dbms = new DBMS();
            DataTable dt = new DataTable();

            DataGridTemplateColumn col1 = new DataGridTemplateColumn();
            col1.Header = "Select";

            FrameworkElementFactory factory1 = new FrameworkElementFactory(typeof(Button));
            //Binding b1 = new Binding("IsSelected");
            //b1.Mode = BindingMode.TwoWay;
            factory1.SetValue(Button.ContentProperty, "Select");
            factory1.AddHandler(Button.ClickEvent, new RoutedEventHandler(SelectBtn_Click));
            DataTemplate cellTemplate1 = new DataTemplate();
            cellTemplate1.VisualTree = factory1;
            col1.CellTemplate = cellTemplate1;

            Products.Columns.Add(col1);
            dt = dbms.DisplayProducts();
            Products.ItemsSource = dt.DefaultView;

        }
        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
