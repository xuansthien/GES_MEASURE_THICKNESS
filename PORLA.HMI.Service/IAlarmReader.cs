using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PORLA.HMI.Service;

namespace PORLA.HMI.Service
{
    public interface IAlarmReader
    {   
        List<AlarmDefinition> ReadAlarmDefinitions(string filePath);
    }
}
