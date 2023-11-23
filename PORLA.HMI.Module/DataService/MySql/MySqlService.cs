using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POLAR.DataService.MySql
{
    public class MySqlService
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string Username { get; private set; }
        public static string Password { get; private set; }
        public static string ServerName { get; private set; }
        public static string DatabaseName { get; private set; }
        public static string ConnectionString { get; private set; }

        public static void Initialize(string servername, string username, string password, string databasename)
        {
            try
            {
                Username = username;
                Password = password;
                ServerName = servername;
                DatabaseName = databasename;
                ConnectionString = String.Format("SERVER={0}; UID={1}; PASSWORD={2}; DATABASE={3}", servername, username, password, databasename);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public static bool CreateDatabase()
        {
            string connectionString = String.Format("SERVER={0}; UID={1}; PASSWORD={2};", ServerName, Username, Password);
            bool result = false;
            try
            {
                string createquery = string.Format("CREATE SCHEMA IF NOT EXISTS `{0}` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci", DatabaseName);
                MySqlHelper.ExecuteNonQuery(connectionString, createquery);
                result = true;
            }
            catch (MySqlException ex)
            {
                logger.Error(ex);
            }
            return result;
        }

        public static bool CreateDatabase(string servername, string rootusername, string rootpassword, string databasename, string createquery)
        {
            string connectionString = String.Format("SERVER={0}; UID={1}; PASSWORD={2};", servername, rootusername, rootpassword);
            bool result = false;
            try
            {
                MySqlHelper.ExecuteNonQuery(connectionString, createquery);
                result = true;
            }
            catch (MySqlException ex)
            {
                logger.Error(ex);
            }
            return result;
        }

        public static bool CreateDatabase(string servername, string rootusername, string rootpassword, string databasename)
        {
            string connectionString = String.Format("SERVER={0}; UID={1}; PASSWORD={2};", servername, rootusername, rootpassword);
            bool result = false;
            try
            {
                string createquery = string.Format("CREATE SCHEMA IF NOT EXISTS `{0}` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci", databasename);
                MySqlHelper.ExecuteNonQuery(connectionString, createquery);
                result = true;
            }
            catch (MySqlException ex)
            {
                logger.Error(ex);
            }
            return result;
        }

        public static bool IsUsernamePresent(string servername, string rootusername, string rootpassword, string username)
        {
            bool result = false;
            string connectionString = String.Format("SERVER={0}; UID={1}; PASSWORD={2};", servername, rootusername, rootpassword);

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(String.Format("SELECT EXISTS(SELECT 1 FROM mysql.user WHERE user = '{0}')", username), connection);

                    if (Convert.ToInt32(cmd.ExecuteScalar()) == 1)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                    connection.Close();
                }
            }
            catch (MySqlException)
            {
                result = false;
            }
            return result;
        }

        public static MYSQLCONResult CheckConnection()
        {
            string connectionString = String.Format("SERVER={0}; UID={1}; PASSWORD={2}; DATABASE={3}", ServerName, Username, Password, DatabaseName);
            MYSQLCONResult result = MYSQLCONResult.CannotConnectToServer;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    connection.Close();
                    result = MYSQLCONResult.ConnectionSuccessful;
                }
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        result = MYSQLCONResult.CannotConnectToServer;
                        break;
                    case 1042:
                        result = MYSQLCONResult.ServerNotPresent;
                        break;
                    case 1045:
                        result = MYSQLCONResult.InvalidCredentials;
                        break;
                    default:
                        break;
                }
            }
            return result;
        }

        public static MYSQLCONResult CheckConnection(string servername)
        {
            string connectionString = String.Format("SERVER={0};", servername);
            MYSQLCONResult result = MYSQLCONResult.CannotConnectToServer;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    connection.Close();
                    result = MYSQLCONResult.ConnectionSuccessful;
                }
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        result = MYSQLCONResult.CannotConnectToServer;
                        break;
                    case 1042:
                        result = MYSQLCONResult.ServerNotPresent;
                        break;
                    case 1045:
                        result = MYSQLCONResult.InvalidCredentials;
                        break;
                    default:
                        break;
                }
            }
            return result;
        }

        public static MYSQLCONResult CheckConnection(string servername, string username, string password)
        {
            string connectionString = String.Format("SERVER={0}; UID={1}; PASSWORD={2};", servername, username, password);
            MYSQLCONResult result = MYSQLCONResult.CannotConnectToServer;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    connection.Close();
                    result = MYSQLCONResult.ConnectionSuccessful;
                }
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        result = MYSQLCONResult.CannotConnectToServer;
                        break;
                    case 1042:
                        result = MYSQLCONResult.ServerNotPresent;
                        break;
                    case 1045:
                        result = MYSQLCONResult.InvalidCredentials;
                        break;
                    default:
                        break;
                }
            }
            return result;
        }

        public static MYSQLCONResult CheckConnection(string servername, string username, string password, string databasename)
        {
            string connectionString = String.Format("SERVER={0}; UID={1}; PASSWORD={2}; DATABASE={3}", servername, username, password, databasename);
            MYSQLCONResult result = MYSQLCONResult.CannotConnectToServer;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    connection.Close();
                    result = MYSQLCONResult.ConnectionSuccessful;
                }
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        result = MYSQLCONResult.CannotConnectToServer;
                        break;
                    case 1042:
                        result = MYSQLCONResult.ServerNotPresent;
                        break;
                    case 1045:
                        result = MYSQLCONResult.InvalidCredentials;
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
    }
    public enum MYSQLCONResult { CannotConnectToServer, ServerNotPresent, InvalidCredentials, ConnectionSuccessful }

}
