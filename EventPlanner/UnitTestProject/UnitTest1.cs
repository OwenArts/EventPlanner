namespace TestProject;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        Assert.Pass("test");
    }

    [Test]
    public void Test2()
    {
        Assert.That(1 == 1, "1 equals 1");
        Assert.That(1 == 2, "1 equals 2");
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