using Gyminize_API.Data.Model;

namespace Gyminize_API.Data.Repositories;

/// <summary>
/// Lớp Repository để quản lý các thao tác dữ liệu thực phẩm trong cơ sở dữ liệu.
/// </summary>
public class FoodRepository
{
    private readonly EntityDatabaseContext _context;

    /// <summary>
    /// Khởi tạo một instance mới của lớp <see cref="FoodRepository"/> với context cơ sở dữ liệu.
    /// </summary>
    /// <param name="context">Context cơ sở dữ liệu để tương tác với dữ liệu thực phẩm.</param>
    public FoodRepository(EntityDatabaseContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lấy tất cả các thực phẩm từ cơ sở dữ liệu.
    /// </summary>
    /// <returns>Danh sách tất cả các thực phẩm.</returns>
    public List<Food> GetAllFood()
    {
        try
        {
            // Trả về danh sách tất cả thực phẩm từ cơ sở dữ liệu
            return _context.FoodEntity
            .Select(f => new Food
            {
                food_id = f.food_id,
                food_name = f.food_name,
                calories = f.calories,
                serving_unit = f.serving_unit
            }).Take(100)
            .ToList();
        }
        catch (Exception ex)
        {
            // Log chi tiết lỗi nếu có
            Console.WriteLine($"Lỗi trong GetAllFood: {ex.Message}");
            throw;
        }
    }

    public List<Food> GetFoodByName(string food_name)
    {
        try
        {
            // Trả về danh sách tất cả thực phẩm từ cơ sở dữ liệu
            return _context.FoodEntity
            .Select(f => new Food
            {
                food_id = f.food_id,
                food_name = f.food_name,
                calories = f.calories,
                serving_unit = f.serving_unit
            }).Where(f => f.food_name.ToLower().Contains(food_name.ToLower()))
            .ToList();
        }
        catch (Exception ex)
        {
            // Log chi tiết lỗi nếu có
            Console.WriteLine($"Lỗi trong GetFoodByName: {ex.Message}");
            throw;
        }
    }
}
