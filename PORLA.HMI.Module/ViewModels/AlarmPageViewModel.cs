using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using Prism.Commands;
using Prism.Mvvm;
using PORLA.HMI.Service;
using PORLA.HMI.Module.Model;
using SuproCL;
using System.Windows;
using System.Timers;
using Prism.Events;
using PORLA.HMI.Module.Model.DataHandle;
using POLAR.ModelAggregator.Alarm;
using POLAR.EventAggregator;
using System.Threading.Tasks;
using PORLA.HMI.Module.Model.AlarmHandle;
using System.Linq;
using PORLA.HMI.Service.Enums;
using PORLA.HMI.Module.DataService.DataHandle;
using Castle.Core.Internal;

namespace PORLA.HMI.Module.ViewModels
{
    public class AlarmPageViewModel : BindableBase
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        IEventAggregator _eventAggregator;

        private IAppService _appService;
        public IAppService AppService
        {
            get { return _appService; }
            set { SetProperty(ref _appService, value); }
        }

        IDataHandler _dataHandler;

        private object LockAlarm = new object();
        private object LockAlarmList = new object();
        private AlarmDefinitions AlarmInstance;
        private ObservableCollection<Alarm> _alarmList = new ObservableCollection<Alarm>();
        public ObservableCollection<Alarm> AlarmList
        {
            get { return _alarmList; }
            set { SetProperty(ref _alarmList, value); }
        }

        private ObservableCollection<AlarmItems> _currentAlarmList;
        public ObservableCollection<AlarmItems> CurrentAlarmList
        {
            get { return _currentAlarmList; }
            set { SetProperty(ref _currentAlarmList, value); }
        }

        private ObservableCollection<AlarmItems> _historyAlarmList;
        public ObservableCollection<AlarmItems> HistoryAlarmList
        {
            get { return _historyAlarmList; }
            set { SetProperty(ref _historyAlarmList, value); }
        }

        private AlarmItems _selectedAlarmItems;
        public AlarmItems SelectedAlarmItems
        {
            get { return _selectedAlarmItems; }
            set
            {
                SetProperty(ref _selectedAlarmItems, value);

                if (_selectedAlarmItems != null)
                {
                    switch (_selectedAlarmItems.ActionOptions)
                    {
                        case AlarmActionOptions.Retry:
                            VisibleRetry = "Visible";
                            VisibleDiscard = "Collapsed";
                            VisibleResume = "Collapsed";
                            break;
                        case AlarmActionOptions.Resume:
                            VisibleRetry = "Collapsed";
                            VisibleDiscard = "Collapsed";
                            VisibleResume = "Visible";
                            break;
                        case AlarmActionOptions.Discard:
                            VisibleRetry = "Collapsed";
                            VisibleDiscard = "Visible";
                            VisibleResume = "Collapsed";
                            break;
                        default:
                            VisibleRetry = "Collapsed";
                            VisibleDiscard = "Collapsed";
                            VisibleResume = "Collapsed";
                            break;
                    }
                }
            }
        }

        private string _VisibleRetry;
        public string VisibleRetry
        {
            get { return _VisibleRetry; }
            set { SetProperty(ref _VisibleRetry, value); }
        }

        private string _VisibleResume;
        public string VisibleResume
        {
            get { return _VisibleResume; }
            set { SetProperty(ref _VisibleResume, value); }
        }

        private string _VisibleDiscard;
        public string VisibleDiscard
        {
            get { return _VisibleDiscard; }
            set { SetProperty(ref _VisibleDiscard, value); }
        }

        private string _datePickFrom;
        public string DatePickFrom
        {
            get { return _datePickFrom; }
            set
            {
                if (!string.Equals(_datePickFrom, value))
                {
                    _datePickFrom = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string _datePickTo;
        public string DatePickTo
        {
            get { return _datePickTo; }
            set
            {
                if (!string.Equals(_datePickTo, value))
                {
                    _datePickTo = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string _txtCategory;
        public string txtCategory
        {
            get { return _txtCategory; }
            set
            {

                if (!string.Equals(_txtCategory, value))
                {
                    _txtCategory = value;
                    RaisePropertyChanged();
                }
            }
        }

        private DataTable _myDataTable;
        public DataTable myDataTable
        {
            get { return _myDataTable; }
            set
            {

                if (!DataTable.Equals(_myDataTable, value))
                {
                    _myDataTable = value;
                    RaisePropertyChanged();
                }
            }
        }

        public DelegateCommand TestCmd { get; private set; }
        public DelegateCommand TestClearCmd { get; private set; }
        public DelegateCommand ResetAllAlarmCmd { get; private set; }
        public DelegateCommand ResetAlarmCmd { get; private set; }  
        public DelegateCommand LoadAlarmHistoryCmd { get;private set; }

        public readonly Timer _timerAlarm;
        private bool[] _curAlarmList = new bool[9];
        private bool[] _preAlarmList = new bool[9];
        object lockalarm = new object();

        public AlarmPageViewModel(IAppService appService, IEventAggregator eventAggregator, IDataHandler dataHandler)
        {
            _appService = appService;
            _eventAggregator = eventAggregator;
            _dataHandler = dataHandler;
            AlarmInstance = new AlarmDefinitions();

            _eventAggregator.GetEvent<AlarmRaiseEvent>().Subscribe(AlarmCreateMethod);
            _eventAggregator.GetEvent<AlarmCreateEvent>().Subscribe(AlarmShowUp, ThreadOption.UIThread);
            _eventAggregator.GetEvent<AlarmResetEvent>().Subscribe(AlarmClearMethod);
            _eventAggregator.GetEvent<AlarmClearEvent>().Subscribe(AlarmHistoryShowUp, ThreadOption.UIThread);

            _currentAlarmList = new ObservableCollection<AlarmItems>();
            _historyAlarmList = new ObservableCollection<AlarmItems>();

            TestCmd = new DelegateCommand(() => exTestmethod());
            TestClearCmd = new DelegateCommand(() => exTestClearCmd());
            ResetAlarmCmd = new DelegateCommand(() => exResetAlarmCmd());
            ResetAllAlarmCmd = new DelegateCommand(() => exResetAllAlarmCmd());
            LoadAlarmHistoryCmd = new DelegateCommand(() => exLoadAlarmHistory());

            _timerAlarm = new Timer();
            _timerAlarm.Elapsed += _timerMonitorAlarm_Elapsed;
            _timerAlarm.Interval = 150;
            _timerAlarm.Start();
        }

        private async void exResetAllAlarmCmd()
        {
            await Task.Run(() =>
            {

                ObservableCollection<AlarmItems> alarmItems = GetAlarmList();
                try
                {
                    foreach (var item in alarmItems)
                    {
                        if (!_curAlarmList[item.AlarmId])
                        {
                            RemoveAlarm(item.AlarmId);
                        }
                        //RemoveAlarm(item.AlarmId);
                    }
                }
                catch (Exception e)
                {
                    logger.Error($"ResetAllMethod alarm fail with exception : {e}");
                }
            });
        }

        private ObservableCollection<AlarmItems> GetAlarmList()
        {
            ObservableCollection<AlarmItems> alarmList = new ObservableCollection<AlarmItems>();
            try
            {
                lock (LockAlarm)
                {
                    alarmList.AddRange(CurrentAlarmList);
                }
                return alarmList;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return alarmList;
        }

        private async void exResetAlarmCmd()
        {
            await Task.Run(() =>
            {
                try
                {
                    AlarmItems alarmItems = SelectedAlarmItems;
                    if (alarmItems != null)
                    {
                        if (!_curAlarmList[alarmItems.AlarmId])
                        {
                            RemoveAlarm(alarmItems.AlarmId);
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    logger.Error($"ResetMethod alarm fail with exception : {ex}");
                }
            });
        }

        private void exTestClearCmd()
        {
            AppService.DIOModule1.IOMapping[DIODescriptions.In_EMOSignal] = false;
            //AppService.DIOModule1.IOMapping[DIODescriptions.In_DoorSignal] = true;
            //AppService.DIOModule1.IOMapping[DIODescriptions.In_MainAirSignal] = true;
            //AppService.DIOModule2.IOMapping[DIODescriptions.In_SesorDetectZStage] = true;
            //AppService.DIOModule2.IOMapping[DIODescriptions.In_SensorDetectFixture] = true;
            //AppService.DIOModule1.IOMapping[DIODescriptions.In_LeftDoorSignal] = true;
            //AppService.DIOModule1.IOMapping[DIODescriptions.In_RightDoorSignal] = true;
            //AppService.DIOModule1.IOMapping[DIODescriptions.In_SafetyCircuitSignal] = true;

            CheckDIChanged();
        }

        private void AlarmHistoryShowUp(AlarmItems alarmItems)
        {
            try
            {
                _currentAlarmList.Remove(alarmItems);
                _historyAlarmList.Insert(0, alarmItems);
            }
            catch (Exception e)
            {
                logger.Error($"AlarmCreateMethod with exception: {e}");
            }
        }

        private void AlarmShowUp(AlarmItems alarmItems)
        {
            try
            {
                _currentAlarmList.Add(alarmItems);
            }
            catch (Exception e)
            {
                logger.Error($"AlarmCreateMethod with exception: {e}");
            }
        }

        private void _timerMonitorAlarm_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (lockalarm)
            {
                try
                {
                    if (!(AppService.DIOModule1.ConnectionState == HardwareConnectionState.Online) &&
                        !(AppService.DIOModule2.ConnectionState == HardwareConnectionState.Online))
                    {
                        return;
                    }
                    CheckDIChanged();
                    if (!_curAlarmList.SequenceEqual(_preAlarmList))
                    {
                        for (int i = 0; i < _curAlarmList.Length; i++)
                        {
                            //Raise alarm 
                            if ((_curAlarmList[i] == true && _preAlarmList[i] == false) ||
                                (_curAlarmList[i] == true && _preAlarmList[i] == true))
                            {
                                _eventAggregator.GetEvent<AlarmRaiseEvent>().Publish(i);
                                AppService.AlarmPresence = true;
                                if (AppService.MachineStatus.MachineMode == MachineModeEnum.AUTO)
                                {
                                    _eventAggregator.GetEvent<StopMachineEvent>().Publish(true);
                                }
                                //logger.Info($"LoopMonitorAlarm: Finish Publish execute with raise event, alarm No: {i}");
                            }
                            //Clear alarm
                            if (_curAlarmList[i] == false && _preAlarmList[i] == true)
                            {
                                _eventAggregator.GetEvent<AlarmResetEvent>().Publish(i);
                                AppService.AlarmPresence = false;
                                if (AppService.MachineStatus.MachineMode == MachineModeEnum.AUTO)
                                {
                                    _eventAggregator.GetEvent<StopMachineEvent>().Publish(false);
                                }
                                //logger.Info($"LoopMonitorAlarm: Finish Publish execute with reset event, alarm No: {i}");
                            }
                            //Value out of range
                            //else
                            //{
                            //    //logger.Error($"LoopMonitorAlarm -> Current value of Alarm is: {CurrentAlarmList[i]} unexpected");
                            //}
                            _preAlarmList[i] = _curAlarmList[i];
                        }

                    }
                }
                catch (Exception ex)
                {
                    logger.Error($"Monitor Alarm fail with exception e: {ex}");
                }
            }
        }

        private async void AlarmClearMethod(int obj)
        {
            await Task.Run(() =>
            {
                RemoveAlarm((short)obj);
                if (_curAlarmList[6])
                {
                    AppService.MachineStatus.MachineState = MachineStateEnum.NOTREADY;
                }
            });

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
                            _dataHandler.UpdateAlarmClear(alarmItem);
                            logger.Info($"Alarm Removed With Alarm ID: {alarmId}, " +
                                $"Description: {defineItems.Description}, " +
                                $"Alarm Module: {defineItems.SourceModule}");
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
                lock(LockAlarm)
                {
                    _eventAggregator.GetEvent<AlarmClearEvent>().Publish(alarmItem);
                }
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

        private async void AlarmCreateMethod(int obj)
        {
            await Task.Run(() =>
            {
                InternalGenerateAlarm((short)obj);
                AppService.MachineStatus.MachineState = MachineStateEnum.ERROR;
            });

        }
        private void InternalGenerateAlarm(short alarm)
        {
            GenerateAlarm(alarm, DateTime.Now);
        }

        private void GenerateAlarm(short alarmIndex, DateTime alarmTime)
        {
            long alarmDBId = 0;
            AlarmDefineItems defineItems = null;
            try
            {
                lock(LockAlarmList)
                {
                    if(AlarmInstance.GetDefinition(alarmIndex, out defineItems))
                    {
                        string createTime = alarmTime.ToString("yyyy-MM-dd HH:mm:ss");
                        
                        AlarmItems alarmItems = new AlarmItems(createTime, defineItems.AlarmCategory, defineItems.SourceModule, alarmDBId, alarmIndex, defineItems.Description, defineItems.Instruction, defineItems.ActionOptions, createTime);
                        AlarmItems nullAlarm = CheckCurrentAlarm(alarmItems.AlarmId);
                        if(nullAlarm == null)
                        {
                            _dataHandler.InsertAlarm(alarmItems, out alarmDBId);
                            alarmItems.AlarmDBId = alarmDBId;
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
                //_currentAlarmList.Add(alarmItems);
                _eventAggregator.GetEvent<AlarmCreateEvent>().Publish(alarmItems);
            }
            catch (Exception e)
            {
                logger.Error($"GetCurrentAlarmList with exception: {e}");
            }
        }

        private Timer alarmCheckTimer;

        public void InitializeAlarmCheckTimer()
        {
            alarmCheckTimer = new Timer(1000); 
            alarmCheckTimer.Elapsed += AlarmCheckTimerElapsed;
            alarmCheckTimer.AutoReset = true;
            alarmCheckTimer.Enabled = true;
        }
        private void AlarmCheckTimerElapsed(object sender, ElapsedEventArgs e)
        {
            CheckListAlarmCurrent();
        }
        public void CheckListAlarmCurrent()
        {
            if (CurrentAlarmList != null && CurrentAlarmList.Count > 0)
            {
                _appService.AlarmPresence = true;
            }
            else
            {
                _appService.AlarmPresence = false;
            }
        }
        
        private void exLoadAlarmHistory()
        {
            if (!string.IsNullOrEmpty(DatePickFrom) && !string.IsNullOrEmpty(DatePickTo))
            {
                string pathFileTest = $@"Report\AlarmList\AlarmHistoryList{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv";
                try
                {
                    DataTable dt = MySQLProvider.LoadTestReport("alarmhistory", "AlarmTime", Convert.ToDateTime(DatePickFrom), Convert.ToDateTime(DatePickTo));

                    ExportDataTableToFile(dt, pathFileTest);

                    System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show($"Saved to: {pathFileTest}. Open file?",
                              "Confirmation",
                              System.Windows.MessageBoxButton.YesNo,
                              System.Windows.MessageBoxImage.Question);

                    if (result == System.Windows.MessageBoxResult.Yes)
                    {
                        System.Diagnostics.Process.Start(pathFileTest);
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"An error occurred: {ex.Message}");
                }

            }
            else
            {
                System.Windows.MessageBox.Show("Please select a valid date range.");
            }

            //if (DatePickFrom != null && DatePickTo != null)
            //{
            //    try
            //    {
            //        if (myDataTable != null) myDataTable.Clear();
            //        string startTime = Convert.ToDateTime(DatePickFrom).ToString("yyyy-MM-dd HH:mm:ss");
            //        string endTime = Convert.ToDateTime(DatePickTo).ToString("yyyy-MM-dd HH:mm:ss");
            //        string SelectQuery = $@"SELECT *
            //                    FROM alarmhistory
            //                    WHERE AlarmTime >= '{startTime}' AND AlarmTime <= '{endTime}'";

            //        myDataTable = MySQLProvider.ExecuteQuery(SelectQuery);
            //        if (myDataTable != null)
            //        {
            //            HistoryAlarmList.Clear();

            //            foreach (DataRow alarmRow in myDataTable.Rows)
            //            {
            //                DataSqlModel alarmRecord = new DataSqlModel
            //                {
            //                    IDAlarm = alarmRow["AlarmID"].ToString(),
            //                    AlarmTime = Convert.ToDateTime(alarmRow["AlarmTime"]),
            //                    AlarmClearedTime = Convert.ToDateTime(alarmRow["AlarmClearedTime"]),
            //                    Category = alarmRow["Category"].ToString(),
            //                    Source = alarmRow["Source"].ToString(),
            //                    Description = alarmRow["Description"].ToString()
            //                };
            //            }
            //            logger.Info("Load history Alarm successfully.");
            //        }
            //    }
            //    catch (Exception)
            //    {
            //        MessageBox.Show("Please enter the start date and end date!");
            //    }

            //}
        }
        public void exTestmethod()
        {
            AppService.DIOModule1.IOMapping[DIODescriptions.In_EMOSignal] = true;
            AppService.DIOModule1.IOMapping[DIODescriptions.In_DoorSignal] = true;
            AppService.DIOModule1.IOMapping[DIODescriptions.In_MainAirSignal] = true;

            AppService.DIOModule2.IOMapping[DIODescriptions.In_SesorDetectZStage] = true;
            AppService.DIOModule2.IOMapping[DIODescriptions.In_SensorDetectFixture] = true;
            AppService.DIOModule2.IOMapping[DIODescriptions.In_LeftDoorSignal] = true;
            AppService.DIOModule2.IOMapping[DIODescriptions.In_RightDoorSignal] = true;
            AppService.DIOModule2.IOMapping[DIODescriptions.In_SafetyCircuitSignal] = true;

            CheckDIChanged();
        }
        private void CheckDIChanged()
        {
            _curAlarmList[0] = AppService.DIOModule1.IOMapping[DIODescriptions.In_EMOSignal];
            _curAlarmList[1] = !AppService.DIOModule1.IOMapping[DIODescriptions.In_MainAirSignal];
            _curAlarmList[2] = !AppService.DIOModule1.IOMapping[DIODescriptions.In_DoorSignal];

            _curAlarmList[3] = AppService.DIOModule2.IOMapping[DIODescriptions.In_SesorDetectZStage];
            _curAlarmList[4] = AppService.DIOModule2.IOMapping[DIODescriptions.In_SensorDetectFixture];
            _curAlarmList[5] = false;
            _curAlarmList[6] = !AppService.DIOModule2.IOMapping[DIODescriptions.In_SafetyCircuitSignal];
            _curAlarmList[7] = !AppService.DIOModule2.IOMapping[DIODescriptions.In_LeftDoorSignal];
            _curAlarmList[8] = !AppService.DIOModule2.IOMapping[DIODescriptions.In_RightDoorSignal];
        }

        private void ExportDataTableToFile(DataTable dataTable, string filePath)
        {
            try
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(filePath))
                {

                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        writer.Write(dataTable.Columns[i]);
                        if (i < dataTable.Columns.Count - 1)
                        {
                            writer.Write(",");
                        }
                    }
                    writer.WriteLine();


                    foreach (DataRow row in dataTable.Rows)
                    {
                        for (int i = 0; i < dataTable.Columns.Count; i++)
                        {
                            writer.Write(row[i].ToString());
                            if (i < dataTable.Columns.Count - 1)
                            {
                                writer.Write(",");
                            }
                        }
                        writer.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public List<DataSqlModel> exGetHistoryAlarmsByTimeRange(DateTime startTime, DateTime endTime)
        {
          
            List<DataSqlModel> result = new List<DataSqlModel>();

            try
            {
                string selectQuery = "SELECT * FROM HistoryAlarm WHERE AlarmTime BETWEEN @StartTime AND @EndTime";
                object[] parameters = { startTime, endTime };

                DataTable alarmDataTable = MySQLProvider.ExecuteQuery(selectQuery, parameters);

                if (alarmDataTable != null)
                {
                    foreach (DataRow alarmRow in alarmDataTable.Rows)
                    {
                        DataSqlModel alarmRecord = new DataSqlModel
                        {
                            IDAlarm = alarmRow["AlarmID"].ToString(),
                            AlarmTime = alarmRow.Field<DateTime>("AlarmTime"),
                            AlarmClearedTime = alarmRow.Field<DateTime>("AlarmClearedTime"),
                            Category = alarmRow["Category"].ToString(),
                            Source = alarmRow["Source"].ToString(),
                            Description = alarmRow["Description"].ToString()
                        };

                        result.Add(alarmRecord);
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred while retrieving history alarms: {ex.Message}";
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return result;
        }
    }

    public class Alarm : BindableBase
    {
        private string _idAlarm;
        public string IDAlarm
        {
            get { return _idAlarm; }
            set { SetProperty(ref _idAlarm, value); }
        }
       
        private string _category;
        public string Category
        {
            get { return _category; }
            set { SetProperty(ref _category, value); }
        }
        private string _source;
        public string Source
        {
            get { return _source; }
            set { SetProperty(ref _source, value); }
        }
        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { SetProperty(ref _Description, value); }
        }
        private string _alarmTime;
        public string AlarmTime
        {
            get { return _alarmTime; }
            set { SetProperty(ref _alarmTime, value); }
        }
    }
}
