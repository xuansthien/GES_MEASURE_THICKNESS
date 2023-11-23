namespace POLAR.ModelAggregator.Alarm
{
    public enum AlarmCategory
    {
        Warning = 0,
        Error = 1,
    }

    public enum AlarmActionOptions
    {
        Reset = 0,
        Retry = 1,
        Clear = 2,
        AutoClear = 3,
        ClearAndManualAck = 4,
        Reset_Clear = 5,
        Resume = 6,
        Discard = 7
    }

    public enum SourceModule
    {
        DIO = 0,
        HMI = 1
    }
}
