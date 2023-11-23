using System.Collections.ObjectModel;

namespace PORLA.HMI.Service
{
    public interface IAppService
    {
        MachineRunStatus MachineStatus { get; set; }
        ObservableCollection<LogInfor> LogInfors { get; set; }
        ObservableCollection<LogRspPrecitec> LogRspPrecitec { get; set; }
        double ScrollViewerVerticalOffset { get; set; }
        bool[] AlarmArray { get; set; }
        bool AlarmPresence { get; set; }
        string DutBarcode { get; set; }
        DIOAdam6052Module1 DIOModule1 { get; set; }
        DIOAdam6052Module2 DIOModule2 { get; set; }
        string Exposure { get; set; }
        string HMIVer { get; set; }
        IAIModbusTCPIP IAIMotion { get; set; }
        bool IsEnable { get; set; }
        string language { get; set; }
        bool ManualModeActivated { get; set; }
        string pageLoad { get; set; }
        string pageLogin { get; set; }
        string pagePanel { get; set; }
        ObservableCollection<int> ProductionCounter { get; set; }
        bool SettingModeActivated { get; set; }
        string settingPanel { get; set; }
        string Slogin { get; set; }
        ObservableCollection<double> TimeDetail { get; set; }
        string user { get; set; }
        string userType { get; set; }
        bool[] PreviousAlarmList { get; set; }
        bool[] CurrentAlarmList { get; set; }
        RecipeConfig RecipeService { get; set; }
        string RecipeName { get; set; }
        PrecitecControl PrecitecService { get; set; }
    }
}