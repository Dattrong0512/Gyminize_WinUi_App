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

                // Lọc các bản ghi có diary_date là ngày 2-11-2024 từ danh sách đã lấy
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




        public Dailydiary? GetDailydiaryById(int id)
        {
            return _context.DailydiaryEntity.Where(x => x.dailydiary_id == id).FirstOrDefault();
        }
        public Dailydiary addDailydiary(Dailydiary dailydiary)
        {
            _context.DailydiaryEntity.Add(dailydiary);
            _context.SaveChanges();
            return dailydiary;
        }
        public Dailydiary updateDailydiary(int id, Dailydiary dailydiary)
        {
            var check_dailydiary = _context.DailydiaryEntity.Where(x => x.dailydiary_id == dailydiary.dailydiary_id).FirstOrDefault();
            if (check_dailydiary != null)
            {
                check_dailydiary.dailydiary_id = dailydiary.dailydiary_id;
                check_dailydiary.customer_id = dailydiary.customer_id;
                check_dailydiary.diary_date = dailydiary.diary_date;
                check_dailydiary.calories_remain = dailydiary.calories_remain;
                check_dailydiary.daily_weight = dailydiary.daily_weight;
                check_dailydiary.total_calories = dailydiary.total_calories;
                check_dailydiary.notes = dailydiary.notes;
                return dailydiary;
            }
            _context.SaveChanges();
            return check_dailydiary;
        }
        public void DeleteDailydiary(Dailydiary dailydiary)
        {
            var check_dailydiary = _context.DailydiaryEntity.Where(x => x.dailydiary_id == dailydiary.dailydiary_id).FirstOrDefault();
            if (check_dailydiary != null)
            {
                _context.DailydiaryEntity.Remove(dailydiary);
            }
            _context.SaveChanges();
        }
        public List<Dailydiary> GetDailydiaryByCustomerId(int customerId)
        {
            return _context.DailydiaryEntity.Where(x => x.customer_id == customerId).ToList();
        }
    }
}



