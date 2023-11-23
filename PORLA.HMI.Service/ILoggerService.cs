using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PORLA.HMI.Service
{
    public interface ILoggerService
    {
        ObservableCollection<string> LogMessages { get; }
        void AddLogMessage(string message, string logLevel = "Info", string loggerName = "Machine");
    }
}
