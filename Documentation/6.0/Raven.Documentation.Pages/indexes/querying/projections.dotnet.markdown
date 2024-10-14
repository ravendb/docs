# Project Index Query Results
---

{NOTE: }

* This article provides examples of projecting query results when querying a **static-index**.

* Prior to reading this article, please refer to [query results projection overview](../../client-api/session/querying/how-to-project-query-results) 
  for general knowledge about Projections and for dynamic-queries examples.  

* In this page:  
    * [Projection Methods](../../indexes/querying/projections#select):  
      * [Select](../../indexes/querying/projections#select)  
      * [ProjectInto](../../indexes/querying/projections#projectinto)  
      * [SelectFields](../../indexes/querying/projections#selectfields)  
    * [Projection behavior with a static-index](../../indexes/querying/projections#projection-behavior-with-a-static-index)  
    * [OfType](../../indexes/querying/projections#oftype)  

{NOTE/}

---

{PANEL: Select}

{NOTE: }

**Example I - Projecting individual fields of the document**:

{CODE-TABS}
{CODE-TAB:csharp:Query projections_1@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Query_async projections_1_async@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index index_1@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByNameAndTitle"
where Title == "sales representative"
select FirstName as EmployeeFirstName, LastName as EmployeeLastName
{CODE-TAB-BLOCK/}
{CODE-TABS/}

* **Type of projection fields**:  

  * In the above example, the fields to return by the projection that are specified in the `Select` method  
    (`x.FirstName` & `x.LastName`) are recognized by the compiler as fields of the `IndexEntry` class.
  
  * If you wish to specify fields from the original 'Employee' class type then follow [this example](../../indexes/querying/projections#oftype) that uses `OfType`.  

* **Source of projection fields**:  

  * Since the index-fields in this example are not [Stored in the index](../../indexes/storing-data-in-index), and no projection behavior was defined,  
    resulting values for `FirstName` & `LastName` will be retrieved from the matching Employee document in the storage.
  
  * This behavior can be modified by setting the [projection behavior](../../indexes/querying/projections#projection-behavior-with-a-static-index) used when querying a static-index.

{NOTE/}

{NOTE: }

**Example II - Projecting stored fields**:

{CODE-TABS}
{CODE-TAB:csharp:Query projections_1_stored@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Query_async projections_1_stored_async@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index index_1_stored@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByNameAndTitleWithStoredFields"
select FirstName as EmployeeFirstName, LastName as EmployeeLastName
{CODE-TAB-BLOCK/}
{CODE-TABS/}

* In this example, the projected fields (`FirstName` and `LastName`) are stored in the index,  
  so by default, the resulting values will come directly from the index and Not from the Employee document in the storage.

* This behavior can be modified by setting the [projection behavior](../../indexes/querying/projections#projection-behavior-with-a-static-index) used when querying a static-index.
{NOTE/}

{NOTE: }

**Example III - Projecting arrays and objects**:

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

**Example IV - Projection with expression**:

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

**Example V - Projection with calculations**:

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

**Example VI - Projecting using functions**:

{CODE-TABS}
{CODE-TAB:csharp:Query projections_5@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Query_async projections_5_async@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index index_1@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
declare function output(x) {
var format = p => p.FirstName + " " + p.LastName;
    return { FullName: format(x) };
}
from index "Employees/ByNameAndTitle" as e
select output(e)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Example VII - Projecting using a loaded document**:

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

**Example VIII - Projection with dates**:

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

**Example IX - Projection with raw JavaScript code**:

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

**Example X - Projection with metadata**:

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

**Using projection class type**:

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

**Using specific fields**:

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

* **By default**, when querying a static-index and projecting query results,  
  the server will try to retrieve the fields' values from the fields [stored in the index](../../indexes/storing-data-in-index).  
  If the index does Not store those fields then the fields' values will be retrieved from the documents.

* This behavior can be modified by setting the **projection behavior**.

* Note: Storing fields in the index can increase query performance when projecting,  
  but this comes at the expense of the disk space used by the index.

{NOTE: }

**Example**:

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

**Syntax for projection behavior**:

{CODE:csharp projection_behavior syntax@Indexes\Querying\Projections.cs /}

* `Default`  
  Retrieve values from the stored index fields when available.  
  If fields are not stored then get values from the document,  
  a field that is not found in the document is skipped.

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

{PANEL: OfType}

* When making a projection query, converting the shape of the matching documents to the requested projection is done on the **server-side**.

* On the other hand, `OfType` is a **client-side**  type conversion that is only used to map the resulting objects to the provided type.

* We differentiate between the following cases:  
  * Using _OfType_ with projection queries - resulting objects are Not tracked by the session  
  * Using _OfType_ with non-projection queries - resulting documents are tracked by the session

{NOTE: }

**Using OfType with projection queries**:

{CODE-TABS}
{CODE-TAB:csharp:Query projections_15@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Query_async projections_15_async@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index index_5@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Companies/ByContactDetailsAndPhone"
where ContactTitle == "owner"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Using OfType with non-projection queries**:

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
