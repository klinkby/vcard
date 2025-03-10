using System.Collections;

namespace Klinkby.VCard.Tests;

public sealed class TestDataGenerator : IEnumerable<object[]>
{
    private const string ExpectedVAlarm =
        "BEGIN:VALARM\nTRIGGER:trigger\nACTION:action\nDESCRIPTION:alarm\nEND:VALARM\n";

    private const string ExpectedVEvent =
        $"BEGIN:VEVENT\nORGANIZER:CN=\"organizer\"\nDTSTART:20220101T000000Z\nDTEND:20220101T010000Z\nLOCATION:location\nDESCRIPTION:description\nTRANSP:transp\nSEQUENCE:1\nUID:uid\nDTSTAMP:20220101T000000Z\nSUMMARY:summary\nPRIORITY:1\nCLASS:class\n{ExpectedVAlarm}END:VEVENT\n";

    private const string ExpectedVCalendar =
        $"BEGIN:VCALENDAR\nMETHOD:PUBLISH\n{ExpectedVEvent}{ExpectedVEvent}END:VCALENDAR\n";

    public IEnumerator<object[]> GetEnumerator()
    {
        yield return [ExpectedVAlarm, CreateVAlarm()];
        yield return [ExpectedVEvent, CreateVEvent()];
        yield return [ExpectedVCalendar, CreateVCalendar()];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private static VAlarm CreateVAlarm() =>
        new()
        {
            Trigger = "trigger",
            Action = "action",
            Description = "alarm"
        };

    private static VEvent CreateVEvent() =>
        new()
        {
            Organizer = "organizer",
            DtStart = new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            DtEnd = new DateTime(2022, 1, 1, 1, 0, 0, DateTimeKind.Utc),
            Location = "location",
            Description = "description",
            Transp = "transp",
            Sequence = 1,
            UId = "uid",
            DtStamp = new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            Summary = "summary",
            Priority = 1,
            Class = "class",
            Alarm = CreateVAlarm()
        };

    private static VCalendar CreateVCalendar() =>
        new()
        {
            Method = "PUBLISH",
            Events =
            [
                CreateVEvent(),
                CreateVEvent()
            ]
        };
}