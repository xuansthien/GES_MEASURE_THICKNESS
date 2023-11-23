using PORLA.HMI.Main.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using PORLA.HMI.Module.Views.Statistic;
using PORLA.HMI.Service;
using PORLA.HMI.Module.Views.SettingPages;
using PORLA.HMI.Module.Views.LogPagess;
using Prism.Mvvm;
using Prism.Commands;
using PORLA.HMI.Module.Views;
using PORLA.HMI.Module.ViewModels.SettingPages;
using log4net;
using PORLA.HMI.Service.Configuration;
using POLAR.DIOADAM6052;
using Prism.DryIoc;
using POLAR.IAIMotionControl;
using PORLA.HMI.Module;
using System;
using POLAR.CompositeAppCommand;
using Prism.Events;
using PORLA.HMI.Module.Model.AlarmHandle;
using PORLA.HMI.Module.Views.Dialogs;
using PORLA.HMI.Module.ViewModels.Dialogs;
using POLAR.PrecitecControl;
using PORLA.HMI.Main.Views.Dialog;
using PORLA.HMI.Main.ViewModels.Dialog;
using PORLA.HMI.Module.DataService.DataHandle;
using System.Windows.Threading;

namespace PORLA.HMI.Main
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAdam6052Module1, Adam6052Module1>();
            containerRegistry.RegisterSingleton<IAdam6052Module2, Adam6052Module2>();
            containerRegistry.RegisterSingleton<IIAIMotion, IAIMotion>();
            containerRegistry.RegisterSingleton<IConfigHandler, ConfigHandler>();
            containerRegistry.RegisterSingleton<ILoggerService, LoggerService>();
            containerRegistry.RegisterSingleton<IAppService, AppService>();
            containerRegistry.RegisterSingleton<ICompositeAppCommand, CompositeAppCommand>();
            containerRegistry.RegisterSingleton<IAlarmHandler, AlarmHandler>();
            containerRegistry.RegisterSingleton<IFSSAreaScan, FSSAreaScan>();
            containerRegistry.RegisterSingleton<IDataHandler, DataHandle>();
            containerRegistry.RegisterSingleton<IOneDAreaScan, OneDAreaScan>();

            containerRegistry.RegisterForNavigation<HomePage>("HomePage");
            containerRegistry.RegisterForNavigation<AccountPage>("AccountPage");
            containerRegistry.RegisterForNavigation<AlarmPage>("AlarmPage");
            containerRegistry.RegisterForNavigation<SettingPage>("SettingPage");
            containerRegistry.RegisterForNavigation<VersionPage>("VersionPage");
            containerRegistry.RegisterForNavigation<Login>("Login");
            containerRegistry.RegisterForNavigation<MachineLogPage>("MachineLogPage");
            containerRegistry.RegisterForNavigation<LogPanel>("LogPanel");
            containerRegistry.RegisterForNavigation<StatisticPanel>("StatisticPanel");
            containerRegistry.RegisterForNavigation<EmptyPanel>("EmptyPanel");
            containerRegistry.RegisterForNavigation<TestResult>("TestResult");
            containerRegistry.RegisterForNavigation<RecipePage>("RecipeID");
            containerRegistry.RegisterForNavigation<ManualPageView>("ManualPageView");
            containerRegistry.RegisterForNavigation<SettingPanel>("SettingPanel");          
            containerRegistry.RegisterForNavigation<ShowDialogView, ShowDialogViewModel>("ShowDialogView");

            containerRegistry.RegisterDialog<CreateNewRecipe, CreateNewRecipeViewModel>();
            containerRegistry.RegisterDialog<ScanDutBarcodeDialog, ScanDutBarcodeDialogViewModel>();
            containerRegistry.RegisterDialog<SelectPrecitecSensorDialog, SelectPrecitecSensorDialogViewModel>();
            containerRegistry.RegisterDialog<EditRecipe, EditRecipeViewModel>();
            containerRegistry.RegisterDialog<ProccessScanDialog, ProccessScanDialogViewModel>();

            logger.Info("Application Started!");
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<IAIMotionControlModule>();
            moduleCatalog.AddModule<DIOADAM6052Module>();
        }
        
    }
}
