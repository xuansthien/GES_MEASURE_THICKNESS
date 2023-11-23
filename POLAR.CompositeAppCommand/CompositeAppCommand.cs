using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POLAR.CompositeAppCommand
{
    public class CompositeAppCommand : ICompositeAppCommand
    {
        #region IAI Motion Commands
        public CompositeCommand cmdIAIServoOnOff { get; } = new CompositeCommand();
        public CompositeCommand cmdIAIAbsoluteMove { get; } = new CompositeCommand();
        public CompositeCommand cmdIAIRelativeMove { get; } = new CompositeCommand();
        public CompositeCommand cmdIAIJogForward { get; } = new CompositeCommand();
        public CompositeCommand cmdIAIJogBackward { get; } = new CompositeCommand();
        public CompositeCommand cmdIAIAxisYJogForward { get; } = new CompositeCommand();
        public CompositeCommand cmdIAIAxisYJogBackward { get; } = new CompositeCommand();
        public CompositeCommand cmdIAIUpdateJogSpeed { get; } = new CompositeCommand();
        public CompositeCommand cmdIAIAlarmReset { get; } = new CompositeCommand();
        public CompositeCommand cmdIAIToggleCheckBox { get; } = new CompositeCommand();
        public CompositeCommand cmdIAITestMove { get; } = new CompositeCommand();
        public CompositeCommand cmdIAITestPatternMove { get; } = new CompositeCommand();
        public CompositeCommand cmdIAIStopPatternMove { get; } = new CompositeCommand();
        #endregion

        #region DIO Adam-6052 Commands
        public CompositeCommand cmdDOVaccumOnOff { get; } = new CompositeCommand();
        public CompositeCommand CmdDOPowerSupplySensor { get; } = new CompositeCommand();
        public CompositeCommand cmdDOLightOnOff { get; } = new CompositeCommand();
        #endregion

        #region Precitect Commands
        public CompositeCommand cmdFssPlotSignalId { get; } = new CompositeCommand();
        public CompositeCommand cmdFssRunProgram { get; } = new CompositeCommand();
        public CompositeCommand cmdFssSendCmd { get; } = new CompositeCommand();
        public CompositeCommand cmdOneDSendSetting { get; } = new CompositeCommand();
        public CompositeCommand cmdOneDStartScan { get; } = new CompositeCommand();
        public CompositeCommand cmdOneDSetEncPos { get; } = new CompositeCommand();
        public CompositeCommand cmdOneDSendCmd { get; } = new CompositeCommand();
        public CompositeCommand cmdOneDTriggerEvent { get; } = new CompositeCommand();
        #endregion
    }


}
