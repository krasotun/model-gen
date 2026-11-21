using System.Text.Json;

namespace ModelGen.Tests;

public class FieldTypeMapperTests
{
    [Fact]
    public void Throws_NotSupportedException_when_type_is_unsupported()
    {
        using var doc = JsonDocument.Parse("""{ "type": "object" }""");

        Assert.Throws<NotSupportedException>(() => FieldTypeMapper.Transform(doc.RootElement));
    }

    [Fact]
    public void Throws_NotSupportedException_when_type_and_ref_are_missing()
    {
        using var doc = JsonDocument.Parse("""{ "mock": "mosk" }""");

        Assert.Throws<NotSupportedException>(() => FieldTypeMapper.Transform(doc.RootElement));
    }

    [Fact]
    public void Throws_NotSupportedException_for_nested_array()
    {
        using var doc = JsonDocument.Parse(
            """{ "type": "array", "items": { "type": "array", "items": { "type": "string" } } }"""
        );

        Assert.Throws<NotSupportedException>(() => FieldTypeMapper.Transform(doc.RootElement));
    }

    [Theory]
    [InlineData("""{ "type": "number", "format": "integer" }""", "int")]
    [InlineData("""{ "type": "number" }""", "double")]
    [InlineData("""{ "type": "array", "items": { "type": "integer" } }""", "int[]")]
    [InlineData(
        """{ "type": "array", "items": { "type": "number", "format": "integer" } }""",
        "int[]"
    )]
    [InlineData("""{ "type": "array", "items": { "type": "string" } }""", "string[]")]
    [InlineData("""{ "$ref": "#/definitions/ChatsResponse" }""", "ChatsResponse")]
    [InlineData(
        """{ "type": "array", "items": { "$ref": "#/definitions/Sticker" } }""",
        "Sticker[]"
    )]
    [InlineData("""{ "type": "string", "format": "date-time" }""", "DateTimeOffset")]
    [InlineData(
        """{ "type": "array", "items": { "type": "string", "format": "date-time" } }""",
        "DateTimeOffset[]"
    )]
    public void Maps_swagger_type_to_csharp_type(string fieldJson, string expectedType)
    {
        using var doc = JsonDocument.Parse(fieldJson);
        var csharpType = FieldTypeMapper.Transform(doc.RootElement);

        Assert.Equal(expectedType, csharpType);
    }
}
