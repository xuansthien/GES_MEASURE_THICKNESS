using POLAR.ModelAggregator.Alarm;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace POLAR.HMI.Alarm.Models.Define
{
    public class AlarmDefinitions
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Dictionary<short, AlarmDefineItems> _alarmDetail;
        public Dictionary<short, AlarmDefineItems> AlarmDetail
        {
            get { return _alarmDetail; }
            private set { _alarmDetail = value; }
        }
        public AlarmDefinitions()
        {
            _alarmDetail = new Dictionary<short, AlarmDefineItems>();
            LoadDefinationAlarmFromConfig();
        }

        //Read file from embeded resource
        private static DataTable ConvertCSVtoDataTable()
        {
            DataTable dt = new DataTable();
            try
            {
                var assembly = Assembly.LoadFrom("PORLA.exe");
                var resourceName = "PORLA.HMI.Main.Resource.AlarmDefine.csv";

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader sr = new StreamReader(stream))
                {
                    string[] headers = sr.ReadLine().Split(',');
                    foreach (string header in headers)
                    {
                        dt.Columns.Add(header);
                    }
                    while (!sr.EndOfStream)
                    {
                        string[] rows = sr.ReadLine().Split(',');
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i];
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return dt;
        }

        private void LoadDefinationAlarmFromConfig()
        {
            try
            {
                DataTable dataTable = ConvertCSVtoDataTable();

                //Create List defination for alarm
                List<AlarmDefineItems> vs = new List<AlarmDefineItems>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    AlarmDefineItems alarmLoad = new AlarmDefineItems();

                    alarmLoad.AlarmId = short.Parse(dataTable.Rows[i].Field<string>(0));
                    alarmLoad.AlarmCategory = (AlarmCategory)int.Parse(dataTable.Rows[i].Field<string>(1));
                    alarmLoad.Description = dataTable.Rows[i].Field<string>(2);
                    alarmLoad.SourceModule = (SourceModule)int.Parse(dataTable.Rows[i].Field<string>(3));
                    alarmLoad.ActionOptions = (AlarmActionOptions)int.Parse(dataTable.Rows[i].Field<string>(4));
                    alarmLoad.Instruction = dataTable.Rows[i].Field<string>(5);
                    vs.Add(alarmLoad);
                }

                //Create alarm defination
                foreach (var item in vs)
                {
                    AddDefinition(item.AlarmId, item.AlarmCategory, item.Description, item.SourceModule,
                        item.ActionOptions, item.Instruction);
                }
            }
            catch (Exception e)
            {
                logger.Error($"Load LoadDefinationAlarmFromConfig fail with exception: {e}");
            }
        }
        public void AddDefinition(short AlarmID, AlarmCategory Category, string Description,
            SourceModule Module, AlarmActionOptions ActionOptions, string Instruction)
        {
            AlarmDefineItems AlarmDefineItems = new AlarmDefineItems()
            {
                AlarmCategory = Category,
                Description = Description,
                SourceModule = Module,
                ActionOptions = ActionOptions,
                Instruction = Instruction
            };

            if (_alarmDetail.ContainsKey(AlarmID))
            {
                logger.Error("Key Already Exit");
            }
            else
            {
                _alarmDetail.Add(AlarmID, AlarmDefineItems);
            }
        }


        public bool GetDefinition(short ID, out AlarmDefineItems alarmDefinition)
        {
            alarmDefinition = null;
            if (_alarmDetail.ContainsKey(ID))
            {
                _alarmDetail.TryGetValue(ID, out alarmDefinition);
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
