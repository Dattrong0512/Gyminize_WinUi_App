using Gyminize_API.Data.Model;
using Gyminize_API.Data.Models;
using Gyminize_API.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
namespace Gyminize_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodetailController : ControllerBase
    {
        private readonly FooddetailRepository _foodetailRepository;
        public FoodetailController(FooddetailRepository foodetailRepository)
        {
            _foodetailRepository = foodetailRepository;
        }
        [HttpGet]
        public IActionResult GetAllFoodetail()
        {
            try
            {
                var allFoodetail = _foodetailRepository.GetAllFooddetail();
                return Ok(allFoodetail);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("get/{food_id:int}")]
        public IActionResult GetFooddetailById(int food_id)
        {
            var foodetail = _foodetailRepository.GetFooddetailById(food_id);
            if (foodetail == null)
            {
                return NotFound();
            }
            return Ok(foodetail);
        }
        [HttpPost("create")]
        public IActionResult AddFooddetail(Fooddetail foodetail)
        {
            var newFoodetail = _foodetailRepository.addFooddetail(foodetail);
            return Ok(newFoodetail);
        }
        [HttpPut("update/{food_id:int}")]
        public IActionResult UpdateFooddetail(int food_id, Fooddetail foodetail)
        {
            var updateFooddetail = _foodetailRepository.updateFooddetail(food_id, foodetail);
            return Ok(updateFooddetail);
        }
        [HttpDelete("delete/{food_id:int}")]
        public IActionResult DeleteFooddetail(int food_id)
        {
            var foodetail = _foodetailRepository.GetFooddetailById(food_id);
            if (foodetail == null)
            {
                return NotFound();
            }
            else
            {
                _foodetailRepository.DeleteFooddetail(foodetail);
            }
            return Ok();
        }

    }
}


