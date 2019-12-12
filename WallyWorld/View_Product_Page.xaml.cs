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
    /// Interaction logic for View_Product_Page.xaml
    /// </summary>
    public partial class View_Product_Page : Page
    {
        public View_Product_Page()
        {
            InitializeComponent();
        }

        private void View_Click(object sender, RoutedEventArgs e)
        {
            string name = productName.Text;
            DBMS dbms = new DBMS();
            DataTable dt = new DataTable();
            if(name != "")
            {
                dt = dbms.GetProductInfo(name);
                if(dt != null)
                {
                    Product.ItemsSource = dt.DefaultView;
                }
            }
            productName.Text = "";
        }
    }
}
