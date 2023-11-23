using POLAR.ModelAggregator.Alarm;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;

namespace PORLA.HMI.Module.Model.AlarmHandle
{
    public class AlarmHandler : BindableBase, IAlarmHandler
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        IEventAggregator _eventAggregator;

        private object LockAlarm = new object();
        //private object LockAlarmList = new object();

        private ObservableCollection<AlarmItems> CurrentAlarms { get; set; }
        private ObservableCollection<AlarmItems> ListHistoryAlarm { get; set; }

        public int AlarmCount { get; set; }
        public AlarmHandler(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            CurrentAlarms = new ObservableCollection<AlarmItems>();
            ListHistoryAlarm = new ObservableCollection<AlarmItems>();
        }


        public ObservableCollection<AlarmItems> GetAlarmList()
        {
            ObservableCollection<AlarmItems> alarmList = new ObservableCollection<AlarmItems>();
            try
            {
                lock (LockAlarm)
                {
                    alarmList.AddRange(CurrentAlarms);
                }
                return alarmList;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return alarmList;
        }
    }
}
