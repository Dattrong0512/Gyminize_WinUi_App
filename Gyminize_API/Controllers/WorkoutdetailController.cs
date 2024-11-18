using Gyminize_API.Data.Models;
using Gyminize_API.Data.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace Gyminize_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutdetailController : ControllerBase
    {
        private readonly WorkoutdetailRepository _workoutdetailRepository;

        public WorkoutdetailController(WorkoutdetailRepository workoutdetailRepository)
        {
            _workoutdetailRepository = workoutdetailRepository;
        }
        [HttpPut("update/{workoutId:int}/decription/{decription}")]
        public IActionResult UpdateDecriptionWorkoutDetail(int workoutId, string decription)
        {
            var workoutDetail = _workoutdetailRepository.UpdateDecription(workoutId, decription);
            return Ok(workoutDetail);
        }
    }
}


