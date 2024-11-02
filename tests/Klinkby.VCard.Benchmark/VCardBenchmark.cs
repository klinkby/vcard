using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace Klinkby.VCard.Benchmark;

[Config(typeof(Config))]
public class VCardBenchmark
{
    private VCalendar? _vCalendar;

    [GlobalSetup]
    public void Setup() => _vCalendar = CreateVCalendar();
        
    [Benchmark]
    public void VCalendarToString() => _ = _vCalendar!.ToString();
    
    private static VEvent CreateVEvent()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        VEvent evt = new(
            new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            new DateTime(2022, 1, 1, 1, 0, 0, DateTimeKind.Utc),
            new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        )
        {
            Organizer = "organizer",
            Location = "location",
            Description = "description",
            Transp = "transp",
            Sequence = 1,
            UId = "uid",
            Summary = "summary",
            Priority = 1,
            Class = "class",
        };
#pragma warning restore CS0618 // Type or member is obsolete
        return evt;
    }

    private static VCalendar CreateVCalendar() =>
        new()
        {
            Method = "PUBLISH",
            Events = Enumerable.Range(0, 10).Select(_ => CreateVEvent()).ToArray()
        };

    #pragma warning disable CA1812
    /// <inheritdoc />
    private sealed class Config : ManualConfig
    {
        /// <inheritdoc />
        public Config()
        {
            var baseJob = Job.MediumRun;

            AddJob(baseJob.WithNuGet("Klinkby.VCard", "2.0.0").WithId("2.0.0"));
            AddJob(baseJob.WithNuGet("Klinkby.VCard", "3.0.2").WithId("3.0.2"));
        }
    }
    #pragma warning restore CA1812
}