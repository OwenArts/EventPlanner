using EventPlanner.Data;
using EventPlanner.Data.DataClasses;
using EventPlanner.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

namespace EventPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlannerController : ControllerBase
    {
        [HttpGet("validate/{festivalId}")]
        public IActionResult ValidateFestival(string festivalId)
        {
            DataFestival? festival = null;
            try
            {
                festival = DatabaseManager.Instance.RequestFestivalByIdAsync(festivalId).Result;
                if (festival == null)
                    return BadRequest("Festival ID not found.");
                PlannerManager.Instance.ValidateFestival(festival);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok($"Festival \"{festival.name}\" successfully validated");
        }
    }
}