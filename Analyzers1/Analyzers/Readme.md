# Roslyn Analyzers Sample

A sample projects that includes Roslyn analyzers with code fix providers.

## Content
### Analyzers
A .NET Standard project with implementations of analyzers and code fix providers.
**You must build this project to see the results (warnings) in the IDE.**

- [RequestSemanticAnalyzer.cs](SampleSemanticAnalyzer.cs): An analyzer that reports classes that implement `IRequest` that are named without Request suffix.
- [RequestCodeFixProvider.cs](SampleCodeFixProvider.cs): A code fix that renames classes with Request suffix. The fix is linked to [RequestSyntaxAnalyzer.cs](SampleSyntaxAnalyzer.cs).

### Analyzers.Sample
A project that references the sample analyzers. Note the parameters of `ProjectReference` in [Analyzers.Sample.csproj](../Analyzers1.Sample/Analyzers1.Sample.csproj), they make sure that the project is referenced as a set of analyzers. 

### Analyzers.Tests
Unit tests for the sample analyzers and code fix provider. The easiest way to develop language-related features is to start with unit tests.

## How To?
### How to debug?
- Use the [launchSettings.json](Properties/launchSettings.json) profile.
- Debug tests.

### How can I determine which syntax nodes I should expect?
Consider installing the Roslyn syntax tree viewer plugin [Rossynt](https://plugins.jetbrains.com/plugin/16902-rossynt/).

### Learn more about wiring analyzers
The complete set of information is available at [roslyn github repo wiki](https://github.com/dotnet/roslyn/blob/main/docs/wiki/README.md).