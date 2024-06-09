using EventPlanner.Controllers;
using EventPlanner.Data;
using EventPlanner.Data.DataClasses;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace EventPlanner.Test
{
    [TestFixture]
    public class RoomControllerTests
    {
        private Mock<IDatabaseManager> _mockDbManager;
        private RoomController _controller;

        [SetUp]
        public void Setup()
        {
            _mockDbManager = new Mock<IDatabaseManager>();
            _controller = new RoomController(_mockDbManager.Object);
        }

        [Test]
        public void Post_WithValidRoom_ReturnsOk()
        {
            // Arrange
            var festival = new DataRoom("Room1", DateTime.Now, DateTime.Now.AddDays(1));
            _mockDbManager.Setup(db => db.AddNewRoomAsync(festival));

            // Act
            var result = _controller.Post(festival) as OkObjectResult;

            // Assert
            Assert.That(result != null);
            Assert.That(200 == result.StatusCode);
            Assert.That(festival == result.Value);
        }

        [Test]
        public void Post_WithEmptyRoomId_ReturnsOk()
        {
            // Arrange
            var festival = new DataRoom("Room1", DateTime.Now, DateTime.Now.AddDays(1), "");
            _mockDbManager.Setup(db => db.AddNewRoomAsync(festival));

            // Act
            var result = _controller.Post(festival) as OkObjectResult;

            // Assert
            Assert.That(result != null);
            Assert.That(200 == result.StatusCode);
            Assert.That(festival == result.Value);
        }

        [Test]
        public void Post_WithNullRoom_ReturnsBadRequest()
        {
            // Act
            var result = _controller.Post(null) as BadRequestObjectResult;

            // Assert
            Assert.That(result != null);
            Assert.That(400 == result.StatusCode);
            Assert.That("Body is empty." == result.Value);
        }

        /*[Test]
        public async Task Get_ReturnsAllRooms()
        {
            // Arrange
            var festivals = new List<DataRoom>
            {
                new("Room1", DateTime.Now, DateTime.Now.AddDays(1), Guid.NewGuid().ToString()),
                new("Room2", DateTime.Now, DateTime.Now.AddDays(1), Guid.NewGuid().ToString())
            };
            _mockDbManager.Setup(db => db.ReadAllRooms()).ReturnsAsync(festivals);

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.That(result != null, Is.True, "Result was null");
            Assert.That(result, Is.InstanceOf<List<DataRoom>>(), "Result is not a list of festivals");
            CollectionAssert.AreEqual(festivals, result as List<DataRoom>, "Rooms in result are not as expected");
        }*/

        /*[Test]
        public void Get_WithValidId_ReturnsRoom()
        {
            // Arrange
            var festivalId = "1";
            var festival = new DataRoom("Room1", DateTime.Now, DateTime.Now.AddDays(1), festivalId);
            _mockDbManager.Setup(db => db.RequestRoomByIdAsync(festivalId)).ReturnsAsync(festival);

            // Act
            var result = _controller.Get(festivalId);

            // Assert
            Assert.That(festival == result);
        }*/

        /*[Test]
        public async Task Post_WithValidIds_BoundsRoomToRoom()
        {
            // Arrange
            var festivalId = Guid.NewGuid().ToString();
            var roomId = Guid.NewGuid().ToString();
            _mockDbManager.Setup(db => db.BoundRoomToRoom(festivalId, roomId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Post(festivalId, roomId) as OkObjectResult;

            // Assert
            Assert.That(result != null);
            Assert.That(200 == result.StatusCode);
        }*/

        /*[Test]
        public void Put_WithValidRoom_ReturnsNoContent()
        {
            // Arrange
            var festivalId = Guid.NewGuid().ToString();
            var newValues = new DataRoom("UpdatedRoom", DateTime.Now, DateTime.Now.AddDays(1), festivalId);
            var oldRoom = new DataRoom("Room1", DateTime.Now, DateTime.Now.AddDays(1), festivalId);
            _mockDbManager.Setup(db => db.RequestRoomByIdAsync(festivalId)).ReturnsAsync(oldRoom);
            _mockDbManager.Setup(db => db.UpdateRoom(oldRoom));

            // Act
            var result = _controller.Put(festivalId, newValues) as NoContentResult;

            // Assert
            Assert.That(result != null);
            Assert.That(204 == result.StatusCode);
        }*/

        [Test]
        public void Delete_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var festivalId = Guid.NewGuid().ToString();
            _mockDbManager.Setup(db => db.DeleteRoomAsync(festivalId)).Returns(Task.CompletedTask);

            // Act
            var result = _controller.Delete(festivalId) as NoContentResult;

            // Assert
            Assert.That(result != null);
            Assert.That(204 == result.StatusCode);
        }

        [Test]
        public void Delete_WithInvalidId_ReturnsBadRequest()
        {
            // Act
            var result = _controller.Delete("invalid-guid") as BadRequestObjectResult;

            // Assert
            Assert.That(result != null);
            Assert.That(400 == result.StatusCode);
        }

        [Test]
        public void Delete_WithValidIds_ReturnsNoContent()
        {
            // Arrange
            var roomId = Guid.NewGuid().ToString();
            var segmentId = Guid.NewGuid().ToString();
            _mockDbManager.Setup(db => db.RemoveSegmentFromRoom(roomId, segmentId)).Returns(Task.CompletedTask);

            // Act
            var result = _controller.Delete(roomId, segmentId) as NoContentResult;

            // Assert
            Assert.That(result != null);
            Assert.That(204 == result.StatusCode);
        }

        [Test]
        public void Delete_WithInvalidRoomId_ReturnsBadRequest()
        {
            // Act
            var result = _controller.Delete("1", "invalid-guid") as BadRequestObjectResult;

            // Assert
            Assert.That(result != null);
            Assert.That(400 == result.StatusCode);
        }
    }
}
