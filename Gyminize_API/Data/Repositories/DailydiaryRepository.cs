using Gyminize_API.Data.Model;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;
namespace Gyminize_API.Data.Repositories
{
    public class DailydiaryRepository
    {
        private readonly EntityDatabaseContext _context;
        public DailydiaryRepository(EntityDatabaseContext context)
        {
            _context = context;
        }
        public List<Dailydiary> GetAllDailydiary()
        {
            try
            {
                return _context.DailydiaryEntity.ToList();
            }
            catch (Exception ex)
            {
                // Log chi tiết lỗi
                Console.WriteLine($"Error in GetAllDailydiary: {ex.Message}");
                throw;
            }

        }
        public Dailydiary? GetDailydiaryByIdCustomer(int customerId, DateTime day)
        {
            try
            {
                          
                // Lấy tất cả các bản ghi của customerId từ cơ sở dữ liệu
                var context = _context.DailydiaryEntity
                    .Include(dd => dd.Fooddetails)
                    .ThenInclude(fd => fd.Food)
                    .Where(dd => dd.customer_id == customerId)
                    .ToList();
           
                var filteredList = context
                    .FirstOrDefault(dd => dd.diary_date.Date == day.Date);
                return filteredList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDailydiaryByIdCustomer: {ex.Message}");
                throw;
            }
        }

        public Dailydiary addDailydiary(Dailydiary dailydiary)
        {
            _context.DailydiaryEntity.Add(dailydiary);
            _context.SaveChanges();
            return dailydiary;
        }
    }
}



