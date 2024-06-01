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
            return CreatedAtAction(nameof(GetId), new { userId = segment.id }, segment);
        }

        // POST api/<SegmentController>
        [HttpPost("{segmentId}")] //TODO: test this and place in festival and room if successful
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
            return Ok(new { Message = "Segment updated successfully", Value = userId });
        }

        // PUT api/<SegmentController>/5
        [HttpPut("{SegmentId}")]
        public void Put(int id, [FromBody] string value)
        {
            //todo (van belang dat bij het updaten de volgende "id" en "contestants" wordt overgeslagen in de switch-cas
        }

        // DELETE api/<SegmentController>/5
        [HttpDelete("{SegmentId}")]
        public void Delete(int SegmentId)
        {
        }

        // DELETE api/<SegmentController>/5
        [HttpDelete("{segmentId}/{participantId}")]
        public void Delete(string segmentId, string participantId) => DatabaseManager.Instance.RemoveBoundedParticipantFromSegmentAsync(segmentId, participantId);

        [NonAction]
        public bool IsGuid(string value)
        {
            return Guid.TryParse(value, out _);
        }
    }
}
