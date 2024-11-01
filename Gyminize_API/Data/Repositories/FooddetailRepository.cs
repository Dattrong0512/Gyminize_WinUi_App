using Gyminize_API.Data.Model;
using Gyminize_API.Data.Models;
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
                return _context.FooddetailEntity.ToList();
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


