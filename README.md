# Klinkby.VCard

[![NuGet](https://img.shields.io/nuget/v/Klinkby.VCard.svg)](https://www.nuget.org/packages/Klinkby.VCard/)
[![Workflow](https://github.com/klinkby/vcard/actions/workflows/dotnet.yml/badge.svg)](https://github.com/klinkby/vcard/actions/workflows/dotnet.yml)
[![CodeQL](https://github.com/klinkby/vcard/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/klinkby/vcard/actions/workflows/github-code-scanning/codeql)
[![License](https://img.shields.io/github/license/klinkby/vcard.svg)](LICENSE)

## Summary

Super simple serialize dotnet to iCal/iCalendar/vCal/VCard format from .NET objects.

## Package

Nuget package: [Klinkby.VCard](https://www.nuget.org/packages/Klinkby.VCard/) netstandard2.0

## License

MIT licensed. See [LICENSE](https://github.com/klinkby/vcard/blob/main/LICENSE) for details.

## Dependencies

- netstandard2.0 or later

## Revisions

V3 adds source generator for more efficient serialization.
The included BenchmarkDotNet test shows a 3x speedup over V2, at 1/7th memory usage.

| Method            | Job   | NuGetReferences     | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------ |------ |-------------------- |----------:|----------:|----------:|------:|--------:|--------:|-------:|----------:|------------:|
| VCalendarToString | 2.0.1 | Klinkby.VCard 2.0.1 | 20.400 us | 0.3844 us | 0.5261 us |  1.00 |    0.04 | 14.6484 | 0.2136 | 119.81 KB |        1.00 |
| VCalendarToString | 3.1.2 | Klinkby.VCard 3.1.2 |  6.445 us | 0.0430 us | 0.0603 us |  0.32 |    0.01 |  2.0065 | 0.0610 |  16.43 KB |        0.14 |

