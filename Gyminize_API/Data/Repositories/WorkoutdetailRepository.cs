using Gyminize_API.Data.Model;

namespace Gyminize_API.Data.Repositories;

/// <summary>
/// Lớp Repository để quản lý các thao tác với Workoutdetail trong cơ sở dữ liệu.
/// </summary>
public class WorkoutdetailRepository
{
    private readonly EntityDatabaseContext _context;

    /// <summary>
    /// Khởi tạo một instance mới của lớp <see cref="WorkoutdetailRepository"/> với context cơ sở dữ liệu.
    /// </summary>
    /// <param name="context">Context cơ sở dữ liệu để tương tác với bảng Workoutdetail.</param>
    public WorkoutdetailRepository(EntityDatabaseContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Cập nhật mô tả (description) của một Workoutdetail.
    /// </summary>
    /// <param name="workoutdetail_id">ID của Workoutdetail cần cập nhật.</param>
    /// <param name="decription">Mô tả mới để cập nhật.</param>
    /// <returns>Đối tượng Workoutdetail đã được cập nhật.</returns>
    public Workoutdetail UpdateDecription(int workoutdetail_id, string decription)
    {
        // Tìm kiếm Workoutdetail theo workoutdetail_id
        var check_workoutdetail = _context.WorkoutDetailEntity
            .Where(x => x.workoutdetail_id == workoutdetail_id)
            .FirstOrDefault();

        if (check_workoutdetail != null)
        {
            // Kiểm tra và chuyển đổi thời gian về UTC nếu cần
            if (check_workoutdetail.date_workout.Kind == DateTimeKind.Unspecified)
            {
                check_workoutdetail.date_workout = DateTime.SpecifyKind(
                    check_workoutdetail.date_workout, DateTimeKind.Utc
                );
            }
            else if (check_workoutdetail.date_workout.Kind == DateTimeKind.Local)
            {
                check_workoutdetail.date_workout = check_workoutdetail.date_workout.ToUniversalTime();
            }

            // Cập nhật mô tả
            check_workoutdetail.description = decription;

            // Cập nhật thông tin vào cơ sở dữ liệu
            _context.WorkoutDetailEntity.Update(check_workoutdetail);
        }

        // Lưu thay đổi vào cơ sở dữ liệu
        _context.SaveChanges();

        // Trả về đối tượng Workoutdetail đã cập nhật
        return check_workoutdetail;
    }
}
