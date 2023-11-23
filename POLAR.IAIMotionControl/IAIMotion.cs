using POLAR.CompositeAppCommand;
using POLAR.DIOADAM6052;
using PORLA.HMI.Service;
using PORLA.HMI.Service.Configuration;
using PORLA.HMI.Service.Enums;
using Prism.Mvvm;
using SuperSimpleTcp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Web.UI.WebControls.WebParts;
using System.Windows;
using static PORLA.HMI.Service.Configuration.ConfigValue;

namespace POLAR.IAIMotionControl
{
    public partial class IAIMotion : BindableBase, IIAIMotion
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

        IAdam6052Module1 _adam6052Module1;

        const string Header = "!";
        private event EventHandler<AxisStatus> AxisStatusChange;
        public event EventHandler<FlagStatus> FlagStatusChange;
        public string AxisPattern = "000";
        public string Speed = "000";
        public string AccDcl = "000";
        public string Station { get; set; } = "00";

        SimpleTcpClient IAIClient;

        public SimpleTcpClientEvents Events;
        public IAIMotion(IAppService appService, ICompositeAppCommand CompositeAppCommand, 
            IConfigHandler configHandler, ILoggerService loggerService, IAdam6052Module1 adam6052Module1)
        {
            _appService = appService;
            AppCommand = CompositeAppCommand;
            ConfigHandler = configHandler;
            LogInfor = loggerService; 
            _adam6052Module1 = adam6052Module1;

            //LoadConfig();
            //Initialize(AppService.IAIMotion.IPIAI, AppService.IAIMotion.PortAuto);
            InitializeCommand();
        }

        private void LoadConfig()
        {
            AppService.IAIMotion.IPIAI = ConfigHandler.GetValue<string>(IAIConfig.IPAddress);
            AppService.IAIMotion.PortAuto = ConfigHandler.GetValue<string>(IAIConfig.PortAuto);
            AppService.IAIMotion.Station = ConfigHandler.GetValue<string>(IAIConfig.Station);
        }

        public void Initialize(string IP, string Port)
        {
            try
            {
                IAIClient = new SimpleTcpClient(IP + ":" + Port);
                Events = new SimpleTcpClientEvents();
                Station = AppService.IAIMotion.Station;
                AxisPattern = AppService.IAIMotion.AxisPattern;
                Speed = AppService.IAIMotion.Speed;
                AccDcl = AppService.IAIMotion.AccDcl;
                Events.DataReceived += Events_DataReceived;

                if (IAIClient.IsConnected)
                {
                    IAIClient.Disconnect();
                }
                IAIClient.Events = Events;
                IAIClient.Connect();
                isconnect = true;
                AppService.IAIMotion.Connected = true;

                bool[] CurrentAxis = { true, true };

                System.Timers.Timer aTimer = new System.Timers.Timer();
                aTimer.Elapsed += ATimer_Elapsed;
                aTimer.Interval = 200;
                aTimer.Enabled = true;

                ResetErr();
                string result = BoolArrToHex(CurrentAxis);
                SetSpeed_AccDcl(Speed, AccDcl);
                if (isconnect)
                {
                    AppService.IAIMotion.ConnectState = "Connected";
                    Logging(new LogInfor(LogType.Motion, $"IAI Motion Connected"));
                    logger.Info("connected to IAI motion");
                    logger.Info($"Axis pattern: {result}");
                    logger.Info($"Set Speed - {Speed} and Acc/Dcc - {AccDcl}");
                }
                AxisStatusChange += IAIMotion_AxisStatusChange;
            }
            catch (Exception ex)
            {
                AppService.IAIMotion.ConnectState = "DisConnected";
                isconnect = false;
                AppService.IAIMotion.Connected = false;
                AppService.MachineStatus.MachineState = MachineStateEnum.ERROR;
                Logging(new LogInfor(LogType.Motion, $"IAI Motion - Connection is failure - {ex}"));
                logger.Error(ex);
            }
        }
        private double _xFixture, _yFixture;
        private void IAIMotion_AxisStatusChange(object sender, AxisStatus e)
        {
            try
            {
                AppService.IAIMotion.XCoordinate = e.X.ToString();
                AppService.IAIMotion.YCoordinate = e.Y.ToString();

                AppService.IAIMotion.XBusy = e.XBusy.ToString();
                AppService.IAIMotion.YBusy = e.YBusy.ToString();

                AppService.IAIMotion.XError = e.XError.ToString();
                AppService.IAIMotion.YError = e.YError.ToString();

                AppService.IAIMotion.XFinish = e.XPositioningCompleted.ToString();
                AppService.IAIMotion.YFinish = e.YPositioningCompleted.ToString();
                AppService.IAIMotion.ServoOn = e.ServoOn;

                if (!string.IsNullOrEmpty(AppService.IAIMotion.XAxisSave) & !string.IsNullOrEmpty(AppService.IAIMotion.YAxisSave))
                {
                    _xFixture = Math.Round(double.Parse(AppService.IAIMotion.XAxisSave, CultureInfo.InvariantCulture), 3);
                    _yFixture = Math.Round(double.Parse(AppService.IAIMotion.YAxisSave, CultureInfo.InvariantCulture), 3);
                }
                AppService.IAIMotion.XAxisFixture = (e.X - _xFixture).ToString();
                AppService.IAIMotion.YAxisFixture = (e.Y - _yFixture).ToString();

            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private void ATimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    ReadAxisStatus();
                    Thread.Sleep(50);
                    //ReadFlag("600", "8");
                }
                catch (Exception)
                {

                }

            });

        }

        private string ReadAxisStatus()
        {
            // ***07**: indicate for 3 axes : x, y, z

            //string data = $"{Header}{Station}21207@@\r\n";

            // ****03**: for 2 axes: x, y
            string data = $"{Header}{Station}21203@@\r\n";
            Send(data);
            return data;
        }

        void Send(string data)
        {
            if (isconnect == true)
            {
                try
                {
                    IAIClient.Send(data);
                }
                catch (Exception)
                {

                }
            }
        }
        async Task SendAsync(string data)
        {
            if (isconnect == true)
            {
                try
                {
                    await IAIClient.SendAsync(data);
                }
                catch (Exception ex)
                {
                    logger.Error($"{ex.Message}");
                }
            }
        }

        private string result;

        private byte[] _convertToArray;
        private void Events_DataReceived(object sender, SuperSimpleTcp.DataReceivedEventArgs e)
        {
            Task.Run(() =>
            {
                switch (e.Data.Array[0])
                {
                    case 35: // #
                        _convertToArray = e.Data.ToArray();
                        result = System.Text.Encoding.UTF8.GetString(_convertToArray);
                        switch (result.Substring(1, 5))
                        {
                            case "99212":
                                AxisStatus(result.Substring(8));
                                break;
                            case "9920D":
                                FlagStatus(result.Substring(8));
                                break;
                            default:
                                break;
                        }
                        break;
                    case 38: //&
                        _convertToArray = e.Data.ToArray();
                        result = System.Text.Encoding.UTF8.GetString(_convertToArray);

                        logger.Error(result);
                        break;

                    default:
                        break;
                }
            });
        }
        void FlagStatus(string value)
        {
            Task.Run(() =>
            {
                try
                {
                    int i = Convert.ToInt32(value.Substring(0, 4), 16);
                    int y = Convert.ToInt32(value.Substring(4, 4), 16);
                    var x = ConvertHexToBitArray(value.Substring(8, 2));
                    FlagStatusChange?.Invoke(this, new FlagStatus(i, y, x));
                }
                catch (Exception)
                {

                }

            });
        }
        void AxisStatus(string status)
        {
            Task.Run(() =>
            {

                //// Returns 1111111111111111111111111111111111111111111111111111111111111111
                //string binRepresentation = Convert.ToString(longValue, 2);
                var x = ConvertHexToBitArray(status.Substring(0, 2));
                var y = ConvertHexToBitArray(status.Substring(16, 2));
                var z = ConvertHexToBitArray(status.Substring(32, 2));
                if (x != null && y != null && z != null)
                {

                    var coordinatesX = Convert.ToUInt64(status.Substring(8, 8), 16);
                    var coordinatesY = Convert.ToUInt64(status.Substring(24, 8), 16);
                    //var coordinatesZ = Convert.ToUInt64(status.Substring(40, 8), 16);
                    var coordinatesZ = 0;
                    AxisStatusChange?.Invoke(this, new AxisStatus((double)coordinatesX / 1000, (double)coordinatesY / 1000, (double)coordinatesZ / 1000, x, y, z));
                }
            });
        }

        bool isconnect = false;
        private void Connect(string IP, string Port)
        {
            IAIClient = new SimpleTcpClient(IP + ":" + Port);
            try
            {
                if (IAIClient.IsConnected)
                {
                    IAIClient.Disconnect();
                }
                IAIClient.Events = Events;
                IAIClient.Connect();
                isconnect = true;
                ResetErr();
            }
            catch (Exception)
            {
                isconnect = false;
            }
        }
        public bool Disconnect()
        {
            bool state = false;
            if (isconnect)
            {
                IAIClient.Disconnect();
                isconnect = false;
                state = true;
            }
            return state;
        }
        public Tuple<string, string> SetSpeed_AccDcl(string speed, string accDcl)
        {
            Speed = StringtoHexString(speed, 4, 1);
            AccDcl = StringtoHexString(accDcl, 4, 100);
            return Tuple.Create(Speed, AccDcl);
        }
        public string CallPoint(string point)
        {
            string data = $"{Header}{Station}237{AxisPattern}{AccDcl}{AccDcl}{Speed}{point}@@\r\n";
            Send(data);
            return data;
        }
        private string RelativeMove(string x, string y, string z)
        {
            string data = $"{Header}{Station}235{AxisPattern}{AccDcl}{AccDcl}{Speed}{StringtoHexString(x, 8, 1000)}{StringtoHexString(y, 8, 1000)}{StringtoHexString(z, 8, 1000)}@@\r\n";
            Send(data);
            return data;
        }
        private string AbsoluteMove(string x, string y, string z)
        {
            string data = $"{Header}{Station}234{AxisPattern}{AccDcl}{AccDcl}{Speed}{StringtoHexString(x, 8, 1000)}{StringtoHexString(y, 8, 1000)}{StringtoHexString(z, 8, 1000)}@@\r\n";
            Send(data);
            return data;
        }
        private async Task<bool> AbsoluteMoveAsync(string x, string y, string z)
        {
            try
            {
                string data = $"{Header}{Station}234{AxisPattern}{AccDcl}{AccDcl}{Speed}{StringtoHexString(x, 8, 1000)}{StringtoHexString(y, 8, 1000)}{StringtoHexString(z, 8, 1000)}@@\r\n";
                await SendAsync(data);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);   
                return false;
            }
            
        }
        public async Task<MotionStatus> AbsoluteMoveWithCheckFinish(string state, CancellationToken cancellationToken)
        {
            MotionStatus _motionStatus = MotionStatus.NONE;
            return await Task.Run(() =>
            {
                if (state == "ABS")
                {
                    if (AppService.IAIMotion.IsSelectedAxisX & AppService.IAIMotion.IsSelectedAxisY & !string.IsNullOrEmpty(AppService.IAIMotion.AxisXValue) & !string.IsNullOrEmpty(AppService.IAIMotion.AxisYValue))
                    {
                        AbsoluteMove(AppService.IAIMotion.AxisXValue, AppService.IAIMotion.AxisYValue, "0");
                        Thread.Sleep(500);
                        while (_motionStatus != MotionStatus.FINISH)
                        {
                            if (cancellationToken.IsCancellationRequested)
                            {
                                OperationStopAndCancel();
                                Logging(new LogInfor(LogType.Sequence, "Stop Moving"));
                                _motionStatus = MotionStatus.EXCEPTION_STATUS;
                                break;
                            }
                            if (AppService.IAIMotion.XFinish == "True" & AppService.IAIMotion.YFinish == "True")
                            {
                                _motionStatus = MotionStatus.FINISH;
                            }
                            if (!AppService.DIOModule2.IOMapping[DIODescriptions.In_SafetyCircuitSignal])
                            {
                                Logging(new LogInfor(LogType.Sequence, "Break out of sequence because safety is violated"));
                                _motionStatus = MotionStatus.EXCEPTION_STATUS;
                                break;
                            }
                            if (AppService.DIOModule2.IOMapping[DIODescriptions.In_ServiceKey])
                            {
                                Logging(new LogInfor(LogType.Sequence, "Break out of sequence because service key is switched"));
                                _motionStatus = MotionStatus.EXCEPTION_STATUS;
                                break;
                            }
                        }
                    }
                    else if (AppService.IAIMotion.IsSelectedAxisY & !string.IsNullOrEmpty(AppService.IAIMotion.AxisYValue))
                    {
                        AbsoluteMove(AppService.IAIMotion.AxisYValue, "0", "0");
                        Thread.Sleep(500);
                        while (_motionStatus != MotionStatus.FINISH)
                        {
                            if (cancellationToken.IsCancellationRequested)
                            {
                                OperationStopAndCancel();
                                Logging(new LogInfor(LogType.Sequence, "Stop Moving"));
                                _motionStatus = MotionStatus.EXCEPTION_STATUS;
                                break;
                            }
                            if (AppService.IAIMotion.YFinish == "True")
                            {
                                _motionStatus = MotionStatus.FINISH;
                            }
                            if (!AppService.DIOModule2.IOMapping[DIODescriptions.In_SafetyCircuitSignal])
                            {
                                Logging(new LogInfor(LogType.Motion, "Safety is violated!!"));
                                _motionStatus = MotionStatus.EXCEPTION_STATUS;
                                break;
                            }
                        }
                    }
                    else if (AppService.IAIMotion.IsSelectedAxisX & !string.IsNullOrEmpty(AppService.IAIMotion.AxisXValue))
                    {
                        AbsoluteMove(AppService.IAIMotion.AxisXValue, "0", "0");
                        Thread.Sleep(500);
                        while (_motionStatus != MotionStatus.FINISH)
                        {
                            if (cancellationToken.IsCancellationRequested)
                            {
                                OperationStopAndCancel();
                                Logging(new LogInfor(LogType.Sequence, "Stop Moving"));
                                _motionStatus = MotionStatus.EXCEPTION_STATUS;
                                break;
                            }
                            if (AppService.IAIMotion.XFinish == "True")
                            {
                                _motionStatus = MotionStatus.FINISH;
                            }
                            if (!AppService.DIOModule2.IOMapping[DIODescriptions.In_SafetyCircuitSignal])
                            {
                                Logging(new LogInfor(LogType.Motion, "Safety is violated!!"));
                                _motionStatus = MotionStatus.EXCEPTION_STATUS;
                                break;
                            }
                        }
                    }
                }
                if (state == "home")
                {
                    AbsoluteMove("0", "0", "0");
                    Thread.Sleep(500);
                    while (_motionStatus != MotionStatus.FINISH)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            OperationStopAndCancel();
                            Logging(new LogInfor(LogType.Sequence, "Stop Moving"));
                            logger.Info("Home request - Stop Moving");
                            _motionStatus = MotionStatus.EXCEPTION_STATUS;
                            break;
                        }
                        if (AppService.IAIMotion.XFinish == "True" & AppService.IAIMotion.YFinish == "True")
                        {
                            _motionStatus = MotionStatus.FINISH;
                        }
                    }

                }
                return _motionStatus;
            });
            
        }
        public async Task<MotionStatus> PatternMove(string[] paramters, CancellationToken cancellationToken)
        {
            MotionStatus _motionStatus = MotionStatus.NONE;
            // string[] paraPaternMove = {Dx, Dy, Pitchy, SpeedPatern, Acc, Dcc}; // get value from recipe
            return await Task.Run(() =>
            {
                UpdateRealVariable("X1", paramters[0]);
                Thread.Sleep(100);
                UpdateRealVariable("Y1", paramters[1]);
                Thread.Sleep(100);
                UpdateRealVariable("DX", paramters[2]);
                Thread.Sleep(100);
                UpdateRealVariable("DY", paramters[3]);
                Thread.Sleep(100);
                //UpdateIntegerVariable("PitchY", paramters[4]);
                UpdateRealVariable("PitchY", paramters[4]);
                Thread.Sleep(100);
                UpdateIntegerVariable("SpeedPattern", paramters[5]);
                Thread.Sleep(150);
                // Program: 12 is define in IAI controller script for pattern moving
                RunProgram("15");
                Thread.Sleep(10000);
                while (_motionStatus != MotionStatus.FINISH)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        StopProgram("15");
                        Logging(new LogInfor(LogType.Sequence, "Stop request!"));
                        _motionStatus = MotionStatus.EXCEPTION_STATUS;
                        break;
                    }
                    if (AppService.IAIMotion.XFinish == "True" & AppService.IAIMotion.YFinish == "True")
                    {
                        StopProgram("15");
                        Logging(new LogInfor(LogType.Sequence, "move done!"));
                        _motionStatus = MotionStatus.FINISH;
                    }
                    if (!AppService.DIOModule2.IOMapping[DIODescriptions.In_SafetyCircuitSignal])
                    {
                        StopProgram("15");
                        Logging(new LogInfor(LogType.Sequence, "Break out of sequence because safety is violated"));
                        _motionStatus = MotionStatus.EXCEPTION_STATUS;
                        break;
                    }
                    if (AppService.DIOModule2.IOMapping[DIODescriptions.In_ServiceKey])
                    {
                        StopProgram("15");
                        Logging(new LogInfor(LogType.Sequence, "Break out of sequence because service key is switched"));
                        _motionStatus = MotionStatus.EXCEPTION_STATUS;
                        break;
                    }
                }
                return _motionStatus;
            });
        }

        private string AbsoluteSingleAxisMove(string axisPattern,string acc, string dcc, string speed, string value)
        {
            string data = "";
            string speed1 = StringtoHexString(speed, 4, 1);
            string acc1 = StringtoHexString(acc, 4, 100);
            string dcc1 = StringtoHexString(dcc, 4, 100);
            switch (axisPattern)
            {
                case "01":
                    data = $"{Header}{Station}234{axisPattern}{acc1}{dcc1}{speed1}{StringtoHexString(value, 8, 1000)}{StringtoHexString("0", 8, 1000)}{StringtoHexString("0", 8, 1000)}@@\r\n";
                    Send(data);
                    break;
                case "02":
                    data = $"{Header}{Station}234{axisPattern}{acc1}{acc1}{acc1}{StringtoHexString(value, 8, 1000)}{StringtoHexString("0", 8, 1000)}{StringtoHexString("0", 8, 1000)}@@\r\n";
                    Send(data);
                    break;
                default:
                    break;
            }
            
            return data;
        }
        public string ChangePoint(PointData point, string pointnumber)
        {
            string data = $"{Header}{Station}245001{StringtoHexString(pointnumber, 3, 1)}{AxisPattern}{AccDcl}{AccDcl}{Speed}{point.X}{point.Y}{point.Z}@@\r\n";
            Send(data);
            return data;
        }
        private string ServoON_OFF(int value)
        {
            string data = $"{Header}{Station}232{AxisPattern}{value}@@\r\n";
            Send(data);
            return data;
        }
        public string ReadFlag(string start, string number)
        {
            string data = $"{Header}{Station}20D00{StringtoHexString(start, 4, 1)}{StringtoHexString(number, 4, 1)}@@\r\n";
            Send(data);
            return data;
        }
        private string FlagChange(string number, string type)
        {
            string data = $"{Header}{Station}24B00{StringtoHexString(number, 4, 1)}{StringtoHexString(type, 1, 1)}@@\r\n";
            Send(data);
            return data;
        }
        private string RunProgram(string progarmNumber)
        {
            string data = $"{Header}{Station}253{StringtoHexString(progarmNumber, 2, 1)}@@\r\n";
            Send(data);
            return data;
        }
        public string StopProgram(string progarmNumber)
        {
            string data = $"{Header}{Station}254{StringtoHexString(progarmNumber, 2, 1)}@@\r\n";
            Send(data);

            return data;
        }
        public string ResetErr()
        {
            string data = $"{Header}{Station}252@@\r\n";
            Send(data);
            return string.Empty;
        }
        public string SoftReset()
        {
            string data = $"{Header}{Station}25B@@\r\n";
            Send(data);
            return string.Empty;
        }
        private string Jogging(string axes, string ot)
        {
            string data = $"{Header}{Station}236{AxisPattern}{AccDcl}{AccDcl}{Speed}{StringtoHexString(axes, 8, 1000)}{ot}@@\r\n";
            Send(data);
            return data;
        }
        public string OperationStopAndCancel()
        {
            string data = $"{Header}{Station}238{AxisPattern}01@@\r\n";
            Send(data);
            return data;
        }
        private string UpdateIntegerSpeedVariable(string speed)
        {
            // Hex: E6 -> DEC: 230: register number 230
            string data = $"{Header}{Station}24C000E601{StringtoHexString(speed, 8, 1)}7@@\r\n";
            Send(data);
            return data;
        }
        private string UpdateIntegerVariable(string register, string value)
        {
            string data = "";
            switch (register)
            {
                case "DX":
                    // Hex: C8 -> DEC: 200: register number 200
                    data = $"{Header}{Station}24C000C801{StringtoHexString(value, 8, 1)}7@@\r\n";
                    Send(data);
                    break;
                case "DY":
                    // Hex: C9 -> DEC: 201: register number 201
                    data = $"{Header}{Station}24C000C901{StringtoHexString(value, 8, 1)}7@@\r\n";
                    Send(data);
                    break;
                case "PitchY":
                    // Hex: CA -> DEC: 202: register number 202
                    data = $"{Header}{Station}24C000CA01{StringtoHexString(value, 8, 1)}7@@\r\n";
                    Send(data);
                    break;
                case "SpeedPattern":
                    // Hex: 104 -> DEC: 260: register number 260
                    data = $"{Header}{Station}24C0010401{StringtoHexString(value, 8, 1)}7@@\r\n";
                    Send(data);
                    break;
                case "AccPattern":
                    // Hex: 105 -> DEC: 261: register number 261
                    data = $"{Header}{Station}24C0010501{StringtoHexString(value, 8, 1)}1@@\r\n";
                    Send(data);
                    break;
                case "DclPattern":
                    // Hex: 106 -> DEC: 262: register number 262
                    data = $"{Header}{Station}24C0010601{StringtoHexString(value, 8, 1)}1@@\r\n";
                    Send(data);
                    break;
                case "X1":
                    // Hex: 4B1 -> DEC: 1201: register number 1201
                    data = $"{Header}{Station}24C004B101{StringtoHexString(value, 8, 1)}1@@\r\n";
                    Send(data);
                    break;
                case "Y1":
                    // Hex: 4B3 -> DEC: 1203: register number 1203
                    data = $"{Header}{Station}24C004B301{StringtoHexString(value, 8, 1)}1@@\r\n";
                    Send(data);
                    break;
                default:
                    break;
            }
            return data;
        }
        private string UpdateRealVariable(string register, string value)
        {
            string data = "";
            
            switch (register)
            {
                case "X1":
                    // Hex: 515 -> DEC: 1301: register number 1301
                    data = $"{Header}{Station}24D0051501{StringtoHexString16(value)}1@@\r\n";
                    Send(data);
                    break;
                case "Y1":
                    // Hex: 517 -> DEC: 1303: register number 1303
                    data = $"{Header}{Station}24D0051701{StringtoHexString16(value)}1@@\r\n";
                    Send(data);
                    break;
                case "DX":
                    // Hex: 15E -> DEC: 350: register number 350
                    data = $"{Header}{Station}24D0015E01{StringtoHexString16(value)}1@@\r\n";
                    Send(data);
                    break;
                case "DY":
                    // Hex: 15F -> DEC: 351: register number 351
                    data = $"{Header}{Station}24D0015F01{StringtoHexString16(value)}1@@\r\n";
                    Send(data);
                    break;
                case "PitchY":
                    //// Hex: CA -> DEC: 202: register number 202
                    //data = $"{Header}{Station}24C000CA01{StringtoHexString(value, 8, 1)}7@@\r\n";
                    // Hex: 160 -> DEC: 352: register number 352
                    data = $"{Header}{Station}24D0016001{StringtoHexString16(value)}1@@\r\n";
                    Send(data);
                    break;
                case "SpeedPattern":
                    // Hex: 104 -> DEC: 260: register number 260
                    data = $"{Header}{Station}24C0010401{StringtoHexString(value, 8, 1)}1@@\r\n";
                    Send(data);
                    break;
                case "AccPattern":
                    // Hex: 12C -> DEC: 300: register number 300
                    data = $"{Header}{Station}24D0012C01{StringtoHexString16(value)}1@@\r\n";
                    Send(data);
                    break;
                case "DclPattern":
                    // Hex: 12D -> DEC: 301: register number 301
                    data = $"{Header}{Station}24D0012D01{StringtoHexString16(value)}1@@\r\n";
                    Send(data);
                    break;
                default:
                    break;
            }
            return data;
        }
        public static string StringtoHexString(string stringnumber, int pad, double mul = 1)
        {
            //stringnumber = stringnumber.Replace('.', ',');
            string Hex = "0".PadLeft(pad, '0'); ;
            bool p = double.TryParse(stringnumber, out double value);
            if (p) { Hex = ((int)(value * mul)).ToString("X").PadLeft(pad, '0'); }
            return Hex;
        }
        public static string StringtoHexString16(string stringnumber)
        {
            double doubleValue = double.Parse(stringnumber);
            string Hex = BitConverter.DoubleToInt64Bits(doubleValue).ToString("X");
            return Hex;
        }
        public string BoolArrToHex(bool[] Axis)
        {
            string input = string.Empty;
            foreach (var a in Axis)
            {
                input = input + Convert.ToInt16(a).ToString();
            }
            AxisPattern = Convert.ToInt32(input, 2).ToString("X").PadLeft(2, '0');
            return AxisPattern;
        }
        public static BitArray ConvertHexToBitArray(string hexData)
        {
            if (hexData == null)
                return null; // or do something else, throw, ...
            BitArray ba = new BitArray(4 * hexData.Length);
            for (int i = 0; i < hexData.Length; i++)
            {
                byte b = byte.Parse(hexData[i].ToString(), NumberStyles.HexNumber);
                for (int j = 0; j < 4; j++)
                {
                    ba.Set(i * 4 + j, (b & (1 << (3 - j))) != 0);
                }
            }
            return ba;
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
    public class AxisStatus : EventArgs
    {
        public double X { get; private set; } = 0;
        public double Y { get; private set; } = 0;
        public double Z { get; private set; } = 0;
        public bool XBusy { get; private set; } = false;
        public bool XPositioningCompleted { get; private set; } = false;
        public bool XError { get; private set; } = false;
        public bool YBusy { get; private set; } = false;
        public bool YPositioningCompleted { get; private set; } = false;
        public bool YError { get; private set; } = false;
        public bool ZBusy { get; private set; } = false;
        public bool ZPositioningCompleted { get; private set; } = false;
        public bool ZError { get; private set; } = false;
        public bool ServoOn { get; private set; } = false;

        public AxisStatus(double x, double y, double z, BitArray statusX, BitArray statusY, BitArray statusZ)
        {
            X = x;
            Y = y;
            Z = z;
            XBusy = statusY[7];
            YBusy = statusY[7];
            ZBusy = statusZ[7];
            XError = statusX[7] == false && statusX[4] == false && statusX[5] == false;
            YError = statusY[7] == false && statusY[4] == false && statusY[5] == false;
            ZError = statusZ[7] == false && statusZ[4] == false && statusZ[5] == false;
            XPositioningCompleted = statusX[7] == false && statusX[5] == true;
            YPositioningCompleted = statusY[7] == false && statusY[5] == true;
            ZPositioningCompleted = statusZ[7] == false && statusZ[5] == true;
            ServoOn = statusX[4];
        }
    }
    public class FlagStatus
    {
        public int Startnumber { get; private set; } = 0;
        public int NumbersofFlag { get; private set; } = 0;
        public BitArray Flags { get; private set; } = new BitArray(8);
        public FlagStatus(int start, int number, BitArray flags)
        {
            Startnumber = start; NumbersofFlag = number;
            for (int i = 0; i < 8; i++)
            {
                Flags[i] = flags[7 - i];
            }
        }
    }
    public class PointData
    {
        public string X { get; private set; } = "00000000";
        public string Y { get; private set; } = "00000000";
        public string Z { get; private set; } = "00000000";
        public PointData(string x, string y, string z)
        {
            X = IAIMotion.StringtoHexString(x, 8, 1000);
            Y = IAIMotion.StringtoHexString(y, 8, 1000);
            Z = IAIMotion.StringtoHexString(z, 8, 1000);
        }
    }
}

