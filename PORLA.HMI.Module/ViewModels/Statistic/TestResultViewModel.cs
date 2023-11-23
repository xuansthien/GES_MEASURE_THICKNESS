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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;
using POLAR.ModelAggregator.TestReport;

namespace PORLA.HMI.Module.ViewModels.Statistic
{
    public class TestResultViewModel : BindableBase
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IAppService _appService;

        private IRegionManager _regionManager;

        private ObservableCollection<DataSqlModel> _historyTestList = new ObservableCollection<DataSqlModel>();
        public ObservableCollection<DataSqlModel> HistoryTestList 
        { 
            get { return _historyTestList; } 
            set { SetProperty(ref _historyTestList, value); } 
        }

        public DataSqlModel MySelectedItem { get; set; }
        public DelegateCommand btnClearAlarm { get; private set; }
        public DelegateCommand ApplyCmd { get; private set; }
        public DelegateCommand ExPortCmd { get; private set; }
        private string _txtFrom;
        public string TxtFrom
        {
            get { return _txtFrom; }
            set
            {
                if (!string.Equals(_txtFrom, value))
                {
                    _txtFrom = value;
                    RaisePropertyChanged();
                }
            }
        }
        private string _txtTo;
        public string TxtTo
        {
            get { return _txtTo; }
            set
            {
                if (!string.Equals(_txtTo, value))
                {
                    _txtTo = value;
                    RaisePropertyChanged();
                }
            }
        }
        private DataTable _testReportList;
        public DataTable TestReportList
        {
            get { return _testReportList; }
            set
            {
                if (!DataTable.Equals(_testReportList, value))
                {
                    _testReportList = value;
                    RaisePropertyChanged();
                }
            }
        }
        private string _selectedFileType;
        public string SelectedFileType
        {
            get { return _selectedFileType; }
            set { SetProperty(ref _selectedFileType, value); }
        }      
        public List<string> FileTypes { get; } = new List<string> { "csv", "txt" };
        public TestResultViewModel(IRegionManager regionManager, IAppService appService)
        {
            _appService = appService;
            _regionManager = regionManager;
            ApplyCmd = new DelegateCommand(() => exApply());
            ExPortCmd = new DelegateCommand(() => exExPort());
            SelectedFileType = FileTypes[0];
        }
        private void exExPort()
        {
            if (!string.IsNullOrEmpty(TxtFrom) && !string.IsNullOrEmpty(TxtTo))
            {
                if (!string.IsNullOrEmpty(SelectedFileType))
                {
                    string fileType = SelectedFileType;
                    if (fileType == "csv" || fileType == "txt")
                    {
                        string pathFileTest = $@"Report\TestResult\TestResult{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.{fileType}";
                        try
                        {
                            DataTable dt = MySQLProvider.LoadTestReport("testreport", "TimeDB", Convert.ToDateTime(TxtFrom), Convert.ToDateTime(TxtTo));

                            if (fileType == "txt")
                            {
                               
                                ExportDataTableToFile(dt, pathFileTest);
                            }
                            else if (fileType == "csv")
                            {
                               
                                ExportDataTableToFile(dt, pathFileTest);
                            }

                            System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show($"Saved to: {pathFileTest}. Open file?",
                                      "Confirmation",
                                      System.Windows.MessageBoxButton.YesNo,
                                      System.Windows.MessageBoxImage.Question);

                            if (result == System.Windows.MessageBoxResult.Yes)
                            {
                                System.Diagnostics.Process.Start(pathFileTest);
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Windows.MessageBox.Show($"An error occurred: {ex.Message}");
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Please select a valid file type.");
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("Please select a file type.");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Please select a valid date range.");
            }
        }
        private void ExportDataTableToFile(DataTable dataTable, string filePath)
        {
            try
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(filePath))
                {
                   
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        writer.Write(dataTable.Columns[i]);
                        if (i < dataTable.Columns.Count - 1)
                        {
                            writer.Write(",");
                        }
                    }
                    writer.WriteLine();

                   
                    foreach (DataRow row in dataTable.Rows)
                    {
                        for (int i = 0; i < dataTable.Columns.Count; i++)
                        {
                            writer.Write(row[i].ToString());
                            if (i < dataTable.Columns.Count - 1)
                            {
                                writer.Write(",");
                            }
                        }
                        writer.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while exporting to file: {ex.Message}", ex);
            }
        }
        private void exApply()
        {
            if (!string.IsNullOrEmpty(TxtFrom) && !string.IsNullOrEmpty(TxtTo))
            {
                try
                {
                    if (TestReportList != null) TestReportList.Clear();

                    DateTime startTime = Convert.ToDateTime(TxtFrom);
                    DateTime endTime = Convert.ToDateTime(TxtTo);

                    TestReportList = MySQLProvider.LoadTestReport("testreport", "TimeDB", startTime, endTime);

                    if (TestReportList != null)
                    {
                        HistoryTestList.Clear();

                        foreach (DataRow value in TestReportList.Rows)
                        {
                            DateTime _datetimeExpect = Convert.ToDateTime(value["TimeDB"]);

                            DataSqlModel ouput = new DataSqlModel
                            {
                                TimeDB = Convert.ToDateTime(value["TimeDB"]),
                                RecipeID = value["`Recipe Id`"].ToString(),
                                DutID = value["`Dut Id`"].ToString(),
                                RecipeSensorType = value["`Recipe Sensor Type`"].ToString(),
                                RecipeX0 = value["`Recipe X0`"].ToString(),
                                RecipeY0 = value["`Recipe Y0`"].ToString(),
                                RecipeDX = value["`Recipe DX`"].ToString(),
                                RecipeDY = value["`Recipe DY`"].ToString(),
                                RecipeSpeedXAxis = value["`Recipe speed X axis`"].ToString(),
                                RecipeSpeedYAxis = value["`Recipe speed Y axis`"].ToString()
                            };
                            HistoryTestList.Add(ouput);
                        }
                    }
                }
                catch (Exception)
                {
                    ;
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Please select a valid date range");
            }
        }
    }
}
