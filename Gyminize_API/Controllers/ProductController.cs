using Gyminize_API.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
namespace Gyminize_API.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductRepository _productRepository;
        public ProductController(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            try
            {
                var allProducts = _productRepository.GetallProduct();
                return Ok(allProducts);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("get/category_name/{category}")]
        public IActionResult GetAllProductsByCategory(string category)
        {
            try
            {
                var allProducts = _productRepository.GetAllProductByCategory(category);
                return Ok(allProducts);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}




