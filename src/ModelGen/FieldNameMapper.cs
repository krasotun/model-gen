using System.Text;

namespace ModelGen;

public static class FieldNameMapper
{
    public static string Transform(string fieldName)
    {
        var words = fieldName.Split("_");
        var sb = new StringBuilder();

        foreach (var word in words)
        {
            sb.Append(Capitalize(word));
        }

        return sb.ToString();
    }

    private static string Capitalize(string word) => char.ToUpperInvariant(word[0]) + word[1..];
}
