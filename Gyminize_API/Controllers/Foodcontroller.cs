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
        [HttpGet("get/{food_id:int}")]
        public IActionResult GetFoodById(int food_id)
        {
            var food = _foodRepository.GetFoodById(food_id);
            if (food == null)
            {
                return NotFound();
            }
            return Ok(food);
        }
        [HttpPost("create")]
        public IActionResult AddFood(Food food)
        {
            var newFood = _foodRepository.addFood(food);
            return Ok(newFood);
        }
        [HttpPut("update/{food_id:int}")]
        public IActionResult UpdateFood(int food_id, Food food)
        {
            var updateFood = _foodRepository.updateFood(food_id, food);
            return Ok(updateFood);
        }
        [HttpDelete("delete/{food_id:int}")]
        public IActionResult DeleteFood(int food_id)
        {
            var food = _foodRepository.GetFoodById(food_id);
            if (food == null)
            {
                return NotFound();
            }
            else
            {
                _foodRepository.DeleteFood(food);
            }
            return Ok();
        }
    }
}


