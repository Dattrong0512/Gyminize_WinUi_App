using Gyminize_API.Data.Model;
using Gyminize_API.Data.Models;

namespace Gyminize_API.Data.Repositories
{
    public class FoodRepository
    {
        private readonly EntityDatabaseContext _context;
        public FoodRepository(EntityDatabaseContext context)
        {
            _context = context;
        }
        public List<Food> GetAllFood()
        {
            try
            {
                return _context.FoodEntity.ToList();
            }
            catch (Exception ex)
            {
                // Log chi tiết lỗi
                Console.WriteLine($"Error in GetAllFood: {ex.Message}");
                throw;
            }
        }
        public Food? GetFoodById(int id)
        {
            return _context.FoodEntity.Where(x => x.food_id == id).FirstOrDefault();
        }
        public Food addFood(Food food)
        {
            _context.FoodEntity.Add(food);
            _context.SaveChanges();
            return food;
        }
        public Food updateFood(int id, Food food)
        {
            var check_food = _context.FoodEntity.Where(x => x.food_id == food.food_id).FirstOrDefault();
            if (check_food != null)
            {
                check_food.food_id = food.food_id;
                check_food.food_name = food.food_name;
                check_food.calories = food.calories;
                check_food.protein = food.protein;
                check_food.carbs = food.carbs;
                check_food.fats = food.fats;
                return food;
            }
            _context.SaveChanges();
            return check_food;
        }
        public void DeleteFood(Food food)
        {
            var check_food = _context.FoodEntity.Where(x => x.food_id == food.food_id).FirstOrDefault();
            if (check_food != null)
            {
                _context.FoodEntity.Remove(food);
            }
            _context.SaveChanges();
        }

    }
}


