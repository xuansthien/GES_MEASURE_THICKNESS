using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHRocodileLib;
using FSSCommon;
using System.Data.Common;
using System.Net;
using System.Windows;
using System.IO;
using POLAR.CompositeAppCommand;
using PORLA.HMI.Service;
using System.Drawing;
using System.Web;
using System.Windows.Media.Imaging;
using PORLA.HMI.Service.Enums;
using Prism.Events;
using POLAR.EventAggregator;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Threading;
using System.Threading;
using Prism.Services.Dialogs;
using System.Windows.Shapes;
using System.Runtime.InteropServices.ComTypes;

namespace POLAR.PrecitecControl
{
    public partial class FSSAreaScan : BindableBase, IFSSAreaScan
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
        private readonly IDialogService _dialogService;

        /// <summary>
        /// File name of the scanner global configuration.
        /// </summary>
        public const string CONFIG_FILE_NAME = "ScannerGlobalConfig.cfg";
        /// <summary>
        /// Gets the scanner object.
        /// </summary>
        /// 
        public FlyingSpotScanner Scanner;
        SynchronousConnection ConnSync;

        private CHRLibPlugin.FSS_PluginShape _shape;
        private DataProcessor _dataProc;
        public Mutex mutex = new Mutex(false);

        private Int32 _plotSignal = 82;
        private int Data_Length = 1000;
        private bool IsSpectrumDownload = false;
        private enum State : Int32
        {
            Disconnected,
            Connected,
            ScanRunning,
        }

        public System.Timers.Timer _timerFssSpectrum;
        private DispatcherTimer dispatcherTimer;

        public FSSAreaScan(IAppService appService, ICompositeAppCommand compositeAppCommand,
            IEventAggregator eventAggregator, IDialogService dialogService)
        {
            _eventAggregator = eventAggregator;
            _appCommand = compositeAppCommand;
            _appService = appService;

            InitializeCommand();
            _dialogService = dialogService;
        }
        public void InitializeFssAsync()
        {
            _dataProc = new DataProcessor();
            // Create a scanner instance and register to events
            Scanner = new FlyingSpotScanner();
            
            string IpAddress = "192.168.170.2";
            bool rawDataMode = false;
            ConnectAsync(IpAddress, rawDataMode);
        }

        private async Task ConnectAsync(string ip, bool dataMode)
        {
            if (!Scanner.IsConnected)
            {
                Scanner.Open(ip, dataMode);
                Scanner.GeneralCommandCallback = OnGeneralCommandResponse;
                Scanner.ScanProgramCallback = OnScanProgramCallback;
                AppService.PrecitecService.Connected = true;
                dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Interval = TimeSpan.FromMilliseconds(50);
                dispatcherTimer.Tick += DispatcherTimer_Tick;
                if (Scanner.IsConnected)
                {
                    await Scanner.Config(CONFIG_FILE_NAME, 11000000); // this is a waiting commandk
                }
                //State.Connected;
                //ConnSync = new SynchronousConnection(Scanner.Conn);
                SendConfig();
            }
            else
            {
                Scanner.Close();
                AppService.PrecitecService.Connected = false;
                AppService.MachineStatus.MachineState = MachineStateEnum.ERROR;
                Logging(new LogInfor(LogType.PricitecController, $"Precitec controller can not establish connection"));
            }
        }

        private void SendConfig()
        {
            Scanner.Conn.ExecString("FLTI -64 128 -64", null);
            Scanner.Conn.ExecString("FLTC -64 128 -64", null);
        }

        private async void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            //Synchronously download spectrum.
            if (AppService.PrecitecService.FssSelected)
            {
                try
                {
                    await Task.Run(() => 
                    {
                        
                        var specType = SpecType.FT;
                        Response oRsp;
                        //here only download the specturm of the first channel
                        oRsp = ConnSync.Exec(CmdID.DownloadSpectrum, specType);
                        //the last parameter of the response is the spectrum data
                        var aBytes = oRsp.GetParam<byte[]>(oRsp.ParamCount - 1);
                        //convert to 16bit data
                        Int16[] SpecData = new Int16[aBytes.Length / 2];
                        Buffer.BlockCopy(aBytes, 0, SpecData, 0, aBytes.Length);
                        int len = Math.Min(Data_Length, SpecData.Length);

                        for(short i = 0; i < len; i++)
                        {
                            AppService.PrecitecService.DataPoints[i] = SpecData[i];
                        }
                        //for (int i = 0; i < SpecData.Length; i++)
                        //    AppService.PrecitecService.DataPoints.Add(SpecData[i]);
                    });
                    //_eventAggregator.GetEvent<SpectrumDowloadEvent>().Publish(true);
                }
                catch
                {
                    Debug.Fail("Cannot set download spectrum");
                }
            }
        } 

        private void _timerFssSpectrum_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //Synchronously download spectrum.
            if (AppService.PrecitecService.FssSelected)
            {
                try
                {
                    var specType = SpecType.Raw;
                    if (AppService.PrecitecService.SpecTrumFt)
                    {
                        specType = SpecType.FT;
                    }
                    Response oRsp;
                    //here only download the specturm of the first channel
                    oRsp = ConnSync.Exec(CmdID.DownloadSpectrum, specType);
                    //the last parameter of the response is the spectrum data
                    var aBytes = oRsp.GetParam<byte[]>(oRsp.ParamCount - 1);
                    //convert to 16bit data
                    Int16[] SpecData = new Int16[aBytes.Length / 2];
                    Buffer.BlockCopy(aBytes, 0, SpecData, 0, aBytes.Length);
                    //int len = Math.Min(Data_Length, SpecData.Length);
                }
                catch
                {
                    Debug.Fail("Cannot set download spectrum");
                }
            }
        }

        public void Disconnect()
        {
            try
            {
                Scanner.Close();
                //ConnSync.Close();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            
        }

        private async void OnScanProgramCallback(Response rsp)
        {
            try
            {
                //Console.WriteLine($"Scan program callback called {rsp}");
                // Shape data are stored in a blob argument of the response
                if (!(rsp.ParamCount > 0 && rsp.TryGetParam(0, out byte[] blob)))
                {
                    return;
                }

                var shape = new CHRLibPlugin.FSS_PluginShape(blob);
                if (shape.Type == CHRLibPlugin.FSS_PluginDataType.RecipeTerminate)
                {
                    //Console.WriteLine("ScanCallback: scan finished..");
                    Action updateUIAction = () =>
                    {
                        StopProgram();
                    };
                    Application.Current.Dispatcher.BeginInvoke(updateUIAction);
                    return;
                }
                //shape.Detach(); // "detaches" the shape from the FSS internal buffer by copying
                                // all signal data to the local storage: this is not absolutely necessary
                                // but is a safe way if the data is not supposed to be processed immediately
                _shape = shape; // otherwise, save this shape for future use
                _dataProc.Shape = _shape;    // point data manipulator to the new shape
                if (shape.Type == CHRLibPlugin.FSS_PluginDataType.RawData)
                {
                    string _fixname = $"{AppService.DutBarcode}_{AppService.RecipeService.RYPosition}x{AppService.RecipeService.RXPosition}_{AppService.RecipeService.DYPosition}x{AppService.RecipeService.DXPosition}_{AppService.RecipeService.RecipeName}";
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

                    var fname1Task = SaveAsCSV_ThicknessAsync(shape, 1, pathName1);
                    var fname2Task = SaveAsCSV_ThicknessAsync(shape, 2, pathName2);
                    var fname3Task = SaveAsCSV_ThicknessAsync(shape, 3, pathName3);
                    var fname4Task = SaveAsCSV_ThicknessAsync(shape, 4, pathName4);
                    var fname5Task = SaveAsCSV_ThicknessAsync(shape, 5, pathName5);
                    var fname6Task = SaveAsCSV_ThicknessAsync(shape, 6, pathName6);
                    //==========For testing 
                    var fname7Task = SaveAsCSV_ThicknessAsync(shape, 7, pathName7);
                    var fname8Task = SaveAsCSV_ThicknessAsync(shape, 8, pathName8);
                    //==========
                    var fnameAllTask = SaveAsCSV_ThicknessAsync(shape, 100, pathNameAll); // 100: get all thickness into one file


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
                else
                {
                    for (int i = 0; i < _shape.SignalInfos.Length; i++)
                    {
                        string fname = _dataProc.SaveAsBCRF(i);
                        Logging(new LogInfor(LogType.PricitecController, $"Signal #{i} saved to {fname}"));
                    }
                }
            }
            catch (Exception ex)
            {
                Action updateUIActionError = () =>
                {
                    Logging(new LogInfor(LogType.PricitecController, $"{ex.Message}"));
                };
                Application.Current.Dispatcher.BeginInvoke(updateUIActionError);
            }

        }

        private void UpdateSignals()
        {
            //CmbPlotSignal.Items.Clear();
            AppService.PrecitecService.PlotSignal.Clear();
            if (_shape?.SignalInfos != null)
            {
                foreach (var sig in _shape.SignalInfos)
                {
                    string temp = ((int)sig.SignalID).ToString();
                    //AppService.PrecitecService.PlotSignal.Add(((int)sig.SignalID).ToString());
                    _eventAggregator.GetEvent<UpdateSignalIdEvent>().Publish(temp);
                }
                //CmbPlotSignal.Items.Add((int)sig.SignalID);

                //CmbPlotSignal.SelectedIndex = 0;
            }
            //CmbPlotSignal.Enabled = CmbPlotSignal.Items.Count > 0;
        }

        private void OnGeneralCommandResponse(Response rsp)
        {
            Action updateUIActionCmd = () =>
            {
                Logging(new LogInfor(LogType.PricitecController, $"{rsp.ToString()}"));
                LoggingRsp(new LogRspPrecitec(LogType.PricitecController, $"{rsp.ToString()}"));
            };
            Application.Current.Dispatcher.BeginInvoke(updateUIActionCmd);

        }
        
        public async Task<bool> CompileScriptAsync()
        {
            bool output = false;
            try
            {
                string startupPath = System.AppDomain.CurrentDomain.BaseDirectory + @"Scripts\RectScan_Script.rs";

                var programCode = File.ReadAllText(startupPath);
                var progHandle = await Scanner.Compile(programCode);
                Scanner.Run(progHandle);
                output = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return output;
        }
        
        public async Task<ScanStatus> RunScriptAutoAsync()
        {
            ScanStatus _scanStatus = ScanStatus.NONE;
            _shape = null;
            AppService.PrecitecService.ScanDone = false;
            string startupPath = System.AppDomain.CurrentDomain.BaseDirectory + @"Scripts\RectScan_Script.rs";

            var programCode = File.ReadAllText(startupPath);
            //var programCodeUpdate = UpdateScanScript(programCode);
            var progHandleTask = await Scanner.Compile(programCode);

            Scanner.Run(progHandleTask);
            
            return await Task.Run(() =>
            {
                try
                {
                    while (!AppService.PrecitecService.ScanDone)
                    {
                        if (AppService.PrecitecService.ScanDone & !Scanner.IsScanning)
                        {
                            _scanStatus = ScanStatus.FINISH;
                            break;
                        }
                        if (!AppService.DIOModule2.IOMapping[DIODescriptions.In_SafetyCircuitSignal])
                        {
                            _scanStatus = ScanStatus.EXCEPTION;
                            Logging(new LogInfor(LogType.Sequence, "Break out of Scan because safety is violated"));
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
                return _scanStatus;
            });
            
        }
        
        public string GetScriptContent()
        {
            string startupPath = System.AppDomain.CurrentDomain.BaseDirectory + @"Scripts\RectScan_Script.rs";
            var programContent = File.ReadAllText(startupPath);
            return programContent;
        }

        public void WriteUpdatedScriptToFile(string programCode)
        {
            string startupPath = System.AppDomain.CurrentDomain.BaseDirectory + @"Scripts\RectScan_Script.rs";
            File.WriteAllText(startupPath, programCode);
        }

        public string UpdateScanScript(string typeUpdate, string scanPrg, params double[] args)
        {
            string result = "";
            string pattern = "";
            string replacement = "";
            try
            {
                if (typeUpdate == "area")
                {
                    double x1 = args[0] / 2;
                    double y1 = args[1] / 2;
                    double x0 = -x1;
                    double y0 = -y1;
                    //Scan Area is defined by the top left corner (x0, y0) and the bottom right corner of the coordinate system 
                    // scan 20x20 => x0 = -10, y0 = -10, x1 = 10, y1 = 10
                    //pattern = @"x0=-\d+\.\d+,y0=-\d+\.\d+,x1=\d+\.\d+,y1=\d+\.\d+";
                    pattern = @"x0=-\d+(\.\d+)?,y0=-\d+(\.\d+)?,x1=\d+(\.\d+)?,y1=\d+(\.\d+)?";
                    replacement = $"x0={x0},y0={y0},x1={x1},y1={y1}";
                }
                if (typeUpdate == "resolution")
                {
                    pattern = @"nCols=\d+,nRows=\d+";
                    replacement = $"nCols={args[0]},nRows={args[1]}";
                }

                result = Regex.Replace(scanPrg, pattern, replacement);
                logger.Info(result);
                Logging(new LogInfor(LogType.General, result));
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            
            return result;  
        }

        public async Task RunProgram()
        {
            try
            {
                _shape = null;
                AppService.PrecitecService.ScanDone = false;
                string startupPath = System.AppDomain.CurrentDomain.BaseDirectory + @"Scripts\RectScan_Script.rs";

                var programCode = File.ReadAllText(startupPath);

                var progHandle = await Scanner.Compile(programCode);
                Scanner.Run(progHandle);
                
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
        private async Task<ScanStatus> OnProcessScanning()
        {
            ScanStatus _scanStatus = ScanStatus.NONE;
            return await Task.Run(() =>
            {
                try
                {
                    while (!AppService.PrecitecService.ScanDone)
                    {
                        if (AppService.PrecitecService.ScanDone & !Scanner.IsScanning)
                        {
                            _scanStatus = ScanStatus.FINISH;
                            break;
                        }
                        if (!AppService.DIOModule2.IOMapping[DIODescriptions.In_SafetyCircuitSignal])
                        {
                            _scanStatus = ScanStatus.EXCEPTION;
                            Logging(new LogInfor(LogType.Sequence, "Break out of Scan because safety is violated"));
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
                return _scanStatus;
            });
        }
        public void StopProgram()
        {
            try
            {
                Task.Run(() => 
                {
                    Scanner.Stop();
                    UpdateSignals();
                });
            }
            catch (Exception ex )
            {
                logger.Error(ex);
            }
            
        }
        public void PlotSignalId(string signalId)
        {
            try
            {
                _plotSignal = int.Parse(signalId);
                Bitmap _bitMapConvert = null;
                if (_shape != null)
                {
                    // obtain the signal index from signal ID
                    var sigIdx = _shape.SignalIndex(_plotSignal);
                    if (_shape.Type == CHRLibPlugin.FSS_PluginDataType.Interpolated2D)
                    {
                        //ImgAreaScan.Image = _dataProc.GridDataToBitmapRGB(sigIdx);
                        _bitMapConvert = _dataProc.GridDataToBitmapRGB(sigIdx);
                        AppService.PrecitecService.HeatMap = ConvertToBitMapImage(_bitMapConvert);
                    }
                    else
                    {
                        int Xidx = _shape.SignalIndex(65),  // signal indices for X- and Y-encoder coordinates
                            Yidx = _shape.SignalIndex(66);
                        // plot raw 2D data onto RGB bitmap with a given size
                        //ImgAreaScan.Image = _dataProc.RawDataToBitmapRGB(sigIdx, Xidx, Yidx, 512, 512);
                        _bitMapConvert = _dataProc.RawDataToBitmapRGB(sigIdx, Xidx, Yidx, 512, 512);
                        AppService.PrecitecService.HeatMap = ConvertToBitMapImage(_bitMapConvert);
                    }
                }
            }
            catch (Exception ex) 
            {
                logger.Error(ex);
            }
            
        }

        private BitmapImage ConvertToBitMapImage(Bitmap bitMapConvert)
        {
            try
            {
                // Convert from bitmap to BitmapImage for wpf
                // Convert the System.Drawing.Bitmap to a MemoryStream
                MemoryStream memoryStream = new MemoryStream();
                bitMapConvert.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
                memoryStream.Seek(0, SeekOrigin.Begin);

                // Create a new BitmapImage and set its source to the MemoryStream
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return null;
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

        // Test Save csv with separate file for every thickness
        private string SaveAsCSV_Thickness(CHRLibPlugin.FSS_PluginShape shape, byte thicknesIndex, string filePath = "")
        {
            var NF = CultureInfo.GetCultureInfo("de-DE").NumberFormat;
            var fname = filePath.Length > 0 ? filePath : $"Fig_{shape.Label}_{shape.ShapeIndex}_Thickness{thicknesIndex}.csv";

            using (var fs = File.CreateText(fname))
            {
                int sigX = shape.SignalIndex(65),
                    sigY = shape.SignalIndex(66),
                    sigThickness = shape.SignalIndex(thicknesIndex * 8 + 248);

                foreach (var S in shape.Samples())
                {
                    string str = S.Get(sigX).ToString(NF) + ";"
                            + S.Get(sigY).ToString(NF) + ";"
                            + S.Get(sigThickness).ToString(NF);
                    fs.WriteLine(str);
                }
            }
            return fname;
        }
        private Task<string> SaveAsCSV_ThicknessAsync(CHRLibPlugin.FSS_PluginShape shape, byte thicknesIndex, string filePath = "")
        {
            return Task.Run(() =>
            {
                try
                {
                    mutex.WaitOne();
                    var NF = CultureInfo.GetCultureInfo("de-DE").NumberFormat;
                    var fname = filePath.Length > 0 ? filePath : $"Fig_{shape.Label}_{shape.ShapeIndex}_Thickness{thicknesIndex}.csv";
                    
                    // Convert to X, Y Fixture
                    double _xOffset = double.Parse(AppService.IAIMotion.XAxisSave);
                    double _yOffset = double.Parse(AppService.IAIMotion.YAxisSave);
                    double _xOrgRecipe = double.Parse(AppService.RecipeService.XOriginalPosition);
                    double _yOrgRecipe = double.Parse(AppService.RecipeService.YOriginalPosition);
                    if (thicknesIndex < 100)
                    {
                        using (var fs = File.CreateText(fname))
                        {
                            // get index signal
                            int sigX = shape.SignalIndex(65),
                                sigY = shape.SignalIndex(66),
                                sigThickness = shape.SignalIndex(thicknesIndex * 8 + 248);
                            //foreach (var S in shape.Samples())
                            //{
                            //    string str = S.Get(sigX).ToString(NF) + ";"
                            //            + S.Get(sigY).ToString(NF) + ";"
                            //            + S.Get(sigThickness).ToString(NF);
                            //    fs.WriteLine(str);
                            //}
                            // Round to 4 digits
                            //foreach (var S in shape.Samples())
                            //{
                            //    string str = Math.Round(S.Get(sigX), 4).ToString() + ";"
                            //               + Math.Round(S.Get(sigY), 4).ToString() + ";"
                            //               + Math.Round(S.Get(sigThickness), 4).ToString();
                            //    fs.WriteLine(str);
                            //}

                            foreach (var S in shape.Samples())
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.Append(Math.Round(_xOffset - _xOrgRecipe - S.Get(sigX), 4) + ",");
                                sb.Append(Math.Round(_yOffset - _yOrgRecipe - S.Get(sigY), 4) + ",");
                                sb.Append(Math.Round(S.Get(sigThickness), 4));
                                fs.WriteLine(sb);
                            }
                        }
                        
                    }
                    else
                    {
                        using (var fs = File.CreateText(fname))
                        {
                            // get index signal
                            int sigX = shape.SignalIndex(65),
                                sigY = shape.SignalIndex(66);
                            string str = "Signals: ";
                            foreach (var s in shape.SignalInfos)
                            {
                                str += "; " + s.SignalID;
                            }
                            fs.WriteLine(str);

                            //foreach (var S in shape.Samples())
                            //{
                            //    str = "";
                            //    for (int i = 0; i < shape.SignalInfos.Length; i++)
                            //    {
                            //        str += ";" + S.Get(i).ToString(NF);
                            //    }
                            //    fs.WriteLine(str);
                            //}
                            // Round to 4 digits
                            foreach (var S in shape.Samples())
                            {
                                //str = "";
                                //for (int i = 0; i < shape.SignalInfos.Length; i++)
                                //{
                                //    str += ";" + Math.Round(S.Get(i), 4).ToString(NF);
                                //}
                                //fs.WriteLine(str);

                                StringBuilder sb = new StringBuilder();
                                for (int i = 0; i < shape.SignalInfos.Length; i++)
                                {
                                    if (i == sigX)
                                    {
                                        sb.Append(Math.Round(_xOffset - _xOrgRecipe - S.Get(sigX), 4) + ",");
                                    }
                                    else if (i == sigY)
                                    {
                                        sb.Append(Math.Round(_yOffset - _yOrgRecipe - S.Get(sigY), 4) + ",");
                                    }
                                    else
                                    {
                                        sb.Append(Math.Round(S.Get(i), 4) + ",");
                                    }
                                }
                                fs.WriteLine(sb);
                            }
                        }
                    }
                    return fname;
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            });
        }
        private void SendAsyncCmd(string cmd)
        {
            //Asynchronous string command
            Scanner.Conn.ExecString(cmd, null);
        }
        public void EnaTimerDownloadSpectrum(bool state)
        {
            if (state)
            {
                dispatcherTimer.Start();
                IsSpectrumDownload = true;
            }
            else
            {
                dispatcherTimer.Stop();
                IsSpectrumDownload = false;
            }
        }

    }
}
