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

        [HttpGet("get/{dailydiary_id:int}")]
        public IActionResult GetDailyDiaryById(int dailydiary_id)
        {
            var dailyDiary = _dailyDiaryRepository.GetDailydiaryById(dailydiary_id);
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

        [HttpPut("update/{dailydiary_id:int}")]
        public IActionResult UpdateDailyDiary(int dailydiary_id, Dailydiary dailyDiary)
        {
            var updateDailyDiary = _dailyDiaryRepository.updateDailydiary(dailydiary_id, dailyDiary);
            return Ok(updateDailyDiary);
        }

        [HttpDelete("delete/{dailydiary_id:int}")]
        public IActionResult DeleteDailyDiary(int dailydiary_id)
        {
            var dailyDiary = _dailyDiaryRepository.GetDailydiaryById(dailydiary_id);
            if (dailyDiary == null)
            {
                return NotFound();
            }
            else
            {
                _dailyDiaryRepository.DeleteDailydiary(dailyDiary);
            }
            return Ok();
        }
    }
}


