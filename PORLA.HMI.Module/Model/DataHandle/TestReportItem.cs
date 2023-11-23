using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PORLA.HMI.Module.Model.DataHandle
{
    public class TestReportItem
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
        public TestReportItem(string startTest, string endTest,
            string recipeId, string dutId, string recipeSensorType, string recipeX0, string recipeY0, 
            string recipeDx, string recipeDy, string recipeSpeedX, string recipeSpeedY, DateTime dateTime)
        {
            RecipeID = recipeId;
            DutID = dutId;
            RecipeSensorType = recipeSensorType;
            RecipeX0 = recipeX0;
            RecipeY0 = recipeY0;
            RecipeDX = recipeDx;
            RecipeDY = recipeDy;
            RecipeSpeedXAxis = recipeSpeedX;
            RecipeSpeedYAxis = recipeSpeedY;
        }
    }
}
