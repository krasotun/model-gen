using System.Text.Json;

namespace ModelGen.Tests;

public class FieldTypeMapperTests
{
    [Fact]
    public void Throws_NotSupportedException_when_type_is_missing()
    {
        using var doc = JsonDocument.Parse("""{ "$ref": "#/definitions/User" }""");

        Assert.Throws<NotSupportedException>(() =>
            FieldTypeMapper.Transform(doc.RootElement, "author")
        );
    }
}
