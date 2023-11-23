namespace POLAR.DIOADAM6052
{
    public interface IAdam6052Module1
    {
        void Connect();
        void Disconnect();
        void Initialize(string IpAddress);
        void RefreshDIO();
        void SetOutput(int i_iCh, int iOnOff);
    }
}