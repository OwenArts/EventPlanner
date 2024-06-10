using EventPlanner.Controllers;
using EventPlanner.Data.AbstractClasses;
using EventPlanner.Data.DataClasses;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using EventPlanner.Data;
using NUnit.Framework.Legacy;

namespace EventPlanner.Test
{
    [TestFixture]
     public class ParticipantControllerTests
    {
        private ParticipantController _controller;
        private Mock<IDatabaseManager> _mockDbManager;

        [SetUp]
        public void Setup()
        {
            _mockDbManager = new Mock<IDatabaseManager>();
            _controller = new ParticipantController(_mockDbManager.Object);
        }

        [Test]
        public void Post_WithValidParticipant_ReturnsOk()
        {
            // Arrange
            var participant = new DataParticipant("John", "Doe", "john.doe@email.com");
            _mockDbManager.Setup(db => db.AddNewParticipantAsync(participant));

            // Act
            var result = _controller.Post(participant) as OkObjectResult;

            // Assert
            Assert.That(result != null);
            Assert.That(200 == result.StatusCode);
            Assert.That(participant == result.Value);
        }

        [Test]
        public void Post_WithEmptyParticipantId_ReturnsOk()
        {
            // Arrange
            var participant = new DataParticipant("John", "Doe", "john.doe@email.com", "");
            _mockDbManager.Setup(db => db.AddNewParticipantAsync(participant));

            // Act
            var result = _controller.Post(participant) as OkObjectResult;

            // Assert
            Assert.That(result != null);
            Assert.That(200 == result.StatusCode);
            Assert.That(participant == result.Value);
        }

        [Test]
        public void Post_WithNullParticipant_ReturnsBadRequest()
        {
            // Act
            var result = _controller.Post(null) as BadRequestObjectResult;

            // Assert
            Assert.That(result != null);
            Assert.That(400 == result.StatusCode);
            Assert.That("Body is empty." == result.Value);
        }

        [Test]
        public async Task Get_ReturnsAllParticipants()
        {
            // Arrange
            var participants = new List<Participant>
            {
                new DataParticipant("John","Doe", "John.Doe@email.com" ,Guid.NewGuid().ToString()),
                new DataParticipant("Jane","Doe", "Jane.Doe@email.com" ,Guid.NewGuid().ToString()),
            };
            _mockDbManager.Setup(db => db.ReadAllParticipants()).ReturnsAsync(participants);

            // Act
            var result = _controller.Get();

            // Assert
            Assert.That(result != null, Is.True, "Result was null");
            Assert.That(result, Is.InstanceOf<List<Participant>>(), "Result is not a list of participants");
            CollectionAssert.AreEqual(participants, result as List<Participant>, "Participants in result are not as expected");
        }

        [Test]
        public void Get_WithValidId_ReturnsParticipant()
        {
            // Arrange
            var participantId = Guid.NewGuid().ToString();
            var participant = new DataParticipant("John", "Doe", "John.Doe@email.com", participantId);
            _mockDbManager.Setup(db => db.RequestParticipantByIdAsync(participantId)).ReturnsAsync(participant);

            // Act
            var result = _controller.GetId(participantId).Result.Result as OkObjectResult;

            // Assert
            Assert.That(result != null);
            Assert.That(200 == result.StatusCode);
            Assert.That(participant == result.Value);
        }

        [Test]
        public void Get_WithValidEmail_ReturnsParticipant()
        {
            // Arrange
            var participantEmail = "John.Doe@email.com";
            var participant = new DataParticipant("John", "Doe", participantEmail, Guid.NewGuid().ToString());
            _mockDbManager.Setup(db => db.RequestParticipantByEmailAsync(participantEmail)).ReturnsAsync(participant);

            // Act
            var result = _controller.GetEmail(participantEmail).Result as OkObjectResult;

            // Assert
            Assert.That(result != null);
            Assert.That(200 == result.StatusCode);
            Assert.That(participant == result.Value);
        }

        [Test]
        public void Put_WithValidParticipantId_ReturnsNoContent()
        {
            // Arrange
            var participantId = Guid.NewGuid().ToString();
            var newValues = new DataParticipant("John", "Doe", "John.Doe@email.com", participantId);
            var oldParticipant = new DataParticipant("Jane", "Doe", "Jane.Doe@email.com", participantId);
            _mockDbManager.Setup(db => db.RequestParticipantByIdAsync(participantId)).ReturnsAsync(oldParticipant);
            _mockDbManager.Setup(db => db.UpdateParticipant(oldParticipant));

            // Act
            var result = _controller.PutId(participantId, newValues) as NoContentResult;

            // Assert
            Assert.That(result != null);
            Assert.That(204 == result.StatusCode);
        }

        [Test]
        public void Put_WithValidParticipantEmail_ReturnsNoContent()
        {
            // Arrange
            var participantEmail = "John.Doe@email.com";
            var newValues = new DataParticipant("Joh", "Doeh", participantEmail, Guid.NewGuid().ToString());
            var oldParticipant = new DataParticipant("John", "Doe", participantEmail, Guid.NewGuid().ToString());
            _mockDbManager.Setup(db => db.RequestParticipantByEmailAsync(participantEmail)).ReturnsAsync(oldParticipant);
            _mockDbManager.Setup(db => db.UpdateParticipant(oldParticipant));

            // Act
            var result = _controller.PutEmail(participantEmail, newValues) as NoContentResult;

            // Assert
            Assert.That(result != null);
            Assert.That(204 == result.StatusCode);
        }
        
        [Test]
        public void Delete_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            _mockDbManager.Setup(db => db.DeleteParticipantAsync(null, userId)).Returns(Task.CompletedTask);

            // Act
            var result = _controller.DeleteId(userId) as NoContentResult;

            // Assert
            Assert.That(result != null);
            Assert.That(204 == result.StatusCode);
        }
        
        [Test]
        public void Delete_WithValidEmail_ReturnsNoContent()
        {
            // Arrange
            var Email = "John.Doe@email.com";
            _mockDbManager.Setup(db => db.DeleteParticipantAsync(Email, null)).Returns(Task.CompletedTask);

            // Act
            var result = _controller.DeleteEmail(Email) as NoContentResult;

            // Assert
            Assert.That(result != null);
            Assert.That(204 == result.StatusCode);
        }

        [Test]
        public void Delete_WithInvalidId_ReturnsBadRequest()
        {
            // Act
            var result = _controller.DeleteId("invalid-guid") as BadRequestObjectResult;

            // Assert
            Assert.That(result != null);
            Assert.That(400 == result.StatusCode);
        }

        [Test]
        public void Delete_WithInvalidEmail_ReturnsBadRequest()
        {
            // Act
            var result = _controller.DeleteEmail("invalid-email") as BadRequestObjectResult;

            // Assert
            Assert.That(result != null);
            Assert.That(400 == result.StatusCode);
        }
/*
        [Test]
        public async Task Get_ReturnsAllParticipants()
        {
            // Arrange
            var participants = new List<DataParticipant>
            {
                new DataParticipant("John", "Doe", "john@example.com"),
                new DataParticipant("Jane", "Smith", "jane@example.com")
            };
            _mockDbManager.Setup(db => db.ReadAllParticipants()).ReturnsAsync(participants);

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(participants));
        }

        [Test]
        public async Task GetId_WithValidId_ReturnsParticipant()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var participant = new DataParticipant("John", "Doe", "john.doe@email.com", userId);
            _mockDbManager.Setup(db => db.RequestParticipantByIdAsync(userId)).ReturnsAsync(participant);

            // Act
            var result = await _controller.GetId(userId);

            // Assert
            Assert.That(result.Result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(participant));
        }

        // Add more GetId tests for scenarios like invalid ID, non-existent participant, etc.
        [Test]
        public void GetEmail_WithValidEmail_ReturnsParticipant()
        {
            // Arrange
            var email = "john@example.com";
            var participant = new DataParticipant ("John", "Doe", email, Guid.NewGuid().ToString());
            _mockDbManager.Setup(db => db.RequestParticipantByEmailAsync(email)).ReturnsAsync(participant);

            // Act
            var result = _controller.GetEmail(email) as ActionResult<Participant>;

            // Assert
            Assert.That(result != null);
            Assert.That(result.Value, Is.EqualTo(participant));
        }


        // Add more GetEmail tests for scenarios like invalid email, non-existent participant, etc.

        [Test]
        public void Post_WithValidParticipant_ReturnsCreatedAtAction()
        {
            // Arrange
            var participant = new DataParticipant("John", "Doe", "john@example.com"); // Instantiate DataParticipant with required parameters
            var expectedId = Guid.NewGuid().ToString(); // Generate expected ID
            _mockDbManager.Setup(db => db.AddNewParticipantAsync(It.IsAny<DataParticipant>())).Callback<DataParticipant>(p => p.id = expectedId);

            // Act
            var result = _controller.Post(participant) as CreatedAtActionResult;

            // Assert
            Assert.That(result != null);
            Assert.That(result.ActionName, Is.EqualTo(nameof(ParticipantController.GetId)));
            Assert.That(result.RouteValues["userId"], Is.EqualTo(expectedId));
            Assert.That(result.Value, Is.EqualTo(participant));
        }


        // Add more Post tests for scenarios like null participant, validation errors, etc.

        [Test]
        public void PutId_WithValidIdAndParticipant_ReturnsNoContent()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var updatedParticipant = new DataParticipant("Updated", "Participant", "updated@example.com", userId); // Instantiate DataParticipant with required parameters
            var existingParticipant = new DataParticipant("John", "Doe", "john@example.com", userId); // Instantiate DataParticipant with required parameters
            _mockDbManager.Setup(db => db.RequestParticipantByIdAsync(userId)).ReturnsAsync(existingParticipant);

            // Act
            var result = _controller.PutId(userId, updatedParticipant) as IActionResult;

            // Assert
            Assert.That(result != null);
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        // Add more PutId tests for scenarios like invalid ID, non-existent participant, null participant, etc.

        [Test]
        public void DeleteId_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            _mockDbManager.Setup(db => db.DeleteParticipantAsync(null, userId)).Returns(Task.CompletedTask);

            // Act
            var result = _controller.DeleteId(userId) as IActionResult;

            // Assert
            Assert.That(result != null);
            Assert.That(result.GetType() == typeof(NoContentResult));
        }

        // Add more DeleteId tests for scenarios like invalid ID, non-existent participant, etc.
        */
    }
}
