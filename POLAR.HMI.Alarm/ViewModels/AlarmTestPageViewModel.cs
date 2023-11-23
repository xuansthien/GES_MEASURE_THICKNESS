using log4net.Repository.Hierarchy;
using POLAR.EventAggregator;
using POLAR.HMI.Alarm.Models.Define;
using POLAR.ModelAggregator.Alarm;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POLAR.HMI.Alarm.ViewModels
{
    public class AlarmTestPageViewModel : BindableBase
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        IEventAggregator _eventAggregator;
        private object LockAlarm = new object();
        private object LockAlarmList = new object();
        private AlarmDefinitions AlarmInstance;
        private ObservableCollection<AlarmItems> _currentAlarmList;
        public ObservableCollection<AlarmItems> CurrentAlarmList
        {
            get { return _currentAlarmList; }
            set { SetProperty(ref _currentAlarmList, value); }
        }
        public AlarmTestPageViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            AlarmInstance = new AlarmDefinitions();

            _eventAggregator.GetEvent<AlarmRaiseEvent>().Subscribe(AlarmCreateMethod, ThreadOption.UIThread);
            _eventAggregator.GetEvent<AlarmResetEvent>().Subscribe(AlarmClearMethod, ThreadOption.UIThread);

            _currentAlarmList = new ObservableCollection<AlarmItems>();
            //_historyAlarmList = new ObservableCollection<AlarmItems>();
            //_dummyTestReportList = new ObservableCollection<TestReportItem>();

            //TestCmd = new DelegateCommand(() => exTestmethod());
            //ClearCmd = new DelegateCommand(() => exClearMethod());
            //RemoveLatesAlarmCmd = new DelegateCommand(() => exRemoveLatestAlarmAndMoveToHistory());
            //LoadAlarmHistoryCmd = new DelegateCommand(() => exLoadAlarmHistory());
        }
        private void AlarmClearMethod(int obj)
        {
            //await Task.Run(() =>
            //{
            //    RemoveAlarm((short)obj);
            //});
            RemoveAlarm((short)obj);

        }

        private void RemoveAlarm(short alarmId)
        {
            try
            {
                AlarmDefineItems defineItems = null;
                AlarmItems alarmItem = null;
                bool ContainAlarm = false;
                if (AlarmInstance.GetDefinition(alarmId, out defineItems))
                {
                    ContainAlarm = ContainsAlarm(alarmId, out alarmItem);

                    if (ContainAlarm)
                    {
                        if (alarmItem.ActionOptions == AlarmActionOptions.AutoClear)
                        {
                            alarmItem.ClearAlarm();
                            ExRemoveAlarm(alarmItem);
                            //_DataHandle.UpdateAlarmClear(alarmItem);
                            logger.Info($"Alarm Removed With Alarm ID: {alarmId}, Description: {defineItems.Description}, Alarm Module: {defineItems.SourceModule}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private void ExRemoveAlarm(AlarmItems alarmItem)
        {
            try
            {
                CurrentAlarmList.Remove(alarmItem);
                //HistoryAlarmList.Insert(0, alarmItem);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private bool ContainsAlarm(short alarmId, out AlarmItems alarmItem)
        {
            alarmItem = null;
            try
            {
                lock (LockAlarm)
                {
                    foreach (AlarmItems currentAlarm in CurrentAlarmList)
                    {
                        if (currentAlarm.AlarmId == alarmId)
                        {
                            alarmItem = currentAlarm;
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return false;
        }

        private void AlarmCreateMethod(int obj)
        {
            //await Task.Run(() =>
            //{
            //    GenerateAlarm((short)obj, DateTime.Now);
            //});
            GenerateAlarm((short)obj, DateTime.Now);

        }
        private void GenerateAlarm(short alarmIndex, DateTime alarmTime)
        {
            long alarmDBId = 0;
            AlarmDefineItems defineItems = null;
            try
            {
                lock (LockAlarmList)
                {
                    if (AlarmInstance.GetDefinition(alarmIndex, out defineItems))
                    {
                        AlarmItems alarmItems = new AlarmItems(alarmTime, defineItems.AlarmCategory, defineItems.SourceModule, alarmDBId, alarmIndex, defineItems.Description, defineItems.Instruction, defineItems.ActionOptions, alarmTime);
                        AlarmItems nullAlarm = CheckCurrentAlarm(alarmItems.AlarmId);
                        if (nullAlarm == null)
                        {
                            //_DataHandle.InsertAlarm(alarmItems, out alarmDBId);
                            //alarmItems.AlarmDBId = alarmDBId;
                            CreateAlarm(alarmItems);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
        private AlarmItems CheckCurrentAlarm(short alarmId)
        {
            return CurrentAlarmList.Where(s => s.AlarmId == alarmId).FirstOrDefault();
        }

        private void CreateAlarm(AlarmItems alarmItems)
        {
            try
            {
                _currentAlarmList.Add(alarmItems);
            }
            catch (Exception e)
            {
                logger.Error($"GetCurrentAlarmList with exception: {e}");
            }
        }
    }
}
