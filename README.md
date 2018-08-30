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

### LoadAsync

Use `LoadAsync` for asynchronous operations:

```c#
var properties = await Properties.LoadAsync(@"C:\temp\db.properties");
```

## Getting the Properties

By default, lines beginning with the characters `"`, `;` and `#` are considered comments and will not be treated as a property. A valid property, by default, follows the template: `host=localhost` where `host` is the key and `localhost` is the value.

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
properties.Add("database", "test"); // add a property "database" with value of "test"
properties.Save(@"C:\temp\db.properties"); // The file where the properties will be saved
```

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

#Don"t make this!
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

## Properties Strategy

You can load and save the properties from Json, Xml and csv.

### Json

```c#
var properties = Properties.LoadFromJson(new StringReader(content));
```

> The json should follow the following scheme

```json
{
    "type": "array",
    "items": {
        "type": "object",
        "properties": {
            "key": {
                "type": "string",
                "required": true
            },
            "value": {
                "type": "string",
                "required": true
            }
        }
    }
}
```

```json
[
    {
        "key": "host",
        "value": "localhost"
    },
    {
        "key": "database",
        "value": "inception"
    }
]
```

### Xml

```c#
var properties = Properties.LoadFromXml(new StringReader(content));
```

> The xml should follow the following scheme

```xml
<?xml version='1.0' encoding='UTF-8'?>
<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'
	attributeFormDefault='unqualified' elementFormDefault='qualified'>
    <xs:element name='properties' type='propertiesType' />
    <xs:complexType name='propertiesType'>
        <xs:sequence>
            <xs:element type='propertyType' name='property' maxOccurs='unbounded' minOccurs='0' />
        </xs:sequence>
    </xs:complexType>
    <xs:complexType name='propertyType'>
        <xs:simpleContent>
            <xs:extension base='xs:string'>
                <xs:attribute type='keyType' name='key' use='required' />
                <xs:attribute type='valueType' name='value' use='required' />
            </xs:extension>
        </xs:simpleContent>
    </xs:complexType>
    <xs:simpleType name='keyType'>
        <xs:restriction base='xs:string'>
            <xs:minLength value='1' />
        </xs:restriction>
    </xs:simpleType>

    <xs:simpleType name='valueType'>
        <xs:restriction base='xs:string'>
            <xs:minLength value='0' />
        </xs:restriction>
    </xs:simpleType>
	
</xs:schema>
```

```xml
<?xml version='1.0' encoding='UTF-8'?>
<properties>
    <property key='name' value='Saito' />
    <property key='role' value='Turist' />
</properties>
```

### CSV

```c#
var properties = Properties.LoadFromCsv(new StringReader(content));
```

> The delimiter character used is the semicolon (';')

```csv
email;saito@mail.com
role;turist
```

## Defining a Strategy

To define your own strategy use the Delegate `PropertiesStrategy`

```c#
PropertiesStrategy myStrategy = content => 
{
    //Do yout logic here...

    return Of(properties);
};
```

`PropertiesStrategy` receives as argument a string that is the content read from the file (or `Stream`) and returns an `IProperties`. 
Use the `LoadFromStrategy` method to load properties based on the created strategy.

```c#
var properties = Properties.LoadFromStrategy(myStrategy, file);
```

## <> With :heart: and [VSCode](https://code.visualstudio.com)
