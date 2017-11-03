# CSharpProperties

Java Properties implementation for C#

## What is CSharpProperties?

CSharProperties is a set of key value pair that  can be saved to a stream or loaded from a stream.

## How its works?

To load the Properties, use the `Load` static method from Properties class.

There are three ways to load properties:

### 1. Load from file path
``` c#
var properties = Properties.Load(@"C:\temp\db.properties");
```

### 2. Load from Stream

```c#
var properties = Properties.Load(stream);
```

where `stream` is a stream for data that contains the properties.

Ex:
```c#
var path = Path.GetTempFileName();
var properties = Properties.Load(File.Open(path, FileMode.Open));
```

`File.Open()` return a `FileStream` to file specified by `path` variable.

> The `Load` method calls `Dispose` in its execution, so do not reuse the stream object.

### 3. Load from Reader

You can use a `StreamReader` or `StringReader` for load the properties.

```c#
var content = @"
# This is a comment
host=localhost
port=123
database=test
";
var properties = Properties.Load(new StringReader(content));
```

> The `Load` method calls `Dispose` in its execution, so do not reuse the reader object.

### LoadAsync

Use `LoadAsync` for async operations:

```c#
var properties = await Properties.LoadAsync(@"C:\temp\db.properties");
```

## Getting the Properties

By default, lines beginning with the characters `'`, `;` and `#` are considered comments and will not be treated as a property. A valid property, by default, follows the template: `host=localhost` where `host` is the key and `localhost` is the value.

> Do not use the same key twice, the Properties class will only handle the first key-value pair, the others will be disregarded.

Use the method `Get` or the operator `[]` by passing a string that represents the key

```c#
var host = properties["host"]; //or properties.Get("host");
```

Consult [IProperties](https://github.com/MosaicoSolutions/CSharpProperties/blob/master/MosaicoSolutions.CSharpProperties/IProperties.cs) to view all methods and properties of the class.

## Iterating through Properties

Use foreach to iterate through Properties:

```c#
foreach (var property in properties)
  Console.WriteLine($"{property.Key}={property.Value}");
```

## Saving the Properties

