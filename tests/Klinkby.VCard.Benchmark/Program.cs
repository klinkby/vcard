using BenchmarkDotNet.Running;

BenchmarkSwitcher.FromAssemblies([typeof(Program).Assembly]).Run(args);
