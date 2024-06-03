using EventPlanner.Data;
using EventPlanner.Managers;
using Microsoft.AspNetCore.Mvc;

namespace EventPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlannerController : ControllerBase
    {
        [HttpGet("validate/{festivalId}")]
        public IActionResult ValidateFestival(string festivalId)
        {
            Console.WriteLine(0);

            try
            {
                Festival festival = DatabaseManager.Instance.RequestFestivalByIdAsync(festivalId).Result;
                PlannerManager.Instance.ValidateFestival(festival);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok("Festival successfully validated");
        }
    }
}