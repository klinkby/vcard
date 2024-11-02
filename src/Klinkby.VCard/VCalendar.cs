namespace Klinkby.VCard;

/// <summary>
///     <see href="https://datatracker.ietf.org/doc/html/rfc9074.html" />
/// </summary>
[VCardToString]
public sealed partial record VCalendar : IVCardWriter
{
    /// <summary>
    ///     Publish
    /// </summary>
    public string Method { get; init; } = "PUBLISH";

    /// <summary>
    ///     Events
    /// </summary>
    [VCardWritable]
    public IEnumerable<VEvent> Events { get; set; } = [];

    /// <inheritdoc />
    public override string ToString()
    {
        using var writer = new StringWriter();
        WriteVCard(writer);
        return writer.ToString();
    }
}