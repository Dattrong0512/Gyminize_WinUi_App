using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyminize.Helpers;
public class HealthCalculator
{
    public static double CalculateBMI(double weight, double height)
    {
        return weight / ((height/100) * (height/100));
    }

    public static double CalculateBMR(double weight, double bodyfat)
    {
        return 370 + (21.6 * CalculateLBM(weight, bodyfat));
    }
    public static double CalculateLBM(double weight, double bodyfat)
    {
        return (weight * (100 - bodyfat))/100;
    }

    public static double GetActivityLevel(int level)
    {
        return level switch
        {
            1 => 1.2,    // Mức 1: Hầu như không vận động
            2 => 1.375,  // Mức 2: Thấp (1 - 3 buổi/tuần)
            3 => 1.55,   // Mức 3: Trung Bình (3 - 5 buổi/tuần)
            4 => 1.725,  // Mức 4: Cao (6 - 7 buổi/tuần)
            _ => 0.0     // Mức không xác định
        };
    }

    public static double CalculateTDEE(double weight, double bodyfat, int activityLevel)
    {
        return CalculateBMR(weight, bodyfat) * GetActivityLevel(activityLevel);
    }

    public static double CalculateNutritionGram(string nutritionstype, string plantype, double percentage, double totalcalories)
    {
        if(plantype == "c")
        {
            totalcalories -= 500;
        }
        else if (plantype == "b")
        {
            totalcalories += 500;
        }
        if (nutritionstype == "fats") //calories per gram = 9
        {
            return (totalcalories / 9) * (percentage / 100);
        }
        else // proteins and carbs calories per gram = 4
        {
            return (totalcalories / 4) * (percentage / 100);
        }
    }
    public static string HealthStatus(double bmi)
    {
        if (bmi < 18.5)
        {
            return "Thiếu cân";
        }
        else if (bmi >= 18.5 && bmi < 24.9)
        {
            return "Cân đối";
        }
        else if (bmi >= 25 && bmi < 29.9)
        {
            return "Thừa cân";
        }
        else
        {
            return "Béo phì";
        }
    }
}
