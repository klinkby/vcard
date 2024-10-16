namespace Klinkby.VCard;

/// <summary>
///     <see href="https://datatracker.ietf.org/doc/html/rfc9074.html" />
/// </summary>
public sealed class VCalendar : VSerializable
{
    /// <summary>
    ///     Publish
    /// </summary>
    public string Method { get; set; } = "PUBLISH";

    /// <summary>
    ///     Events
    /// </summary>
    public IEnumerable<VEvent> Events { get; set; } = [];
}