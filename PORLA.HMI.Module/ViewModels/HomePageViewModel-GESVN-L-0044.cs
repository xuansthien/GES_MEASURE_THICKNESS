using PORLA.HMI.Module.Model;
using PORLA.HMI.Service;
using Org.BouncyCastle.Asn1.Mozilla;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.UI.WebControls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows;
using Prism.Services.Dialogs;
using System.Security.AccessControl;
using System.Windows.Forms;
using System.ComponentModel;
using log4net;

namespace PORLA.HMI.Module.ViewModels
{
    
    public class HomePageViewModel : BindableBase
    {
        #region Property
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IAppService _appService;
        public IAppService AppService
        {
            get { return _appService; }
            set { SetProperty(ref _appService, value); }
        }

        private ILoggerService _loggerService;

        private int _totalDut = 0;
        public int TotalDut
        {
            get { return _totalDut; }
            set { SetProperty(ref _totalDut, value); }
        }
        private bool _auto = false;
        public bool Auto
        {
            get { return _auto; }
            set
            {
                SetProperty(ref _auto, value);
            }
        }

        public DelegateCommand AutoCmd { get; private set; }
        public DelegateCommand ManualCmd { get; private set; }
        public DelegateCommand InitCmd { get; private set; }
        public DelegateCommand StartCmd { get; private set; }
        public DelegateCommand StopCmd { get; private set; }
        public DelegateCommand ResetCmd { get; private set; }
        public DelegateCommand ClearCmd { get; private set; }
        
        #endregion
        #region Constructors
        public HomePageViewModel(IAppService appService, ILoggerService loggerService)
        {
            _loggerService = loggerService;
            _appService = appService;

            AutoCmd = new DelegateCommand(() => ExAuto());
            ManualCmd = new DelegateCommand(() => ExManual());
            InitCmd = new DelegateCommand(() => ExInitialize());
            StartCmd = new DelegateCommand(() => ExStart());
            StopCmd = new DelegateCommand(() => ExStop());
            ResetCmd = new DelegateCommand(() => ExReset());
            ClearCmd = new DelegateCommand(() => ExClear());

            logger.Info("Porla HMI Initialize successful!");
        }       
        #endregion
        #region Methods
        private void ExAuto()
        {
            if (System.Windows.MessageBox.Show("Do you want to change the mode to Auto?", "Information", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK) 
            {
               
               
                Auto = true;
               
            }
            else
            {
              
            }
           
        }
        private void ExManual()
        {
            if (System.Windows.MessageBox.Show("Do you want to change the mode to Manual?", "Information", MessageBoxButton.OKCancel, MessageBoxImage.Information)== MessageBoxResult.OK) 
            {
                
                Auto = false;
               

            }
        }
        private void ExInitialize()
        {
            if (System.Windows.MessageBox.Show("Do you want to Initialize?", "Information", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
            {
                
            }
        }
        private void ExStart()
        {
            if (Auto)
            {
                if (string.IsNullOrEmpty(AppService.RecipeName))
                {
                    System.Windows.MessageBox.Show("Please load recipe first!!", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                }
            }
        }
        private void ExStop()
        {
            ;
        }
        private void ExReset()
        {
            ;
        }
        private void ExClear()
        {
            ;

        }
        public void MainSequence()
        {

        }
        #endregion
    }
}
