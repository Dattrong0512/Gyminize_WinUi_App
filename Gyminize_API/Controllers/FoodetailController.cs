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


