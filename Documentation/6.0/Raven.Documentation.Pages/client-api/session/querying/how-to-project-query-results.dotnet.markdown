# Project Query Results
---

{NOTE: }

* Applying a projection in a query allows you to shape the query results to meet specific requirements,  
  delivering just the data needed instead of the original full document content. 

* In this page:

  * [Projections overview](../../../client-api/session/querying/how-to-project-query-results#overview)

  * [Projection Methods](../../../client-api/session/querying/how-to-project-query-results#select):  
      * [Select](../../../client-api/session/querying/how-to-project-query-results#select)  
      * [SelectFields](../../../client-api/session/querying/how-to-project-query-results#selectfields)  
      * [ProjectInto](../../../client-api/session/querying/how-to-project-query-results#projectinto)   

  * [Single projection usage](../../../)

{NOTE/}

---

{PANEL: Projections overview}

---

__What are projections__:

* A projection refers to the __transformation of query results__ into a customized structure,  
  modifying the shape of the data returned from the server:
  
      * You can return a subset of the data, specifying the document fields you want to get from the server,  
        instead of retrieving the full document and then picking relevant data from it.  

      * The query can load [related documents](../../../indexes/indexing-related-documents#what-are-related-documents) and have their data merged into the projection results.

      * Objects and arrays can be projected, fields can be renamed, and any calculation can be made within the projection.

---

__When to use projections__:

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

---

__Projections are not tracked by the session__:

* On the client side, the resulting projected entities returned by the query are not tracked by the Session.

* Any modification made to a projection entity will not modify the corresponding document on the server when SaveChanges is called.

---

__Projections are the final stage in the query pipeline__:

* Projections are applied as the last stage in the query pipeline,  
  after the query has been processed, filtered, sorted, and paged.

* This means that the projection does Not apply to all the documents in the collection,  
  but only to the documents matching the query predicate.

* Within the projection you can only filter what data will be returned from the matching documents,  
  but you cannot filter which documents will be returned. That has already been determined earlier in the query pipeline.

---

__The cost of projections__:

* While computations within a projection are allowed, having a very complex logic can impact query performance.

* RavenDB limits the total time it will spend processing a query and its projections.  
  Exceeding this time limit will fail the query. This is configurable, see the following configuration keys:  
      * [Databases.QueryTimeoutInSec](../../..server/configuration/database-configuration#databases.querytimeoutinsec)
      * [Databases.QueryOperationTimeoutInSec](../../../server/configuration/database-configuration#databases.queryoperationtimeoutinsec)

{PANEL/}

{PANEL: Select}

The most common way to perform a query with a projection is to use the `Select` method.  
You can specify what fields from a document you want to retrieve and even provide a complex expression.

### Example I - Projecting Individual Fields of the Document

{CODE-TABS}
{CODE-TAB:csharp:Sync projections_1@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Async projections_1_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Companies
select Name, Address.City as City, Address.Country as Country
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example II - Projecting Arrays and Objects

{CODE-TABS}
{CODE-TAB:csharp:Sync projections_2@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Async projections_2_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders
select ShipTo, Lines[].ProductName as Products
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example III - Projection with Expression

{CODE-TABS}
{CODE-TAB:csharp:Sync projections_3@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Async projections_3_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Employees as e
select {
    FullName : e.FirstName + " " + e.LastName
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example IV - Projection with `let`

{CODE-TABS}
{CODE-TAB:csharp:Sync projections_12@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Async projections_12_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
declare function output(e) {
	var format = function(p){ return p.FirstName + " " + p.LastName; };
	return { FullName : format(e) };
}
from Employees as e select output(e)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example V - Projection with Calculation

Queries in RavenDB do not allow any computation to occur during the query phase. 
This is done to ensure that queries in RavenDB can always use an index to answer the query promptly and efficiently. 
RQL does allow you to perform computation inside the select,
either to project specific fields out or to massage the data from the query in every way imaginable

{CODE-TABS}
{CODE-TAB:csharp:Sync projections_4@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Async projections_4_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders as o
select {
    Total : o.Lines.reduce(
        (acc , l) => acc += l.PricePerUnit * l.Quantity, 0)
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example VI - Projection Using a Loaded Document

{CODE-TABS}
{CODE-TAB:csharp:Sync projections_5@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Async projections_5_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders as o
load o.Company as c
select {
	CompanyName: c.Name,
	ShippedAt: o.ShippedAt
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example VII - Projection with Dates

{CODE-TABS}
{CODE-TAB:csharp:Sync projections_6@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Async projections_6_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Employees as e 
select { 
    DayOfBirth : new Date(Date.parse(e.Birthday)).getDate(), 
    MonthOfBirth : new Date(Date.parse(e.Birthday)).getMonth() + 1,
    Age : new Date().getFullYear() - new Date(Date.parse(e.Birthday)).getFullYear() 
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example VIII - Projection with Raw JavaScript Code

{CODE-TABS}
{CODE-TAB:csharp:Sync projections_7@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Async projections_7_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Employees as e 
select {
    Date : new Date(Date.parse(e.Birthday)), 
    Name : e.FirstName.substr(0,3)
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example IX - Projection with Metadata

{CODE-TABS}
{CODE-TAB:csharp:Sync projections_13@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Async projections_13_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Employees as e 
select {
     Name : e.FirstName, 
     Metadata : getMetadata(e)
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL:SelectFields}

The `SelectFields` method can only be used with the [Document Query](../../../client-api/session/querying/document-query/what-is-document-query). 
It has two overloads:

{CODE-BLOCK: csharp}
// 1) By array of fields
IDocumentQuery<TProjection> SelectFields<TProjection>(params string[] fields);
// 2) By projection type
IDocumentQuery<TProjection> SelectFields<TProjection>();
{CODE-BLOCK/}

1) The fields of the projection are specified as a `string` array of field names. It also takes the type of the projection as 
a generic parameter.  

{CODE-TABS}
{CODE-TAB:csharp:Sync selectFields@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Async selectFields_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Index projections_9@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Class projections_9_class@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Companies/ByContact'
select Name, Phone
{CODE-TAB-BLOCK/}
{CODE-TABS/}

2) The projection is defined by simply passing the projection type as the generic parameter.  

{CODE-TABS}
{CODE-TAB:csharp:Sync selectFields_2@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Async selectFields_2_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Index projections_9@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Class projections_9_class@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Companies/ByContact'
select Name, Phone
{CODE-TAB-BLOCK/}
{CODE-TABS/}

#### Projection Behavior

The `SelectFields` methods can also take a `ProjectionBehavior` parameter, which 
determines whether the query should retrieve indexed data or directly retrieve 
document data, and what to do when the data can't be retrieved. Learn more 
[here](../../../client-api/session/querying/how-to-customize-query#projection).  

{CODE-BLOCK: csharp}
IDocumentQuery<TProjection> SelectFields<TProjection>(ProjectionBehavior projectionBehavior,
                                                      params string[] fields);

IDocumentQuery<TProjection> SelectFields<TProjection>(ProjectionBehavior projectionBehavior);
{CODE-BLOCK/}

{PANEL/}

{PANEL:ProjectInto}

This extension method retrieves all public fields and properties of the type given in generic and uses them to perform projection to the requested type.
You can use this method instead of using `Select` together with all fields of the projection class.

### Example

{CODE-TABS}
{CODE-TAB:csharp:Sync projections_8@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Async projections_8_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Companies/ByContact' 
select Name, Phone
{CODE-TAB-BLOCK/}
{CODE-TAB:csharp:Index projections_9@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Class projections_9_class@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}

{CODE-TABS/}

{PANEL/}

{PANEL:OfType (As) - simple projection}

`OfType` or `As` is a client-side projection. The easiest explanation of how it works is to take the results that the server returns and map them to given type. This may become useful when querying an index that contains fields that are not available in mapped type.

### Example

{CODE-TABS}
{CODE-TAB:csharp:Sync projections_10@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Async projections_10_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Index projections_11@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TABS/}

{PANEL/}

// move to indexes article...
{NOTE If the projected fields are stored inside the index itself (`FieldStorage.Yes` in the index definition), then the query results will be created directly from there instead of retrieving documents in order to project. /}

## Related Articles

### Session

- [Query Overview](../../../client-api/session/querying/how-to-query)
- [How to Stream Query Results](../../../client-api/session/querying/how-to-stream-query-results)

### Querying

- [Querying an Index](../../../indexes/querying/query-index)
- [Projections](../../../indexes/querying/projections)

### Server

- [JavaScript Engine](../../../server/kb/javascript-engine)  
