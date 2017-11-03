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

Use `LoadAsync` for asynchronous operations:

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

You can save the properties just by passing the `File Path`, or a `Stream`, or a `Reader`.

```c#
properties.Add("database", "test"); // add a property 'database' with value of 'test'
properties.Save(@"C:\temp\db.properties"); // The file where the properties will be saved
```

> The `Save` method calls `Dispose` in its execution, so if you use a `Stream` or a `Writer` to store the properties, do not reuse the object.

### SaveAsync

Use `SaveAsync` for asynchronous operations:

```c#
await Properties.SaveAsync(@"C:\temp\db.properties");
```

## PropertiesBuild

You can customize how the `Properties` class handles properties. See the example below.

```c#
var content = @"
username:=cooper

#Don't make this!
password:=1234

#Email will not be captured by the handle
email=cooper@mail
";

bool IsValidLine(string line) =>
    !line.StartsWith("#") && line.Contains(":=");

KeyValuePair<string, string> PropertyHandle(string line)
{
    var tokens = line.Split(":=");
    return tokens.Length == 2
            ? new KeyValuePair<string, string>(tokens[0].Trim(), tokens[1].Trim())
            : new KeyValuePair<string, string>();
}

var properties = PropertiesBuild.NewPropertiesBuild()
                                .WithValidLineHandle(IsValidLine)
                                .WithExtractPropertyHandle(PropertyHandle)
                                .BuildWithTextReader(new StringReader(content));
```

The `IsValidLine` method evaluates whether the line is valid, in this example, a line that does not start with `#` and contains `:=` is considered valid.

The `PropertyHandle` method is responsible for extracting the property of a string, in this example it separates the string by `:=` and gets a key-value pair. The property `email=cooper@mail` will not be captured by the handler, because according to the `IsValidLine` method, it is an invalid line.


Finally, use `PropertiesBuilder` to construct the properties using the handlers, in a fluent interface.

## <> With :heart: and [VSCode](https://code.visualstudio.com)
