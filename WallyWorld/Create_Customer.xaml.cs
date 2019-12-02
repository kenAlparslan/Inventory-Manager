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
    /// Interaction logic for Create_Customer.xaml
    /// </summary>
    public partial class Create_Customer : Window
    {
        public Create_Customer()
        {
            InitializeComponent();
        }

        private void Add_Cust_Click(object sender, RoutedEventArgs e)
        {
            string firstName = firstN.Text;
            string lastName = lastN.Text;
            string phoneNum = phoneN.Text;
            bool status;

            if (Add_Customer_Page.ValidateString(firstName) == true)
            {
                if (Add_Customer_Page.ValidateString(lastName) == true)
                {
                    if (phoneNum.Length == 12)
                    {
                        status = true;
                    }
                    else
                    {
                        status = false;
                    }
                }
                else
                {
                    status = false;
                }
            }
            else
            {
                status = false;
            }

            if (status)
            {
                DBMS dbms = new DBMS();

                try
                {
                    int retCode = dbms.CreateCustomer(firstName, lastName, phoneNum);
                    if (retCode == 1)
                    {
                        MessageBox.Show("New customer added successfully");
                        
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch (Exception)
                {
                    errorMsg.Visibility = Visibility.Visible;
                }

            }

        }
    }
    
}
