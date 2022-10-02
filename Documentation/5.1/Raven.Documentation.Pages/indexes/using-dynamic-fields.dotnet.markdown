# Indexes: Dynamic Index Fields
---

{NOTE: }

* In RavenDB different documents can have different shapes.  
  Documents are schemaless - new fields can be added or removed as needed.

* For such dynamic data, you can define indexes with __dynamic-index-fields__.
  
* This allows querying the index on fields that aren't yet known at index creation time,  
  which is very useful when working on highly dynamic systems.

* Any value type can be indexed, string, number, date, etc.

* An index definition can contain both dynamic-index-fields and regular-index-fields.

* In this page:

  * [Indexing documents fields KEYS](../indexes/using-dynamic-fields#indexing-documents-fields-keys)
     * [Example - index any field](../indexes/using-dynamic-fields#example---index-any-field)
  * [Indexing documents fields VALUES](../indexes/using-dynamic-fields#indexing-documents-fields-values)
     * [Example - basic](../indexes/using-dynamic-fields#example---basic)
     * [Example - list](../indexes/using-dynamic-fields#example---list)
  * [CreateField syntax](../indexes/using-dynamic-fields#createfield-syntax)

{NOTE/}

{PANEL: Indexing documents fields KEYS}

{NOTE: }
#### Example - index any field

---

The following allows you to:  

* Index any field that is under the 'Attributes' object from the document.  
* After index is deployed, any new field added to the this object will be indexed as well.

---

__The document__:
{CODE:csharp dynamic_fields_1@Indexes\DynamicFields.cs /}

{CODE-BLOCK:json}
// Sample document content
{
    "Attributes": {
        "Color": "Red",
        "Size": 42
    }
}
{CODE-BLOCK/}

__The index__:

* The following index will index any field under the `Attributes` object from the document,  
  a dynamic-index-field will be created for each such field.  
  New fields added to the object after index creation time will be dynamically indexed as well.  

* The actual dynamic-index-field name on which you can query will be the attribute field __key__.  
  e.g. Keys `Color` & `Size` will become the actual dynamic-index-fields.  

{CODE:csharp dynamic_fields_2@Indexes\DynamicFields.cs /}

__The query__:

* You can now query the generated dynamic-index fields.  
  Property `_` is Not queryable, it is only used in the index definition syntax.

* To get all documents with some 'Size' use:

{CODE-TABS}
{CODE-TAB:csharp:DocumentQuery dynamic_fields_3@Indexes\DynamicFields.cs /}
{CODE-TAB-BLOCK:sql:RQL}
// 'Size' is a dynamic-index-field that was indexed from the Attributes object
from index 'Products/ByAttribute' where Size = 42
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Indexing documents fields VALUES}

{NOTE: }
#### Example - basic

---

This example shows:  

  * Only the __basic concept__ of creating a dynamic-index-field from the __value__ of a document field.  
  * Documents can then be queried based on those indexed values.
  * For a more practical usage see the [Example](../indexes/using-dynamic-fields#example---index-a-list-of-properties) below.

---

__The document__:
{CODE:csharp dynamic_fields_4@Indexes\DynamicFields.cs /}

{CODE-BLOCK:json}
// Sample document content
{
    "ProductType": "Electronics",
    "PricePerUnit": 23
}
{CODE-BLOCK/}

__The index__:

* The following index will index the value of document field 'ProductType'.

* This value will be the dynamic-index-field name on which you can query.  
  e.g. Field value `Electronics` will be the dynamic-index-field.

{CODE:csharp dynamic_fields_5@Indexes\DynamicFields.cs /}

__The query__:

* To get all documents of some product type having a specific price per unit use:

{CODE-TABS}
{CODE-TAB:csharp:DocumentQuery dynamic_fields_6@Indexes\DynamicFields.cs /}
{CODE-TAB-BLOCK:sql:RQL}
// 'Electronics' is the dynamic-index-field that was indexed from document field 'ProductType'
from index 'Products/ByName' where Electronics = 23
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE /}

{NOTE: }
#### Example - list

---

The following allows you to:

* Index values from items in a list  
* After index is deployed, any item added this list in the document will be dynamically indexed as well.

---

__The document__:
{CODE:csharp dynamic_fields_7@Indexes\DynamicFields.cs /}

{CODE-BLOCK:json}
// Sample document content
{
    "Name": "Product-A",
    "Attributes": [
       {  
           "PropName": "Color",
           "PropValue": "Blue"
       },
       {
           "PropName": "Width",
           "PropValue": "10"
       },
       {
           "PropName": "Length",
           "PropValue": "20"
       },
       ...
    ]
}
{CODE-BLOCK/}

__The index__:

* The following index will create a dynamic-index-field per item in the document's `Attributes` list.  
  New items added to the Attributes list after index creation time will be dynamically indexed as well.

* The actual dynamic-index-field name on which you can query will be the item's PropName __value__.  
  e.g. 'PropName' value `Width` will be a dynamic-index-field.

{CODE:csharp dynamic_fields_8@Indexes\DynamicFields.cs /}

__The query__:

* To get all documents matching a specific attribute property use:

{CODE-TABS}
{CODE-TAB:csharp:DocumentQuery dynamic_fields_9@Indexes\DynamicFields.cs /}
{CODE-TAB-BLOCK:sql:RQL}
// 'Width' is a dynamic-index-field that was indexed from the Attributes list
from index 'Attributes/ByName' where Width = 10
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE /}
{PANEL/}

{PANEL: CreateField syntax}

{CODE:csharp syntax@Indexes\DynamicFields.cs /}

| Parameters       |                      |                                                                                                                                                                                    |
|------------------|----------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **fieldName**    | `string`             | Name of the dynamic-index-field                                                                                                                                                    |
| **fieldValue**   | `object`             | Value of the dynamic-index-field<br/>The field Terms are derived from this value.                                                                                                  |
| **stored**       | `bool`               | Sets [FieldStorage](../indexes/storing-data-in-index)<br/><br/>`false` - will set `FieldStorage.No` (default value)<br/>`true` - will set `FieldStorate.Yes`                       |
| **analyzed**     | `bool`               | Sets [FieldIndexing](../indexes/using-analyzers)<br/><br/>`null` - `FieldIndexing.Default` (default value)<br/>`false` - `FieldIndexing.Exact`<br/>`true` - `FieldIndexing.Search` |
| **options**      | `CreateFieldOptions` | Dynamic-index-field options                                                                                                                                                        |

| CreateFieldOptions |                    |                                                                            |
|--------------------|--------------------|----------------------------------------------------------------------------|
| **Indexing**       | `FieldIndexing?`   | Learn about [using analyzers](../indexes/using-analyzers) in the index.    |
| **Storage**        | `FieldStorage?`    | Learn about [storing data](../indexes/storing-data-in-index) in the index. |
| **TermVector**     | `FieldTermVector?` | Learn about [term vectors](../indexes/using-term-vectors) in the index.    |

{INFO: }

* All above examples have used the character `_` in the dynamic-index-field definition.  
  However, using it is just a convention. Any other string can be used instead.

* This property is Not queryable, it is only used in the index definition syntax.  
  The actual dynamic-index-fields that are generated are defined by the `CreateField` method.

{INFO /}

{PANEL/}

## Related Articles

### Indexes

- [Boosting](../indexes/boosting)
- [Analyzers](../indexes/using-analyzers)
- [Storing Data in Index](../indexes/storing-data-in-index)
- [Term Vectors](../indexes/using-term-vectors)
