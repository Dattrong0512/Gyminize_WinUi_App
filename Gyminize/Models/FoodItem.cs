using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyminize.Models;
public class FoodItem
{
    public int food_id
    {
        get; set;
    }
    public string food_name
    {
        get; set;
    }

    // Lượng calo.
    public int calories
    {
        get; set;
    }

    public int amount
    {
        get; set;
    }

    
    public string serving_unit;
}
