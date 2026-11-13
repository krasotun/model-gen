
namespace ModelGen.Tests;

public class GreeterTests
{
    [Fact]
    public void Greets_by_name()
    {
        var greeter = new Greeter();

        Assert.Equal("Hello, Ada!", greeter.Greet("Ada"));
    }
}
