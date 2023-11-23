using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace POLAR.ModelAggregator.TestReport
{
    public class TestReportItems
    {
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
        public string DutID { get; set; }
        public string RecipeID { get; set; }
        public string RecipeSensorType { get; set; }
        public string RecipeX0 { get; set; }

        public string RecipeY0 { get; set; }

        public string RecipeDX { get; set; }

        public string RecipeDY { get; set; }

        public string RecipeSpeedXAxis { get; set; }

        public string RecipeSpeedYAxis { get; set; }
        public string StartTest { get; set; }
        public string FinishTest { get; set; }
        public string RecipeDWD { get; set; }
        public string RecipeQTH { get; set; }

        //public TestReportItems(string startTest,string finishTest,
        //    string recipeId, string dutId, string recipeSensorType, string recipeX0, string recipeY0,
        //    string recipeDx, string recipeDy, string recipeDWD, string recipeQTH)
        //{
        //    StartTest = startTest;
        //    FinishTest = finishTest;
        //    RecipeID = recipeId;
        //    DutID = dutId;
        //    RecipeSensorType = recipeSensorType;
        //    RecipeX0 = recipeX0;
        //    RecipeY0 = recipeY0;
        //    RecipeDX = recipeDx;
        //    RecipeDY = recipeDy;
        //    RecipeDWD = recipeDWD;
        //    RecipeQTH = recipeQTH;
        //}
        public void ClearAllData()
        {
            StartTest = "";
            FinishTest = "";
            RecipeID = "";
            DutID = "";
            RecipeSensorType = "";
            RecipeX0 = "";
            RecipeY0 = "";
            RecipeDX = "";
            RecipeDY = "";
            RecipeDWD = "";
            RecipeQTH = "";
        }
    }
}
