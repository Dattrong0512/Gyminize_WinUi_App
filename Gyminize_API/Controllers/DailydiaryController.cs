using Gyminize_API.Data.Model;
using Gyminize_API.Data.Models;
using Gyminize_API.Data.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Gyminize_API.Controllers;

/// <summary>
/// Controller để quản lý các hoạt động liên quan đến nhật ký hàng ngày (Daily Diary).
/// Các hành động trong controller này cho phép truy cập, tạo mới và lấy thông tin nhật ký hàng ngày.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class DailydiaryController : ControllerBase
{
    private readonly DailydiaryRepository _dailyDiaryRepository;

    /// <summary>
    /// Khởi tạo controller với repository để quản lý nhật ký hàng ngày.
    /// </summary>
    /// <param name="dailyDiaryRepository">Repository chứa các phương thức thao tác với dữ liệu nhật ký hàng ngày</param>
    public DailydiaryController(DailydiaryRepository dailyDiaryRepository)
    {
        _dailyDiaryRepository = dailyDiaryRepository;
    }

    /// <summary>
    /// Lấy tất cả các nhật ký hàng ngày từ cơ sở dữ liệu.
    /// </summary>
    /// <returns>Danh sách tất cả các nhật ký hàng ngày</returns>
    [HttpGet]
    public IActionResult GetAllDailyDiary()
    {
        try
        {
            var allDailyDiary = _dailyDiaryRepository.GetAllDailydiary();
            return Ok(allDailyDiary); // Trả về danh sách nhật ký hàng ngày
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}"); // Log lỗi nếu có
            return StatusCode(500, "Internal server error"); // Trả về lỗi 500 nếu có sự cố
        }
    }

    /// <summary>
    /// Lấy nhật ký hàng ngày của khách hàng theo ID và ngày cụ thể.
    /// </summary>
    /// <param name="customer_id">ID khách hàng</param>
    /// <param name="day">Ngày cần lấy nhật ký</param>
    /// <returns>Trả về nhật ký hàng ngày của khách hàng nếu tồn tại, hoặc lỗi 404 nếu không tìm thấy</returns>
    [HttpGet("get/daily_customer/{customer_id:int}/day/{day}")]
    public IActionResult GetDailydiaryByIdCustomer(int customer_id, DateTime day)
    {
        var dailyDiary = _dailyDiaryRepository.GetDailydiaryByIdCustomer(customer_id, day);
        if (dailyDiary == null)
        {
            return NotFound(); // Nếu không tìm thấy nhật ký hàng ngày, trả về lỗi 404
        }
        return Ok(dailyDiary); // Trả về nhật ký hàng ngày của khách hàng
    }

    /// <summary>
    /// Thêm một nhật ký hàng ngày mới vào cơ sở dữ liệu.
    /// </summary>
    /// <param name="dailyDiary">Đối tượng nhật ký hàng ngày cần thêm vào</param>
    /// <returns>Trả về đối tượng nhật ký hàng ngày mới được tạo</returns>
    [HttpPost("create")]
    public IActionResult AddDailyDiary(Dailydiary dailyDiary)
    {
        var newDailyDiary = _dailyDiaryRepository.AddDailydiary(dailyDiary); // Thêm nhật ký mới vào cơ sở dữ liệu
        return Ok(newDailyDiary); // Trả về nhật ký mới vừa được tạo
    }
}
