using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Config;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using PORLA.HMI.Service;
using System.Windows;
using System;
using LiveCharts.Configurations;

namespace PORLA.HMI.Module.ViewModels.LogPagess
{
    public class MachineLogPageViewModel : BindableBase
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IAppService _appService;
        public IAppService AppService
        {
            get { return _appService; }
            set { SetProperty(ref _appService, value); }
        }

        public MachineLogPageViewModel(IAppService appService)
        {
            AppService = appService;
        }
    }
}
