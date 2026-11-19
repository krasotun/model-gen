using System.Text;
using System.Text.Json;

namespace ModelGen;

public static class ModelGenerator
{
    public static string Generate(string swaggerJson)
    {
        using var doc = JsonDocument.Parse(swaggerJson);
        JsonElement root = doc.RootElement;

        var sb = new StringBuilder();

        sb.Append("public record ");

        var definitions = root.GetProperty("definitions");

        foreach (JsonProperty definition in definitions.EnumerateObject())
        {
            var name = definition.Name;
            var properties = definition.Value.GetProperty("properties");

            sb.Append(name).Append('(');

            List<string> props = [];

            foreach (JsonProperty property in properties.EnumerateObject())
            {
                var propertyName = property.Name;
                props.Add(
                    $"{FieldTypeMapper.Transform(property.Value, propertyName)} {FieldNameMapper.Transform(propertyName)}"
                );
            }

            sb.Append(string.Join(", ", props)).Append(");");
        }

        return sb.ToString();
    }
}
