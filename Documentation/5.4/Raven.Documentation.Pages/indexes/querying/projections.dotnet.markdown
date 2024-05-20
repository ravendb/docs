# Querying: Projections

There are couple of ways to perform projections in RavenDB:

- projections using [Select](../../indexes/querying/projections#select)
- using [SelectFields](../../indexes/querying/projections#selectfields)
- using [ProjectInto](../../indexes/querying/projections#projectinto)
- using [OfType (As)](../../indexes/querying/projections#oftype-(as))

## What are Projections and When to Use Them

When performing a query, we usually pull the full document back from the server.

However, we often need to display the data to the user. Instead of pulling the whole document back and picking 
just what we'll show, we can ask the server to send us just the details we want to show the user and thus reduce 
the amount of traffic on the network.   

The savings can be very significant if we need to show just a bit of information on a large document.  

A good example in the sample data set would be the order document. If we ask for all the Orders where Company 
is "companies/65-A", the size of the result that we get back from the server is 19KB.

However, if we perform the same query and ask to get back only the Employee and OrderedAt fields, the size of 
the result is only 5KB.  

Aside from allowing you to pick only a portion of the data, projection functions give you the ability to 
rename some fields, load external documents, and perform transformations on the results. 

## Projections are Applied as the Last Stage in the Query

It is important to understand that projections are applied after the query has been processed, filtered, 
sorted, and paged. The projection doesn't apply to all the documents in the database, only to the results 
that are actually returned.  
This reduces the load on the server significantly, since we can avoid doing work only to throw it immediately 
after. It also means that we cannot do any filtering work as part of the projection. You can filter what will 
be returned, but not which documents will be returned. That has already been determined earlier in the query 
pipeline.  

## The Cost of Running a Projection

Another consideration to take into account is the cost of running the projection. It is possible to make the 
projection query expensive to run. RavenDB has limits on the amount of time it will spend in evaluating the 
projection, and exceeding these (quite generous) limits will fail the query.

## Projections and Stored Fields

If a projection function only requires fields that are stored, then the document will not be loaded from 
storage and all data will come from the index directly. This can increase query performance (by the cost 
of disk space used) in many situations when whole document is not needed. You can read more about field 
storing [here](../../indexes/storing-data-in-index).

{PANEL:Select}
The most basic projection can be done using LINQ `Select` method:

### Example I - Projecting Individual Fields of the Document

{CODE-TABS}
{CODE-TAB:csharp:Query projections_1@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index indexes_1@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFirstAndLastName'
select FirstName, LastName
{CODE-TAB-BLOCK/}
{CODE-TABS/}

This will issue a query to a database, requesting only `FirstName` and `LastName` from all documents that 
index entries match query predicate from `Employees/ByFirstAndLastName` index. What does it mean? If an index 
entry matches our query predicate, then we will try to extract all requested fields from that particular entry. 
If all requested fields are available in there, then we do not download it from storage. 
The index `Employees/ByFirstAndLastName` used in the above query is not storing any fields, 
so the documents will be fetched from storage.

### Example II - Projecting Stored Fields

If we create an index that stores `FirstName` and `LastName` and it requests only those fields in query, 
then **the data will come from the index directly**.

{CODE-TABS}
{CODE-TAB:csharp:Query projections_1_stored@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index indexes_1_stored@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFirstAndLastNameWithStoredFields'
select FirstName, LastName
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example III - Projecting Arrays and Objects

{CODE-TABS}
{CODE-TAB:csharp:Query projections_2@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index indexes_3@Indexes\Querying\Projections.cs /}
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
{CODE-TAB:csharp:Query projections_3@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index indexes_1@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFirstAndLastName' as e
select 
{ 
    FullName : e.FirstName + " " + e.LastName 
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example V - Projection with `let`
{CODE-TABS}
{CODE-TAB:csharp:Query projections_4@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index indexes_1@Indexes\Querying\Projections.cs /}
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
{CODE-TAB:csharp:Query projections_9@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index indexes_3@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Orders/ByShipToAndLines' as o
select {
    Total : o.Lines.reduce(
        (acc , l) => acc += l.PricePerUnit * l.Quantity, 0)
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example VII - Projection With a Count() Predicate

{CODE-TABS}
{CODE-TAB:csharp:Query projections_count_in_projection@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Index indexes_4@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders as o 
load o.Company as c 
select 
{ 
    CompanyName : c.Name, 
    ShippedAt : o.ShippedAt, 
    TotalProducts : o.Lines.length, 
    TotalDiscountedProducts : o.Lines.filter(x => x.Discount > 0 ).length 
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}


### Example VIII - Projection Using a Loaded Document

{CODE-TABS}
{CODE-TAB:csharp:Query projections_5@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index indexes_4@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Orders/ByShippedAtAndCompany' as o
load o.Company as c
select {
	CompanyName: c.Name,
	ShippedAt: o.ShippedAt
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example IX - Projection with Dates

{CODE-TABS}
{CODE-TAB:csharp:Query projections_6@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index indexes_2@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFirstNameAndBirthday' as e 
select { 
    DayOfBirth : new Date(Date.parse(e.Birthday)).getDate(), 
    MonthOfBirth : new Date(Date.parse(e.Birthday)).getMonth() + 1,
    Age : new Date().getFullYear() - new Date(Date.parse(e.Birthday)).getFullYear() 
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example X - Projection with Raw JavaScript Code

{CODE-TABS}
{CODE-TAB:csharp:Query projections_7@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index indexes_2@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFirstNameAndBirthday' as e 
select {
    Date : new Date(Date.parse(e.Birthday)), 
    Name : e.FirstName.substr(0,3)
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example XI - Projection with Metadata

{CODE-TABS}
{CODE-TAB:csharp:Query projections_8@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index indexes_1@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFirstAndLastName' as e 
select {
     Name : e.FirstName, 
     Metadata : getMetadata(e)
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL:SelectFields}

The `SelectFields` method can only be used with the 
[Document Query](../../client-api/session/querying/document-query/what-is-document-query). 
It has two overloads:

{CODE-BLOCK: csharp}
// 1) By array of fields
IDocumentQuery<TProjection> SelectFields<TProjection>(params string[] fields);
// 2) By projection type
IDocumentQuery<TProjection> SelectFields<TProjection>();
{CODE-BLOCK/}

1) The fields of the projection are specified as a `string` array of field names. 
It also takes the type of the projection as a generic parameter.  

{CODE-TABS}
{CODE-TAB:csharp:Query selectFields_1@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index index_10@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Class projections_10_class@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Companies/ByContact'
select Name, Phone
{CODE-TAB-BLOCK/}
{CODE-TABS/}

2) The projection is defined by simply passing the projection type as the generic parameter.  

{CODE-TABS}
{CODE-TAB:csharp:Query selectFields_2@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index index_10@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Class projections_10_class@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Companies/ByContact'
select Name, Phone
{CODE-TAB-BLOCK/}
{CODE-TABS/}

#### Projection Behavior

The `SelectFields` methods can also take a `ProjectionBehavior` parameter, which 
determines whether the query should retrieve indexed data or directly retrieve 
document data, and what to do when the data can't be retrieved. Learn more 
[here](../../client-api/session/querying/how-to-customize-query#projection).  

{CODE-BLOCK: csharp}
IDocumentQuery<TProjection> SelectFields<TProjection>(ProjectionBehavior projectionBehavior,
                                                      params string[] fields);

IDocumentQuery<TProjection> SelectFields<TProjection>(ProjectionBehavior projectionBehavior);
{CODE-BLOCK/}

{PANEL/}

{PANEL:ProjectInto}

This extension method retrieves all public fields and properties of the type given in generic and uses 
them to perform projection to the requested type.

You can use this method instead of using `Select` together with all fields of the projection class.

### Example

{CODE-TABS}
{CODE-TAB:csharp:Query projections_10@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Companies/ByContact' 
select Name, Phone
{CODE-TAB-BLOCK/}
{CODE-TAB:csharp:Index index_10@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Class projections_10_class@Indexes\Querying\Projections.cs /}

{CODE-TABS/}

{PANEL/}

{PANEL:OfType (As)}

`OfType` or `As` is a client-side projection. You can read more about it 
[here](../../client-api/session/querying/how-to-project-query-results#oftype-(as)---simple-projection).

{PANEL/}

## Projections and the Session
Because you are working with projections and not directly with documents, they are _not_ tracked by the session. 
Modifications to a projection will not modify the document when SaveChanges is called.

## Related Articles

### Querying 

- [Query Overview](../../client-api/session/querying/how-to-query)
- [Querying an Index](../../indexes/querying/query-index)

### Client API

- [How to Project Query Results](../../client-api/session/querying/how-to-project-query-results)

### Knowledge Base

- [JavaScript Engine](../../server/kb/javascript-engine)
