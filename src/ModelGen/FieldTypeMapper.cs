using System.Text.Json;

namespace ModelGen;

public static class FieldTypeMapper
{
    public static string Transform(JsonElement field, string fieldName)
    {
        var swaggerType = field.TryGetProperty("type", out var typeElement)
            ? typeElement.GetString()
            : null;
        return swaggerType switch
        {
            "string" => "string",
            "integer" => "int",
            _ => throw new NotSupportedException(
                $"Unsupported type '{swaggerType}' in field '{fieldName}'"
            ),
        };
    }
}
