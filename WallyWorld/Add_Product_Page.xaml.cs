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
    /// Interaction logic for Add_Product_Page.xaml
    /// </summary>
    public partial class Add_Product_Page : Page
    {
        public Add_Product_Page()
        {
            InitializeComponent();
        }

        private void Add_Product_To_Inventory_Click(object sender, RoutedEventArgs e)
        {
            string name = productName.Text;
            string price = productPrice.Text;

            price = "$"+price;
            DBMS dbms = new DBMS();
            if (name != "" && price != "")
            {


                if (dbms.AddNewProduct(name, price) == 1)
                {
                    MessageBox.Show("Product Added Successfully");
                }
                else
                {
                    MessageBox.Show("Error Occured, Please try again");
                }
            }
            else
            {
                MessageBox.Show("Fields cannot be empty");
            }
        }
    }
}
