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

## Example - index any field under object

{NOTE: The following example allows you to:}

* Index any field that is under the some object from the document.  
* After index is deployed, any new field added to the this object will be indexed as well.

{NOTE/}

* **The document**:  
  {CODE:php dynamic_fields_1@Indexes\DynamicFields.php /}
  {CODE-BLOCK:json}
// Sample document content
{
    "Attributes": {
        "Color": "Red",
        "Size": 42
    }
}
{CODE-BLOCK/}

* **The index**:  
  The below index will index any field under the `Attributes` object from the document,  
  a dynamic-index-field will be created for each such field.  
  New fields added to the object after index creation time will be dynamically indexed as well.  
  
     The actual dynamic-index-field name on which you can query will be the attribute field **key**.  
     E.g., Keys `Color` & `Size` will become the actual dynamic-index-fields.  

     {CODE-TABS}
     {CODE-TAB:php:Index dynamic_fields_2@Indexes\DynamicFields.php /}
     {CODE-TAB:php:JavaScript-index dynamic_fields_2_JS@Indexes\DynamicFields.php /}
     {CODE-TABS/}


* **The query**:  
   * You can now query the generated dynamic-index fields.  
   * To get all documents with some 'size' use:
     {CODE-TABS}
{CODE-TAB:php:Query dynamic_fields_3@Indexes\DynamicFields.php /}
{CODE-TAB-BLOCK:sql:RQL}
// 'Size' is a dynamic-index-field that was indexed from the Attributes object
from index 'Products/ByAttributeKey' where Size = 42
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Example - index any field

{NOTE: The following example allows you to:}

  * Define an index on a collection **without** needing any common structure between the indexed documents.  
  * After index is deployed, any new field added to the document will be indexed as well.  

{NOTE/}

{INFO: }
Consider whether this is really necessary, as indexing every single field can end up costing time and disk space.  
{INFO/}

* **The document**:  
  {CODE:php dynamic_fields_4@Indexes\DynamicFields.php /}

     {CODE-BLOCK:json}
// Sample document content
    {
        "FirstName": "John",
        "LastName": "Doe",
        "Title": "Engineer",
        // ...
}
{CODE-BLOCK/}

* **The index**:  
  The below index will index any field from the document,  
  a dynamic-index-field will be created for each field.  
  New fields added to the document after index creation time will be dynamically indexed as well.  
  
     The actual dynamic-index-field name on which you can query will be the field **key**.  
     E.g., Keys `FirstName` & `LastName` will become the actual dynamic-index-fields.  

     {CODE-TABS}
     {CODE-TAB:php:JavaScript-index dynamic_fields_5_JS@Indexes\DynamicFields.php /}
     {CODE-TABS/}

* **The query**:
   * To get all documents with some 'LastName' use:
     {CODE-TABS}
{CODE-TAB:php:Query dynamic_fields_6@Indexes\DynamicFields.php /}
{CODE-TAB-BLOCK:sql:RQL}
// 'LastName' is a dynamic-index-field that was indexed from the document
from index 'Products/ByAnyField/JS' where LastName = "Doe"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Indexing documents fields VALUES}

## Example - basic

{NOTE: The following example shows:}

* Only the **basic concept** of creating a dynamic-index-field from the **value** of a document field.  
* Documents can then be queried based on those indexed values.  

{NOTE/}

* **The document**:  
  {CODE:php dynamic_fields_7@Indexes\DynamicFields.php /}

     {CODE-BLOCK:json}
// Sample document content
{
    "ProductType": "Electronics",
    "PricePerUnit": 23
}
{CODE-BLOCK/}

* **The index**:  
  The below index will index the **value** of document field 'ProductType'.  
  
     This value will be the dynamic-index-field name on which you can query.  
     E.g., Field value `Electronics` will be the dynamic-index-field.  

     {CODE-TABS}
     {CODE-TAB:php:Index dynamic_fields_8@Indexes\DynamicFields.php /}
     {CODE-TAB:php:JavaScript-index dynamic_fields_8_JS@Indexes\DynamicFields.php /}
     {CODE-TABS/}

* **The query**:  
   * To get all documents of some product type having a specific price per unit use:
     {CODE-TABS}
{CODE-TAB:php:Query dynamic_fields_9@Indexes\DynamicFields.php /}
{CODE-TAB-BLOCK:sql:RQL}
// 'Electronics' is the dynamic-index-field that was indexed from document field 'ProductType'
from index 'Products/ByProductType' where Electronics = 23
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Example - list

{NOTE: The following example allows you to:}

* Index **values** from items in a list
* After index is deployed, any item added this list in the document will be dynamically indexed as well.

{NOTE/}

* **The document**:  
  {CODE:php dynamic_fields_10@Indexes\DynamicFields.php /}

     {CODE-BLOCK:json}
     // Sample document content
{
    "Name": "SomeName",
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

* **The index**:  
  The below index will create a dynamic-index-field per item in the document's `Attributes` list.  
  New items added to the Attributes list after index creation time will be dynamically indexed as well.  
  
     The actual dynamic-index-field name on which you can query will be the item's PropName **value**.  
     E.g., 'PropName' value `width` will be a dynamic-index-field.  

     {CODE-TABS}
     {CODE-TAB:php:Index dynamic_fields_11@Indexes\DynamicFields.php /}
     {CODE-TAB:php:JavaScript-index dynamic_fields_11_JS@Indexes\DynamicFields.php /}
     {CODE-TABS/}

* **The query**:  
  To get all documents matching a specific attribute property use:
  {CODE-TABS}
{CODE-TAB:php:Query dynamic_fields_12@Indexes\DynamicFields.php /}
{CODE-TAB-BLOCK:sql:RQL}
// 'Width' is a dynamic-index-field that was indexed from the Attributes list
from index 'Attributes/ByName' where Width = 10
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: CreateField syntax}

{CODE:php syntax@Indexes\DynamicFields.php /}

| Parameters   |                      |                                                                                                                                                                                    |
|--------------|----------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **name**     | `string`             | Name of the dynamic-index-field                                                                                                                                                    |
| **value**    | `object`             | Value of the dynamic-index-field<br/>The field Terms are derived from this value.                                                                                                  |
| **stored**   | `bool`               | Sets [FieldStorage](../indexes/storing-data-in-index)<br/><br/>`False` - will set `FieldStorage.NO` (default value)<br/>`True` - will set `FieldStorate.YES`                       |
| **analyzed** | `bool`               | Sets [FieldIndexing](../indexes/using-analyzers)<br/><br/>`None` - `FieldIndexing.Default` (default value)<br/>`False` - `FieldIndexing.Exact`<br/>`True` - `FieldIndexing.Search` |
| **options**  | `CreateFieldOptions` | Dynamic-index-field options                                                                                                                                                        |

| CreateFieldOptions |                    |                                                                            |
|--------------------|--------------------|----------------------------------------------------------------------------|
| **Storage** | `FieldStorage` | Learn about [storing data](../indexes/storing-data-in-index) in the index. |
| **Indexing** | `FieldIndexing` | |
| **TermVector** | `FieldTermVector` | |

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
