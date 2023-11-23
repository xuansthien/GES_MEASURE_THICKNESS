using POLAR.DataService.DataHandle;
using POLAR.DataService.Helper;
using POLAR.DataService.MySql;
using POLAR.ModelAggregator.Alarm;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PORLA.HMI.Module.DataService.DataHandle
{
    public partial class DataHandle : DataHandleBase, IDataHandler
    {
        public bool InsertAlarm(AlarmItems alarmItem, out long alarmDBId)
        {
            try
            {
                DataHandler_Alarm alarmHistory = new DataHandler_Alarm()
                {
                    AlarmId = alarmItem.AlarmId,
                    SourceModule = alarmItem.SourceModule,
                    Category = alarmItem.AlarmCategory,
                    Description = alarmItem.Description,
                    AlarmTime = alarmItem.AlarmCreateTime,
                };

                return this.Insert(alarmHistory, out alarmDBId);
            }
            catch (Exception exp)
            {
                logger.Error(exp);

            }

            alarmDBId = 0;
            return false;
        }

        public bool UpdateAlarmClear(AlarmItems alarmItem)
        {
            try
            {
                DataHandler_Alarm alarmHistory = new DataHandler_Alarm()
                {
                    DB_Alarm_Id = alarmItem.AlarmDBId,
                    AlarmId = alarmItem.AlarmId,
                    SourceModule = alarmItem.SourceModule,
                    Category = alarmItem.AlarmCategory,
                    Description = alarmItem.Description,
                    AlarmTime = alarmItem.AlarmCreateTime,
                    AlarmClearedTime = alarmItem.AlarmClearTime
                };

                //MysqlUpdateQueryBuilder queryBuilder = new MysqlUpdateQueryBuilder(DataHandler_Alarm.TableName);
                //queryBuilder.AddField(DataHandler_Alarm.db_Alarm_Cleared_Time, alarmItem.AlarmClearTime);
                //queryBuilder.AddCondition("{0}={1}", DataHandler_Alarm.db_ID, alarmItem.AlarmDBId);
                return this.Update(alarmHistory.GetUpdateQuery());
            }
            catch (Exception exp)
            {
                logger.Error(exp);

            }
            return false;
        }
    }
}
