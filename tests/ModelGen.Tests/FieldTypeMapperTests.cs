using System.Text.Json;

namespace ModelGen.Tests;

public class FieldTypeMapperTests
{
    [Fact]
    public void Throws_NotSupportedException_when_type_is_missing()
    {
        using var doc = JsonDocument.Parse("""{ "$ref": "#/definitions/User" }""");

        Assert.Throws<NotSupportedException>(() => FieldTypeMapper.Transform(doc.RootElement));
    }

    [Theory]
    [InlineData("""{ "type": "number", "format": "integer" }""", "int")]
    [InlineData("""{ "type": "number" }""", "double")]
    public void Maps_swagger_type_to_csharp_type(string fieldJson, string expectedType)
    {
        using var doc = JsonDocument.Parse(fieldJson);
        var csharpType = FieldTypeMapper.Transform(doc.RootElement);

        Assert.Equal(expectedType, csharpType);
    }
}
