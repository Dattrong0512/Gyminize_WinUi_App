using Gyminize_API.Data.Model;
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



