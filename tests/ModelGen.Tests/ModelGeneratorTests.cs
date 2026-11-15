namespace ModelGen.Tests;

public class ModelGeneratorTests
{
    [Fact]
    public void Generates_record_for_schema_with_several_string_fields()
    {
        var swaggerJson = """
            {
            "definitions": {
                "SignInRequest": {
                "required": ["login", "password"],
                "properties": {
                    "login": {
                    "type": "string",
                    "description": "User login"
                    },
                    "password": {
                    "type": "string",
                    "description": "Password"
                    }
                }
                }
            }
            }
            """;

        var expected = "public record SignInRequest(string Login, string Password);";

        Assert.Equal(expected, ModelGenerator.Generate(swaggerJson));
    }

    [Fact]
    public void Generates_record_for_schema_with_one_string_field()
    {
        var swaggerJson = """
            {
                "definitions": {
                    "CreateChatRequest": {
                        "required": ["title"],
                        "properties": {
                            "title": {
                                "type": "string",
                                "description": "Chat title"
                            }
                        }
                    }
                }
            }
            """;

        var expected = "public record CreateChatRequest(string Title);";

        Assert.Equal(expected, ModelGenerator.Generate(swaggerJson));
    }
}
