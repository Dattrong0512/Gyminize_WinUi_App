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
        //[HttpGet]
        //public IActionResult GetAllFoodetail()
        //{
        //    try
        //    {
        //        var allFoodetail = _foodetailRepository.GetAllFooddetail();
        //        return Ok(allFoodetail);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error: {ex.Message}");
        //        return StatusCode(500, "Internal server error");
        //    }
        //}
        //[HttpGet("get/{food_id:int}")]
        //public IActionResult GetFooddetailById(int food_id)
        //{
        //    var foodetail = _foodetailRepository.GetFooddetailById(food_id);
        //    if (foodetail == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(foodetail);
        //}

        //[HttpGet("get/dailyfood/{customer_id:int}")]
        //public IActionResult GetFoodDetailsByCustomerId(int customer_id)
        //{
        //    var foodDetails = _foodetailRepository.GetFoodDetailsByCustomerId(customer_id);
        //    if (foodDetails == null || !foodDetails.Any())
        //    {
        //        return NotFound();
        //    }
        //    return Ok(foodDetails);
        //}

        [HttpPost("create")]
        public IActionResult AddFooddetail(Fooddetail foodetail)
        {
            var newFoodetail = _foodetailRepository.addFooddetail(foodetail);
            return Ok(newFoodetail);
        }
        [HttpPut("update")]
        public IActionResult UpdateFooddetail([FromBody] Fooddetail fooddetail)
        {
            var updateFooddetail = _foodetailRepository.AddOrUpdateFooddetail(
                fooddetail.dailydiary_id,
                fooddetail.meal_type, 
                fooddetail.Food,
                fooddetail.food_amount
               

            );
            return Ok(updateFooddetail);
        }
        [HttpDelete("delete")]
        public IActionResult DeleteFooddetail([FromBody] Fooddetail fooddetail)
        {

            var deleteFood = _foodetailRepository.DeleteFoodFromFooddetail(
                fooddetail.dailydiary_id,
                fooddetail.meal_type,
                fooddetail.Food


            );

            return Ok();
        }

    }
}


