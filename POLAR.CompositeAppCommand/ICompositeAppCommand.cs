using Prism.Commands;

namespace POLAR.CompositeAppCommand
{
    public interface ICompositeAppCommand
    {
        #region IAI Motion Commands
        CompositeCommand cmdIAIServoOnOff { get; }
        CompositeCommand cmdIAIAbsoluteMove { get; }
        CompositeCommand cmdIAIRelativeMove { get; }
        CompositeCommand cmdIAIJogForward { get; }
        CompositeCommand cmdIAIJogBackward { get; }
        CompositeCommand cmdIAIAxisYJogForward { get; }
        CompositeCommand cmdIAIAxisYJogBackward { get; }
        CompositeCommand cmdIAIUpdateJogSpeed { get; }
        CompositeCommand cmdIAIAlarmReset { get; }
        CompositeCommand cmdIAIToggleCheckBox { get; }
        CompositeCommand cmdIAITestMove { get; }
        CompositeCommand cmdIAITestPatternMove { get; }
        CompositeCommand cmdIAIStopPatternMove { get; }
        #endregion

        #region DIO Adam-6052 Commands
        CompositeCommand cmdDOVaccumOnOff { get;}
        CompositeCommand CmdDOPowerSupplySensor { get; }
        CompositeCommand cmdDOLightOnOff { get; }
        #endregion

        #region Precitec Commands
        CompositeCommand cmdFssPlotSignalId { get; }
        CompositeCommand cmdFssRunProgram { get; }
        CompositeCommand cmdFssSendCmd { get; }
        CompositeCommand cmdOneDSendSetting { get; }
        CompositeCommand cmdOneDStartScan { get; }
        CompositeCommand cmdOneDSetEncPos { get; }
        CompositeCommand cmdOneDSendCmd { get; }
        CompositeCommand cmdOneDTriggerEvent { get; }
        #endregion
    }
}