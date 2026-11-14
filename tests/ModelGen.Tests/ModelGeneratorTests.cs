namespace ModelGen.Tests;

public class ModelGeneratorTests
{
    [Fact]
    public void Generates_record_for_schema_with_string_fields()
    {
        var swaggerJson = """
                            {
                "definitions": {
                    "SignInRequest": {
                    "required": [
                        "login",
                        "password"
                    ],
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
}
