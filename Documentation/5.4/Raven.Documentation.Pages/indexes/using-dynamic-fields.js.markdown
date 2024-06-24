# Indexes: Dynamic Index Fields
---

{NOTE: }

* In RavenDB different documents can have different shapes.  
  Documents are schemaless - new fields can be added or removed as needed.

* For such dynamic data, you can define indexes with **dynamic-index-fields**.

* This allows querying the index on fields that aren't yet known at index creation time,  
  which is very useful when working on highly dynamic systems.

* Any value type can be indexed, string, number, date, etc.

* An index definition can contain both dynamic-index-fields and regular-index-fields.

* In this page:

    * [Indexing documents fields KEYS](../indexes/using-dynamic-fields#indexing-documents-fields-keys)
        * [Example - index any field under object](../indexes/using-dynamic-fields#example---index-any-field-under-object)
        * [Example - index any field](../indexes/using-dynamic-fields#example---index-any-field)
    * [Indexing documents fields VALUES](../indexes/using-dynamic-fields#indexing-documents-fields-values)
        * [Example - basic](../indexes/using-dynamic-fields#example---basic)
        * [Example - list](../indexes/using-dynamic-fields#example---list)
    * [CreateField syntax](../indexes/using-dynamic-fields#createfield-syntax)
    * [Indexed fields & terms view](../indexes/using-dynamic-fields#indexed-fields-&-terms-view)

{NOTE/}

{PANEL: Indexing documents fields KEYS}

{NOTE: }
#### Example - index any field under object

---

The following allows you to:

* Index any field that is under the some object from the document.
* After index is deployed, any new field added to the this object will be indexed as well.

---

**The document**:
{CODE:nodejs dynamic_fields_1@Indexes\DynamicFields.js /}

{CODE-BLOCK:json}
// Sample document content
{
    "attributes": {
        "color": "Red",
        "size": 42
    }
}
{CODE-BLOCK/}

**The index**:

* The following index will index any field under the `attributes` object from the document,  
  a dynamic-index-field will be created for each such field.  
  New fields added to the object after index creation time will be dynamically indexed as well.

* The actual dynamic-index-field name on which you can query will be the attribute field **key**.  
  e.g. Keys `color` & `size` will become the actual dynamic-index-fields.

{CODE-TABS}
{CODE-TAB:nodejs:JavaScript-index dynamic_fields_2_JS@Indexes\DynamicFields.js /}
{CODE-TABS/}

**The query**:

* You can now query the generated dynamic-index fields.  
  Property `_` is Not queryable, it is only used in the index definition syntax.

* To get all documents with some 'size' use:

{CODE-TABS}
{CODE-TAB:nodejs:Query dynamic_fields_3@Indexes\DynamicFields.js /}
{CODE-TAB-BLOCK:sql:RQL}
// 'size' is a dynamic-index-field that was indexed from the attributes object
from index 'Products/ByAttributeKey' where size = 42
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }
#### Example - index any field

---

The following allows you to:

* Define an index on a collection **without** needing any common structure between the indexed documents.
* After index is deployed, any new field added to the document will be indexed as well.

{INFO: }

Consider if that is a true necessity, as indexing every single field can end up costing time and disk space.

{INFO/}

---

**The document**:
{CODE:nodejs dynamic_fields_4@Indexes\DynamicFields.js /}

{CODE-BLOCK:json}
// Sample document content
{
    "firstName": "John",
    "lastName": "Doe",
    "title": "Engineer",
    // ...
}
{CODE-BLOCK/}

**The index**:

* The following index will index any field from the document,  
  a dynamic-index-field will be created for each field.  
  New fields added to the document after index creation time will be dynamically indexed as well.

* The actual dynamic-index-field name on which you can query will be the field **key**.  
  e.g. Keys `firstName` & `lastName` will become the actual dynamic-index-fields.

{CODE-TABS}
{CODE-TAB:nodejs:JavaScript-index dynamic_fields_5_JS@Indexes\DynamicFields.js /}
{CODE-TABS/}

**The query**:

* To get all documents with some 'lastName' use:

{CODE-TABS}
{CODE-TAB:nodejs:Query dynamic_fields_6@Indexes\DynamicFields.js /}
{CODE-TAB-BLOCK:sql:RQL}
// 'lastName' is a dynamic-index-field that was indexed from the document
from index 'Products/ByAnyField/JS' where lastName = "Doe"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Indexing documents fields VALUES}

{NOTE: }
#### Example - basic

---

This example shows:

* Only the **basic concept** of creating a dynamic-index-field from the **value** of a document field.
* Documents can then be queried based on those indexed values.
* For a more practical usage see the [Example](../indexes/using-dynamic-fields#example---index-a-list-of-properties) below.

---

**The document**:
{CODE:nodejs dynamic_fields_7@Indexes\DynamicFields.js /}

{CODE-BLOCK:json}
// Sample document content
{
    "productType": "Electronics",
    "pricePerUnit": 23
}
{CODE-BLOCK/}

**The index**:

* The following index will index the **value** of document field 'productType'.

* This value will be the dynamic-index-field name on which you can query.  
  e.g. Field value `Electronics` will be the dynamic-index-field.

{CODE-TABS}
{CODE-TAB:nodejs:LINQ-index dynamic_fields_8@Indexes\DynamicFields.js /}
{CODE-TAB:nodejs:JavaScript-index dynamic_fields_8_JS@Indexes\DynamicFields.js /}
{CODE-TABS/}

**The query**:

* To get all documents of some product type having a specific price per unit use:

{CODE-TABS}
{CODE-TAB:nodejs:Query dynamic_fields_9@Indexes\DynamicFields.js /}
{CODE-TAB-BLOCK:sql:RQL}
// 'Electronics' is the dynamic-index-field that was indexed from document field 'productType'
from index 'Products/ByProductType' where Electronics = 23
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE /}

{NOTE: }
#### Example - list

---

The following allows you to:

* Index **values** from items in a list
* After index is deployed, any item added this list in the document will be dynamically indexed as well.

---

**The document**:
{CODE:nodejs dynamic_fields_10@Indexes\DynamicFields.js /}

{CODE-BLOCK:json}
// Sample document content
{
    "name": "SomeName",
    "attributes": [
        {  
            "propName": "Color",
            "propValue": "Blue"
        },
        {
            "propName": "Width",
            "propValue": "10"
        },
        {
            "propName": "Length",
            "propValue": "20"
        },
        ...
    ]
}
{CODE-BLOCK/}

**The index**:

* The following index will create a dynamic-index-field per item in the document's `attributes` list.  
  New items added to the attributes list after index creation time will be dynamically indexed as well.

* The actual dynamic-index-field name on which you can query will be the item's propName **value**.  
  e.g. 'propName' value `Width` will be a dynamic-index-field.

{CODE-TABS}
{CODE-TAB:nodejs:LINQ-index dynamic_fields_11@Indexes\DynamicFields.js /}
{CODE-TAB:nodejs:JavaScript-index dynamic_fields_11_JS@Indexes\DynamicFields.js /}
{CODE-TABS/}

**The query**:

* To get all documents matching a specific attribute property use:

{CODE-TABS}
{CODE-TAB:nodejs:Query dynamic_fields_12@Indexes\DynamicFields.js /}
{CODE-TAB-BLOCK:sql:RQL}
// 'Width' is a dynamic-index-field that was indexed from the attributes list
from index 'Attributes/ByName' where Width = 10
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE /}
{PANEL/}

{PANEL: CreateField syntax}

#### Syntax for LINQ-index:

{CODE:csharp syntax@Indexes\DynamicFields.cs /}

#### Syntax for JavaScript-index:

{CODE:nodejs syntax_JS@Indexes\DynamicFields.js /}

| Parameters       |                      |                                                                                                                                                                                    |
|------------------|----------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **fieldName**    | `string`             | Name of the dynamic-index-field                                                                                                                                                    |
| **fieldValue**   | `object`             | Value of the dynamic-index-field<br/>The field Terms are derived from this value.                                                                                                  |
| **stored**       | `bool`               | Sets [FieldStorage](../indexes/storing-data-in-index)<br/><br/>`false` - will set `FieldStorage.No` (default value)<br/>`true` - will set `FieldStorate.Yes`                       |
| **analyzed**     | `bool`               | Sets [FieldIndexing](../indexes/using-analyzers)<br/><br/>`null` - `FieldIndexing.Default` (default value)<br/>`false` - `FieldIndexing.Exact`<br/>`true` - `FieldIndexing.Search` |
| **options**      | `CreateFieldOptions` | Dynamic-index-field options                                                                                                                                                        |

| CreateFieldOptions |                    |                                                                            |
|--------------------|--------------------|----------------------------------------------------------------------------|
| **Storage**        | `FieldStorage?`    | Learn about [storing data](../indexes/storing-data-in-index) in the index. |
| **Indexing**       | `FieldIndexing?`   | Learn about [using analyzers](../indexes/using-analyzers) in the index.    |
| **TermVector**     | `FieldTermVector?` | Learn about [term vectors](../indexes/using-term-vectors) in the index.    |

{INFO: }

* All above examples have used the character `_` in the dynamic-index-field definition.  
  However, using `_` is just a convention. Any other string can be used instead.

* This property is Not queryable, it is only used in the index definition syntax.  
  The actual dynamic-index-fields that are generated are defined by the `CreateField` method.

{INFO /}

{PANEL/}

{PANEL: Indexed fields & terms view}

The generated dynamic-index-fields and their indexed terms can be viewed in the **Terms View**.  
Below are sample index fields & their terms generated from the last example.

![Figure 1. Go to terms view](images/dynamic-index-fields-1.png "Figure-1: Go to Terms View")

![Figure 2. Indexed fields & terms](images/dynamic-index-fields-2.png "Figure-2: Indexed fields & terms")

{PANEL/}

## Related Articles

### Indexes

- [Boosting](../indexes/boosting)
- [Analyzers](../indexes/using-analyzers)
- [Storing Data in Index](../indexes/storing-data-in-index)
- [Term Vectors](../indexes/using-term-vectors)
