using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;
using EventPlanner.Data;
using EventPlanner.Data.AbstractClasses;
using EventPlanner.Data.DataClasses;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IDatabaseManager _dbManager;

        public RoomController(IDatabaseManager dbManager)
        {
            _dbManager = dbManager;
        }

        // GET: api/<RoomController>
        [HttpGet]
        public IEnumerable<Room> Get() => _dbManager.ReadAllRooms().Result;

        // GET api/<RoomController>/5
        [HttpGet("{roomId}")]
        public Room Get(string roomId) => _dbManager.RequestRoomByIdAsync(roomId).Result;

        // POST api/<RoomController>
        [HttpPost]
        public IActionResult Post([FromBody] DataRoom room)
        {
            if (room == null)
                return BadRequest("Body is empty.");

            if (!string.IsNullOrWhiteSpace(room.id))
                room.id = Guid.NewGuid().ToString();

            // You might want to add the participant to your database or list here
            _dbManager.AddNewRoomAsync(room);

            // For simplicity, we will just return the participant as a response
            return Ok(room);
        }

        // POST api/<RoomController>
        [HttpPost("{roomId}")]
        public IActionResult Post(string roomId, [FromBody] string segmentId)
        {
            if (string.IsNullOrEmpty(roomId))
            {
                return BadRequest("id required");
            }
            if (string.IsNullOrEmpty(segmentId))
            {
                return BadRequest("value required");
            }

            // You might want to add the participant to your database or list here
            _dbManager.BoundSegmentToRoom(roomId, segmentId);
            // For simplicity, we will just return the participant as a response
            return Ok(_dbManager.RequestSegmentByIdAsync(segmentId));
        }

        // PUT api/<RoomController>/5
        [HttpPut("{roomId}")]
        public IActionResult Put(string roomId, [FromBody] DataRoom newValues)
        {
            if (string.IsNullOrWhiteSpace(roomId) || newValues == null)
                return BadRequest("ID and updated segment data must be provided.");

            if (!IsGuid(roomId))
                return BadRequest("Invalid ID format.");

            var olderValues = (DataRoom) _dbManager.RequestRoomByIdAsync(roomId).Result;
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
                        case "timeOpen":
                            if (!string.IsNullOrEmpty(difference.Value))
                                olderValues.timeOpen = DateTime.Parse(difference.Value.Replace("\"", ""));

                            break;
                        case "timeClose":
                            if (!string.IsNullOrEmpty(difference.Value))
                                olderValues.timeClose = DateTime.Parse(difference.Value.Replace("\"", ""));

                            break;
                        case "segments":
                            if (!string.IsNullOrEmpty(difference.Value))
                            {
                                try
                                {
                                    olderValues.segments = JsonConvert.DeserializeObject<List<Segment>>(difference.Value);
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
            
            _dbManager.UpdateRoom(olderValues);

            return NoContent();
        }

        // DELETE api/<RoomController>/5
        [HttpDelete("{roomId}")]
        public IActionResult Delete(string roomId)
        {
            if (!IsGuid(roomId))
                return BadRequest("Invalid ID format.");
            
            _dbManager.DeleteRoomAsync(roomId);
            
            return NoContent();
        }

        // DELETE api/<RoomController>/5
        [HttpDelete("{roomId}/{segmentId}")]
        public IActionResult Delete(string roomId, string segmentId)
        {
            if (!IsGuid(roomId))
                return BadRequest("Invalid Room ID format.");
         
            if (!IsGuid(segmentId))
                return BadRequest("Invalid Segment ID format. Id povided");
            
            _dbManager.RemoveSegmentFromRoom(roomId, segmentId);
            return NoContent();
        }

        [NonAction]
        public bool IsGuid(string value)
        {
            return Guid.TryParse(value, out _);
        }

        [NonAction]
        private Dictionary<string, string> CheckForDifferences(DataRoom r1, DataRoom r2)
        {
            var differences = new Dictionary<string, string>();
            Type type = typeof(Room);

            foreach (PropertyInfo property in type.GetProperties())
            {
                object value1 = property.GetValue(r1);
                object value2 = property.GetValue(r2);

                if (!Equals(value1, value2))
                    differences[property.Name] = JsonConvert.SerializeObject(value2);
            }
            return differences;
        }
    }
}
