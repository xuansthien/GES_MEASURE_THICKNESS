using POLAR.CompositeAppCommand;
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PORLA.HMI.Module.ViewModels.Dialogs
{
    public class EditRecipeViewModel : BindableBase, IDialogAware
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        IEventAggregator _eventAggregator;
        private SubscriptionToken _subscriptionToken;

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

        private RecipeParameterModel _recipeParameter;
        public RecipeParameterModel RecipeParameter
        {
            get { return _recipeParameter; }
            set { SetProperty(ref _recipeParameter, value); }
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
        private void CloseDialog(string parameter)
        {
            ButtonResult result = ButtonResult.None;
            if (parameter?.ToLower() == "save")
            {
                RecipeParameter = new RecipeParameterModel
                {
                    RecipeName = AppService.RecipeService.RecipeName,
                    XOriginalPosition = AppService.RecipeService.XOriginalPosition,
                    YOriginalPosition = AppService.RecipeService.YOriginalPosition,
                    DXPosition = AppService.RecipeService.DXPosition,
                    DYPosition = AppService.RecipeService.DYPosition,
                    RXPosition = AppService.RecipeService.RXPosition,
                    RYPosition = AppService.RecipeService.RYPosition,
                    SpeedAxisX = AppService.RecipeService.SpeedAxisX,
                    SpeedAxisY = AppService.RecipeService.SpeedAxisY,
                    DWDLeft = AppService.RecipeService.DWDLeft,
                    DWDRight = AppService.RecipeService.DWDRight,
                    QTH = AppService.RecipeService.QTH
                    //SensorType = GetStringFrom("sensor"),
                    //ThicknessSelection = GetStringFrom("thickness")
                };
                SaveIntoRecipeList(RecipeParameter);
                _eventAggregator.GetEvent<RecipeUpdateDoneEvent>().Publish(true);
                RaiseRequestClose(new DialogResult(result));
            }

            if (parameter?.ToLower() == "cancel")
                result = ButtonResult.Cancel;

            RaiseRequestClose(new DialogResult(result));
        }

        private void SaveIntoRecipeList(RecipeParameterModel recipeParameter)
        {
            if (_configHandler.LoadXML(ConfigValue.AppSetting.RecipeConfigPath))
            {
                try
                {
                    if (_configHandler.CheckNode(ConfigValue.RecipeSetting.SelectNode, ConfigValue.RecipeSetting.RecipeName, RcName))
                    {
                        _configHandler.UpdateInnerSingleNode<string>(ConfigValue.RecipeSetting.RecipeName, recipeParameter.RecipeName);
                        //_configHandler.UpdateInnerSingleNode<string>(ConfigValue.RecipeSetting.SensorType, recipeParameter.SensorType);
                        //_configHandler.UpdateInnerSingleNode<string>(ConfigValue.RecipeSetting.ThicknessSelection, recipeParameter.ThicknessSelection);
                        _configHandler.UpdateInnerSingleNode<string>(ConfigValue.RecipeSetting.XOriginPosition, recipeParameter.XOriginalPosition);
                        _configHandler.UpdateInnerSingleNode<string>(ConfigValue.RecipeSetting.YOriginPosition, recipeParameter.YOriginalPosition);
                        _configHandler.UpdateInnerSingleNode<string>(ConfigValue.RecipeSetting.DXPosition, recipeParameter.DXPosition);
                        _configHandler.UpdateInnerSingleNode<string>(ConfigValue.RecipeSetting.DYPosition, recipeParameter.DYPosition);
                        _configHandler.UpdateInnerSingleNode<string>(ConfigValue.RecipeSetting.RXPosition, recipeParameter.RXPosition);
                        _configHandler.UpdateInnerSingleNode<string>(ConfigValue.RecipeSetting.RYPosition, recipeParameter.RYPosition);
                        _configHandler.UpdateInnerSingleNode<string>(ConfigValue.RecipeSetting.SpeedAxisX, recipeParameter.SpeedAxisX);
                        _configHandler.UpdateInnerSingleNode<string>(ConfigValue.RecipeSetting.SpeedAxisY, recipeParameter.SpeedAxisY);
                        _configHandler.UpdateInnerSingleNode<string>(ConfigValue.RecipeSetting.DWDLeftSide, recipeParameter.DWDLeft);
                        _configHandler.UpdateInnerSingleNode<string>(ConfigValue.RecipeSetting.DWDRightSide, recipeParameter.DWDRight);
                        _configHandler.UpdateInnerSingleNode<string>(ConfigValue.RecipeSetting.QualityThreshold, recipeParameter.QTH);
                        _configHandler.SaveXmlFile();
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);   
                }
            }
        }

        private void RaiseRequestClose(DialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        private ObservableCollection<RecipeParameterModel> _recipeDetail;
        public ObservableCollection<RecipeParameterModel> RecipeDetail
        {
            get { return _recipeDetail; }
            set { SetProperty(ref _recipeDetail, value); }
        }
        private RecipeParameterModel _editParameter;
        public RecipeParameterModel EditParameter
        {
            get { return _editParameter; }
            set { SetProperty(ref _editParameter, value); }
        }

        private string _rcName = "";
        public string RcName
        {
            get { return _rcName; }
            set { SetProperty(ref _rcName, value); }
        }
        
        public EditRecipeViewModel(ICompositeAppCommand appCommand, IAppService appService,
            IEventAggregator eventAggregator, IConfigHandler configHandler)
        {
            _eventAggregator = eventAggregator;
            _configHandler = configHandler;
            _appCommand = appCommand;
            _appService = appService;

            RecipeDetail = new ObservableCollection<RecipeParameterModel>();

            _subscriptionToken = _eventAggregator.GetEvent<RecipeItemSelectedEvent>().Subscribe(OnRecipeUpdate);
            RcName = _appService.RecipeService.RecipeName;
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

        private void OnRecipeUpdate(RecipeParameterModel obj)
        {
            try
            {
                RcName = obj.RecipeName;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            //RecipeDetail.Clear();
            //RecipeDetail.Add(obj);
        }

        public string Title => "Edit Recipe";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            
        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
            ;
        }
    }
}
