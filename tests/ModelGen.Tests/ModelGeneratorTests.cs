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
            "public record Resource(int Id, [property: System.Text.Json.Serialization.JsonPropertyName(\"user_id\")] int UserId, string Path, string Filename, [property: System.Text.Json.Serialization.JsonPropertyName(\"content_type\")] string ContentType, [property: System.Text.Json.Serialization.JsonPropertyName(\"content_size\")] int ContentSize, [property: System.Text.Json.Serialization.JsonPropertyName(\"upload_date\")] DateTimeOffset UploadDate);";

        Assert.Equal(expected, ModelGenerator.Generate(swaggerJson));
    }

    [Fact]
    public void Generates_record_for_schema_with_not_required_fields()
    {
        var swaggerJson = ReadFixture("chat-message.json");

        var expected =
            "public record ChatMessage(int Id, [property: System.Text.Json.Serialization.JsonPropertyName(\"user_id\")] int UserId, [property: System.Text.Json.Serialization.JsonPropertyName(\"chat_id\")] int ChatId, DateTimeOffset Time, string Type, string Content, Resource? File);";

        Assert.Equal(expected, ModelGenerator.Generate(swaggerJson));
    }

    [Fact]
    public void Generates_record_for_schema_when_required_is_missing()
    {
        var swaggerJson = ReadFixture("sticker.json");

        var expected = "public record Sticker(int? Id, string? Path);";

        Assert.Equal(expected, ModelGenerator.Generate(swaggerJson));
    }

    [Fact]
    public void Generates_record_with_json_names_for_snake_case_fields()
    {
        var swaggerJson = ReadFixture("user-response.json");

        var expected =
            "public record UserResponse(int Id, [property: System.Text.Json.Serialization.JsonPropertyName(\"first_name\")] string FirstName, [property: System.Text.Json.Serialization.JsonPropertyName(\"second_name\")] string SecondName, [property: System.Text.Json.Serialization.JsonPropertyName(\"display_name\")] string DisplayName, string Login, string Email, string Phone, string Avatar);";

        Assert.Equal(expected, ModelGenerator.Generate(swaggerJson));
    }

    private static string ReadFixture(string name)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Fixtures", name);

        return File.ReadAllText(path);
    }
}
