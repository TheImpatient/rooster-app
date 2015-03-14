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
        public static List<StatusImg> GetStatusImgData()
        {
            var list = new List<StatusImg>
            {
                new StatusImg
                {
                    ID = 1,
                    Name = "default",
                    ImageUrl = "../../Images/heartbeat2.png"
                },
                new StatusImg
                {
                    ID = 2,
                    Name = "Online",
                    ImageUrl = "../../Images/heartbeat_green2.png"
                },
                new StatusImg
                {
                    ID = 3,
                    Name = "Offline",
                    ImageUrl = "../../Images/heartbeat_red2.png"
                }
            };

            return list;
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

                string query = string.Format("SELECT details FROM schedule_log WHERE timestamp > '{0}';",DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd"));
                var sqlCommand = new MySqlCommand(query, connection);
                reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    list.Add((StatusLog)jss.Deserialize<StatusLog>((string)reader.GetValue(0)));
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