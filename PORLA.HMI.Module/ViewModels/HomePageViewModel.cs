using PORLA.HMI.Module.Model;
using PORLA.HMI.Service;
using Org.BouncyCastle.Asn1.Mozilla;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.UI.WebControls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows;
using Prism.Services.Dialogs;
using System.Security.AccessControl;
using System.Windows.Forms;
using System.ComponentModel;
using log4net;
using System.Windows.Media.Imaging;
using PORLA.HMI.Module.Events;
using Prism.Events;
using Google.Protobuf.WellKnownTypes;
using POLAR.CompositeAppCommand;
using POLAR.EventAggregator;
using LiveCharts.Wpf.Charts.Base;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts.Geared;
using TestStack.White.UIItems.TabItems;
using PORLA.HMI.Service.Enums;
using System.Threading.Tasks;
using System.Threading;
using POLAR.IAIMotionControl;
using POLAR.PrecitecControl;
using PORLA.HMI.Module.DataService.DataHandle;
using POLAR.ModelAggregator.Alarm;
using POLAR.ModelAggregator.TestReport;
using POLAR.DIOADAM6052;
using System.Linq.Expressions;
using System.Globalization;
using System.Text;
using System.Windows.Media;

namespace PORLA.HMI.Module.ViewModels
{
    public class HomePageViewModel : BindableBase
    {
        #region Property
        private static readonly log4net.ILog logger = 
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IAppService _appService;
        public IAppService AppService
        {
            get { return _appService; }
            set { SetProperty(ref _appService, value); }
        }

        IEventAggregator _eventAggregator;

        private ICompositeAppCommand _appCommand;
        public ICompositeAppCommand AppCommand
        {
            get { return _appCommand; }
            set { SetProperty(ref _appCommand, value); }
        }

        private readonly IDialogService _dialogService;

        private IIAIMotion _iaiMotion;
        public IIAIMotion IAIMotionControl
        {
            get { return _iaiMotion; }
            set { SetProperty(ref _iaiMotion, value); }
        }

        private IFSSAreaScan _fssControl;
        public IFSSAreaScan FssControl
        {
            get { return _fssControl; }
            set { SetProperty(ref _fssControl, value); }
        }

        private IDataHandler _dataHandler;
        public IDataHandler DataHandler
        {
            get { return _dataHandler; }
            set { SetProperty(ref _dataHandler, value); }
        }

        IAdam6052Module1 _adam6052Module1;

        private IOneDAreaScan _oneDControl;
        public IOneDAreaScan OneDControl
        {
            get { return _oneDControl; }
            set { SetProperty(ref _oneDControl, value); }
        }

        private int _totalDut = 0;
        public int TotalDut
        {
            get { return _totalDut; }
            set { SetProperty(ref _totalDut, value); }
        }
        private bool _auto = false;
        public bool Auto
        {
            get { return _auto; }
            set
            {
                SetProperty(ref _auto, value);
            }
        }
        private object Lockdata = new object();
        public DelegateCommand AutoCmd { get; private set; }
        public DelegateCommand ManualCmd { get; private set; }
        public DelegateCommand InitCmd { get; private set; }
        public DelegateCommand StartCmd { get; private set; }
        public DelegateCommand StopCmd { get; private set; }
        public DelegateCommand ResetCmd { get; private set; }
        public DelegateCommand ClearCmd { get; private set; }
        public DelegateCommand ScanBarcodeCmd { get; private set; }

        private BitmapImage _fSSImageSignalId;
        public BitmapImage FSSImageSignalId
        {
            get { return _fSSImageSignalId; }
            set { SetProperty(ref _fSSImageSignalId, value); }
        }

        public DelegateCommand ConnectFssCmd { get; private set; }
        public DelegateCommand TestMultiPointCmd { get; private set; }

        private string _selectedSignalId = "";
        public string SelectedSignalId
        {
            get { return _selectedSignalId; }
            set 
            { 
                SetProperty(ref _selectedSignalId, value);
                if (!string.IsNullOrEmpty(value))
                {
                    _eventAggregator.GetEvent<PrecitecPlotSignalEvent>().Publish(value);
                }
            }
        }

        private SeriesCollection _dataPoints = new SeriesCollection
        {
            new LineSeries
            {
                Title = "Data",
                Values = new GearedValues<short>(),
                Stroke = Brushes.Yellow,
                StrokeThickness = 1,
            }
        };
        public SeriesCollection DataPoints
        {
            get { return _dataPoints; }
            set 
            {
                SetProperty(ref _dataPoints, value); 
            }
        }

        public bool SelectedRecipe { get; private set; }
        public bool FSSRecipeSelected { get; private set; }

        private bool _isStartEna = true;
        public bool IsStartEna
        {
            get { return _isStartEna; }
            set { SetProperty(ref _isStartEna, value); }
        }

        private bool _canStart = true;
        public bool CanStart
        {
            get { return _canStart; }
            set { SetProperty(ref _canStart, value); }
        }
        public bool OneDRecipeSelected { get; private set; }
        private CancellationTokenSource cancellationTokenSource;
        private bool _isStart;
        public bool IsStart
        {
            get { return _isStart; }
            set { SetProperty(ref _isStart, value); }
        }
        private bool _isStop;
        public bool IsStop
        {
            get { return _isStop; }
            set { SetProperty(ref _isStop, value); }
        }
        private int _precitecTabSelect;
        public int PrecitecTabSelect
        {
            get { return _precitecTabSelect; }
            set { SetProperty(ref _precitecTabSelect, value); }
        }
        private Visibility _tab1D;
        public Visibility Tab1D
        {
            get { return _tab1D; }
            set { SetProperty(ref _tab1D, value); }
        }
        private Visibility _tabFss;
        public Visibility TabFss
        {
            get { return _tabFss; }
            set { SetProperty(ref _tabFss, value); }
        }
        private TestReportItems _reportItems = new TestReportItems();
        public TestReportItems ReportItems
        {
            get { return _reportItems; }
            set { SetProperty(ref _reportItems, value); }
        }
        
        private string[] _paraPaternMove = null;
        public string[] ParaPaternMove
        {
            get { return _paraPaternMove; }
            set { SetProperty(ref _paraPaternMove, value); }
        }

        private bool _isDownloadSpectrum;
        public bool IsDownloadSpectrum
        {
            get { return _isDownloadSpectrum; }
            set 
            {
                SetProperty(ref _isDownloadSpectrum, value);
                if (value)
                {
                    if (!IsRunning)
                    {
                        //RequestDownloadSpectrum(true);
                    }
                    else
                    {
                        //RequestDownloadSpectrum(false);
                    }
                }
                else
                {
                    //RequestDownloadSpectrum(false);
                }
            }
        }

        private void RequestDownloadSpectrum(bool state)
        {
            if (state)
            {
                if (AppService.PrecitecService.FssSelected)
                {
                    _fssControl.EnaTimerDownloadSpectrum(true);
                    dispatcherTimer.Start();
                }
                if (AppService.PrecitecService.OneDSelected || AppService.PrecitecService.OneDMPSelected)
                {
                    _oneDControl.EnaTimerDownloadSpectrum(true);
                    //dispatcherTimer.Start();
                }
            }
            else
            {
                if (AppService.PrecitecService.FssSelected)
                {
                    _fssControl.EnaTimerDownloadSpectrum(false);
                    if (dispatcherTimer != null)
                    {
                        dispatcherTimer.Stop();
                    }
                    
                }
                if (AppService.PrecitecService.OneDSelected || AppService.PrecitecService.OneDMPSelected)
                {
                    _oneDControl.EnaTimerDownloadSpectrum(false);
                    //dispatcherTimer.Stop();
                }
            }
        }

        private bool _isRunning;
        public bool IsRunning
        {
            get { return _isRunning; }
            set { SetProperty(ref _isRunning, value); }
        }
        private DispatcherTimer dispatcherTimer;
        #endregion
        #region Constructors
        public HomePageViewModel(IAppService appService, IIAIMotion iAIMotion, IAdam6052Module1 adam6052Module1,
            IEventAggregator eventAggregator, ICompositeAppCommand compositeAppCommand, IDialogService dialogService,
            IFSSAreaScan fSSAreaScan, IDataHandler dataHandler, IOneDAreaScan oneDAreaScan)
        {
            _appService = appService;
            _eventAggregator = eventAggregator;
            _appCommand = compositeAppCommand;
            _dialogService = dialogService;
            _dataHandler = dataHandler;
            _adam6052Module1 = adam6052Module1;

            AutoCmd = new DelegateCommand(() => ExAuto());
            ManualCmd = new DelegateCommand(() => ExManual());
            InitCmd = new DelegateCommand(() => ExInitialize());
            //StartCmd = new DelegateCommand(() => ExStart());
            StartCmd = new DelegateCommand(ExStart).ObservesCanExecute(() => CanStart);
            StopCmd = new DelegateCommand(() => ExStop());
            ResetCmd = new DelegateCommand(() => ExReset());
            ClearCmd = new DelegateCommand(() => ExClear());
            ConnectFssCmd = new DelegateCommand(ExConnectFssCmd);
            ScanBarcodeCmd = new DelegateCommand(ExScanBarcodeCmd);
            TestMultiPointCmd = new DelegateCommand(ExTestMultiPointCmd);

            _eventAggregator.GetEvent<PrecitecPlotSignalEvent>().Subscribe(OnPlotSignalId, ThreadOption.UIThread);
            _eventAggregator.GetEvent<UpdateSignalIdEvent>().Subscribe(OnUpdateSignalId);
            _eventAggregator.GetEvent<SpectrumDataEvent>().Subscribe(OnSpectrumData, ThreadOption.UIThread);
            _eventAggregator.GetEvent<CloseAppEvent>().Subscribe(OnCloseConnectionDevices);
            _eventAggregator.GetEvent<FssRecipeSelectedEvent>().Subscribe(OnParsingForFssScriptFile);
            _eventAggregator.GetEvent<OneDRecipeSelectedEvent>().Subscribe(OnOneDDownloadParas);
            _eventAggregator.GetEvent<SpectrumDowloadEvent>().Subscribe(OnShowOnChart, ThreadOption.UIThread);
            _eventAggregator.GetEvent<RaiseProccessDialogEvent>().Subscribe(OnRaiseProccessDialog);
            _eventAggregator.GetEvent<RaiseProcessScanEvent>().Subscribe(OnRaiseProcessScan, ThreadOption.UIThread);
            _eventAggregator.GetEvent<StartStopEvent>().Subscribe(OnStartStopEvent);
            _eventAggregator.GetEvent<StopMachineEvent>().Subscribe(OnStopMachine, ThreadOption.BackgroundThread);

            _fssControl = fSSAreaScan;
            _oneDControl = oneDAreaScan;
            _iaiMotion = iAIMotion;
            IsDownloadSpectrum = false;
            IsRunning = false;
            IntializeDevice();
            PrecitecVisiblity();
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(500);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
        }

        private void OnOneDDownloadParas(bool obj)
        {
            StringBuilder _dwd = new StringBuilder();
            StringBuilder _qth = new StringBuilder();
            _dwd.Append("DWD ");
            _dwd.Append(AppService.RecipeService.DWDLeft + " " + AppService.RecipeService.DWDRight);
            _qth.Append("QTH ");
            _qth.Append(AppService.RecipeService.QTH);
            _oneDControl.SendCommand(_dwd.ToString());
            _oneDControl.SendCommand(_qth.ToString());
        }

        private void OnStopMachine(bool obj)
        {
            if (obj)
            {
                StopMachine();
            }
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            Task.Run(async() => 
            {
                Action _plotChart = () =>
                {
                    OnShowOnChart(true);
                };
                await System.Windows.Application.Current.Dispatcher.BeginInvoke(_plotChart);
            });
            
        }

        private void OnStartStopEvent(string obj)
        {
            if (obj == DIODescriptions.Start)
            {
                Action _startAction = () =>
                {
                    ExStart();
                };
                System.Windows.Application.Current.Dispatcher.BeginInvoke(_startAction);
            }
            if (obj == DIODescriptions.Stop)
            {
                Action _stopAction = () =>
                {
                    ExStop();
                };
                System.Windows.Application.Current.Dispatcher.BeginInvoke(_stopAction);
            }
        }

        private async void ExTestMultiPointCmd()
        {
            MotionStatus _motionStatus = MotionStatus.NONE;
            cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;
            // First Point
            AppService.IAIMotion.AxisXValue = "100";
            AppService.IAIMotion.AxisYValue = "100";
            _motionStatus = await _iaiMotion.AbsoluteMoveWithCheckFinish("ABS", cancellationToken);
            Thread.Sleep(500);
            if (_motionStatus == MotionStatus.EXCEPTION_STATUS)
            {
                AppService.MachineStatus.MachineState = MachineStateEnum.STOP;
                IsStart = false;
                IsStop = true;
                System.Windows.MessageBox.Show("Abort Scan, Please check alarm -> reset -> initialize!", "Alarm",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            AppCommand.cmdOneDTriggerEvent.Execute("test1");
            Thread.Sleep(1000);
            // Secode Point
            AppService.IAIMotion.AxisXValue = "120";
            AppService.IAIMotion.AxisYValue = "105";
            _motionStatus = await _iaiMotion.AbsoluteMoveWithCheckFinish("ABS", cancellationToken);
            Thread.Sleep(500);
            if (_motionStatus == MotionStatus.EXCEPTION_STATUS)
            {
                AppService.MachineStatus.MachineState = MachineStateEnum.STOP;
                IsStart = false;
                IsStop = true;
                System.Windows.MessageBox.Show("Abort Scan, Please check alarm -> reset -> initialize!", "Alarm",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            AppCommand.cmdOneDTriggerEvent.Execute("test2");
            Thread.Sleep(1000);
            // Third Point
            AppService.IAIMotion.AxisXValue = "130";
            AppService.IAIMotion.AxisYValue = "110";
            _motionStatus = await _iaiMotion.AbsoluteMoveWithCheckFinish("ABS", cancellationToken);
            Thread.Sleep(500);
            if (_motionStatus == MotionStatus.EXCEPTION_STATUS)
            {
                AppService.MachineStatus.MachineState = MachineStateEnum.STOP;
                IsStart = false;
                IsStop = true;
                System.Windows.MessageBox.Show("Abort Scan, Please check alarm -> reset -> initialize!", "Alarm",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            AppCommand.cmdOneDTriggerEvent.Execute("test3");
            Thread.Sleep(1000);
            AppCommand.cmdOneDTriggerEvent.Execute("log");
        }

        private void OnRaiseProcessScan(string sensorType)
        {
            if (sensorType == "FSS")
            {
                OnProcessingScanning();
            }
            if (sensorType == "1D")
            {
                OneDOnProcessingScanning();
            }
            if (sensorType == "1DMP")
            {
                OneDMPOnProcessingScanning();
            }
            
        }

        private void OnRaiseProccessDialog(bool obj)
        {
            var parameter = "";
            if (AppService.PrecitecService.FssSelected)
            {
                parameter = "FSS";
            }
            if (AppService.PrecitecService.OneDSelected)
            {
                parameter = "1D";
            }
            if (AppService.PrecitecService.OneDMPSelected)
            {
                parameter = "1DMP";
            }
            _dialogService.ShowDialog("ProccessScanDialog", new DialogParameters($"parameter={parameter}"), result => { });
        }

        private void ExScanBarcodeCmd()
        {
            var parameters = new DialogParameters("ScanDutBarcodeDialog");
            _dialogService.ShowDialog("ScanDutBarcodeDialog", parameters, result => { });
        }

        private void OnShowOnChart(bool obj)
        {
            Task.Run(async() => 
            {
                Action updateUIPlot = () =>
                {
                    DataPoints[0].Values.Clear();
                    foreach (var item in AppService.PrecitecService.DataPoints)
                    {
                        DataPoints[0].Values.Add(item);
                    }
                };
                await System.Windows.Application.Current.Dispatcher.InvokeAsync(updateUIPlot);
            });
            
        }

        private void OnParsingForFssScriptFile(bool obj)
        {
            string _scanProgram = "";
            StringBuilder _dwd = new StringBuilder();
            StringBuilder _qth = new StringBuilder();
            _dwd.Append("DWD ");
            _dwd.Append(AppService.RecipeService.DWDLeft + " " + AppService.RecipeService.DWDRight);
            _qth.Append("QTH ");
            _qth.Append(AppService.RecipeService.QTH);
            double Dx = double.Parse(AppService.IAIMotion.RcdXDistance + ".", CultureInfo.InvariantCulture);
            double Dy = double.Parse(AppService.IAIMotion.RcdYDistance + ".", CultureInfo.InvariantCulture);
            double nCols = double.Parse(AppService.IAIMotion.RcPitchX);
            double nRows = double.Parse(AppService.IAIMotion.RcPitchY);
            _scanProgram = _fssControl.GetScriptContent();
            _scanProgram = _fssControl.UpdateScanScript("area", _scanProgram, Dx, Dy);
            _scanProgram = _fssControl.UpdateScanScript("resolution", _scanProgram, nCols, nRows);
            _fssControl.WriteUpdatedScriptToFile(_scanProgram);
            AppCommand.cmdFssSendCmd.Execute(_dwd.ToString());
            AppCommand.cmdFssSendCmd.Execute(_qth.ToString());
        }

        private void PrecitecVisiblity()
        {
            if (AppService.PrecitecService.FssSelected)
            {
                PrecitecTabSelect = 0; // 0: Tab for 1D sensor, 1: Tab for FSS
                Tab1D = Visibility.Visible;
                TabFss = Visibility.Visible;
            }
            if (AppService.PrecitecService.OneDSelected || AppService.PrecitecService.OneDMPSelected)
            {
                PrecitecTabSelect = 1; // 0: Tab for 1D sensor, 1: Tab for FSS
                Tab1D = Visibility.Visible;
                TabFss = Visibility.Collapsed;
            }
        }

        private void ExConnectFssCmd()
        {
            _fssControl.InitializeFssAsync();
        }

        private void OnCloseConnectionDevices(bool obj)
        {
            if(_iaiMotion.Disconnect())
            {
                logger.Info("Disconnect IAI motion controller");
            }
        }

        private void IntializeDevice()
        {
            _iaiMotion.Initialize(AppService.IAIMotion.IPIAI, AppService.IAIMotion.PortAuto);
            if (AppService.IAIMotion.Connected)
            {
                AppCommand.cmdIAIServoOnOff.Execute("SVON");
                logger.Info("SVON");
                Logging(new LogInfor(LogType.Motion, $"Servo On!"));
            }
            else
            {
                logger.Info("Can not initialize IAI motion");
                Logging(new LogInfor(LogType.Motion, $"Can not initialize IAI motion"));
            }

        }

        private void OnSpectrumData(short[] specData)
        {
            for (int i = 0; i < specData.Length; i++)
                DataPoints = new SeriesCollection
                {
                    new LineSeries
                    {
                        Title = "Series 1",
                        Values = new ChartValues<ObservableValue>
                        {
                            new ObservableValue(i)
                        }
                    }
                };
            // chart1.Series[0].Points[i].YValues[0] = SpecData[i];
        }

        private void OnUpdateSignalId(string signalId)
        {
            AppService.PrecitecService.PlotSignal.Add(signalId);
        }

        private void OnPlotSignalId(string signalId)
        {
            AppCommand.cmdFssPlotSignalId.Execute(signalId);
        }
        #endregion
        #region Methods
        private void ExAuto()
        {
            if (System.Windows.MessageBox.Show("Do you want to change the mode to Auto?", 
                "Information", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK) 
            {
                Auto = true;
                AppService.MachineStatus.MachineMode = Service.Enums.MachineModeEnum.AUTO;
            }
           
        }
        private void ExManual()
        {
            if (System.Windows.MessageBox.Show("Do you want to change the mode to Manual?", "Information", MessageBoxButton.OKCancel, MessageBoxImage.Information)== MessageBoxResult.OK) 
            {
                Auto = false;
                AppService.MachineStatus.MachineMode = Service.Enums.MachineModeEnum.MANUAL;
                AppService.MachineStatus.MachineState = MachineStateEnum.NOTREADY;
                AppService.MachineStatus.MachineInitState = MachineInitStateEnum.None;
            }
        }
        private async void ExInitialize()
        {
            MotionStatus _motionStatus = MotionStatus.NONE;
            cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;
            try
            {
                if (AppService.MachineStatus.TestMode == TestModeEnum.PRODUCTION
                //& AppService.MachineStatus.MachineState == MachineStateEnum.READY
                  & AppService.MachineStatus.MachineMode == MachineModeEnum.AUTO
                  & !AppService.DIOModule2.IOMapping[DIODescriptions.In_ServiceKey])
                {
                    if (!AppService.RecipeService.Selected)
                    {
                        System.Windows.MessageBox.Show("Please Download Recipe First!", "Warning",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    AppService.MachineStatus.MachineInitState = MachineInitStateEnum.INITIALIZING;
                    // X Y axes move home : 0, 0
                    //AppCommand.cmdIAIAbsoluteMove.Execute("home");
                    
                    if (_adam6052Module1.CheckSafetyCondition())
                    {
                        IsStart = false;
                        IsStop = false;
                        AppCommand.cmdIAIServoOnOff.Execute("SVON");
                        Thread.Sleep(500);
                        _motionStatus = await _iaiMotion.AbsoluteMoveWithCheckFinish("home", cancellationToken);
                        Thread.Sleep(500);
                        await Task.Run(() =>
                        {
                            // Turn off all DO
                            AppCommand.cmdDOVaccumOnOff.Execute("VacOff");
                            Thread.Sleep(300);
                            AppCommand.cmdDOLightOnOff.Execute("LOff");
                            Thread.Sleep(300);
                            // Set ENC O for 1D
                            if (_motionStatus == MotionStatus.FINISH)
                            {
                                if (AppService.PrecitecService.OneDSelected || AppService.PrecitecService.OneDMPSelected)
                                {
                                    _oneDControl.SendCommand("ENC 0 0");
                                    _oneDControl.SendCommand("ENC 1 0");
                                }
                            }
                            
                        });
                    }
                    else
                    {
                        if (AppService.DIOModule2.IOMapping[DIODescriptions.In_ServiceKey])
                        {
                            Logging(new LogInfor(LogType.DIOModule2, $"Can not Initialize because: switched to service mode"));
                        }
                        AppService.MachineStatus.MachineInitState = MachineInitStateEnum.INITIALFAILED;
                        return;
                    }
                    AppService.MachineStatus.MachineState = MachineStateEnum.READY;
                    AppService.MachineStatus.MachineInitState = MachineInitStateEnum.INITIALIZED;
                    CanStart = true;
                }
                if (AppService.MachineStatus.TestMode == TestModeEnum.PRODUCTION
                  & AppService.MachineStatus.MachineMode == MachineModeEnum.AUTO
                  & AppService.DIOModule2.IOMapping[DIODescriptions.In_ServiceKey])
                {
                    Logging(new LogInfor(LogType.DIOModule2, $"Can not Initialize because in service mode"));
                    AppService.MachineStatus.MachineInitState = MachineInitStateEnum.INITIALFAILED;
                    System.Windows.MessageBox.Show("Can not initialize in service mode!", "Warning",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                if (AppService.MachineStatus.MachineMode == MachineModeEnum.MANUAL
                    || AppService.MachineStatus.MachineState == MachineStateEnum.ERROR)
                {
                    System.Windows.MessageBox.Show("Can not initialize in this mode!", "Warning",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                Logging(new LogInfor(LogType.Sequence, $"Can not Initialize because: {ex}"));
                logger.Error(ex);
            }
        }
        private async void ExStart()
        {
            
            AppService.MachineStatus.MachineState = MachineStateEnum.RUN;
            // 1. Reset Alarm after close door - > 2. Servo On.
            AppCommand.cmdIAIAlarmReset.Execute(null);
            Thread.Sleep(300);
            AppCommand.cmdIAIServoOnOff.Execute("SVON");
            Thread.Sleep(300);
            IsRunning = true;
            IsDownloadSpectrum = false;
            IsStart = true;
            IsStop = false;
            FSSRecipeSelected = true;
            IsStartEna = false;
            //AppService.DutBarcode = "";
            MotionStatus _motionStatus = MotionStatus.NONE;
            
            if (AppService.MachineStatus.TestMode == TestModeEnum.PRODUCTION
                & AppService.MachineStatus.MachineState == MachineStateEnum.RUN
                & AppService.MachineStatus.MachineMode == MachineModeEnum.AUTO
                & !AppService.DIOModule2.IOMapping[DIODescriptions.In_ServiceKey]
                & AppService.MachineStatus.MachineInitState == MachineInitStateEnum.INITIALIZED)
            {
                if (AppService.PrecitecService.SensorType == "FSS")
                {
                    cancellationTokenSource = new CancellationTokenSource();
                    CancellationToken cancellationToken = cancellationTokenSource.Token;
                    await FssSequence(_motionStatus, cancellationToken);
                }
                if (AppService.PrecitecService.SensorType == "1D")
                {
                    await OnDSequence(_motionStatus);
                }
                if (AppService.PrecitecService.OneDMPSelected)
                {
                    OneDMPSequence(_motionStatus);
                }       
                IsStartEna = true;
            }
        }
        private void OneDMPSequence(MotionStatus motionStatus)
        {
            if (_adam6052Module1.CheckSafetyCondition())
            {
                if (!string.IsNullOrEmpty(AppService.DutBarcode))
                {
                    if (System.Windows.MessageBox.Show("Are you sure currently Probe sensor", "Information",
                        MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        ;
                    }
                    else
                    {
                        AppService.MachineStatus.MachineState = MachineStateEnum.NOTREADY;
                        IsStartEna = true;
                        return;
                    }
                    _eventAggregator.GetEvent<RaiseProccessDialogEvent>().Publish(true);
                }
                else
                {
                    System.Windows.MessageBox.Show("Please Scan Barcode!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            else
            {
                AppService.MachineStatus.MachineState = MachineStateEnum.NOTREADY;
                return;
            }
        }
        private async Task MoveToPointAndTrigger(int i, CancellationToken cancellationToken)
        {
            MotionStatus motionStatus = MotionStatus.NONE;
            int ParameterID = 0;
            int t = i + 1;
            switch (t)
            {
                // T1
                case 1:
                    AppService.IAIMotion.AxisXValue = AppService.RecipeService.MultiPointT1X;
                    AppService.IAIMotion.AxisYValue = AppService.RecipeService.MultiPointT1Y;
                    motionStatus = await _iaiMotion.AbsoluteMoveWithCheckFinish("ABS", cancellationToken);
                    Thread.Sleep(500);
                    if (motionStatus == MotionStatus.EXCEPTION_STATUS)
                    {
                        AppService.MachineStatus.MachineState = MachineStateEnum.STOP;
                        IsStart = false;
                        IsStop = true;
                        System.Windows.MessageBox.Show("Abort Scan, Please check alarm -> reset -> initialize!", "Alarm",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    _oneDControl.SoftwareTriggerAndRecording(100);
                    Thread.Sleep(100);
                    ParameterID = i + 1;
                    _oneDControl.SoftwareTriggerAndRecording(ParameterID);
                    Thread.Sleep(100);
                    break;
                // T2
                case 2:
                    AppService.IAIMotion.AxisXValue = AppService.RecipeService.MultiPointT2X;
                    AppService.IAIMotion.AxisYValue = AppService.RecipeService.MultiPointT2Y;
                    motionStatus = await _iaiMotion.AbsoluteMoveWithCheckFinish("ABS", cancellationToken);
                    Thread.Sleep(100);
                    if (motionStatus == MotionStatus.EXCEPTION_STATUS)
                    {
                        AppService.MachineStatus.MachineState = MachineStateEnum.STOP;
                        IsStart = false;
                        IsStop = true;
                        System.Windows.MessageBox.Show("Abort Scan, Please check alarm -> reset -> initialize!", "Alarm",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    ParameterID = i + 1;
                    _oneDControl.SoftwareTriggerAndRecording(ParameterID);
                    Thread.Sleep(100);
                    break;
                case 3:
                    AppService.IAIMotion.AxisXValue = AppService.RecipeService.MultiPointT3X;
                    AppService.IAIMotion.AxisYValue = AppService.RecipeService.MultiPointT3Y;
                    motionStatus = await _iaiMotion.AbsoluteMoveWithCheckFinish("ABS", cancellationToken);
                    Thread.Sleep(500);
                    if (motionStatus == MotionStatus.EXCEPTION_STATUS)
                    {
                        AppService.MachineStatus.MachineState = MachineStateEnum.STOP;
                        IsStart = false;
                        IsStop = true;
                        System.Windows.MessageBox.Show("Abort Scan, Please check alarm -> reset -> initialize!", "Alarm",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    ParameterID = i + 1;
                    _oneDControl.SoftwareTriggerAndRecording(ParameterID);
                    Thread.Sleep(100);
                    break;
                case 4:
                    AppService.IAIMotion.AxisXValue = AppService.RecipeService.MultiPointT4X;
                    AppService.IAIMotion.AxisYValue = AppService.RecipeService.MultiPointT4Y;
                    motionStatus = await _iaiMotion.AbsoluteMoveWithCheckFinish("ABS", cancellationToken);
                    Thread.Sleep(500);
                    if (motionStatus == MotionStatus.EXCEPTION_STATUS)
                    {
                        AppService.MachineStatus.MachineState = MachineStateEnum.STOP;
                        IsStart = false;
                        IsStop = true;
                        System.Windows.MessageBox.Show("Abort Scan, Please check alarm -> reset -> initialize!", "Alarm",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    ParameterID = i + 1;
                    _oneDControl.SoftwareTriggerAndRecording(ParameterID);
                    Thread.Sleep(100);
                    break;
                case 5:
                    AppService.IAIMotion.AxisXValue = AppService.RecipeService.MultiPointT5X;
                    AppService.IAIMotion.AxisYValue = AppService.RecipeService.MultiPointT5Y;
                    motionStatus = await _iaiMotion.AbsoluteMoveWithCheckFinish("ABS", cancellationToken);
                    Thread.Sleep(500);
                    if (motionStatus == MotionStatus.EXCEPTION_STATUS)
                    {
                        AppService.MachineStatus.MachineState = MachineStateEnum.STOP;
                        IsStart = false;
                        IsStop = true;
                        System.Windows.MessageBox.Show("Abort Scan, Please check alarm -> reset -> initialize!", "Alarm",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    ParameterID = i + 1;
                    _oneDControl.SoftwareTriggerAndRecording(ParameterID);
                    Thread.Sleep(100);
                    break;
                case 6:
                    AppService.IAIMotion.AxisXValue = AppService.RecipeService.MultiPointT6X;
                    AppService.IAIMotion.AxisYValue = AppService.RecipeService.MultiPointT6Y;
                    motionStatus = await _iaiMotion.AbsoluteMoveWithCheckFinish("ABS", cancellationToken);
                    Thread.Sleep(500);
                    if (motionStatus == MotionStatus.EXCEPTION_STATUS)
                    {
                        AppService.MachineStatus.MachineState = MachineStateEnum.STOP;
                        IsStart = false;
                        IsStop = true;
                        System.Windows.MessageBox.Show("Abort Scan, Please check alarm -> reset -> initialize!", "Alarm",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    ParameterID = i + 1;
                    _oneDControl.SoftwareTriggerAndRecording(ParameterID);
                    Thread.Sleep(100);
                    break;
                case 7:
                    AppService.IAIMotion.AxisXValue = AppService.RecipeService.MultiPointT7X;
                    AppService.IAIMotion.AxisYValue = AppService.RecipeService.MultiPointT7Y;
                    motionStatus = await _iaiMotion.AbsoluteMoveWithCheckFinish("ABS", cancellationToken);
                    Thread.Sleep(500);
                    if (motionStatus == MotionStatus.EXCEPTION_STATUS)
                    {
                        AppService.MachineStatus.MachineState = MachineStateEnum.STOP;
                        IsStart = false;
                        IsStop = true;
                        System.Windows.MessageBox.Show("Abort Scan, Please check alarm -> reset -> initialize!", "Alarm",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    ParameterID = i + 1;
                    _oneDControl.SoftwareTriggerAndRecording(ParameterID);
                    Thread.Sleep(100);
                    break;
                case 8:
                    AppService.IAIMotion.AxisXValue = AppService.RecipeService.MultiPointT8X;
                    AppService.IAIMotion.AxisYValue = AppService.RecipeService.MultiPointT8Y;
                    motionStatus = await _iaiMotion.AbsoluteMoveWithCheckFinish("ABS", cancellationToken);
                    Thread.Sleep(500);
                    if (motionStatus == MotionStatus.EXCEPTION_STATUS)
                    {
                        AppService.MachineStatus.MachineState = MachineStateEnum.STOP;
                        IsStart = false;
                        IsStop = true;
                        System.Windows.MessageBox.Show("Abort Scan, Please check alarm -> reset -> initialize!", "Alarm",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    ParameterID = i + 1;
                    _oneDControl.SoftwareTriggerAndRecording(ParameterID);
                    Thread.Sleep(500);
                    break;
                case 9:
                    AppService.IAIMotion.AxisXValue = AppService.RecipeService.MultiPointT9X;
                    AppService.IAIMotion.AxisYValue = AppService.RecipeService.MultiPointT9Y;
                    motionStatus = await _iaiMotion.AbsoluteMoveWithCheckFinish("ABS", cancellationToken);
                    Thread.Sleep(500);
                    if (motionStatus == MotionStatus.EXCEPTION_STATUS)
                    {
                        AppService.MachineStatus.MachineState = MachineStateEnum.STOP;
                        IsStart = false;
                        IsStop = true;
                        System.Windows.MessageBox.Show("Abort Scan, Please check alarm -> reset -> initialize!", "Alarm",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    ParameterID = i + 1;
                    _oneDControl.SoftwareTriggerAndRecording(ParameterID);
                    Thread.Sleep(100);
                    break;
                case 10:
                    AppService.IAIMotion.AxisXValue = AppService.RecipeService.MultiPointT10X;
                    AppService.IAIMotion.AxisYValue = AppService.RecipeService.MultiPointT10Y;
                    motionStatus = await _iaiMotion.AbsoluteMoveWithCheckFinish("ABS", cancellationToken);
                    Thread.Sleep(500);
                    if (motionStatus == MotionStatus.EXCEPTION_STATUS)
                    {
                        AppService.MachineStatus.MachineState = MachineStateEnum.STOP;
                        IsStart = false;
                        IsStop = true;
                        System.Windows.MessageBox.Show("Abort Scan, Please check alarm -> reset -> initialize!", "Alarm",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    ParameterID = i + 1;
                    _oneDControl.SoftwareTriggerAndRecording(ParameterID);
                    Thread.Sleep(100);
                    break;
                case 11:
                    AppService.IAIMotion.AxisXValue = AppService.RecipeService.MultiPointT11X;
                    AppService.IAIMotion.AxisYValue = AppService.RecipeService.MultiPointT11Y;
                    motionStatus = await _iaiMotion.AbsoluteMoveWithCheckFinish("ABS", cancellationToken);
                    Thread.Sleep(500);
                    if (motionStatus == MotionStatus.EXCEPTION_STATUS)
                    {
                        AppService.MachineStatus.MachineState = MachineStateEnum.STOP;
                        IsStart = false;
                        IsStop = true;
                        System.Windows.MessageBox.Show("Abort Scan, Please check alarm -> reset -> initialize!", "Alarm",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    ParameterID = i + 1;
                    _oneDControl.SoftwareTriggerAndRecording(ParameterID);
                    Thread.Sleep(100);
                    break;
                case 12:
                    AppService.IAIMotion.AxisXValue = AppService.RecipeService.MultiPointT12X;
                    AppService.IAIMotion.AxisYValue = AppService.RecipeService.MultiPointT12Y;
                    motionStatus = await _iaiMotion.AbsoluteMoveWithCheckFinish("ABS", cancellationToken);
                    Thread.Sleep(500);
                    if (motionStatus == MotionStatus.EXCEPTION_STATUS)
                    {
                        AppService.MachineStatus.MachineState = MachineStateEnum.STOP;
                        IsStart = false;
                        IsStop = true;
                        System.Windows.MessageBox.Show("Abort Scan, Please check alarm -> reset -> initialize!", "Alarm",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    ParameterID = i + 1;
                    _oneDControl.SoftwareTriggerAndRecording(ParameterID);
                    Thread.Sleep(100);
                    break;
                case 13:
                    AppService.IAIMotion.AxisXValue = AppService.RecipeService.MultiPointT13X;
                    AppService.IAIMotion.AxisYValue = AppService.RecipeService.MultiPointT13Y;
                    motionStatus = await _iaiMotion.AbsoluteMoveWithCheckFinish("ABS", cancellationToken);
                    Thread.Sleep(500);
                    if (motionStatus == MotionStatus.EXCEPTION_STATUS)
                    {
                        AppService.MachineStatus.MachineState = MachineStateEnum.STOP;
                        IsStart = false;
                        IsStop = true;
                        System.Windows.MessageBox.Show("Abort Scan, Please check alarm -> reset -> initialize!", "Alarm",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    ParameterID = i + 1;
                    _oneDControl.SoftwareTriggerAndRecording(ParameterID);
                    Thread.Sleep(100);
                    break;
                case 14:
                    AppService.IAIMotion.AxisXValue = AppService.RecipeService.MultiPointT14X;
                    AppService.IAIMotion.AxisYValue = AppService.RecipeService.MultiPointT14Y;
                    motionStatus = await _iaiMotion.AbsoluteMoveWithCheckFinish("ABS", cancellationToken);
                    Thread.Sleep(500);
                    if (motionStatus == MotionStatus.EXCEPTION_STATUS)
                    {
                        AppService.MachineStatus.MachineState = MachineStateEnum.STOP;
                        IsStart = false;
                        IsStop = true;
                        System.Windows.MessageBox.Show("Abort Scan, Please check alarm -> reset -> initialize!", "Alarm",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    ParameterID = i + 1;
                    _oneDControl.SoftwareTriggerAndRecording(ParameterID);
                    Thread.Sleep(100);
                    break;
                case 15:
                    AppService.IAIMotion.AxisXValue = AppService.RecipeService.MultiPointT15X;
                    AppService.IAIMotion.AxisYValue = AppService.RecipeService.MultiPointT15Y;
                    motionStatus = await _iaiMotion.AbsoluteMoveWithCheckFinish("ABS", cancellationToken);
                    Thread.Sleep(500);
                    if (motionStatus == MotionStatus.EXCEPTION_STATUS)
                    {
                        AppService.MachineStatus.MachineState = MachineStateEnum.STOP;
                        IsStart = false;
                        IsStop = true;
                        System.Windows.MessageBox.Show("Abort Scan, Please check alarm -> reset -> initialize!", "Alarm",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    ParameterID = i + 1;
                    _oneDControl.SoftwareTriggerAndRecording(ParameterID);
                    Thread.Sleep(100);
                    break;
                case 16:
                    AppService.IAIMotion.AxisXValue = AppService.RecipeService.MultiPointT16X;
                    AppService.IAIMotion.AxisYValue = AppService.RecipeService.MultiPointT16Y;
                    motionStatus = await _iaiMotion.AbsoluteMoveWithCheckFinish("ABS", cancellationToken);
                    Thread.Sleep(500);
                    if (motionStatus == MotionStatus.EXCEPTION_STATUS)
                    {
                        AppService.MachineStatus.MachineState = MachineStateEnum.STOP;
                        IsStart = false;
                        IsStop = true;
                        System.Windows.MessageBox.Show("Abort Scan, Please check alarm -> reset -> initialize!", "Alarm",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    ParameterID = i + 1;
                    _oneDControl.SoftwareTriggerAndRecording(ParameterID);
                    Thread.Sleep(100);
                    break;
            }
        }
        private async Task OnDSequence(MotionStatus _motionStatus)
        {
            if (_adam6052Module1.CheckSafetyCondition())
            {
                if (!string.IsNullOrEmpty(AppService.DutBarcode))
                {
                    if (System.Windows.MessageBox.Show("Are you sure currently Probe sensor", "Information",
                        MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        ;
                    }
                    else
                    {
                        AppService.MachineStatus.MachineState = MachineStateEnum.NOTREADY;
                        IsStartEna = true;
                        return;
                    }
                    // Pattern Moving Parameters:
                    string x1 = (double.Parse(AppService.IAIMotion.RcXOriginalPos) + (double.Parse(AppService.IAIMotion.RcdXDistance)/2) + 5).ToString();
                    string y1 = (double.Parse(AppService.IAIMotion.RcYOriginalPos) + (double.Parse(AppService.IAIMotion.RcdYDistance)/2)).ToString();
                    string Dx = (double.Parse(AppService.IAIMotion.RcdXDistance) + 10).ToString();
                    string Dy = AppService.IAIMotion.RcdYDistance;
                    string Pitchy = AppService.IAIMotion.RcPitchY;
                    string SpeedPatern = AppService.IAIMotion.RcSpeedAxisX;
                    //string Acc = "0.3";
                    //string Dcc = "0.3";
                    ParaPaternMove = new string[] { x1, y1, Dx, Dy, Pitchy, SpeedPatern};
                    // Encoder trigger parameter:
                    int startPos = (int)((double.Parse(AppService.IAIMotion.RcXOriginalPos) + (double.Parse(AppService.IAIMotion.RcdXDistance) / 2)) * -200);
                    int stopPos = startPos + (int)(double.Parse(AppService.IAIMotion.RcdXDistance)  * 200);
                    float intervalCount = float.Parse(AppService.IAIMotion.RcPitchX) * 200;
                    int numberPoints = (int)Math.Floor(double.Parse(AppService.IAIMotion.RcdXDistance) * 200 / intervalCount + 1);
                    int numberLines = (int)(Math.Floor(double.Parse(AppService.IAIMotion.RcdYDistance) / double.Parse(AppService.IAIMotion.RcPitchY)) + 1);
                    string AllSampleCount = (numberPoints * numberLines).ToString();
                    bool onReturnX = true;
                    AppService.RecipeService.NumberOfRow1D = numberLines.ToString();
                    AppService.RecipeService.NumberOfColumn1D = numberPoints.ToString();
                    Thread.Sleep(200);
                    // Send setting for encoder X axis
                    if (_oneDControl.SendTriggerSetting("x", startPos, stopPos, intervalCount, onReturnX))
                    {
                        Logging(new LogInfor(LogType.Sequence, $"Set parameter for enc x axis trigger: Start_Pos - {startPos}, " +
                                                               $"Stop_Pos - {stopPos}, interval - {intervalCount}, OnReturnTrigger - {onReturnX}"));
                        logger.Info($"Set parameter for enc x axis trigger: Start_Pos - {startPos}, " +
                                    $"Stop_Pos - {stopPos}, interval - {intervalCount}, OnReturnTrigger - {onReturnX}");
                    }
                    else
                    {
                        Logging(new LogInfor(LogType.Sequence, "Can not set parameters for encoder x axis"));
                        logger.Error("Can not set parameters for encoder x axis");
                        return;
                    }
                    Thread.Sleep(200);
                    await _oneDControl.StartScan(AllSampleCount);
                    _eventAggregator.GetEvent<RaiseProccessDialogEvent>().Publish(true);
                }
            }
            else
            {
                AppService.MachineStatus.MachineState = MachineStateEnum.NOTREADY;
                return;
            }
        }
        private async Task FssSequence(MotionStatus _motionStatus, CancellationToken cancellationToken)
        {
            AppService.MachineStatus.MachineState = MachineStateEnum.RUN;
            // Check Vacuum status is on when place DUT on nest
            if (_adam6052Module1.CheckSafetyCondition())
            {
                if (!string.IsNullOrEmpty(AppService.DutBarcode))
                {
                    if (System.Windows.MessageBox.Show("Are you sure currently Probe sensor", "Information",
                        MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        ;
                    }
                    else
                    {
                        AppService.MachineStatus.MachineState = MachineStateEnum.NOTREADY;
                        IsStartEna = true;
                        return;
                    }
                    // Get value X, Y from recipe is loaded
                    AppService.IAIMotion.AxisXValue = AppService.IAIMotion.RcXOriginalPos;
                    AppService.IAIMotion.AxisYValue = AppService.IAIMotion.RcYOriginalPos;
                    // rewrite function move abs and wait to move done
                    _motionStatus = await _iaiMotion.AbsoluteMoveWithCheckFinish("ABS", cancellationToken);
                    Thread.Sleep(500);
                    if (_motionStatus == MotionStatus.EXCEPTION_STATUS)
                    {
                        AppService.MachineStatus.MachineState = MachineStateEnum.STOP;
                        IsStart = false;
                        IsStop = true;
                        System.Windows.MessageBox.Show("Abort Scan, Please check alarm -> reset -> initialize!", "Alarm",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    _eventAggregator.GetEvent<RaiseProccessDialogEvent>().Publish(true);
                }
            }
            else
            {
                AppService.MachineStatus.MachineState = MachineStateEnum.NOTREADY;
                return;
            }
        }
        private async Task OnProcessingScanning()
        {
            MotionStatus _motionStatus = MotionStatus.NONE;
            cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;
            ReportItems.StartTest = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            await _fssControl.RunScriptAutoAsync();
            if (_motionStatus == MotionStatus.EXCEPTION_STATUS)
            {
                AppService.MachineStatus.MachineState = MachineStateEnum.STOP;
                System.Windows.MessageBox.Show("Abort Scan", "Warning",
                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                _eventAggregator.GetEvent<CloseProcessDialogEvent>().Publish(true);
                // Insert test report to database after close dialog
                long _reportDBId = 0;
                ReportItems.RecipeID = AppService.RecipeService.RecipeName;
                ReportItems.DutID = AppService.DutBarcode;
                ReportItems.RecipeSensorType = AppService.PrecitecService.SensorType; // fix
                ReportItems.RecipeX0 = AppService.RecipeService.XOriginalPosition;
                ReportItems.RecipeY0 = AppService.RecipeService.YOriginalPosition;
                ReportItems.RecipeDX = AppService.RecipeService.DXPosition;
                ReportItems.RecipeDY = AppService.RecipeService.DYPosition;
                ReportItems.RecipeDWD = AppService.RecipeService.DWDLeft + " " + AppService.RecipeService.DWDRight;
                ReportItems.RecipeQTH = AppService.RecipeService.QTH;
                ReportItems.FinishTest = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                _dataHandler.InsertReportData(ReportItems, out _reportDBId);
                ReportItems.ClearAllData();
            }
            _motionStatus = await _iaiMotion.AbsoluteMoveWithCheckFinish("home", cancellationToken);
            AppCommand.cmdDOVaccumOnOff.Execute("VacOff");
            AppService.MachineStatus.MachineState = MachineStateEnum.STOP;
            IsStart = false;
            IsStop = true;
            IsRunning = false;
            System.Windows.MessageBox.Show("Finish Scan", "Information",
            MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private async Task OneDOnProcessingScanning()
        {
            MotionStatus _motionStatus = MotionStatus.NONE;
            cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;
            ReportItems.StartTest = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Thread.Sleep(200);
            _motionStatus = await _iaiMotion.PatternMove(ParaPaternMove, cancellationToken);
            Thread.Sleep(200);
            if (_motionStatus == MotionStatus.EXCEPTION_STATUS)
            {
                AppService.MachineStatus.MachineState = MachineStateEnum.STOP;
                IsStart = false;
                IsStop = true;
                System.Windows.MessageBox.Show("Abort Scan, Please check alarm -> reset -> initialize!", "Alarm",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                _eventAggregator.GetEvent<CloseProcessDialogEvent>().Publish(true);
                // Insert test report to database after close dialog
                long _reportDBId = 0;
                ReportItems.RecipeID = AppService.RecipeService.RecipeName;
                ReportItems.DutID = AppService.DutBarcode;
                ReportItems.RecipeSensorType = AppService.PrecitecService.SensorType; // fix
                ReportItems.RecipeX0 = AppService.RecipeService.XOriginalPosition;
                ReportItems.RecipeY0 = AppService.RecipeService.YOriginalPosition;
                ReportItems.RecipeDX = AppService.RecipeService.DXPosition;
                ReportItems.RecipeDY = AppService.RecipeService.DYPosition;
                ReportItems.RecipeDWD = AppService.RecipeService.DWDLeft + " " + AppService.RecipeService.DWDRight;
                ReportItems.RecipeQTH = AppService.RecipeService.QTH;
                ReportItems.FinishTest = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                _dataHandler.InsertReportData(ReportItems, out _reportDBId);
                ReportItems.ClearAllData();
            }
            _motionStatus = await _iaiMotion.AbsoluteMoveWithCheckFinish("home", cancellationToken);
            AppCommand.cmdDOVaccumOnOff.Execute("VacOff");
            AppService.MachineStatus.MachineState = MachineStateEnum.STOP;
            IsStart = false;
            IsStop = true;
            IsRunning = false;
            System.Windows.MessageBox.Show("Finish Scan", "Information",
            MessageBoxButton.OK, MessageBoxImage.Information);


        }
        private async Task OneDMPOnProcessingScanning()
        {
            MotionStatus _motionStatus = MotionStatus.NONE;
            cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;
            ReportItems.StartTest = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int _loopCount = 16;
            for (int i = 0; i < _loopCount; i++)
            {
                await MoveToPointAndTrigger(i, cancellationToken);
            }

            await _oneDControl.SaveMultiPointDataIntoFile();

            if (_motionStatus == MotionStatus.EXCEPTION_STATUS)
            {
                AppService.MachineStatus.MachineState = MachineStateEnum.STOP;
                IsStart = false;
                IsStop = true;
                System.Windows.MessageBox.Show("Abort Scan, Please check alarm -> reset -> initialize!", "Alarm",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                _eventAggregator.GetEvent<CloseProcessDialogEvent>().Publish(true);
                // Insert test report to database after close dialog
                long _reportDBId = 0;
                ReportItems.RecipeID = AppService.RecipeService.RecipeName;
                ReportItems.DutID = AppService.DutBarcode;
                ReportItems.RecipeSensorType = AppService.PrecitecService.SensorType; // fix
                ReportItems.RecipeX0 = AppService.RecipeService.XOriginalPosition;
                ReportItems.RecipeY0 = AppService.RecipeService.YOriginalPosition;
                ReportItems.RecipeDX = AppService.RecipeService.DXPosition;
                ReportItems.RecipeDY = AppService.RecipeService.DYPosition;
                ReportItems.RecipeDWD = AppService.RecipeService.DWDLeft + " " + AppService.RecipeService.DWDRight;
                ReportItems.RecipeQTH = AppService.RecipeService.QTH;
                ReportItems.FinishTest = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                _dataHandler.InsertReportData(ReportItems, out _reportDBId);
                ReportItems.ClearAllData();
            }
            _motionStatus = await _iaiMotion.AbsoluteMoveWithCheckFinish("home", cancellationToken);
            AppCommand.cmdDOVaccumOnOff.Execute("VacOff");
            AppService.MachineStatus.MachineState = MachineStateEnum.STOP;
            IsStart = false;
            IsStop = true;
            IsRunning = false;
            System.Windows.MessageBox.Show("Finish Scan", "Information",
            MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void ExStop()
        {
            StopMachine();
        }
        private void StopMachine()
        {
            cancellationTokenSource?.Cancel();
            IsStart = false;
            IsStop = true;
            IsStartEna = true;
            if (AppService.PrecitecService.OneDSelected)
            {
                _oneDControl.StopScan();
            }
            if (AppService.PrecitecService.FssSelected)
            {
                _fssControl.StopProgram();
            }
            if (AppService.PrecitecService.OneDMPSelected)
            {
                _oneDControl.StopRecording();
            }
            _eventAggregator.GetEvent<CloseProcessDialogEvent>().Publish(true);
        }
        private void ExReset()
        {
            // test insert test report
            //_eventAggregator.GetEvent<RaiseProccessDialogEvent>().Publish(true);
            long _reportDBId = 0;
            ReportItems.StartTest = DateTime.Now.ToString();
            ReportItems.RecipeID = "T1FSS";
            ReportItems.DutID = "DUT12314";
            ReportItems.RecipeSensorType = "FSS"; // fix
            ReportItems.RecipeX0 = "198.5";
            ReportItems.RecipeY0 = "100";
            ReportItems.RecipeDX = "25";
            ReportItems.RecipeDY = "18";
            ReportItems.RecipeDWD = "400 1100";
            ReportItems.RecipeQTH = "7";
            ReportItems.FinishTest = DateTime.Now.ToString();
            _dataHandler.InsertReportData(ReportItems, out _reportDBId);
            ReportItems.ClearAllData();
        }
        private void ExClear()
        {
            //_eventAggregator.GetEvent<RaiseProccessDialogEvent>().Publish(true);
        }
        public void Logging(LogInfor _logData)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                AppService.LogInfors.Add(_logData);
                AppService.ScrollViewerVerticalOffset = _appService.LogInfors.Count - 1;
            });
        }
        #endregion
    }
}
