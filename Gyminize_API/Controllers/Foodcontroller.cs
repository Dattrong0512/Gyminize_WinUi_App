using Gyminize_API.Data.Model;
using Gyminize_API.Data.Models;
using Gyminize_API.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Gyminize_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Foodcontroller : ControllerBase
    {
        private readonly FoodRepository _foodRepository;
        public Foodcontroller(FoodRepository foodRepository)
        {
            _foodRepository = foodRepository;
        }
        [HttpGet]
        public IActionResult GetAllFood()
        {
            try
            {
                var allFood = _foodRepository.GetAllFood();
                return Ok(allFood);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}


