# Indexes: Dynamic Fields
---

{NOTE: }

Use `CreateField()` to index dynamic fields which can be created on the fly. 

* In this page:
   * [About Indexing Dynamic Fields](../indexes/using-dynamic-fields#about-indexing-dynamic-fields)
   * [CSharp Example](../indexes/using-dynamic-fields#csharp-example)
   * [CSharp Syntax](../indexes/using-dynamic-fields#csharp-syntax)
   * [JavaScript Index Example](../indexes/using-dynamic-fields#javascript-index-example)

{NOTE/}

{PANEL: About Indexing Dynamic Fields}

While strongly typed entities are well processed by LINQ expressions, some scenarios demand the use of dynamic properties. 

To support searching in object graphs they cannot have the entire structure declared upfront. 

RavenDB exposes an indexing API for creating fields dynamically.

{INFO All types of values are supported by dynamically created fields. They can be numbers, dates, etc. /}

With this feature, you can search for documents using fields which are created on the fly. 

### CSharp Example

For example, consider a `Product` object that is declared as follows:

{CODE dynamic_fields_1@Indexes\DynamicFields.cs /}

Properties such as color or size are added only to some products, while other ones can have the weight and volume defined. 
Since `Attribute` has string fields, they can specify very different properties of products.
In order to query on fields which aren't known at index creation time, we introduced the ability to create them dynamically during indexing.

The following index can be created to index each attribute value under its name as a separate field:

{CODE dynamic_fields_2@Indexes\DynamicFields.cs /}

The `_` character used as the field name in the mapping definition is just a convention. 
You can use any name, it won't be used by the index anyway. The actual field name
that you want to query by is defined in `CreateField(...)`. 
It will generate an index field based on the properties of indexed documents and passed parameters. 

The index can have more fields defined, just like in any other ordinary index.

#### Querying

Looking for products by attributes with the usage of such a defined index is supported as if it were real object properties:

{CODE-TABS}
{CODE-TAB:csharp:Query dynamic_fields_4@Indexes\DynamicFields.cs /}
{CODE-TAB:csharp:DocumentQuery dynamic_fields_3@Indexes\DynamicFields.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: CSharp Syntax}

The signatures are:

{CODE syntax@Indexes\DynamicFields.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **name** | `string` | Name of the dynamic field |
| **value** | `object` | Value of the dynamic field |
| **stored** | `bool` | Sets [FieldStorage](../indexes/storing-data-in-index). By default value is set to `false` which equals to `FieldStorage.No`. |
| **analyzed** | `bool` | Sets [FieldIndexing](../indexes/using-analyzers).<br/><br/>Values:<br/>`null` - `FieldIndexing.Default` (set by overloads without this 'parameter')<br/>`false` - `FieldIndexing.Exact`<br/>`true` - `FieldIndexing.Search` |
| **options** | `CreateFieldOptions` | Dynamic field options |

### Options

| CreateFieldOptions | | |
| ------------- | ------------- | ----- |
| **Indexing** | `FieldIndexing?` | More information about analyzers in index can be found [here](../indexes/using-analyzers). |
| **Storage** | `FieldStorage?` | More information about storing data in index can be found [here](../indexes/storing-data-in-index). |
| **TermVector** | `FieldTermVector?` | More information about term vectors in index can be found [here](../indexes/using-term-vectors). |

{PANEL/}

{PANEL: JavaScript Index Example}

For Node.JS, see the dedicated article by selecting Node.JS at the top of the article. 

A JavaScript index in a C#/.NET environment using the [JavaScript](../indexes/javascript-indexes) version of CreateFields - `createField(name, value, options)`:

{CODE dynamic_fields_JS_index@Indexes\DynamicFields.cs /}

{PANEL/}

## Related Articles

### Indexes

- [Boosting](../indexes/boosting)
- [Analyzers](../indexes/using-analyzers)
- [Storing Data in Index](../indexes/storing-data-in-index)
- [Term Vectors](../indexes/using-term-vectors)
