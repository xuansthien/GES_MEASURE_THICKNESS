using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POLAR.ModelAggregator.Recipes
{
    public class RecipeItems
    {
        public string RecipeName { get; set; }
        public RecipeItems(string recipeName)
        {

            RecipeName = recipeName;

        }
    }
}
