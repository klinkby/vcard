namespace Klinkby.VCard;

/// <summary>
///     <see href="https://datatracker.ietf.org/doc/html/rfc9074.html" />
/// </summary>
public sealed class VAlarm : VSerializable
{
    /// <summary>
    ///     <see href="https://datatracker.ietf.org/doc/html/rfc9074.html#name-alarm-proximity-trigger" />
    /// </summary>
    public string Trigger { get; set; } = "-PT15M";

    /// <summary>
    ///     <see href="https://datatracker.ietf.org/doc/html/rfc9074.html#name-alarm-proximity-trigger" />
    /// </summary>
    public string Action { get; set; } = "DISPLAY";

    /// <summary>
    ///     <see href="https://datatracker.ietf.org/doc/html/rfc9074.html#section-8.1" />
    /// </summary>
    public string Description { get; set; } = "Reminder";
}