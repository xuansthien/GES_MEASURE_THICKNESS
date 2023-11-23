using PORLA.HMI.Service;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Forms;
using PORLA.HMI.Service.Enums;
using PORLA.HMI.Module.DataService.DataHandle;
using Prism.Events;
using POLAR.EventAggregator;

namespace PORLA.HMI.Main.ViewModels
{
    public class ToppanelViewModel : BindableBase
    {
        private HardwareConnectionState _connectionState;
        public HardwareConnectionState ConnectionState
        {
            get { return _connectionState; }
            set { SetProperty(ref _connectionState, value); }
        }

        private IAppService _appServiceInstance;
        public IAppService AppServiceInstance
        {
            get { return _appServiceInstance; }
            set { SetProperty(ref _appServiceInstance, value); }
        }

        private IDataHandler _dataHandler;
        public IDataHandler DataHandler
        {
            get { return _dataHandler; }
            set { SetProperty(ref _dataHandler, value); }
        }

        IEventAggregator _eventAggregator;

        private string _hour = "0";
        public string Hour
        {
            get { return this._hour; }
            set
            {
                if (!string.Equals(this._hour, value))
                {
                    _hour = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string _date = "0";
        public string Date
        {
            get { return _date; }
            set
            {
                if (!string.Equals(_date, value))
                {
                    _date = value;
                    RaisePropertyChanged();
                }
            }
        }   

        public DelegateCommand MinimizeWindowCommand { get; private set; }
        public DelegateCommand CloseWindowCommand { get; private set; }
        public DelegateCommand MaximizeWindowCommand { get; private set; }

        public ToppanelViewModel(IAppService appServiceModule, IDataHandler dataHandler, IEventAggregator eventAggregator)
        {
            _dataHandler = dataHandler;
            AppServiceInstance = appServiceModule;
            _eventAggregator = eventAggregator;

            MinimizeWindowCommand = new DelegateCommand(ExecuteMinimizeWindow);
            CloseWindowCommand = new DelegateCommand(ExecuteCloseWindow);
            MaximizeWindowCommand = new DelegateCommand(ExecuteMaximizeWindow);
            
            DispatcherTimer _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += timer_Tick;
            _timer.Start();

            InitializeDB();
        }

        private void InitializeDB()
        {
            if (DataHandler.InitializeDataHandler())
            {
                Logging(new LogInfor(LogType.Connection, $"Database Initialized!"));
            }
            else
            {
                Logging(new LogInfor(LogType.Connection, $"Database Initialize Failed!"));

            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Hour = DateTime.Now.ToString("HH:mm:ss");
            Date = DateTime.Now.ToString("MM/dd/yyyy");
        }       
        private void ExecuteMaximizeWindow()
        {
            foreach (Window window in System.Windows.Application.Current.Windows)
            {
                if (window != null)
                {
                    window.WindowState = WindowState.Maximized;
                }
            }
        }
        private void ExecuteCloseWindow()
        {
            if (System.Windows.Forms.MessageBox.Show("Do you want to Close HMI?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (Window window in System.Windows.Application.Current.Windows)
                {
                    if (window != null)
                    {
                        // need to close connection all device if connection is alive
                        _eventAggregator.GetEvent<CloseAppEvent>().Publish(true);
                        window.Close();
                    }
                }                 
            }
        }
        private void ExecuteMinimizeWindow()
        {
            foreach (Window window in System.Windows.Application.Current.Windows)
            {
                if (window != null)
                {
                    window.WindowState = WindowState.Minimized;
                }
            }
        }

        public void Logging(LogInfor _logData)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                AppServiceInstance.LogInfors.Add(_logData);
                AppServiceInstance.ScrollViewerVerticalOffset = AppServiceInstance.LogInfors.Count - 1;
            });
        }
    }
}
