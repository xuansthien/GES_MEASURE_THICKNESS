using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POLAR.ModelAggregator.Alarm
{
    public class AlarmDefineItems
    {
        public short AlarmId { get; set; }
        public AlarmCategory AlarmCategory { get; set; }
        public string Description { get; set; }
        public string Instruction { get; set; }
        public SourceModule SourceModule { get; set; }
        public AlarmActionOptions ActionOptions { get; set; }

        public AlarmDefineItems()
        {
            AlarmId = 0;
            AlarmCategory = AlarmCategory.Error;
            Description = "";
            Instruction = "";
            SourceModule = SourceModule.HMI;
            ActionOptions = AlarmActionOptions.Clear;
        }
    }
}
