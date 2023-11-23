using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using PORLA.HMI.Service;
using System.Windows;
namespace PORLA.HMI.Service
{
    public class AlarmConfig : IAlarmReader
    {
        public List<AlarmDefinition> ReadAlarmDefinitions(string filePath)
        {
            List<AlarmDefinition> alarmDefinitions = new List<AlarmDefinition>();

            try
            {
                XDocument xmlDoc = XDocument.Load(filePath);

                foreach (var alarmElement in xmlDoc.Descendants("Alarm"))
                {
                    AlarmDefinition alarm = new AlarmDefinition();
                    alarm.ID = alarmElement.Attribute("ID").Value;
                    alarm.Category = alarmElement.Element("Category").Value;
                    alarm.Description = alarmElement.Element("Description").Value;
                    alarmDefinitions.Add(alarm);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading alarm definitions: " + ex.Message);
            }
            return alarmDefinitions;
        }
    }
    public class AlarmDefinition
    {
        public string ID { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }

}

