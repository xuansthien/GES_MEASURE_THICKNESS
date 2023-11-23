using Prism.Commands;
using Prism.Mvvm;
using PORLA.HMI.Service;
using POLAR.CompositeAppCommand;
using Prism.Events;
using POLAR.PrecitecControl;
using System.Windows.Controls;
using System.Windows;
using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using PORLA.HMI.Service.Enums;
using System.Threading;

namespace PORLA.HMI.Module.ViewModels.SettingPages
{
    public class ManualPageViewModel : BindableBase
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Property
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
        public DelegateCommand PrecitecSendCmd { get; set; }
        public DelegateCommand<string> ReleaseVaccumOnOffCmd { get; private set; }
        public DelegateCommand TestEvent { get; private set; }

        private string _cmdInput;
        public string CmdInput
        {
            get { return _cmdInput; }
            set { SetProperty(ref _cmdInput, value); }
        }

        private int _precitecTabSelect;
        public int PrecitecTabSelect
        {
            get { return _precitecTabSelect; }
            set { SetProperty(ref _precitecTabSelect, value); }
        }
        private Visibility _tab1D;
        public Visibility Tab1D
        {
            get { return _tab1D; }
            set { SetProperty(ref _tab1D, value); }
        }
        private Visibility _tabFss;
        public Visibility TabFss
        {
            get { return _tabFss; }
            set { SetProperty(ref _tabFss, value); }
        }
        private Visibility _tabSendCmd;
        public Visibility TabSendCmd
        {
            get { return _tabSendCmd; }
            set { SetProperty(ref _tabSendCmd, value); }
        }
        #endregion

        #region Contructors
        public ManualPageViewModel(IAppService appService, ICompositeAppCommand compositeAppCommand)
        {
            _appService = appService;
            _appCommand = compositeAppCommand;
            PrecitecSendCmd = new DelegateCommand(exPrecitecSendCmd);
            PrecitecVisiblity();
        }

        

        private void PrecitecVisiblity()
        {
            if (AppService.PrecitecService.FssSelected)
            {
                PrecitecTabSelect = 1; // 0: Tab for 1D sensor, 1: Tab for FSS
                Tab1D = Visibility.Collapsed;
                TabFss = Visibility.Visible;
                TabSendCmd = Visibility.Visible;
            }
            if (AppService.PrecitecService.OneDSelected)
            {
                PrecitecTabSelect = 0; // 0: Tab for 1D sensor, 1: Tab for FSS
                Tab1D = Visibility.Visible;
                TabFss = Visibility.Collapsed;
                TabSendCmd = Visibility.Visible;
            }
        }

        private void exPrecitecSendCmd()
        {
            if (AppService.PrecitecService.FssSelected)
            {
                AppCommand.cmdFssSendCmd.Execute(CmdInput);
            }
            if (AppService.PrecitecService.OneDSelected)
            {
                AppCommand.cmdOneDSendCmd.Execute(CmdInput);
            }
        }

        #endregion
    }
}
