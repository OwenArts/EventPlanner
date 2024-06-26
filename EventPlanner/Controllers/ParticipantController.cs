﻿using EventPlanner.Data.AbstractClasses;
using System.Net;
using System.Reflection;
using EventPlanner.Data;
using EventPlanner.Data.DataClasses;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EventPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantController : ControllerBase
    {
        private readonly IDatabaseManager _dbManager;

        public ParticipantController(IDatabaseManager dbManager)
        {
            _dbManager = dbManager;
        }

        // Get all participants
        [HttpGet]
        public IEnumerable<Participant> Get()
        {
            return _dbManager.ReadAllParticipants().Result;
        } 
        
        // Get participant by ID
        [HttpGet("id/{userId}")]
        public async Task<ActionResult<Participant>> GetId(string userId)
        {
            if (!IsGuid(userId))
                return BadRequest("Invalid ID format.");

            var participant = await _dbManager.RequestParticipantByIdAsync(userId);
            if (participant == null)
                return NotFound();

            return Ok(participant);
        }


        // Get participant by email
        [HttpGet("email/{email}")]
        public ActionResult<Participant> GetEmail(string email)
        {
            if (!IsValidEmail(email))
                return BadRequest("Invalid email format.");

            var participant = _dbManager.RequestParticipantByEmailAsync(email).Result;
            if (participant == null)
                return NotFound();

            return Ok(participant);
        }

        // Create a new participant
        [HttpPost]
        public IActionResult Post([FromBody] DataParticipant participant)
        {
            if (participant == null)
                return BadRequest("Body is empty.");

            if (string.IsNullOrWhiteSpace(participant.id))
                participant.id = Guid.NewGuid().ToString();
            if (string.IsNullOrEmpty(participant.id))
                participant.id = Guid.NewGuid().ToString();

            _dbManager.AddNewParticipantAsync(participant);
            return Ok(participant);
        }

        // Update participant by ID
        [HttpPut("id/{userId}")]
        public IActionResult PutId(string userId, [FromBody] DataParticipant newValues)
        {
            if (string.IsNullOrWhiteSpace(userId) || newValues == null)
                return BadRequest("ID and updated participant data must be provided.");

            if (!IsGuid(userId))
                return BadRequest("Invalid ID format.");

            var olderValues = (DataParticipant)_dbManager.RequestParticipantByIdAsync(userId).Result;
            if (olderValues == null)
                return NotFound();

            var differences = CheckForDifferences(olderValues, newValues);

            foreach (var difference in differences)
            {
                try
                {
                    switch (difference.Key)
                    {
                        case ("id"):
                            break;
                        case ("email"):
                            olderValues.email = difference.Value;
                            break;
                        case ("firstName"):
                            olderValues.firstName = difference.Value;
                            break;
                        case ("middleName"):
                            olderValues.middleName = difference.Value;
                            break;
                        case ("lastName"):
                            olderValues.lastName = difference.Value;
                            break;
                        case ("birthDay"):
                            olderValues.birthDay = DateTime.Parse(difference.Value);
                            break;
                        case ("phoneNUmber"):
                            olderValues.phoneNumber = difference.Value;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return BadRequest(ex.Message);
                }
            }

            _dbManager.UpdateParticipant(olderValues);

            return NoContent();
        }

        // Update participant by email
        [HttpPut("email/{email}")]
        public IActionResult PutEmail(string email, [FromBody] DataParticipant newValues)
        {
            if (string.IsNullOrWhiteSpace(email) || newValues == null)
                return BadRequest("Email and updated participant data must be provided.");

            if (!IsValidEmail(email))
                return BadRequest("Invalid email format.");

            var olderValues = (DataParticipant)_dbManager.RequestParticipantByEmailAsync(email).Result;
            if (olderValues == null)
                return NotFound();

            var differences = CheckForDifferences(olderValues, newValues);
            // Apply logic to update the participant with new values

            foreach (var difference in differences)
            {
                try
                {
                    switch (difference.Key)
                    {
                        case ("id"):
                        case ("email"):
                            break;
                        case ("firstName"):
                            olderValues.firstName = difference.Value;
                            break;
                        case ("middleName"):
                            olderValues.middleName = difference.Value;
                            break;
                        case ("lastName"):
                            olderValues.lastName = difference.Value;
                            break;
                        case ("birthDay"):
                            olderValues.birthDay = DateTime.Parse(difference.Value);
                            break;
                        case ("phoneNUmber"):
                            olderValues.phoneNumber = difference.Value;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return BadRequest(ex.Message);
                }
            }

            _dbManager.UpdateParticipant(olderValues);

            return NoContent();
        }

        // Delete participant by email
        [HttpDelete("email/{email}")]
        public IActionResult DeleteEmail(string email)
        {
            if (!IsValidEmail(email))
                return BadRequest("Invalid email format.");

            _dbManager.DeleteParticipantAsync(email, null);
            return NoContent();
        }

        // Delete participant by ID
        [HttpDelete("id/{id}")]
        public IActionResult DeleteId(string id)
        {
            if (!IsGuid(id))
                return BadRequest("Invalid ID format.");

            _dbManager.DeleteParticipantAsync(null, id);
            return NoContent();
        }

        [NonAction]
        public bool IsGuid(string value)
        {
            return Guid.TryParse(value, out _);
        }

        [NonAction]
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        [NonAction]
        private Dictionary<string, string> CheckForDifferences(DataParticipant p1, DataParticipant p2)
        {
            var differences = new Dictionary<string, string>();
            Type type = typeof(Participant);

            foreach (PropertyInfo property in type.GetProperties())
            {
                object value1 = property.GetValue(p1);
                object value2 = property.GetValue(p2);

                if (!Equals(value1, value2))
                    differences[property.Name] = value2?.ToString();
            }

            return differences;
        }
    }
}