using PORLA.HMI.Main.Views;
using PORLA.HMI.Module.Views;
using PORLA.HMI.Module.Views.LogPagess;
using PORLA.HMI.Module.Views.SettingPages;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using log4net;
using PORLA.HMI.Service;
using PORLA.HMI.Module.ViewModels;
using POLAR.DIOADAM6052;
using POLAR.IAIMotionControl;
using PORLA.HMI.Service.Configuration;
using static PORLA.HMI.Service.Configuration.ConfigValue;
using PORLA.HMI.Module.Model.AlarmHandle;
using System.Timers;
using POLAR.PrecitecControl;
using Prism.Services.Dialogs;
using Prism.Events;
using POLAR.EventAggregator;

namespace PORLA.HMI.Main.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        
        private static readonly log4net.ILog logger = 
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IDialogService _dialogService;
        private IAppService _appService;
        public IAppService AppService
        {
            get { return _appService; }
            set { SetProperty(ref _appService, value); }
        }

        private IConfigHandler _configHandler;
        public IConfigHandler ConfigHandler
        {
            get { return _configHandler; }
            set { SetProperty(ref _configHandler, value); }
        }

        private IAdam6052Module1 _adam6052Module1;
        public IAdam6052Module1 Adam6052Module1
        {
            get { return _adam6052Module1; }
            set { SetProperty(ref _adam6052Module1, value); }
        }

        private IAdam6052Module2 _adam6052Module2;
        public IAdam6052Module2 Adam6052Module2
        {
            get { return _adam6052Module2; }
            set { SetProperty(ref _adam6052Module2, value); }
        }

        private IIAIMotion _iaiMotion;
        public IIAIMotion IAIMotionControl
        {
            get { return _iaiMotion; }
            set { SetProperty(ref _iaiMotion, value); }
        }

        private IFSSAreaScan _fssObject;
        public IFSSAreaScan FssObject
        {
            get { return _fssObject; }
            set { SetProperty(ref _fssObject, value); }
        }

        private IOneDAreaScan _oneDObject;
        public IOneDAreaScan OneDObject
        {
            get { return _oneDObject; }
            set { SetProperty(ref _oneDObject, value); }
        }

        IEventAggregator _eventAggregator;

        private string _title = "PoLar App";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public Timer TimerStartup { get; }

        private readonly IRegionManager _regionManager;

        private IAlarmHandler _alarmHandler;

        public MainWindowViewModel(IRegionManager regionManager, 
            IConfigHandler configHandler, 
            IAlarmHandler alarmHandler, IDialogService dialogService,
            IAppService appService, IEventAggregator eventAggregator,
            IAdam6052Module1 adam6052Module1, IOneDAreaScan oneDAreaScan, 
            IAdam6052Module2 adam6052Module2, IFSSAreaScan fSSAreaScan)
        {
            configHandler.LoadXML(AppSetting.ConfigPath);
            _regionManager = regionManager;
            _dialogService = dialogService; 
            _appService = appService;
            _alarmHandler = alarmHandler;
            _configHandler = configHandler;
            _eventAggregator = eventAggregator;

            _regionManager.RegisterViewWithRegion(Regions.TopPanelRegion, typeof(Toppanel));
            _regionManager.RegisterViewWithRegion(Regions.BottomPanelRegion, typeof(Bottompanel));
            _regionManager.RegisterViewWithRegion(Regions.RightPanelRegion, typeof(Rightpanel));
            _regionManager.RegisterViewWithRegion(Regions.OptionPanelRegion, typeof(EmptyPanel));
            _regionManager.RegisterViewWithRegion(Regions.ContentRegion, typeof(HomePage));
            _regionManager.RegisterViewWithRegion(Regions.ContentRegion, typeof(AlarmPage));
            _regionManager.RegisterViewWithRegion(Regions.ContentRegion, typeof(SettingPage));
            _regionManager.RegisterViewWithRegion(Regions.ContentRegion, typeof(VersionPage));
            _regionManager.RegisterViewWithRegion(Regions.ContentRegion, typeof(LogPanel));

            _adam6052Module1 = adam6052Module1;
            _adam6052Module2 = adam6052Module2;
            
            _fssObject = fSSAreaScan;
            _oneDObject = oneDAreaScan;
            
            InitializeDevices();
            LoadConfig();
            _eventAggregator.GetEvent<CloseAppEvent>().Subscribe(OnDisconnectDevice);
        }

        private void OnDisconnectDevice(bool obj)
        {
            try
            {
                _adam6052Module1.Disconnect();
                _adam6052Module2.Disconnect();
                if (AppService.PrecitecService.FssSelected)
                {
                    FssObject.Disconnect();
                }
                if (AppService.PrecitecService.OneDSelected || AppService.PrecitecService.OneDMPSelected)
                {
                    _oneDObject.CloseConnection();
                }
                logger.Info("Disconnected Adam Module 1, Adam Module 2");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            
        }

        private void InitializeDevices()
        {
            var parameters = new DialogParameters("SelectPrecitecSensorDialog");
            _dialogService.ShowDialog("SelectPrecitecSensorDialog", parameters, result => { });

            if (AppService.PrecitecService.FssSelected)
            {
                FssObject.InitializeFssAsync();
            }
            if (AppService.PrecitecService.OneDSelected || AppService.PrecitecService.OneDMPSelected)
            {
                OneDObject.InitializeOneD();
            }
            _adam6052Module1.Initialize(AppService.DIOModule1.IpAddress);
        }

        private void LoadConfig()
        {
            try
            {
                AppService.IAIMotion.IPIAI = ConfigHandler.GetValue<string>(IAIConfig.IPAddress);
                AppService.IAIMotion.PortAuto = ConfigHandler.GetValue<string>(IAIConfig.PortAuto);
                AppService.IAIMotion.Station = ConfigHandler.GetValue<string>(IAIConfig.Station);
                if (AppService.PrecitecService.FssSelected)
                { 
                    AppService.IAIMotion.XAxisSave = ConfigHandler.GetValue<string>(IAIConfig.XFixtureOffsetFSS);
                    AppService.IAIMotion.YAxisSave = ConfigHandler.GetValue<string>(IAIConfig.YFixtureOffsetFSS);
                }
                if (AppService.PrecitecService.OneDSelected || AppService.PrecitecService.OneDMPSelected)
                {
                    AppService.IAIMotion.XAxisSave = ConfigHandler.GetValue<string>(IAIConfig.XFixtureOffset1D);
                    AppService.IAIMotion.YAxisSave = ConfigHandler.GetValue<string>(IAIConfig.YFixtureOffset1D);
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            
        }
    }
}
