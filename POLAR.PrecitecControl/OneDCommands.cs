using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CHRocodileLib.Data;
using System.Windows.Shapes;
using CHRocodileLib;
using System.Threading;
using PORLA.HMI.Service.Enums;
using PORLA.HMI.Service;

namespace POLAR.PrecitecControl
{
    public partial class OneDAreaScan : BindableBase
    {
        public DelegateCommand<string> SendSettingParameterCmd { get; set; }
        public DelegateCommand<string> SetTriggerEncPosCmd { get; set; }
        public DelegateCommand<string> StartScanCmd { get; set; }
        public DelegateCommand<string> OneDSendCmd { get; set; }
        public DelegateCommand<string> OneDTriggerEvent { get; set; }

        private bool _canStart;
        public bool CanStart
        {
            get { return _canStart; }
            set { SetProperty(ref _canStart, value); }
        }
        public void InitializeCommand()
        {
            SendSettingParameterCmd = new DelegateCommand<string>(ExSendSettingParameter);
            SetTriggerEncPosCmd = new DelegateCommand<string>(ExSetTriggerEncPos);
            StartScanCmd = new DelegateCommand<string>(ExStartScanCmd).ObservesCanExecute(() => CanStart);
            OneDSendCmd = new DelegateCommand<string>(ExOneDSendCmd);
            OneDTriggerEvent = new DelegateCommand<string>(ExOneDTriggerEvent);

            AppCommand.cmdOneDSendSetting.RegisterCommand(SendSettingParameterCmd);
            AppCommand.cmdOneDSetEncPos.RegisterCommand(SetTriggerEncPosCmd);
            AppCommand.cmdOneDStartScan.RegisterCommand(StartScanCmd);
            AppCommand.cmdOneDSendCmd.RegisterCommand(OneDSendCmd);
            AppCommand.cmdOneDTriggerEvent.RegisterCommand(OneDTriggerEvent);
        }

        private void ExOneDTriggerEvent(string para)
        {
            if (para == "log")
            {
                AppService.DutBarcode = "test123";
                SaveMultiPointDataIntoFile();
            }
            if (para == "test1")
            {
                multiScanData = new List<Data>();
                // TRG
                //SendCommand("TRG");
                SendCommand("TRE");
                SendCommand("AVD 10");
                //Conn.FlushConnectionBuffer();
                ScanData = null;
                // Delay
                //Thread.Sleep(10);
                // STR
                SendCommand("STR");
                // Delay
                //Thread.Sleep(10);
                //// Collect data
                Conn.StartRecording(1);
                Thread.Sleep(100);
                ScanData = Conn.GetNextSamples();
                Conn.StopRecording();
                multiScanData.Add(ScanData);
                //Conn.StopRecording();
                // Set back to free run mode
                Conn.Exec(CHRocodileLib.CmdID.DeviceTriggerMode, CHRocodileLib.TriggerMode.FreeRun);
                // Save File
                //SaveData(256);
                ScanData = null;
                Logging(new LogInfor(LogType.PricitecController, $"OneD - Scan and add first time"));
            }
            if (para == "test2")
            {
                // TRG
                //SendCommand("TRG");
                SendCommand("TRE");
                //SendCommand("AVD 10");
                Conn.FlushConnectionBuffer();
                ScanData = null;
                // Delay
                //Thread.Sleep(10);
                // STR
                SendCommand("STR");
                // Delay
                //Thread.Sleep(10);
                //// Collect data
                Conn.StartRecording(1);
                Thread.Sleep(100);
                ScanData = Conn.GetNextSamples();
                Conn.StopRecording();
                multiScanData.Add(ScanData);
                //_arrayData[1] = ScanData;
                //Conn.StopRecording();
                // Set back to free run mode
                Conn.Exec(CHRocodileLib.CmdID.DeviceTriggerMode, CHRocodileLib.TriggerMode.FreeRun);
                // Save File
                //SaveData(256);

                ScanData = null;
                Logging(new LogInfor(LogType.PricitecController, $"OneD - Scan and add second time"));
            }
            if (para == "test3")
            {
                // TRG
                //SendCommand("TRG");
                SendCommand("TRE");
                SendCommand("AVD 10");
                //Conn.FlushConnectionBuffer();
                ScanData = null;
                // Delay
                //Thread.Sleep(10);
                // STR
                SendCommand("STR");
                // Delay
                //Thread.Sleep(10);
                //// Collect data
                Conn.StartRecording(1);
                Thread.Sleep(100);
                ScanData = Conn.GetNextSamples();
                Conn.StopRecording();
                multiScanData.Add(ScanData);
                //_arrayData[1] = ScanData;
                //Conn.StopRecording();
                // Set back to free run mode
                Conn.Exec(CHRocodileLib.CmdID.DeviceTriggerMode, CHRocodileLib.TriggerMode.FreeRun);
                // Save File
                //SaveData(256);

                ScanData = null;
                Logging(new LogInfor(LogType.PricitecController, $"OneD - Scan and add second time"));
            }
            if (para == "manu")
            {
                //SendCommand("TRG");
                SendCommand("TRE");
                SendCommand("AVD 1");
                Conn.FlushConnectionBuffer();
                ScanData = null;
                // Delay
                //Thread.Sleep(10);
                // STR
                SendCommand("STR");
                // Delay
                //Thread.Sleep(10);
                //// Collect data
                //Conn.StartDataStream();
                //ScanData = Conn.GetNextSamples();
                //Conn.StopDataStream();
                Conn.StartRecording(1);
                ScanData = Conn.GetNextSamples();
                Conn.StopRecording();
                // Set back to free run mode
                Conn.Exec(CHRocodileLib.CmdID.DeviceTriggerMode, CHRocodileLib.TriggerMode.FreeRun);
                // Save File
                SaveData(256);
                ScanData = null;
                Logging(new LogInfor(LogType.PricitecController, $"OneD - Scan and add second time"));
            }
        }

        private void ExOneDSendCmd(string cmd)
        {
            SendCommand(cmd);
        }

        private void ExStartScanCmd(string state)
        {
            if (state == "start") 
            {
                int _lineScanNum = int.Parse(AppService.PrecitecService.OneDScanLineNo);
                int _numberSampleCount = int.Parse(AppService.PrecitecService.OneDSampleCountPerLine);
                string _allSampleCount = (_lineScanNum * _numberSampleCount).ToString();
                StartScan(_allSampleCount);
            }
            if (state == "stop")
            {
                StopScan();
            }
        }

        private void ExSetTriggerEncPos(string axis)
        {
            if (axis == "x")
            {
                SetTriggerEncoderPos(AppService.PrecitecService.OneDValueEncX,axis);
            }
            if (axis == "y")
            {
                SetTriggerEncoderPos(AppService.PrecitecService.OneDValueEncY, axis);
            }
        }

        private void ExSendSettingParameter(string axis)
        {
            if (axis == "x")
            {
                ConvertEncXMmToCount();
                SendTriggerSetting(axis, AppService.PrecitecService.OneDStartPos, AppService.PrecitecService.OneDStopPos,
                AppService.PrecitecService.OneDInterval, true);
                CanStart = true;
            }
            if (axis == "y")
            {
                ConvertEncYMmToCount();
                SendTriggerSetting(axis, AppService.PrecitecService.OneDStartPos, AppService.PrecitecService.OneDStopPos,
                AppService.PrecitecService.OneDInterval, false);
            }
        }

        private void ConvertEncYMmToCount()
        {
            int _startPos = 0;
            int _stopPos = 0;
            float _step = 0.0f;
            // 1mm = 200count
            _startPos = (int.Parse(AppService.PrecitecService.OneDStartPos)) * 200;
            _stopPos = (int.Parse(AppService.PrecitecService.OneDStopPos)) * 200;
            _step = (float.Parse(AppService.PrecitecService.OneDInterval)) * 200;

            AppService.PrecitecService.OneDStartPos = _startPos.ToString();
            AppService.PrecitecService.OneDStopPos = _stopPos.ToString();
            AppService.PrecitecService.OneDInterval = _step.ToString();
        }

        private void ConvertEncXMmToCount()
        {
            float _startPos = 0;
            float _stopPos = 0;
            float _step = 0.0f;
            
            // 1mm = 200count
            _startPos = (float.Parse(AppService.PrecitecService.OneDStartPos)) * 200;
            _stopPos = (float.Parse(AppService.PrecitecService.OneDStopPos)) * 200;
            _step = (float.Parse(AppService.PrecitecService.OneDInterval)) * 200;
            // Number of line scan
            float _dy = (float.Parse(AppService.IAIMotion.PmHeightDy)) * 200;
            float _pitchy = (float.Parse(AppService.IAIMotion.PmPitchy)) * 200;

            AppService.PrecitecService.OneDSampleCountPerLine = ((int)((_stopPos - _startPos) / _step + 1)).ToString();
            //AppService.PrecitecService.OneDSampleCountPerLine = ((int)((_startPos - _stopPos) / _step + 1)).ToString();
            AppService.PrecitecService.OneDScanLineNo = ((int)((_dy/_pitchy)+1)).ToString();

            AppService.PrecitecService.OneDStartPos = _startPos.ToString();
            AppService.PrecitecService.OneDStopPos = _stopPos.ToString();
            AppService.PrecitecService.OneDInterval = _step.ToString();
        }
    }
}
