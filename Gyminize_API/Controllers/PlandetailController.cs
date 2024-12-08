using Gyminize_API.Data.Model;
using Gyminize_API.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Gyminize_API.Controllers;

/// <summary>
/// Controller quản lý các hoạt động liên quan đến chi tiết kế hoạch của khách hàng,
/// bao gồm việc lấy thông tin chi tiết kế hoạch và thêm chi tiết kế hoạch mới cho khách hàng.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class PlandetailController : ControllerBase
{
    private readonly PlandetailRepository _plandetailRepository;

    /// <summary>
    /// Khởi tạo controller với repository để quản lý các hoạt động chi tiết kế hoạch.
    /// </summary>
    /// <param name="plandetailRepository">Repository chứa các phương thức thao tác với dữ liệu chi tiết kế hoạch</param>
    public PlandetailController(PlandetailRepository plandetailRepository)
    {
        _plandetailRepository = plandetailRepository;
    }

    /// <summary>
    /// Lấy thông tin chi tiết kế hoạch của khách hàng dựa trên ID khách hàng.
    /// </summary>
    /// <param name="customerId">ID khách hàng cần lấy thông tin chi tiết kế hoạch</param>
    /// <returns>Trả về chi tiết kế hoạch nếu tìm thấy, hoặc trả về lỗi 404 nếu không tìm thấy</returns>
    [HttpGet("get/plandetail/{customerId:int}")]
    public IActionResult GetPlandetailByCustomerId(int customerId)
    {
        var plandetail = _plandetailRepository.GetPlandetailById(customerId);
        if (plandetail == null)
        {
            return NotFound(); // Trả về lỗi 404 nếu không tìm thấy chi tiết kế hoạch
        }
        return Ok(plandetail); // Trả về chi tiết kế hoạch nếu tìm thấy
    }

    /// <summary>
    /// Thêm một chi tiết kế hoạch mới cho khách hàng.
    /// </summary>
    /// <param name="customerId">ID khách hàng cần thêm chi tiết kế hoạch</param>
    /// <param name="plan">ID kế hoạch cần thêm cho khách hàng</param>
    /// <returns>Trả về thông tin chi tiết kế hoạch đã thêm vào hệ thống</returns>
    [HttpPost("create/customer_id/{customerId:int}/plan/{plan:int}")]
    public IActionResult AddPlandetail(int customerId, int plan)
    {
        var newPlandetail = _plandetailRepository.AddPlandetail(customerId, plan);
        return Ok(newPlandetail); // Trả về chi tiết kế hoạch đã thêm
    }
}
