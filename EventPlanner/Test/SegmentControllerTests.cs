using EventPlanner.Controllers;
using EventPlanner.Data;
using EventPlanner.Data.AbstractClasses;
using EventPlanner.Data.DataClasses;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;

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
        public async Task Get_ReturnsAllSegments()
        {
            // Arrange
            var segments = new List<Segment>
            {
                new DataSegment("Segment1", 10, Guid.NewGuid().ToString()),
                new DataSegment("Segment2", 10, Guid.NewGuid().ToString())
            };
            _mockDbManager.Setup(db => db.ReadAllSegments()).ReturnsAsync(segments);

            // Act
            var result = _controller.Get();

            // Assert
            Assert.That(result != null, Is.True, "Result was null");
            Assert.That(result, Is.InstanceOf<List<Segment>>(), "Result is not a list of segments");
            CollectionAssert.AreEqual(segments, result as List<Segment>, "Segments in result are not as expected");
        }

        [Test]
        public void Get_WithValidId_ReturnsSegment()
        {
            // Arrange
            var segmentId = Guid.NewGuid().ToString();
            var segment = new DataSegment("Segment1", 10, segmentId);
            _mockDbManager.Setup(db => db.RequestSegmentByIdAsync(segmentId)).ReturnsAsync(segment);

            // Act
            var result = _controller.GetId(segmentId).Result as OkObjectResult;

            // Assert
            Assert.That(result != null);
            Assert.That(200 == result.StatusCode);
            Assert.That(segment == result.Value);
        }

        [Test]
        public async Task Post_WithValidIds_BoundsRoomToSegment()
        {
            // Arrange
            var segmentId = Guid.NewGuid().ToString();
            var participantId = Guid.NewGuid().ToString();
            _mockDbManager.Setup(db => db.BoundParticipantToSegmentAsync(segmentId, participantId)).Returns(Task.CompletedTask);

            // Act
            var result = _controller.PostByUserId(segmentId, participantId) as OkObjectResult;

            // Assert
            Assert.That(result != null);
            Assert.That(200 == result.StatusCode);
        }

        [Test]
        public void Put_WithValidSegment_ReturnsNoContent()
        {
            // Arrange
            var segmentId = Guid.NewGuid().ToString();
            var newValues = new DataSegment("UpdatedSegment", 15, segmentId);
            var oldSegment = new DataSegment("Segment1", 5, segmentId);
            _mockDbManager.Setup(db => db.RequestSegmentByIdAsync(segmentId)).ReturnsAsync(oldSegment);
            _mockDbManager.Setup(db => db.UpdateSegment(oldSegment));

            // Act
            var result = _controller.Put(segmentId, newValues) as NoContentResult;

            // Assert
            Assert.That(result != null);
            Assert.That(204 == result.StatusCode);
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