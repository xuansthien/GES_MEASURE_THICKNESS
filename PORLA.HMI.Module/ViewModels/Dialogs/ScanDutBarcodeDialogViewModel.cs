using MySqlX.XDevAPI.Common;
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

namespace PORLA.HMI.Module.ViewModels.Dialogs
{
    public class ScanDutBarcodeDialogViewModel : BindableBase, IDialogAware
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
        public ScanDutBarcodeDialogViewModel(IAppService appService)
        {
            _appService = appService;
        }

        private void CloseDialog(string obj)
        {
            ButtonResult result = ButtonResult.None;
            if (obj?.ToLower() == "ok") 
            {
                if (!string.IsNullOrEmpty(AppService.DutBarcode))
                {
                    RaiseRequestClose(new DialogResult(result));
                    return;
                }
                System.Windows.MessageBox.Show("Please Scan Barcode", "Warning",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            if (obj?.ToLower() == "cancel")
                result = ButtonResult.Cancel;

            RaiseRequestClose(new DialogResult(result));
        }

        public string Title => "Scan Dut Barcode";

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
        private void RaiseRequestClose(DialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }
    }
}
