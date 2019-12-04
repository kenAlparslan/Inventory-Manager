/*
 * Author: Ken Alparslan
 * Date: 03-12-2019
 * Description: This page has the logic for adding a new customer into the database
 */

using MySql.Data.MySqlClient;
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
    /// Interaction logic for Add_Customer_Page.xaml
    /// </summary>
    public partial class Add_Customer_Page : Page
    {
        private string branch;
        public Add_Customer_Page(string branchName)
        {
            InitializeComponent();
            branch = branchName;
            DisplayBranch.Content = "Branch: " + branchName;
        }
        /*
         *  Function    : Add_Customer_Click
         *  Description : Inserts the new Customer into the database
         *      
         *
         *  Parameters  : object sender, RoutedEventArgs e
         *      
         *  Returns     : void
         *      
         */
        public void Add_Customer_Click(object sender, RoutedEventArgs e)
        {
            string firstName = firstN.Text;
            string lastName = lastN.Text;
            string phoneNum = phoneN.Text;
            bool status;

            if(ValidateString(firstName) == true)
            {
                if(ValidateString(lastName) == true)
                {
                    if(phoneNum.Length == 12)
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

            if(status)
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
                catch(Exception)
                {
                    errorMsg.Visibility = Visibility.Visible;
                }
                
            }
            else
            {
                errorMsg.Visibility = Visibility.Visible;
            }
            
        }
        /*
         *  Function    : ValidateString
         *  Description : This function validates the string entered into the program
         *      
         *
         *  Parameters  : string string1
         *      
         *  Returns     : bool
         *      
         */
        public static bool ValidateString(string string1)
        {
            List<string> invalidChars = new List<string>() { "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "-" };
            List<string> numbers = new List<string>() { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            // Check for length
            if (string1.Length > 100)
            {
                return false;
            }
            else
            {
                //Iterate your list of invalids and check if input has one
                foreach (string s in invalidChars)
                {
                    if (string1.Contains(s))
                    {
                        return false;
                    }
                }
                foreach (string s in numbers)
                {
                    if (string1.Contains(s))
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        /*
         *  Function    : Refresh_Click
         *  Description : This function refreshes the form and clears the fileds
         *      
         *
         *  Parameters  : object sender, RoutedEventArgs e
         *      
         *  Returns     : void
         *      
         */
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Add_Customer_Page(branch));
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
            this.NavigationService.Navigate(new Main_Page(branch));
        }
    }
 
}
