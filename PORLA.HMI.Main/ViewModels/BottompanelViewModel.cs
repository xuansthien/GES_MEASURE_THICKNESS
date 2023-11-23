using PORLA.HMI.Service;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using log4net;

namespace PORLA.HMI.Main.ViewModels
{
    public class BottompanelViewModel : BindableBase
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        private IAppService _appServiceInstance;

        public IAppService AppServiceInstance
        {
            get { return _appServiceInstance; }
            set { SetProperty(ref _appServiceInstance, value); }
        }
        public DelegateCommand<string> NavigateCommand { get; private set; }
        private readonly IRegionManager _regionManager;
        public BottompanelViewModel(IRegionManager regionManager, IAppService appServiceModule)
        {
            _regionManager = regionManager;
            AppServiceInstance = appServiceModule;
            AppServiceInstance.pageLogin = "HomePage";
            NavigateCommand = new DelegateCommand<string>(ExecuteNavigateWindow);
        }

        private void ExecuteNavigateWindow(string navigatePath)
        {
            if (navigatePath != null)
            {
                AppServiceInstance.pageLoad = navigatePath;
                // remove later
                AppServiceInstance.pageLogin = navigatePath;
                if (navigatePath == "SettingPage")
                {
                    _regionManager.RequestNavigate(Regions.OptionPanelRegion, "SettingPanel");
                }
                _regionManager.RequestNavigate(Regions.ContentRegion, navigatePath);
                

                //if ((navigatePath == "SettingPage" | navigatePath == "TeachingPage" | navigatePath == "AlignPage" | navigatePath=="ManualPage") && AppServiceInstance.userType == "Operator")
                //{
                //    _regionManager.RequestNavigate(Regions.ContentRegion, "Login");
                //    _regionManager.RequestNavigate(Regions.OptionPanelRegion, "EmptyPanel");
                //}
                //else
                //{
                //    AppServiceInstance.pageLogin = navigatePath;
                //    _regionManager.RequestNavigate(Regions.ContentRegion, navigatePath);
                //    if (navigatePath == "TestResult")
                //    {
                //        _regionManager.RequestNavigate(Regions.OptionPanelRegion, "StatisticPanel");
                //        AppServiceInstance.pagePanel = "TestResult";
                //    }
                //    else if (navigatePath == "SettingPage")
                //    {
                //        _regionManager.RequestNavigate(Regions.OptionPanelRegion, "SettingPanel");
                //        AppServiceInstance.settingPanel = "SettingPage";
                //    }

                //    else if (navigatePath == "MachineLogPage")
                //    {
                //        _regionManager.RequestNavigate(Regions.OptionPanelRegion, "LogPanel");
                //        AppServiceInstance.settingPanel = "MachineLogPage";
                //    }

                //    else
                //    {

                //        _regionManager.RequestNavigate(Regions.OptionPanelRegion, "EmptyPanel");
                //    }
                //}
            }
        }
    }
}
