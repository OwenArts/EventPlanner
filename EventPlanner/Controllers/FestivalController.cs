using EventPlanner.Data.AbstractClasses;
using System.Data;
using System.Reflection;
using EventPlanner.Data;
using EventPlanner.Data.DataClasses;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FestivalController : ControllerBase
    {
        
        private readonly IDatabaseManager _dbManager;

        public FestivalController(IDatabaseManager dbManager)
        {
            _dbManager = dbManager;
        }
        
        // GET: api/<FestivalController>
        [HttpGet]
        public async Task<IEnumerable<Festival>> Get() => _dbManager.ReadAllFestivals().Result;

        // GET api/<FestivalController>/5
        [HttpGet("{festivalId}")]
        public Festival Get(string festivalId) => _dbManager.RequestFestivalByIdAsync(festivalId).Result;
        
        [HttpPost]
        public IActionResult Post([FromBody] DataFestival festival)
        {
            if (festival == null)
                return BadRequest("Body is empty.");

            if (!string.IsNullOrWhiteSpace(festival.id))
                festival.id = Guid.NewGuid().ToString();

            _dbManager.AddNewFestivalAsync(festival);

            return Ok(festival);
        }
        
        // POST api/<FestivalController>
        [HttpPost("{festivalId}")]
        public async Task<IActionResult> Post(string festivalId, [FromBody] string roomId)
        {
            if (string.IsNullOrEmpty(festivalId))
                return BadRequest("id required");
    
            if (string.IsNullOrEmpty(roomId))
                return BadRequest("value required");

            await _dbManager.BoundRoomToFestival(festivalId, roomId);

            return Ok(await _dbManager.RequestRoomByIdAsync(roomId));
        }

        // PUT api/<RoomController>/5
        [HttpPut("{festivalId}")]
        public IActionResult Put(string festivalId, [FromBody] DataFestival newValues)
        {
            if (string.IsNullOrWhiteSpace(festivalId) || newValues == null)
                return BadRequest("ID and updated segment data must be provided.");

            if (!IsGuid(festivalId))
                return BadRequest("Invalid ID format.");

            var olderValues = _dbManager.RequestFestivalByIdAsync(festivalId).Result;
            if (olderValues == null)
                return NotFound();

            var differences = CheckForDifferences(olderValues, newValues);

            foreach (var difference in differences)
            {
                try
                {
                    switch (difference.Key)
                    {
                        case "id":
                            break;
                        case "name":
                            if (!string.IsNullOrEmpty(difference.Value))
                                olderValues.name = difference.Value.Replace("\"", "");

                            break;
                        case "startMoment":
                            if (!string.IsNullOrEmpty(difference.Value))
                                olderValues.startMoment = DateTime.Parse(difference.Value.Replace("\"", ""));

                            break;
                        case "endMoment":
                            if (!string.IsNullOrEmpty(difference.Value))
                                olderValues.endMoment = DateTime.Parse(difference.Value.Replace("\"", ""));

                            break;
                        case "rooms":
                            if (!string.IsNullOrEmpty(difference.Value))
                            {
                                try
                                {
                                    olderValues.rooms = JsonConvert.DeserializeObject<List<Room>>(difference.Value);
                                }
                                catch (JsonException ex)
                                {
                                    return BadRequest($"Error deserializing segments: {ex.Message}");
                                }
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing difference: {ex}");
                    return BadRequest(ex.Message);
                }
            }
            
            _dbManager.UpdateFestival(olderValues);

            return NoContent();
        }


        // DELETE api/<FestivalController>/5
        [HttpDelete("{festivalId}")]
        public IActionResult Delete(string festivalId)
        {
            if (!IsGuid(festivalId))
                return BadRequest("Invalid ID format.");
            
            _dbManager.DeleteFestivalAsync(festivalId);
            
            return NoContent();
        }

        // DELETE api/<FestivalController>/5
        [HttpDelete("{festivalId}/{roomId}")]
        public IActionResult Delete(string festivalId, string roomId)
        {
            if (!IsGuid(festivalId))
                return BadRequest("Invalid Festival ID format.");
         
            if (!IsGuid(roomId))
                return BadRequest("Invalid Room ID format povided");
            
            _dbManager.RemoveRoomFromFestival(festivalId, roomId);
            return NoContent();
        }

        [NonAction]
        public bool IsGuid(string value)
        {
            return Guid.TryParse(value, out _);
        }

        [NonAction]
        private Dictionary<string, string> CheckForDifferences(DataFestival f1, DataFestival f2)
        {
            var differences = new Dictionary<string, string>();
            Type type = typeof(Festival);

            foreach (PropertyInfo property in type.GetProperties())
            {
                object value1 = property.GetValue(f1);
                object value2 = property.GetValue(f2);

                if (!Equals(value1, value2))
                    differences[property.Name] = JsonConvert.SerializeObject(value2);
            }
            return differences;
        }
    }
}
