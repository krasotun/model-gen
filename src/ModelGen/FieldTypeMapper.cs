using System.Text.Json;

namespace ModelGen;

public static class FieldTypeMapper
{
    public static string Transform(JsonElement field)
    {
        var swaggerType = GetStringProperty(field, "type");
        var swaggerFormat = GetStringProperty(field, "format");

        return swaggerType == "array"
            ? GetArrayType(field)
            : GetScalarType(swaggerType, swaggerFormat);
    }

    private static string? GetStringProperty(JsonElement field, string propertyName) =>
        field.TryGetProperty(propertyName, out var propertyElement)
            ? propertyElement.GetString()
            : null;

    private static string GetScalarType(string? swaggerType, string? swaggerFormat) =>
        swaggerType switch
        {
            "string" => "string",
            "number" when swaggerFormat == "integer" => "int",
            "number" => "double",
            "integer" => "int",

            _ => throw new NotSupportedException($"Unsupported type '{swaggerType}'"),
        };

    private static string GetArrayType(JsonElement field)
    {
        var itemsElement = field.GetProperty("items");

        var itemsSwaggerType = GetStringProperty(itemsElement, "type");
        var itemsSwaggerFormat = GetStringProperty(itemsElement, "format");

        return GetScalarType(itemsSwaggerType, itemsSwaggerFormat) + "[]";
    }
}
