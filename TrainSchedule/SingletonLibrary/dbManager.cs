using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingletonLibrary
{
    public class dbManager
    {
        private static dbManager instance;

        private MySqlConnection connection;

        private readonly string connectionString =
            "Server=localhost;Database=TrainScheduleDB;Uid=root;Pwd=dc23rf34;";

        private dbManager()
        {
        }

        public static dbManager GetInstance()
        {
            if (instance == null)
            {
                instance = new dbManager();
            }

            return instance;
        }

        public void OpenConnection()
        {
            if (connection == null)
            {
                connection = new MySqlConnection(connectionString);
            }

            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (connection != null &&
                connection.State != System.Data.ConnectionState.Closed)
            {
                connection.Close();
            }
        }

        public MySqlConnection GetConnection()
        {
            OpenConnection();
            return connection;
        }
    }
}
