using Gyminize_API.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Gyminize_API.Controllers;

/// <summary>
/// Controller quản lý các hoạt động liên quan đến sản phẩm, bao gồm việc lấy danh sách sản phẩm và lọc sản phẩm theo danh mục.
/// </summary>
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ProductRepository _productRepository;

    /// <summary>
    /// Khởi tạo controller với repository để quản lý các hoạt động liên quan đến sản phẩm.
    /// </summary>
    /// <param name="productRepository">Repository chứa các phương thức thao tác với dữ liệu sản phẩm</param>
    public ProductController(ProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    /// <summary>
    /// Lấy danh sách tất cả các sản phẩm.
    /// </summary>
    /// <returns>Trả về danh sách sản phẩm nếu thành công, hoặc trả về lỗi 500 nếu có lỗi hệ thống.</returns>
    [HttpGet]
    public IActionResult GetAllProducts()
    {
        try
        {
            var allProducts = _productRepository.GetallProduct();
            return Ok(allProducts); // Trả về danh sách sản phẩm
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, "Internal server error"); // Trả về lỗi 500 nếu có lỗi
        }
    }

    /// <summary>
    /// Lọc danh sách sản phẩm theo danh mục.
    /// </summary>
    /// <param name="category">Tên danh mục sản phẩm cần tìm kiếm</param>
    /// <returns>Trả về danh sách sản phẩm thuộc danh mục nếu thành công, hoặc trả về lỗi 500 nếu có lỗi hệ thống.</returns>
    [HttpGet("get/category_name/{category}")]
    public IActionResult GetAllProductsByCategory(string category)
    {
        try
        {
            var allProducts = _productRepository.GetAllProductByCategory(category);
            return Ok(allProducts); // Trả về danh sách sản phẩm theo danh mục
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, "Internal server error"); // Trả về lỗi 500 nếu có lỗi
        }
    }
}
