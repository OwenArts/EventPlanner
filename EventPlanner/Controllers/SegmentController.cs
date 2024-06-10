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
    public class SegmentController : ControllerBase
    {
        private readonly IDatabaseManager _dbManager;

        public SegmentController(IDatabaseManager dbManager)
        {
            _dbManager = dbManager;
        }

        // GET: api/<SegmentController>
        [HttpGet]
        public IEnumerable<Segment> Get()
        {
            return _dbManager.ReadAllSegments().Result;
        }

        // GET api/<SegmentController>/5
        [HttpGet("{segmentId}")]
        public ActionResult<Segment> GetId(string segmentId)
        {
            if (!IsGuid(segmentId))
                return BadRequest("Invalid ID format.");

            var segment = _dbManager.RequestSegmentByIdAsync(segmentId).Result;
            if (segment == null)
                return NotFound();

            return Ok(segment);
        }

        // POST api/<SegmentController>
        [HttpPost]
        public IActionResult Post([FromBody] DataSegment segment)
        {
            if (segment == null) 
                return BadRequest("Body is empty.");

            if (string.IsNullOrWhiteSpace(segment.id))
                segment.id = Guid.NewGuid().ToString();

            _dbManager.AddNewSegmentAsync(segment);
            
            return Ok(segment);
        }


        // POST api/<SegmentController>
        [HttpPost("{segmentId}")]
        public IActionResult PostByUserId(string segmentId, [FromBody] string userId)
        {
            if (string.IsNullOrEmpty(segmentId))
                return BadRequest("id is required.");
            if (userId == null)
                return BadRequest("Value is required.");
            if (!IsGuid(segmentId))
                return BadRequest("Invalid Segment ID format.");
            if (!IsGuid(userId))
                return BadRequest("Invalid User ID format.");

            _dbManager.BoundParticipantToSegmentAsync(segmentId, userId);

            // Return a success response
            return Ok(new { Message = "Segment updated successfully", Value = _dbManager.RequestParticipantByIdAsync(userId) });
        }

        // POST api/<SegmentController>
        [HttpPost("{segmentId}/first")]
        public IActionResult PostFirstUserId(string segmentId, [FromBody] string userId)
        {
            if (string.IsNullOrEmpty(segmentId))
                return BadRequest("id is required.");
            if (userId == null)
                return BadRequest("Value is required.");
            if (!IsGuid(segmentId))
                return BadRequest("Invalid Segment ID format.");
            if (!IsGuid(userId))
                return BadRequest("Invalid User ID format.");

            _dbManager.BoundParticipantToSegmentPositionAsync(segmentId, userId, 1);

            // Return a success response
            return Ok(new { Message = "Segment updated successfully", Value = userId });
        }

        // POST api/<SegmentController>
        [HttpPost("{segmentId}/second")]
        public IActionResult PostSecondUserId(string segmentId, [FromBody] string userId)
        {
            if (string.IsNullOrEmpty(segmentId))
                return BadRequest("id is required.");
            if (userId == null)
                return BadRequest("Value is required.");
            if (!IsGuid(segmentId))
                return BadRequest("Invalid Segment ID format.");
            if (!IsGuid(userId))
                return BadRequest("Invalid User ID format.");

            _dbManager.BoundParticipantToSegmentPositionAsync(segmentId, userId, 2);

            // Return a success response
            return Ok(new { Message = "Segment updated successfully", Value = userId });
        }

        // POST api/<SegmentController>
        [HttpPost("{segmentId}/third")]
        public IActionResult PostThirdUserId(string segmentId, [FromBody] string userId)
        {
            if (string.IsNullOrEmpty(segmentId))
                return BadRequest("id is required.");
            if (userId == null)
                return BadRequest("Value is required.");
            if (!IsGuid(segmentId))
                return BadRequest("Invalid Segment ID format.");
            if (!IsGuid(userId))
                return BadRequest("Invalid User ID format.");

            _dbManager.BoundParticipantToSegmentPositionAsync(segmentId, userId, 3);

            // Return a success response
            return Ok(new { Message = "Segment updated successfully", Value = userId });
        }

        [HttpPut("{segmentId}")]
        public IActionResult Put(string segmentId, [FromBody] DataSegment newValues)
        {
            if (string.IsNullOrWhiteSpace(segmentId) || newValues == null)
                return BadRequest("ID and updated segment data must be provided.");

            if (!IsGuid(segmentId))
                return BadRequest("Invalid ID format.");

            var olderValues = (DataSegment) _dbManager.RequestSegmentByIdAsync(segmentId).Result;
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
                        case "contestants":
                            if (!string.IsNullOrEmpty(difference.Value))
                                olderValues.contestants = JsonConvert.DeserializeObject<List<Participant>>(difference.Value);

                            break;
                        case "firstPlace":
                            if (!string.IsNullOrEmpty(difference.Value))
                                olderValues.firstPlace = JsonConvert.DeserializeObject<DataParticipant>(difference.Value);

                            break;
                        case "secondPlace":
                            if (!string.IsNullOrEmpty(difference.Value))
                                olderValues.secondPlace = JsonConvert.DeserializeObject<DataParticipant>(difference.Value);

                            break;
                        case "thirdPlace":
                            if (!string.IsNullOrEmpty(difference.Value))
                                olderValues.thirdPlace = JsonConvert.DeserializeObject<DataParticipant>(difference.Value);

                            break;
                        case "name":
                            if (!string.IsNullOrEmpty(difference.Value))
                                olderValues.name = difference.Value;

                            break;
                        case "duration":
                            if (int.TryParse(difference.Value, out int newDuration))
                                olderValues.duration = newDuration;

                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing difference: {ex}");
                    return BadRequest(ex.Message);
                }
            }

            _dbManager.UpdateSegment(olderValues);

            return NoContent();
        }

        // DELETE api/<SegmentController>/5
        [HttpDelete("{segmentId}")]
        public IActionResult Delete(string segmentId)
        {
            if (string.IsNullOrEmpty(segmentId))
                return BadRequest("Id is required.");
            if (!IsGuid(segmentId))
                return BadRequest("Invalid ID format.");

            _dbManager.DeleteSegmentAsync(segmentId);
            return NoContent();
        }

        // DELETE api/<SegmentController>/5
        [HttpDelete("{segmentId}/{participantId}")]
        public IActionResult Delete(string segmentId, string participantId)
        {
            
            if (!IsGuid(segmentId))
                return BadRequest("Invalid Segment ID format.");
         
            if (!IsGuid(participantId))
                return BadRequest("Invalid Participant ID format povided");
            
            _dbManager.RemoveBoundedParticipantFromSegmentAsync(segmentId, participantId);
            return NoContent();
        }

        // DELETE api/<SegmentController>/5
        [HttpDelete("{segmentId}/first")]
        public async void DeleteFirstPlace(string segmentId)
        {
            var segment = await _dbManager.RequestSegmentByIdAsync(segmentId);
            await _dbManager.RemoveBoundedParticipantFromSegmentPositionAsync(segmentId,
                segment.firstPlace.id,
                1);
        }

        // DELETE api/<SegmentController>/5
        [HttpDelete("{segmentId}/second")]
        public async void DeleteSecondPlace(string segmentId)
        {
            var segment = await _dbManager.RequestSegmentByIdAsync(segmentId);
            await _dbManager.RemoveBoundedParticipantFromSegmentPositionAsync(segmentId,
                segment.secondPlace.id,
                2);
        }

        // DELETE api/<SegmentController>/5
        [HttpDelete("{segmentId}/third")]
        public async void DeleteThirdPlace(string segmentId)
        {
            var segment = await _dbManager.RequestSegmentByIdAsync(segmentId);
            await _dbManager.RemoveBoundedParticipantFromSegmentPositionAsync(segmentId,
                segment.thirdPlace.id,
                3);
        }

        [NonAction]
        public bool IsGuid(string value)
        {
            return Guid.TryParse(value, out _);
        }

        [NonAction]
        private Dictionary<string, string> CheckForDifferences(DataSegment s1, DataSegment s2)
        {
            var differences = new Dictionary<string, string>();
            Type type = typeof(DataSegment);

            foreach (PropertyInfo property in type.GetProperties())
            {
                object value1 = property.GetValue(s1);
                object value2 = property.GetValue(s2);

                if (value1 is DataParticipant || value1 is List<DataParticipant>)
                {
                    string json1 = JsonConvert.SerializeObject(value1);
                    string json2 = JsonConvert.SerializeObject(value2);
                    if (!json1.Equals(json2))
                    {
                        differences[property.Name] = json2;
                    }
                }
                else if (!Equals(value1, value2))
                {
                    differences[property.Name] = JsonConvert.SerializeObject(value2);
                }
            }

            return differences;
        }
    }
}