using EventPlanner.Controllers;
using EventPlanner.Data;
using EventPlanner.Data.DataClasses;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace EventPlanner.Test
{
    [TestFixture]
    public class SegmentControllerTests
    {
        private Mock<IDatabaseManager> _mockDbManager;
        private SegmentController _controller;

        [SetUp]
        public void Setup()
        {
            _mockDbManager = new Mock<IDatabaseManager>();
            _controller = new SegmentController(_mockDbManager.Object);
        }

        [Test]
        public void Post_WithValidSegment_ReturnsOk()
        {
            // Arrange
            var segment = new DataSegment("Segment1", 5);
            _mockDbManager.Setup(db => db.AddNewSegmentAsync(segment));

            // Act
            var result = _controller.Post(segment) as OkObjectResult;

            // Assert
            Assert.That(result != null);
            Assert.That(200 == result.StatusCode);
            Assert.That(segment == result.Value);
        }

        [Test]
        public void Post_WithEmptySegmentId_ReturnsOk()
        {
            // Arrange
            var segment = new DataSegment("Segment", 10, "");
            _mockDbManager.Setup(db => db.AddNewSegmentAsync(segment));

            // Act
            var result = _controller.Post(segment) as OkObjectResult;

            // Assert
            Assert.That(result != null);
            Assert.That(200 == result.StatusCode);
            Assert.That(segment == result.Value);
        }

        [Test]
        public void Post_WithNullSegment_ReturnsBadRequest()
        {
            // Arrange
            _mockDbManager.Setup(db => db.AddNewSegmentAsync(It.IsAny<DataSegment>())).Returns(Task.CompletedTask);

            // Act
            var result = _controller.Post(null) as BadRequestObjectResult;

            // Assert
            Assert.That(result != null);
            Assert.That(400 == result.StatusCode);
            Assert.That("Body is empty." == result.Value);
        }


        [Test]
        public void Delete_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var segmentId = Guid.NewGuid().ToString();
            _mockDbManager.Setup(db => db.DeleteSegmentAsync(segmentId)).Returns(Task.CompletedTask);

            // Act
            var result = _controller.Delete(segmentId) as NoContentResult;

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
            var segmentId = Guid.NewGuid().ToString();
            var participantId = Guid.NewGuid().ToString();
            _mockDbManager.Setup(db => db.RemoveBoundedParticipantFromSegmentAsync(segmentId, participantId)).Returns(Task.CompletedTask);

            // Act
            var result = _controller.Delete(segmentId, participantId) as NoContentResult;

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