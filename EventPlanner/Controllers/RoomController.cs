﻿using EventPlanner.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private List<Room> Rooms = new List<Room>();
        /*{
            new Participant("John", "Gatehouse", "JohnGatehous@teleworm.us", null, null, new DateTime(1970, 4, 6), "0654548403"),
            new Participant("Emma", "Johnson", "emma.johnson@example.com", null, "Lynn", new DateTime(1985, 5, 15), "0654548412"),
            new Participant("Liam", "Smith", "liam.smith@example.com", null, "James", new DateTime(1990, 11, 23), "0654548421"),
            new Participant("Olivia", "Brown", "olivia.brown@example.com", null, "Marie", new DateTime(1995, 2, 17), "0654548430"),
            new Participant("Noah", "Jones", "noah.jones@example.com", null, "Alexander", new DateTime(1982, 8, 30), "0654548449"),
            new Participant("Ava", "Garcia", "ava.garcia@example.com", null, "Grace", new DateTime(1978, 3, 9), "0654548458"),
            new Participant("Elijah", "Martinez", "elijah.martinez@example.com", null, null, new DateTime(1987, 7, 21), "0654548467"),
            new Participant("Isabella", "Rodriguez", "isabella.rodriguez@example.com", null, "Rose", new DateTime(1992, 10, 5), "0654548476"),
            new Participant("James", "Wilson", "james.wilson@example.com", null, "Edward", new DateTime(1975, 6, 13), "0654548485"),
            new Participant("Sophia", "Lopez", "sophia.lopez@example.com", null, null, new DateTime(1980, 1, 29), "0654548494"),
            new Participant("Benjamin", "Gonzalez", "benjamin.gonzalez@example.com", null, "David", new DateTime(1989, 4, 22), "0654548501"),
            new Participant("Mia", "Perez", "mia.perez@example.com", null, "Ann", new DateTime(1976, 12, 3), "0654548510"),
            new Participant("Lucas", "White", "lucas.white@example.com", null, "Michael", new DateTime(1991, 9, 18), "0654548529"),
            new Participant("Charlotte", "Harris", "charlotte.harris@example.com", null, "Jane", new DateTime(1984, 5, 27), "0654548538"),
            new Participant("Henry", "Clark", "henry.clark@example.com", null, "Thomas", new DateTime(1973, 8, 7), "0654548547"),
            new Participant("Amelia", "Lewis", "amelia.lewis@example.com", null, "Elaine", new DateTime(1993, 6, 25), "0654548556"),
            new Participant("Alexander", "Robinson", "alexander.robinson@example.com", null, "John", new DateTime(1986, 2, 14), "0654548565"),
            new Participant("Evelyn", "Walker", "evelyn.walker@example.com", null, "Faye", new DateTime(1979, 11, 30), "0654548574"),
            new Participant("Mason", "Young", "mason.young@example.com", null, "Anthony", new DateTime(1994, 3, 6), "0654548583"),
            new Participant("Harper", "Allen", "harper.allen@example.com", null, "Louise", new DateTime(1977, 7, 11), "0654548592"),
            new Participant("Ethan", "King", "ethan.king@example.com", null, "Samuel", new DateTime(1992, 1, 19), "0654548609"),
            new Participant("Abigail", "Scott", "abigail.scott@example.com", null, "Beth", new DateTime(1983, 12, 22), "0654548618"),
            new Participant("Logan", "Adams", "logan.adams@example.com", null, "Henry", new DateTime(1991, 8, 3), "0654548627"),
            new Participant("Avery", "Baker", "avery.baker@example.com", null, "Irene", new DateTime(1988, 5, 29), "0654548636"),
            new Participant("Daniel", "Hill", "daniel.hill@example.com", null, "Joseph", new DateTime(1974, 2, 15), "0654548645"),
            new Participant("Scarlett", "Green", "scarlett.green@example.com", null, "Claire", new DateTime(1981, 10, 8), "0654548654"),
            new Participant("Matthew", "Nelson", "matthew.nelson@example.com", null, "Andrew", new DateTime(1986, 4, 19), "0654548663"),
            new Participant("Sofia", "Carter", "sofia.carter@example.com", null, "Renee", new DateTime(1972, 11, 4), "0654548672"),
            new Participant("Jackson", "Mitchell", "jackson.mitchell@example.com", null, "William", new DateTime(1989, 9, 16), "0654548681"),
            new Participant("Victoria", "Perez", "victoria.perez@example.com", null, "Mae", new DateTime(1995, 1, 28), "0654548690")
        };*/


        // GET: api/<RoomController>
        [HttpGet]
        public IEnumerable<Room> Get()
        {
                return Rooms != null && Rooms.Count > 0 ? Rooms : new List<Room>();
        }

        // GET api/<RoomController>/5
        [HttpGet("{RoomId}")]
        public Room Get(int RoomId)
        {
            if (RoomId>=Rooms.Count) return null;
            return Rooms[RoomId];
        }

        // POST api/<RoomController>
        [HttpPost]
        public IActionResult Post([FromBody] Room room)
        {
            if (room == null)
            {
                return BadRequest();
            }

            // You might want to add the participant to your database or list here
            Rooms.Add(room);
            Console.WriteLine(room + $", \r\nList is now filled with {Rooms.Count} entries.");

            // For simplicity, we will just return the participant as a response
            return Ok(room);
        }

        // POST api/<RoomController>
        [HttpPost("{RoomId}")]
        public IActionResult Post(string RoomId, [FromBody] Segment segment)
        {
            if (string.IsNullOrEmpty(RoomId))
            {
                return BadRequest("id required");
            }
            if (segment == null)
            {
                return BadRequest("value required");
            }

            // You might want to add the participant to your database or list here
            // Rooms.Add(segment);
            Console.WriteLine(segment);

            // For simplicity, we will just return the participant as a response
            return Ok(segment);
        }

        // PUT api/<RoomController>/5
        [HttpPut("{RoomId}")]
        public void Put(int RoomId, [FromBody] string value)
        {
        }

        // DELETE api/<RoomController>/5
        [HttpDelete("{RoomId}")]
        public void Delete(int RoomId)
        {
        }

        // DELETE api/<RoomController>/5
        [HttpDelete("{RoomId}/{SegmentId}")]
        public void Delete(int RoomId, int SegmentId)
        {
        }
    }
}
