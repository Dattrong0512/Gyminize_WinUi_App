using Gyminize_API.Data.Models;
using Gyminize_API.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Gyminize_API.Controllers;

/// <summary>
/// Controller để quản lý các cập nhật chi tiết bài tập, bao gồm việc cập nhật mô tả bài tập.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class WorkoutdetailController : ControllerBase
{
    private readonly WorkoutdetailRepository _workoutdetailRepository;

    /// <summary>
    /// Khởi tạo một instance của <see cref="WorkoutdetailController"/> với repository được chỉ định.
    /// </summary>
    /// <param name="workoutdetailRepository">Repository để quản lý dữ liệu chi tiết bài tập.</param>
    public WorkoutdetailController(WorkoutdetailRepository workoutdetailRepository)
    {
        _workoutdetailRepository = workoutdetailRepository;
    }

    /// <summary>
    /// Cập nhật mô tả chi tiết bài tập theo ID của bài tập.
    /// </summary>
    /// <param name="workoutId">Mã ID duy nhất của bài tập cần cập nhật.</param>
    /// <param name="decription">Mô tả mới cần cập nhật cho bài tập.</param>
    /// <returns>Trả về thông tin chi tiết bài tập đã được cập nhật nếu thành công, nếu không sẽ trả về lỗi 500 Internal Server Error.</returns>
    [HttpPut("update/{workoutId:int}/decription/{decription}")]
    public IActionResult UpdateDecriptionWorkoutDetail(int workoutId, string decription)
    {
        var workoutDetail = _workoutdetailRepository.UpdateDecription(workoutId, decription);

        if (workoutDetail == null)
        {
            return NotFound("Không tìm thấy chi tiết bài tập");
        }

        return Ok(workoutDetail);
    }
}
