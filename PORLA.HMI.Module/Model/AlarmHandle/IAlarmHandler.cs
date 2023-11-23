using POLAR.ModelAggregator.Alarm;
using System.Collections.ObjectModel;

namespace PORLA.HMI.Module.Model.AlarmHandle
{
    public interface IAlarmHandler
    {
        ObservableCollection<AlarmItems> GetAlarmList();
    }
}