# ilspy
.Net Bytecode Inspector

## Development

- Requires .Net 5.0

```
$ dotnet restore
$ dotnet build
```

## Usage

Show all classes and methods from a given assembly:

```
$ dotnet run myassembly.dll
```

Print bytecode instructions for a specific method:

```
$ dotnet run myassembly.dll Greeter.Hello
```

## License

MIT
