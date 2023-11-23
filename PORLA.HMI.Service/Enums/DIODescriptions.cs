using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PORLA.HMI.Service.Enums
{
    public class DIODescriptions
    {
        #region Module Adam 1
        // 8 input
        public const int In_DoorSignal = 0; // Add Alarm
        public const int In_EMOSignal = 1; // Add Alarm
        public const int In_ResetSignal = 2;
        public const int In_StopSignal = 3;
        public const int In_StartSignal = 4;
        public const int In_VacuumSignal = 5;
        public const int In_MainAirSignal = 6; // Add Alarm
        public const int In_VacuumBtn = 7; // button vacuum.
        // 8 output
        public const int Out_FSSOnOffSignal = 8;
        public const int Out_LightOnOffSignal = 9;
        public const int Out_VacuumOnOffSignal = 10;
        public const int Out_ReleaseVacuumOnOffSignal = 11;
        #endregion

        #region Module Adam 2
        // 8 input 
        public const int In_ServiceKey = 0;
        public const int In_SesorDetectZStage = 1; // Add Alarm
        public const int In_SensorDetectFixture = 2; // Add Alarm
        public const int In_LeftDoorSignal = 3; // Add Alarm
        public const int In_RightDoorSignal = 4; // Add Alarm
        public const int In_SafetyCircuitSignal = 5; // Add Alarm

        // On off status
        public const int Turn_Off = 0;
        public const int Turn_On = 1;
        #endregion
        public const string Start = "Start";
        public const string Stop = "Stop";

    }
}
