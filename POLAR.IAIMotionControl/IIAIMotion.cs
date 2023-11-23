using PORLA.HMI.Service.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace POLAR.IAIMotionControl
{
    public interface IIAIMotion
    {
        string Station { get; set; }

        event EventHandler<FlagStatus> FlagStatusChange;
        string BoolArrToHex(bool[] Axis);
        string CallPoint(string point);
        string ChangePoint(PointData point, string pointnumber);
        void Initialize(string IP, string Port);
        string OperationStopAndCancel();
        string ReadFlag(string start, string number);
        string ResetErr();
        Tuple<string, string> SetSpeed_AccDcl(string speed, string accDcl);
        string SoftReset();
        string StopProgram(string progarmNumber);
        Task<MotionStatus> AbsoluteMoveWithCheckFinish(string state, CancellationToken cancellationToken);
        Task<MotionStatus> PatternMove(string[] paramters, CancellationToken cancellationToken);
        bool Disconnect();
    }
}