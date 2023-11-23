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
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using static PORLA.HMI.Service.Configuration.ConfigValue;

namespace POLAR.DIOADAM6052
{
    public partial class Adam6052Module1 : BindableBase, IAdam6052Module1
    {
        private static readonly log4net.ILog logger = 
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
        private AdamSocket adamModbus;
        private string m_szIP;
        private int m_iPort;
        private int m_iDoTotal, m_iDiTotal;
        public readonly System.Timers.Timer _timer;
        public readonly System.Timers.Timer _timerAlarm;
        public Adam6052Module1(IAppService appService, ICompositeAppCommand compositeAppCommand, ILoggerService logInfor,
            IConfigHandler configHandler, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            AppService = appService;
            AppCommand = compositeAppCommand;
            ConfigHandler = configHandler;
            LogInfor = logInfor;
            
            _timer = new System.Timers.Timer();
            //_timerAlarm = new Timer();
            //LoadConfig();
            //Initialize(_appService.DIOModule1.IpAddress);
            InitializeCommand();
        }

        private void LoadConfig()
        {
            AppService.DIOModule1.IpAddress = ConfigHandler.GetValue<string>(ADAMConfig.Module1IPAddress); 
            AppService.DIOModule2.IpAddress = ConfigHandler.GetValue<string>(ADAMConfig.Module2IPAddress);
        }

        private async void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _timer.Stop();

            await RefreshDIO();

            _timer.Start();
        }

        public void Initialize(string IpAddress)
        {
            m_szIP = IpAddress;	// modbus slave IP address
            m_iPort = 502;		// modbus TCP port is 502
            adamModbus = new AdamSocket();
            adamModbus.SetTimeout(1000, 1000, 1000); // set timeout for TCP

            _timer.Elapsed += _timer_Elapsed;
            _timer.Interval = 200;

            m_iDiTotal = 8;
            m_iDoTotal = 8;

            Connect();
        }

        public void Connect()
        {
            try
            {
                if (adamModbus.Connect(m_szIP, ProtocolType.Tcp, m_iPort))
                {
                    _timer.Start(); // enable timer
                    AppService.DIOModule1.Connected = true;
                    //_timerAlarm.Start();
                    AppService.DIOModule1.ConnectionState = HardwareConnectionState.Online;
                    Logging(new LogInfor(LogType.DIOModule1, $"ADAM 6052 - Module 1 - Connected"));
                    logger.Info("[ADAM 6052 - Module 1] - Connected");
                }
                else
                {
                    //Testing 
                    //_timerAlarm.Start();
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
            //adamModbus.Disconnect(); // disconnect slave
            _timer.Stop(); // disable timer
        }

        int iDiStart = 1, iDoStart = 17;
        int iChTotal;
        bool[] bDiData, bDoData, bData;
        bool _isStartPress, _isStopPress;
        public async Task RefreshDIO()
        {
            //int iDiStart = 1, iDoStart = 17;
            //int iChTotal;
            //bool[] bDiData, bDoData, bData;
            await Task.Run(() =>
            {
                if (adamModbus.Modbus().ReadCoilStatus(iDiStart, m_iDiTotal, out bDiData) &&
                    adamModbus.Modbus().ReadCoilStatus(iDoStart, m_iDoTotal, out bDoData))
                {
                    iChTotal = m_iDiTotal + m_iDoTotal;
                    bData = new bool[iChTotal];
                    Array.Copy(bDiData, 0, bData, 0, m_iDiTotal);
                    Array.Copy(bDoData, 0, bData, m_iDiTotal, m_iDoTotal);
                    AppService.DIOModule1.IOMapping = bData;
                    if (AppService.DIOModule1.IOMapping[DIODescriptions.In_VacuumBtn])
                    {
                        SetOutput(DIODescriptions.Out_VacuumOnOffSignal, DIODescriptions.Turn_On);
                    }
                    if (AppService.DIOModule1.IOMapping[DIODescriptions.In_StartSignal])
                    {
                        if (!_isStartPress)
                        {
                            _isStartPress = true;
                            _eventAggregator.GetEvent<StartStopEvent>().Publish(DIODescriptions.Start);
                            _isStartPress = false;
                        }

                    }
                    if (AppService.DIOModule1.IOMapping[DIODescriptions.In_StopSignal])
                    {
                        if (!_isStopPress)
                        {
                            _isStopPress = true;
                            _eventAggregator.GetEvent<StartStopEvent>().Publish(DIODescriptions.Stop);
                            _isStopPress = false;
                        }

                    }
                }
                else
                {
                    AppService.MachineStatus.MachineState = MachineStateEnum.ERROR;
                    Logging(new LogInfor(LogType.DIOModule1, $"ADAM 6052 - Module 1 - RefreshDIO - Can not read status"));
                    logger.Error("[ADAM 6052 - Module 1] - RefreshDIO - Can not read status");
                    _timer.Stop();
                }
            });
            
            //GC.Collect();
        }

        public void SetOutput(int i_iCh, int iOnOff)
        {
            int iStart = 17 + i_iCh - m_iDiTotal;

            _timer.Stop();

            if (adamModbus.Modbus().ForceSingleCoil(iStart, iOnOff))
            {
                RefreshDIO();
            } 
            else
            {
                AppService.MachineStatus.MachineState = MachineStateEnum.ERROR;
                Logging(new LogInfor(LogType.DIOModule1, $"ADAM 6052 - Module 1 - SetOutput - Set digital output failed"));
                logger.Error("[ADAM 6052 - Module 1] - SetOutput - Set digital output failed");
                //MessageBox.Show("Set digital output failed!", "Error");
            }

            _timer.Start();
        }

        public bool CheckSafetyCondition()
        {
            bool _state = false;
            if (AppService.MachineStatus.MachineMode == MachineModeEnum.AUTO)
            {
                if (AppService.MachineStatus.MachineState == MachineStateEnum.RUN)
                {
                    if (CheckDi(DIODescriptions.In_VacuumSignal)
                    & !CheckDi(DIODescriptions.In_SensorDetectFixture)
                    & !CheckDi(DIODescriptions.In_SesorDetectZStage))
                    {
                        _state = true;
                    }
                    //if (!CheckDi(DIODescriptions.In_SensorDetectFixture)
                    //& !CheckDi(DIODescriptions.In_SesorDetectZStage))
                    //{
                    //    _state = true;
                    //}
                    else
                    {
                        if (!CheckDi(DIODescriptions.In_VacuumSignal))
                        {
                            Logging(new LogInfor(LogType.DIOModule2, $"Vacuum is not enough to hold DUT"));
                        }
                        if (CheckDi(DIODescriptions.In_SensorDetectFixture))
                        {
                            Logging(new LogInfor(LogType.DIOModule2, $"Fixture is in danger zone"));
                        }
                        if (CheckDi(DIODescriptions.In_SesorDetectZStage))
                        {
                            Logging(new LogInfor(LogType.DIOModule2, $"Z stage is in danger zone"));
                        }
                        if (CheckDiModule2(DIODescriptions.In_SafetyCircuitSignal))
                        {
                            Logging(new LogInfor(LogType.DIOModule2, $"Safety circuit is violated"));
                        }
                        _state = false;
                    }
                }
                if (AppService.MachineStatus.MachineInitState == MachineInitStateEnum.INITIALIZING)
                {
                    if (CheckDiModule2(DIODescriptions.In_SafetyCircuitSignal)
                        & !CheckDi(DIODescriptions.In_SensorDetectFixture)
                        & !CheckDi(DIODescriptions.In_SesorDetectZStage))
                    {
                        _state = true;
                    }
                    else
                    {
                        if (CheckDi(DIODescriptions.In_SensorDetectFixture))
                        {
                            Logging(new LogInfor(LogType.DIOModule2, $"Fixture is in danger zone"));
                        }
                        if (CheckDi(DIODescriptions.In_SesorDetectZStage))
                        {
                            Logging(new LogInfor(LogType.DIOModule2, $"Z stage is in danger zone"));
                        }
                        if (CheckDiModule2(DIODescriptions.In_SafetyCircuitSignal))
                        {
                            Logging(new LogInfor(LogType.DIOModule2, $"Safety circuit is violated"));
                        }
                        _state = false;
                    }
                }
            }
            else
            {
                if ( !CheckDi(DIODescriptions.In_SensorDetectFixture)
                    & !CheckDi(DIODescriptions.In_SesorDetectZStage))
                {
                    _state = true;
                }
                else
                {
                    if (CheckDi(DIODescriptions.In_SensorDetectFixture))
                    {
                        Logging(new LogInfor(LogType.DIOModule2, $"Fixture is in danger zone"));
                    }
                    if (CheckDi(DIODescriptions.In_SesorDetectZStage))
                    {
                        Logging(new LogInfor(LogType.DIOModule2, $"Z stage is in danger zone"));
                    }
                    if (CheckDiModule2(DIODescriptions.In_SafetyCircuitSignal))
                    {
                        Logging(new LogInfor(LogType.DIOModule2, $"Safety circuit is violated"));
                    }
                    _state = false;
                }
            }
            return _state;
        }

        public bool CheckDi(int iODescriptions)
        {
            bool output = false;
            switch (iODescriptions)
            {
                case DIODescriptions.In_DoorSignal:
                    if (AppService.DIOModule1.IOMapping[DIODescriptions.In_DoorSignal])
                    {
                        Thread.Sleep(200);
                        output = true;
                    }
                    else
                    {
                        output = false;
                    }
                    break;
                case DIODescriptions.In_VacuumSignal:
                    if (AppService.DIOModule1.IOMapping[DIODescriptions.In_VacuumSignal])
                    {
                        Thread.Sleep(200);
                        output = true;
                    }
                    else
                    {
                        output = false;
                    }
                    break;
                case DIODescriptions.In_SesorDetectZStage:
                    if (AppService.DIOModule2.IOMapping[DIODescriptions.In_SesorDetectZStage])
                    {
                        Thread.Sleep(200);
                        AppService.MachineStatus.MachineState = MachineStateEnum.ERROR;
                        output = true;
                    }
                    else
                    {
                        output = false;
                    }
                    break;
                case DIODescriptions.In_SensorDetectFixture:
                    if (AppService.DIOModule2.IOMapping[DIODescriptions.In_SensorDetectFixture])
                    {
                        Thread.Sleep(200);
                        AppService.MachineStatus.MachineState = MachineStateEnum.ERROR;
                        output = true;
                    }
                    else
                    {
                        output = false;
                    }
                    break;
                default:
                    break;
            }
            return output;
        }
        
        public bool CheckDiModule2(int iODes)
        {
            bool output = false;
            switch (iODes)
            {
                case DIODescriptions.In_SafetyCircuitSignal:
                    if (AppService.DIOModule2.IOMapping[DIODescriptions.In_SafetyCircuitSignal])
                    {
                        Thread.Sleep(200);
                        AppService.MachineStatus.MachineState = MachineStateEnum.ERROR;
                        output = true;
                    }
                    else
                    {
                        output = false;
                    }
                    break;
                default:
                    break;
            }
            return output;
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
