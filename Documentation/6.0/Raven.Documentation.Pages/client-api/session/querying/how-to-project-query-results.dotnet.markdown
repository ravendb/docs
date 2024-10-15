# Project Query Results
---

{NOTE: }

* Applying a projection in a query allows you to shape the query results to meet specific requirements,  
  delivering just the data needed instead of the original full document content. 

* This article provides examples of projecting query results when making a **dynamic-query**.  
  For projecting results when querying a **static-index** see [project index query results](../../../indexes/querying/projections).

* In this page:

  * [Projections overview](../../../client-api/session/querying/how-to-project-query-results#projections-overview)

  * [Projection Methods](../../../client-api/session/querying/how-to-project-query-results#projection-methods)  
      * [Select](../../../client-api/session/querying/how-to-project-query-results#select)  
      * [ProjectInto](../../../client-api/session/querying/how-to-project-query-results#projectinto)  
      * [SelectFields](../../../client-api/session/querying/how-to-project-query-results#selectfields)  

  * [Single projection per query](../../../client-api/session/querying/how-to-project-query-results#single-projection-per-query)

{NOTE/}

---

{PANEL: Projections overview}

### What are projections:

* A projection refers to the **transformation of query results** into a customized structure,  
  modifying the shape of the data returned by the server.

* Instead of retrieving the full document from the server and then picking relevant data from it on the client,  
  you can request a subset of the data, specifying the document fields you want to get from the server.

* The query can load [related documents](../../../indexes/indexing-related-documents#what-are-related-documents) and have their data merged into the projection results.

* Objects and arrays can be projected, fields can be renamed, and any calculations can be made within the projection.

* Content from inner objects and arrays can be projected.  
  An alias name can be given to the projected fields, and any calculations can be made within the projection.

### When to use projections:

* Projections allow you to tailor the query results specifically to your needs.  
  Getting specific details to display can be useful when presenting data to users or populating user interfaces.  
  Projection queries are also useful with [subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions) 
  since all transformation work is done on the server side without having to send a lot of data over the wire.

* Returning partial document data from the server reduces network traffic,  
  as clients receive only relevant data required for a particular task, enhancing overall performance.  

* Savings can be significant if you need to show just a bit of information from a large document. For example:  
  the size of the result when querying for all "Orders" documents where "Company" is "companies/65-A" is 19KB.  
  Performing the same query and projecting only the "Employee" and "OrderedAt" fields results in only 5KB.

* However, when you need to actively work with the complete set of data on the client side,  
  then do use a query without a projection to retrieve the full document from the server.

### Projections are not tracked by the session:

* On the client side, the resulting projected entities returned by the query are Not tracked by the Session.

* Any modification made to a projection entity will not modify the corresponding document on the server when _SaveChanges_ is called.

### Projections are the final stage in the query pipeline:

* Projections are applied as the last stage in the query pipeline,  
  after the query has been processed, filtered, sorted, and paged.

* This means that the projection does Not apply to all the documents in the collection,  
  but only to the documents matching the query predicate.

* Within the projection you can only filter what data will be returned from the matching documents,  
  but you cannot filter which documents will be returned. That has already been determined earlier in the query pipeline.

* Only a single projection request can be made per Query (and DocumentQuery).  
  Learn more in [single projection per query](../../../client-api/session/querying/how-to-project-query-results#single-projection-per-query).

### The cost of projections:

* Queries in RavenDB do not allow any computation to occur during the query phase.  
  However, you can perform any [calculations](../../../client-api/session/querying/how-to-project-query-results#example---projection-with-calculations) 
  inside the projection.

* But while calculations within a projection are allowed, having a very complex logic can impact query performance.  
  So RavenDB limits the total time it will spend processing a query and its projections.  
  Exceeding this time limit will fail the query. This is configurable, see the following configuration keys:  
      * [Databases.QueryTimeoutInSec](../../../server/configuration/database-configuration#databases.querytimeoutinsec)
      * [Databases.QueryOperationTimeoutInSec](../../../server/configuration/database-configuration#databases.queryoperationtimeoutinsec)

{PANEL/}

## Projection Methods

{PANEL: Select}

* The most common way to perform a query with a projection is to use the `Select` method.  

* You can specify what fields from the document you want to retrieve and even provide a complex expression.

### Example - Projecting individual fields of the document:

{CODE-TABS}
{CODE-TAB:csharp:Query projections_1@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Query_async projections_1_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Companies"
select Name, Address.City as City, Address.Country as Country
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example - Projecting arrays and objects:

{CODE-TABS}
{CODE-TAB:csharp:Query projections_2@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Query_async projections_2_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
// Using simple expression:
from "Orders"
select ShipTo, Lines[].ProductName as ProductNames

// Using JavaScript object literal syntax:
from "Orders" as x
select {
    ShipTo: x.ShipTo, 
    ProductNames: x.Lines.map(y => y.ProductName)
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example - Projection with expression:

{CODE-TABS}
{CODE-TAB:csharp:Query projections_3@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Query_async projections_3_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees" as e
select {
    FullName: e.FirstName + " " + e.LastName
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example - Projection with calculations:

{CODE-TABS}
{CODE-TAB:csharp:Query projections_4@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Query_async projections_4_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Orders" as x
select {
    TotalProducts: x.Lines.length,
    TotalDiscountedProducts: x.Lines.filter(x => x.Discount > 0).length,
    TotalPrice: x.Lines
                  .map(l => l.PricePerUnit * l.Quantity)
                  .reduce((a, b) => a + b, 0)
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example - Projecting using functions:

{CODE-TABS}
{CODE-TAB:csharp:Query projections_5@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Query_async projections_5_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
declare function output(e) {
    var format = p => p.FirstName + " " + p.LastName;
    return { FullName: format(e) };
}
from "Employees" as e select output(e)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example - Projecting using a loaded document:

{CODE-TABS}
{CODE-TAB:csharp:Query projections_6@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Query_async projections_6_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Orders" as o
load o.Company as c
select {
	CompanyName: c.Name,
	ShippedAt: o.ShippedAt
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example - Projection with dates:

{CODE-TABS}
{CODE-TAB:csharp:Query projections_7@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Query_async projections_7_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees" as e 
select { 
    DayOfBirth: new Date(Date.parse(e.Birthday)).getDate(), 
    MonthOfBirth: new Date(Date.parse(e.Birthday)).getMonth() + 1,
    Age: new Date().getFullYear() - new Date(Date.parse(e.Birthday)).getFullYear() 
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example - Projection with raw JavaScript code:

{CODE-TABS}
{CODE-TAB:csharp:Query projections_8@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Query_async projections_8_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees" as e 
select {
    Date: new Date(Date.parse(e.Birthday)), 
    Name: e.FirstName.substr(0, 3)
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example - Projection with metadata:

{CODE-TABS}
{CODE-TAB:csharp:Query projections_9@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Query_async projections_9_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees" as e 
select {
     Name: e.FirstName, 
     Metadata: getMetadata(e)
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: ProjectInto}

* Instead of `Select`, you can use `ProjectInto` to project all public fields from a generic type.
 
* The results will be projected into objects of the specified projection class.

{CODE-TABS}
{CODE-TAB:csharp:Query projections_10@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Query_async projections_10_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Projection_class projections_class@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Companies"
select Name, Phone, Fax
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: SelectFields}

The `SelectFields` method can only be used by a [Document Query](../../../client-api/session/querying/document-query/what-is-document-query).  
It has two overloads:

{CODE-BLOCK: csharp}
// 1) Select fields to project by the projection class type
IDocumentQuery<TProjection> SelectFields<TProjection>();

// 2) Select specific fields to project
IDocumentQuery<TProjection> SelectFields<TProjection>(params string[] fields);
{CODE-BLOCK/}

### Using projection class type:

The projection class fields are the fields that you want to project from the document class.

{CODE-TABS}
{CODE-TAB:csharp:DocumentQuery projections_11@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:DocumentQuery_async projections_11_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Projection_class projections_class@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Companies"
select Name, Phone, Fax
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Using specific fields:

The fields specified are the fields that you want to project from the projection class.

{CODE-TABS}
{CODE-TAB:csharp:DocumentQuery projections_12@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:DocumentQuery_async projections_12_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Projection_class projections_class@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Companies"
select Name, Phone
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Single projection per query}

* As of RavenDB v6.0, only a single projection request can be made per Query (and DocumentQuery).

* Attempting multiple projection executions in the same query will result in an exception.

    * Query:  
      Multiple `Select` calls or a combination of `ProjectInto` with a `Select` call will result in an exception.
   
    * DocumentQuery:  
      Multiple `SelectFields` calls will result in an exception.

{CODE projections_13@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}

{PANEL/}

## Related Articles

### Session

- [Query Overview](../../../client-api/session/querying/how-to-query)
- [How to Stream Query Results](../../../client-api/session/querying/how-to-stream-query-results)

### Indexes

- [Querying an index](../../../indexes/querying/query-index)
- [Project index query results](../../../indexes/querying/projections)

### Server

- [JavaScript Engine](../../../server/kb/javascript-engine)  
