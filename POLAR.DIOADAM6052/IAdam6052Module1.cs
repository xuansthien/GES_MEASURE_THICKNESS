using System.Threading.Tasks;

namespace POLAR.DIOADAM6052
{
    public interface IAdam6052Module1
    {
        void Connect();
        void Disconnect();
        void Initialize(string IpAddress);
        Task RefreshDIO();
        void SetOutput(int i_iCh, int iOnOff);
        bool CheckSafetyCondition();
        bool CheckDi(int iODescriptions);
    }
}