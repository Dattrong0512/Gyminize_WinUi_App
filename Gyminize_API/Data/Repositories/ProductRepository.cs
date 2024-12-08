using Gyminize_API.Data.Model;
using Gyminize_API.Data.Models;

namespace Gyminize_API.Data.Repositories;

/// <summary>
/// Lớp Repository để quản lý các thao tác với sản phẩm (Product) trong cơ sở dữ liệu.
/// </summary>
public class ProductRepository
{
    private readonly EntityDatabaseContext _context;

    /// <summary>
    /// Khởi tạo một instance mới của lớp <see cref="ProductRepository"/> với context cơ sở dữ liệu.
    /// </summary>
    /// <param name="context">Context cơ sở dữ liệu để tương tác với bảng sản phẩm.</param>
    public ProductRepository(EntityDatabaseContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lấy danh sách tất cả các sản phẩm từ cơ sở dữ liệu.
    /// </summary>
    /// <returns>Danh sách các sản phẩm.</returns>
    public List<Product> GetallProduct()
    {
        try
        {
            return _context.ProductEntity.ToList(); // Lấy tất cả sản phẩm trong cơ sở dữ liệu
        }
        catch (Exception ex)
        {
            // Log lỗi nếu có
            Console.WriteLine($"Error in GetAllProduct: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách sản phẩm theo danh mục (category) từ cơ sở dữ liệu.
    /// </summary>
    /// <param name="category_name">Tên danh mục cần tìm sản phẩm.</param>
    /// <returns>Danh sách các sản phẩm thuộc danh mục đã cho.</returns>
    public List<Product> GetAllProductByCategory(string category_name)
    {
        try
        {
            return _context.ProductEntity
                .Where(x => x.Category.category_name == category_name) // Lọc sản phẩm theo danh mục
                .ToList();
        }
        catch (Exception ex)
        {
            // Log lỗi nếu có
            Console.WriteLine($"Error in GetAllProductByCategory: {ex.Message}");
            throw;
        }
    }
}
