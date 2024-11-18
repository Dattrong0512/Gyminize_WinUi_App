using Gyminize_API.Data.Model;


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

    }
}


