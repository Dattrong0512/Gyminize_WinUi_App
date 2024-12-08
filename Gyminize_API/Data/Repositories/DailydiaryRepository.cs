using Gyminize_API.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Gyminize_API.Data.Repositories;

/// <summary>
/// Lớp Repository để quản lý các thao tác dữ liệu nhật ký hàng ngày của khách hàng trong cơ sở dữ liệu.
/// </summary>
public class DailydiaryRepository
{
    private readonly EntityDatabaseContext _context;

    /// <summary>
    /// Khởi tạo một instance mới của lớp <see cref="DailydiaryRepository"/>.
    /// </summary>
    /// <param name="context">Context cơ sở dữ liệu để tương tác với dữ liệu nhật ký hàng ngày.</param>
    public DailydiaryRepository(EntityDatabaseContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lấy tất cả các bản ghi nhật ký hàng ngày từ cơ sở dữ liệu.
    /// </summary>
    /// <returns>Danh sách tất cả các bản ghi nhật ký hàng ngày.</returns>
    public List<Dailydiary> GetAllDailydiary()
    {
        try
        {
            return _context.DailydiaryEntity.ToList();
        }
        catch (Exception ex)
        {
            // Log chi tiết lỗi
            Console.WriteLine($"Lỗi trong GetAllDailydiary: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Lấy nhật ký hàng ngày của một khách hàng theo ID khách hàng và ngày.
    /// </summary>
    /// <param name="customerId">Mã ID khách hàng.</param>
    /// <param name="day">Ngày của nhật ký cần lấy.</param>
    /// <returns>Nhật ký hàng ngày của khách hàng nếu tìm thấy, nếu không trả về null.</returns>
    public Dailydiary? GetDailydiaryByIdCustomer(int customerId, DateTime day)
    {
        try
        {
            // Lấy tất cả các bản ghi của customerId từ cơ sở dữ liệu và bao gồm chi tiết thực phẩm
            var context = _context.DailydiaryEntity
                .Include(dd => dd.Fooddetails)
                .ThenInclude(fd => fd.Food)
                .Where(dd => dd.customer_id == customerId)
                .ToList();

            // Lọc theo ngày để tìm nhật ký đúng với ngày yêu cầu
            var filteredList = context
                .FirstOrDefault(dd => dd.diary_date.Date == day.Date);
            return filteredList;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi trong GetDailydiaryByIdCustomer: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Thêm một bản ghi nhật ký hàng ngày mới vào cơ sở dữ liệu.
    /// </summary>
    /// <param name="dailydiary">Dữ liệu nhật ký hàng ngày cần thêm.</param>
    /// <returns>Dữ liệu nhật ký hàng ngày đã được thêm vào cơ sở dữ liệu.</returns>
    public Dailydiary AddDailydiary(Dailydiary dailydiary)
    {
        _context.DailydiaryEntity.Add(dailydiary);
        _context.SaveChanges();
        return dailydiary;
    }
}
