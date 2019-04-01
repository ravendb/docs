# Indexes: Dynamic Fields

While strongly typed entities are well processed by LINQ expressions, some scenarios demand the use of dynamic properties. 

To support searching in object graphs they cannot have the entire structure declared upfront. 

RavenDB exposes an indexing API for creating fields dynamically.

With this feature, you can search for documents using fields which are created on the fly. For example, consider a `Product` object that is declared as follows:

{CODE dynamic_fields_1@Indexes\DynamicFields.cs /}

Properties such as color or size are added only to some products, while other ones can have the weight and volume defined. Since `Attribute` has string fields, they can specify very different properties of products.
In order to query on fields which aren't known at index creation time, we introduced the ability to create them dynamically during indexing.

The following index can be created in order to index each attribute value under its name as a separate field:

{CODE dynamic_fields_2@Indexes\DynamicFields.cs /}

The `_` character used as the field name in the mapping definition is just a convention. You can use any name, it won't be used by the index anyway. The actual field name
that you want to query by is defined in `CreateField(...)`. It will generate an index field based on the properties of indexed documents and passed parameters 

The index can have more fields defined, just like in any other ordinary index.

{INFO: Options}
Field options like `FieldStorage` and `FieldIndexing` are configurable by arguments of the `CreateField` method:   

  * stored   
    * false (default) - `FieldStorage.No`   
    * true - `FieldStorage.Yes`   
  * analyzed   
    * null (default) - `FieldIndexing.Default`   
    * true - `FieldIndexing.Search`   
    * false - `FieldIndexing.Exact`  
{INFO/}

Looking for products by attributes with the usage of such a defined index is supported as if it were real object properties:

{CODE-TABS}
{CODE-TAB:csharp:Query dynamic_fields_4@Indexes\DynamicFields.cs /}
{CODE-TAB:csharp:DocumentQuery dynamic_fields_3@Indexes\DynamicFields.cs /}
{CODE-TABS/}

{INFO All types of values are supported by dynamically created fields. They can be numbers, dates, etc. /}

## Related Articles

### Indexes

- [Boosting](../indexes/boosting)
- [Analyzers](../indexes/using-analyzers)
- [Storing Data in Index](../indexes/storing-data-in-index)
- [Term Vectors](../indexes/using-term-vectors)
