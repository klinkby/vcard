namespace Klinkby.VCard;

/// <summary>
///     <see href="https://datatracker.ietf.org/doc/html/rfc5545#section-3.6.1" />
/// </summary>
[VCardToString]
public sealed partial record VEvent : IVCardWriter
{
    /// <summary>
    ///     Create a new event
    /// </summary>
    public VEvent()
    {
    }

    /// <summary>
    ///     Create a new event
    /// </summary>
    /// <param name="fromTime">Start time</param>
    /// <param name="toTime">End time</param>
    /// <param name="createdDateTime">Create time</param>
    [Obsolete("Use object initializer instead")]
    public VEvent(DateTime fromTime, DateTime toTime, DateTime createdDateTime) : this()
    {
        DtStart = fromTime;
        DtEnd = toTime;
        DtStamp = createdDateTime;
    }

    /// <summary>
    ///     Organizer
    /// </summary>
    public string? Organizer { get; init; }

    /// <summary>
    ///     Start time
    /// </summary>
    public DateTime DtStart { get; init; }

    /// <summary>
    ///     End time
    /// </summary>
    public DateTime DtEnd { get; init; }

    /// <summary>
    ///     Location
    /// </summary>
    public string? Location { get; init; }

    /// <summary>
    ///     Description
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    ///     OPAQUE
    /// </summary>
    public string Transp { get; init; } = "OPAQUE";

    /// <summary>
    ///     Recurrence sequence
    /// </summary>
    public int Sequence { get; init; }

    /// <summary>
    ///     Recurrence unique identifier
    /// </summary>
    public string? UId { get; init; }

    /// <summary>
    ///     Time stamp
    /// </summary>
    public DateTime DtStamp { get; init; } = DateTime.UtcNow;

    /// <summary>
    ///     Summary
    /// </summary>
    public string? Summary { get; init; }

    /// <summary>
    ///     Priority
    /// </summary>
    public int Priority { get; init; } = 5;

    /// <summary>
    ///     PUBLIC
    /// </summary>
    public string Class { get; init; } = "PUBLIC";

    /// <summary>
    ///     Alarm/Reminder
    /// </summary>
    [VCardWritable]
    public VAlarm Alarm { get; init; } = VAlarm.Default;
}