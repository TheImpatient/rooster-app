using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using MySql.Data.MySqlClient;


namespace RoosterApp.Models
{
    public class Repository
    {
        private static List<Les> _rooster; 

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

                string query = string.Format("SELECT timestamp , details FROM schedule_log WHERE timestamp > '{0}' ORDER BY  timestamp DESC;", DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd"));
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

        public static IEnumerable<Les> GetRoosterData(string searchQuery="")
        {
            const string connectionString = @"Server=195.8.208.128;Port=3351;Database=rooster;Uid=app;Pwd=&O6zWYLUEIg9lNhxXdzy;";
            MySqlConnection connection = null;
            MySqlDataReader reader = null;
            var list = new List<Les>();
            var jss = new JavaScriptSerializer();

            string query = string.Format("SELECT * FROM les WHERE start_tijd > '{0}';", DateTime.Today.ToString("yyyy-MM-dd HH:mm:ss"));
            if (!String.IsNullOrEmpty(searchQuery))
            {
                query = string.Format("SELECT * FROM les WHERE start_tijd > '{0}' AND docent LIKE '%{1}%' OR vak LIKE '%{1}%' OR vak_code LIKE '%{1}%' OR lokaal LIKE '%{1}%' OR klas LIKE '%{1}%';", DateTime.Today.ToString("yyyy-MM-dd HH:mm:ss"), searchQuery);
            }

            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
               
                var sqlCommand = new MySqlCommand(query, connection);
                reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    int vakid;
                    list.Add(new Les()
                    {
                        Docent = String.IsNullOrEmpty((string)reader.GetValue(1)) ? "" : (string)reader.GetValue(1),
                        Klas = String.IsNullOrEmpty((string)reader.GetValue(8)) ? "" : (string)reader.GetValue(8),
                        Lengte = (TimeSpan)reader.GetValue(6),
                        StartTijd = (DateTime)reader.GetValue(5),
                        Lokaal = String.IsNullOrEmpty((string)reader.GetValue(7)) ? "" : (string)reader.GetValue(7),
                        Vak = String.IsNullOrEmpty((string)reader.GetValue(2)) ? "" : (string)reader.GetValue(2),
                        VakCode = String.IsNullOrEmpty((string)reader.GetValue(3)) ? "" : (string)reader.GetValue(3),
                        VakId = int.TryParse(reader.GetValue(4).ToString(), out vakid) ? 0 : vakid
                    });
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

            _rooster = list.ToList();
            return list;
        }

        public static List<Les> GetRooster()
        {
            if (_rooster == null)
            {
                _rooster = new List<Les>();
            }

            return _rooster;
        }
    }
}