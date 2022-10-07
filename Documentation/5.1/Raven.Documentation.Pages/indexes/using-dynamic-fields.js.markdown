# Indexes: Dynamic Fields

{NOTE: }

* In RavenDB different documents can have different shapes.  
  Documents are schemaless - new fields can be added or removed as needed.

* For such dynamic data, you can define indexes with __dynamic-index-fields__.

* This allows querying the index on fields that aren't yet known at index creation time,  
  which is very useful when working on highly dynamic systems.

* Any value type can be indexed, string, number, date, etc.

* An index definition can contain both dynamic-index-fields and regular-index-fields.

{NOTE/}

---

RavenDB exposes an indexing API for creating fields dynamically.

With this feature, you can search for documents using fields which are created on the fly.  
For example, consider a `Product` object that is declared as follows:

{CODE:nodejs dynamic_fields_1@indexes\dynamicFields.js /}

Properties such as color or size are added only to some products, while other ones can have the weight and volume defined. Since `Attribute` has string fields, they can specify very different properties of products.
In order to query on fields which aren't known at index creation time, we introduced the ability to create them dynamically during indexing.

The following index can be created in order to index each attribute value under its name as a separate field:

{CODE:nodejs dynamic_fields_2@indexes\dynamicFields.js /}

The `_` character used as the field name in the mapping definition is just a convention. You can use any name, it won't be used by the index anyway. The actual field name
that you want to query by is defined in `CreateField(...)`. It will generate an index field based on the properties of indexed documents and passed parameters 

The index can have more fields defined, just like in any other ordinary index.

## Syntax

{CODE:nodejs syntax@indexes\dynamicFields.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **name** | `string` | Name of the dynamic field |
| **value** | `object` | Value of the dynamic field |
| **stored** | `boolean` | Sets [FieldStorage](../indexes/storing-data-in-index). By default value is set to `false` which equals to `FieldStorage.No`. |
| **analyzed** | `boolean` | Sets [FieldIndexing](../indexes/using-analyzers).<br/><br/>Values:<br/>`null` - `FieldIndexing.Default` (set by overloads without this 'parameter')<br/>`false` - `FieldIndexing.Exact`<br/>`true` - `FieldIndexing.Search` |
| **options** | `CreateFieldOptions` | Dynamic field options |

### Options

| CreateFieldOptions | | |
| ------------- | ------------- | ----- |
| **Storage** | `FieldStorage?` | More information about storing data in index can be found [here](../indexes/storing-data-in-index). |
| **Indexing** | `FieldIndexing?` | More information about analyzers in index can be found [here](../indexes/using-analyzers). |
| **TermVector** | `FieldTermVector?` | More information about term vectors in index can be found [here](../indexes/using-term-vectors). |

#### Querying

Looking for products by attributes with the usage of such a defined index is supported as if it were real object properties:

{CODE:nodejs dynamic_fields_4@indexes\dynamicFields.js /}


## Related Articles

### Indexes

- [Boosting](../indexes/boosting)
- [Analyzers](../indexes/using-analyzers)
- [Storing Data in Index](../indexes/storing-data-in-index)
- [Term Vectors](../indexes/using-term-vectors)
