namespace Klinkby.VCard;

/// <summary>
///     <see href="https://datatracker.ietf.org/doc/html/rfc5545#section-3.6.1" />
/// </summary>
[VCardToString]
public partial record VEvent : IVCardWriter
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
    public string? Organizer { get; set; }

    /// <summary>
    ///     Start time
    /// </summary>
    public DateTime DtStart { get; set; }

    /// <summary>
    ///     End time
    /// </summary>
    public DateTime DtEnd { get; set; }

    /// <summary>
    ///     Location
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    ///     Description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///     OPAQUE
    /// </summary>
    public string Transp { get; set; } = "OPAQUE";

    /// <summary>
    ///     Recurrence sequence
    /// </summary>
    public int Sequence { get; set; }

    /// <summary>
    ///     Recurrence unique identifier
    /// </summary>
    public string? UId { get; set; }

    /// <summary>
    ///     Time stamp
    /// </summary>
    public DateTime DtStamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    ///     Summary
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    ///     Priority
    /// </summary>
    public int Priority { get; set; } = 5;

    /// <summary>
    ///     PUBLIC
    /// </summary>
    public string Class { get; set; } = "PUBLIC";

    /// <summary>
    ///     Alarm/Reminder
    /// </summary>
    [VCardWritable]
    public VAlarm Alarm { get; set; } = VAlarm.Default;
}