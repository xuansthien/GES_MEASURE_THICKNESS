using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POLAR.ModelAggregator.Alarm
{
    public class AlarmItems
    {
        public long AlarmDBId { get; set; }
        public short AlarmId { get; private set; }
        public AlarmCategory AlarmCategory { get; set; }
        public string Description { get; set; }
        public string Instruction { get; set; }
        public string AlarmCreateTime { get; set; }
        public string AlarmClearTime { get; set; }
        public SourceModule SourceModule { get; set; }
        public AlarmActionOptions ActionOptions { get; set; }
        public AlarmItems(string alarmTime,
                          AlarmCategory category,
                          SourceModule sourceModule,
                          long alarmDBId,
                          short alarmId,
                          string description,
                          string instruction,
                          AlarmActionOptions alarmActionOptions,
                          string alarmTimeClear)
        {
            AlarmCreateTime = alarmTime;
            AlarmCategory = category;
            SourceModule = sourceModule;

            AlarmDBId = alarmDBId;
            AlarmId = alarmId;

            Description = description;
            Instruction = instruction;

            ActionOptions = alarmActionOptions;
            AlarmClearTime = alarmTimeClear;
        }
        public void ClearAlarm()
        {
            AlarmClearTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
