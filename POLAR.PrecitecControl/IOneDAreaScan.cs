using System.Threading.Tasks;

namespace POLAR.PrecitecControl
{
    public interface IOneDAreaScan
    {
        void CloseConnection();
        Task<bool> InitializeOneD();
        bool SendTriggerSetting(string axis, string StartPos, string StopPos, string interval, bool triggerOnReturn);
        bool SendTriggerSetting(string axis, int nStartPos, int nStopPos, float nInterval, bool triggerOnReturn);
        Task StartScan(string lineNo, string SampleNo, bool triggerOnReturn);
        Task StartScan(string SampleNo);
        bool SetTriggerEncoderPos(string EncoderPos, string axis);
        void StopRecording();
        void SendCommand(string _cmd);
        bool SoftwareTriggerAndRecording(int parameter);
        Task SaveMultiPointDataIntoFile();
        Task StopScan();
        void EnaTimerDownloadSpectrum(bool state);
    }
}