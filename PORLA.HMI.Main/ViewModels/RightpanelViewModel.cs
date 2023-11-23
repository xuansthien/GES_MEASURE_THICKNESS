using PORLA.HMI.Service;
using log4net;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace PORLA.HMI.Main.ViewModels
{
    public class RightpanelViewModel : BindableBase
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IAppService _appServiceInstance;

        public IAppService AppServiceInstance
        {
            get { return _appServiceInstance; }
            set { SetProperty(ref _appServiceInstance, value); }
        }
        public DelegateCommand<string> NavigateCommand { get; private set; }
        public DelegateCommand<string> InitReqCommand { get; private set; }
        public DelegateCommand<string> AutoReqCommand { get; private set; }
        public DelegateCommand<string> ManualReqCommand { get; private set; }
        public DelegateCommand<string> SetupReqCommand { get; private set; }
        public DelegateCommand<string> StopReqCommand { get; private set; }

        public DelegateCommand<string> ProdReqCommand { get; private set; }
        public DelegateCommand<string> CalibReqCommand { get; private set; }
        public DelegateCommand<string> GRRReqCommand { get; private set; }


        private readonly IRegionManager _regionManager;
        public RightpanelViewModel(IRegionManager regionManager, IAppService appServiceModule)
        {
            _regionManager = regionManager;
            AppServiceInstance = appServiceModule;
            NavigateCommand = new DelegateCommand<string>(ExecuteNavigateWindow);
           
        }

       

        private void ExecuteNavigateWindow(string navigatePath)
        {
            if (navigatePath != null)
            {
                AppServiceInstance.pageLoad = navigatePath;
                if (navigatePath == "AccountPage" && AppServiceInstance.userType == "Operator")
                {
                    _regionManager.RequestNavigate(Regions.ContentRegion, "Login");
                    _regionManager.RequestNavigate(Regions.OptionPanelRegion, "EmptyPanel");
                   
                    
                }
                else
                {
                    AppServiceInstance.pageLogin = navigatePath;
                    _regionManager.RequestNavigate(Regions.ContentRegion, navigatePath);
                    _regionManager.RequestNavigate(Regions.OptionPanelRegion, "EmptyPanel");
                }
            }

        }
    }
}
