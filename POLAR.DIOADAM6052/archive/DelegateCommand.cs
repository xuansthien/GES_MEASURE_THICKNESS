using PORLA.HMI.Service.Enums;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POLAR.DIOADAM6052
{
    public partial class Adam6052Module1 : BindableBase, IAdam6052Module1
    {
        private DelegateCommand<string> cmdVaccumOnOff;
        private DelegateCommand<string> cmdPowerSupplySensor;
        private DelegateCommand<string> cmdLightOnOff;
        private void InitializeCommand()
        {
            cmdVaccumOnOff = new DelegateCommand<string>(ExcmdVaccumOnOff);
            cmdPowerSupplySensor = new DelegateCommand<string>(ExcmdPowerSupplySensor);
            cmdLightOnOff = new DelegateCommand<string>(ExcmdLightOnOff);

            AppCommand.cmdDOVaccumOnOff.RegisterCommand(cmdVaccumOnOff);
            AppCommand.CmdDOPowerSupplySensor.RegisterCommand(cmdPowerSupplySensor);
            AppCommand.cmdDOLightOnOff.RegisterCommand(cmdLightOnOff);
        }

        private void ExcmdLightOnOff(string state)
        {
            if (state == "LOn")
            {
                SetOutput(DIODescriptions.Out_LightOnOffSignal, DIODescriptions.Turn_On);
                RefreshDIO();
            }
            if (state == "LOff")
            {
                SetOutput(DIODescriptions.Out_LightOnOffSignal, DIODescriptions.Turn_Off);
                RefreshDIO();
            }
        }

        private void ExcmdPowerSupplySensor(string state)
        {
            if (state == "FSS")
            {
                SetOutput(DIODescriptions.Out_FSSOnOffSignal, DIODescriptions.Turn_On);
                RefreshDIO();
            }
            if (state == "1D")
            {
                SetOutput(DIODescriptions.Out_FSSOnOffSignal, DIODescriptions.Turn_Off);
                RefreshDIO();
            }
        }

        private void ExcmdVaccumOnOff(string state)
        {
            if (state == "VacOn")
            {
                SetOutput(DIODescriptions.Out_VacuumOnOffSignal, DIODescriptions.Turn_On);
                RefreshDIO();
            }
            if (state == "VacOff")
            {
                SetOutput(DIODescriptions.Out_VacuumOnOffSignal, DIODescriptions.Turn_Off);
                RefreshDIO();
            }
        }
    }
}
