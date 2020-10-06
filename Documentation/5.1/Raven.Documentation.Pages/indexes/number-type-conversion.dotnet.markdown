# Indexing: Numerical Type Conversion

---

{NOTE: }

* The `TryConvert()` method can be used to safely convert values to numerical types.  

* Learn more about how numbers are stored in RavenDB [here](../server/kb/numbers-in-ravendb).  

* In this page:  
  * [Syntax](..\indexes\number-type-conversion#syntax)  
  * [Examples](..\indexes\number-type-conversion#examples)  

{NOTE/}

---

{PANEL: Syntax}

The following methods are used to convert values into one of the common primitive numerical 
types - `int`, `long`, `float`, or `double`. They are called from within the 
[Map](../indexes/map-indexes) or [Reduce](../indexes/map-reduce-indexes) functions of the 
index. If the submitted value cannot be converted to the specified type, these methods return 
`null`.  

In **LINQ syntax**, use `TryConvert<T>()`:  

{CODE-BLOCK:csharp}
protected T? TryConvert<T>(object value)
{CODE-BLOCK/}

| Parameter | Type | Description |
| - | - | - |
| **T** | Generic type parameter | The numerical type to which you want to convert your value. Possible values:<br/>- `int`<br/>- `long`<br/>- `float`<br/>- `double` |
| **value** | `object` | The value you want to convert, such as a document field. If you pass a `string` or `object`, the method will attempt to parse it for a numerical value. |

In **JavaScript syntax**, use `tryConvertToNumber()`.

{CODE-BLOCK:javascript}
tryConvertToNumber(value)
{CODE-BLOCK/}

| Parameter | Type | Description |
| - | - | - |
| **value** | `object` | The value you want to convert, such as a document field. If you pass a `string` or `object`, the method will attempt to parse it for a numerical value. |

{PANEL/}

{PANEL: Examples}

The class `Item` has fields of type `int`, `long`, `float`, `double`, `string`, and an object 
field of type [Company](../start/about-examples). The following indexes take an `Item` 
entity and attempt to convert each of its fields into the corresponding type. In case of 
failure, the field is indexed with value `-1` instead.  

{CODE-TABS}
{CODE-TAB:csharp:LINQ tryconvert_linq@Indexes/NumberTypeConversion.cs /}
{CODE-TAB:csharp:JavaScript tryconvert_js@Indexes/NumberTypeConversion.cs /}
{CODE-TAB:csharp:Class tryconvert_class@Indexes/NumberTypeConversion.cs /}
{CODE-TABS/}

---

This next index takes the [`string` field `Employee.Address.PostalCode`](../start/about-examples) 
and attempts to convert it to `long`.  

The query below it finds all employees that do not have a valid `PostalCode` field - whether 
because the employee does not have a postal code or because the value could not be converted to 
a valid `long`.  

{CODE:csharp tryconvert_postal@Indexes/NumberTypeConversion.cs /}
Query:  

{CODE:csharp query@Indexes/NumberTypeConversion.cs /}

{PANEL/}

## Related Articles

### Client API

- [Conventions: Serialization](../client-api/configuration/serialization)

### Indexes

- [Creating and Deploying Indexes](../indexes/creating-and-deploying)

### Server

- [Knowledge Base: Numbers in RavenDB](../server/kb/numbers-in-ravendb)
