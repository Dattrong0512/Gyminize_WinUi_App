using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gyminize.Core.Models;
namespace Gyminize.Models;
public class SampleFoodDataService
{
    public ObservableCollection<FoodItem> GetSampleFoodData()
    {
        return new ObservableCollection<FoodItem>
            {
                new FoodItem { Name = "White Rice", Amount = "1 cup", Calories = 204 },
                new FoodItem { Name = "Chicken Breast", Amount = "100g", Calories = 136 },
                new FoodItem { Name = "Salad", Amount = "1 bowl", Calories = 80 },
                new FoodItem { Name = "Egg", Amount = "1 large", Calories = 90 }
            };
    }
}
