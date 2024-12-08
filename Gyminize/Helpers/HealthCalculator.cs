using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyminize.Helpers
{
    /// <summary>
    /// Lớp này cung cấp các phương thức tính toán liên quan đến sức khỏe như chỉ số BMI, BMR, LBM, TDEE, và tính toán dinh dưỡng.
    /// </summary>
    public class HealthCalculator
    {
        /// <summary>
        /// Tính toán chỉ số BMI (Body Mass Index) dựa trên cân nặng và chiều cao.
        /// </summary>
        /// <param name="weight">Cân nặng (kg)</param>
        /// <param name="height">Chiều cao (cm)</param>
        /// <returns>Trả về giá trị BMI</returns>
        public static double CalculateBMI(double weight, double height)
        {
            return weight / ((height / 100) * (height / 100)); // Công thức BMI
        }

        /// <summary>
        /// Tính toán BMR (Basal Metabolic Rate) dựa trên cân nặng và tỷ lệ mỡ cơ thể.
        /// </summary>
        /// <param name="weight">Cân nặng (kg)</param>
        /// <param name="bodyfat">Tỷ lệ mỡ cơ thể (%)</param>
        /// <returns>Trả về giá trị BMR</returns>
        public static double CalculateBMR(double weight, double bodyfat)
        {
            return 370 + (21.6 * CalculateLBM(weight, bodyfat)); // Công thức BMR
        }

        /// <summary>
        /// Tính toán LBM (Lean Body Mass) dựa trên cân nặng và tỷ lệ mỡ cơ thể.
        /// </summary>
        /// <param name="weight">Cân nặng (kg)</param>
        /// <param name="bodyfat">Tỷ lệ mỡ cơ thể (%)</param>
        /// <returns>Trả về giá trị LBM</returns>
        public static double CalculateLBM(double weight, double bodyfat)
        {
            return (weight * (100 - bodyfat)) / 100; // Công thức LBM
        }

        /// <summary>
        /// Trả về mức độ hoạt động dựa trên cấp độ hoạt động của người dùng.
        /// </summary>
        /// <param name="level">Mức độ hoạt động (1 - 4)</param>
        /// <returns>Hệ số hoạt động tương ứng với mức độ</returns>
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

        /// <summary>
        /// Tính toán TDEE (Total Daily Energy Expenditure) dựa trên BMR, mức độ hoạt động và tỷ lệ mỡ cơ thể.
        /// </summary>
        /// <param name="weight">Cân nặng (kg)</param>
        /// <param name="bodyfat">Tỷ lệ mỡ cơ thể (%)</param>
        /// <param name="activityLevel">Mức độ hoạt động (1 - 4)</param>
        /// <returns>Trả về giá trị TDEE</returns>
        public static double CalculateTDEE(double weight, double bodyfat, int activityLevel)
        {
            return CalculateBMR(weight, bodyfat) * GetActivityLevel(activityLevel); // Công thức TDEE
        }

        /// <summary>
        /// Tính toán lượng dinh dưỡng cần thiết (calo từ chất béo, đạm hoặc carbohydrate) dựa trên tỷ lệ phần trăm và tổng số calo.
        /// </summary>
        /// <param name="nutritionstype">Loại dinh dưỡng ("fats", "proteins", "carbs")</param>
        /// <param name="plantype">Loại kế hoạch ("c" cho cắt giảm, "b" cho bổ sung)</param>
        /// <param name="percentage">Tỷ lệ phần trăm dinh dưỡng</param>
        /// <param name="totalcalories">Tổng số calo</param>
        /// <returns>Trả về lượng gram của chất dinh dưỡng cần thiết</returns>
        public static double CalculateNutritionGram(string nutritionstype, string plantype, double percentage, double totalcalories)
        {
            if (plantype == "c")
            {
                totalcalories -= 500; // Cắt giảm 500 calo nếu là kế hoạch giảm cân
            }
            else if (plantype == "b")
            {
                totalcalories += 500; // Thêm 500 calo nếu là kế hoạch bổ sung
            }
            if (nutritionstype == "fats") // calo mỗi gram = 9
            {
                return (totalcalories / 9) * (percentage / 100);
            }
            else // protein và carbohydrate, calo mỗi gram = 4
            {
                return (totalcalories / 4) * (percentage / 100);
            }
        }

        /// <summary>
        /// Đánh giá tình trạng sức khỏe của một người dựa trên chỉ số BMI.
        /// </summary>
        /// <param name="bmi">Giá trị BMI của người dùng</param>
        /// <returns>Trả về tình trạng sức khỏe (Thiếu cân, Cân đối, Thừa cân, Béo phì)</returns>
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
}
