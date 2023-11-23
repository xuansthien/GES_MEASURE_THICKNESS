using log4net;
using PORLA.HMI.Module.Model;
using PORLA.HMI.Service;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using SuproCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace PORLA.HMI.Module.ViewModels
{
    public class AccountPageViewModel : BindableBase
    {
        #region Property
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ObservableCollection<DataSqlModel> _observableListAccount = new ObservableCollection<DataSqlModel>();
        public ObservableCollection<DataSqlModel> ObservableListAccount { get { return _observableListAccount; } set { SetProperty(ref _observableListAccount, value); } }

        private IAppService _appServiceInstance;
        public DataSqlModel MySelectedItem { get; set; }
        public IAppService AppServiceInstance
        {
            get { return _appServiceInstance; }
            set { SetProperty(ref _appServiceInstance, value); }
        }

        private DelegateCommand _AddCmd;
        public DelegateCommand AddCmd =>
            _AddCmd ?? (_AddCmd = new DelegateCommand(Add_Click));

        private DelegateCommand<string> _updateCmd;
        public DelegateCommand<string> updateCmd =>
            _updateCmd ?? (_updateCmd = new DelegateCommand<string>(Update_Click));

        private string _cbuserType;
        public string cbuserType
        {
            get { return this._cbuserType; }
            set
            {
                if (!string.Equals(this._cbuserType, value))
                {
                    this._cbuserType = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private string _txtName;
        public string txtName
        {
            get { return this._txtName; }
            set
            {

                if (!string.Equals(this._txtName, value))
                {
                    this._txtName = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private string _txtUsername;
        public string txtUsername
        {
            get { return this._txtUsername; }
            set
            {

                if (!string.Equals(this._txtUsername, value))
                {
                    this._txtUsername = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private string _password2;

        public string Password2
        {
            get { return _password2; }
            set { SetProperty(ref _password2, value); }
        }

        string selectID = "";

        public DelegateCommand NavigateCommand { get; private set; }
        public DelegateCommand EditCmd { get; private set; }
        public DelegateCommand DeleteCmd { get; private set; }

        private readonly IRegionManager _regionManager;
        #endregion
        public AccountPageViewModel(IRegionManager regionManager, IAppService appServiceModule)
        {
            _regionManager = regionManager;
            AppServiceInstance = appServiceModule;
            NavigateCommand = new DelegateCommand(ExecuteNavigateWindow);
            EditCmd = new DelegateCommand(() => Edit_Click());
            DeleteCmd = new DelegateCommand(() => Delete_Click());
            updateGrid();
        }

        private void ExecuteNavigateWindow()
        {
            _regionManager.RequestNavigate("ContentRegion", "HomePage");
            AppServiceInstance.Slogin = "false";
            AppServiceInstance.pageLogin = "HomePage";
            AppServiceInstance.user = "Operator";
            AppServiceInstance.userType = "Operator";
        }
        private void Add_Click()
        {
            string queryCon = "";
            string UserType = "";
            if (cbuserType != null)
            {
                string[] a = cbuserType.Split(' ');
                UserType = a[5];
            }
            if (Password == Password2 && Password != null && txtUsername != null && UserType != "")
            {
                queryCon = "INSERT INTO user (Username,Usertype,Password) VALUES ('" + txtUsername + "','" + UserType + "','" + CreateMD5(Password) + "')";
                MySQLProvider.ExecuteNonQuery(queryCon);
                updateGrid();
                txtName = txtUsername = Password = Password2 = null;
                System.Windows.MessageBox.Show("Successfully!", "Info", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            else
            {
                System.Windows.MessageBox.Show("Check your information again!", "Info", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
        protected virtual void Update_Click(string parameter)
        {
            string UserType = "";
            if (cbuserType != null)
            {
                string[] a = cbuserType.Split(' ');
                UserType = a[5];
            }
            if (Password == Password2 && Password != null && txtUsername != null && UserType != "" && selectID != null)
            {
                string queryString = string.Format(@"UPDATE user SET Username = '{0}',Usertype = '{1}',Password = '{2}'
                                                WHERE id ='{3}'", txtUsername, UserType, CreateMD5(Password), selectID);
                MySQLProvider.ExecuteNonQuery(queryString);
                updateGrid();
                txtUsername = Password = Password2 = selectID = null;
                System.Windows.MessageBox.Show("Successfully!", "Info", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            else
            {
                System.Windows.MessageBox.Show("Check your information again!", "Info", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
        private void Edit_Click()
        {
            DataSqlModel selectedRow = MySelectedItem as DataSqlModel;
            if (selectedRow != null)
            {
                selectID = selectedRow.IDUser.ToString();
                txtUsername = selectedRow.UserName.ToString();
                cbuserType = selectedRow.UserType.ToString();
            }
            else
            {
                System.Windows.MessageBox.Show("Choose a row!", "Info", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }
        }
        private void Delete_Click()
        {
            DataSqlModel selectedRow = MySelectedItem as DataSqlModel;
            if (selectedRow != null)
            {
                selectID = selectedRow.IDUser.ToString();
                string queryString = string.Format(@"DELETE FROM user WHERE id ='" + selectID + "' ");
                MySQLProvider.ExecuteNonQuery(queryString);
                updateGrid();
                System.Windows.MessageBox.Show("Successfully!", "Info", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            else
            {
                System.Windows.MessageBox.Show("Choose a row!", "Info", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }
        }

        public static string CreateMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        void updateGrid()
        {
            string queryString = string.Format(@"SELECT id,Username,Usertype FROM user");
            DataTable dt = MySQLProvider.ExecuteQuery(queryString);
            if (dt == null) return;
            ObservableListAccount.Clear();
            foreach (DataRow r in dt.Rows)
            {
                ObservableListAccount.Add(new DataSqlModel()
                {
                    IDUser = Convert.ToInt32(r["id"]),
                    UserName = r["Username"].ToString(),
                    UserType = r["Usertype"].ToString(),
                });
            }
        }
    }
}
