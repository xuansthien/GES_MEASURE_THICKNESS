using POLAR.CompositeAppCommand;
using PORLA.HMI.Module.Enums;
using PORLA.HMI.Module.Events;
using PORLA.HMI.Module.Model;
using PORLA.HMI.Service;
using PORLA.HMI.Service.Configuration;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PORLA.HMI.Module.ViewModels.Dialogs
{
    public class CreateNewRecipeViewModel : BindableBase, IDialogAware
    {
        #region Properties
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        IEventAggregator _eventAggregator;

        IConfigHandler _configHandler;

        private ICompositeAppCommand _appCommand;
        public ICompositeAppCommand AppCommand
        {
            get { return _appCommand; }
            set { SetProperty(ref _appCommand, value); }
        }

        private IAppService _appService;
        public IAppService AppService
        {
            get { return _appService; }
            set { SetProperty(ref _appService, value); }
        }

        private DelegateCommand<string> _closeDialogCommand;
        public DelegateCommand<string> CloseDialogCommand =>
            _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));

        private DelegateCommand<string> _teachPositionCommand;
        public DelegateCommand<string> TeachPositionCommand =>
            _teachPositionCommand ?? (_teachPositionCommand = new DelegateCommand<string>(ExTeachPositionCommand));

        private RecipeParameterModel _recipeParameter;
        public RecipeParameterModel RecipeParameter
        {
            get { return _recipeParameter; }
            set { SetProperty(ref _recipeParameter, value); }
        }
        private string _recipeNameInput;
        public string RecipeNameInput
        {
            get { return _recipeNameInput; }
            set { SetProperty(ref _recipeNameInput, value); }
        }

        private string _xOriginalPosInput;
        public string XOriginalPosInput
        {
            get { return _xOriginalPosInput; }
            set { SetProperty(ref _xOriginalPosInput, value); }
        }
        private string _yOriginalPosInput;
        public string YOriginalPosInput
        {
            get { return _yOriginalPosInput; }
            set { SetProperty(ref _yOriginalPosInput, value); }
        }
        private string _dXPositionInput;
        public string DXPositionInput
        {
            get { return _dXPositionInput; }
            set { SetProperty(ref _dXPositionInput, value); }
        }
        private string _dYPositionInput;
        public string DYPositionInput
        {
            get { return _dYPositionInput; }
            set { SetProperty(ref _dYPositionInput, value); }
        }
        private string _rXPositionInput;
        public string RXPositionInput
        {
            get { return _rXPositionInput; }
            set { SetProperty(ref _rXPositionInput, value); }
        }
        private string _rYPositionInput;
        public string RYPositionInput
        {
            get { return _rYPositionInput; }
            set { SetProperty(ref _rYPositionInput, value); }
        }
        private string _speedAxisXInput;
        public string SpeedAxisXInput
        {
            get { return _speedAxisXInput; }
            set { SetProperty(ref _speedAxisXInput, value); }
        }
        private string _speedAxisYInput;
        public string SpeedAxisYInput
        {
            get { return _speedAxisYInput; }
            set { SetProperty(ref _speedAxisYInput, value); }
        }
        private string _dWDLeftInput;
        public string DWDLeftInput
        {
            get { return _dWDLeftInput; }
            set { SetProperty(ref _dWDLeftInput, value); }
        }
        private string _dWDRightInput;
        public string DWDRightInput
        {
            get { return _dWDRightInput; }
            set { SetProperty(ref _dWDRightInput, value); }
        }

        private string _qTHInput;
        public string QTHInput
        {
            get { return _qTHInput; }
            set { SetProperty(ref _qTHInput, value); }
        }

        private Visibility _is1DMultiPoints;
        public Visibility Is1DMultiPoints
        {
            get { return _is1DMultiPoints; }
            set { SetProperty(ref _is1DMultiPoints, value); }
        }
        private Visibility _isFssOr1D;
        public Visibility IsFssOr1D
        {
            get { return _isFssOr1D; }
            set { SetProperty(ref _isFssOr1D, value); }
        }

        private string _rXRecipeLbl;
        public string RXRecipeLbl
        {
            get { return _rXRecipeLbl; }
            set { SetProperty(ref _rXRecipeLbl, value); }
        }
        private string _rYRecipeLbl;
        public string RYRecipeLbl
        {
            get { return _rYRecipeLbl; }
            set { SetProperty(ref _rYRecipeLbl, value); }
        }
        #endregion
        #region Multipoint Properties
        private string _multiPointT1XInput;
        public string MultiPointT1XInput
        {
            get { return _multiPointT1XInput; }
            set { SetProperty(ref _multiPointT1XInput, value); }
        }
        private string _multiPointT1YInput;
        public string MultiPointT1YInput
        {
            get { return _multiPointT1YInput; }
            set { SetProperty(ref _multiPointT1YInput, value); }
        }
        private string _multiPointT2XInput;
        public string MultiPointT2XInput
        {
            get { return _multiPointT2XInput; }
            set { SetProperty(ref _multiPointT2XInput, value); }
        }
        private string _multiPointT2YInput;
        public string MultiPointT2YInput
        {
            get { return _multiPointT2YInput; }
            set { SetProperty(ref _multiPointT2YInput, value); }
        }
        private string _multiPointT3XInput;
        public string MultiPointT3XInput
        {
            get { return _multiPointT3XInput; }
            set { SetProperty(ref _multiPointT3XInput, value); }
        }
        private string _multiPointT3YInput;
        public string MultiPointT3YInput
        {
            get { return _multiPointT3YInput; }
            set { SetProperty(ref _multiPointT3YInput, value); }
        }
        private string _multiPointT4XInput;
        public string MultiPointT4XInput
        {
            get { return _multiPointT4XInput; }
            set { SetProperty(ref _multiPointT4XInput, value); }
        }
        private string _multiPointT4YInput;
        public string MultiPointT4YInput
        {
            get { return _multiPointT4YInput; }
            set { SetProperty(ref _multiPointT4YInput, value); }
        }
        private string _multiPointT5XInput;
        public string MultiPointT5XInput
        {
            get { return _multiPointT5XInput; }
            set { SetProperty(ref _multiPointT5XInput, value); }
        }
        private string _multiPointT5YInput;
        public string MultiPointT5YInput
        {
            get { return _multiPointT5YInput; }
            set { SetProperty(ref _multiPointT5YInput, value); }
        }
        private string _multiPointT6XInput;
        public string MultiPointT6XInput
        {
            get { return _multiPointT6XInput; }
            set { SetProperty(ref _multiPointT6XInput, value); }
        }
        private string _multiPointT6YInput;
        public string MultiPointT6YInput
        {
            get { return _multiPointT6YInput; }
            set { SetProperty(ref _multiPointT6YInput, value); }
        }
        private string _multiPointT7XInput;
        public string MultiPointT7XInput
        {
            get { return _multiPointT7XInput; }
            set { SetProperty(ref _multiPointT7XInput, value); }
        }
        private string _multiPointT7YInput;
        public string MultiPointT7YInput
        {
            get { return _multiPointT7YInput; }
            set { SetProperty(ref _multiPointT7YInput, value); }
        }
        private string _multiPointT8XInput;
        public string MultiPointT8XInput
        {
            get { return _multiPointT8XInput; }
            set { SetProperty(ref _multiPointT8XInput, value); }
        }
        private string _multiPointT8YInput;
        public string MultiPointT8YInput
        {
            get { return _multiPointT8YInput; }
            set { SetProperty(ref _multiPointT8YInput, value); }
        }
        private string _multiPointT9XInput;
        public string MultiPointT9XInput
        {
            get { return _multiPointT9XInput; }
            set { SetProperty(ref _multiPointT9XInput, value); }
        }
        private string _multiPointT9YInput;
        public string MultiPointT9YInput
        {
            get { return _multiPointT9YInput; }
            set { SetProperty(ref _multiPointT9YInput, value); }
        }
        private string _multiPointT10XInput;
        public string MultiPointT10XInput
        {
            get { return _multiPointT10XInput; }
            set { SetProperty(ref _multiPointT10XInput, value); }
        }
        private string _multiPointT10YInput;
        public string MultiPointT10YInput
        {
            get { return _multiPointT10YInput; }
            set { SetProperty(ref _multiPointT10YInput, value); }
        }
        private string _multiPointT11XInput;
        public string MultiPointT11XInput
        {
            get { return _multiPointT11XInput; }
            set { SetProperty(ref _multiPointT11XInput, value); }
        }
        private string _multiPointT11YInput;
        public string MultiPointT11YInput
        {
            get { return _multiPointT11YInput; }
            set { SetProperty(ref _multiPointT11YInput, value); }
        }
        private string _multiPointT12XInput;
        public string MultiPointT12XInput
        {
            get { return _multiPointT12XInput; }
            set { SetProperty(ref _multiPointT12XInput, value); }
        }
        private string _multiPointT12YInput;
        public string MultiPointT12YInput
        {
            get { return _multiPointT12YInput; }
            set { SetProperty(ref _multiPointT12YInput, value); }
        }
        private string _multiPointT13XInput;
        public string MultiPointT13XInput
        {
            get { return _multiPointT13XInput; }
            set { SetProperty(ref _multiPointT13XInput, value); }
        }
        private string _multiPointT13YInput;
        public string MultiPointT13YInput
        {
            get { return _multiPointT13YInput; }
            set { SetProperty(ref _multiPointT13YInput, value); }
        }
        private string _multiPointT14XInput;
        public string MultiPointT14XInput
        {
            get { return _multiPointT14XInput; }
            set { SetProperty(ref _multiPointT14XInput, value); }
        }
        private string _multiPointT14YInput;
        public string MultiPointT14YInput
        {
            get { return _multiPointT14YInput; }
            set { SetProperty(ref _multiPointT14YInput, value); }
        }
        private string _multiPointT15XInput;
        public string MultiPointT15XInput
        {
            get { return _multiPointT15XInput; }
            set { SetProperty(ref _multiPointT15XInput, value); }
        }
        private string _multiPointT15YInput;
        public string MultiPointT15YInput
        {
            get { return _multiPointT15YInput; }
            set { SetProperty(ref _multiPointT15YInput, value); }
        }
        private string _multiPointT16XInput;
        public string MultiPointT16XInput
        {
            get { return _multiPointT16XInput; }
            set { SetProperty(ref _multiPointT16XInput, value); }
        }
        private string _multiPointT16YInput;
        public string MultiPointT16YInput
        {
            get { return _multiPointT16YInput; }
            set { SetProperty(ref _multiPointT16YInput, value); }
        }
        #endregion
        private void ExTeachPositionCommand(string axis)
        {
            if (axis == "AxisX")
            {
                //AppService.RecipeService.XOriginalPosition = AppService.IAIMotion.XCoordinate;
                XOriginalPosInput = AppService.IAIMotion.XCoordinate;
            }
            if (axis == "AxisY")
            {
                //AppService.RecipeService.YOriginalPosition = AppService.IAIMotion.YCoordinate;
                YOriginalPosInput = AppService.IAIMotion.YCoordinate;
            }
        }

        private void CloseDialog(string parameter)
        {
            ButtonResult result = ButtonResult.None;
            if (parameter?.ToLower() == "save")
            {
                if (AppService.PrecitecService.OneDMPSelected)
                {
                    RecipeParameter = new RecipeParameterModel
                    {
                        RecipeName = RecipeNameInput,
                        XOriginalPosition = XOriginalPosInput,
                        YOriginalPosition = YOriginalPosInput,
                        DXPosition = DXPositionInput,
                        DYPosition = DYPositionInput,
                        RXPosition = RXPositionInput,
                        RYPosition = RYPositionInput,
                        SpeedAxisX = SpeedAxisXInput,
                        SpeedAxisY = SpeedAxisYInput,
                        SensorType = GetStringFrom("sensor"),
                        ThicknessSelection = GetStringFrom("thickness"),
                        DWDLeft = DWDLeftInput,
                        DWDRight = DWDRightInput,
                        QTH = QTHInput,
                        MultiPointT1X = MultiPointT1XInput,
                        MultiPointT1Y = MultiPointT1YInput,
                        MultiPointT2X = MultiPointT2XInput,
                        MultiPointT2Y = MultiPointT2YInput,
                        MultiPointT3X = MultiPointT3XInput,
                        MultiPointT3Y = MultiPointT3YInput,
                        MultiPointT4X = MultiPointT4XInput,
                        MultiPointT4Y = MultiPointT4YInput,
                        MultiPointT5X = MultiPointT5XInput,
                        MultiPointT5Y = MultiPointT5YInput,
                        MultiPointT6X = MultiPointT6XInput,
                        MultiPointT6Y = MultiPointT6YInput,
                        MultiPointT7X = MultiPointT7XInput,
                        MultiPointT7Y = MultiPointT7YInput,
                        MultiPointT8X = MultiPointT8XInput,
                        MultiPointT8Y = MultiPointT8YInput,
                        MultiPointT9X = MultiPointT9XInput,
                        MultiPointT9Y = MultiPointT9YInput,
                        MultiPointT10X = MultiPointT10XInput,
                        MultiPointT10Y = MultiPointT10YInput,
                        MultiPointT11X = MultiPointT11XInput,
                        MultiPointT11Y = MultiPointT11YInput,
                        MultiPointT12X = MultiPointT12XInput,
                        MultiPointT12Y = MultiPointT12YInput,
                        MultiPointT13X = MultiPointT13XInput,
                        MultiPointT13Y = MultiPointT13YInput,
                        MultiPointT14X = MultiPointT14XInput,
                        MultiPointT14Y = MultiPointT14YInput,
                        MultiPointT15X = MultiPointT15XInput,
                        MultiPointT15Y = MultiPointT15YInput,
                        MultiPointT16X = MultiPointT16XInput,
                        MultiPointT16Y = MultiPointT16YInput
                    };
                    SaveIntoRecipeList(RecipeParameter);
                    _eventAggregator.GetEvent<RecipeCreateNewEvent>().Publish(RecipeParameter);
                }
                else
                {
                    RecipeParameter = new RecipeParameterModel
                    {
                        RecipeName = RecipeNameInput,
                        XOriginalPosition = XOriginalPosInput,
                        YOriginalPosition = YOriginalPosInput,
                        DXPosition = DXPositionInput,
                        DYPosition = DYPositionInput,
                        RXPosition = RXPositionInput,
                        RYPosition = RYPositionInput,
                        SpeedAxisX = SpeedAxisXInput,
                        SpeedAxisY = SpeedAxisYInput,
                        SensorType = GetStringFrom("sensor"),
                        ThicknessSelection = GetStringFrom("thickness"),
                        DWDLeft = DWDLeftInput,
                        DWDRight = DWDRightInput,
                        QTH = QTHInput
                    };
                    SaveIntoRecipeList(RecipeParameter);
                    _eventAggregator.GetEvent<RecipeCreateNewEvent>().Publish(RecipeParameter);
                }
                
                RaiseRequestClose(new DialogResult(result));
            }

            if (parameter?.ToLower() == "cancel")
                result = ButtonResult.Cancel;

            RaiseRequestClose(new DialogResult(result));
        }

        private void SaveIntoRecipeList(RecipeParameterModel recipeParameter)
        {
            if(_configHandler.LoadXML(ConfigValue.AppSetting.RecipeConfigPath))
            {
                if(_configHandler.CreateNewHeaderElement(ConfigValue.RecipeSetting.Header))
                {
                    if (AppService.PrecitecService.OneDMPSelected)
                    {
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.RecipeName, recipeParameter.RecipeName);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.SensorType, recipeParameter.SensorType);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.ThicknessSelection, recipeParameter.ThicknessSelection);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.XOriginPosition, recipeParameter.XOriginalPosition);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.YOriginPosition, recipeParameter.YOriginalPosition);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.DXPosition, recipeParameter.DXPosition);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.DYPosition, recipeParameter.DYPosition);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.RXPosition, recipeParameter.RXPosition);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.RYPosition, recipeParameter.RYPosition);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.SpeedAxisX, recipeParameter.SpeedAxisX);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.SpeedAxisY, recipeParameter.SpeedAxisY);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.DWDLeftSide, recipeParameter.DWDLeft);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.DWDRightSide, recipeParameter.DWDRight);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.QualityThreshold, recipeParameter.QTH);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T1X, recipeParameter.MultiPointT1X);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T1Y, recipeParameter.MultiPointT1Y);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T2X, recipeParameter.MultiPointT2X);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T2Y, recipeParameter.MultiPointT2Y);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T3X, recipeParameter.MultiPointT3X);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T3Y, recipeParameter.MultiPointT3Y);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T4X, recipeParameter.MultiPointT4X);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T4Y, recipeParameter.MultiPointT4Y);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T5X, recipeParameter.MultiPointT5X);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T5Y, recipeParameter.MultiPointT5Y);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T6X, recipeParameter.MultiPointT6X);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T6Y, recipeParameter.MultiPointT6Y);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T7X, recipeParameter.MultiPointT7X);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T7Y, recipeParameter.MultiPointT7Y);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T8X, recipeParameter.MultiPointT8X);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T8Y, recipeParameter.MultiPointT8Y);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T9X, recipeParameter.MultiPointT9X);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T9Y, recipeParameter.MultiPointT9Y);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T10X, recipeParameter.MultiPointT10X);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T10Y, recipeParameter.MultiPointT10Y);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T11X, recipeParameter.MultiPointT11X);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T11Y, recipeParameter.MultiPointT11Y);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T12X, recipeParameter.MultiPointT12X);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T12Y, recipeParameter.MultiPointT12Y);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T13X, recipeParameter.MultiPointT13X);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T13Y, recipeParameter.MultiPointT13Y);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T14X, recipeParameter.MultiPointT14X);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T14Y, recipeParameter.MultiPointT14Y);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T15X, recipeParameter.MultiPointT15X);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T15Y, recipeParameter.MultiPointT15Y);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T16X, recipeParameter.MultiPointT16X);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.T16Y, recipeParameter.MultiPointT16Y);
                    }
                    else
                    {
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.RecipeName, recipeParameter.RecipeName);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.SensorType, recipeParameter.SensorType);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.ThicknessSelection, recipeParameter.ThicknessSelection);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.XOriginPosition, recipeParameter.XOriginalPosition);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.YOriginPosition, recipeParameter.YOriginalPosition);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.DXPosition, recipeParameter.DXPosition);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.DYPosition, recipeParameter.DYPosition);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.RXPosition, recipeParameter.RXPosition);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.RYPosition, recipeParameter.RYPosition);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.SpeedAxisX, recipeParameter.SpeedAxisX);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.SpeedAxisY, recipeParameter.SpeedAxisY);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.DWDLeftSide, recipeParameter.DWDLeft);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.DWDRightSide, recipeParameter.DWDRight);
                        _configHandler.CreateNewElement<string>(ConfigValue.RecipeSetting.QualityThreshold, recipeParameter.QTH);

                    }
                }    
            }    
            
        }

        private string GetStringFrom(string input)
        {
            string output = string.Empty;
            if (input == "sensor")
            {
                if (AppService.PrecitecService.FssSelected)
                {
                    output = RecipeConstants.FSS;
                }
                if (AppService.PrecitecService.OneDSelected)
                {
                    output = RecipeConstants.SinglePoint;
                }
                if (AppService.PrecitecService.OneDMPSelected)
                {
                    output = RecipeConstants.MultiPoint;
                }
            }
            if (input == "thickness")
            {
                if (AppService.RecipeService.OCAThichknessSelect)
                {
                    //output = RecipeConstants.OCA;
                    output = "8 Layers";
                }
                if (AppService.RecipeService.PolarizerThichknessSelect)
                {
                    //output = RecipeConstants.Polarizer;
                    output = "8 Layers";
                }
                if (AppService.PrecitecService.OneDMPSelected)
                {
                    output = "8 Layers";
                }
                output = "8 Layers";
            }
            return output;
        }

        private void RaiseRequestClose(DialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        public CreateNewRecipeViewModel(ICompositeAppCommand appCommand, IAppService appService, 
            IEventAggregator eventAggregator, IConfigHandler configHandler)
        {
            _eventAggregator = eventAggregator;
            _configHandler = configHandler; 
            _appCommand = appCommand;
            _appService = appService;
            RecipeVisibility();
        }

        private void RecipeVisibility()
        {
            if (AppService.PrecitecService.OneDMPSelected)
            {
                Is1DMultiPoints = Visibility.Visible;
                IsFssOr1D = Visibility.Hidden;
            }
            else
            {
                IsFssOr1D = Visibility.Visible;
                Is1DMultiPoints = Visibility.Hidden;
                if (AppService.PrecitecService.FssSelected)
                {
                    RXRecipeLbl = "No. of Points";
                    RYRecipeLbl = "No. of Lines";
                }
                if (AppService.PrecitecService.OneDSelected)
                {
                    RXRecipeLbl = "RX";
                    RYRecipeLbl = "RX";
                }
            }
            
        }

        public string Title => "Create New Recipe";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            
        }
    }
}
