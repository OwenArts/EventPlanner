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
            var room = new DataRoom("Room1", DateTime.Now, DateTime.Now.AddDays(1));
            _mockDbManager.Setup(db => db.AddNewRoomAsync(room));

            // Act
            var result = _controller.Post(room) as OkObjectResult;

            // Assert
            Assert.That(result != null);
            Assert.That(200 == result.StatusCode);
            Assert.That(room == result.Value);
        }

        [Test]
        public void Post_WithEmptyRoomId_ReturnsOk()
        {
            // Arrange
            var room = new DataRoom("Room1", DateTime.Now, DateTime.Now.AddDays(1), "");
            _mockDbManager.Setup(db => db.AddNewRoomAsync(room));

            // Act
            var result = _controller.Post(room) as OkObjectResult;

            // Assert
            Assert.That(result != null);
            Assert.That(200 == result.StatusCode);
            Assert.That(room == result.Value);
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

        [Test]
        public async Task Get_ReturnsAllRooms()
        {
            // Arrange
            var rooms = new List<DataRoom>
            {
                new("Room1", DateTime.Now, DateTime.Now.AddDays(1), Guid.NewGuid().ToString()),
                new("Room2", DateTime.Now, DateTime.Now.AddDays(1), Guid.NewGuid().ToString())
            };
            _mockDbManager.Setup(db => db.ReadAllRooms()).ReturnsAsync(rooms);

            // Act
            var result = _controller.Get();

            // Assert
            Assert.That(result != null, Is.True, "Result was null");
            Assert.That(result, Is.InstanceOf<List<DataRoom>>(), "Result is not a list of rooms");
            CollectionAssert.AreEqual(rooms, result as List<DataRoom>, "Rooms in result are not as expected");
        }

        [Test]
        public void Get_WithValidId_ReturnsRoom()
        {
            // Arrange
            var roomId = Guid.NewGuid().ToString();
            var room = new DataRoom("Room1", DateTime.Now, DateTime.Now.AddDays(1), roomId);
            _mockDbManager.Setup(db => db.RequestRoomByIdAsync(roomId)).ReturnsAsync(room);

            // Act
            var result = _controller.Get(roomId);

            // Assert
            Assert.That(room == result);
        }

        [Test]
        public async Task Post_WithValidIds_BoundsRoomToRoom()
        {
            // Arrange
            var roomId = Guid.NewGuid().ToString();
            var segmentId = Guid.NewGuid().ToString();
            _mockDbManager.Setup(db => db.BoundSegmentToRoom(roomId, segmentId)).Returns(Task.CompletedTask);

            // Act
            var result = _controller.Post(roomId, segmentId) as OkObjectResult;

            // Assert
            Assert.That(result != null);
            Assert.That(200 == result.StatusCode);
        }

        [Test]
        public void Put_WithValidRoom_ReturnsNoContent()
        {
            // Arrange
            var roomId = Guid.NewGuid().ToString();
            var newValues = new DataRoom("UpdatedRoom", DateTime.Now, DateTime.Now.AddDays(1), roomId);
            var oldRoom = new DataRoom("Room1", DateTime.Now, DateTime.Now.AddDays(1), roomId);
            _mockDbManager.Setup(db => db.RequestRoomByIdAsync(roomId)).ReturnsAsync(oldRoom);
            _mockDbManager.Setup(db => db.UpdateRoom(oldRoom));

            // Act
            var result = _controller.Put(roomId, newValues) as NoContentResult;

            // Assert
            Assert.That(result != null);
            Assert.That(204 == result.StatusCode);
        }
        
        [Test]
        public void Delete_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var roomId = Guid.NewGuid().ToString();
            _mockDbManager.Setup(db => db.DeleteRoomAsync(roomId)).Returns(Task.CompletedTask);

            // Act
            var result = _controller.Delete(roomId) as NoContentResult;

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
