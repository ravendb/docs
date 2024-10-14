# Project Query Results
---

{NOTE: }

* Applying a projection in a query allows you to shape the query results to meet specific requirements,  
  delivering just the data needed instead of the original full document content.

* This article provides examples of projecting query results when making a **dynamic-query**.  
  For projecting results when querying a **static-index** see [project index query results](../../../indexes/querying/projections).

* In this page:

    * [Projections overview](../../../client-api/session/querying/how-to-project-query-results#projections-overview)

    * [SelectFields](../../../client-api/session/querying/how-to-project-query-results#selectfields)
  
    * [Projecting nested object types](../../../client-api/session/querying/how-to-project-query-results#projecting-nested-object-types)
  
    * [Syntax](../../../client-api/session/querying/how-to-project-query-results#syntax)

{NOTE/}

---

{PANEL: Projections overview}

---

**What are projections**:

* A projection refers to the **transformation of query results** into a customized structure,  
  modifying the shape of the data returned by the server.

* Instead of retrieving the full document from the server and then picking relevant data from it on the client,  
  you can request a subset of the data, specifying the document fields you want to get from the server.

* The query can load [related documents](../../../indexes/indexing-related-documents#what-are-related-documents) and have their data merged into the projection results.

* Content from inner objects and arrays can be projected in addition to [projecting the nested object types](../../../client-api/session/querying/how-to-project-query-results#projecting-nested-object-types).

* An alias name can be given to the projected fields, and any calculations can be made within the projection.

---

**When to use projections**:

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

**Projections are not tracked by the session**:

* On the client side, the resulting projected entities returned by the query are Not tracked by the Session.

* Any modification made to a projection entity will not modify the corresponding document on the server when _SaveChanges_ is called.

---

**Projections are the final stage in the query pipeline**:

* Projections are applied as the last stage in the query pipeline,  
  after the query has been processed, filtered, sorted, and paged.

* This means that the projection does Not apply to all the documents in the collection,  
  but only to the documents matching the query predicate.

* Within the projection you can only filter what data will be returned from the matching documents,  
  but you cannot filter which documents will be returned. That has already been determined earlier in the query pipeline.

---

**The cost of projections**:

* Queries in RavenDB do not allow any computation to occur during the query phase.  
  However, you can perform any [calculations](../../../client-api/session/querying/how-to-project-query-results#projectionWithCalculations) inside the projection.

* But while calculations within a projection are allowed, having a very complex logic can impact query performance.  
  So RavenDB limits the total time it will spend processing a query and its projections.  
  Exceeding this time limit will fail the query. This is configurable, see the following configuration keys:  
  * [Databases.QueryTimeoutInSec](../../../server/configuration/database-configuration#databases.querytimeoutinsec)
  * [Databases.QueryOperationTimeoutInSec](../../../server/configuration/database-configuration#databases.queryoperationtimeoutinsec)

{PANEL/}

{PANEL: SelectFields}

* Use `selectFields` to specify which fields should be returned per document that is matching the query criteria.

* Complex projection expressions can be provided directly with RQL via the `rawQuery` syntax,  
  see examples below.

{NOTE: }

**Example I - Projecting individual fields of the document**:

{CODE-TABS}
{CODE-TAB:nodejs:Query projections_1@client-api\session\querying\howToProjectQueryResults.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "companies"
select Name, Address.City, Address.Country
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Example II - Projecting individual fields with alias**:

{CODE-TABS}
{CODE-TAB:nodejs:Query projections_2@client-api\session\querying\howToProjectQueryResults.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "companies"
select Name as CompanyName, Address.City as City, Address.Country as Country
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Example III - Projecting arrays and objects**:

{CODE-TABS}
{CODE-TAB:nodejs:Query projections_3@client-api\session\querying\howToProjectQueryResults.js /}
{CODE-TAB-BLOCK:sql:RQL}
// Using simple expression:
from "orders"
select ShipTo, Lines[].ProductName as ProductNames

// Using JavaScript object literal syntax:
from "Orders" as x
select {
    ShipTo: x.ShipTo,
    ProductNames: x.Lines.map(y => y.ProductName)
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Example IV - Projection with expression**:

{CODE-TABS}
{CODE-TAB:nodejs:Query projections_4@client-api\session\querying\howToProjectQueryResults.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "employees" as e
select {
    FullName: e.FirstName + " " + e.LastName
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

<a id="projectionWithCalculations" /> **Example V - Projection with calculations**:

{CODE-TABS}
{CODE-TAB:nodejs:Query projections_5@client-api\session\querying\howToProjectQueryResults.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "orders" as x
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
{CODE-TAB:nodejs:Query projections_6@client-api\session\querying\howToProjectQueryResults.js /}
{CODE-TAB-BLOCK:sql:RQL}
declare function output(e) {
    var format = p => p.FirstName + " " + p.LastName;
    return { FullName: format(e) };
}
from "employees" as e select output(e)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Example VII - Projecting using a loaded document**:

{CODE-TABS}
{CODE-TAB:nodejs:Query projections_7@client-api\session\querying\howToProjectQueryResults.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "orders" as o
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
{CODE-TAB:nodejs:Query projections_8@client-api\session\querying\howToProjectQueryResults.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "employees" as e
select {
    DayOfBirth: new Date(Date.parse(e.Birthday)).getDate(),
    MonthOfBirth: new Date(Date.parse(e.Birthday)).getMonth() + 1,
    Age: new Date().getFullYear() - new Date(Date.parse(e.Birthday)).getFullYear()
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Example IX - Projection with metadata**:

{CODE-TABS}
{CODE-TAB:nodejs:Query projections_9@client-api\session\querying\howToProjectQueryResults.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "employees" as e
select {
    Name: e.FirstName,
    Metadata: getMetadata(e)
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}
{PANEL/}

{PANEL: Projecting nested object types}

In the Node.js client, when projecting query results using the `selectFields` method (not via the `rawQuery` syntax),  
the metadata field `@nested-object-types` from the document will be automatically added to the list of fields to project in the generated RQL that is sent to the server.

{CODE:nodejs projections_10@client-api\session\querying\howToProjectQueryResults.js /}

{CODE-BLOCK:sql}
// The following RQL is generated by the Node.js client:
// =====================================================

from "users"
select name, @metadata.@nested-object-types as **PROJECTED_NESTED_OBJECT_TYPES**
{CODE-BLOCK/}

{CODE:nodejs projections_10_results@client-api\session\querying\howToProjectQueryResults.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax@client-api\session\querying\howToProjectQueryResults.js /}

| Parameter              | Type        | Description                                                                                                                                             |
|------------------------|-------------|---------------------------------------------------------------------------------------------------------------------------------------------------------|
| **property**           | `string`    | Field name to project                                                                                                                                   |
| **properties**         | `string[]`  | List of field names to project                                                                                                                          |
| **queryData**          | `QueryData` | Object with projection query definitions                                                                                                                |
| **projectionClass**    | `object`    | The class type of the projected fields                                                                                                                  |
| **projectionBehavior** | `string`    | Projection behavior is useful when querying a static-index.<br>Learn more in [projection behavior with indexes](../../../indexes/querying/projections). |

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
