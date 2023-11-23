using POLAR.DataService.Helper;
using POLAR.DataService.MySql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using PORLA.HMI.Module.DataService.DataHandle;

namespace POLAR.DataService.DataHandle
{
    public class DataHandleBase
    {
        protected bool IsInitialized { get; private set; }

        protected MySqlConnection connection { get; private set; }

        private object connectionLock = new object();

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected bool InitializeDB(string serverName, string username, string password, string databaseName)
        {
            try
            {
                MYSQLCONResult result = MySqlService.CheckConnection(serverName, username, password);

                if (result != MYSQLCONResult.ConnectionSuccessful)
                {
                    logger.Error(DesciptionHelper.ParseEnumToString<MYSQLCONResult>(result));
                    this.IsInitialized = false;
                    return this.IsInitialized;
                }

                result = MySqlService.CheckConnection(serverName, username, password, databaseName);

                if (result != MYSQLCONResult.ConnectionSuccessful)
                {
                    logger.Error("Database " + databaseName + " not found | Error : " + DesciptionHelper.ParseEnumToString<MYSQLCONResult>(result));
                    string dbCreateQuery = this.GetDBCreateScript();

                    if (MySqlService.CreateDatabase(serverName, username, password, databaseName, dbCreateQuery))
                    {
                        logger.Info("Database " + databaseName + "created");
                        result = MySqlService.CheckConnection(serverName, username, password, databaseName);
                    }
                    else
                    {
                        logger.Info("Database " + databaseName + " creation failed");
                        this.IsInitialized = false;
                        return this.IsInitialized;
                    }
                }

                MySqlService.Initialize(serverName, username, password, databaseName);
                connection = new MySqlConnection(MySqlService.ConnectionString);

                if (result == MYSQLCONResult.ConnectionSuccessful)
                {
                    this.IsInitialized = true;
                    return this.IsInitialized;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            this.IsInitialized = false;
            return this.IsInitialized;
        }

        protected bool InitializeDB(string serverName, string username, string password, string databaseName, string createDB)
        {
            try
            {
                MYSQLCONResult result = MySqlService.CheckConnection(serverName, username, password);

                if (result != MYSQLCONResult.ConnectionSuccessful)
                {
                    logger.Error(DesciptionHelper.ParseEnumToString<MYSQLCONResult>(result));
                    this.IsInitialized = false;
                    return this.IsInitialized;
                }

                result = MySqlService.CheckConnection(serverName, username, password, databaseName);

                if (result != MYSQLCONResult.ConnectionSuccessful)
                {
                    logger.Error("Database " + databaseName + " not found | Error : " + result);
                    string dbCreateQuery = this.GetDBCreateScript(createDB);

                    if (MySqlService.CreateDatabase(serverName, username, password, databaseName, dbCreateQuery))
                    {
                        logger.Info("Database " + databaseName + "created");
                        result = MySqlService.CheckConnection(serverName, username, password, databaseName);
                    }
                    else
                    {
                        logger.Info("Database " + databaseName + " creation failed");
                        this.IsInitialized = false;
                        return this.IsInitialized;
                    }
                }

                MySqlService.Initialize(serverName, username, password, databaseName);
                connection = new MySqlConnection(MySqlService.ConnectionString);

                if (result == MYSQLCONResult.ConnectionSuccessful)
                {
                    this.IsInitialized = true;
                    return this.IsInitialized;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            this.IsInitialized = false;
            return this.IsInitialized;
        }

        private string GetDBCreateScript()
        {
            string script = "";
            try
            {
                var assembly = Assembly.LoadFrom("GES.HMI.Main.exe");
                var resourceName = "GES.HMI.Main.Resource.dbdltx.sql";

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader sr = new StreamReader(stream))
                {
                    script = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return script;
        }

        private string GetDBCreateScript(string createDB)
        {
            string script = "";

            try
            {
                using (StreamReader reader = new StreamReader(createDB))
                {
                    script = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return script;
        }

        protected bool Insert(IDbObject dataObject)
        {
            long lastInsertId = 0;
            return this.Insert(dataObject, out lastInsertId);
        }

        protected bool Insert(IDbObject dataObject, out long lastInsertId)
        {
            lastInsertId = 0;
            string query = "";

            try
            {
                if (!this.IsInitialized)
                {
                    return false;
                }

                query = dataObject.GetInsertQuery();
                int rowsEffected = 0;

                lock (this.connectionLock)
                {
                    if (this.OpenConnection())
                    {
                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        rowsEffected = (int)cmd.ExecuteNonQuery();
                        lastInsertId = cmd.LastInsertedId;
                    }
                    if (rowsEffected == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Error(exp);
                logger.Error("Query : " + query);
                return false;
            }
        }

        protected bool Update(IDbObject dataObject)
        {
            try
            {
                if (!this.IsInitialized)
                {
                    return false;
                }

                string query = dataObject.GetUpdateQuery();
                return this.Update(query);
            }
            catch (Exception exp)
            {
                logger.Error(exp);
                return false;
            }
        }

        protected bool Update(string query)
        {
            try
            {
                if (!this.IsInitialized)
                {
                    return false;
                }

                lock (this.connectionLock)
                {
                    int rowsEffected = 0;

                    if (this.OpenConnection())
                    {
                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        rowsEffected = cmd.ExecuteNonQuery();
                    }
                    if (rowsEffected == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Error(exp);
                logger.Error("Query : " + query);
                return false;
            }
        }

        protected bool UpdateMultiple(string query, out int count)
        {
            count = 0;

            try
            {
                if (!this.IsInitialized)
                {
                    return false;
                }

                lock (this.connectionLock)
                {
                    int rowsEffected = 0;

                    if (this.OpenConnection())
                    {
                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        rowsEffected = cmd.ExecuteNonQuery();
                    }

                    if (rowsEffected > 0)
                    {
                        count = rowsEffected;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Error(exp);
                logger.Error("Query : " + query);
                return false;
            }
        }

        protected delegate TDbObject ParseObjectDelegate<TDbObject>(MySqlDataReader dataReader);

        protected List<TDbObject> Select<TDbObject>(string query, ParseObjectDelegate<TDbObject> ParseFunction) where TDbObject : IDbObject
        {
            List<TDbObject> objList = new List<TDbObject>();

            if (!this.IsInitialized)
            {
                return objList;
            }

            try
            {
                lock (this.connectionLock)
                {
                    if (this.OpenConnection())
                    {
                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        MySqlDataReader dataReader = cmd.ExecuteReader();

                        try
                        {
                            while (dataReader.Read())
                            {
                                try
                                {
                                    TDbObject obj = ParseFunction(dataReader);
                                    objList.Add(obj);
                                }
                                catch (Exception e)
                                {
                                    logger.Error($"Error in parse.: {e}");
                                }
                            }
                        }
                        catch (MySqlException)
                        {
                            logger.Error("Error in select.");
                        }
                        catch (Exception exp)
                        {
                            logger.Error(exp);
                        }
                        finally
                        {
                            dataReader.Close();
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Error(exp);
                logger.Error("Query : " + query);
            }

            return objList;
        }

        protected DataTable Select(string query)
        {
            DataTable objDataTable = new DataTable();

            if (!this.IsInitialized)
            {
                return objDataTable;
            }

            try
            {
                lock (this.connectionLock)
                {
                    if (this.OpenConnection())
                    {
                        MySqlCommand cmd = new MySqlCommand(query, this.connection);
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);

                        try
                        {
                            dataAdapter.Fill(objDataTable);
                        }
                        catch (MySqlException)
                        {
                            logger.Error("Error in select.");
                        }
                        catch (Exception exp)
                        {
                            logger.Error(exp);
                        }
                        finally
                        {
                            dataAdapter.Dispose();
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Error($"Query select: {query} fail with exception: {exp}");
            }

            return objDataTable;
        }

        protected long GetLongCount(string query)
        {
            try
            {
                if (!this.IsInitialized)
                {
                    return 0;
                }

                try
                {
                    lock (this.connectionLock)
                    {
                        if (this.OpenConnection())
                        {
                            MySqlCommand cmd = new MySqlCommand(query, connection);

                            if (cmd.ExecuteScalar().GetType() != typeof(DBNull))
                            {
                                return Convert.ToInt64(cmd.ExecuteScalar());
                            }
                            else
                            {

                            }
                        }
                    }
                }
                catch (MySqlException)
                {
                    logger.Error("Error in get long count.");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return 0;
        }

        protected int GetCount(string query)
        {
            try
            {
                if (!this.IsInitialized)
                {
                    return 0;
                }

                try
                {
                    lock (this.connectionLock)
                    {
                        if (this.OpenConnection())
                        {
                            MySqlCommand cmd = new MySqlCommand(query, connection);
                            return Convert.ToInt32(cmd.ExecuteScalar());
                        }
                    }
                }
                catch (MySqlException)
                {
                    logger.Error("Error in get count.");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return 0;
        }

        protected bool OpenConnection()
        {
            try
            {
                if (!this.IsInitialized)
                {
                    return false;
                }

                try
                {
                    if (this.connection.State != System.Data.ConnectionState.Open)
                    {
                        this.connection.Open();
                        return true;
                    }
                }
                catch (MySqlException)
                {
                    logger.Error("Error in open connection.");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return false;
            }
        }

        protected bool CloseConnection()
        {
            try
            {
                if (!this.IsInitialized)
                {
                    return false;
                }

                try
                {
                    this.connection.Close();
                }
                catch (MySqlException)
                {
                    logger.Error("Error in close connection.");
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return false;
            }
        }

        public static bool GetBoolFromBit(object bit)
        {
            switch (bit.ToString())
            {
                default:
                case "0":
                    return false;
                case "1":
                    return true;
            }
        }

        public static int GetBitFromBool(bool value)
        {
            return (value) ? 1 : 0;
        }

        private static string GetEscapedString(string input)
        {
            return MySqlHelper.EscapeString(input);
        }

        protected bool Delete(string query)
        {
            try
            {
                if (!this.IsInitialized)
                {
                    return false;
                }

                lock (this.connectionLock)
                {
                    int rowsEffected = 0;

                    if (this.OpenConnection())
                    {
                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        rowsEffected = cmd.ExecuteNonQuery();
                    }
                    if (rowsEffected == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Error(exp);
                logger.Error("Query : " + query);
                return false;
            }
        }

    }
}
