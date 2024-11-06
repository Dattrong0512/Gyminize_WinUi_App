using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gyminize.Models;

namespace Gyminize.ViewModels;
public class FoodViewModel
{
    public async Task<List<Food>> getAllFood()
    {
        // Giả sử bạn load dữ liệu từ một API hoặc một cơ sở dữ liệu
        await Task.Delay(1000); // Mô phỏng tải dữ liệu
        return new List<Food>
        {

        };
    }
}
