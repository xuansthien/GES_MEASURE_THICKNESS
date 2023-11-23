using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PORLA.HMI.Service.Enums
{
    public enum HardwareConnectionState
    {
        Online,
        Offline
    }
    public enum TestModeEnum
    {
        None = 0,
        PRODUCTION = 1,
        GRR = 2
    }

    public enum MachineModeEnum
    {
        None = 0,
        AUTO = 1,
        MANUAL = 2,
        SETTING = 3
    }

    public enum MachineStateEnum
    {
        None = 0,
        RUN = 1,
        STOP = 2,
        READY = 3,
        NOTREADY = 4,
        ERROR = 5,
        WARNING = 6,
        IDLE = 7
    }

    public enum MachineInitStateEnum
    {
        None = 0,
        INITIALIZING = 1,
        INITIALIZED = 2,
        INITIALFAILED = 3
    }

    public enum LogType
    {
        [Description("Connection Status")]
        Connection,
        [Description("Motion Status")]
        Motion,
        [Description("Precitec Status")]
        PricitecController,
        [Description("Adam 6052 Module 1 Status")]
        DIOModule1,
        [Description("Adam 6052 Module 2 Status")]
        DIOModule2,
        [Description("Sequence Status")]
        Sequence,
        [Description("General Status")]
        General
    }
    public enum MotionStatus { FINISH, MOVING, TIMEOUT, SENDCMDERROR, ERRORCONNECTION, EMO_PRESS, STOP_SIGNAL, EXCEPTION_STATUS, NONE }
    public enum ScanStatus
    {
        FINISH,
        TIMEOUT,
        EXCEPTION,
        NONE
    }


}

