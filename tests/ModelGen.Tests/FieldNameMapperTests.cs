namespace ModelGen.Tests;

public class FieldNameMapperTests
{
    [Theory]
    [InlineData("first_name", "FirstName")]
    [InlineData("unread_count", "UnreadCount")]
    [InlineData("chatId", "ChatId")]
    [InlineData("id", "Id")]
    public void Capitalizes_each_word_and_removes_underscores(string input, string expected)
    {
        var mappedField = FieldNameMapper.Transform(input);
        Assert.Equal(expected, mappedField);
    }

    [Fact(Skip = "first__Name throws IndexOutOfRangeException, decide in lesson 18")]
    public void Double_underscore_is_handled() { }
}
