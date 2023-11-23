using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Prism.Commands;
using PORLA.HMI.Module.Model;
using SuproCL;
using System.Windows;
using System;
using PORLA.HMI.Module.Views.SettingPages;
using MySql.Data.MySqlClient;
using System.Data;
using Prism.Services.Dialogs;
using Org.BouncyCastle.Asn1.Mozilla;
using Prism.Regions;
using System.Security.AccessControl;
using PORLA.HMI.Service.Configuration;
using static PORLA.HMI.Service.Configuration.ConfigValue;
using System.Xml;
using System.IO;
using Prism.Events;
using PORLA.HMI.Module.Events;
using System.Windows.Forms;
using PORLA.HMI.Service;
using POLAR.ModelAggregator.Recipes;
using POLAR.EventAggregator;
using System.Xml.Linq;

namespace PORLA.HMI.Module.ViewModels.SettingPages
{
    public class RecipePageViewModel : BindableBase
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IConfigHandler _configHandler;

        private readonly IDialogService _dialogService;

        IEventAggregator _eventAggregator;

        IAppService _appService;
        public DelegateCommand ShowPopupCommand { get; set; }

        private ObservableCollection<RecipeParameterModel> _recipeDetail;
        public ObservableCollection<RecipeParameterModel> RecipeDetail
        {
            get { return _recipeDetail; }
            set { SetProperty(ref _recipeDetail, value); }
        }

        private RecipeParameterModel _selectedRecipe;
        public RecipeParameterModel SelectedRecipe
        {
            get { return _selectedRecipe; }
            set
            {
                SetProperty(ref _selectedRecipe, value);
                if (_selectedRecipe != null)
                {
                    if (_appService.PrecitecService.OneDMPSelected)
                    {
                        if (Parameter == null)
                            Parameter = new RecipeParameterModel();

                        Parameter.RecipeName = value.RecipeName;
                        Parameter.XOriginalPosition = value.XOriginalPosition;
                        Parameter.YOriginalPosition = value.YOriginalPosition;
                        Parameter.DXPosition = value.DXPosition;
                        Parameter.DYPosition = value.DYPosition;
                        Parameter.RXPosition = value.RXPosition;
                        Parameter.RYPosition = value.RYPosition;
                        Parameter.SpeedAxisX = value.SpeedAxisX;
                        Parameter.SpeedAxisY = value.SpeedAxisY;
                        Parameter.SensorType = value.SensorType;
                        Parameter.ThicknessSelection = value.ThicknessSelection;
                        Parameter.DWDLeft = value.DWDLeft;
                        Parameter.DWDRight = value.DWDRight;
                        Parameter.QTH = value.QTH;
                        Parameter.MultiPointT1X = value.MultiPointT1X;
                        Parameter.MultiPointT1Y = value.MultiPointT1Y;
                        Parameter.MultiPointT2X = value.MultiPointT2X;
                        Parameter.MultiPointT2Y = value.MultiPointT2Y;
                        Parameter.MultiPointT3X = value.MultiPointT3X;
                        Parameter.MultiPointT3Y = value.MultiPointT3Y;
                        Parameter.MultiPointT4X = value.MultiPointT4X;
                        Parameter.MultiPointT4Y = value.MultiPointT4Y;
                        Parameter.MultiPointT5X = value.MultiPointT5X;
                        Parameter.MultiPointT5Y = value.MultiPointT5Y;
                        Parameter.MultiPointT6X = value.MultiPointT6X;
                        Parameter.MultiPointT6Y = value.MultiPointT6Y;
                        Parameter.MultiPointT7X = value.MultiPointT7X;
                        Parameter.MultiPointT7Y = value.MultiPointT7Y;
                        Parameter.MultiPointT8X = value.MultiPointT8X;
                        Parameter.MultiPointT8Y = value.MultiPointT8Y;
                        Parameter.MultiPointT9X = value.MultiPointT9X;
                        Parameter.MultiPointT9Y = value.MultiPointT9Y;
                        Parameter.MultiPointT10X = value.MultiPointT10X;
                        Parameter.MultiPointT10Y = value.MultiPointT10Y;
                        Parameter.MultiPointT11X = value.MultiPointT11X;
                        Parameter.MultiPointT11Y = value.MultiPointT11Y;
                        Parameter.MultiPointT12X = value.MultiPointT12X;
                        Parameter.MultiPointT12Y = value.MultiPointT12Y;
                        Parameter.MultiPointT13X = value.MultiPointT13X;
                        Parameter.MultiPointT13Y = value.MultiPointT13Y;
                        Parameter.MultiPointT14X = value.MultiPointT14X;
                        Parameter.MultiPointT14Y = value.MultiPointT14Y;
                        Parameter.MultiPointT15X = value.MultiPointT15X;
                        Parameter.MultiPointT15Y = value.MultiPointT15Y;
                        Parameter.MultiPointT16X = value.MultiPointT16X;
                        Parameter.MultiPointT16Y = value.MultiPointT16Y;
                        _eventAggregator.GetEvent<RecipeItemSelectedEvent>().Publish(Parameter);
                    }
                    else
                    {
                        if (Parameter == null)
                            Parameter = new RecipeParameterModel();

                        Parameter.RecipeName = value.RecipeName;
                        Parameter.XOriginalPosition = value.XOriginalPosition;
                        Parameter.YOriginalPosition = value.YOriginalPosition;
                        Parameter.DXPosition = value.DXPosition;
                        Parameter.DYPosition = value.DYPosition;
                        Parameter.RXPosition = value.RXPosition;
                        Parameter.RYPosition = value.RYPosition;
                        Parameter.SpeedAxisX = value.SpeedAxisX;
                        Parameter.SpeedAxisY = value.SpeedAxisY;
                        Parameter.SensorType = value.SensorType;
                        Parameter.ThicknessSelection = value.ThicknessSelection;
                        Parameter.DWDLeft = value.DWDLeft;
                        Parameter.DWDRight = value.DWDRight;
                        Parameter.QTH = value.QTH;
                        _eventAggregator.GetEvent<RecipeItemSelectedEvent>().Publish(Parameter);
                    }
                    
                }
            }
        }

        private RecipeParameterModel _parameter;
        public RecipeParameterModel Parameter
        {
            get { return _parameter; }
            set { SetProperty(ref _parameter, value); }
        }
        public DelegateCommand NewRecipeCmd { get; private set; }
        public DelegateCommand UpdateRecipeParameterCmd { get; private set; }
        public DelegateCommand DeleteRecipeParameterCmd { get; private set; }
        public DelegateCommand DownloadRecipeCmd { get; private set; }
        private string _rcName = "";
        public string RcName
        {
            get { return _rcName; }
            set { SetProperty(ref _rcName, value); }
        }
        private Visibility _isOneDMP;
        public Visibility IsOneDMP
        {
            get { return _isOneDMP; }
            set { SetProperty(ref _isOneDMP, value); }
        }

        private Visibility _isOneD;
        public Visibility IsOneD
        {
            get { return _isOneD; }
            set { SetProperty(ref _isOneD, value); }
        }

        private Visibility _isFSS;
        public Visibility IsFSS
        {
            get { return _isFSS; }
            set { SetProperty(ref _isFSS, value); }
        }
        public RecipePageViewModel(IDialogService dialogService, IRegionManager regionManager, IConfigHandler configHandler,
            IEventAggregator eventAggregator, IAppService appService)
        {
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _configHandler = configHandler; 
            _appService = appService;

            ShowPopupCommand = new DelegateCommand(ShowPopup);
            UpdateRecipeParameterCmd = new DelegateCommand(exUpdateRecipeParameter);
            DownloadRecipeCmd = new DelegateCommand(ExDownloadRecipe);
            DeleteRecipeParameterCmd = new DelegateCommand(exDeleteRecipeParameter);
            
            RecipeDetail = new ObservableCollection<RecipeParameterModel>();
            SelectedRecipe = new RecipeParameterModel();
            Parameter = new RecipeParameterModel();

            _eventAggregator.GetEvent<RecipeItemSelectedEvent>().Subscribe(OnRecipeItemSelected, ThreadOption.UIThread);
            _eventAggregator.GetEvent<RecipeCreateNewEvent>().Subscribe(OnRecipeCreateNew);
            _eventAggregator.GetEvent<RecipeUpdateDoneEvent>().Subscribe(OnRecipeUpdateDone);
            RecipeVisibility();            
            LoadRecipeConfig();            
        }

        private void RecipeVisibility()
        {
            if (_appService.PrecitecService.OneDMPSelected)
            {
                IsOneDMP = Visibility.Visible;
            }
            else
            {
                IsOneDMP = Visibility.Hidden;
                if (_appService.PrecitecService.OneDSelected)
                {
                    IsOneD = Visibility.Visible;
                    IsFSS = Visibility.Collapsed;
                }
                if (_appService.PrecitecService.FssSelected)
                {
                    IsFSS = Visibility.Visible;
                    IsOneD = Visibility.Collapsed;
                }
            }
        }

        private void OnRecipeUpdateDone(bool obj)
        {
            RecipeList.Clear();
            LoadRecipeConfig();
        }

        private void ExDownloadRecipe()
        {
            if(RecipeDetail != null)
            {
                _appService.RecipeName = RecipeDetail[0].RecipeName;
                _appService.PrecitecService.SensorType = RecipeDetail[0].SensorType;
                _appService.IAIMotion.RcXOriginalPos = RecipeDetail[0].XOriginalPosition;
                _appService.IAIMotion.RcYOriginalPos = RecipeDetail[0].YOriginalPosition;
                _appService.IAIMotion.RcdXDistance = RecipeDetail[0].DXPosition;
                _appService.IAIMotion.RcdYDistance = RecipeDetail[0].DYPosition;
                _appService.IAIMotion.RcSpeedAxisX = RecipeDetail[0].SpeedAxisX;
                _appService.IAIMotion.RcSpeedAxisY = RecipeDetail[0].SpeedAxisY;
                _appService.IAIMotion.RcPitchX = RecipeDetail[0].RXPosition;
                _appService.IAIMotion.RcPitchY = RecipeDetail[0].RYPosition;
                _appService.RecipeService.Selected = true;
                if (_appService.PrecitecService.FssSelected)
                {
                    _eventAggregator.GetEvent<FssRecipeSelectedEvent>().Publish(true);
                }
                if (_appService.PrecitecService.OneDSelected || _appService.PrecitecService.OneDMPSelected)
                {
                    _eventAggregator.GetEvent<OneDRecipeSelectedEvent>().Publish(true);
                }
                System.Windows.Forms.MessageBox.Show("Downloaded the current recipe!.", "Recipe Download", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        private void OnRecipeCreateNew(RecipeParameterModel obj)
        {
            RecipeList.Add(obj);
        }

        private void OnRecipeItemSelected(RecipeParameterModel obj)
        {
            RecipeDetail.Clear();
            RecipeDetail.Add(obj);
            if (_appService.PrecitecService.OneDMPSelected)
            {
                _appService.RecipeService.RecipeName = obj.RecipeName;
                _appService.RecipeService.XOriginalPosition = obj.XOriginalPosition;
                _appService.RecipeService.YOriginalPosition = obj.YOriginalPosition;
                _appService.RecipeService.DXPosition = obj.DXPosition;
                _appService.RecipeService.DYPosition = obj.DYPosition;
                _appService.RecipeService.RXPosition = obj.RXPosition;
                _appService.RecipeService.RYPosition = obj.RYPosition;
                _appService.RecipeService.SpeedAxisX = obj.SpeedAxisX;
                _appService.RecipeService.SpeedAxisY = obj.SpeedAxisY;
                _appService.RecipeService.DWDLeft = obj.DWDLeft;
                _appService.RecipeService.DWDRight = obj.DWDRight;
                _appService.RecipeService.QTH = obj.QTH;
                _appService.RecipeService.MultiPointT1X = obj.MultiPointT1X;
                _appService.RecipeService.MultiPointT1Y = obj.MultiPointT1Y;
                _appService.RecipeService.MultiPointT2X = obj.MultiPointT2X;
                _appService.RecipeService.MultiPointT2Y = obj.MultiPointT2Y;
                _appService.RecipeService.MultiPointT3X = obj.MultiPointT3X;
                _appService.RecipeService.MultiPointT3Y = obj.MultiPointT3Y;
                _appService.RecipeService.MultiPointT4X = obj.MultiPointT4X;
                _appService.RecipeService.MultiPointT4Y = obj.MultiPointT4Y;
                _appService.RecipeService.MultiPointT5X = obj.MultiPointT5X;
                _appService.RecipeService.MultiPointT5Y = obj.MultiPointT5Y;
                _appService.RecipeService.MultiPointT6X = obj.MultiPointT6X;
                _appService.RecipeService.MultiPointT6Y = obj.MultiPointT6Y;
                _appService.RecipeService.MultiPointT7X = obj.MultiPointT7X;
                _appService.RecipeService.MultiPointT7Y = obj.MultiPointT7Y;
                _appService.RecipeService.MultiPointT8X = obj.MultiPointT8X;
                _appService.RecipeService.MultiPointT8Y = obj.MultiPointT8Y;
                _appService.RecipeService.MultiPointT9X = obj.MultiPointT9X;
                _appService.RecipeService.MultiPointT9Y = obj.MultiPointT9Y;
                _appService.RecipeService.MultiPointT10X = obj.MultiPointT10X;
                _appService.RecipeService.MultiPointT10Y = obj.MultiPointT10Y;
                _appService.RecipeService.MultiPointT11X = obj.MultiPointT11X;
                _appService.RecipeService.MultiPointT11Y = obj.MultiPointT11Y;
                _appService.RecipeService.MultiPointT12X = obj.MultiPointT12X;
                _appService.RecipeService.MultiPointT12Y = obj.MultiPointT12Y;
                _appService.RecipeService.MultiPointT13X = obj.MultiPointT13X;
                _appService.RecipeService.MultiPointT13Y = obj.MultiPointT13Y;
                _appService.RecipeService.MultiPointT14X = obj.MultiPointT14X;
                _appService.RecipeService.MultiPointT14Y = obj.MultiPointT14Y;
                _appService.RecipeService.MultiPointT15X = obj.MultiPointT15X;
                _appService.RecipeService.MultiPointT15Y = obj.MultiPointT15Y;
                _appService.RecipeService.MultiPointT16X = obj.MultiPointT16X;
                _appService.RecipeService.MultiPointT16Y = obj.MultiPointT16Y;
            }
            else
            {
                _appService.RecipeService.RecipeName = obj.RecipeName;
                _appService.RecipeService.XOriginalPosition = obj.XOriginalPosition;
                _appService.RecipeService.YOriginalPosition = obj.YOriginalPosition;
                _appService.RecipeService.DXPosition = obj.DXPosition;
                _appService.RecipeService.DYPosition = obj.DYPosition;
                _appService.RecipeService.RXPosition = obj.RXPosition;
                _appService.RecipeService.RYPosition = obj.RYPosition;
                _appService.RecipeService.SpeedAxisX = obj.SpeedAxisX;
                _appService.RecipeService.SpeedAxisY = obj.SpeedAxisY;
                _appService.RecipeService.DWDLeft = obj.DWDLeft;
                _appService.RecipeService.DWDRight = obj.DWDRight;
                _appService.RecipeService.QTH = obj.QTH;
            }
            
        }

        private ObservableCollection<RecipeParameterModel> _recipeList = new ObservableCollection<RecipeParameterModel>();
        public ObservableCollection<RecipeParameterModel> RecipeList
        {
            get { return _recipeList; }
            set { SetProperty(ref _recipeList, value); }
        }

        private void LoadRecipeConfig()
        {
            var xmlString = File.ReadAllText(AppSetting.RecipeConfigPath);

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            var recipeNode = xmlDoc.SelectNodes("/RECIPE/RECIPECONFIG");
            int _index = 0;

            foreach (XmlNode item in recipeNode)
            {
                if (_appService.PrecitecService.FssSelected)
                {
                    _index++;
                    string recipeName = "";
                    string x0 = "";
                    string y0 = "";
                    string dX = "";
                    string dY = "";
                    string rX = "";
                    string rY = "";
                    string sensorType = "";
                    string thickness = "";
                    string speedX = "";
                    string speedY = "";
                    string dwdLeft = "";
                    string dwdRight = "";
                    string qth = "";
                    //string recipeName = item.SelectSingleNode("RecipeName").InnerText;
                    if (item.SelectSingleNode("RecipeName") != null)
                    { recipeName = item.SelectSingleNode("RecipeName").InnerText; }

                    if (item.SelectSingleNode("XOriginalPosition") != null)
                    { x0 = item.SelectSingleNode("XOriginalPosition").InnerText; }

                    if (item.SelectSingleNode("YOriginalPosition") != null)
                    { y0 = item.SelectSingleNode("YOriginalPosition").InnerText; }

                    if (item.SelectSingleNode("DXDistance") != null)
                    { dX = item.SelectSingleNode("DXDistance").InnerText; }

                    if (item.SelectSingleNode("DYDistance") != null)
                    { dY = item.SelectSingleNode("DYDistance").InnerText; }

                    if (item.SelectSingleNode("RXDistance") != null)
                    { rX = item.SelectSingleNode("RXDistance").InnerText; }

                    if (item.SelectSingleNode("RYDistance") != null)
                    { rY = item.SelectSingleNode("RYDistance").InnerText; }

                    if (item.SelectSingleNode("SpeedAxisX") != null)
                    { speedX = item.SelectSingleNode("SpeedAxisX").InnerText; }

                    if (item.SelectSingleNode("SpeedAxisY") != null)
                    { speedY = item.SelectSingleNode("SpeedAxisY").InnerText; }

                    if (item.SelectSingleNode("SensorType") != null)
                    { sensorType = item.SelectSingleNode("SensorType").InnerText; }

                    if (item.SelectSingleNode("Thickness") != null)
                    { thickness = item.SelectSingleNode("Thickness").InnerText; }

                    if (item.SelectSingleNode("DWDLeftSide") != null)
                    { dwdLeft = item.SelectSingleNode("DWDLeftSide").InnerText; }

                    if (item.SelectSingleNode("DWDRightSide") != null)
                    { dwdRight = item.SelectSingleNode("DWDRightSide").InnerText; }

                    if (item.SelectSingleNode("QualityThreshold") != null)
                    { qth = item.SelectSingleNode("QualityThreshold").InnerText; }

                    if (sensorType == "FSS")
                    {
                        RecipeList.Add(new RecipeParameterModel
                        {
                            NumberId = _index,
                            RecipeName = recipeName,
                            XOriginalPosition = x0,
                            YOriginalPosition = y0,
                            DXPosition = dX,
                            DYPosition = dY,
                            RXPosition = rX,
                            RYPosition = rY,
                            SpeedAxisX = speedX,
                            SpeedAxisY = speedY,
                            SensorType = sensorType,
                            ThicknessSelection = thickness,
                            DWDLeft = dwdLeft,
                            DWDRight = dwdRight,
                            QTH = qth
                        });

                    }

                }
                if (_appService.PrecitecService.OneDSelected)
                {
                    _index++;
                    string recipeName = "";
                    string x0 = "";
                    string y0 = "";
                    string dX = "";
                    string dY = "";
                    string rX = "";
                    string rY = "";
                    string sensorType = "";
                    string thickness = "";
                    string speedX = "";
                    string speedY = "";
                    string dwdLeft = "";
                    string dwdRight = "";
                    string qth = "";
                    //string recipeName = item.SelectSingleNode("RecipeName").InnerText;
                    if (item.SelectSingleNode("RecipeName") != null)
                    { recipeName = item.SelectSingleNode("RecipeName").InnerText; }

                    if (item.SelectSingleNode("XOriginalPosition") != null)
                    { x0 = item.SelectSingleNode("XOriginalPosition").InnerText; }

                    if (item.SelectSingleNode("YOriginalPosition") != null)
                    { y0 = item.SelectSingleNode("YOriginalPosition").InnerText; }

                    if (item.SelectSingleNode("DXDistance") != null)
                    { dX = item.SelectSingleNode("DXDistance").InnerText; }

                    if (item.SelectSingleNode("DYDistance") != null)
                    { dY = item.SelectSingleNode("DYDistance").InnerText; }

                    if (item.SelectSingleNode("RXDistance") != null)
                    { rX = item.SelectSingleNode("RXDistance").InnerText; }

                    if (item.SelectSingleNode("RYDistance") != null)
                    { rY = item.SelectSingleNode("RYDistance").InnerText; }

                    if (item.SelectSingleNode("SpeedAxisX") != null)
                    { speedX = item.SelectSingleNode("SpeedAxisX").InnerText; }

                    if (item.SelectSingleNode("SpeedAxisY") != null)
                    { speedY = item.SelectSingleNode("SpeedAxisY").InnerText; }

                    if (item.SelectSingleNode("SensorType") != null)
                    { sensorType = item.SelectSingleNode("SensorType").InnerText; }

                    if (item.SelectSingleNode("Thickness") != null)
                    { thickness = item.SelectSingleNode("Thickness").InnerText; }

                    if (item.SelectSingleNode("DWDLeftSide") != null)
                    { dwdLeft = item.SelectSingleNode("DWDLeftSide").InnerText; }

                    if (item.SelectSingleNode("DWDRightSide") != null)
                    { dwdRight = item.SelectSingleNode("DWDRightSide").InnerText; }

                    if (item.SelectSingleNode("QualityThreshold") != null)
                    { qth = item.SelectSingleNode("QualityThreshold").InnerText; }

                    if (sensorType == "1D")
                    {
                        RecipeList.Add(new RecipeParameterModel
                        {
                            NumberId = _index,
                            RecipeName = recipeName,
                            XOriginalPosition = x0,
                            YOriginalPosition = y0,
                            DXPosition = dX,
                            DYPosition = dY,
                            RXPosition = rX,
                            RYPosition = rY,
                            SpeedAxisX = speedX,
                            SpeedAxisY = speedY,
                            SensorType = sensorType,
                            ThicknessSelection = thickness,
                            DWDLeft = dwdLeft,
                            DWDRight = dwdRight,
                            QTH = qth
                        });

                    }

                }
                if(_appService.PrecitecService.OneDMPSelected)
                {
                    _index++;
                    string recipeName = "";
                    string x0 = "";
                    string y0 = "";
                    string dX = "";
                    string dY = "";
                    string rX = "";
                    string rY = "";
                    string sensorType = "";
                    string thickness = "";
                    string speedX = "";
                    string speedY = "";
                    string dwdLeft = "";
                    string dwdRight = "";
                    string qth = "";
                    string multipoin1X = "";
                    string multipoin1Y = "";
                    string multipoin2X = "";
                    string multipoin2Y = "";
                    string multipoin3X = "";
                    string multipoin3Y = "";
                    string multipoin4X = "";
                    string multipoin4Y = "";
                    string multipoin5X = "";
                    string multipoin5Y = "";
                    string multipoin6X = "";
                    string multipoin6Y = "";
                    string multipoin7X = "";
                    string multipoin7Y = "";
                    string multipoin8X = "";
                    string multipoin8Y = "";
                    string multipoin9X = "";
                    string multipoin9Y = "";
                    string multipoin10X = "";
                    string multipoin10Y = "";
                    string multipoin11X = "";
                    string multipoin11Y = "";
                    string multipoin12X = "";
                    string multipoin12Y = "";
                    string multipoin13X = "";
                    string multipoin13Y = "";
                    string multipoin14X = "";
                    string multipoin14Y = "";
                    string multipoin15X = "";
                    string multipoin15Y = "";
                    string multipoin16X = "";
                    string multipoin16Y = "";
                    //string recipeName = item.SelectSingleNode("RecipeName").InnerText;
                    if (item.SelectSingleNode("RecipeName") != null)
                    { recipeName = item.SelectSingleNode("RecipeName").InnerText; }

                    if (item.SelectSingleNode("XOriginalPosition") != null)
                    { x0 = item.SelectSingleNode("XOriginalPosition").InnerText; }

                    if (item.SelectSingleNode("YOriginalPosition") != null)
                    { y0 = item.SelectSingleNode("YOriginalPosition").InnerText; }

                    if (item.SelectSingleNode("DXDistance") != null)
                    { dX = item.SelectSingleNode("DXDistance").InnerText; }

                    if (item.SelectSingleNode("DYDistance") != null)
                    { dY = item.SelectSingleNode("DYDistance").InnerText; }

                    if (item.SelectSingleNode("RXDistance") != null)
                    { rX = item.SelectSingleNode("RXDistance").InnerText; }

                    if (item.SelectSingleNode("RYDistance") != null)
                    { rY = item.SelectSingleNode("RYDistance").InnerText; }

                    if (item.SelectSingleNode("SpeedAxisX") != null)
                    { speedX = item.SelectSingleNode("SpeedAxisX").InnerText; }

                    if (item.SelectSingleNode("SpeedAxisY") != null)
                    { speedY = item.SelectSingleNode("SpeedAxisY").InnerText; }

                    if (item.SelectSingleNode("SensorType") != null)
                    { sensorType = item.SelectSingleNode("SensorType").InnerText; }

                    if (item.SelectSingleNode("Thickness") != null)
                    { thickness = item.SelectSingleNode("Thickness").InnerText; }

                    if (item.SelectSingleNode("DWDLeftSide") != null)
                    { dwdLeft = item.SelectSingleNode("DWDLeftSide").InnerText; }

                    if (item.SelectSingleNode("DWDRightSide") != null)
                    { dwdRight = item.SelectSingleNode("DWDRightSide").InnerText; }

                    if (item.SelectSingleNode("QualityThreshold") != null)
                    { qth = item.SelectSingleNode("QualityThreshold").InnerText; }

                    if (item.SelectSingleNode("T1X") != null)
                    { multipoin1X = item.SelectSingleNode("T1X").InnerText; }
                    if (item.SelectSingleNode("T1Y") != null)
                    { multipoin1Y = item.SelectSingleNode("T1Y").InnerText; }

                    if (item.SelectSingleNode("T2X") != null)
                    { multipoin2X = item.SelectSingleNode("T2X").InnerText; }
                    if (item.SelectSingleNode("T2Y") != null)
                    { multipoin2Y = item.SelectSingleNode("T2Y").InnerText; }

                    if (item.SelectSingleNode("T3X") != null)
                    { multipoin3X = item.SelectSingleNode("T3X").InnerText; }
                    if (item.SelectSingleNode("T3Y") != null)
                    { multipoin3Y = item.SelectSingleNode("T3Y").InnerText; }

                    if (item.SelectSingleNode("T4X") != null)
                    { multipoin4X = item.SelectSingleNode("T4X").InnerText; }
                    if (item.SelectSingleNode("T4Y") != null)
                    { multipoin4Y = item.SelectSingleNode("T4Y").InnerText; }

                    if (item.SelectSingleNode("T5X") != null)
                    { multipoin5X = item.SelectSingleNode("T5X").InnerText; }
                    if (item.SelectSingleNode("T5Y") != null)
                    { multipoin5Y = item.SelectSingleNode("T5Y").InnerText; }

                    if (item.SelectSingleNode("T6X") != null)
                    { multipoin6X = item.SelectSingleNode("T6X").InnerText; }
                    if (item.SelectSingleNode("T6Y") != null)
                    { multipoin6Y = item.SelectSingleNode("T6Y").InnerText; }

                    if (item.SelectSingleNode("T7X") != null)
                    { multipoin7X = item.SelectSingleNode("T7X").InnerText; }
                    if (item.SelectSingleNode("T7Y") != null)
                    { multipoin7Y = item.SelectSingleNode("T7Y").InnerText; }

                    if (item.SelectSingleNode("T8X") != null)
                    { multipoin8X = item.SelectSingleNode("T8X").InnerText; }
                    if (item.SelectSingleNode("T8Y") != null)
                    { multipoin8Y = item.SelectSingleNode("T8Y").InnerText; }

                    if (item.SelectSingleNode("T9X") != null)
                    { multipoin9X = item.SelectSingleNode("T9X").InnerText; }
                    if (item.SelectSingleNode("T9Y") != null)
                    { multipoin9Y = item.SelectSingleNode("T9Y").InnerText; }

                    if (item.SelectSingleNode("T10X") != null)
                    { multipoin10X = item.SelectSingleNode("T10X").InnerText; }
                    if (item.SelectSingleNode("T10Y") != null)
                    { multipoin10Y = item.SelectSingleNode("T10Y").InnerText; }

                    if (item.SelectSingleNode("T11X") != null)
                    { multipoin11X = item.SelectSingleNode("T11X").InnerText; }
                    if (item.SelectSingleNode("T11Y") != null)
                    { multipoin11Y = item.SelectSingleNode("T11Y").InnerText; }

                    if (item.SelectSingleNode("T12X") != null)
                    { multipoin12X = item.SelectSingleNode("T12X").InnerText; }
                    if (item.SelectSingleNode("T12Y") != null)
                    { multipoin12Y = item.SelectSingleNode("T12Y").InnerText; }

                    if (item.SelectSingleNode("T13X") != null)
                    { multipoin13X = item.SelectSingleNode("T13X").InnerText; }
                    if (item.SelectSingleNode("T13Y") != null)
                    { multipoin13Y = item.SelectSingleNode("T13Y").InnerText; }

                    if (item.SelectSingleNode("T14X") != null)
                    { multipoin14X = item.SelectSingleNode("T14X").InnerText; }
                    if (item.SelectSingleNode("T14Y") != null)
                    { multipoin14Y = item.SelectSingleNode("T14Y").InnerText; }

                    if (item.SelectSingleNode("T15X") != null)
                    { multipoin15X = item.SelectSingleNode("T15X").InnerText; }
                    if (item.SelectSingleNode("T15Y") != null)
                    { multipoin15Y = item.SelectSingleNode("T15Y").InnerText; }

                    if (item.SelectSingleNode("T16X") != null)
                    { multipoin16X = item.SelectSingleNode("T16X").InnerText; }
                    if (item.SelectSingleNode("T16Y") != null)
                    { multipoin16Y = item.SelectSingleNode("T16Y").InnerText; }

                    if (sensorType == "1DMP")
                    {
                        RecipeList.Add(new RecipeParameterModel
                        {
                            NumberId = _index,
                            RecipeName = recipeName,
                            XOriginalPosition = x0,
                            YOriginalPosition = y0,
                            DXPosition = dX,
                            DYPosition = dY,
                            RXPosition = rX,
                            RYPosition = rY,
                            SpeedAxisX = speedX,
                            SpeedAxisY = speedY,
                            SensorType = sensorType,
                            ThicknessSelection = thickness,
                            DWDLeft = dwdLeft,
                            DWDRight = dwdRight,
                            QTH = qth,
                            MultiPointT1X = multipoin1X,
                            MultiPointT1Y = multipoin1Y,
                            MultiPointT2X = multipoin2X,
                            MultiPointT2Y = multipoin2Y,
                            MultiPointT3X = multipoin3X,
                            MultiPointT3Y = multipoin3Y,
                            MultiPointT4X = multipoin4X,
                            MultiPointT4Y = multipoin4Y,
                            MultiPointT5X = multipoin5X,
                            MultiPointT5Y = multipoin5Y,
                            MultiPointT6X = multipoin6X,
                            MultiPointT6Y = multipoin6Y,
                            MultiPointT7X = multipoin7X,
                            MultiPointT7Y = multipoin7Y,
                            MultiPointT8X = multipoin8X,
                            MultiPointT8Y = multipoin8Y,
                            MultiPointT9X = multipoin9X,
                            MultiPointT9Y = multipoin9Y,
                            MultiPointT10X = multipoin10X,
                            MultiPointT10Y = multipoin10Y,
                            MultiPointT11X = multipoin11X,
                            MultiPointT11Y = multipoin11Y,
                            MultiPointT12X = multipoin12X,
                            MultiPointT12Y = multipoin12Y,
                            MultiPointT13X = multipoin13X,
                            MultiPointT13Y = multipoin13Y,
                            MultiPointT14X = multipoin14X,
                            MultiPointT14Y = multipoin14Y,
                            MultiPointT15X = multipoin15X,
                            MultiPointT15Y = multipoin15Y,
                            MultiPointT16X = multipoin16X,
                            MultiPointT16Y = multipoin16Y,
                        });

                    }
                }

            }
        }

        private void ShowPopup()
        {
            var parameters = new DialogParameters("Create New Recipe");
            _dialogService.ShowDialog("CreateNewRecipe", parameters, result => { });
        }
        private void exUpdateRecipeParameter()
        {
            var parameters = new DialogParameters("Edit Recipe");
            _dialogService.ShowDialog("EditRecipe", parameters, result => {});

        }

        private void exDeleteRecipeParameter()
        {
            RcName = _appService.RecipeService.RecipeName;

            if (_configHandler.LoadXML(ConfigValue.AppSetting.RecipeConfigPath))
            {
                try
                {
                    if (_configHandler.RemoveNode(RcName))
                    {
                        _eventAggregator.GetEvent<RecipeUpdateDoneEvent>().Publish(true);
                    }
                    
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
                
            }
        }
    }
}
