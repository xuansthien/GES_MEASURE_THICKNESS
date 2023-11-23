using log4net.Repository.Hierarchy;
using POLAR.DataService.DataHandle;
using POLAR.ModelAggregator.Alarm;
using POLAR.ModelAggregator.TestReport;
using PORLA.HMI.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PORLA.HMI.Module.DataService.DataHandle
{
    public partial class DataHandle : DataHandleBase, IDataHandler
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool InitializeDataHandler()
        {
            try
            {
                this.InitializeDB("127.0.0.1", "root", "root", "dbpolar");
                if (IsInitialized)
                {
                    logger.Info("Module DataHandler Initialize Complete");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return false;
            }
        }

        public bool InsertReportData(TestReportItems dataItem, out long dataDBId)
        {
            try
            {
                DataHandler_TestReport dataReport = new DataHandler_TestReport()
                {
                    StartTestTime = dataItem.StartTest,
                    FinishTestTime = dataItem.FinishTest,
                    RecipeName = dataItem.RecipeID,
                    DutId = dataItem.DutID,
                    SensorType = dataItem.RecipeSensorType,
                    RecipeX0 = dataItem.RecipeX0,
                    RecipeY0 = dataItem.RecipeY0,
                    RecipeDX = dataItem.RecipeDX,
                    RecipeDY = dataItem.RecipeDY,
                    RecipeDWD = dataItem.RecipeDWD,
                    RecipeQTH = dataItem.RecipeQTH
                };

                return this.Insert(dataReport, out dataDBId);
            }
            catch (Exception exp)
            {
                logger.Error(exp);
            }

            dataDBId = 0;
            return false;
        }
    }
}
