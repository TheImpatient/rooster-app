using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using MySql.Data.MySqlClient;


namespace RoosterApp.Models
{
    public class Repository
    {
        public static StatusImg GetStatusImgData()
        {
            const string connectionString = @"Server=195.8.208.128;Port=3351;Database=rooster;Uid=app;Pwd=&O6zWYLUEIg9lNhxXdzy;";
            MySqlConnection connection = null;
            MySqlDataReader reader = null;
            var img = new StatusImg();
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string query = string.Format("SELECT * FROM heartbeat;");
                var sqlCommand = new MySqlCommand(query, connection);
                reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    img.ID = (int) reader.GetValue(0);
                    img.Timestamp = (DateTime) reader.GetValue(1);
                    img.Running = ((int) reader.GetValue(2)) == 1 ? true : false;
                }
            }
            catch (MySqlException err)
            {
                Console.WriteLine("Error: " + err.ToString());
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (connection != null)
                {
                    connection.Close();
                }
            }
            return img;
        }

        public static List<StatusLog> GetStatusLogData()
        {
            const string connectionString = @"Server=195.8.208.128;Port=3351;Database=rooster;Uid=app;Pwd=&O6zWYLUEIg9lNhxXdzy;";
            MySqlConnection connection = null;
            MySqlDataReader reader = null;
            var list = new List<StatusLog>();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string query = string.Format("SELECT timestamp , details FROM schedule_log WHERE timestamp > '{0}';",DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd"));
                var sqlCommand = new MySqlCommand(query, connection);
                reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    var item = jss.Deserialize<StatusLog>((string) reader.GetValue(1));
                    item.Timestamp = (DateTime) reader.GetValue(0);

                    list.Add(item);
                }
            }
            catch (MySqlException err)
            {
                Console.WriteLine("Error: " + err.ToString());
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (connection != null)
                {
                    connection.Close();
                }
            }
            return list;
        }
    }
}