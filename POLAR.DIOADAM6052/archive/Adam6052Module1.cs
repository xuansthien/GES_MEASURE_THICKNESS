using Advantech.Adam;
using POLAR.CompositeAppCommand;
using POLAR.EventAggregator;
using PORLA.HMI.Service;
using PORLA.HMI.Service.Configuration;
using PORLA.HMI.Service.Enums;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Timers;
using System.Windows;
using static PORLA.HMI.Service.Configuration.ConfigValue;

namespace POLAR.DIOADAM6052
{
    public partial class Adam6052Module1 : BindableBase, IAdam6052Module1
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IAppService _appService;
        public IAppService AppService
        {
            get { return _appService; }
            set { SetProperty(ref _appService, value); }
        }

        private ICompositeAppCommand _appCommand;
        public ICompositeAppCommand AppCommand
        {
            get { return _appCommand; }
            set { SetProperty(ref _appCommand, value); }
        }

        private IConfigHandler _configHandler;
        public IConfigHandler ConfigHandler
        {
            get { return _configHandler; }
            set { SetProperty(ref _configHandler, value); }
        }

        private ILoggerService _logInfor;
        public ILoggerService LogInfor
        {
            get { return _logInfor; }
            set { SetProperty(ref _logInfor, value); }
        }

        IEventAggregator _eventAggregator;

        private bool m_bStart;
        private AdamSocket adamModbus;
        private Adam6000Type m_Adam6000Type;
        private string m_szIP;
        private int m_iPort;
        private int m_iDoTotal, m_iDiTotal, m_iCount;
        public readonly System.Timers.Timer _timer;
        public readonly System.Timers.Timer _timerAlarm;
        private bool _bStartUp = false;
        bool[] PreviousAlarmList = new bool[8];
        bool[] CurrentAlarmList = new bool[8];
        public Adam6052Module1(IAppService appService, ICompositeAppCommand compositeAppCommand, ILoggerService logInfor,
            IConfigHandler configHandler, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            AppService = appService;
            AppCommand = compositeAppCommand;
            ConfigHandler = configHandler;
            LogInfor = logInfor;
            
            _timer = new Timer();
            _timerAlarm = new Timer();
            LoadConfig();
            Initialize(_appService.DIOModule1.IpAddress);
            InitializeCommand();
        }

        private void LoadConfig()
        {
            AppService.DIOModule1.IpAddress = ConfigHandler.GetValue<string>(ADAMConfig.Module1IPAddress); 
            AppService.DIOModule2.IpAddress = ConfigHandler.GetValue<string>(ADAMConfig.Module2IPAddress);
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _timer.Stop();

            RefreshDIO();

            _timer.Start();
        }

        public void Initialize(string IpAddress)
        {
            m_bStart = false;			// the action stops at the beginning
            m_szIP = IpAddress;	// modbus slave IP address
            m_iPort = 502;				// modbus TCP port is 502
            adamModbus = new AdamSocket();
            adamModbus.SetTimeout(1000, 1000, 1000); // set timeout for TCP

            _timer.Elapsed += _timer_Elapsed;
            _timer.Interval = 200;

            _timerAlarm.Elapsed += _timerAlarm_Elapsed;
            _timerAlarm.Interval = 200;
            _timerAlarm.Start();

            m_Adam6000Type = Adam6000Type.Adam6052;
            m_iDiTotal = 8;
            m_iDoTotal = 8;

            //AppService.DIOModule1.IOMapping[DIODescriptions.In_DoorSignal] = true;
            //AppService.DIOModule1.IOMapping[DIODescriptions.In_EMOSignal] = true;
            //AppService.DIOModule1.IOMapping[DIODescriptions.In_MainAirSignal] = true;
            //AppService.DIOModule2.IOMapping[DIODescriptions.In_SesorDetectZStage] = true;
            //AppService.DIOModule2.IOMapping[DIODescriptions.In_SensorDetectFixture] = true;
            //AppService.DIOModule1.IOMapping[DIODescriptions.In_LeftDoorSignal] = true;
            //AppService.DIOModule1.IOMapping[DIODescriptions.In_RightDoorSignal] = true;
            //AppService.DIOModule1.IOMapping[DIODescriptions.In_SafetyCircuitSignal] = true;

            Connect();
        }

        object lockalarm = new object();

        private void _timerAlarm_Elapsed(object sender, ElapsedEventArgs e)
        {
//            lock(lockalarm)
//            {
//                try
//                {
//                    if (!(AppService.DIOModule1.ConnectionState == HardwareConnectionState.Online) &&
//                        !(AppService.DIOModule2.ConnectionState == HardwareConnectionState.Online))
//                    {
//                        return;
//                    }
//                    CheckDIChanged();
//                    if (!CurrentAlarmList.SequenceEqual(PreviousAlarmList))
//                    {
//                        for (int i = 0; i < CurrentAlarmList.Length; i++)
//                        {
//                            //Raise alarm 
//                            if (CurrentAlarmList[i] == true && PreviousAlarmList[i] == false)
//                            {
//                                _eventAggregator.GetEvent<AlarmRaiseEvent>().Publish(i);
//                                logger.Info($"LoopMonitorAlarm: Finish Publish execute with raise event, alarm No: {i}");
//                            }
//                            //Clear alarm
//                            else if (CurrentAlarmList[i] == false && PreviousAlarmList[i] == true)
//                            {
//                                _eventAggregator.GetEvent<AlarmResetEvent>().Publish(i);
//                                logger.Info($"LoopMonitorAlarm: Finish Publish execute with reset event, alarm No: {i}");
//                            }
//                            //Value out of range
//                            else
//                            {
//                                //logger.Error($"LoopMonitorAlarm -> Current value of Alarm is: {CurrentAlarmList[i]} unexpected");
//                            }
//                            //PreviousAlarmList[i] = CurrentAlarmList[i];
//                        }
//;
//                    }
//                    //else
//                    //{
//                    //    for (int i = 0; i < CurrentAlarmList.Length; i++)
//                    //    {
//                    //        //Raise alarm 
//                    //        if (CurrentAlarmList[i] == true && PreviousAlarmList[i] == false)
//                    //        {
//                    //            _eventAggregator.GetEvent<AlarmRaiseEvent>().Publish(i);
//                    //            logger.Info($"LoopMonitorAlarm: Finish Publish execute with raise event, alarm No: {i}");
//                    //        }
//                    //        //Clear alarm
//                    //        else if (CurrentAlarmList[i] == false && PreviousAlarmList[i] == true)
//                    //        {
//                    //            _eventAggregator.GetEvent<AlarmResetEvent>().Publish(i);
//                    //            logger.Info($"LoopMonitorAlarm: Finish Publish execute with reset event, alarm No: {i}");
//                    //        }
//                    //        //Value out of range
//                    //        else
//                    //        {
//                    //            //logger.Error($"LoopMonitorAlarm -> Current value of Alarm is: {CurrentAlarmList[i]} unexpected");
//                    //        }
//                    //        PreviousAlarmList[i] = CurrentAlarmList[i];
//                    //    }
//                    //    logger.Info($"LoopMonitorAlarm with string: ");
//                    //}
//                }
//                catch (Exception ex)
//                {
//                    logger.Error($"Monitor Alarm fail with exception e: {ex}");
//                }
//            }
        }

        private void CheckDIChanged()
        {

            CurrentAlarmList[0] = AppService.DIOModule1.IOMapping[DIODescriptions.In_DoorSignal];
            CurrentAlarmList[1] = AppService.DIOModule1.IOMapping[DIODescriptions.In_EMOSignal];
            CurrentAlarmList[2] = AppService.DIOModule1.IOMapping[DIODescriptions.In_MainAirSignal];

            CurrentAlarmList[3] = AppService.DIOModule2.IOMapping[DIODescriptions.In_SesorDetectZStage];
            CurrentAlarmList[4] = AppService.DIOModule2.IOMapping[DIODescriptions.In_SensorDetectFixture];
            CurrentAlarmList[5] = AppService.DIOModule1.IOMapping[DIODescriptions.In_LeftDoorSignal];
            CurrentAlarmList[6] = AppService.DIOModule1.IOMapping[DIODescriptions.In_RightDoorSignal];
            CurrentAlarmList[7] = AppService.DIOModule1.IOMapping[DIODescriptions.In_SafetyCircuitSignal];
        }

        public void Connect()
        {
            try
            {
                if (adamModbus.Connect(m_szIP, ProtocolType.Tcp, m_iPort))
                {
                    m_iCount = 0; // reset the reading counter
                    _timer.Start(); // enable timer
                    m_bStart = true; // starting flag
                    AppService.DIOModule1.Connected = true;
                    _timerAlarm.Start();
                    Logging(new LogInfor(LogType.DIOModule1, $"ADAM 6052 - Module 1 - Connected"));
                    logger.Info("[ADAM 6052 - Module 1] - Connected");
                }
                else
                {
                    AppService.DIOModule1.Connected = false;
                    AppService.MachineStatus.MachineState = MachineStateEnum.ERROR;
                    Logging(new LogInfor(LogType.DIOModule1, $"ADAM 6052 - Module 1 - Connect is failure"));
                    logger.Info("[ADAM 6052 - Module 1] - Connect is failure");
                }
            }
            catch (Exception ex)
            {
                AppService.DIOModule1.Connected = false;
                AppService.MachineStatus.MachineState = MachineStateEnum.ERROR;
                Logging(new LogInfor(LogType.DIOModule1, $"ADAM 6052 - Module 1 - something happened {ex} "));
                logger.Error(ex);
            }
        }

        public void Disconnect()
        {
            m_bStart = false;       // starting flag
            _timer.Stop(); // disable timer
            adamModbus.Disconnect(); // disconnect slave
        }

        public void RefreshDIO()
        {
            int iDiStart = 1, iDoStart = 17;
            int iChTotal;
            bool[] bDiData, bDoData, bData;

            if (adamModbus.Modbus().ReadCoilStatus(iDiStart, m_iDiTotal, out bDiData) &&
                    adamModbus.Modbus().ReadCoilStatus(iDoStart, m_iDoTotal, out bDoData))
            {
                iChTotal = m_iDiTotal + m_iDoTotal;
                bData = new bool[iChTotal];
                Array.Copy(bDiData, 0, bData, 0, m_iDiTotal);
                Array.Copy(bDoData, 0, bData, m_iDiTotal, m_iDoTotal);
                AppService.DIOModule1.IOMapping = bData;
            }
            else
            {
                Logging(new LogInfor(LogType.DIOModule1, $"ADAM 6052 - Module 1 - RefreshDIO - Can not read status"));
                logger.Error("[ADAM 6052 - Module 1] - RefreshDIO - Can not read status");
                _timer.Stop();
            }

            GC.Collect();
        }

        public void SetOutput(int i_iCh, int iOnOff)
        {
            int iStart = 17 + i_iCh - m_iDiTotal;

            _timer.Stop();

            if (adamModbus.Modbus().ForceSingleCoil(iStart, iOnOff))
                RefreshDIO();
            else
                Logging(new LogInfor(LogType.DIOModule1, $"ADAM 6052 - Module 1 - SetOutput - Set digital output failed"));
                logger.Error("[ADAM 6052 - Module 1] - SetOutput - Set digital output failed");
                MessageBox.Show("Set digital output failed!", "Error");

            _timer.Start();
        }
        public void Logging(LogInfor _logData)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                AppService.LogInfors.Add(_logData);
                AppService.ScrollViewerVerticalOffset = _appService.LogInfors.Count - 1;
            });
        }

    }
}
