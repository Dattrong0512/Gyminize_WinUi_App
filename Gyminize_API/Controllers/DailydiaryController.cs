using Gyminize_API.Data.Model;
using Gyminize_API.Data.Models;
using Gyminize_API.Data.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
namespace Gyminize_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailydiaryController : ControllerBase
    {
        private readonly DailydiaryRepository _dailyDiaryRepository;

        public DailydiaryController(DailydiaryRepository dailyDiaryRepository)
        {
            _dailyDiaryRepository = dailyDiaryRepository;
        }

        [HttpGet]
        public IActionResult GetAllDailyDiary()
        {
            try
            {
                var allDailyDiary = _dailyDiaryRepository.GetAllDailydiary();
                return Ok(allDailyDiary);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("get/daily_customer/{customer_id:int}/day/{day}")]
        public IActionResult GetDailydiaryByIdCustomer(int customer_id, DateTime day)
        {
            var dailyDiary = _dailyDiaryRepository.GetDailydiaryByIdCustomer(customer_id, day);
            if (dailyDiary == null)
            {
                return NotFound();
            }
            return Ok(dailyDiary);
        }
        [HttpPost("create")]
        public IActionResult AddDailyDiary(Dailydiary dailyDiary)
        {
            var newDailyDiary = _dailyDiaryRepository.addDailydiary(dailyDiary);
            return Ok(newDailyDiary);
        }
    }
}


