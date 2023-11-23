using PORLA.HMI.Service;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using SuproCL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using log4net;

namespace PORLA.HMI.Main.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IAppService _appServiceInstance;

        public IAppService AppServiceInstance
        {
            get { return _appServiceInstance; }
            set { SetProperty(ref _appServiceInstance, value); }
        }

        private DelegateCommand<string> _closeDialogCommand;
        public DelegateCommand<string> CloseDialogCommand =>
            _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(LoginDialog));
        public DelegateCommand NavigateCommand { get; private set; }
        private readonly IRegionManager _regionManager;
        private string _username;
        public string Username
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }
        private string _notification;
        public string Notification
        {
            get { return this._notification; }
            set
            {

                if (!string.Equals(this._notification, value))
                {
                    this._notification = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        public LoginViewModel(IRegionManager regionManager, IAppService appServiceModule)
        {
            _regionManager = regionManager;
            AppServiceInstance = appServiceModule;
            NavigateCommand = new DelegateCommand(homep);
        }
        private void homep()
        {
            _regionManager.RequestNavigate(Regions.ContentRegion, "HomePage");
            AppServiceInstance.pageLogin = "HomePage";
        }
        private void ExecuteNavigateWindow(string navigatePath)
        {
            if (navigatePath != null)
            {
                AppServiceInstance.pageLogin = navigatePath;

                if (navigatePath == "SettingPage")
                {
                    _regionManager.RequestNavigate(Regions.OptionPanelRegion, "SettingPanel");
                    _regionManager.RequestNavigate(Regions.ContentRegion, navigatePath);
                    AppServiceInstance.pagePanel = "STmanual";
                }
                else
                {
                    _regionManager.RequestNavigate(Regions.ContentRegion, navigatePath);
                }
            }
        }
        protected virtual void LoginDialog(string parameter)
        {
            if (Username != null && Password != null)
            {
                string queryString = string.Format(@"SELECT Username,Usertype,Password
                                                    FROM user WHERE Username ='{0}' AND Password = '{1}'       
                                                    ", Username, Password);
                DataTable _dataTable = MySQLProvider.ExecuteQuery(queryString);

                if (_dataTable != null && _dataTable.Rows.Count > 0)
                {
                    AppServiceInstance.user = _dataTable.Rows[0]["Username"].ToString();
                    logger.Info($"{AppServiceInstance.user} log in successfully");
                    AppServiceInstance.userType = _dataTable.Rows[0]["Usertype"].ToString();
                    ExecuteNavigateWindow(AppServiceInstance.pageLoad);
                    Username = null;
                    Password = null;
                }
                else
                {
                    Notification = "  * User or Password is incorrect!";
                }
            }
            else
            {
                Notification = "  * User or Password is incorrect!";
                logger.Error("Login data error.");
            }
        }
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
