namespace ModelGen.Tests;

public class ModelGeneratorTests
{
    [Fact]
    public void Generates_record_for_schema_with_several_string_fields()
    {
        var swaggerJson = ReadFixture("sign-in-request.json");

        var expected = "public record SignInRequest(string Login, string Password);";

        Assert.Equal(expected, ModelGenerator.Generate(swaggerJson));
    }

    [Fact]
    public void Generates_record_for_schema_with_one_string_field()
    {
        var swaggerJson = ReadFixture("create-chat-request.json");

        var expected = "public record CreateChatRequest(string Title);";

        Assert.Equal(expected, ModelGenerator.Generate(swaggerJson));
    }

    [Fact]
    public void Generates_record_for_schema_with_string_and_integer_field()
    {
        var swaggerJson = ReadFixture("leaderboard-request.json");

        var expected =
            "public record LeaderboardRequest(string RatingFieldName, int Cursor, int Limit);";

        Assert.Equal(expected, ModelGenerator.Generate(swaggerJson));
    }

    [Fact]
    public void Generates_record_for_schema_with_array_field()
    {
        var swaggerJson = ReadFixture("users-request.json");

        var expected = "public record UsersRequest(int[] Users, int ChatId);";

        Assert.Equal(expected, ModelGenerator.Generate(swaggerJson));
    }

    [Fact]
    public void Generates_record_for_schema_with_datetime_field()
    {
        var swaggerJson = ReadFixture("resource.json");

        var expected =
            "public record Resource(int Id, int UserId, string Path, string Filename, string ContentType, int ContentSize, DateTimeOffset UploadDate);";

        Assert.Equal(expected, ModelGenerator.Generate(swaggerJson));
    }

    private static string ReadFixture(string name)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Fixtures", name);

        return File.ReadAllText(path);
    }
}
