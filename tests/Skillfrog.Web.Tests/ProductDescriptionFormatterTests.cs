using Skillfrog.Web.Demo;

namespace Skillfrog.Web.Tests;

public class ProductDescriptionFormatterTests
{
    [Fact]
    public void Format_ReturnsEmpty_WhenInputIsNullOrWhitespace()
    {
        Assert.Equal(string.Empty, ProductDescriptionFormatter.Format(null));
        Assert.Equal(string.Empty, ProductDescriptionFormatter.Format("   "));
    }

    [Fact]
    public void Format_TrimsAndCollapsesWhitespace()
    {
        var result = ProductDescriptionFormatter.Format("  Compact   wireless   keyboard  ");

        Assert.Equal("Compact wireless keyboard", result);
    }

    [Fact]
    public void Format_TruncatesLongText_AtWordBoundary()
    {
        var input = "A durable backpack designed for daily commuting and weekend travel adventures";

        var result = ProductDescriptionFormatter.Format(input, maxLength: 40);

        Assert.Equal("A durable backpack designed for daily…", result);
        Assert.True(result.Length <= 41);
    }

    [Fact]
    public void Format_Throws_WhenMaxLengthIsInvalid()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ProductDescriptionFormatter.Format("text", 0));
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("Available now", true)]
    public void HasValue_ReflectsMeaningfulContent(string? value, bool expected)
    {
        Assert.Equal(expected, ProductDescriptionFormatter.HasValue(value));
    }
}
