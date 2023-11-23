using PORLA.HMI.Module.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using PORLA.HMI.Service;
using log4net;
using System.Windows;
using System.Xml.Linq;
using System.Security.Policy;
using PORLA.HMI.Service.Configuration;
using static PORLA.HMI.Service.Configuration.ConfigValue;

namespace PORLA.HMI.Module.ViewModels.SettingPages
{
    public class SettingPageViewModel : BindableBase
    {
        #region Property
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
        public DelegateCommand btnsave { get; private set; }
        
        private string _originalCoordinates;
        public string OriginalCoordinates
        {
            get { return _originalCoordinates; }
            set { SetProperty(ref _originalCoordinates, value); }
        }
        private string _settingAcceleration;
        public string SettingAcceleration
        {
            get { return _settingAcceleration; }
            set { SetProperty(ref _settingAcceleration, value); }
        }
        private string _highSpeedSensor;
        public string HighSpeedSensor
        {
            get { return _highSpeedSensor; }
            set { SetProperty(ref _highSpeedSensor, value); }
        }
        private string _lowSpeedSensor;
        public string LowSpeedSensor
        {
            get { return _lowSpeedSensor; }
            set { SetProperty(ref _lowSpeedSensor, value); }
        }
        private string _settingIPSensor;
        public string SettingIPSensor
        {
            get { return _settingIPSensor; }
            set { SetProperty(ref _settingIPSensor, value); }
        }
        private string _settingIPAdam;
        public string SettingIPAdam
        {
            get { return _settingIPAdam; }
            set { SetProperty(ref _settingIPAdam, value); }
        }
        private string _xLimit;
        public string XLimit
        {
            get { return _xLimit; }
            set { SetProperty(ref _xLimit, value); }
        }
        private string _yLimit;
        public string YLimit
        {
            get { return _yLimit; }
            set 
            {
                SetProperty(ref _yLimit, value); 
            }
        }
        public DelegateCommand SaveConfigFileCmd { get; private set;}
        public DelegateCommand LoadConfigFileCmd { get; private set;}
        public DelegateCommand SetFixtureCoordinate { get; set; }

        #endregion
        #region Contructors
        public SettingPageViewModel(IAppService Appservice, IConfigHandler configHandler)
        {
            AppService = Appservice;
            _configHandler = configHandler;
            SaveConfigFileCmd = new DelegateCommand(() => exSaveConfig());
            LoadConfigFileCmd = new DelegateCommand(() => exLoadConfig());
            SetFixtureCoordinate = new DelegateCommand(exSetFixtureCoordinate);
        }

        private void exSetFixtureCoordinate()
        {
            AppService.IAIMotion.XAxisSave = AppService.IAIMotion.XCoordinate;
            AppService.IAIMotion.YAxisSave = AppService.IAIMotion.YCoordinate;
            if (AppService.PrecitecService.FssSelected)
            {
                ConfigHandler.SetValue<string>(IAIConfig.XFixtureOffsetFSS, AppService.IAIMotion.XAxisSave);
                ConfigHandler.SetValue<string>(IAIConfig.YFixtureOffsetFSS, AppService.IAIMotion.YAxisSave);
            }
            if (AppService.PrecitecService.OneDSelected || AppService.PrecitecService.OneDMPSelected)
            {
                ConfigHandler.SetValue<string>(IAIConfig.XFixtureOffset1D, AppService.IAIMotion.XAxisSave);
                ConfigHandler.SetValue<string>(IAIConfig.YFixtureOffset1D, AppService.IAIMotion.YAxisSave);
            }
            //AppService.IAIMotion.YAxisSave = ConfigHandler.GetValue<string>(IAIConfig.XFixtureOffset);
            MessageBox.Show("Set zero done!", "Information",
                        MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion
        #region Methods
        private void exLoadConfig()
        {
            XDocument doc = XDocument.Load(@".\ConfigFile\SettingConfig.xml");
            XElement appSettingsElement = doc.Root.Element("appSettings");
            XElement generalSettingsElement = appSettingsElement.Elements("add").FirstOrDefault(e => e.Attribute("key")?.Value == "General");
            XElement advancedSettingsElement = appSettingsElement.Elements("add").FirstOrDefault(e => e.Attribute("key")?.Value == "Advanced");
            OriginalCoordinates = generalSettingsElement.Attribute("OriginalCoordinates")?.Value;
            SettingAcceleration = generalSettingsElement.Attribute("SettingAcceleration")?.Value;
            HighSpeedSensor = generalSettingsElement.Attribute("HighSpeedSensor")?.Value;
            LowSpeedSensor = generalSettingsElement.Attribute("LowSpeedSensor")?.Value;
            SettingIPSensor = generalSettingsElement.Attribute("SettingIPSensor")?.Value;
            SettingIPAdam = generalSettingsElement.Attribute("SettingIPAdam")?.Value;
            XLimit = advancedSettingsElement.Attribute("XLimit")?.Value;
            YLimit = advancedSettingsElement.Attribute("YLimit")?.Value;
           
        }
        private void exSaveConfig()
        {
            System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure to save?",
                                           "Information",
                                           System.Windows.MessageBoxButton.OKCancel,
                                           System.Windows.MessageBoxImage.Question);
            if (result == System.Windows.MessageBoxResult.OK)
            {
                XDocument doc = XDocument.Load(@"C:\Users\hoanvduong\Desktop\Porla\PorlaSorftware\Porla\PORLA.HMI.Main\bin\Debug\ConfigFile\SettingConfig.xml");
                XElement appSettingsElement = doc.Root.Element("appSettings");
                XElement generalSettingsElement = appSettingsElement.Elements("add").FirstOrDefault(e => e.Attribute("key")?.Value == "General");
                XElement advancedSettingsElement = appSettingsElement.Elements("add").FirstOrDefault(e => e.Attribute("key")?.Value == "Advanced");
                generalSettingsElement.SetAttributeValue("OriginalCoordinates", OriginalCoordinates.ToString());
                generalSettingsElement.SetAttributeValue("SettingAcceleration", SettingAcceleration);
                generalSettingsElement.SetAttributeValue("HighSpeedSensor", HighSpeedSensor);
                generalSettingsElement.SetAttributeValue("LowSpeedSensor", LowSpeedSensor);
                generalSettingsElement.SetAttributeValue("SettingIPSensor", SettingIPSensor);
                generalSettingsElement.SetAttributeValue("SettingIPAdam", SettingIPAdam);
                advancedSettingsElement.SetAttributeValue("XLimit", XLimit);
                advancedSettingsElement.SetAttributeValue("YLimit", YLimit);
                doc.Save(@"C:\Users\hoanvduong\Desktop\Porla\PorlaSorftware\Porla\PORLA.HMI.Main\bin\Debug\ConfigFile\SettingConfig.xml");
                //logMC.Info(ProgName + "Setting Saved");
            }
        }
        #endregion

    }
}
