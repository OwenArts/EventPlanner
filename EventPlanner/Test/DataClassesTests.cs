using EventPlanner.Controllers;
using EventPlanner.Data.DataClasses;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using EventPlanner.Data.TimetableClasses;

namespace EventPlanner.Test
{
    [TestFixture]
    public class DataClassesTests
    {
        [Test]
        public void AddingIdToDataParticipant()
        {
            var tempParticipant = new DataParticipant("John", "Doe", "John.Doe@Email.com");
            Assert.That(tempParticipant.id != null && tempParticipant.id.Length > 0 && IsGuid(tempParticipant.id));
            
            tempParticipant = new DataParticipant("John", "Doe", "John.Doe@Email.com", "");
            Assert.That(tempParticipant.id != null && tempParticipant.id.Length > 0 && IsGuid(tempParticipant.id));
        }

        [Test]
        public void AddingIdToDataFestival()
        {
            var tempFestival = new DataFestival("Festival Name", DateTime.Now, DateTime.Now.AddDays(1));
            Assert.That(tempFestival.id != null && tempFestival.id.Length > 0 && IsGuid(tempFestival.id));
            
            tempFestival = new DataFestival("Festival Name", DateTime.Now, DateTime.Now.AddDays(1), "");
            Assert.That(tempFestival.id != null && tempFestival.id.Length > 0 && IsGuid(tempFestival.id));
        }

        [Test]
        public void AddingIdToDataRoom()
        {
            var tempRoom = new DataRoom("Room Name", DateTime.Now, DateTime.Now.AddHours(5));
            Assert.That(tempRoom.id != null && tempRoom.id.Length > 0 && IsGuid(tempRoom.id));
            
            tempRoom = new DataRoom("Room Name", DateTime.Now, DateTime.Now.AddHours(5), "");
            Assert.That(tempRoom.id != null && tempRoom.id.Length > 0 && IsGuid(tempRoom.id));
        }

        [Test]
        public void AddingIdToDataSegment()
        {
            var tempSegment = new DataSegment("Segment Name", 60);
            Assert.That(tempSegment.id != null && tempSegment.id.Length > 0 && IsGuid(tempSegment.id));
            
            tempSegment = new DataSegment("Segment Name", 60, "");
            Assert.That(tempSegment.id != null && tempSegment.id.Length > 0 && IsGuid(tempSegment.id));
        }

        [Test]
        public void AddingIdToPlannerFestival()
        {
            var tempFestival = new PlannerFestival("Festival Name", DateTime.Now, DateTime.Now.AddDays(1));
            Assert.That(tempFestival.id != null && tempFestival.id.Length > 0 && IsGuid(tempFestival.id));
            
            tempFestival = new PlannerFestival("Festival Name", DateTime.Now, DateTime.Now.AddDays(1), "");
            Assert.That(tempFestival.id != null && tempFestival.id.Length > 0 && IsGuid(tempFestival.id));
        }

        [Test]
        public void AddingIdToPlannerParticipant()
        {
            var tempParticipant = new PlannerParticipant(DateTime.Now, DateTime.Now.AddHours(1), "John", "Doe", "John.Doe@Email.com");
            Assert.That(tempParticipant.id != null && tempParticipant.id.Length > 0 && IsGuid(tempParticipant.id));
            
            tempParticipant = new PlannerParticipant(DateTime.Now, DateTime.Now.AddHours(1), "John", "Doe", "John.Doe@Email.com", "");
            Assert.That(tempParticipant.id != null && tempParticipant.id.Length > 0 && IsGuid(tempParticipant.id));
        }

        [Test]
        public void AddingIdToPlannerRoom()
        {
            var tempRoom = new PlannerRoom("Room Name", DateTime.Now, DateTime.Now.AddHours(5));
            Assert.That(tempRoom.id != null && tempRoom.id.Length > 0 && IsGuid(tempRoom.id));
            
            tempRoom = new PlannerRoom("Room Name", DateTime.Now, DateTime.Now.AddHours(5), "");
            Assert.That(tempRoom.id != null && tempRoom.id.Length > 0 && IsGuid(tempRoom.id));
        }

        [Test]
        public void AddingIdToPlannerSegment()
        {
            var tempSegment = new PlannerSegment(DateTime.Now, "Segment Name", 60);
            Assert.That(tempSegment.id != null && tempSegment.id.Length > 0 && IsGuid(tempSegment.id));
            
            tempSegment = new PlannerSegment(DateTime.Now, "Segment Name", 60, "");
            Assert.That(tempSegment.id != null && tempSegment.id.Length > 0 && IsGuid(tempSegment.id));
        }
        
        public bool IsGuid(string value)
        {
            return Guid.TryParse(value, out _);
        }

        /* Example for how to test APIs
        public class MyApiControllerTests
        {
            [Fact]
            public void Get_ReturnsOkResult_WithAListOfItems()
            {
                // Arrange
                var mockService = new Mock<IMyService>();
                mockService.Setup(service => service.GetAll()).Returns(GetTestItems());
                var controller = new MyApiController(mockService.Object);

                // Act
                var result = controller.Get();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var items = Assert.IsType<List<Item>>(okResult.Value);
                Assert.Equal(3, items.Count);
            }

            private List<Item> GetTestItems()
            {
                return new List<Item>
                {
                    new Item {},
                    new Item {},
                    new Item {}
                };
            }
        }
        */
    }
}
