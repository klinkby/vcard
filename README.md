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

V3 adds source generator for faster serialization.
BenchmarkDotNet shows a 3x speedup:

| Method            | Job   | NuGetReferences     | Mean      | Error     | StdDev    | Ratio | RatioSD |
|------------------ |------ |-------------------- |----------:|----------:|----------:|------:|--------:|
| VCalendarToString | 2.0.1 | Klinkby.VCard 2.0.1 | 19.759 us | 0.3502 us | 0.5023 us |  1.00 |    0.04 |
| VCalendarToString | 3.1.2 | Klinkby.VCard 3.1.2 |  6.408 us | 0.0587 us | 0.0804 us |  0.32 |    0.01 |
