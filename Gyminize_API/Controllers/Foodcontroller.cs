using Gyminize_API.Data.Model;
using Gyminize_API.Data.Models;
using Gyminize_API.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Gyminize_API.Controllers;

/// <summary>
/// Controller để quản lý các hoạt động liên quan đến thực phẩm (Food).
/// Các hành động trong controller này cho phép truy cập và lấy thông tin các loại thực phẩm.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class Foodcontroller : ControllerBase
{
    private readonly FoodRepository _foodRepository;

    /// <summary>
    /// Khởi tạo controller với repository để quản lý các loại thực phẩm.
    /// </summary>
    /// <param name="foodRepository">Repository chứa các phương thức thao tác với dữ liệu thực phẩm</param>
    public Foodcontroller(FoodRepository foodRepository)
    {
        _foodRepository = foodRepository;
    }

    /// <summary>
    /// Lấy tất cả các loại thực phẩm từ cơ sở dữ liệu.
    /// </summary>
    /// <returns>Danh sách tất cả các loại thực phẩm</returns>
    [HttpGet]
    public IActionResult GetAllFood()
    {
        try
        {

            var allFood = _foodRepository.GetAllFood(); // Lấy tất cả các loại thực phẩm từ repository
            return Ok(allFood); // Trả về danh sách thực phẩm
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}"); // Log lỗi nếu có
            return StatusCode(500, "Internal server error"); // Trả về lỗi 500 nếu có sự cố
        }
    }

    [HttpGet("get/food_name/{food_name}")]
    public IActionResult GetFoodByName(string food_name)
    {
        var foods = _foodRepository.GetFoodByName(food_name);
        if (foods == null)
        {
            return NotFound(); // Nếu không tìm thấy nhật ký hàng ngày, trả về lỗi 404
        }
        return Ok(foods); // Trả về nhật ký hàng ngày của khách hàng
    }
}
