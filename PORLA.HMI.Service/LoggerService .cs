using Prism.Mvvm;
using log4net;
using System;
using System.Collections.ObjectModel;

namespace PORLA.HMI.Service
{
    public class LoggerService : BindableBase, ILoggerService
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        private ObservableCollection<string> _logMessages = new ObservableCollection<string>();
        public ObservableCollection<string> LogMessages
        {
            get { return _logMessages; }
            set { SetProperty(ref _logMessages, value); }
        }

        public void AddLogMessage(string message, string logLevel = "Info", string loggerName = "Machine")
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff");

            logger.Info(message);

            string formattedLogMessage = FormatLogMessage(timestamp, logLevel, loggerName, message);

            LogMessages.Add(formattedLogMessage);
        }

        private string FormatLogMessage(string timestamp, string logLevel, string loggerName, string originalMessage)
        {
            string formattedMessage = $"[{timestamp}] [{logLevel}] [{loggerName}] - {originalMessage}";
            return formattedMessage;
        }
    }
}
