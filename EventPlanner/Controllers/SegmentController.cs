using System.Reflection;
using EventPlanner.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SegmentController : ControllerBase
    {
        // GET: api/<SegmentController>
        [HttpGet]
        public IEnumerable<Segment> Get()
        {
            return DatabaseManager.Instance.ReadAllSegments();
        }

        // GET api/<SegmentController>/5
        [HttpGet("{segmentId}")]
        public ActionResult<Segment> GetId(string segmentId)
        {
            if (!IsGuid(segmentId))
                return BadRequest("Invalid ID format.");

            var segment = DatabaseManager.Instance.RequestSegmentByIdAsync(segmentId).Result;
            if (segment == null)
                return NotFound();

            return segment;
        }

        // POST api/<SegmentController>
        [HttpPost]
        public IActionResult Post([FromBody] Segment segment)
        {
            if (segment == null)
                return BadRequest();

            if (string.IsNullOrWhiteSpace(segment.id))
                segment.id = Guid.NewGuid().ToString();

            DatabaseManager.Instance.AddNewSegmentAsync(segment);
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

            DatabaseManager.Instance.BoundParticipantToSegmentAsync(segmentId, userId);

            // Return a success response
            return Ok(new { Message = "Segment updated successfully", Value = DatabaseManager.Instance.RequestParticipantByIdAsync(userId) });
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

            DatabaseManager.Instance.BoundParticipantToSegmentPositionAsync(segmentId, userId, 1);

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

            DatabaseManager.Instance.BoundParticipantToSegmentPositionAsync(segmentId, userId, 2);

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

            DatabaseManager.Instance.BoundParticipantToSegmentPositionAsync(segmentId, userId, 3);

            // Return a success response
            return Ok(new { Message = "Segment updated successfully", Value = userId });
        }

        [HttpPut("{segmentId}")]
        public IActionResult Put(string segmentId, [FromBody] Segment newValues)
        {
            Console.WriteLine(JsonConvert.SerializeObject(newValues));

            if (string.IsNullOrWhiteSpace(segmentId) || newValues == null)
                return BadRequest("ID and updated segment data must be provided.");

            if (!IsGuid(segmentId))
                return BadRequest("Invalid ID format.");

            var olderValues = DatabaseManager.Instance.RequestSegmentByIdAsync(segmentId).Result;
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
                                olderValues.firstPlace = JsonConvert.DeserializeObject<Participant>(difference.Value);

                            break;
                        case "secondPlace":
                            if (!string.IsNullOrEmpty(difference.Value))
                                olderValues.secondPlace = JsonConvert.DeserializeObject<Participant>(difference.Value);

                            break;
                        case "thirdPlace":
                            if (!string.IsNullOrEmpty(difference.Value))
                                olderValues.thirdPlace = JsonConvert.DeserializeObject<Participant>(difference.Value);

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

            DatabaseManager.Instance.UpdateSegment(olderValues);

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

            DatabaseManager.Instance.DeleteSegmentAsync(segmentId);
            return Ok();
        }

        // DELETE api/<SegmentController>/5
        [HttpDelete("{segmentId}/{participantId}")]
        public void Delete(string segmentId, string participantId) =>
            DatabaseManager.Instance.RemoveBoundedParticipantFromSegmentAsync(segmentId, participantId);

        // DELETE api/<SegmentController>/5
        [HttpDelete("{segmentId}/first")]
        public async void DeleteFirstPlace(string segmentId)
        {
            var segment = await DatabaseManager.Instance.RequestSegmentByIdAsync(segmentId);
            await DatabaseManager.Instance.RemoveBoundedParticipantFromSegmentPositionAsync(segmentId,
                segment.firstPlace.id,
                1);
        }

        // DELETE api/<SegmentController>/5
        [HttpDelete("{segmentId}/second")]
        public async void DeleteSecondPlace(string segmentId)
        {
            var segment = await DatabaseManager.Instance.RequestSegmentByIdAsync(segmentId);
            await DatabaseManager.Instance.RemoveBoundedParticipantFromSegmentPositionAsync(segmentId,
                segment.secondPlace.id,
                2);
        }

        // DELETE api/<SegmentController>/5
        [HttpDelete("{segmentId}/third")]
        public async void DeleteThirdPlace(string segmentId)
        {
            var segment = await DatabaseManager.Instance.RequestSegmentByIdAsync(segmentId);
            await DatabaseManager.Instance.RemoveBoundedParticipantFromSegmentPositionAsync(segmentId,
                segment.thirdPlace.id,
                3);
        }

        [NonAction]
        public bool IsGuid(string value)
        {
            return Guid.TryParse(value, out _);
        }

        [NonAction]
        private Dictionary<string, string> CheckForDifferences(Segment s1, Segment s2)
        {
            var differences = new Dictionary<string, string>();
            Type type = typeof(Segment);

            foreach (PropertyInfo property in type.GetProperties())
            {
                object value1 = property.GetValue(s1);
                object value2 = property.GetValue(s2);

                if (value1 is Participant || value1 is List<Participant>)
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

            Console.WriteLine("Detected differences:");
            foreach (string key in differences.Keys)
            {
                Console.WriteLine($"[{key} : {differences[key]}]");
            }

            return differences;
        }
    }
}