namespace Klinkby.VCard;

/// <summary>
///     <see href="https://datatracker.ietf.org/doc/html/rfc9074.html" />
/// </summary>
[VCardToString]
public sealed partial record VAlarm : IVCardWriter
{
    internal static readonly VAlarm Default = new()
    {
        Trigger = "-P15M",
        Action = "DISPLAY",
        Description = "Reminder"
    };
    
    /// <summary>
    ///     <see href="https://datatracker.ietf.org/doc/html/rfc9074.html#name-alarm-proximity-trigger" />
    /// </summary>
    public string Trigger { get; init; } = "-PT15M";

    /// <summary>
    ///     <see href="https://datatracker.ietf.org/doc/html/rfc9074.html#name-alarm-proximity-trigger" />
    /// </summary>
    public string Action { get; init; } = "DISPLAY";

    /// <summary>
    ///     <see href="https://datatracker.ietf.org/doc/html/rfc9074.html#section-8.1" />
    /// </summary>
    public string Description { get; init; } = "Reminder";
}