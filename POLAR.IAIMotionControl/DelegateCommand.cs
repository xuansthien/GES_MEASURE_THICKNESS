using PORLA.HMI.Service.Enums;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace POLAR.IAIMotionControl
{
    public partial class IAIMotion : BindableBase, IIAIMotion
    {
        private DelegateCommand<string> cmdServoOnOff;
        private DelegateCommand<string> cmdAbsoluteMove;
        private DelegateCommand cmdRelativeMove;
        private DelegateCommand<string> cmdJogForward;
        private DelegateCommand<string> cmdJogBackward;
        private DelegateCommand<string> cmdJogAxisYForward;
        private DelegateCommand<string> cmdJogAxisYBackward;
        private DelegateCommand<string> cmdUpdateJogSpeed;
        private DelegateCommand cmdAlarmReset;
        private DelegateCommand<string> cmdToggleCheckBox;
        private DelegateCommand<string> cmdTestMove;
        private DelegateCommand<string> cmdTestPatternMove;
        private DelegateCommand<string> cmdStopPatternMove;

        private void InitializeCommand() 
        {
            cmdServoOnOff = new DelegateCommand<string>(ExcmdServoOnOff);
            cmdAbsoluteMove = new DelegateCommand<string>(ExcmdAbsoluteMove);
            cmdRelativeMove = new DelegateCommand(ExcmdRelativeMove);
            cmdJogForward = new DelegateCommand<string>(ExcmdJogForward);
            cmdJogBackward = new DelegateCommand<string>(ExcmdJogBackward);
            cmdJogAxisYForward = new DelegateCommand<string>(ExcmdJogAxisYForward);
            cmdJogAxisYBackward = new DelegateCommand<string>(ExcmdJogAxisYBackward);
            cmdUpdateJogSpeed = new DelegateCommand<string>(ExcmdUpdateJogSpeed);
            cmdAlarmReset = new DelegateCommand(ExcmdAlarmReset);
            cmdToggleCheckBox = new DelegateCommand<string>(ExcmdToggleCheckBox);
            cmdTestMove = new DelegateCommand<string>(ExcmdTestMove);
            cmdTestPatternMove = new DelegateCommand<string>(ExcmdTestPatternMove);
            cmdStopPatternMove = new DelegateCommand<string>(ExcmdStopPatternMove);

            // Register Delegatecommand for Compositecommand which is used throughout app.
            AppCommand.cmdIAIServoOnOff.RegisterCommand(cmdServoOnOff);
            AppCommand.cmdIAIAbsoluteMove.RegisterCommand(cmdAbsoluteMove);
            AppCommand.cmdIAIRelativeMove.RegisterCommand(cmdRelativeMove);
            AppCommand.cmdIAIJogForward.RegisterCommand(cmdJogForward);
            AppCommand.cmdIAIJogBackward.RegisterCommand(cmdJogBackward);
            AppCommand.cmdIAIAxisYJogForward.RegisterCommand(cmdJogAxisYForward);
            AppCommand.cmdIAIAxisYJogBackward.RegisterCommand(cmdJogAxisYBackward);
            AppCommand.cmdIAIUpdateJogSpeed.RegisterCommand(cmdUpdateJogSpeed);
            AppCommand.cmdIAIAlarmReset.RegisterCommand(cmdAlarmReset);
            AppCommand.cmdIAIToggleCheckBox.RegisterCommand(cmdToggleCheckBox);
            AppCommand.cmdIAITestMove.RegisterCommand(cmdTestMove);
            AppCommand.cmdIAITestPatternMove.RegisterCommand(cmdTestPatternMove);
            AppCommand.cmdIAIStopPatternMove.RegisterCommand(cmdStopPatternMove);
        }

        private void ExcmdStopPatternMove(string obj)
        {
            if (AppService.MachineStatus.MachineMode == MachineModeEnum.MANUAL || AppService.MachineStatus.MachineMode == MachineModeEnum.SETTING)
            {
                //StopProgram("12");
                StopProgram("13");
            }
            else
            {
                System.Windows.MessageBox.Show("Please switch to manual/setting mode", "Warning",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ExcmdTestPatternMove(string obj)
        {
            if (AppService.MachineStatus.MachineMode == MachineModeEnum.MANUAL || AppService.MachineStatus.MachineMode == MachineModeEnum.SETTING)
            {
                UpdateIntegerVariable("X1", AppService.IAIMotion.PmX1);
                //UpdateIntegerVariable("X1", "130");
                Thread.Sleep(50);
                UpdateIntegerVariable("Y1", AppService.IAIMotion.PmY1);
                //UpdateIntegerVariable("Y1", "120");
                Thread.Sleep(50);
                UpdateIntegerVariable("DX", AppService.IAIMotion.PmWidthDx);
                //UpdateIntegerVariable("DX", "30");
                Thread.Sleep(50);
                UpdateIntegerVariable("DY", AppService.IAIMotion.PmHeightDy);
                //UpdateIntegerVariable("DY", "-20");
                Thread.Sleep(50);
                UpdateIntegerVariable("PitchY", AppService.IAIMotion.PmPitchy);
                //UpdateIntegerVariable("PitchY", "1");
                Thread.Sleep(50);
                UpdateIntegerVariable("SpeedPattern", AppService.IAIMotion.PmSpeed);
                //UpdateIntegerVariable("SpeedPattern", "60");
                Thread.Sleep(50);
                UpdateRealVariable("AccPattern", AppService.IAIMotion.PmAcc);
                //UpdateRealVariable("AccPattern", "0.25");
                Thread.Sleep(50);
                UpdateRealVariable("DclPattern", AppService.IAIMotion.PmDcl);
                //UpdateRealVariable("DclPattern", "0.25");
                Thread.Sleep(50);
                //Old pattern
                //RunProgram("12");
                // Revert pattern
                RunProgram("13");
            }
            else
            {
                System.Windows.MessageBox.Show("Please switch to manual/setting mode", "Warning",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ExcmdTestMove(string obj)
        {
            AbsoluteSingleAxisMove(AppService.IAIMotion.AxisXSelected, AppService.IAIMotion.AccX, AppService.IAIMotion.DclX, AppService.IAIMotion.SpeedX, "50");
            Thread.Sleep(100);
            AbsoluteSingleAxisMove(AppService.IAIMotion.AxisYSelected, AppService.IAIMotion.AccY, AppService.IAIMotion.DclY, AppService.IAIMotion.SpeedY, "50");

        }

        private void ExcmdJogAxisYForward(string state)
        {
            if (AppService.MachineStatus.MachineMode == MachineModeEnum.MANUAL || AppService.MachineStatus.MachineMode == MachineModeEnum.SETTING)
            {
                if (AppService.IAIMotion.IsSelectedAxisY)
                {
                    if (state == "Press")
                    {
                        RunProgram("10");
                        FlagChange("652", "1");
                    }
                    if (state == "Release")
                    {
                        FlagChange("652", "0");
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Please switch to manual/setting mode", "Warning",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ExcmdJogAxisYBackward(string state)
        {
            if (AppService.MachineStatus.MachineMode == MachineModeEnum.MANUAL || AppService.MachineStatus.MachineMode == MachineModeEnum.SETTING)
            {
                if (AppService.IAIMotion.IsSelectedAxisY)
                {
                    if (state == "Press")
                    {
                        RunProgram("11");
                        FlagChange("653", "1");
                    }
                    if (state == "Release")
                    {
                        FlagChange("653", "0");
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Please switch to manual/setting mode", "Warning",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ExcmdToggleCheckBox(string state)
        {
            if (state == "XCheck")
            {
                bool[] CurrentAxis = { AppService.IAIMotion.IsSelectedAxisX, AppService.IAIMotion.IsSelectedAxisY };
                BoolArrToHex(CurrentAxis);
            }
            if (state == "YCheck")
            {
                bool[] CurrentAxis = { AppService.IAIMotion.IsSelectedAxisX, AppService.IAIMotion.IsSelectedAxisY };
                BoolArrToHex(CurrentAxis);
            }
        }

        private void ExcmdAlarmReset()
        {
            ResetErr();
        }

        private void ExcmdUpdateJogSpeed(string speed)
        {
            if (AppService.MachineStatus.MachineMode == MachineModeEnum.MANUAL || AppService.MachineStatus.MachineMode == MachineModeEnum.SETTING)
            {
                if (speed != null)
                {
                    UpdateIntegerSpeedVariable(speed);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Please switch to manual/setting mode", "Warning",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ExcmdJogBackward(string state)
        {
            if (AppService.MachineStatus.MachineMode == MachineModeEnum.MANUAL || AppService.MachineStatus.MachineMode == MachineModeEnum.SETTING)
            {
                if (AppService.IAIMotion.IsSelectedAxisX)
                {
                    if (state == "Press")
                    {
                        RunProgram("11");
                        FlagChange("651", "1");
                    }
                    if (state == "Release")
                    {
                        FlagChange("651", "0");
                    }
                }

            }
            else
            {
                System.Windows.MessageBox.Show("Please switch to manual/setting mode", "Warning",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ExcmdJogForward(string state)
        {
            if (AppService.MachineStatus.MachineMode == MachineModeEnum.MANUAL || AppService.MachineStatus.MachineMode == MachineModeEnum.SETTING)
            {
                if (AppService.IAIMotion.IsSelectedAxisX)
                {
                    if (state == "Press")
                    {
                        RunProgram("10");
                        FlagChange("650", "1");
                    }
                    if (state == "Release")
                    {
                        FlagChange("650", "0");
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Please switch to manual/setting mode", "Warning",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ExcmdRelativeMove()
        {
            if (AppService.MachineStatus.MachineMode == MachineModeEnum.MANUAL || 
                AppService.MachineStatus.MachineMode == MachineModeEnum.SETTING)
            {
                if (AppService.IAIMotion.IsSelectedAxisX & AppService.IAIMotion.IsSelectedAxisY)
                {
                    if (AppService.IAIMotion.AxisXValue != null & AppService.IAIMotion.AxisYValue != null)
                    {
                        RelativeMove(AppService.IAIMotion.AxisXValue, AppService.IAIMotion.AxisYValue, "0");
                    }

                }

                else if (AppService.IAIMotion.IsSelectedAxisX)
                {
                    if (AppService.IAIMotion.AxisXValue != null)
                    {
                        RelativeMove(AppService.IAIMotion.AxisXValue, "0", "0");
                    }
                }
                else if (AppService.IAIMotion.IsSelectedAxisY)
                {
                    if (AppService.IAIMotion.AxisYValue != null)
                    {
                        RelativeMove(AppService.IAIMotion.AxisYValue, "0", "0");
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Please switch to manual/setting mode", "Warning",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ExcmdAbsoluteMove(string state)
        {
            if (AppService.MachineStatus.MachineMode == MachineModeEnum.MANUAL
                || AppService.MachineStatus.MachineMode == MachineModeEnum.SETTING)
            {
                if (_adam6052Module1.CheckSafetyCondition())
                {
                    if (state == "ABS")
                    {
                        if (AppService.IAIMotion.IsSelectedAxisX & AppService.IAIMotion.IsSelectedAxisY &
                            !string.IsNullOrEmpty(AppService.IAIMotion.AxisXValue) &
                            !string.IsNullOrEmpty(AppService.IAIMotion.AxisYValue))
                        {
                            AbsoluteMove(AppService.IAIMotion.AxisXValue, AppService.IAIMotion.AxisYValue, "0");
                        }
                        else if (AppService.IAIMotion.IsSelectedAxisY &
                            !string.IsNullOrEmpty(AppService.IAIMotion.AxisYValue))
                        {
                            AbsoluteMove(AppService.IAIMotion.AxisYValue, "0", "0");
                        }
                        else if (AppService.IAIMotion.IsSelectedAxisX &
                            !string.IsNullOrEmpty(AppService.IAIMotion.AxisXValue))
                        {
                            AbsoluteMove(AppService.IAIMotion.AxisXValue, "0", "0");
                        }
                    }
                    if (state == "home")
                    {
                        AbsoluteMove("0", "0", "0");
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Please switch to manual/setting mode", "Warning",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ExcmdServoOnOff(string state)
        {
            if (state == "SVON")
            {
                ServoON_OFF(1);
            }
            if (state == "SVOFF")
            {
                ServoON_OFF(0);
            }
            //if (AppService.MachineStatus.MachineMode == MachineModeEnum.MANUAL ||
            //    AppService.MachineStatus.MachineMode == MachineModeEnum.SETTING ||
            //    AppService.MachineStatus.MachineInitState == MachineInitStateEnum.INITIALIZING)
            //{
            //    if (state == "SVON")
            //    {
            //        ServoON_OFF(1);
            //    }
            //    if (state == "SVOFF")
            //    {
            //        ServoON_OFF(0);
            //    }

            //}
            //else
            //{
            //    System.Windows.MessageBox.Show("Please switch to manual/setting mode", "Warning",
            //                        MessageBoxButton.OK, MessageBoxImage.Warning);
            //}
        }
    }
}
