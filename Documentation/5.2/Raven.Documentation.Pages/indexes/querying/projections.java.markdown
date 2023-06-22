# Querying: Projections

There are couple of ways to perform projections in RavenDB:

- projections using [SelectFields](../../indexes/querying/projections#selectfields)
- using [OfType](../../indexes/querying/projections#oftype)

## What are Projections and When to Use Them

When performing a query, we usually pull the full document back from the server.

However, we often need to display the data to the user. Instead of pulling the whole document back and picking just what we'll show, we can ask the server to send us just the details we want to show the user and thus reduce the amount of traffic on the network.   

The savings can be very significant if we need to show just a bit of information on a large document.  

A good example in the sample data set would be the order document. If we ask for all the Orders where Company is "companies/65-A", the size of the result that we get back from the server is 19KB.

However, if we perform the same query and ask to get back only the Employee and OrderedAt fields, the size of the result is only 5KB.  

Aside from allowing you to pick only a portion of the data, projection functions give you the ability to rename some fields, load external documents, and perform transformations on the results. 

## Projections are Applied as the Last Stage in the Query

It is important to understand that projections are applied after the query has been processed, filtered, sorted, and paged. The projection doesn't apply to all the documents in the database, only to the results that are actually returned.  
This reduces the load on the server significantly, since we can avoid doing work only to throw it immediately after. It also means that we cannot do any filtering work as part of the projection. You can filter what will be returned, but not which documents will be returned. That has already been determined earlier in the query pipeline.  

## The Cost of Running a Projection

Another consideration to take into account is the cost of running the projection. It is possible to make the projection query expensive to run. RavenDB has limits on the amount of time it will spend in evaluating the projection, and exceeding these (quite generous) limits will fail the query.

## Projections and Stored Fields

If a projection function only requires fields that are stored, then the document will not be loaded from storage and all data will come from the index directly. This can increase query performance (by the cost of disk space used) in many situations when whole document is not needed. You can read more about field storing [here](../../indexes/storing-data-in-index).

{PANEL:SelectFields}
The most basic projection can be done using `selectFields` method:

### Example I - Projecting Individual Fields of the Document

{CODE-TABS}
{CODE-TAB:java:Query projections_1@Indexes\Querying\Projections.java /}
{CODE-TAB:java:Index indexes_1@Indexes\Querying\Projections.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFirstAndLastName'
select FirstName, LastName
{CODE-TAB-BLOCK/}
{CODE-TABS/}

This will issue a query to a database, requesting only `FirstName` and `LastName` from all documents that index entries match query predicate from `Employees/ByFirstAndLastName` index. What does it mean? If an index entry matches our query predicate, then we will try to extract all requested fields from that particular entry. If all requested fields are available in there, then we do not download it from storage. The index `Employees/ByFirstAndLastName` used in the above query is not storing any fields, so the documents will be fetched from storage.

### Example II - Projecting Stored Fields

If we create an index that stores `FirstName` and `LastName` and it requests only those fields in query, then **the data will come from the index directly**.

{CODE-TABS}
{CODE-TAB:java:Query projections_1_stored@Indexes\Querying\Projections.java /}
{CODE-TAB:java:Index indexes_1_stored@Indexes\Querying\Projections.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFirstAndLastNameWithStoredFields'
select FirstName, LastName
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example III - Projecting Arrays and Objects

{CODE-TABS}
{CODE-TAB:java:Query projections_2@Indexes\Querying\Projections.java /}
{CODE-TAB:java:Index indexes_3@Indexes\Querying\Projections.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Orders/ByShipToAndLines' as o
select 
{ 
    ShipTo: o.ShipTo, 
    Products : o.Lines.map(function(y){return y.ProductName;}) 
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example IV - Projection with Expression

{CODE-TABS}
{CODE-TAB:java:Query projections_3@Indexes\Querying\Projections.java /}
{CODE-TAB:java:Index indexes_1@Indexes\Querying\Projections.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFirstAndLastName' as e
select 
{ 
    FullName : e.FirstName + " " + e.LastName 
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example V - Projection with `declared function`
{CODE-TABS}
{CODE-TAB:java:Query projections_4@Indexes\Querying\Projections.java /}
{CODE-TAB:java:Index indexes_1@Indexes\Querying\Projections.java /}
{CODE-TAB-BLOCK:sql:RQL}
declare function output(e) {
	var format = function(p){ return p.FirstName + " " + p.LastName; };
	return { FullName : format(e) };
}
from index 'Employees/ByFirstAndLastName' as e select output(e)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example VI - Projection with Calculation

{CODE-TABS}
{CODE-TAB:java:Query projections_9@Indexes\Querying\Projections.java /}
{CODE-TAB:java:Index indexes_3@Indexes\Querying\Projections.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Orders/ByShipToAndLines' as o
select {
    Total : o.Lines.reduce(
        (acc , l) => acc += l.PricePerUnit * l.Quantity, 0)
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example VII - Projection Using a Loaded Document

{CODE-TABS}
{CODE-TAB:java:Query projections_5@Indexes\Querying\Projections.java /}
{CODE-TAB:java:Index indexes_4@Indexes\Querying\Projections.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Orders/ByShippedAtAndCompany' as o
load o.Company as c
select {
	CompanyName: c.Name,
	ShippedAt: o.ShippedAt
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example VIII - Projection with Dates

{CODE-TABS}
{CODE-TAB:java:Query projections_6@Indexes\Querying\Projections.java /}
{CODE-TAB:java:Index indexes_2@Indexes\Querying\Projections.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFirstNameAndBirthday' as e 
select { 
    DayOfBirth : new Date(Date.parse(e.Birthday)).getDate(), 
    MonthOfBirth : new Date(Date.parse(e.Birthday)).getMonth() + 1,
    Age : new Date().getFullYear() - new Date(Date.parse(e.Birthday)).getFullYear() 
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example IX - Projection with Raw JavaScript Code

{CODE-TABS}
{CODE-TAB:java:Query projections_7@Indexes\Querying\Projections.java /}
{CODE-TAB:java:Index indexes_2@Indexes\Querying\Projections.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFirstNameAndBirthday' as e 
select {
    Date : new Date(Date.parse(e.Birthday)), 
    Name : e.FirstName.substr(0,3)
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example X - Projection with Metadata

{CODE-TABS}
{CODE-TAB:java:Query projections_8@Indexes\Querying\Projections.java /}
{CODE-TAB:java:Index indexes_1@Indexes\Querying\Projections.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFirstAndLastName' as e 
select {
     Name : e.FirstName, 
     Metadata : getMetadata(e)
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}


{PANEL:OfType}

`OfType` is a client-side projection. You can read more about it [here](../../client-api/session/querying/how-to-project-query-results#oftype-(as)---simple-projection).

{PANEL/}

## Projections and the Session
Because you are working with projections and not directly with documents, they are _not_ tracked by the session. Modifications to a projection will not modify the document when `saveChanges` is called.

## Related Articles

### Querying 

- [Basics](../../indexes/querying/query-index)

### Client API

- [How to Project Query Results](../../client-api/session/querying/how-to-project-query-results)

### Knowledge Base

- [JavaScript Engine](../../server/kb/javascript-engine)
