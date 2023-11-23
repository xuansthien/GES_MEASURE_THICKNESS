using POLAR.DataService.Helper;
using POLAR.DataService.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PORLA.HMI.Module.DataService.DataHandle
{
    public class DataHandler_TestReport : IDbObject
    {
        public const string TableName = "dbpolar.testreport";
        public const string db_ID = "id";
        public const string db_StartTest = "Start Test";
        public const string db_FinishTest = "Finish Test";
        public const string db_RecipeName = "Recipe Id";
        public const string db_DutId = "Dut Id";
        public const string db_RecipeSensorType = "Recipe Sensor Type";
        public const string db_RecipeX0 = "Recipe X0";
        public const string db_RecipeY0 = "Recipe Y0";
        public const string db_RecipeDX = "Recipe DX";
        public const string db_RecipeDY = "Recipe DY";
        public const string db_RecipeSpeedAxisX = "Recipe speed X axis";
        public const string db_RecipeSpeedAxisY = "Recipe speed Y axis";
        public const string db_RecipeDWD = "Recipe DWD";
        public const string db_RecipeQTH = "Recipe QTH";
        public const string db_TimeDB = "TimeDB";

        public string StartTestTime { get; set; }
        public string FinishTestTime { get; set; }
        public string RecipeName { get; set; }
        public string DutId { get; set; }
        public string SensorType { get; set; }
        public string RecipeX0 { get; set; }
        public string RecipeY0 { get; set; }
        public string RecipeDX { get; set; }
        public string RecipeDY { get; set; }
        public string RecipeSpeedAxisX { get; set; }
        public string RecipeSpeedAxisY { get; set; }
        public string RecipeDWD { get; set; }
        public string RecipeQTH { get; set; }
        public DataHandler_TestReport()
        
        {
            StartTestTime = "";
            FinishTestTime = "";
            RecipeName = "";
            DutId = "";
            SensorType = "";
            RecipeX0 = "";
            RecipeY0 = "";
            RecipeDX = "";
            RecipeDY = "";
            RecipeDWD = "";
            RecipeQTH = "";
        }
        public string GetInsertQuery()
        {
            MysqlInsertQueryBuilder queryBuilder = new MysqlInsertQueryBuilder(DataHandler_TestReport.TableName);

            queryBuilder.AddField(DataHandler_TestReport.db_StartTest, this.StartTestTime);
            queryBuilder.AddField(DataHandler_TestReport.db_FinishTest, this.FinishTestTime);
            queryBuilder.AddField(DataHandler_TestReport.db_RecipeName, this.RecipeName);
            queryBuilder.AddField(DataHandler_TestReport.db_DutId, this.DutId);
            queryBuilder.AddField(DataHandler_TestReport.db_RecipeSensorType, this.SensorType);
            queryBuilder.AddField(DataHandler_TestReport.db_RecipeX0, this.RecipeX0);
            queryBuilder.AddField(DataHandler_TestReport.db_RecipeY0, this.RecipeY0);

            queryBuilder.AddField(DataHandler_TestReport.db_RecipeDX, this.RecipeDX);
            queryBuilder.AddField(DataHandler_TestReport.db_RecipeDY, this.RecipeDY);
            queryBuilder.AddField(DataHandler_TestReport.db_RecipeDWD, this.RecipeDWD);
            queryBuilder.AddField(DataHandler_TestReport.db_RecipeQTH, this.RecipeQTH);
            queryBuilder.AddField(DataHandler_TestReport.db_TimeDB, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            return queryBuilder.GetInsertQuery();
        }
        public string GetUpdateQuery()
        {
            MysqlUpdateQueryBuilder queryBuilder = new MysqlUpdateQueryBuilder(DataHandler_TestReport.TableName);

            queryBuilder.AddField(DataHandler_TestReport.db_RecipeName, this.RecipeName);
            queryBuilder.AddField(DataHandler_TestReport.db_StartTest, this.StartTestTime);
            queryBuilder.AddField(DataHandler_TestReport.db_FinishTest, this.FinishTestTime);
            queryBuilder.AddField(DataHandler_TestReport.db_RecipeSensorType, this.SensorType);
            queryBuilder.AddField(DataHandler_TestReport.db_RecipeX0, this.RecipeX0);
            queryBuilder.AddField(DataHandler_TestReport.db_RecipeY0, this.RecipeY0);

            queryBuilder.AddField(DataHandler_TestReport.db_RecipeDX, this.RecipeDX);
            queryBuilder.AddField(DataHandler_TestReport.db_RecipeDY, this.RecipeDY);
            queryBuilder.AddField(DataHandler_TestReport.db_RecipeDWD, this.RecipeDWD);
            queryBuilder.AddField(DataHandler_TestReport.db_RecipeQTH, this.RecipeQTH);

            queryBuilder.AddCondition("{0}={1}", DataHandler_TestReport.db_DutId, this.DutId);

            return queryBuilder.GetUpdateQuery();
        }


    }
}
