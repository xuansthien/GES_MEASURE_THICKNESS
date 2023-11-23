using PORLA.HMI.Service;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZstdSharp.Unsafe;

namespace PORLA.HMI.Main.ViewModels.Dialog
{
    public class SelectPrecitecSensorDialogViewModel : BindableBase, IDialogAware
    {
        private IAppService _appService;
        public IAppService AppService
        {
            get { return _appService; }
            set { SetProperty(ref _appService, value); }
        }
        private DelegateCommand<string> _closeDialogCommand;
        public DelegateCommand<string> CloseDialogCommand =>
            _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));

        private bool _isFss;
        public bool IsFss
        {
            get { return _isFss; }
            set 
            { 
                SetProperty(ref _isFss, value);
                if (_isFss)
                {
                    IsOneD = false;
                    IsOneDMultiPoints = false;
                }    
            }
        }
        private bool _isOneD;
        public bool IsOneD
        {
            get { return _isOneD; }
            set 
            { 
                SetProperty(ref _isOneD, value);
                if (_isOneD)
                {
                    IsFss = false;
                    IsOneDMultiPoints = false;
                }
            }
        }
        private bool _isOneDMultiPoints;
        public bool IsOneDMultiPoints
        {
            get { return _isOneDMultiPoints; }
            set
            {
                SetProperty(ref _isOneDMultiPoints, value);
                if (_isOneDMultiPoints)
                {
                    IsFss = false;
                    IsOneD = false;
                }

            }
        }
        private void CloseDialog(string obj)
        {
            ButtonResult result = ButtonResult.None;
            bool _isChecked = false;
            if (IsFss != _isChecked || IsOneD != _isChecked || IsOneDMultiPoints != _isChecked)
            {
                AppService.PrecitecService.FssSelected = IsFss;
                AppService.PrecitecService.OneDSelected = IsOneD;
                AppService.PrecitecService.OneDMPSelected = IsOneDMultiPoints;
                RaiseRequestClose(new DialogResult(result));
                return;
            }
        }

        private void RaiseRequestClose(DialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        public SelectPrecitecSensorDialogViewModel(IAppService appService)
        {
            _appService = appService;   
        }

        public string Title => "Select Proper Precitec Sensor";

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
