using Microsoft.Xaml.Behaviors.Layout;
using POLAR.ModelAggregator.Recipes;
using PORLA.HMI.Service.Enums;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace PORLA.HMI.Service
{
    public class AppService : BindableBase, IAppService
    {
        private MachineRunStatus _machineStatus = new MachineRunStatus();
        public MachineRunStatus MachineStatus
        {
            get { return _machineStatus; }
            set { SetProperty(ref _machineStatus, value); }
        }

        private IAIModbusTCPIP _iAIMotion = new IAIModbusTCPIP();
        public IAIModbusTCPIP IAIMotion
        {
            get { return _iAIMotion; }
            set { SetProperty(ref _iAIMotion, value); }
        }

        private DIOAdam6052Module1 _dIOModule1 = new DIOAdam6052Module1();
        public DIOAdam6052Module1 DIOModule1
        {
            get { return _dIOModule1; }
            set { SetProperty(ref _dIOModule1, value); }
        }

        private DIOAdam6052Module2 _dIOModule2 = new DIOAdam6052Module2();
        public DIOAdam6052Module2 DIOModule2
        {
            get { return _dIOModule2; }
            set { SetProperty(ref _dIOModule2, value); }
        }

        private ObservableCollection<LogInfor> _logInfors = new ObservableCollection<LogInfor>();
        public ObservableCollection<LogInfor> LogInfors
        {
            get { return _logInfors; }
            set { SetProperty(ref _logInfors, value); }
        }

        private ObservableCollection<LogRspPrecitec> _logRspPrecitec = new ObservableCollection<LogRspPrecitec>();
        public ObservableCollection<LogRspPrecitec> LogRspPrecitec
        {
            get { return _logRspPrecitec; }
            set { SetProperty(ref _logRspPrecitec, value); }
        }

        private double _scrollViewerVerticalOffset;
        public double ScrollViewerVerticalOffset
        {
            get { return _scrollViewerVerticalOffset; }
            set { SetProperty(ref _scrollViewerVerticalOffset, value); }
        }

        private RecipeConfig _recipeService = new RecipeConfig();
        public RecipeConfig RecipeService
        {
            get { return _recipeService; }
            set { SetProperty(ref _recipeService, value); }
        }

        private PrecitecControl _precitecService = new PrecitecControl();
        public PrecitecControl PrecitecService
        {
            get { return _precitecService; }
            set { SetProperty(ref _precitecService, value); }
        }

        private bool[] _previousAlarmList = new bool[8];
        public bool[] PreviousAlarmList
        {
            get { return _previousAlarmList; }
            set { SetProperty(ref _previousAlarmList, value); }
        }

        private bool[] _currentAlarmList = new bool[8];
        public bool[] CurrentAlarmList
        {
            get { return _currentAlarmList; }
            set { SetProperty(ref _currentAlarmList, value); }
        }

        private bool _alarmPresence;
        public bool AlarmPresence
        {
            get { return _alarmPresence; }
            set { SetProperty(ref _alarmPresence, value); }
        }

        private string _dutBarcode = "";
        public string DutBarcode
        {
            get { return _dutBarcode; }
            set { SetProperty(ref _dutBarcode, value); }
        }

        private bool _isEnable = true;
        public bool IsEnable
        {
            get { return _isEnable; }
            set { SetProperty(ref _isEnable, value); }
        }
        private string _pLogin;
        public string pageLogin
        {
            get { return this._pLogin; }
            set
            {

                if (!string.Equals(this._pLogin, value))
                {
                    this._pLogin = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string _ppanel;
        public string pagePanel
        {
            get { return this._ppanel; }
            set
            {

                if (!string.Equals(this._ppanel, value))
                {
                    this._ppanel = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string _spanel;
        public string settingPanel
        {
            get { return this._spanel; }
            set
            {

                if (!string.Equals(this._spanel, value))
                {
                    this._spanel = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string _language;
        public string language
        {
            get { return this._language; }
            set
            {

                if (!string.Equals(this._language, value))
                {
                    this._language = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string _user = "Operator";
        public string user
        {
            get { return this._user; }
            set
            {

                if (!string.Equals(this._user, value))
                {
                    this._user = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string _userType = "Operator";
        public string userType
        {
            get { return this._userType; }
            set
            {

                if (!string.Equals(this._userType, value))
                {
                    this._userType = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string _pload;
        public string pageLoad
        {
            get { return this._pload; }
            set
            {

                if (!string.Equals(this._pload, value))
                {
                    this._pload = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string _slogin = "false";
        public string Slogin
        {
            get { return this._slogin; }
            set
            {

                if (!string.Equals(this._slogin, value))
                {
                    this._slogin = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private bool[] _AlarmArray = new bool[100];
        public bool[] AlarmArray
        {
            get { return this._AlarmArray; }
            set
            {

                if (!bool.Equals(this._AlarmArray, value))
                {
                    this._AlarmArray = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private string _HMIVer = "";
        public string HMIVer
        {
            get { return this._HMIVer; }
            set
            {

                if (!string.Equals(this._HMIVer, value))
                {
                    this._HMIVer = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private string _VisionVer = "";
        public string VisionVer
        {
            get { return this._VisionVer; }
            set
            {

                if (!string.Equals(this._VisionVer, value))
                {
                    this._VisionVer = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private bool _ManualModeActivated;
        public bool ManualModeActivated
        {
            get { return this._ManualModeActivated; }
            set
            {

                if (!bool.Equals(this._ManualModeActivated, value))
                {
                    this._ManualModeActivated = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private string _Exposure;
        public string Exposure
        {
            get { return this._Exposure; }
            set
            {

                if (!string.Equals(this._Exposure, value))
                {
                    this._Exposure = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private bool _settingModeActivated;
        public bool SettingModeActivated
        {
            get { return this._settingModeActivated; }
            set
            {

                if (!bool.Equals(this._settingModeActivated, value))
                {
                    this._settingModeActivated = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private string _recipeName = "";
        public string RecipeName
        {
            get { return _recipeName; }
            set { SetProperty(ref _recipeName, value); }
        }

        private ObservableCollection<int> _ProductionCounter = new ObservableCollection<int>();
        public ObservableCollection<int> ProductionCounter { get { return _ProductionCounter; } set { SetProperty(ref _ProductionCounter, value); } }

        private ObservableCollection<double> _TimeDetail = new ObservableCollection<double>();
        public ObservableCollection<double> TimeDetail { get { return _TimeDetail; } set { SetProperty(ref _TimeDetail, value); } }

    }
    public class IAIModbusTCPIP : BindableBase
    {
        private bool _connected;
        public bool Connected
        {
            get { return _connected; }
            set { SetProperty(ref _connected, value); }
        }
        private string _ipIAI = "192.168.1.19";
        public string IPIAI
        {
            get { return _ipIAI; }
            set { SetProperty(ref _ipIAI, value); }
        }

        private string _portAuto = "64516";
        public string PortAuto
        {
            get { return _portAuto; }
            set { SetProperty(ref _portAuto, value); }
        }

        private string _portManual = "64511";
        public string PortManual
        {
            get { return _portManual; }
            set { SetProperty(ref _portManual, value); }
        }

        private string _station = "99";
        public string Station
        {
            get { return _station; }
            set { SetProperty(ref _station, value); }
        }

        private string _axisPattern = "000";
        public string AxisPattern
        {
            get { return _axisPattern; }
            set { SetProperty(ref _axisPattern, value); }
        }

        private bool _servoOn = false;
        public bool ServoOn
        {
            get { return _servoOn; }
            set { SetProperty(ref _servoOn, value); }
        }

        private string _speed = "50";
        public string Speed
        {
            get { return _speed; }
            set { SetProperty(ref _speed, value); }
        }

        private string _accDcl = "0.3";
        public string AccDcl
        {
            get { return _accDcl; }
            set { SetProperty(ref _accDcl, value); }
        }

        private string _speedX = "200";
        public string SpeedX
        {
            get { return _speedX; }
            set { SetProperty(ref _speedX, value); }
        }

        private string _accX = "0.3";
        public string AccX
        {
            get { return _accX; }
            set { SetProperty(ref _accX, value); }
        }
        private string _dclX = "0.4";
        public string DclX
        {
            get { return _dclX; }
            set { SetProperty(ref _dclX, value); }
        }

        private string _speedY = "250";
        public string SpeedY
        {
            get { return _speedY; }
            set { SetProperty(ref _speedY, value); }
        }

        private string _accY = "0.35";
        public string AccY
        {
            get { return _accY; }
            set { SetProperty(ref _accY, value); }
        }
        private string _dclY = "0.45";
        public string DclY
        {
            get { return _dclY; }
            set { SetProperty(ref _dclY, value); }
        }

        private string _connectState = "";
        public string ConnectState
        {
            get { return _connectState; }
            set { SetProperty(ref _connectState, value); }
        }

        private bool _isSelectedAxisX = true;
        public bool IsSelectedAxisX
        {
            get { return _isSelectedAxisX; }
            set { SetProperty(ref _isSelectedAxisX, value); }
        }
        private bool _isSelectedAxisY = true;
        public bool IsSelectedAxisY
        {
            get { return _isSelectedAxisY; }
            set { SetProperty(ref _isSelectedAxisY, value); }
        }

        private string _axisXValue = "0";
        public string AxisXValue
        {
            get { return _axisXValue; }
            set { SetProperty(ref _axisXValue, value); }
        }

        private string _axisYValue = "0";
        public string AxisYValue
        {
            get { return _axisYValue; }
            set { SetProperty(ref _axisYValue, value); }
        }
        // Axis status
        private string _xCoordinate = "0";
        public string XCoordinate
        {
            get { return _xCoordinate; }
            set { SetProperty(ref _xCoordinate, value); }
        }

        private string _xBusy = "";
        public string XBusy
        {
            get { return _xBusy; }
            set { SetProperty(ref _xBusy, value); }
        }

        private string _xFinish = "";
        public string XFinish
        {
            get { return _xFinish; }
            set { SetProperty(ref _xFinish, value); }
        }

        private string _xError = "";
        public string XError
        {
            get { return _xError; }
            set { SetProperty(ref _xError, value); }
        }

        private string _yCoordinate = "0";
        public string YCoordinate
        {
            get { return _yCoordinate; }
            set { SetProperty(ref _yCoordinate, value); }
        }

        private string _yBusy = "";
        public string YBusy
        {
            get { return _yBusy; }
            set { SetProperty(ref _yBusy, value); }
        }

        private string _yFinish = "";
        public string YFinish
        {
            get { return _yFinish; }
            set { SetProperty(ref _yFinish, value); }
        }

        private string _yError = "";
        public string YError
        {
            get { return _yError; }
            set { SetProperty(ref _yError, value); }
        }
        private string _axisXSelected = "01";
        public string AxisXSelected
        {
            get { return _axisXSelected; }
            set { SetProperty(ref _axisXSelected, value); }
        }
        private string _axisYSelected = "02";
        public string AxisYSelected
        {
            get { return _axisYSelected; }
            set { SetProperty(ref _axisYSelected, value); }
        }

        // Motion Recipe
        private string _rcxOriginalPos = "";
        public string RcXOriginalPos
        {
            get { return _rcxOriginalPos; }
            set { SetProperty(ref _rcxOriginalPos, value); }
        }
        private string _rcyOriginalPos = "";
        public string RcYOriginalPos
        {
            get { return _rcyOriginalPos; }
            set { SetProperty(ref _rcyOriginalPos, value); }
        }
        private string _rcdXDistance = "";
        public string RcdXDistance
        {
            get { return _rcdXDistance; }
            set { SetProperty(ref _rcdXDistance, value); }
        }
        private string _rcdYDistance = "";
        public string RcdYDistance
        {
            get { return _rcdYDistance; }
            set { SetProperty(ref _rcdYDistance, value); }
        }
        private string _rcSpeedAxisX = "";
        public string RcSpeedAxisX
        {
            get { return _rcSpeedAxisX; }
            set { SetProperty(ref _rcSpeedAxisX, value); }
        }
        private string _rcSpeedAxisY = "";
        public string RcSpeedAxisY
        {
            get { return _rcSpeedAxisY; }
            set { SetProperty(ref _rcSpeedAxisY, value); }
        }
        private string _rcPitchX = "";
        public string RcPitchX
        {
            get { return _rcPitchX; }
            set { SetProperty(ref _rcPitchX, value); }
        }

        private string _rcPitchY = "";
        public string RcPitchY
        {
            get { return _rcPitchY; }
            set { SetProperty(ref _rcPitchY, value); }
        }

        // Pattern Moving
        private string _pmX1 = "0";
        public string PmX1
        {
            get { return _pmX1; }
            set { SetProperty(ref _pmX1, value); }
        }

        private string _pmY1 = "0";
        public string PmY1
        {
            get { return _pmY1; }
            set { SetProperty(ref _pmY1, value); }
        }

        private string _pmWidthDx = "100";
        public string PmWidthDx
        {
            get { return _pmWidthDx; }
            set { SetProperty(ref _pmWidthDx, value); }
        }

        private string _pmHeightDy = "100";
        public string PmHeightDy
        {
            get { return _pmHeightDy; }
            set { SetProperty(ref _pmHeightDy, value); }
        }

        private string _pmPitchy = "1";
        public string PmPitchy
        {
            get { return _pmPitchy; }
            set { SetProperty(ref _pmPitchy, value); }
        }

        private string _pmSpeed = "50";
        public string PmSpeed
        {
            get { return _pmSpeed; }
            set { SetProperty(ref _pmSpeed, value); }
        }

        private string _pmAcc = "0.3";
        public string PmAcc
        {
            get { return _pmAcc; }
            set { SetProperty(ref _pmAcc, value); }
        }

        private string _pmDcl = "0.3";
        public string PmDcl
        {
            get { return _pmDcl; }
            set { SetProperty(ref _pmDcl, value); }
        }
        // Fixture Coordinate
        private string _xAxisSave = "";
        public string XAxisSave
        {
            get { return _xAxisSave; }
            set { SetProperty(ref _xAxisSave, value); }
        }
        private string _xAxisFixture = "";
        public string XAxisFixture
        {
            get { return _xAxisFixture; }
            set { SetProperty(ref _xAxisFixture, value); }
        }

        private string _yAxisSave = "";
        public string YAxisSave
        {
            get { return _yAxisSave; }
            set { SetProperty(ref _yAxisSave, value); }
        }
        private string _yAxisFixture = "";
        public string YAxisFixture
        {
            get { return _yAxisFixture; }
            set { SetProperty(ref _yAxisFixture, value); }
        }
    }
    public class DIOAdam6052Module1 : BindableBase
    {
        // 192.168.1.39
        private bool _connected;
        public bool Connected
        {
            get { return _connected; }
            set { SetProperty(ref _connected, value); }
        }
        private string _ipAddress = "192.168.1.39";
        public string IpAddress
        {
            get { return _ipAddress; }
            set { SetProperty(ref _ipAddress, value); }
        }

        private bool[] _iOMapping = new bool[16];
        public bool[] IOMapping
        {
            get { return _iOMapping; }
            set { SetProperty(ref _iOMapping, value); }
        }

        public System.Timers.Timer Module1Timer;

        private HardwareConnectionState _connectionState = HardwareConnectionState.Offline;
        public HardwareConnectionState ConnectionState
        {
            get { return _connectionState; }
            set { SetProperty(ref _connectionState, value); }
        }
    }
    public class DIOAdam6052Module2 : BindableBase
    {
        // 192.168.1.79
        private bool _connected;
        public bool Connected
        {
            get { return _connected; }
            set { SetProperty(ref _connected, value); }
        }
        private string _ipAddress = "192.168.1.79";
        public string IpAddress
        {
            get { return _ipAddress; }
            set { SetProperty(ref _ipAddress, value); }
        }

        private bool[] _iOMapping = new bool[16];
        public bool[] IOMapping
        {
            get { return _iOMapping; }
            set { SetProperty(ref _iOMapping, value); }
        }

        public readonly System.Timers.Timer Module2Timer;
        private HardwareConnectionState _connectionState = HardwareConnectionState.Offline;
        public HardwareConnectionState ConnectionState
        {
            get { return _connectionState; }
            set { SetProperty(ref _connectionState, value); }
        }
    }
    public class LogInfor
    {
        public string DateTime { get; set; }

        private LogType _logTypeEnum;
        public LogType LogTypeEnum
        {
            private get
            {
                return _logTypeEnum;
            }
            set
            {
                _logTypeEnum = value;

                MemberInfo[] memberEnums = typeof(LogType).GetMembers();
                foreach (MemberInfo memberEnum in memberEnums)
                {
                    if (memberEnum.Name.Equals(_logTypeEnum.ToString()))
                    {
                        var descriptionAttribute = memberEnum.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
                        strLogType = descriptionAttribute.Description;
                    }

                }
            }
        }
        public int selectedItem { get; set; }
        public string strLogType { get; private set; }
        public string strContent { get; set; }

        public LogInfor(LogType logType, string content)
        {
            DateTime = System.DateTime.Now.ToString("dd/MM hh:mm:ss tt");
            LogTypeEnum = logType;
            strContent = content;
        }
    }
    public class LogRspPrecitec
    {
        public string DateTime { get; set; }

        private LogType _logTypeEnum;
        public LogType LogTypeEnum
        {
            private get
            {
                return _logTypeEnum;
            }
            set
            {
                _logTypeEnum = value;

                MemberInfo[] memberEnums = typeof(LogType).GetMembers();
                foreach (MemberInfo memberEnum in memberEnums)
                {
                    if (memberEnum.Name.Equals(_logTypeEnum.ToString()))
                    {
                        var descriptionAttribute = memberEnum.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
                        strLogType = descriptionAttribute.Description;
                    }

                }
            }
        }
        public int selectedItem { get; set; }
        public string strLogType { get; private set; }
        public string strContent { get; set; }

        public LogRspPrecitec(LogType logType, string content)
        {
            DateTime = System.DateTime.Now.ToString("dd/MM hh:mm:ss tt");
            LogTypeEnum = logType;
            strContent = content;
        }
    }
    public class MachineRunStatus : BindableBase
    {
        private TestModeEnum _testMode = TestModeEnum.PRODUCTION;
        public TestModeEnum TestMode
        {
            get { return _testMode; }
            set { SetProperty(ref _testMode, value); }
        }
        private MachineModeEnum _machineMode = MachineModeEnum.MANUAL;
        public MachineModeEnum MachineMode
        {
            get { return _machineMode; }
            set { SetProperty(ref _machineMode, value); }
        }
        private MachineStateEnum _machineState = MachineStateEnum.None;
        public MachineStateEnum MachineState
        {
            get { return _machineState; }
            set { SetProperty(ref _machineState, value); }
        }
        private MachineInitStateEnum _machineInitState = MachineInitStateEnum.None;
        public MachineInitStateEnum MachineInitState
        {
            get { return _machineInitState; }
            set { SetProperty(ref _machineInitState, value); }
        }
    }
    public class RecipeConfig : BindableBase
    {
        private string _recipeName = "";
        public string RecipeName
        {
            get { return _recipeName; }
            set { SetProperty(ref _recipeName, value); }
        }

        private string _xOriginalPosition = "";
        public string XOriginalPosition
        {
            get { return _xOriginalPosition; }
            set { SetProperty(ref _xOriginalPosition, value); }
        }

        private string _yOriginalPosition = "";
        public string YOriginalPosition
        {
            get { return _yOriginalPosition; }
            set { SetProperty(ref _yOriginalPosition, value); }
        }

        private string _dXPosition = "";
        public string DXPosition
        {
            get { return _dXPosition; }
            set { SetProperty(ref _dXPosition, value); }
        }

        private string _dYPosition = "";
        public string DYPosition
        {
            get { return _dYPosition; }
            set { SetProperty(ref _dYPosition, value); }
        }

        private string _rXPosition = "";
        public string RXPosition
        {
            get { return _rXPosition; }
            set { SetProperty(ref _rXPosition, value); }
        }

        private string _rYPosition = "";
        public string RYPosition
        {
            get { return _rYPosition; }
            set { SetProperty(ref _rYPosition, value); }
        }

        private string _speedAxisX = "";
        public string SpeedAxisX
        {
            get { return _speedAxisX; }
            set { SetProperty(ref _speedAxisX, value); }
        }

        private string _speedAxisY = "";
        public string SpeedAxisY
        {
            get { return _speedAxisY; }
            set { SetProperty(ref _speedAxisY, value); }
        }

        private bool _fssSensorSelect = false;
        public bool FssSensorSelect
        {
            get { return _fssSensorSelect; }
            set { SetProperty(ref _fssSensorSelect, value); }
        }

        private bool _singlePointSensorSelect = false;
        public bool SinglePointSensorSelect
        {
            get { return _singlePointSensorSelect; }
            set { SetProperty(ref _singlePointSensorSelect, value); }
        }

        private bool _ocaThichknessSelect = false;
        public bool OCAThichknessSelect
        {
            get { return _ocaThichknessSelect; }
            set { SetProperty(ref _ocaThichknessSelect, value); }
        }

        private bool _polarizerThichknessSelect = false;
        public bool PolarizerThichknessSelect
        {
            get { return _polarizerThichknessSelect; }
            set { SetProperty(ref _polarizerThichknessSelect, value); }
        }
        private bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set { SetProperty(ref _selected, value); }
        }
        private ObservableCollection<RecipeItems> _recipeDetail = new ObservableCollection<RecipeItems>();
        public ObservableCollection<RecipeItems> RecipeDetail
        {
            get { return _recipeDetail; }
            set { SetProperty(ref _recipeDetail, value); }
        }
        // DWD left: Detection Window Definition Left side
        private string _dWDLeft = "";
        public string DWDLeft
        {
            get { return _dWDLeft; }
            set { SetProperty(ref _dWDLeft, value); }
        }
        // DWD left: Detection Window Definition Right side
        private string _dWDRight = "";
        public string DWDRight
        {
            get { return _dWDRight; }
            set { SetProperty(ref _dWDRight, value); }
        }
        // QTH: Quality Threshold
        private string _qTH = "";
        public string QTH
        {
            get { return _qTH; }
            set { SetProperty(ref _qTH, value); }
        }
        #region Multipoint Properties
        private string _multiPointT1X;
        public string MultiPointT1X
        {
            get { return _multiPointT1X; }
            set { SetProperty(ref _multiPointT1X, value); }
        }
        private string _multiPointT1Y;
        public string MultiPointT1Y
        {
            get { return _multiPointT1Y; }
            set { SetProperty(ref _multiPointT1Y, value); }
        }
        private string _multiPointT2X;
        public string MultiPointT2X
        {
            get { return _multiPointT2X; }
            set { SetProperty(ref _multiPointT2X, value); }
        }
        private string _multiPointT2Y;
        public string MultiPointT2Y
        {
            get { return _multiPointT2Y; }
            set { SetProperty(ref _multiPointT2Y, value); }
        }
        private string _multiPointT3X;
        public string MultiPointT3X
        {
            get { return _multiPointT3X; }
            set { SetProperty(ref _multiPointT3X, value); }
        }
        private string _multiPointT3Y;
        public string MultiPointT3Y
        {
            get { return _multiPointT3Y; }
            set { SetProperty(ref _multiPointT3Y, value); }
        }
        private string _multiPointT4X;
        public string MultiPointT4X
        {
            get { return _multiPointT4X; }
            set { SetProperty(ref _multiPointT4X, value); }
        }
        private string _multiPointT4Y;
        public string MultiPointT4Y
        {
            get { return _multiPointT4Y; }
            set { SetProperty(ref _multiPointT4Y, value); }
        }
        private string _multiPointT5X;
        public string MultiPointT5X
        {
            get { return _multiPointT5X; }
            set { SetProperty(ref _multiPointT5X, value); }
        }
        private string _multiPointT5Y;
        public string MultiPointT5Y
        {
            get { return _multiPointT5Y; }
            set { SetProperty(ref _multiPointT5Y, value); }
        }
        private string _multiPointT6X;
        public string MultiPointT6X
        {
            get { return _multiPointT6X; }
            set { SetProperty(ref _multiPointT6X, value); }
        }
        private string _multiPointT6Y;
        public string MultiPointT6Y
        {
            get { return _multiPointT6Y; }
            set { SetProperty(ref _multiPointT6Y, value); }
        }
        private string _multiPointT7X;
        public string MultiPointT7X
        {
            get { return _multiPointT7X; }
            set { SetProperty(ref _multiPointT7X, value); }
        }
        private string _multiPointT7Y;
        public string MultiPointT7Y
        {
            get { return _multiPointT7Y; }
            set { SetProperty(ref _multiPointT7Y, value); }
        }
        private string _multiPointT8X;
        public string MultiPointT8X
        {
            get { return _multiPointT8X; }
            set { SetProperty(ref _multiPointT8X, value); }
        }
        private string _multiPointT8Y;
        public string MultiPointT8Y
        {
            get { return _multiPointT8Y; }
            set { SetProperty(ref _multiPointT8Y, value); }
        }
        private string _multiPointT9X;
        public string MultiPointT9X
        {
            get { return _multiPointT9X; }
            set { SetProperty(ref _multiPointT9X, value); }
        }
        private string _multiPointT9Y;
        public string MultiPointT9Y
        {
            get { return _multiPointT9Y; }
            set { SetProperty(ref _multiPointT9Y, value); }
        }
        private string _multiPointT10X;
        public string MultiPointT10X
        {
            get { return _multiPointT10X; }
            set { SetProperty(ref _multiPointT10X, value); }
        }
        private string _multiPointT10Y;
        public string MultiPointT10Y
        {
            get { return _multiPointT10Y; }
            set { SetProperty(ref _multiPointT10Y, value); }
        }
        private string _multiPointT11X;
        public string MultiPointT11X
        {
            get { return _multiPointT11X; }
            set { SetProperty(ref _multiPointT11X, value); }
        }
        private string _multiPointT11Y;
        public string MultiPointT11Y
        {
            get { return _multiPointT11Y; }
            set { SetProperty(ref _multiPointT11Y, value); }
        }
        private string _multiPointT12X;
        public string MultiPointT12X
        {
            get { return _multiPointT12X; }
            set { SetProperty(ref _multiPointT12X, value); }
        }
        private string _multiPointT12Y;
        public string MultiPointT12Y
        {
            get { return _multiPointT12Y; }
            set { SetProperty(ref _multiPointT12Y, value); }
        }
        private string _multiPointT13X;
        public string MultiPointT13X
        {
            get { return _multiPointT13X; }
            set { SetProperty(ref _multiPointT13X, value); }
        }
        private string _multiPointT13Y;
        public string MultiPointT13Y
        {
            get { return _multiPointT13Y; }
            set { SetProperty(ref _multiPointT13Y, value); }
        }
        private string _multiPointT14X;
        public string MultiPointT14X
        {
            get { return _multiPointT14X; }
            set { SetProperty(ref _multiPointT14X, value); }
        }
        private string _multiPointT14Y;
        public string MultiPointT14Y
        {
            get { return _multiPointT14Y; }
            set { SetProperty(ref _multiPointT14Y, value); }
        }
        private string _multiPointT15X;
        public string MultiPointT15X
        {
            get { return _multiPointT15X; }
            set { SetProperty(ref _multiPointT15X, value); }
        }
        private string _multiPointT15Y;
        public string MultiPointT15Y
        {
            get { return _multiPointT15Y; }
            set { SetProperty(ref _multiPointT15Y, value); }
        }
        private string _multiPointT16X;
        public string MultiPointT16X
        {
            get { return _multiPointT16X; }
            set { SetProperty(ref _multiPointT16X, value); }
        }

        private string _multiPointT16Y;
        public string MultiPointT16Y
        {
            get { return _multiPointT16Y; }
            set { SetProperty(ref _multiPointT16Y, value); }
        }
        // Row x Column 
        private string _numberOfRow1D;
        public string NumberOfRow1D
        {
            get { return _numberOfRow1D; }
            set { SetProperty(ref _numberOfRow1D, value); }
        }
        private string _numberOfColumn1D;
        public string NumberOfColumn1D
        {
            get { return _numberOfColumn1D; }
            set { SetProperty(ref _numberOfColumn1D, value); }
        }
        #endregion

    }
    public class PrecitecControl : BindableBase
    {
        private string _sensorType = "";
        public string SensorType
        {
            get { return _sensorType; }
            set { SetProperty(ref _sensorType, value); }
        }

        private BitmapImage _heatMap = null;
        public BitmapImage HeatMap
        {
            get { return _heatMap; }
            set { SetProperty(ref _heatMap, value); }
        }

        private ObservableCollection<string> _plotSignal = new ObservableCollection<string>();
        public ObservableCollection<string> PlotSignal
        {
            get { return _plotSignal; }
            set { SetProperty(ref _plotSignal, value); }
        }
        private bool _enableDownloadSpectrum;
        public bool EnableDownloadSpectrum
        {
            get { return _enableDownloadSpectrum; }
            set { SetProperty(ref _enableDownloadSpectrum, value); }
        }

        private bool _connected;
        public bool Connected
        {
            get { return _connected; }
            set { SetProperty(ref _connected, value); }
        }

        private bool _scanDone;
        public bool ScanDone
        {
            get { return _scanDone; }
            set { SetProperty(ref _scanDone, value); }
        }

        private bool _oneDSelected;
        public bool OneDSelected
        {
            get { return _oneDSelected; }
            set { SetProperty(ref _oneDSelected, value); }
        }
        private bool _oneDMPSelected;
        public bool OneDMPSelected
        {
            get { return _oneDMPSelected; }
            set { SetProperty(ref _oneDMPSelected, value); }
        }

        private bool _fssSelected;
        public bool FssSelected
        {
            get { return _fssSelected; }
            set { SetProperty(ref _fssSelected, value); }
        }

        private string _oneDStartPos = "";
        public string OneDStartPos
        {
            get { return _oneDStartPos; }
            set { SetProperty(ref _oneDStartPos, value); }
        }

        private string _oneDStopPos = "";
        public string OneDStopPos
        {
            get { return _oneDStopPos; }
            set { SetProperty(ref _oneDStopPos, value); }
        }

        private string _oneDInterval = "";
        public string OneDInterval
        {
            get { return _oneDInterval; }
            set { SetProperty(ref _oneDInterval, value); }
        }

        private string _oneDSignalOutput = "65,66,256,264,272";
        public string OneDSignalOutput
        {
            get { return _oneDSignalOutput; }
            set { SetProperty(ref _oneDSignalOutput, value); }
        }

        private string _oneDScanLineNo = "";
        public string OneDScanLineNo
        {
            get { return _oneDScanLineNo; }
            set { SetProperty(ref _oneDScanLineNo, value); }
        }
        private string _oneDSampleCountPerLine = "";
        public string OneDSampleCountPerLine
        {
            get { return _oneDSampleCountPerLine; }
            set { SetProperty(ref _oneDSampleCountPerLine, value); }
        }
        private string _oneDValueEncX = "";
        public string OneDValueEncX
        {
            get { return _oneDValueEncX; }
            set { SetProperty(ref _oneDValueEncX, value); }
        }
        private string _oneDValueEncY = "";
        public string OneDValueEncY
        {
            get { return _oneDValueEncY; }
            set { SetProperty(ref _oneDValueEncY, value); }
        }
        private short[] _dataPoints = new short[1000];
        public short[] DataPoints
        {
            get { return _dataPoints; }
            set { SetProperty(ref _dataPoints, value); }
        }
        private bool _specTrumRaw = true;
        public bool SpecTrumRaw
        {
            get { return _specTrumRaw; }
            set 
            {
                SetProperty(ref _specTrumRaw, value);
                if (value)
                {
                    SpecTrumFt = false;
                }
            }
        }
        private bool _specTrumFt;
        public bool SpecTrumFt
        {
            get { return _specTrumFt; }
            set 
            { 
                SetProperty(ref _specTrumFt, value);
                if (value)
                {
                    SpecTrumRaw = false;
                }
            }
        }
    }
}
