using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyminize.Models;
public class FoodItem
{
    // Tên thực phẩm.
    public string Name
    {
        get; set;
    }

    // Lượng calo.
    public int Calories
    {
        get; set;
    }

    // Số lượng (kèm đơn vị).
    public string Amount
    {
        get; set;
    }
}