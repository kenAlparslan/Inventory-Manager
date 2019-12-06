/*
 * Author: Ken Alparslan
 * Date: 03-12-2019
 * Description: This page has the logic for selecting a branch
 */

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
    /// Interaction logic for Branch_Page.xaml
    /// </summary>
    public partial class Branch_Page : Page
    {
        private string branchN;
        public Branch_Page()
        {
            InitializeComponent();
            FillBranches();
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
            if(BranchCB.SelectedItem == null)
            {
                MessageBox.Show("A Branch has to be selected");
            }
            else
            {
                this.NavigationService.Navigate(new Main_Page(branchN));
            }
            
        }

        /*
         *  Function    : FillBranches
         *  Description : This function is gets the branches from the database and fills the combobox
         *      
         *
         *  Parameters  : void
         *      
         *  Returns     : void
         *      
         */
        private void FillBranches()
        {
            List<string> branches = new List<string>();
            if (BranchCB.Items.Count < 3)
            {

                DBMS dbms = new DBMS();
                branches = dbms.GetBranches();
                for (int i = 0; i < branches.Count; ++i)
                {
                    BranchCB.Items.Add(branches.ElementAt(i));
                }
            }
        }

        /*
         *  Function    : ComboBox_Selection_Changed
         *  Description : This function updates the calculated total cost
         *      
         *
         *  Parameters  : object sender, RoutedEventArgs e
         *      
         *  Returns     : void
         *      
         */
        private void ComboBox_Selection_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (BranchCB.SelectedItem != null)
            {
                branchN = BranchCB.SelectedItem.ToString();
            }

        }

        
    }
}
