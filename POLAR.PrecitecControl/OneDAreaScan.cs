using CHRocodileLib;
using POLAR.CompositeAppCommand;
using POLAR.EventAggregator;
using PORLA.HMI.Service;
using PORLA.HMI.Service.Enums;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace POLAR.PrecitecControl
{
    public partial class OneDAreaScan : BindableBase, IOneDAreaScan
    {
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IAppService _appService;
        public IAppService AppService
        {
            get { return _appService; }
            set { SetProperty(ref _appService, value); }
        }

        private ICompositeAppCommand _appCommand;
        public ICompositeAppCommand AppCommand
        {
            get { return _appCommand; }
            set { SetProperty(ref _appCommand, value); }
        }
        IEventAggregator _eventAggregator;

        SynchronousConnection Conn;

        private CHRocodileLib.Data ScanData = null;
        private List<Data> multiScanData = null;
        private bool InProcess;
        private System.Timers.Timer timerProcess;
        private DispatcherTimer dispatcherTimer;
        private int[] OutputSignals;
        private int AllSampleCount;
        const int Data_Length = 1000;
        private int ScanLineNo, LineSampleCount;
        private Mutex mutex = new Mutex(false);
        public OneDAreaScan(IAppService appService, ICompositeAppCommand appCommand, IEventAggregator eventAggregator)
        {
            _appService = appService;
            _appCommand = appCommand;
            _eventAggregator = eventAggregator;
            InitializeCommand();
            
        }

        public async Task<bool> InitializeOneD()
        {
            bool bConnect = false;
            //connect to device
            try
            {
                timerProcess = new System.Timers.Timer();
                //Open connection in synchronous mode
                var DeviceType = CHRocodileLib.DeviceType.Chr2;
                string strConInfo = "192.168.170.2";
                Conn = new SynchronousConnection(strConInfo, DeviceType);
                await SetupDevice(DeviceType);
                bConnect = true;

                AppService.PrecitecService.Connected = true;
                timerProcess.Elapsed += TimerProcess_Elapsed;
                timerProcess.Interval = 20;
                dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Interval = TimeSpan.FromMilliseconds(100);
                dispatcherTimer.Tick += DispatcherTimer_Tick;
                Logging(new LogInfor(LogType.PricitecController, "OneD - Precitec: Initialized!"));
                logger.Info("OneD - Precitec: Initialized!");
            }
            catch (Exception _e)
            {
                logger.Error($"OneD - {_e.Message}");
                Logging(new LogInfor(LogType.PricitecController, $"OneD - {_e.Message}"));
            }
            return bConnect;
        }

        private async void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            // Spectrum download
            if (AppService.PrecitecService.OneDSelected || AppService.PrecitecService.OneDMPSelected)
            {
                try
                {
                    await Task.Run(() => 
                    {
                        var specType = SpecType.FT;
                        Response oRsp;
                        //here only download the specturm of the first channel
                        oRsp = Conn.Exec(CmdID.DownloadSpectrum, specType);
                        //the last parameter of the response is the spectrum data
                        var aBytes = oRsp.GetParam<byte[]>(oRsp.ParamCount - 1);
                        //convert to 16bit data
                        Int16[] SpecData = new Int16[aBytes.Length / 2];
                        Buffer.BlockCopy(aBytes, 0, SpecData, 0, aBytes.Length);
                        int len = Math.Min(Data_Length, SpecData.Length);

                        for (short i = 0; i < len; i++)
                        {
                            AppService.PrecitecService.DataPoints[i] = SpecData[i];
                        }
                    });
                    _eventAggregator.GetEvent<SpectrumDowloadEvent>().Publish(true);
                }
                catch
                {
                    Debug.Fail("Cannot set download spectrum");
                }
            }
        }

        private async Task SetupDevice(DeviceType deviceType)
        {
            // Set method measure: interferometer or confocal
            SetEncoderConfig();
            SetUpMeasuringMethod();
            SetUpScanrate();
            await SetDeviceOutput();
        }

        private void SetEncoderConfig()
        {
            try
            {
                // Stop FSS mode: SCAN 6 0
                SendCommand("SCAN 6 0");
                // ENABLE EXTERNAL ENCODER X AXIS: ENC 0 1 15
                SendCommand("ENC 0 1 15");
                // ENABLE EXTERNAL ENCODER Y AXIS: ENC 1 1 15
                SendCommand("ENC 1 1 15");
                // NUMBER OF PEAKS: NOP 10
                SendCommand("NOP 10");
                // PEAKS ORDERING : POD 1
                SendCommand("POD 1");
                // LAMP INTERSITY: LAI 95
                SendCommand("LAI 95");
                // DETECTOR GAIN SETTING: GAN 1
                SendCommand("GAN 1");
                // SET REFRACTIVE INDICES: SRI 1.5
                SendCommand("SRI 1.5");
                // ABBE NUMBER: ABE 0
                SendCommand("ABE 0");
                // AVERAGE VALUES OF SPECTRA: AVS 1
                SendCommand("AVS 1");
                // AVERAGE VALUES OF MEASUREMENT DATA: AVD 1
                SendCommand("AVD 1");
                // ACTIVATE THE DETECTION LIMIT
                SendCommand("LMA 1");
                // DEFINE FILTER FOR PEAK DETECTION ALGORITHM
                SendCommand("FLTI -64 128 -64");
                SendCommand("FLTC -64 128 -64");
            }
            catch (Exception _e)
            {
                logger.Error(_e);
            }
        }

        private void SetUpScanrate()
        {
            try
            {
                // 50000Hz
                string scanRate = "50000";
                float nSHZ = float.Parse(scanRate);
                var oRsp = Conn.Exec(CHRocodileLib.CmdID.ScanRate, nSHZ);
                Logging(new LogInfor(LogType.PricitecController, $"OneD_AreaScan - Set scan rate {scanRate}"));
                logger.Info($"OneD_AreaScan - Set scan rate {scanRate}");
            }
            catch(Exception _e)
            {
                logger.Error(_e);
            }
        }

        private void SetUpMeasuringMethod()
        {
            try
            {
                MeasurementMode nMMD = MeasurementMode.Interferometric;
                var oRsp = Conn.Exec(CHRocodileLib.CmdID.MeasuringMethod, nMMD);
                Logging(new LogInfor(LogType.PricitecController, $"OneD_AreaScan - Set measure mode - Interferometric - {oRsp}"));
                logger.Info($"OneD_AreaScan - Set measure mode - Interferometric - {oRsp}");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private string signalId = "65,66,256,257,264,265,272,273,280,281,288,289,296,297,304,305,312,313";
        private async Task SetDeviceOutput()
        {
            try
            {
                // $SODX 65 66 67 68 69 256 82 83 264 272;
                // 256 -  Thickness 1, 264 -  Thickness 2, 272 - Thickness 3, 83 Sample Counter, 82 Intensity, 65-69 - Encoder
                
                char[] delimiters = new char[] { ' ', ',', ';' };
                string[] aTemp = signalId.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                int[] aTempSig = Array.ConvertAll(aTemp, int.Parse);
                Array.Sort(aTempSig);
                await Task.Run(() =>
                {
                    var oRsp = Conn.Exec(CHRocodileLib.CmdID.OutputSignals, aTempSig);
                    OutputSignals = oRsp.GetParam<int[]>(0);
                });
                string _s = String.Join(",", OutputSignals);
                Logging(new LogInfor(LogType.PricitecController, $"OneD_AreaScan - Set Device Ouput - {_s}"));
                logger.Info($"OneD_AreaScan - Set Device Ouput - {_s}");
            }
            catch (Exception ex)
            {
                logger.Error($"OneD - Can not set output device - {ex}");
                Logging(new LogInfor(LogType.PricitecController, $"OneD - Can not set output device"));
            }
        }

        private void TimerProcess_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //to make sure the last process has been finished
            if (InProcess)
                return;

            InProcess = true;

            //Read Data
            ScanData = Conn.GetNextSamples();

            //get total number of recorded data, TotalNumSamples returns the total recorded sample count
            var nTotalSampleCount = ScanData.TotalNumSamples;
            //logger.Info($"OneD - get total sample counts {nTotalSampleCount}");
            
            if (nTotalSampleCount == AllSampleCount)
            {
                StopScan();
            }
            InProcess = false;

        }
        
        public async Task StartScan(string lineNo, string SampleNo, bool triggerOnReturn)
        {
            if (timerProcess.Enabled)
            {
                StopScan();
            }
            else
            {
                try
                {
                    ScanLineNo = int.Parse(lineNo);
                    LineSampleCount = int.Parse(SampleNo);
                    if (triggerOnReturn)
                        ScanLineNo *= 2;

                    AllSampleCount = ScanLineNo * LineSampleCount;

                    if (AllSampleCount == 0)
                        return;

                    await Task.Run(() =>
                    {
                        //Set trigger settings
                        //SendTriggerSetting("0", "1000", "100", true);
                        //use trigger each mode
                        Conn.Exec(CmdID.DeviceTriggerMode, (int)TriggerMode.TriggerEach);
                        //start recording modes
                        Conn.StartRecording(AllSampleCount);
                        //reset record data
                        ScanData = null;
                        InProcess = false;
                        //use timer to display data
                        timerProcess.Start();

                        //PPaint.Invalidate();
                    });
                }
                catch (Exception ex)
                {
                    logger.Error($"OneD - Cannot set scan related parameters - {ex}");
                    Logging(new LogInfor(LogType.PricitecController, $"OneD - Cannot set scan related parameters - {ex}"));

                }

            }
        }
        public async Task StartScan(string SampleNo)
        {
            if (timerProcess.Enabled)
            {
                StopScan();
            }
            //if (dispatcherTimer.IsEnabled)
            //{
            //    StopScan();
            //}
            else
            {
                try
                {
                    await Task.Run(() =>
                    {
                        AllSampleCount = int.Parse(SampleNo);
                        //Set trigger settings
                        //SendTriggerSetting("0", "1000", "100", true); 
                        //use trigger each mode
                        Conn.Exec(CmdID.DeviceTriggerMode, (int)TriggerMode.TriggerEach);
                        //start recording modes
                        Conn.StartRecording(AllSampleCount);
                        //reset record data
                        ScanData = null;
                        InProcess = false;
                        //use timer to display data
                        timerProcess.Start();
                        //dispatcherTimer.Start();
                        //PPaint.Invalidate();
                    });
                }
                catch (Exception ex)
                {
                    logger.Error($"OneD - Cannot set scan related parameters - {ex}");
                    Logging(new LogInfor(LogType.PricitecController, $"OneD - Cannot set scan related parameters - {ex}"));

                }

            }
        }

        public bool SendTriggerSetting(string axis, string StartPos, string StopPos,
                                       string intervalEncCount, bool triggerOnReturn)
        {
            bool state = false;
            try
            {
                //use encoder to trigger
                Conn.Exec(CmdID.EncoderTriggerEnabled, 1);
                //set encoder trigger property
                int nAxis = 0;
                if (axis == "x")
                {
                    nAxis = 0;
                }
                if (axis == "y")
                {
                    nAxis = 1;
                }

                // Set encoder trigger setting
                int nStartPos = int.Parse(StartPos);
                int nStopPos = int.Parse(StopPos);
                float nInterval = float.Parse(intervalEncCount, CultureInfo.InvariantCulture);
                int bTriggerOnReturn = triggerOnReturn ? 1 : 0;
                Conn.Exec(CmdID.EncoderTriggerProperty, nAxis, nStartPos, nStopPos, nInterval, bTriggerOnReturn);

                string SampleNo = ((int)((nStopPos - nStartPos) / nInterval + 1)).ToString();
                state = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return state;

        }
        public bool SendTriggerSetting(string axis, int nStartPos, int nStopPos,
                                       float nInterval, bool triggerOnReturn)
        {
            bool state = false;
            try
            {
                //use encoder to trigger
                Conn.Exec(CmdID.EncoderTriggerEnabled, 1);
                //set encoder trigger property
                int nAxis = 0;
                if (axis == "x")
                {
                    nAxis = 0;
                }
                if (axis == "y")
                {
                    nAxis = 1;
                }

                int bTriggerOnReturn = triggerOnReturn ? 1 : 0;
                Conn.Exec(CmdID.EncoderTriggerProperty, nAxis, nStartPos, nStopPos, nInterval, bTriggerOnReturn);
                string SampleNo = ((int)((nStopPos - nStartPos) / nInterval + 1)).ToString();
                state = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return state;

        }
        public void CloseConnection()
        {
            //StopScan();
            timerProcess.Stop();
            while (InProcess)
                Task.Delay(20);
            if (Conn != null)
            {
                Conn.Close();
            }
            Conn = null;
        }

        public async Task StopScan()
        {
            if (!timerProcess.Enabled)
                return;

            timerProcess.Stop();
            //quit recording modes,
            //StopRecording also returns recorded data object,
            //which is the same as the data object from GetNextSamples in the timer routine,
            Conn.StopRecording();
            try
            {
                await SaveDataIntoFile();
                // Set back to free run mode
                Conn.Exec(CHRocodileLib.CmdID.DeviceTriggerMode, CHRocodileLib.TriggerMode.FreeRun);
            }
            catch
            {
                logger.Error($"OneD - Cannot set back to free run mode.");
                Logging(new LogInfor(LogType.PricitecController, $"OneD - Cannot set back to free run mode."));
            }

        }
        public void StopRecording()
        {
            if (!timerProcess.Enabled)
                return;
            timerProcess.Stop();
            Conn.StopRecording();
            if (AppService.PrecitecService.OneDMPSelected)
            {
                // Set back to free run mode
                Conn.Exec(CHRocodileLib.CmdID.DeviceTriggerMode, CHRocodileLib.TriggerMode.FreeRun);
            }
        }
        private async Task SaveDataIntoFile()
        {
            string _fixname = $"{AppService.DutBarcode}_{AppService.RecipeService.NumberOfRow1D}x{AppService.RecipeService.NumberOfColumn1D}_{AppService.RecipeService.DYPosition}x{AppService.RecipeService.DXPosition}_{AppService.RecipeService.RecipeName}";
            StringBuilder thickness1 = new StringBuilder();
            thickness1.Append(@"Output\");
            thickness1.Append($"{_fixname}_Thickness1_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");
            StringBuilder thickness2 = new StringBuilder();
            thickness2.Append(@"Output\");
            thickness2.Append($"{_fixname}_Thickness2_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");
            StringBuilder thickness3 = new StringBuilder();
            thickness3.Append(@"Output\");
            thickness3.Append($"{_fixname}_Thickness3_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");
            StringBuilder thickness4 = new StringBuilder();
            thickness4.Append(@"Output\");
            thickness4.Append($"{_fixname}_Thickness4_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");
            StringBuilder thickness5 = new StringBuilder();
            thickness5.Append(@"Output\");
            thickness5.Append($"{_fixname}_Thickness5_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");
            StringBuilder thickness6 = new StringBuilder();
            thickness6.Append(@"Output\");
            thickness6.Append($"{_fixname}_Thickness6_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");
            StringBuilder thicknessAll = new StringBuilder();
            thicknessAll.Append(@"Output\");
            thicknessAll.Append($"{_fixname}_DataSummary_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");
            // For testing
            StringBuilder thickness7 = new StringBuilder();
            thickness7.Append(@"Output\");
            thickness7.Append($"{_fixname}_Thickness7_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");
            StringBuilder thickness8 = new StringBuilder();
            thickness8.Append(@"Output\");
            thickness8.Append($"{_fixname}_Thickness8_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");

            string pathName1 = System.AppDomain.CurrentDomain.BaseDirectory + thickness1;
            string pathName2 = System.AppDomain.CurrentDomain.BaseDirectory + thickness2;
            string pathName3 = System.AppDomain.CurrentDomain.BaseDirectory + thickness3;
            string pathName4 = System.AppDomain.CurrentDomain.BaseDirectory + thickness4;
            string pathName5 = System.AppDomain.CurrentDomain.BaseDirectory + thickness5;
            string pathName6 = System.AppDomain.CurrentDomain.BaseDirectory + thickness6;
            string pathNameAll = System.AppDomain.CurrentDomain.BaseDirectory + thicknessAll;
            // For testing 
            string pathName7 = System.AppDomain.CurrentDomain.BaseDirectory + thickness7;
            string pathName8 = System.AppDomain.CurrentDomain.BaseDirectory + thickness8;

            var fname1Task = SaveDataAsync(1, pathName1);
            var fname2Task = SaveDataAsync(2, pathName2);
            var fname3Task = SaveDataAsync(3, pathName3);
            var fname4Task = SaveDataAsync(4, pathName4);
            var fname5Task = SaveDataAsync(5, pathName5);
            var fname6Task = SaveDataAsync(6, pathName6); 
            var fname7Task = SaveDataAsync(7, pathName7);
            var fname8Task = SaveDataAsync(8, pathName8);
            //==========
            var fnameAllTask = SaveDataAsync(100, pathNameAll); // 100: get all thickness into one file

            List<Task<string>> tasks = new List<Task<string>>
            {
                fname1Task,
                fname2Task,
                fname3Task,
                fname4Task,
                fname5Task,
                fname6Task,
                fname7Task,
                fname8Task,
                fnameAllTask
            };
            string[] fNames = await Task.WhenAll(tasks);
            AppService.PrecitecService.ScanDone = true;
            AppService.DutBarcode = "";
            Logging(new LogInfor(LogType.PricitecController, $"The raw scan data has been written to {fNames[0]}, {fNames[1]}, {fNames[2]}," +
                                                             $"{fNames[3]}, {fNames[4]}, {fNames[5]}, {fNames[6]}"));
        }
        public async Task SaveMultiPointDataIntoFile()
        {

            StringBuilder thickness1 = new StringBuilder();
            thickness1.Append(@"Output\");
            thickness1.Append($"{AppService.DutBarcode}_1DMulti_Thickness1_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");
            StringBuilder thickness2 = new StringBuilder();
            thickness2.Append(@"Output\");
            thickness2.Append($"{AppService.DutBarcode}_1DMulti_Thickness2_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");
            StringBuilder thickness3 = new StringBuilder();
            thickness3.Append(@"Output\");
            thickness3.Append($"{AppService.DutBarcode}_1DMulti_Thickness3_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");
            StringBuilder thickness4 = new StringBuilder();
            thickness4.Append(@"Output\");
            thickness4.Append($"{AppService.DutBarcode}_1DMulti_Thickness4_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");
            StringBuilder thickness5 = new StringBuilder();
            thickness5.Append(@"Output\");
            thickness5.Append($"{AppService.DutBarcode}_1DMulti_Thickness5_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");
            StringBuilder thickness6 = new StringBuilder();
            thickness6.Append(@"Output\");
            thickness6.Append($"{AppService.DutBarcode}_1DMulti_Thickness6_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");
            StringBuilder thicknessAll = new StringBuilder();
            thicknessAll.Append(@"Output\");
            thicknessAll.Append($"{AppService.DutBarcode}_1DMulti_DataSummary_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");
            // For testing
            StringBuilder thickness7 = new StringBuilder();
            thickness7.Append(@"Output\");
            thickness7.Append($"{AppService.DutBarcode}_1DMulti_Thickness7_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");
            StringBuilder thickness8 = new StringBuilder();
            thickness8.Append(@"Output\");
            thickness8.Append($"{AppService.DutBarcode}_1DMulti_Thickness8_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");

            string pathName1 = System.AppDomain.CurrentDomain.BaseDirectory + thickness1;
            string pathName2 = System.AppDomain.CurrentDomain.BaseDirectory + thickness2;
            string pathName3 = System.AppDomain.CurrentDomain.BaseDirectory + thickness3;
            string pathName4 = System.AppDomain.CurrentDomain.BaseDirectory + thickness4;
            string pathName5 = System.AppDomain.CurrentDomain.BaseDirectory + thickness5;
            string pathName6 = System.AppDomain.CurrentDomain.BaseDirectory + thickness6;
            string pathNameAll = System.AppDomain.CurrentDomain.BaseDirectory + thicknessAll;
            // For testing 
            string pathName7 = System.AppDomain.CurrentDomain.BaseDirectory + thickness7;
            string pathName8 = System.AppDomain.CurrentDomain.BaseDirectory + thickness8;

            var fname1Task = SaveMultiDataPointAsync(1, pathName1);
            var fname2Task = SaveMultiDataPointAsync(2, pathName2);
            var fname3Task = SaveMultiDataPointAsync(3, pathName3);
            var fname4Task = SaveMultiDataPointAsync(4, pathName4);
            var fname5Task = SaveMultiDataPointAsync(5, pathName5);
            var fname6Task = SaveMultiDataPointAsync(6, pathName6);
            var fname7Task = SaveMultiDataPointAsync(7, pathName7);
            var fname8Task = SaveMultiDataPointAsync(8, pathName8);
            //==========
            var fnameAllTask = SaveMultiDataPointAsync(100, pathNameAll); // 100: get all thickness into one file

            List<Task<string>> tasks = new List<Task<string>>
            {
                fname1Task,
                fname2Task,
                fname3Task,
                fname4Task,
                fname5Task,
                fname6Task,
                fname7Task,
                fname8Task,
                fnameAllTask
            };
            string[] fNames = await Task.WhenAll(tasks);
            AppService.PrecitecService.ScanDone = true;
            AppService.DutBarcode = "";
            Logging(new LogInfor(LogType.PricitecController, $"The raw scan data has been written to {fNames[0]}, {fNames[1]}, {fNames[2]}," +
                                                             $"{fNames[3]}, {fNames[4]}, {fNames[5]}, {fNames[6]}"));
        }
        private void SaveData(ushort thicknesIndex)
        {
            StringBuilder thickness = new StringBuilder();
            thickness.Append(@"Output\");
            thickness.Append($"PointSs_SignalId_{thicknesIndex}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");
            string pathName = System.AppDomain.CurrentDomain.BaseDirectory + thickness;
            
            int sigX = ScanData.SignalIndex(65);
            int sigY = ScanData.SignalIndex(66);
            int sigThickness = ScanData.SignalIndex(thicknesIndex);
            logger.Info($"OneD_AreaScan - Save Data with index: 65 - {sigX}, 66 -  {sigY}, {thicknesIndex} - {sigThickness}");
            try
            {
                #region using writer
                //StreamWriter writer = new StreamWriter(pathName);
                //var nSigCount = ScanData.Info.SignalGenInfo.GlobalSignalCount
                //    + ScanData.Info.SignalGenInfo.PeakSignalCount;
                ////reread all the samples, save...
                //ScanData.Rewind();
                //foreach (var s in ScanData.Samples())
                //{
                //    StringBuilder sb = new StringBuilder();
                //    sb.Append(s.Get(sigX) + ",");
                //    sb.Append(s.Get(sigY) + ",");
                //    sb.Append(s.Get(sigThickness));
                //    //for (int j = 0; j < nSigCount; j++)
                //    //{
                //    //    if (j < ScanData.Info.SignalGenInfo.GlobalSignalCount)
                //    //        sb.Append(s.Get(j) + ", ");
                //    //    else
                //    //    {
                //    //        for (int k = 0; k < ScanData.Info.SignalGenInfo.ChannelCount; k++)
                //    //            sb.Append(s.Get(j, k) + ", ");
                //    //    }
                //    //}
                //    writer.WriteLine(sb.ToString());
                //}
                //writer.Dispose();
                #endregion
                ScanData.Rewind();
                using (var fs = File.CreateText(pathName))
                {
                    foreach (var s in ScanData.Samples())
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(s.Get(sigX) + ",");
                        sb.Append(s.Get(sigY) + ",");
                        sb.Append(s.Get(sigThickness));
                        fs.WriteLine(sb);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"OneD - Save Data {ex.Message}.");
                Logging(new LogInfor(LogType.PricitecController, $"OneD - Save Data {ex.Message}.."));

            }
        }
        private Task<string> SaveDataAsync(byte thicknessIndex, string filePath)
        {
            return Task.Run(() =>
            {
                try
                {
                    mutex.WaitOne();
                    var NF = CultureInfo.GetCultureInfo("de-DE").NumberFormat;
                    // Convert to X, Y Fixture
                    double _xOffset = double.Parse(AppService.IAIMotion.XAxisSave);
                    double _yOffset = double.Parse(AppService.IAIMotion.YAxisSave);
                    double _xOrgRecipe = double.Parse(AppService.RecipeService.XOriginalPosition);
                    double _yOrgRecipe = double.Parse(AppService.RecipeService.YOriginalPosition);
                    if (thicknessIndex < 100)
                    {
                        int sigX = ScanData.SignalIndex(65);
                        int sigY = ScanData.SignalIndex(66);
                        int sigThickness = ScanData.SignalIndex((ushort)(thicknessIndex * 8 + 248));
                        ScanData.Rewind();
                        using (var fs = File.CreateText(filePath))
                        {
                            foreach (var S in ScanData.Samples())
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.Append(_xOffset - _xOrgRecipe + (_xOrgRecipe - (S.Get(sigX)/-200)) + ",");
                                sb.Append(_yOffset - _yOrgRecipe + (_yOrgRecipe - (S.Get(sigY)/200)) + ",");
                                sb.Append(S.Get(sigThickness));
                                fs.WriteLine(sb);
                            }
                        }
                    }
                    else
                    {
                        using (var fs = File.CreateText(filePath))
                        {
                            // get index signal
                            int sigX = ScanData.SignalIndex(65),
                                sigY = ScanData.SignalIndex(66);
                            var nSigCount = ScanData.Info.SignalGenInfo.GlobalSignalCount
                                          + ScanData.Info.SignalGenInfo.PeakSignalCount;

                            //reread all the samples, save...
                            ScanData.Rewind();
                            string str = "Signals: ";
                            foreach (var (sInfo, sOf) in ScanData.Info.SignalInfos)
                            {
                                str += "; " + sInfo.SignalID;
                            }

                            fs.WriteLine(str);

                            foreach (var s in ScanData.Samples())
                            {
                                StringBuilder sb = new StringBuilder();
                                for (int j = 0; j < nSigCount; j++)
                                {
                                    if (j == sigX)
                                    {
                                        sb.Append(Math.Round(_xOffset - _xOrgRecipe + (_xOrgRecipe - (s.Get(sigX) / -200)), 4) + ",");
                                    }
                                    else if (j == sigY)
                                    {
                                        sb.Append(Math.Round(_yOffset - _yOrgRecipe + (_yOrgRecipe - (s.Get(sigY) / 200)), 4) + ",");
                                    }
                                    else
                                    {
                                        sb.Append(Math.Round(s.Get(j), 4) + ",");
                                    }
                                    //if (j < ScanData.Info.SignalGenInfo.GlobalSignalCount)
                                    //{
                                    //    //sb.Append(s.Get(j) + ",");
                                    //    if (j == sigX)
                                    //    {
                                    //        sb.Append(_xOffset - _yOrgRecipe + (_yOrgRecipe - (s.Get(sigX) / -200)) + ",");
                                    //    }
                                    //    if (j == sigY)
                                    //    {
                                    //        sb.Append(_yOffset - _yOrgRecipe + (_yOrgRecipe - (s.Get(sigY) / 200)) + ",");
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    for (int k = 0; k < ScanData.Info.SignalGenInfo.ChannelCount; k++)
                                    //        sb.Append(s.Get(j, k) + ",");
                                    //}
                                }
                                fs.WriteLine(sb);
                            }
                        }
                    }
                    return filePath;
                }
                finally 
                {
                    mutex.ReleaseMutex();
                }
            });
        }
        private Task<string> SaveMultiDataPointAsync(byte thicknessIndex, string filePath)
        {
            return Task.Run(() =>
            {
                try
                {
                    mutex.WaitOne();
                    var NF = CultureInfo.GetCultureInfo("de-DE").NumberFormat;
                    // Convert to X, Y Fixture
                    double _xOffset = double.Parse(AppService.IAIMotion.XAxisSave);
                    double _yOffset = double.Parse(AppService.IAIMotion.YAxisSave);
                    if (thicknessIndex < 100)
                    {
                        using (var fs = File.CreateText(filePath))
                        {
                            foreach (var scanData in multiScanData)
                            {
                                int sigX = scanData.SignalIndex(65);
                                int sigY = scanData.SignalIndex(66);
                                int sigThickness = scanData.SignalIndex((ushort)(thicknessIndex * 8 + 248));
                                //scanData.Rewind();
                                StringBuilder sb = new StringBuilder();
                                foreach (var S in scanData.Samples())
                                {
                                    //sb.Append(S.Get(sigX) + ",");
                                    //sb.Append(S.Get(sigY) + ",");
                                    sb.Append(_xOffset - (S.Get(sigX) / -200) + ",");
                                    sb.Append(_yOffset - (S.Get(sigY) / 200) + ",");
                                    sb.Append(S.Get(sigThickness));   
                                }
                                fs.WriteLine(sb);
                            }
                        } 
                    }
                    else
                    {
                        using (var fs = File.CreateText(filePath))
                        {
                            int sigX = multiScanData[0].SignalIndex(65);
                            int sigY = multiScanData[0].SignalIndex(66);
                            var nSigCount = multiScanData[0].Info.SignalGenInfo.GlobalSignalCount
                                          + multiScanData[0].Info.SignalGenInfo.PeakSignalCount;

                            //reread all the samples, save...
                            //ScanData.Rewind();
                            string str = "Signals: ";
                            foreach (var (sInfo, sOf) in multiScanData[0].Info.SignalInfos)
                            {
                                str += "; " + sInfo.SignalID;
                            }

                            fs.WriteLine(str);
                            foreach (var scanData in multiScanData)
                            {
                                scanData.Rewind();
                                foreach (var s in scanData.Samples())
                                {
                                    StringBuilder sb = new StringBuilder();
                                    for (int j = 0; j < nSigCount; j++)
                                    {
                                        //if (j < scanData.Info.SignalGenInfo.GlobalSignalCount)
                                        //    sb.Append(s.Get(j) + ", ");
                                        //else
                                        //{
                                        //    for (int k = 0; k < scanData.Info.SignalGenInfo.ChannelCount; k++)
                                        //        sb.Append(s.Get(j, k) + ", ");
                                        //}
                                        if (j == sigX)
                                        {
                                            sb.Append(Math.Round(_xOffset  - (s.Get(sigX) / -200), 4) + ",");
                                        }
                                        else if (j == sigY)
                                        {
                                            sb.Append(Math.Round(_yOffset  - (s.Get(sigY) / 200), 4) + ",");
                                        }
                                        else
                                        {
                                            sb.Append(Math.Round(s.Get(j), 4) + ",");
                                        }
                                    }
                                    fs.WriteLine(sb);
                                }

                            }
                        }
                    }
                    return filePath;
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            });
        }
        public bool SetTriggerEncoderPos(string EncoderPos, string axis)
        {
            bool state = false;
            try
            {
                //Set encoder current position
                int nAxis = 0;
                if (axis == "x")
                {
                    nAxis = 0;
                }
                if (axis == "y")
                {
                    nAxis = 1;
                }

                int nPos = int.Parse(EncoderPos);
                var oRsp = Conn.Exec(CmdID.EncoderCounter, nAxis, nPos);
                logger.Error($"OneD - Set encoder value: {oRsp}");
                Logging(new LogInfor(LogType.PricitecController, $"OneD - Set encoder value: {oRsp}"));
                state = true;
            }
            catch (Exception ex)
            {
                logger.Error($"OneD - Cannot set encoder pos - {ex}");
                Logging(new LogInfor(LogType.PricitecController, $"OneD - Cannot set encoder pos."));
            }
            return state;
        }
        public void SendCommand(string _cmd)
        {
            try
            {
                var oRsp = Conn.ExecString(_cmd);
                logger.Info($"OneD - Respone Command: {oRsp}");
                LoggingRsp(new LogRspPrecitec(LogType.PricitecController, oRsp.ToString()));
                Logging(new LogInfor(LogType.PricitecController, oRsp.ToString()));
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
        public void Logging(LogInfor _logData)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                AppService.LogInfors.Add(_logData);
                AppService.ScrollViewerVerticalOffset = _appService.LogInfors.Count - 1;
            });
        }
        public void LoggingRsp(LogRspPrecitec _logData)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                AppService.LogRspPrecitec.Add(_logData);
                AppService.ScrollViewerVerticalOffset = _appService.LogRspPrecitec.Count - 1;
            });
        }
        public bool SoftwareTriggerAndRecording(int parameter)
        {
            bool output = false;
            try
            {
                if (parameter == 100)
                {
                    multiScanData = new List<Data>();
                }
                else
                {
                    // Set Trigger each
                    SendCommand("TRE");
                    // Set average data 
                    SendCommand("AVD 10");
                    ScanData = null;
                    // Send trigger
                    SendCommand("STR");
                    // Record 1 sample
                    Conn.StartRecording(1);
                    Thread.Sleep(100);
                    ScanData = Conn.GetNextSamples();
                    Conn.StopRecording();
                    // Append data to list after stop recording
                    multiScanData.Add(ScanData);
                    // Set back to free run mode
                    Conn.Exec(CHRocodileLib.CmdID.DeviceTriggerMode, CHRocodileLib.TriggerMode.FreeRun);
                    // Reset data
                    ScanData = null;
                    Logging(new LogInfor(LogType.Sequence, $"OneDMultiPoint - Moved and trigger to record data at Point T {parameter}"));
                    logger.Info($"OneDMultiPoint - Moved and trigger to record data at Point T {parameter}");
                    output = true;
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return output;
        }
        public void EnaTimerDownloadSpectrum(bool state)
        {
            if (state)
            {
                dispatcherTimer.Start();
            }
            else
            {
                if (dispatcherTimer != null)
                {
                    dispatcherTimer.Stop();
                }
                
            }
        }
    }
}
