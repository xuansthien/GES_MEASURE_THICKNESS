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

        private bool m_bStart;
        private AdamSocket adamModbus;
        private Adam6000Type m_Adam6000Type;
        private string m_szIP;
        private int m_iPort;
        private int m_iDoTotal, m_iDiTotal, m_iCount;
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
            m_bStart = false;			// the action stops at the beginning
            m_szIP = IpAddress;	// modbus slave IP address
            m_iPort = 502;				// modbus TCP port is 502
            adamModbus = new AdamSocket();
            adamModbus.SetTimeout(1000, 1000, 1000); // set timeout for TCP

            _timer.Elapsed += _timer_Elapsed;
            m_Adam6000Type = Adam6000Type.Adam6052; // the sample is for ADAM-6052
            m_iDiTotal = 7;
            m_iDoTotal = 7;

            Connect();
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
                    AppService.DIOModule2.Connected = true;
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
            m_bStart = false;       // starting flag
            _timer.Stop();
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
                AppService.DIOModule2.IOMapping = bData;
            }
            else
            {
                Logging(new LogInfor(LogType.DIOModule2, $"ADAM 6052 - Module 2 - RefreshDIO - Can not read status"));
                logger.Error("[ADAM 6052 - Module 2] - RefreshDIO - Can not read status");
                _timer.Stop();
            }

            GC.Collect();
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
