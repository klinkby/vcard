using System.Globalization;

namespace Klinkby.VCard;

/// <summary>
///     <see href="https://datatracker.ietf.org/doc/html/rfc5545#section-3.6.1" />
/// </summary>
public sealed class VEvent : VSerializable
{
    /// <summary>
    ///     Create a new VEvent
    /// </summary>
    /// <param name="fromTime">DtStart</param>
    /// <param name="toTime">DtEnd</param>
    /// <param name="createdDateTime">DtStamp</param>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public VEvent(DateTime fromTime, DateTime toTime, DateTime createdDateTime)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        SetDtStart(fromTime);
        SetDtEnd(toTime);
        SetDtStamp(createdDateTime);
    }

    /// <summary>
    ///     Organizer
    /// </summary>
    public string? Organizer { get; set; }

    /// <summary>
    ///     Start time
    /// </summary>
    public string DtStart { get; private set; }

    /// <summary>
    ///     End time
    /// </summary>
    public string DtEnd { get; private set; }

    /// <summary>
    ///     Location
    /// </summary>
    public string Location { get; set; }

    /// <summary>
    ///     Description
    /// </summary>
    public string Description { get; set; }

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
    public string UId { get; set; }

    /// <summary>
    ///     Time stamp
    /// </summary>
    public string DtStamp { get; private set; }

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
    public VAlarm Alarm { get; private set; } = new();

    /// <summary>
    ///     Format the start time
    /// </summary>
    /// <param name="dt"></param>
    public void SetDtStart(DateTime dt)
    {
        DtStart = dt.ToString("yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture);
    }

    /// <summary>
    ///     Format the end time
    /// </summary>
    /// <param name="dt"></param>
    public void SetDtEnd(DateTime dt)
    {
        DtEnd = dt.ToString("yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture);
    }

    /// <summary>
    ///     Format the time stamp
    /// </summary>
    /// <param name="dt"></param>
    public void SetDtStamp(DateTime dt)
    {
        DtStamp = dt.ToString("yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture);
    }
}