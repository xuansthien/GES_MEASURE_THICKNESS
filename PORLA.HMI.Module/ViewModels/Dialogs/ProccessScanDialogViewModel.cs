using MySqlX.XDevAPI.Common;
using POLAR.EventAggregator;
using PORLA.HMI.Service;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Windows.Threading;

namespace PORLA.HMI.Module.ViewModels.Dialogs
{
    public class ProccessScanDialogViewModel : BindableBase, IDialogAware
    {
        IEventAggregator _eventAggregator;
        private DelegateCommand<string> _closeDialogCommand;
        public DelegateCommand<string> CloseDialogCommand =>
            _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));
        private readonly DispatcherTimer _timer;
        private int _count = 0;
        public int Count
        {
            get { return _count; }
            set { SetProperty(ref _count, value); }
        }

        private string _cycleTime = "0";

        public string CycleTime
        {
            get { return _cycleTime; }
            set { SetProperty(ref _cycleTime, value); }
        }
        private float _counter = 0;
        public ProccessScanDialogViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(200);
            _timer.Tick += timer_Tick;
            _eventAggregator.GetEvent<CloseProcessDialogEvent>().Subscribe(OnCloseProcessDialog, ThreadOption.UIThread);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            _counter++;
            CycleTime = ((_counter * 200)/1000).ToString();
        }

        private void OnCloseProcessDialog(bool obj)
        {
            _timer.Stop();
            RaiseRequestClose(new DialogResult(ButtonResult.OK));
        }

        private void CloseDialog(string parameter)
        {
            ButtonResult result = ButtonResult.None;
            if (parameter?.ToLower() == "save")
            {
                RaiseRequestClose(new DialogResult(result));
            }

            if (parameter?.ToLower() == "cancel")
                result = ButtonResult.Cancel;

            RaiseRequestClose(new DialogResult(result));
        }

        public string Title => "Processing...";

        public event Action<IDialogResult> RequestClose;
        private void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
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
            _timer.Start();
            string sensorType = parameters.GetValue<string>("parameter");
            _eventAggregator.GetEvent<RaiseProcessScanEvent>().Publish(sensorType);
        }
    }
}
