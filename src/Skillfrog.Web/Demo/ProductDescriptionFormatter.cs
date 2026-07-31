namespace Skillfrog.Web.Demo;

/// <summary>
/// Formats product short descriptions for display.
/// Used in the session demo and covered by unit tests so CI has a meaningful check.
/// </summary>
public static class ProductDescriptionFormatter
{
    public const int DefaultMaxLength = 160;

    /// <summary>
    /// Returns a trimmed short description suitable for product cards and meta text.
    /// </summary>
    public static string Format(string? shortDescription, int maxLength = DefaultMaxLength)
    {
        if (maxLength <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxLength), "Max length must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(shortDescription))
        {
            return string.Empty;
        }

        var normalized = string.Join(
            ' ',
            shortDescription.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));

        if (normalized.Length <= maxLength)
        {
            return normalized;
        }

        var truncated = normalized[..maxLength].TrimEnd();
        var lastSpace = truncated.LastIndexOf(' ');
        if (lastSpace > maxLength / 2)
        {
            truncated = truncated[..lastSpace];
        }

        return truncated + "…";
    }

    /// <summary>
    /// Returns true when a short description is present and not blank.
    /// </summary>
    public static bool HasValue(string? shortDescription) =>
        !string.IsNullOrWhiteSpace(shortDescription);
}
