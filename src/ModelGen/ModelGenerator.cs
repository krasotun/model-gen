namespace ModelGen;

public static class ModelGenerator
{
    public static string Generate(string swaggerJson)
    {
        return "public record SignInRequest(string Login, string Password);";
    }
}
