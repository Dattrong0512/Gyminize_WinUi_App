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

        //public List<Fooddetail> GetAllFooddetail()
        //{
        //    try
        //    {
        //        // Sử dụng Include để nạp đối tượng Food liên quan
        //        return _context.FooddetailEntity
        //            .Include(fd => fd.Food)
        //            .ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log chi tiết lỗi
        //        Console.WriteLine($"Error in GetAllFooddetail: {ex.Message}");
        //        throw;
        //    }
        //}

        //public List<Fooddetail> GetFoodDetailsByCustomerId(int customerId)
        //{
        //    try
        //    {
        //        DateTime today = new DateTime(2024, 11, 1);
        //        today = DateTime.SpecifyKind(today, DateTimeKind.Utc);



        //        // Tìm daily_diary_id của customer trong ngày hiện tại
        //        var dailyDiary = _context.DailydiaryEntity
        //            .FirstOrDefault(d => d.customer_id == customerId && d.diary_date == today);

        //        if (dailyDiary == null)
        //        {
        //            // Nếu không tìm thấy daily_diary cho ngày hiện tại, trả về danh sách rỗng
        //            return new List<Fooddetail>();
        //        }

        //        int dailyDiaryId = dailyDiary.dailydiary_id;

        //        // Trả về tất cả fooddetail của daily_diary_id tìm được
        //        return _context.FooddetailEntity
        //            .Where(fd => fd.dailydiary_id == dailyDiaryId)
        //            .ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log chi tiết lỗi nếu có lỗi xảy ra
        //        Console.WriteLine($"Error in GetFoodDetailsByCustomerId: {ex.Message}");
        //        throw;
        //    }
        //}
        public Fooddetail? GetFooddetailById(int id)
        {
            return _context.FooddetailEntity.Where(x => x.food_id == id).FirstOrDefault();
        }
        public Fooddetail addFooddetail(Fooddetail fooddetail)
        {
            _context.FooddetailEntity.Add(fooddetail);
            _context.SaveChanges();
            return fooddetail;
        }
        public Fooddetail AddOrUpdateFooddetail(int dailydiaryId, int mealType, Food food, int food_amount)
        {
            try
            {
                // Tìm kiếm đối tượng `Food` trong `DbContext`
                var existingFood = _context.FoodEntity.FirstOrDefault(f => f.food_id == food.food_id);
                if (existingFood != null)
                {
                    food = existingFood; // Sử dụng `Food` đã tồn tại trong `DbContext`
                }
                else
                {
                    _context.FoodEntity.Attach(food); // Đính kèm `Food` nếu chưa được theo dõi
                }

                // Tìm `Fooddetail` dựa trên `dailydiary_id`, `meal_type`, và `food_id`
                var existingFooddetail = _context.FooddetailEntity
                    .Include(fd => fd.Food)
                    .FirstOrDefault(fd => fd.dailydiary_id == dailydiaryId && fd.meal_type == mealType && fd.food_id == food.food_id);

                if (existingFooddetail != null)
                {
                    // Nếu `Fooddetail` đã tồn tại với cùng `meal_type` và `food`, cộng thêm `food_amount`
                    existingFooddetail.food_amount += food_amount;
                }
                else
                {
                    // Kiểm tra nếu đã có `Fooddetail` với `meal_type` nhưng `food_id` khác
                    var sameMealTypeFooddetail = _context.FooddetailEntity
                        .FirstOrDefault(fd => fd.dailydiary_id == dailydiaryId && fd.meal_type == mealType);

                    // Nếu `meal_type` đã tồn tại với `food_id` khác, tạo mới `Fooddetail`
                    var newFooddetail = new Fooddetail
                    {
                        dailydiary_id = dailydiaryId,
                        meal_type = mealType,
                        food_amount = food_amount,
                        Food = food
                    };

                    _context.FooddetailEntity.Add(newFooddetail);
                    existingFooddetail = newFooddetail;
                }

                // Lưu thay đổi vào cơ sở dữ liệu
                _context.SaveChanges();
                return existingFooddetail;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddOrUpdateFooddetail: {ex.Message}");
                throw;
            }
        }




        public bool DeleteFoodFromFooddetail(int dailydiaryId, int mealType, Food food)
        {
            try
            {
                // Tìm `Fooddetail` dựa trên `dailydiary_id` và `meal_type`
                var existingFooddetail = _context.FooddetailEntity
                    .Include(fd => fd.Food)
                    .FirstOrDefault(fd => fd.dailydiary_id == dailydiaryId && fd.meal_type == mealType);

                if (existingFooddetail != null && existingFooddetail.Food != null)
                {
                    // Kiểm tra xem `Food` và `food_amount` có khớp với `Fooddetail` không
                    if (existingFooddetail.Food.food_id == food.food_id)
                    {
                        // Xóa `Food` khỏi `Fooddetail` nếu khớp
                        existingFooddetail.Food = null;
                        existingFooddetail.food_amount = 0; // Đặt `food_amount` về 0 hoặc giá trị mặc định
                        _context.SaveChanges();
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Food or amount does not match the existing Fooddetail.");
                        return false;
                    }
                }

                Console.WriteLine("Fooddetail or Food not found.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteFoodFromFooddetail: {ex.Message}");
                return false;
            }
        }


        //public Fooddetail updateFooddetail(int id, Fooddetail fooddetail)
        //{
        //    var check_fooddetail = _context.FooddetailEntity.Where(x => x.food_id == fooddetail.food_id).FirstOrDefault();
        //    if (check_fooddetail != null)
        //    {
        //        check_fooddetail.food_id = fooddetail.food_id;
        //        check_fooddetail.dailydiary_id = fooddetail.dailydiary_id;
        //        check_fooddetail.food_amount = fooddetail.food_amount;
        //        check_fooddetail.meal_type = fooddetail.meal_type;
        //        return fooddetail;
        //    }
        //    _context.SaveChanges();
        //    return check_fooddetail;
        //}
        //public void DeleteFooddetail(Fooddetail fooddetail)
        //{
        //    var check_fooddetail = _context.FooddetailEntity.Where(x => x.food_id == fooddetail.food_id).FirstOrDefault();
        //    if (check_fooddetail != null)
        //    {
        //        _context.FooddetailEntity.Remove(fooddetail);
        //    }
        //    _context.SaveChanges();
        //}
    }
}


