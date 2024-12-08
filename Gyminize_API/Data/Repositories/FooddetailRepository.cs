using Gyminize_API.Data.Model;
using Gyminize_API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Gyminize_API.Data.Repositories;

/// <summary>
/// Lớp Repository để quản lý các thao tác dữ liệu chi tiết thực phẩm trong nhật ký hàng ngày của khách hàng.
/// </summary>
public class FooddetailRepository
{
    private readonly EntityDatabaseContext _context;

    /// <summary>
    /// Khởi tạo một instance mới của lớp <see cref="FooddetailRepository"/> với context cơ sở dữ liệu.
    /// </summary>
    /// <param name="context">Context cơ sở dữ liệu để tương tác với dữ liệu chi tiết thực phẩm.</param>
    public FooddetailRepository(EntityDatabaseContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Thêm mới hoặc cập nhật chi tiết thực phẩm vào nhật ký hàng ngày của khách hàng.
    /// </summary>
    /// <param name="dailydiaryId">ID của nhật ký hàng ngày.</param>
    /// <param name="mealType">Loại bữa ăn (sáng, trưa, tối,...).</param>
    /// <param name="food">Thông tin thực phẩm cần thêm hoặc cập nhật.</param>
    /// <param name="food_amount">Số lượng thực phẩm.</param>
    /// <returns>Chi tiết thực phẩm sau khi được thêm mới hoặc cập nhật.</returns>
    public Fooddetail AddOrUpdateFooddetail(int dailydiaryId, int mealType, Food food, int food_amount)
    {
        try
        {
            // Tìm kiếm đối tượng `Food` trong `DbContext`
            var existingFood = _context.FoodEntity.FirstOrDefault(f => f.food_id == food.food_id);
            if (existingFood != null)
            {
                food = existingFood; // Sử dụng `Food` đã tồn tại trong `DbContext`
            }
            else
            {
                _context.FoodEntity.Attach(food); // Đính kèm `Food` nếu chưa được theo dõi
            }

            // Tìm `Fooddetail` dựa trên `dailydiary_id`, `meal_type`, và `food_id`
            var existingFooddetail = _context.FooddetailEntity
                .Include(fd => fd.Food)
                .FirstOrDefault(fd => fd.dailydiary_id == dailydiaryId && fd.meal_type == mealType && fd.food_id == food.food_id);

            if (existingFooddetail != null)
            {
                // Nếu `Fooddetail` đã tồn tại với cùng `meal_type` và `food`, cộng thêm `food_amount`
                existingFooddetail.food_amount += food_amount;
            }
            else
            {
                // Kiểm tra nếu đã có `Fooddetail` với `meal_type` nhưng `food_id` khác
                var sameMealTypeFooddetail = _context.FooddetailEntity
                    .FirstOrDefault(fd => fd.dailydiary_id == dailydiaryId && fd.meal_type == mealType);

                // Nếu `meal_type` đã tồn tại với `food_id` khác, tạo mới `Fooddetail`
                var newFooddetail = new Fooddetail
                {
                    dailydiary_id = dailydiaryId,
                    meal_type = mealType,
                    food_amount = food_amount,
                    Food = food
                };

                _context.FooddetailEntity.Add(newFooddetail);
                existingFooddetail = newFooddetail;
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.SaveChanges();
            return existingFooddetail;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi trong AddOrUpdateFooddetail: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Xóa thực phẩm khỏi chi tiết thực phẩm trong nhật ký hàng ngày.
    /// </summary>
    /// <param name="dailydiaryId">ID của nhật ký hàng ngày.</param>
    /// <param name="mealType">Loại bữa ăn (sáng, trưa, tối,...).</param>
    /// <param name="food">Thông tin thực phẩm cần xóa.</param>
    /// <returns>Trả về true nếu thực phẩm đã được xóa thành công, ngược lại trả về false.</returns>
    public bool DeleteFoodFromFooddetail(int dailydiaryId, int mealType, Food food)
    {
        try
        {
            // Tìm tất cả các bản ghi `Fooddetail` dựa trên `dailydiary_id` và `meal_type`
            var existingFooddetails = _context.FooddetailEntity
                .Include(fd => fd.Food)
                .Where(fd => fd.dailydiary_id == dailydiaryId && fd.meal_type == mealType)
                .ToList();

            if (existingFooddetails != null && existingFooddetails.Any())
            {
                bool isDeleted = false;
                foreach (var foodDetail in existingFooddetails)
                {
                    // Kiểm tra xem `Food` có khớp với `Fooddetail` không
                    if (foodDetail.Food != null && foodDetail.Food.food_id == food.food_id)
                    {
                        // Xóa `Food` khỏi `Fooddetail` nếu khớp
                        foodDetail.Food = null;
                        foodDetail.food_amount = 0; // Đặt `food_amount` về 0 hoặc giá trị mặc định
                        isDeleted = true;
                    }
                }

                if (isDeleted)
                {
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    Console.WriteLine("Food hoặc số lượng không khớp với bất kỳ `Fooddetail` nào.");
                    return false;
                }
            }

            Console.WriteLine("Không tìm thấy `Fooddetail` hoặc `Food` khớp.");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi trong DeleteFoodFromFooddetail: {ex.Message}");
            return false;
        }
    }
}
