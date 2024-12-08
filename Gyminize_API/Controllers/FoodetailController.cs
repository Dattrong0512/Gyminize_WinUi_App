using Gyminize_API.Data.Model;
using Gyminize_API.Data.Models;
using Gyminize_API.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Gyminize_API.Controllers;

/// <summary>
/// Controller để quản lý các hoạt động liên quan đến chi tiết thực phẩm trong nhật ký hàng ngày.
/// Các hành động trong controller này cho phép cập nhật và xóa thông tin thực phẩm trong nhật ký.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class FoodetailController : ControllerBase
{
    private readonly FooddetailRepository _foodetailRepository;

    /// <summary>
    /// Khởi tạo controller với repository để quản lý các chi tiết thực phẩm.
    /// </summary>
    /// <param name="foodetailRepository">Repository chứa các phương thức thao tác với dữ liệu chi tiết thực phẩm</param>
    public FoodetailController(FooddetailRepository foodetailRepository)
    {
        _foodetailRepository = foodetailRepository;
    }

    /// <summary>
    /// Cập nhật hoặc thêm chi tiết thực phẩm vào nhật ký hàng ngày.
    /// </summary>
    /// <param name="fooddetail">Thông tin chi tiết thực phẩm cần cập nhật</param>
    /// <returns>Trả về đối tượng chi tiết thực phẩm đã được cập nhật hoặc thêm mới</returns>
    [HttpPut("update")]
    public IActionResult UpdateFooddetail([FromBody] Fooddetail fooddetail)
    {
        var updateFooddetail = _foodetailRepository.AddOrUpdateFooddetail(
            fooddetail.dailydiary_id,
            fooddetail.meal_type,
            fooddetail.Food,
            fooddetail.food_amount
        );
        return Ok(updateFooddetail); // Trả về chi tiết thực phẩm đã cập nhật
    }

    /// <summary>
    /// Xóa chi tiết thực phẩm khỏi nhật ký hàng ngày.
    /// </summary>
    /// <param name="fooddetail">Thông tin chi tiết thực phẩm cần xóa</param>
    /// <returns>Trả về trạng thái thành công sau khi xóa</returns>
    [HttpDelete("delete")]
    public IActionResult DeleteFooddetail([FromBody] Fooddetail fooddetail)
    {
        var deleteFood = _foodetailRepository.DeleteFoodFromFooddetail(
            fooddetail.dailydiary_id,
            fooddetail.meal_type,
            fooddetail.Food
        );
        return Ok(); // Trả về trạng thái thành công sau khi xóa
    }
}
