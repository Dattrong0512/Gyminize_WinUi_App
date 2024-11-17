using Gyminize_API.Data.Model;
using Gyminize_API.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Gyminize_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlandetailController : ControllerBase
    {
        private readonly PlandetailRepository _plandetailRepository;

        public PlandetailController(PlandetailRepository plandetailRepository)
        {
            _plandetailRepository = plandetailRepository;
        }

        [HttpGet("get/plandetail/{customerId:int}")] // Loại bỏ khoảng trắng
        public IActionResult GetPlandetailByCustomerId(int customerId)
        {
           
            var plandetail = _plandetailRepository.GetPlandetailById(customerId);
            if (plandetail == null)
            {
                return NotFound();
            }
            return Ok(plandetail);
        }
        [HttpPost("create/customer_id/{customerId:int}/plan/{plan:int}")]
        public IActionResult AddPlandetail(int customerId, int plan)
        {
            var newPlandetail = _plandetailRepository.AddPlandetail(customerId, plan);
            return Ok(newPlandetail);
        }

    }
}
