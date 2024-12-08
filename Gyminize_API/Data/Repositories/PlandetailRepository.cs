using Gyminize_API.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Gyminize_API.Data.Repositories;

/// <summary>
/// Lớp Repository để quản lý các thao tác với chi tiết kế hoạch (PlanDetail) trong cơ sở dữ liệu.
/// </summary>
public class PlandetailRepository
{
    private readonly EntityDatabaseContext _context;

    /// <summary>
    /// Khởi tạo một instance mới của lớp <see cref="PlandetailRepository"/> với context cơ sở dữ liệu.
    /// </summary>
    /// <param name="context">Context cơ sở dữ liệu để tương tác với chi tiết kế hoạch.</param>
    public PlandetailRepository(EntityDatabaseContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lấy thông tin chi tiết kế hoạch (PlanDetail) của khách hàng dựa trên customerID.
    /// </summary>
    /// <param name="customerID">ID của khách hàng cần tìm chi tiết kế hoạch.</param>
    /// <returns>Chi tiết kế hoạch nếu tìm thấy, ngược lại trả về null.</returns>
    public Plandetail? GetPlandetailById(int customerID)
    {
        var planDetail = _context.PlanDetailEntity
            .Include(pd => pd.Plan) // Bao gồm đối tượng Plan
            .Include(pd => pd.Workoutdetails) // Bao gồm danh sách Workoutdetails từ Plandetail
                .ThenInclude(wd => wd.Typeworkout) // Bao gồm Typeworkout từ mỗi Workoutdetail
                .ThenInclude(tw => tw.Exercisedetails) // Bao gồm danh sách Exercisedetails từ Typeworkout
                    .ThenInclude(ed => ed.Exercise) // Bao gồm đối tượng Exercise từ mỗi Exercisedetail
            .Where(pd => pd.customer_id == customerID)
            .FirstOrDefault(); // Kết thúc truy vấn với FirstOrDefault để trả về một Plandetail

        return planDetail;
    }

    /// <summary>
    /// Thêm mới chi tiết kế hoạch cho khách hàng nếu chưa có kế hoạch hoặc kế hoạch đã hết hạn.
    /// </summary>
    /// <param name="customerId">ID của khách hàng cần thêm chi tiết kế hoạch.</param>
    /// <param name="plan">ID của kế hoạch cần thêm vào chi tiết kế hoạch.</param>
    /// <returns>Chi tiết kế hoạch mới được thêm nếu thành công, ngược lại trả về null.</returns>
    public Plandetail AddPlandetail(int customerId, int plan)
    {
        try
        {
            // Kiểm tra nếu khách hàng đã có chi tiết kế hoạch
            var Plandetail = _context.PlanDetailEntity.FirstOrDefault(pl => pl.customer_id == customerId);
            DateTime date = DateTime.Now;

            // Nếu khách hàng đã có kế hoạch và kế hoạch chưa hết hạn, không thêm chi tiết kế hoạch mới
            if (Plandetail != null && Plandetail.end_date > date)
            {
                return null;
            }
            else
            {
                // Tạo chi tiết kế hoạch mới
                DateTime start_day = DateTime.SpecifyKind(DateTime.Now.Date, DateTimeKind.Utc);
                DateTime endDate = start_day.AddDays(56); // Kế hoạch có thể kéo dài 56 ngày

                Plandetail plandetail = new Plandetail
                {
                    customer_id = customerId,
                    plan_id = plan,
                    created_at = start_day,
                    start_date = start_day,
                    end_date = endDate
                };

                // Thêm chi tiết kế hoạch vào cơ sở dữ liệu và lưu thay đổi
                _context.PlanDetailEntity.Add(plandetail);
                _context.SaveChanges();
                return plandetail;
            }
        }
        catch (Exception ex)
        {
            // Xử lý lỗi (nếu có)
            throw;
        }
    }
}
