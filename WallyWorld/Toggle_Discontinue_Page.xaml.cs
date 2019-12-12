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
    /// Interaction logic for Toggle_Discontinue_Page.xaml
    /// </summary>
    public partial class Toggle_Discontinue_Page : Page
    {
        private string id;
        private string proName;
        public Toggle_Discontinue_Page()
        {
            InitializeComponent();
        }

        private void Select_Product_Click(object sender, RoutedEventArgs e)
        {
            string name = productName.Text;
            DBMS dbms = new DBMS();
            string sku = "", discontinuation = "";
            if(name != "")
            {
                proName = name;
                sku = dbms.GetProductID(name);
                if(sku != "")
                {
                    id = sku;
                    discontinuation = dbms.GetDiscontinuationStatus(sku);
                    if(discontinuation == "1" || discontinuation == "-1")
                    {
                        if(discontinuation == "1")
                        {
                            toggleDisc.Content = "Discontinue";
                            toggleDisc.Visibility = Visibility.Visible;
                            
                        }
                        else
                        {
                            toggleDisc.Content = "Continue";
                            toggleDisc.Visibility = Visibility.Visible;
                            
                        }
                    }
                }
            }
        }

        private void Toggle_Discontinue_Click(object sender, RoutedEventArgs e)
        {
            DBMS dbms = new DBMS();
            if (dbms.ToggleDiscontinue(id) == 1)
            {
                if(toggleDisc.Content.ToString() == "Discontinue")
                {
                    MessageBox.Show(proName + " Discontinued Successfully");
                }
                else
                {
                    MessageBox.Show(proName + " Continued Successfully");
                }
            }
            this.NavigationService.Navigate(new Toggle_Discontinue_Page());
        }
    }
}
