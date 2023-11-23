using POLAR.ModelAggregator.Alarm;
using POLAR.ModelAggregator.TestReport;

namespace PORLA.HMI.Module.DataService.DataHandle
{
    public interface IDataHandler
    {
        bool InitializeDataHandler();
        bool InsertAlarm(AlarmItems alarmItem, out long alarmDBId);
        bool UpdateAlarmClear(AlarmItems alarmItem);
        bool InsertReportData(TestReportItems dataItem, out long dataDBId);
    }
}