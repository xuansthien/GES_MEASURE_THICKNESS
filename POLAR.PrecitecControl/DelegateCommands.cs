using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POLAR.PrecitecControl
{
    public partial class FSSAreaScan : BindableBase
    {
        private DelegateCommand<string> FssPlotSignalId;

        private DelegateCommand FssRunProgram;

        private DelegateCommand<string> FssSendCmd;
        private void InitializeCommand()
        {
            FssPlotSignalId = new DelegateCommand<string>(ExFssPlotSignalId);
            FssRunProgram = new DelegateCommand(ExFssRunProgram);
            FssSendCmd = new DelegateCommand<string>(ExFssSendCmd);

            // Register Delegatecommand for Compositecommand which is used throughout app.
            AppCommand.cmdFssPlotSignalId.RegisterCommand(FssPlotSignalId);
            AppCommand.cmdFssRunProgram.RegisterCommand(FssRunProgram);
            AppCommand.cmdFssSendCmd.RegisterCommand(FssSendCmd);
        }

        private void ExFssSendCmd(string cmd)
        {
            SendAsyncCmd(cmd);
        }

        private async void ExFssRunProgram()
        {
            await RunProgram();
        }

        private void ExFssPlotSignalId(string plotSignal)
        {
            PlotSignalId(plotSignal);
        }
    }
}
