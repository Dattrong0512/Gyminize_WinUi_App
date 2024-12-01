using Gyminize_API.Data.Model;
using Gyminize_API.Data.Models;
namespace Gyminize_API.Data.Repositories
{
    public class ProductRepository
    {
        private readonly EntityDatabaseContext _context;
        public ProductRepository(EntityDatabaseContext context)
        {
            _context = context;
        }
        public List<Product> GetallProduct()
        {
            try
            {
                return _context.ProductEntity.ToList();
            }
            catch (Exception ex)
            {
                // Log chi tiết lỗi
                Console.WriteLine($"Error in GetAllProduct: {ex.Message}");
                throw;
            }
        }
        public List<Product> GetAllProductByCategory(string category_name)
        {
            try
            {
                return _context.ProductEntity.Where(x => x.Category.category_name == category_name).ToList();
            }
            catch (Exception ex)
            {
                // Log chi tiết lỗi
                Console.WriteLine($"Error in GetAllProductByCategory: {ex.Message}");
                throw;
            }
        }
    }
}


