# Project Index Query Results
---

{NOTE: }

* This article provides examples of projecting query results when querying a static-index.

* __Prior to this article__, please refer to [Project query results - Overview](../../client-api/session/querying/how-to-project-query-results)  
  for general knowledge about Projections and dynamic-queries examples.  

* In this page:  

    * [Projection Methods](../../indexes/querying/projections#select):  
        * [Select](../../indexes/querying/projections#select)  
        * [ProjectInto](../../indexes/querying/projections#projectinto)  
        * [SelectFields](../../indexes/querying/projections#selectfields)  
        
    * [Projection behavior with a static-index](../../indexes/querying/projections#projection-behavior-with-a-static-index)  
  
    * [OfType (As)](../../indexes/querying/projections#oftype-(as))  

{NOTE/}

---

{PANEL: Select}

{NOTE: }

__Example I - Projecting individual fields of the document__:

{CODE-TABS}
{CODE-TAB:csharp:Query projections_1@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Query_async projections_1_async@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index index_1@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByNameAndTitle"
where Title = "sales representative"
select FirstName, LastName
{CODE-TAB-BLOCK/}
{CODE-TABS/}

* Since the index-fields in this example are not [Stored in the index](../../indexes/storing-data-in-index), and no projection behavior was defined,  
  resulting values will be retrieved from the matching Employee document in the storage.
  
* This behavior can be modified by setting the [projection behavior](../../indexes/querying/projections#projection-behavior-with-a-static-index) used when querying a static-index.

{NOTE/}

{NOTE: }

__Example II - Projecting stored fields__:

{CODE-TABS}
{CODE-TAB:csharp:Query projections_1_stored@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Query_async projections_1_stored_async@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index index_1_stored@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByNameAndTitleWithStoredFields"
select FirstName, LastName
{CODE-TAB-BLOCK/}
{CODE-TABS/}

* In this example, all projected index-fields (`FirstName` and `LastName`) are stored in the index,  
  so by default, the resulting values will come directly from the index and not from the Employee document in the storage.

* This behavior can be modified by setting the [projection behavior](../../indexes/querying/projections#projection-behavior-with-a-static-index) used when querying a static-index.
{NOTE/}

{NOTE: }

__Example III - Projecting arrays and objects__:

{CODE-TABS}
{CODE-TAB:csharp:Query projections_2@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Query_async projections_2_async@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index index_2@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
// Using simple expression:
from index "Orders/ByCompanyAndShipToAndLines"
where Company == "companies/65-A"
select ShipTo.City as ShipToCity, Lines[].ProductName as Products

// Using JavaScript object literal syntax:
from index "Orders/ByCompanyAndShipToAndLines" as x
where Company == "companies/65-A"
select {
    ShipToCity: x.ShipTo.City,
    Products: x.Lines.map(y => y.ProductName)
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__Example IV - Projection with expression__:

{CODE-TABS}
{CODE-TAB:csharp:Query projections_3@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Query_async projections_3_async@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index index_1@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByNameAndTitle" as x
select 
{ 
    FullName : x.FirstName + " " + x.LastName 
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__Example V - Projection with calculations__:

{CODE-TABS}
{CODE-TAB:csharp:Query projections_4@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Query_async projections_4_async@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index index_2@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Orders/ByCompanyAndShipToAndLines" as x
select {
    TotalProducts: x.Lines.length,
    TotalDiscountedProducts: x.Lines.filter(x => x.Discount > 0).length,
    TotalPrice: x.Lines
                 .map(l => l.PricePerUnit * l.Quantity)
                 .reduce((a, b) => a + b, 0)
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__Example VI - Projecting using functions__:

{CODE-TABS}
{CODE-TAB:csharp:Query projections_5@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Query_async projections_5_async@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index index_1@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
declare function output(x) {
var format = p => p.FirstName + " " + p.LastName;
    return { FullName: format(x) };
}
from index "Employees/ByNameAndTitle" as e select output(e)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__Example VII - Projecting using a loaded document__:

{CODE-TABS}
{CODE-TAB:csharp:Query projections_6@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Query_async projections_6_async@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index index_3@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Orders/ByCompanyAndShippedAt" as o
load o.Company as c
select {
	CompanyName: c.Name,
	ShippedAt: o.ShippedAt
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__Example VIII - Projection with dates__:

{CODE-TABS}
{CODE-TAB:csharp:Query projections_7@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Query_async projections_7_async@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index index_4@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByFirstNameAndBirthday" as x 
select { 
    DayOfBirth: new Date(Date.parse(x.Birthday)).getDate(), 
    MonthOfBirth: new Date(Date.parse(x.Birthday)).getMonth() + 1,
    Age: new Date().getFullYear() - new Date(Date.parse(x.Birthday)).getFullYear() 
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__Example IX - Projection with raw JavaScript code__:

{CODE-TABS}
{CODE-TAB:csharp:Query projections_8@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Query_async projections_8_async@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index index_4@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByFirstNameAndBirthday" as x 
select {
    Date: new Date(Date.parse(x.Birthday)), 
    Name: x.FirstName.substr(0,3)
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__Example X - Projection with metadata__:

{CODE-TABS}
{CODE-TAB:csharp:Query projections_9@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Query_async projections_9_async@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index index_4@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByFirstNameAndBirthday" as x 
select {
     Name : x.FirstName, 
     Metadata : getMetadata(x)
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}
{PANEL/}

{PANEL: ProjectInto}

* Instead of `Select`, you can use `ProjectInto` to project all public fields from a generic type.

* The results will be projected into objects of the specified projection class.

{NOTE: }

{CODE-TABS}
{CODE-TAB:csharp:Query projections_10@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Query_async projections_10_async@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index index_5@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Projection_class projections_class_5@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Companies/ByContactDetailsAndPhone"
where ContactTitle == "owner"
select ContactName, ContactTitle
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}
{PANEL/}

{PANEL: SelectFields}

The `SelectFields` method can only be used by a [Document Query](../../client-api/session/querying/document-query/what-is-document-query).  
It has two overloads:

{CODE-BLOCK: csharp}
// 1) Select fields to project by the projection class type
IDocumentQuery<TProjection> SelectFields<TProjection>();

// 2) Select specific fields to project
IDocumentQuery<TProjection> SelectFields<TProjection>(params string[] fields);
{CODE-BLOCK/}

{NOTE: }

__Using projection class type__:

* The projection class fields are the fields that you want to project from the 'IndexEntry' class.

{CODE-TABS}
{CODE-TAB:csharp:DocumentQuery projections_11@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:DocumentQuery_async projections_11_async@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index index_6@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Projection_class projections_class_6@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Products/ByNamePriceQuantityAndUnits"
select ProductName, PricePerUnit, UnitsInStock
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__Using specific fields__:

* The fields specified are the fields that you want to project from the projection class.

{CODE-TABS}
{CODE-TAB:csharp:DocumentQuery projections_12@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:DocumentQuery_async projections_12_async@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index index_6@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Projection_class projections_class_6@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Companies/ByContactDetailsAndPhone" 
select ProductName, PricePerUnit
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Projection behavior with a static-index}

* __By default__, when querying a static-index and projecting query results,  
  the server will try to retrieve the fields' values from the fields [stored in the index](../../indexes/storing-data-in-index).  
  If the index does Not store those fields then the fields' values will be retrieved from the documents.

* This behavior can be modified by setting the __projection behavior__.

* Note: Storing fields in the index can increase query performance when projecting,  
  but this comes at the expense of the disk space used by the index.

{NOTE: }

__Example__

{CODE-TABS}
{CODE-TAB:csharp:Query projections_13_1@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:DocumentQuery projections_13_2@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:RawQuery projections_13_3@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index index_1_stored@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Projection_class projections_class_1@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByNameAndTitleWithStoredFields"
select FirstName, Title
{CODE-TAB-BLOCK/}
{CODE-TABS/}

The projection behavior in the above example is set to `FromIndexOrThrow` and so the following applies: 

* Field `FirstName` is stored in the index so the server will fetch its values from the index.

* However, field `Title` is Not stored in the index so an exception will be thrown when the query is executed.

{NOTE/}

{NOTE: }

__Syntax__

{CODE:csharp projection_behavior syntax@Indexes\Querying\Projections.cs /}

* `Default`  
  Retrieve values from the stored index fields when available.  
  If fields are not stored then get values from the document.
* `FromIndex`  
  Retrieve values from the stored index fields when available.  
  A field that is not stored in the index is skipped.
* `FromIndexOrThrow`  
  Retrieve values from the stored index fields when available.  
  An exception is thrown if the index does not store the requested field.
* `FromDocument`  
  Retrieve values directly from the documents store.  
  A field that is not found in the document is skipped.
* `FromDocumentOrThrow`  
  Retrieve values directly from the documents store.  
  An exception is thrown if the document does not contain the requested field.

{NOTE/}

{PANEL/}

{PANEL: OfType (As)}

* __Projection queries__:  
  When querying an index with a projection (as described in this article), 
  the server searches for documents that match the query predicate, but the returned objects follow the shape of the specified projection - they are not the actual documents.  

* __Non-Projection queries__:  
  When querying an index without using a projection, the server returns the actual documents that match the query conditions. 
  However, when filtering such a query by the index-entry fields, a type conversion is required for the compiler to understand the resulting objects' shape. 
  This is where _OfType_ (or _As_) comes into play as it.

* __Role of `OfType` (or `As`)__:  
  So this is just a client-side type conversion used to map the query results to the document type.  
  It ensures the compiler recognizes that the resulting objects conform to the expected document shape.

* __Results are tracked__:  
  As opposed to projection queries where results are not tracked by the session,  
  In the case of non-projecting queries that use _OfType_, the session does track the resulting document entities.

{NOTE: }

{CODE-TABS}
{CODE-TAB:csharp:Query projections_14@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Query_async projections_14_async@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index index_5@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Companies/ByContactDetailsAndPhone"
where ContactTitle == "owner"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}
{PANEL/}

## Related Articles

### Querying 

- [Query overview](../../client-api/session/querying/how-to-query)
- [Project dynamic query results](../../client-api/session/querying/how-to-project-query-results)

### Indexes

- [Querying an index](../../indexes/querying/query-index)

### Knowledge Base

- [JavaScript engine](../../server/kb/javascript-engine)
