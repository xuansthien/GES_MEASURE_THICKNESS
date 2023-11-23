
using PORLA.HMI.Service;
using log4net;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PORLA.HMI.Module.ViewModels
{
    public class VersionPageViewModel : BindableBase
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IAppService _appService;

        public IAppService AppService
        {
            get { return _appService; }
            set { SetProperty(ref _appService, value); }
        }

        private string _HMIVer = "";
        public string HMIVer
        {
            get { return this._HMIVer; }
            set
            {

                if (!string.Equals(this._HMIVer, value))
                {
                    this._HMIVer = value;
                    AppService.HMIVer = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        public VersionPageViewModel(IAppService appService)
        {
            AppService = appService;
            try
            {
                FileVersionInfo HMIfileVersionInfo = FileVersionInfo.GetVersionInfo(@".\PORLA.HMI.Module.dll");
                HMIVer = HMIfileVersionInfo.FileVersion.ToString();
                
                logger.Info("HMI Version:" + HMIVer );
            }
            catch (Exception e)
            {
                Debug.WriteLine(e); 
            }
            
        }

    }
}
