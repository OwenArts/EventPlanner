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
    public class FestivalControllerTests
    {
        private Mock<IDatabaseManager> _mockDbManager;
        private FestivalController _controller;

        [SetUp]
        public void Setup()
        {
            _mockDbManager = new Mock<IDatabaseManager>();
            _controller = new FestivalController(_mockDbManager.Object);
        }

        [Test]
        public void Post_WithValidFestival_ReturnsOk()
        {
            // Arrange
            var festival = new DataFestival("Festival1", DateTime.Now, DateTime.Now.AddDays(1));
            _mockDbManager.Setup(db => db.AddNewFestivalAsync(festival));

            // Act
            var result = _controller.Post(festival) as OkObjectResult;

            // Assert
            Assert.That(result != null);
            Assert.That(200 == result.StatusCode);
            Assert.That(festival == result.Value);
        }

        [Test]
        public void Post_WithEmptyFestivalId_ReturnsOk()
        {
            // Arrange
            var festival = new DataFestival("Festival1", DateTime.Now, DateTime.Now.AddDays(1), "");
            _mockDbManager.Setup(db => db.AddNewFestivalAsync(festival));

            // Act
            var result = _controller.Post(festival) as OkObjectResult;

            // Assert
            Assert.That(result != null);
            Assert.That(200 == result.StatusCode);
            Assert.That(festival == result.Value);
        }

        [Test]
        public void Post_WithNullFestival_ReturnsBadRequest()
        {
            // Act
            var result = _controller.Post(null) as BadRequestObjectResult;

            // Assert
            Assert.That(result != null);
            Assert.That(400 == result.StatusCode);
            Assert.That("Body is empty." == result.Value);
        }

        [Test]
        public async Task Get_ReturnsAllFestivals()
        {
            // Arrange
            var festivals = new List<DataFestival>
            {
                new("Festival1", DateTime.Now, DateTime.Now.AddDays(1), Guid.NewGuid().ToString()),
                new("Festival2", DateTime.Now, DateTime.Now.AddDays(1), Guid.NewGuid().ToString())
            };
            _mockDbManager.Setup(db => db.ReadAllFestivals()).ReturnsAsync(festivals);

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.That(result != null, Is.True, "Result was null");
            Assert.That(result, Is.InstanceOf<List<DataFestival>>(), "Result is not a list of festivals");
            CollectionAssert.AreEqual(festivals, result as List<DataFestival>, "Festivals in result are not as expected");
        }

        [Test]
        public void Get_WithValidId_ReturnsFestival()
        {
            // Arrange
            var festivalId = Guid.NewGuid().ToString();
            var festival = new DataFestival("Festival1", DateTime.Now, DateTime.Now.AddDays(1), festivalId);
            _mockDbManager.Setup(db => db.RequestFestivalByIdAsync(festivalId)).ReturnsAsync(festival);

            // Act
            var result = _controller.Get(festivalId);

            // Assert
            Assert.That(festival == result);
        }

        [Test]
        public async Task Post_WithValidIds_BoundsRoomToFestival()
        {
            // Arrange
            var festivalId = Guid.NewGuid().ToString();
            var roomId = Guid.NewGuid().ToString();
            _mockDbManager.Setup(db => db.BoundRoomToFestival(festivalId, roomId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Post(festivalId, roomId) as OkObjectResult;

            // Assert
            Assert.That(result != null);
            Assert.That(200 == result.StatusCode);
        }

        [Test]
        public void Put_WithValidFestival_ReturnsNoContent()
        {
            // Arrange
            var festivalId = Guid.NewGuid().ToString();
            var newValues = new DataFestival("UpdatedFestival", DateTime.Now, DateTime.Now.AddDays(1), festivalId);
            var oldFestival = new DataFestival("Festival1", DateTime.Now, DateTime.Now.AddDays(1), festivalId);
            _mockDbManager.Setup(db => db.RequestFestivalByIdAsync(festivalId)).ReturnsAsync(oldFestival);
            _mockDbManager.Setup(db => db.UpdateFestival(oldFestival));

            // Act
            var result = _controller.Put(festivalId, newValues) as NoContentResult;

            // Assert
            Assert.That(result != null);
            Assert.That(204 == result.StatusCode);
        }

        [Test]
        public void Delete_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var festivalId = Guid.NewGuid().ToString();
            _mockDbManager.Setup(db => db.DeleteFestivalAsync(festivalId)).Returns(Task.CompletedTask);

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
            var festivalId = Guid.NewGuid().ToString();
            var roomId = Guid.NewGuid().ToString();
            _mockDbManager.Setup(db => db.RemoveRoomFromFestival(festivalId, roomId)).Returns(Task.CompletedTask);

            // Act
            var result = _controller.Delete(festivalId, roomId) as NoContentResult;

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
