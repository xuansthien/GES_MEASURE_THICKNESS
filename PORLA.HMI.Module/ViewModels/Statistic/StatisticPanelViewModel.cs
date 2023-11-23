using PORLA.HMI.Service;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PORLA.HMI.Module.ViewModels.Statistic
{
    public class StatisticPanelViewModel : BindableBase
    {
        private IAppService _appServiceInstance;

        public IAppService AppServiceInstance
        {
            get { return _appServiceInstance; }
            set { SetProperty(ref _appServiceInstance, value); }
        }
        public DelegateCommand<string> NavigateCommand { get; private set; }
        private readonly IRegionManager _regionManager;
        public StatisticPanelViewModel(IRegionManager regionManager, IAppService appServiceModule)
        {
            _regionManager = regionManager;
            AppServiceInstance = appServiceModule;
            NavigateCommand = new DelegateCommand<string>(ExecuteNavigateWindow);
        }

        private void ExecuteNavigateWindow(string navigatePath)
        {
            if (navigatePath != null)
            {
                _regionManager.RequestNavigate("ContentRegion", navigatePath);
                AppServiceInstance.pagePanel = navigatePath;
            }

        }
    }
}
