using EventPlanner.Data;
using EventPlanner.Data.AbstractClasses;
using EventPlanner.Data.DataClasses;
using EventPlanner.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;

namespace EventPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlannerController : ControllerBase
    {
        private readonly IDatabaseManager _dbManager;

        public PlannerController(IDatabaseManager dbManager)
        {
            _dbManager = dbManager;
        }

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
        
        [HttpGet("plan/{festivalId}")]
        public IActionResult planFestival(string festivalId)
        {
            DataFestival? festival = null;
            Festival? plannedFestival = null;
            try
            {
                festival = DatabaseManager.Instance.RequestFestivalByIdAsync(festivalId).Result;
                if (festival == null)
                    return BadRequest("Festival ID not found.");
                plannedFestival = PlannerManager.Instance.PlanFestival(festival);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(/*new { Message = $"Festival \"{festival.name}\" successfully planned", Value =*/ JsonConvert.SerializeObject(plannedFestival) /*}*/);
        }
    }
}