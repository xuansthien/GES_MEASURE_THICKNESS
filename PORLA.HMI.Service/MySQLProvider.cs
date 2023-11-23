using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace SuproCL
{
    public static class MySQLProvider
    {
        private static readonly string connStr = "data source=localhost; initial catalog=dbpolar; persist security info=True; user id=root ; password=TSX@1991de;";
        public static DataTable ExecuteQuery(string query, object[] parameters = null)
        {
            try
            {
                DataTable dataTable = new DataTable();

                using (MySqlConnection connection = new MySqlConnection(connStr))
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    if (parameters != null)
                    {
                        string[] listPara = query.Split(' ');
                        int i = 0;
                        foreach (string item in listPara)
                        {
                            if (item.Contains('@'))
                            {
                                command.Parameters.AddWithValue(item, parameters[i]);
                                i++;
                            }
                        }
                    }
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
                return dataTable;
            }
            catch (Exception ex)
            {

                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }
        public static int ExecuteNonQuery(string query, object[] parameters = null)
        {
            try
            {
                int affectedRows = 0;

                using (MySqlConnection connection = new MySqlConnection(connStr))
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    if (parameters != null)
                    {
                        string[] listPara = query.Split(' ');
                        int i = 0;
                        foreach (string item in listPara)
                        {
                            if (item.Contains('@'))
                            {
                                command.Parameters.AddWithValue(item, parameters[i]);
                                i++;
                            }
                        }
                    }
                    affectedRows = command.ExecuteNonQuery();

                   
                }
                return affectedRows;
            }
            catch (Exception ex)
            {
               
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }
        public static object ExecuteScalar(string query, object[] parameters = null)
        {
            try
            {
                object result = null;

                using (MySqlConnection connection = new MySqlConnection(connStr))
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    if (parameters != null)
                    {
                        string[] listPara = query.Split(' ');
                        int i = 0;
                        foreach (string item in listPara)
                        {
                            if (item.Contains('@'))
                            {
                                command.Parameters.AddWithValue(item, parameters[i]);
                                i++;
                            }
                        }
                    }
                    result = command.ExecuteScalar();
                }
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }
        public static int InsertDataIntoTable(string tableName, params object[] parameters)
        {
            try
            {
                int affectedRows = 0;
                using (MySqlConnection connection = new MySqlConnection(connStr))
                {
                    connection.Open();
                    string parameterNames = string.Join(", ", parameters.Select((p, i) => $"@Param{i}"));
                    string query = $"INSERT INTO {tableName} VALUES ({parameterNames})";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            command.Parameters.AddWithValue($"@Param{i}", parameters[i]);
                        }

                        affectedRows = command.ExecuteNonQuery();
                    }
                }
                return affectedRows;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }
        public static int UpdateDataInTable(string tableName, string keyColumnName, object keyValue, params string[] columnNamesAndValues)
        {
            try
            {
                int affectedRows = 0;
                using (MySqlConnection connection = new MySqlConnection(connStr))
                {
                    connection.Open();

                    string setClause = string.Join(", ", Enumerable.Range(0, columnNamesAndValues.Length / 2)
                        .Select(i => $"{columnNamesAndValues[i * 2]} = @{columnNamesAndValues[i * 2]}"));

                    string query = $"UPDATE {tableName} SET {setClause} WHERE {keyColumnName} = @KeyValue";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        for (int i = 0; i < columnNamesAndValues.Length; i += 2)
                        {
                            command.Parameters.AddWithValue($"@{columnNamesAndValues[i]}", columnNamesAndValues[i + 1]);
                        }
                        command.Parameters.AddWithValue("@KeyValue", keyValue);

                        affectedRows = command.ExecuteNonQuery();
                    }
                }
                return affectedRows;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }
        public static int DeleteDataFromTable(string tableName, string keyColumnName, object keyValue)
        {
            try
            {
                int affectedRows = 0;
                using (MySqlConnection connection = new MySqlConnection(connStr))
                {
                    connection.Open();
                    string query = $"DELETE FROM {tableName} WHERE {keyColumnName} = @KeyValue";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@KeyValue", keyValue);

                        affectedRows = command.ExecuteNonQuery();
                    }
                }
                return affectedRows;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }
        public static DataTable SelectDataFromTable(string tableName, string keyColumnName = null, object keyValue = null)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connStr))
                {
                    connection.Open();

                    string query;
                    if (string.IsNullOrEmpty(keyColumnName) || keyValue == null)
                    {
                        query = $"SELECT * FROM {tableName}";
                    }
                    else
                    {
                        query = $"SELECT * FROM {tableName} WHERE {keyColumnName} = @KeyValue";
                    }

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        if (!string.IsNullOrEmpty(keyColumnName) && keyValue != null)
                        {
                            command.Parameters.AddWithValue("@KeyValue", keyValue);
                        }
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            return dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }
        public static DataTable LoadTestReport(string tableName, string dateColumnName, DateTime startTime, DateTime endTime)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connStr))
                {
                    connection.Open();

                    string query = $"SELECT * FROM {tableName} WHERE {dateColumnName} BETWEEN @StartTime AND @EndTime";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StartTime", startTime);
                        command.Parameters.AddWithValue("@EndTime", endTime);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            return dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }

    }
}
