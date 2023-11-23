using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PORLA.HMI.Service.Configuration
{
    public class ConfigValue
    {
        public class AppSetting
        {
            public const string Language = "POLAR.AppSettings.Language";
            public const string OptionsLanguagePath = "POLAR.AppSettings.OptionsLanguagePath";
            public const string FTPServer = "POLAR.AppSettings.FTPServer";
            public const string ConfigPath = "./Config/config.xml";
            public const string LogPath = "POLAR.AppSettings.LogPath";
            public const string AutoLogoffInterval = "POLAR.AppSettings.AutoLogoffInterval";
            public const string RecipeConfigPath = "./Config/recipeConfig.xml";
            
        }

        public class RecipeSetting
        {
            public const string SelectNode = "/RECIPE/RECIPECONFIG";
            public const string Header = "RECIPECONFIG";
            public const string RecipeName = "RecipeName";
            public const string SensorType = "SensorType";
            public const string ThicknessSelection = "Thickness";
            public const string XOriginPosition = "XOriginalPosition";
            public const string YOriginPosition = "YOriginalPosition";
            public const string DXPosition = "DXDistance";
            public const string DYPosition = "DYDistance";
            public const string RXPosition = "RXDistance";
            public const string RYPosition = "RYDistance";
            public const string SpeedAxisX = "SpeedAxisX";
            public const string SpeedAxisY = "SpeedAxisY";
            public const string DWDLeftSide = "DWDLeftSide";
            public const string DWDRightSide = "DWDRightSide";
            public const string QualityThreshold = "QualityThreshold";
            public const string T1X = "T1X";
            public const string T1Y = "T1Y";
            public const string T2X = "T2X";
            public const string T2Y = "T2Y";
            public const string T3X = "T3X";
            public const string T3Y = "T3Y";
            public const string T4X = "T4X";
            public const string T4Y = "T4Y";
            public const string T5X = "T5X";
            public const string T5Y = "T5Y";
            public const string T6X = "T6X";
            public const string T6Y = "T6Y";
            public const string T7X = "T7X";
            public const string T7Y = "T7Y";
            public const string T8X = "T8X";
            public const string T8Y = "T8Y";
            public const string T9X = "T9X";
            public const string T9Y = "T9Y";
            public const string T10X = "T10X";
            public const string T10Y = "T10Y";
            public const string T11X = "T11X";
            public const string T11Y = "T11Y";
            public const string T12X = "T12X";
            public const string T12Y = "T12Y";
            public const string T13X = "T13X";
            public const string T13Y = "T13Y";
            public const string T14X = "T14X";
            public const string T14Y = "T14Y";
            public const string T15X = "T15X";
            public const string T15Y = "T15Y";
            public const string T16X = "T16X";
            public const string T16Y = "T16Y";
        }

        public class ReportSetting
        {
            public const string ReportPath = "POLAR.Report.ReportPath";
            public const string DetailPath = "POLAR.Report.DetailPath";
            public const string Delimiter = "POLAR.Report.Delimiter";
            public const string ReportExtention = "POLAR.Report.Extention";
            public const string FullFailReport = "POLAR.Report.FullFailReport";
        }

        public class IAIConfig
        {
            public const string IPAddress = "POLAR.IAIConfig.IPAddress";
            public const string PortAuto = "POLAR.IAIConfig.PortAuto";
            public const string PortManual = "POLAR.IAIConfig.PortManual";
            public const string Station = "POLAR.IAIConfig.Station";
            public const string XFixtureOffsetFSS = "POLAR.IAIConfig.XFixtureOffsetFSS";
            public const string YFixtureOffsetFSS = "POLAR.IAIConfig.YFixtureOffsetFSS";
            public const string XFixtureOffset1D = "POLAR.IAIConfig.XFixtureOffset1D";
            public const string YFixtureOffset1D = "POLAR.IAIConfig.YFixtureOffset1D";
        }

        public class ADAMConfig
        {
            public const string Module1 = "POLAR.ADAMConfig.Module1";
            public const string Module2 = "POLAR.ADAMConfig.Module2";
            public const string Module1IPAddress = "POLAR.ADAMConfig.Module1IPAddress";
            public const string Module2IPAddress = "POLAR.ADAMConfig.Module2IPAddress";
            public const string Module1Port = "POLAR.ADAMConfig.Module1Port";
            public const string Module2Port = "POLAR.ADAMConfig.Module2Port";
        }

        public class DBConfigKey
        {
            public const string CreateDB = "POLAR.DBConfig.CreateDB";
            public const string Username = "POLAR.DBConfig.Username";
            public const string Password = "POLAR.DBConfig.Password";
            public const string ServerName = "POLAR.DBConfig.ServerName";
            public const string DatabaseName = "POLAR.DBConfig.DatabaseName";
        }
    }
}
