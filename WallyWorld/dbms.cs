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
    }
}
