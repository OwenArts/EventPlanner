using EventPlanner.Controllers;
using EventPlanner.Data.DataClasses;
using NUnit.Framework;

namespace EventPlanner.Test;

[TestFixture]
public class TestTest
{
    private ParticipantController _pController;
    
    [SetUp]
    public void Setup()
    {
        _pController = new ParticipantController();
    }

    [Test]
    public void AddingIdToParticipant()
    {
        var tempParticipant = new DataParticipant("John", "Doe", "John.Doe@Email.com", null, null, null, null);
        Assert.That(tempParticipant.id != null && tempParticipant.id.Length > 0 && IsGuid(tempParticipant.id));
        
        tempParticipant = new DataParticipant("John", "Doe", "John.Doe@Email.com", null, null, null, null);
        Assert.That(tempParticipant.id != null && tempParticipant.id.Length > 0 && IsGuid(tempParticipant.id));
    }

    [Test]
    public void ParticipantTestSetToPass()
    {
        Assert.Pass("This is a test that was meant to fail");
        // Assert.Fail("This is a test that was meant to fail");
    }
    
    public bool IsGuid(string value)
    {
        return Guid.TryParse(value, out _);
    }

    /*gpt example for how to test api's

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
            new Item { /* properties * / },
            new Item { /* properties * / },
            new Item { /* properties * / }
        };
    }
}

     */
}