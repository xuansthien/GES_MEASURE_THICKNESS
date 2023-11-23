using Advantech.Adam;
using PORLA.HMI.Service;
using PORLA.HMI.Service.Enums;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace POLAR.DIOADAM6052
{
    public class Adam6052Module2 : BindableBase, IAdam6052Module2
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IAppService _appService;
        public IAppService AppService
        {
            get { return _appService; }
            set { SetProperty(ref _appService, value); }
        }
        private ILoggerService _logInfor;
        public ILoggerService LogInfor
        {
            get { return _logInfor; }
            set { SetProperty(ref _logInfor, value); }
        }
        private AdamSocket adamModbus;
        private string m_szIP;
        private int m_iPort;
        private int m_iDoTotal, m_iDiTotal;
        public readonly System.Timers.Timer _timer;

        public Adam6052Module2(IAppService appService, ILoggerService loggerService)
        {
            AppService = appService;
            LogInfor = loggerService;

            _timer = new System.Timers.Timer();
            Initialize(_appService.DIOModule2.IpAddress);
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _timer.Stop();

            RefreshDIO();

            _timer.Start();
        }

        public void Initialize(string IpAddress)
        {
            m_szIP = IpAddress;	// modbus slave IP address
            m_iPort = 502;				// modbus TCP port is 502
            adamModbus = new AdamSocket();
            adamModbus.SetTimeout(1000, 1000, 1000); // set timeout for TCP

            _timer.Elapsed += _timer_Elapsed;
            m_iDiTotal = 7;
            m_iDoTotal = 7;

            Connect();
        }
        public void Uninitialize()
        {
            _timer.Stop();
        }

        public void Connect()
        {
            try
            {
                if (adamModbus.Connect(m_szIP, ProtocolType.Tcp, m_iPort))
                {
                    _timer.Start(); // enable timer
                    AppService.DIOModule2.Connected = true;
                    AppService.DIOModule2.ConnectionState = HardwareConnectionState.Online;
                    Logging(new LogInfor(LogType.DIOModule1, $"ADAM 6052 - Module 2 - Connected"));
                    logger.Info("[ADAM 6052 - Module 2] - Connected");
                }
                else
                {
                    AppService.DIOModule2.Connected = false;
                    AppService.MachineStatus.MachineState = MachineStateEnum.ERROR;
                    Logging(new LogInfor(LogType.DIOModule2, $"ADAM 6052 - Module 2 - Connect is failure"));
                    logger.Info("[ADAM 6052 - Module 2] - Connect is failure");
                }
            }
            catch (Exception ex)
            {
                AppService.DIOModule2.Connected = false;
                AppService.MachineStatus.MachineState = MachineStateEnum.ERROR;
                Logging(new LogInfor(LogType.DIOModule2, $"ADAM 6052 - Module 2 - something happened {ex} "));
                logger.Error(ex);
            }
        }

        public void Disconnect()
        {
            _timer.Stop();
            //adamModbus.Disconnect(); // disconnect slave
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
                AppService.DIOModule2.IOMapping = bData;
            }
            else
            {
                AppService.MachineStatus.MachineState = MachineStateEnum.ERROR;
                Logging(new LogInfor(LogType.DIOModule2, $"ADAM 6052 - Module 2 - RefreshDIO - Can not read status"));
                logger.Error("[ADAM 6052 - Module 2] - RefreshDIO - Can not read status");
                _timer.Stop();
            }

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
                Logging(new LogInfor(LogType.DIOModule2, $"ADAM 6052 - Module 2 - SetOutput - Set digital output failed"));
                logger.Error("[ADAM 6052 - Module 2] - SetOutput - Set digital output failed");
                MessageBox.Show("Set digital output failed!", "Error");
            }

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
