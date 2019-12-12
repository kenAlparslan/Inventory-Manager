/*
 * Author: Ken Alparslan
 * Date: 03-12-2019
 * Description: This page has the logic needed for communicating with the database
 */

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WallyWorld
{
    class DBMS
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        /*
         *  Function    : CreateCustomer
         *  Description : This function adds a new customer
         *      
         *
         *  Parameters  : string firstName, string lastName, string phoneNumber
         *      
         *  Returns     : int
         *      
         */
        public int CreateCustomer(string firstName, string lastName, string phoneNumber)
        {
            int result;

            string sqlStatement = @" insert into Customer (first_Name , last_Name , telephone)
                                     values (@f, @l, @p);";

            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(sqlStatement, connection);

            command.Parameters.AddWithValue("@f", firstName);
            command.Parameters.AddWithValue("@l", lastName);
            command.Parameters.AddWithValue("@p", phoneNumber);

            connection.Open();
            result = command.ExecuteNonQuery();


            connection.Close();
            command.Dispose();

            return result;
        }

        /*
         *  Function    : DisplayProducts
         *  Description : This function returns the product table from the database
         *      
         *
         *  Parameters  : void
         *      
         *  Returns     : DataTable
         *      
         */
        public DataTable DisplayProducts(string branchName)
        {

            //string sqlStatement = @"select * from product";
            string sqlStatement = @"select sku,name, wprice, stock, branchname from inventoryInfo where branchname = @br and stock != 0;";
            DataTable dt = new DataTable();
         
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(sqlStatement, connection);

            command.Parameters.AddWithValue("@br", branchName);
            connection.Open();

            //using (MySqlDataAdapter da = new MySqlDataAdapter(sqlStatement, connection))
            using (MySqlDataAdapter da = new MySqlDataAdapter(command))
                da.Fill(dt);

            connection.Close();
            return dt;
        }

        /*
         *  Function    : GetBranches
         *  Description : This function returns the branch table from the database
         *      
         *
         *  Parameters  : void
         *      
         *  Returns     : List<string> : list of branches
         *      
         */
        public List<string> GetBranches()
        {
            string sqlStatement = @"select branchName from branch;";
            List<string> result = new List<string>();
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(sqlStatement, connection);
            int i = 0;
            connection.Open();
            using (MySqlDataReader rdr = command.ExecuteReader())
            {
                while(rdr.Read())
                {
                    result.Add(rdr["branchName"].ToString());
                    ++i;
                }

            }

            connection.Close();
            command.Dispose();

            return result;

        }

        /*
         *  Function    : DisplayCart
         *  Description : This function returns the items from the cart table
         *      
         *
         *  Parameters  : void
         *      
         *  Returns     : DataTable
         *      
         */
        public DataTable DisplayCart()
        {

            string sqlStatement = @"select * from cart";
            DataTable dt = new DataTable();
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            using (MySqlDataAdapter da = new MySqlDataAdapter(sqlStatement, connection))
                da.Fill(dt);

            connection.Close();
            return dt;
        }

        /*
         *  Function    : DisplayOrders
         *  Description : This function creates a view from joining multiple tables together and returns the items from the view
         *      
         *
         *  Parameters  : void
         *      
         *  Returns     : DataTable
         *      
         */
        public DataTable DisplayOrders()
        {
            string mySqlView = @"   drop view if exists OrderDetails;
                                    create view OrderDetails
                                    as
                                    select orderLine.orderID ,orderDate, `status`, concat(first_name, ' ', last_name) as Customer_Name, branchName , orderline.sku, name, quantity from orderline
                                    inner join `order` on orderLine.orderID = `order`.orderID
                                    inner join branch on `order`.branchID = branch.branchID
                                    inner join customer on `order`.customerID = customer.customerID
                                    inner join product on orderLine.sku = product.sku
                                    Order by orderLine.orderID;";

            string sqlStatement = @"select * from orderDetails";

            DataTable dt = new DataTable();
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            MySqlCommand command = new MySqlCommand(mySqlView, connection);
            command.ExecuteNonQuery();

            using (MySqlDataAdapter da = new MySqlDataAdapter(sqlStatement, connection))
                da.Fill(dt);

            connection.Close();
            command.Dispose();
            return dt;
        }

        /*
         *  Function    : DisplayInventory
         *  Description : This function gets the product name and available stock from the products table
         *      
         *
         *  Parameters  : void
         *      
         *  Returns     : DataTable
         *      
         */
        public DataTable DisplayInventory()
        {

            string sqlStatement = @"select name, wprice, stock, discontinued, branchName from inventoryInfo";
            DataTable dt = new DataTable();
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            using (MySqlDataAdapter da = new MySqlDataAdapter(sqlStatement, connection))
                da.Fill(dt);

            connection.Close();
            return dt;
        }

        /*
         *  Function    : AddCart
         *  Description : This function creates a new item in the cart
         *      
         *
         *  Parameters  : string SKU, string customerName, string productName, string branchName, int quantity, string price
         *      
         *  Returns     : int
         *      
         */
        public int AddCart(string SKU, string customerName, string productName, string branchName, int quantity, string price)
        {
            int result;
            string sqlStatement = @" insert into Cart (SKU, CustomerName , ProductName , branchName,  quantity, price)
                                     values (@s, @cN, @pN, @b, @q, @p);";

            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(sqlStatement, connection);
            command.Parameters.AddWithValue("@s", SKU);
            command.Parameters.AddWithValue("@cN", customerName);
            command.Parameters.AddWithValue("@pN", productName);
            command.Parameters.AddWithValue("@b", branchName);
            command.Parameters.AddWithValue("@q", quantity);
            command.Parameters.AddWithValue("@p", price);

            connection.Open();
            result = command.ExecuteNonQuery();
            connection.Close();
            command.Dispose();
            return 0;
        }

        /*
         *  Function    : SearchCustomer
         *  Description : This function searches for a customer in the database
         *      
         *
         *  Parameters  : string s, int status
         *      
         *  Returns     : string
         *      
         */
        public string SearchCustomer(string s, int status)
        {
            string result = "";
            string sqlStatement;
            if (status == 0) // phone number
            {
                sqlStatement = @"select concat(first_name , ' ' , last_name) as `name` from customer where telephone = @t;";
            }
            else //last name
            {
                sqlStatement = @"select concat(first_name , ' ' , last_name) as `name` from customer where last_name = @t;";
            }

            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(sqlStatement, connection);
            command.Parameters.AddWithValue("@t", s);
            connection.Open();
            using (MySqlDataReader rdr = command.ExecuteReader())
            {
                while (rdr.Read())
                {
                    result = rdr["name"].ToString();
                }

            }
            connection.Close();
            command.Dispose();
            return result;
        }

        /*
         *  Function    : UpdateDatabaseQuantity
         *  Description : This function updates the available stock based on the ordered amount
         *      
         *
         *  Parameters  : string id, int quantity, int status
         *      
         *  Returns     : void
         *      
         */
        public void UpdateDatabaseQuantity(string id, int quantity, int status, string branchName)
        {
            string sqlStatement = "";
            //if (status == 0)
            //{
            //    sqlStatement = @" update product set stock = stock - @q
            //                         where SKU = @id;";
            //}
            //else
            //{
            //    sqlStatement = @" update product set stock = stock + @q
            //                         where SKU = @id;";
            //}

            if (status == 0)
            {
                sqlStatement = @"update inventoryinfo set stock = stock - @q where branchname = @br and sku = @id;";
            }
            else
            {
                sqlStatement = @" update inventoryinfo set stock = stock + @q where branchname = @br and sku = @id;";
            }


            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(sqlStatement, connection);

            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@br", branchName);
            command.Parameters.AddWithValue("@q", quantity);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            command.Dispose();
        }

        /*
         *  Function    : AddOrder
         *  Description : This function creates a new order item
         *      
         *
         *  Parameters  : string customerID, string branchID
         *      
         *  Returns     : int
         *      
         */
        public int AddOrder(string customerID, string branchID)
        {
            int result;
            DateTime tmp = DateTime.Today;
            string orderDate = tmp.ToString("D");

            string sqlStatement = @" insert into `Order` (orderDate, CustomerID , branchID)
                                     values (@oD, @cID, @bID);";

            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(sqlStatement, connection);
            command.Parameters.AddWithValue("@oD", orderDate);
            command.Parameters.AddWithValue("@cID", customerID);
            command.Parameters.AddWithValue("@bID", branchID);

            connection.Open();
            result = command.ExecuteNonQuery();
            connection.Close();
            command.Dispose();
            
            return result;
        }

        /*
         *  Function    : AddOrderWithDate
         *  Description : This function creates a new order item with a certain date
         *      
         *
         *  Parameters  : string customerID, string branchID, string orderDate
         *      
         *  Returns     : int
         *      
         */
        public int AddOrderWithDate(string customerID, string branchID, string orderDate)
        {
            int result;
            

            string sqlStatement = @" insert into `Order` (orderDate, CustomerID , branchID)
                                     values (@oD, @cID, @bID);";

            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(sqlStatement, connection);
            command.Parameters.AddWithValue("@oD", orderDate);
            
            command.Parameters.AddWithValue("@cID", customerID);
            command.Parameters.AddWithValue("@bID", branchID);

            connection.Open();
            result = command.ExecuteNonQuery();
            connection.Close();
            command.Dispose();

            return result;
        }

        /*
         *  Function    : AddOrderLine
         *  Description : This function creates a new orderLine item
         *      
         *
         *  Parameters  : string orderID, string SKU, string quantity
         *      
         *  Returns     : int
         *      
         */
        public int AddOrderLine(string orderID, string SKU, string quantity)
        {
            int result;
            string status = "PAID";
            string sqlStatement = @" insert into OrderLine (orderID, SKU, quantity, `status`)
                                     values (@oID, @sku, @qu, @s);";

            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(sqlStatement, connection);
            command.Parameters.AddWithValue("@oID", orderID);
            command.Parameters.AddWithValue("@sku", SKU);
            command.Parameters.AddWithValue("@qu", quantity);
            command.Parameters.AddWithValue("@s", status);


            connection.Open();
            result = command.ExecuteNonQuery();
            connection.Close();
            command.Dispose();
            return result;
        }

        /*
         *  Function    : GetCustomerID
         *  Description : This function returns customers ID
         *      
         *
         *  Parameters  : string customerName
         *      
         *  Returns     : string
         *      
         */
        public string GetCustomerID(string customerName)
        {
            string result = "";
            int space;
            string firstName="", lastName="";
            space = customerName.IndexOf(" ");
            firstName = customerName.Substring(0, space);
            lastName = customerName.Substring(space + 1);

            string sqlStatement = @"Select customerID from customer where first_Name = @fN and  last_name = @lN;";

            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(sqlStatement, connection);
            command.Parameters.AddWithValue("@fN", firstName);
            command.Parameters.AddWithValue("@lN", lastName);
           

            connection.Open();
            using (MySqlDataReader rdr = command.ExecuteReader())
            {
                while (rdr.Read())
                {
                    result = rdr["customerID"].ToString();
                }

            }
            connection.Close();
            command.Dispose();

            return result;
        }

        /*
         *  Function    : GetBranchID
         *  Description : This function returns branch ID
         *      
         *
         *  Parameters  : string branchName
         *      
         *  Returns     : string
         *      
         */
        public string GetBranchID(string branchName)
        {
            string result = "";
            string sqlStatement = @"Select branchID from branch where branchName = @bN;";

            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(sqlStatement, connection);
            command.Parameters.AddWithValue("@bN", branchName);

            connection.Open();
            using (MySqlDataReader rdr = command.ExecuteReader())
            {
                while (rdr.Read())
                {
                    result = rdr["branchID"].ToString();
                }

            }
            connection.Close();
            command.Dispose();

            return result;
            
        }

        /*
         *  Function    : GetProductID
         *  Description : This function returns product ID
         *      
         *
         *  Parameters  : string productName
         *      
         *  Returns     : string
         *      
         */
        public string GetProductID(string productName)
        {
            string result = "";
            string sqlStatement = @"Select sku from product where Name = @pN;";

            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(sqlStatement, connection);
            command.Parameters.AddWithValue("@pN", productName);

            connection.Open();
            using (MySqlDataReader rdr = command.ExecuteReader())
            {
                while (rdr.Read())
                {
                    result = rdr["sku"].ToString();
                }

            }
            connection.Close();
            command.Dispose();

            return result;

        }

        /*
         *  Function    : GetOrderID
         *  Description : This function returns the last inserted order ID
         *      
         *
         *  Parameters  : void
         *      
         *  Returns     : string
         *      
         */
        public string GetOrderID()
        {
            string result = "";
            string sqlStatement = @"SELECT LAST_INSERT_ID();";

            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(sqlStatement, connection);

            connection.Open();
            using (MySqlDataReader rdr = command.ExecuteReader())
            {
                while (rdr.Read())
                {
                    result = rdr["LAST_INSERT_ID()"].ToString();
                }

            }
            connection.Close();
            command.Dispose();

            return result;

        }

        /*
         *  Function    : RemoveFromCart
         *  Description : This function removes and order from the cart
         *      
         *
         *  Parameters  : string sessionID
         *      
         *  Returns     : int
         *      
         */
        public int RemoveFromCart(string sessionID)
        {
            int result;

            string sqlStatement = @" delete from cart where sessionID = @sID;";

            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(sqlStatement, connection);
            command.Parameters.AddWithValue("@sID", sessionID);

            connection.Open();
            result = command.ExecuteNonQuery();
            connection.Close();
            command.Dispose();
            return result;
        }


        /*
         *  Function    : RefundOrderLine
         *  Description : This function refunds an orderLine
         *      
         *
         *  Parameters  : int orderID, int quantity, string sku
         *      
         *  Returns     : int
         *      
         */
        public int RefundOrderLine(int orderID, int quantity, string sku)
        {
            int result;
            string sqlStatement = @" update orderline set quantity = @q, `status` = 'RFND'
                                        where orderID = @oID and sku = @sku;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(sqlStatement, connection);
            command.Parameters.AddWithValue("@q", quantity);
            command.Parameters.AddWithValue("@oID", orderID);
            command.Parameters.AddWithValue("@sku", sku);
            connection.Open();
            result = command.ExecuteNonQuery();
            connection.Close();
            command.Dispose();
            return result;
        }

        /*
         *  Function    : ReturnQuantity
         *  Description : This function returns quantity
         *      
         *
         *  Parameters  : int orderID
         *      
         *  Returns     : string
         *      
         */
        public string ReturnQuantity(int orderID)
        {
            string result = "";
            string sqlStatement = @" select quantity from orderline where orderID = @oID;";

            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(sqlStatement, connection);
            command.Parameters.AddWithValue("@oID", orderID);

            connection.Open();
            using (MySqlDataReader rdr = command.ExecuteReader())
            {
                while (rdr.Read())
                {
                    result = rdr["quantity"].ToString();
                }

            }
            connection.Close();
            command.Dispose();
            return result;
        }

        /*
         *  Function    : ReturnSKUFromOL
         *  Description : This function returns sku from the orderLine
         *      
         *
         *  Parameters  : int orderID
         *      
         *  Returns     : string
         *      
         */
        public string ReturnSKUFromOL(int orderID)
        {
            string result = "";
            string sqlStatement = @" select sku from orderline where orderID = @oID;";

            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(sqlStatement, connection);
            command.Parameters.AddWithValue("@oID", orderID);

            connection.Open();
            using (MySqlDataReader rdr = command.ExecuteReader())
            {
                while (rdr.Read())
                {
                    result = rdr["sku"].ToString();
                }

            }
            connection.Close();
            command.Dispose();
            return result;
        }
        /*
         *  Function    : GetOrderDate
         *  Description : This function returns orderDate
         *      
         *
         *  Parameters  : string orderID
         *      
         *  Returns     : string
         *      
         */
        public string GetOrderDate(string orderID)
        {
            string result = "";
            string sqlStatement = @" select orderDate from `order` where orderID = @oID;";

            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(sqlStatement, connection);
            command.Parameters.AddWithValue("@oID", orderID);

            connection.Open();
            using (MySqlDataReader rdr = command.ExecuteReader())
            {
                while (rdr.Read())
                {
                    result = rdr["orderDate"].ToString();
                }

            }
            connection.Close();
            command.Dispose();
            return result;
        }

        /*
         *  Function    : GetUnitPrice
         *  Description : This function returns unitPrice
         *      
         *
         *  Parameters  : string sku
         *      
         *  Returns     : string
         *      
         */
        public string GetUnitPrice(string sku)
        {
            string result = "";
            string sqlStatement = @" select wprice from product where name = @sku;";

            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(sqlStatement, connection);
            command.Parameters.AddWithValue("@sku", sku);

            connection.Open();
            using (MySqlDataReader rdr = command.ExecuteReader())
            {
                while (rdr.Read())
                {
                    result = rdr["wprice"].ToString();
                }

            }
            connection.Close();
            command.Dispose();
            return result;
        }

        public int AddNewProduct(string name, string price)
        {
            int result;
            string sqlStatement = @" insert into product(`name`, wprice)
                                        values(@name, @price);";
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(sqlStatement, connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@price", price);
            
            connection.Open();
            result = command.ExecuteNonQuery();
            connection.Close();
            command.Dispose();
            return result;
        }

        public int OrderInventory(string branchID, string sku, string amount)
        {
            int result;
            string sqlStatement = @" insert into inventory(branchID, sku, stock)
                                        values(@bID, @sku, @stock);";
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(sqlStatement, connection);
            command.Parameters.AddWithValue("@bID", branchID);
            command.Parameters.AddWithValue("@sku", sku);
            command.Parameters.AddWithValue("@stock", amount);

            connection.Open();
            result = command.ExecuteNonQuery();
            connection.Close();
            command.Dispose();
            return result;
        }

        public string GetDiscontinuationStatus(string sku)
        {
            string result = "";
            string sqlStatement = @" select discontinued from product where sku = @sku;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(sqlStatement, connection);
  
            command.Parameters.AddWithValue("@sku", sku);
           
            connection.Open();

            using (MySqlDataReader rdr = command.ExecuteReader())
            {
                while (rdr.Read())
                {
                    result = rdr["discontinued"].ToString();
                }

            }

            connection.Close();
            command.Dispose();
            return result;
        }

        public int ToggleDiscontinue(string sku)
        {
            int result;
            string status = GetDiscontinuationStatus(sku);
            if(status == "1")
            {
                status = "-1";
            }
            else if(status == "-1")
            {
                status = "1";
            }
            string sqlStatement = @" update product set discontinued = @val where sku = @sku;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(sqlStatement, connection);  
            command.Parameters.AddWithValue("@sku", sku);
            command.Parameters.AddWithValue("@val", status);

            connection.Open();
            result = command.ExecuteNonQuery();
            connection.Close();
            command.Dispose();
            return result;
        }

        public DataTable GetProductInfo(string productName)
        {

            //string sqlStatement = @"select * from product";
            string sqlStatement = @"select * from inventoryinfo where `name` = @name;";
            DataTable dt = new DataTable();

            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(sqlStatement, connection);

            command.Parameters.AddWithValue("@name", productName);
            connection.Open();

            //using (MySqlDataAdapter da = new MySqlDataAdapter(sqlStatement, connection))
            using (MySqlDataAdapter da = new MySqlDataAdapter(command))
                da.Fill(dt);

            connection.Close();
            return dt;
        }
    }
}
