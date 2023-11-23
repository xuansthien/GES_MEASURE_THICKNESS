using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace PORLA.HMI.Module.Model
{
    public class DataSqlModel : BindableBase
    {
        // alarm
        private string iDAlarm;
        private int iD;
        private DateTime alarmTime;
        private DateTime alarmClearedTime;
        private string category;
        private string source;
        private string description;
        public int ID { get => iD; set => iD = value; }
        public string IDAlarm { get => iDAlarm; set => iDAlarm = value; }
        public DateTime AlarmTime { get => alarmTime; set => alarmTime = value; }
        public DateTime AlarmClearedTime { get => alarmClearedTime; set => alarmClearedTime = value; }
        public string Category { get => category; set => category = value; }
        public string Source { get => source; set => source = value; }
        public string Description { get => description; set => description = value; }

        // account
        private int iDUser;

        private string userName;

        private string userType;
        public int IDUser { get => iDUser; set => iDUser = value; }
        public string UserName { get => userName; set => userName = value; }
        public string UserType { get => userType; set => userType = value; }

        //TestResul
        private DateTime _timeDB;
        public DateTime TimeDB
        {
            get => _timeDB;
            set
            {     
                _timeDB = value.Date;
            }
        }

        private string _barcodeNumber;
        public string DutID { get => _barcodeNumber; set => _barcodeNumber = value; }

        private string _recipe;
        public string RecipeID { get => _recipe; set => _recipe = value; }
        private string _layer1;
        public string RecipeSensorType { get => _layer1; set => _layer1 = value; }
        private string _layer2;
        public string RecipeX0{ get => _layer2; set => _layer2 = value; }
        private string _layer3;
        public string RecipeY0{ get => _layer3; set => _layer3 = value; }
        private string _layer4;
        public string RecipeDX { get => _layer4; set => _layer4 = value; }
        private string _layer5;
        public string RecipeDY { get => _layer5; set => _layer5 = value; }
        private string _layer6;
        public string RecipeSpeedXAxis { get => _layer6; set => _layer6 = value; }
        private string _layer7;
        public string RecipeSpeedYAxis { get => _layer7; set => _layer7 = value; }
    }
}
