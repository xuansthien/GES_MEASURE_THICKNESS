using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;

namespace PORLA.HMI.Module.ViewModels.SettingPages
{
    public class ShowDialogViewModel : BindableBase, IDialogAware
    {
        #region Property
        public string Title => "New RecipeID";

        private string _parameter1;
        public string Parameter1
        {
            get { return _parameter1; }
            set { SetProperty(ref _parameter1, value); }
        }

        private string _parameter2;
        public string Parameter2
        {
            get { return _parameter2; }
            set { SetProperty(ref _parameter2, value); }
        }

        private string _parameter3;
        public string Parameter3
        {
            get { return _parameter3; }
            set { SetProperty(ref _parameter3, value); }
        }

        private string _parameter4;
        public string Parameter4
        {
            get { return _parameter4; }
            set { SetProperty(ref _parameter4, value); }
        }

        private string _parameter5;
        public string Parameter5
        {
            get { return _parameter5; }
            set { SetProperty(ref _parameter5, value); }
        }
        private string _parameter6;
        public string Parameter6
        {
            get { return _parameter6; }
            set { SetProperty(ref _parameter6, value); }
        }
        private string _parameter7;
        public string Parameter7
        {
            get { return _parameter7; }
            set { SetProperty(ref _parameter7, value); }
        }
        private string _parameter8;
        public string Parameter8
        {
            get { return _parameter8; }
            set { SetProperty(ref _parameter8, value); }
        }
        private string _parameter9;
        public string Parameter9
        {
            get { return _parameter9; }
            set { SetProperty(ref _parameter9, value); }
        }
        private string _parameter10;
        public string Parameter10
        {
            get { return _parameter10; }
            set { SetProperty(ref _parameter10, value); }
        }


        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CloseDialogCommand { get; }

        public event Action<IDialogResult> RequestClose;

        #endregion

        public ShowDialogViewModel()
        {
            SaveCommand = new DelegateCommand(SaveData);
            CloseDialogCommand = new DelegateCommand(CloseDialog);
        }

        private void SaveData()
        {
            ObservableCollection<string> dataList = new ObservableCollection<string>();
            dataList.Add(Parameter1);
            dataList.Add(Parameter2);
            dataList.Add(Parameter3);
            dataList.Add(Parameter4);
            dataList.Add(Parameter5);
            dataList.Add(Parameter6);
            dataList.Add(Parameter7);
            dataList.Add(Parameter8);
            dataList.Add(Parameter9);
            dataList.Add(Parameter10);
          
        }

        private void CloseDialog()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

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
