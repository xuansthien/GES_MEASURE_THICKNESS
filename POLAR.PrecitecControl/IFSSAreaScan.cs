using PORLA.HMI.Service.Enums;
using System.Threading.Tasks;

namespace POLAR.PrecitecControl
{
    public interface IFSSAreaScan
    {
        void InitializeFssAsync();
        void PlotSignalId(string signalId);
        Task RunProgram();
        void StopProgram();
        Task<ScanStatus> RunScriptAutoAsync();
        Task<bool> CompileScriptAsync();
        void Disconnect();
        string GetScriptContent();
        string UpdateScanScript(string typeUpdate, string scanPrg, params double[] args);
        void WriteUpdatedScriptToFile(string programCode);
        void EnaTimerDownloadSpectrum(bool state);
    }
}