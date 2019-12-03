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

        public DataTable DisplayProducts()
        {

            string sqlStatement = @"select * from product";
            DataTable dt = new DataTable();
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            using (MySqlDataAdapter da = new MySqlDataAdapter(sqlStatement, connection))
                da.Fill(dt);

            connection.Close();
            return dt;
        }

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

        public DataTable DisplayOrders()
        {

            string sqlStatement = @"select * from `order`";
            DataTable dt = new DataTable();
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            using (MySqlDataAdapter da = new MySqlDataAdapter(sqlStatement, connection))
                da.Fill(dt);

            connection.Close();
            return dt;
        }

        public DataTable DisplayInventory()
        {

            string sqlStatement = @"select name, stock from product";
            DataTable dt = new DataTable();
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            using (MySqlDataAdapter da = new MySqlDataAdapter(sqlStatement, connection))
                da.Fill(dt);

            connection.Close();
            return dt;
        }

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

        public void UpdateDatabaseQuantity(string id, int quantity, int status)
        {
            string sqlStatement = "";
            if (status == 0)
            {
                sqlStatement = @" update product set stock = stock - @q
                                     where SKU = @id;";
            }
            else
            {
                sqlStatement = @" update product set stock = stock + @q
                                     where SKU = @id;";
            }
            

            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(sqlStatement, connection);

            command.Parameters.AddWithValue("@id", id);
            
            command.Parameters.AddWithValue("@q", quantity);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            command.Dispose();
        }

        public int AddOrder(string customerID, string branchID)
        {
            int result;
            string status = "PAID";
            DateTime tmp = DateTime.Today;
            string orderDate = tmp.ToString("D");

            string sqlStatement = @" insert into `Order` (orderDate, `status`, CustomerID , branchID)
                                     values (@oD, @s, @cID, @bID);";

            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(sqlStatement, connection);
            command.Parameters.AddWithValue("@oD", orderDate);
            command.Parameters.AddWithValue("@s", status);
            command.Parameters.AddWithValue("@cID", customerID);
            command.Parameters.AddWithValue("@bID", branchID);

            connection.Open();
            result = command.ExecuteNonQuery();
            connection.Close();
            command.Dispose();
            
            return result;
        }

        public int AddOrderLine(string orderID, string SKU, string quantity)
        {
            int result;
            string sqlStatement = @" insert into OrderLine (orderID, SKU, quantity)
                                     values (@oID, @sku, @qu);";

            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(sqlStatement, connection);
            command.Parameters.AddWithValue("@oID", orderID);
            command.Parameters.AddWithValue("@sku", SKU);
            command.Parameters.AddWithValue("@qu", quantity);
            

            connection.Open();
            result = command.ExecuteNonQuery();
            connection.Close();
            command.Dispose();
            return result;
        }

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

        public int RefundOrder(int orderID)
        {
            int result;
            string sqlStatement = @" update `order` set `status` = 'RFND'
                                     where orderID = @oID;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(sqlStatement, connection);
            command.Parameters.AddWithValue("@oID", orderID);

            connection.Open();
            result = command.ExecuteNonQuery();
            connection.Close();
            command.Dispose();
            return result;

        }

        public int RefundOrderLine(int orderID)
        {
            int result;
            string sqlStatement = @" update orderline set quantity = 0
                                     where orderID = @oID;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(sqlStatement, connection);
            command.Parameters.AddWithValue("@oID", orderID);

            connection.Open();
            result = command.ExecuteNonQuery();
            connection.Close();
            command.Dispose();
            return result;
        }

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


    }
}
