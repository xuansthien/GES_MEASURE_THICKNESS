using MySql.Data.MySqlClient;
using POLAR.DataService.Helper;
using POLAR.DataService.MySql;
using POLAR.ModelAggregator.Alarm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PORLA.HMI.Module.DataService.DataHandle
{
    public class DataHandler_Alarm : IDbObject
    {

        public const string TableName = "dbpolar.alarmhistory";

        public const string db_ID = "id";
        public const string db_Alarm_ID = "AlarmID";
        public const string db_Category = "Category";
        public const string db_Source_Module = "Source";
        public const string db_Description = "Description";
        public const string db_Alarm_Time = "AlarmTime";
        public const string db_Alarm_Cleared_Time = "AlarmClearedTime";

        public long DB_Alarm_Id { get; set; }
        public long AlarmId { get; set; }
        public SourceModule SourceModule { get; set; }
        public AlarmCategory Category { get; set; }
        public string Description { get; set; }
        public string AlarmTime { get; set; }
        public string AlarmClearedTime { get; set; }

        public DataHandler_Alarm()
        {
            this.AlarmId = 0;
            this.Category = AlarmCategory.Error;
            this.SourceModule = SourceModule.HMI;
            this.Description = "";
            this.AlarmTime = "";
            this.AlarmClearedTime = "";
        }

        public string GetInsertQuery()
        {
            MysqlInsertQueryBuilder queryBuilder = new MysqlInsertQueryBuilder(DataHandler_Alarm.TableName);

            queryBuilder.AddField(DataHandler_Alarm.db_Alarm_ID, this.AlarmId);
            queryBuilder.AddField(DataHandler_Alarm.db_Category, (int)this.Category);
            queryBuilder.AddField(DataHandler_Alarm.db_Source_Module, (int)this.SourceModule);
            queryBuilder.AddField(DataHandler_Alarm.db_Description, this.Description);
            queryBuilder.AddField(DataHandler_Alarm.db_Alarm_Time, this.AlarmTime);
            queryBuilder.AddField(DataHandler_Alarm.db_Alarm_Cleared_Time, this.AlarmTime);

            return queryBuilder.GetInsertQuery();
        }

        public string GetUpdateQuery()
        {
            MysqlUpdateQueryBuilder queryBuilder = new MysqlUpdateQueryBuilder(DataHandler_Alarm.TableName);

            queryBuilder.AddField(DataHandler_Alarm.db_Alarm_ID, this.AlarmId);
            queryBuilder.AddField(DataHandler_Alarm.db_Category, (int)this.Category);
            queryBuilder.AddField(DataHandler_Alarm.db_Source_Module, (int)this.SourceModule);
            queryBuilder.AddField(DataHandler_Alarm.db_Description, this.Description);
            queryBuilder.AddField(DataHandler_Alarm.db_Alarm_Time, this.AlarmTime);
            queryBuilder.AddField(DataHandler_Alarm.db_Alarm_Cleared_Time, this.AlarmClearedTime);
            queryBuilder.AddCondition("{0}={1}", DataHandler_Alarm.db_ID, this.DB_Alarm_Id);

            return queryBuilder.GetUpdateQuery();
        }

        public static string GetSelectAllQuery(DateTime lowerLimit)
        {
            MysqlSelectQueryBuilder queryBuilder = new MysqlSelectQueryBuilder(DataHandler_Alarm.TableName);
            queryBuilder.AddCondition("{0}>{1}", DataHandler_Alarm.db_Alarm_Time, ConvertTime.TimeToLong(lowerLimit));

            return queryBuilder.GetQuery();
        }

        public static string GetSelectAllQuery(DateTime fromDate, DateTime toDate)
        {
            MysqlSelectQueryBuilder queryBuilder = new MysqlSelectQueryBuilder(DataHandler_Alarm.TableName);

            queryBuilder.AddCondition("{0}>{1} AND {0}<{2}", DataHandler_Alarm.db_Alarm_Time,
                ConvertTime.TimeToLong(fromDate),
                ConvertTime.TimeToLong(toDate));

            return queryBuilder.GetQuery();
        }

        public static DataHandler_Alarm Parse(MySqlDataReader dataReader)
        {
            DataHandler_Alarm obj = new DataHandler_Alarm();

            obj.DB_Alarm_Id = long.Parse(dataReader[db_ID].ToString());
            obj.AlarmId = long.Parse(dataReader[db_Alarm_ID].ToString());
            obj.Category = (AlarmCategory)int.Parse(dataReader[db_Category].ToString());
            obj.SourceModule = (SourceModule)int.Parse(dataReader[db_Source_Module].ToString());
            obj.Description = dataReader[db_Description].ToString();
            obj.AlarmTime = dataReader[db_Alarm_Time].ToString();
            obj.AlarmClearedTime = dataReader[db_Alarm_Cleared_Time].ToString();
            return obj;
        }

        public static string GetDeleteQuery(DateTime dateTime)
        {
            long dateTimeStamp = ConvertTime.TimeToLong(dateTime);
            string query = string.Format("delete from {0} where {1}<{2}", DataHandler_Alarm.TableName, 
                DataHandler_Alarm.db_Alarm_Time, dateTimeStamp);
            return query;
        }

    }
}
