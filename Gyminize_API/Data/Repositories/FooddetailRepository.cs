using Gyminize_API.Data.Model;
using Gyminize_API.Data.Models;
using Microsoft.EntityFrameworkCore;
namespace Gyminize_API.Data.Repositories
{
    public class FooddetailRepository
    {
        private readonly EntityDatabaseContext _context;
        public FooddetailRepository(EntityDatabaseContext context)
        {
            _context = context;
        }

        public List<Fooddetail> GetAllFooddetail()
        {
            try
            {
                // Sử dụng Include để nạp đối tượng Food liên quan
                return _context.FooddetailEntity
                    .Include(fd => fd.Food)
                    .ToList();
            }
            catch (Exception ex)
            {
                // Log chi tiết lỗi
                Console.WriteLine($"Error in GetAllFooddetail: {ex.Message}");
                throw;
            }
        }
        public Fooddetail? GetFooddetailById(int id)
        {
            return _context.FooddetailEntity.Where(x => x.food_id == id).FirstOrDefault();
        }
        public List<Fooddetail> GetFoodDetailsByCustomerId(int customerId)
        {
            try
            {
                DateTime today = new DateTime(2024, 11, 1);
                today = DateTime.SpecifyKind(today, DateTimeKind.Utc);



                // Tìm daily_diary_id của customer trong ngày hiện tại
                var dailyDiary = _context.DailydiaryEntity
                    .FirstOrDefault(d => d.customer_id == customerId && d.diary_date == today);

                if (dailyDiary == null)
                {
                    // Nếu không tìm thấy daily_diary cho ngày hiện tại, trả về danh sách rỗng
                    return new List<Fooddetail>();
                }

                int dailyDiaryId = dailyDiary.dailydiary_id;

                // Trả về tất cả fooddetail của daily_diary_id tìm được
                return _context.FooddetailEntity
                    .Where(fd => fd.dailydiary_id == dailyDiaryId)
                    .ToList();
            }
            catch (Exception ex)
            {
                // Log chi tiết lỗi nếu có lỗi xảy ra
                Console.WriteLine($"Error in GetFoodDetailsByCustomerId: {ex.Message}");
                throw;
            }
        }
        public Fooddetail addFooddetail(Fooddetail fooddetail)
        {
            _context.FooddetailEntity.Add(fooddetail);
            _context.SaveChanges();
            return fooddetail;
        }
        public Fooddetail updateFooddetail(int id, Fooddetail fooddetail)
        {
            var check_fooddetail = _context.FooddetailEntity.Where(x => x.food_id == fooddetail.food_id).FirstOrDefault();
            if (check_fooddetail != null)
            {
                check_fooddetail.food_id = fooddetail.food_id;
                check_fooddetail.dailydiary_id = fooddetail.dailydiary_id;
                check_fooddetail.food_amount = fooddetail.food_amount;
                check_fooddetail.meal_type = fooddetail.meal_type;
                return fooddetail;
            }
            _context.SaveChanges();
            return check_fooddetail;
        }
        public void DeleteFooddetail(Fooddetail fooddetail)
        {
            var check_fooddetail = _context.FooddetailEntity.Where(x => x.food_id == fooddetail.food_id).FirstOrDefault();
            if (check_fooddetail != null)
            {
                _context.FooddetailEntity.Remove(fooddetail);
            }
            _context.SaveChanges();
        }
    }
}


