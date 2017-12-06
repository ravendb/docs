# Projections

There are couple a couple of ways to perform projections in RavenDB:

- simple projections using [Select](../../indexes/querying/projections#select)
- using [ProjectInto](../../indexes/querying/projections#projectinto)
- using [OfType (As)](../../indexes/querying/projections#oftype-(as))

{PANEL:Select}
The most basic projection can be done using LINQ `Select` method:

### Example I - Projecting Individual Fields of the Document

{CODE-TABS}
{CODE-TAB:csharp:Query projections_1_query@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:DocumentQuery projections_1_docquery@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index indexes_1@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from index 'Employees/ByFirstAndLastName'
select FirstName, LastName
{CODE-TAB-BLOCK/}
{CODE-TABS/}

This will issue a query to a database, requesting only `FirstName` and `LastName` from all documents that index entries match query predicate from `Employees/ByFirstAndLastName` index. What does it mean? It means that, if index entry matches our query predicate, then we will try to extract all requested fields from that particular entry and if all requested fields are available in there, then we do not download it from storage. Index `Employees/ByFirstAndLastName` used in above query is not storing any fields so documents will be fetched from storage.

{INFO:Projections and Stored fields}
If projection function only requires fields that are stored, then document will not be loaded from storage and all data will come from index directly. This can increase query performance (by the cost of disk space used) in many situations when whole document is not needed. You can read more about field storing [here](../../indexes/storing-data-in-index).
{INFO/}

So following above rule, if we create index that stores `FirstName` and `LastName` and request only those fields in query, then **data will come from index directly**.

{CODE-TABS}
{CODE-TAB:csharp:Query projections_1_stored_query@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:DocumentQuery projections_1_stored_docquery@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index indexes_1_stored@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from index 'Employees/ByFirstAndLastNameWithStoredFields'
select FirstName, LastName
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example II - Projecting Arrays and Objects

{CODE-TABS}
{CODE-TAB:csharp:Query projections_2_query@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:DocumentQuery projections_2_docquery@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index indexes_3@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from index 'Orders/ByShipToAndLines' as o
select 
{ 
    ShipTo: o.ShipTo, 
    Products : o.Lines.map(function(y){return y.ProductName;}) 
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example III - Projection with Expression

{CODE-TABS}
{CODE-TAB:csharp:Query projections_3_query@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:DocumentQuery projections_3_docquery@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index indexes_1@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from index 'Employees/ByFirstAndLastName' as e
select 
{ 
    FullName : e.FirstName + \" \" + e.LastName 
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example IV - Projection with `let`
{CODE-TABS}
{CODE-TAB:csharp:Query projections_4_query@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index indexes_1@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
declare function output(e) {
	var format = function(p){ return p.FirstName + " " + p.LastName; };
	return { FullName : format(e) };
}
from index 'Employees/ByFirstAndLastName' as e select output(e)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example V - Projection with Calculation

{CODE-TABS}
{CODE-TAB:csharp:Query projections_9_query@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:DocumentQuery projections_9_docquery@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index indexes_3@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from index 'Orders/ByShipToAndLines' as o
select {
    Total : o.Lines.reduce(
        (acc , l) => acc += l.PricePerUnit * l.Quantity, 0)
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example VI - Projection Using a Loaded Document

{CODE-TABS}
{CODE-TAB:csharp:Query projections_5_query@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index indexes_4@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from index 'Orders/ByShippedAtAndCompany' as o
load o.Company as c
select {
	CompanyName: c.Name,
	ShippedAt: o.ShippedAt
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example VII - Projection with Dates

{CODE-TABS}
{CODE-TAB:csharp:Query projections_6_query@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:DocumentQuery projections_6_docquery@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index indexes_2@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from index 'Employees/ByFirstNameAndBirthday' as e 
select { 
    DayOfBirth : new Date(Date.parse(e.Birthday)).getDate(), 
    MonthOfBirth : new Date(Date.parse(e.Birthday)).getMonth() + 1,
    Age : new Date().getFullYear() - new Date(Date.parse(e.Birthday)).getFullYear() 
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example VIII - Projection with Raw JavaScript Code

{CODE-TABS}
{CODE-TAB:csharp:Query projections_7_query@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index indexes_2@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from index 'Employees/ByFirstNameAndBirthday' as e 
select {
    Date : new Date(Date.parse(e.Birthday)), 
    Name : e.FirstName.substr(0,3)
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example IX - Projection with Metadata

{CODE-TABS}
{CODE-TAB:csharp:Query projections_8_query@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:DocumentQuery projections_8_docquery@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index indexes_1@Indexes\Querying\Projections.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from index 'Employees/ByFirstAndLastName' as e 
select {
     Name : e.FirstName, 
     Metadata : getMetadata(e)
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL:ProjectInto}

This extension method retrieves all public fields and properties of the type given in generic and uses them to perform projection to the requested type.
You can use this method instead of using `Select` together with all fields of the projection class.

### Example

{CODE-TABS}
{CODE-TAB:csharp:Sync projections_8@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Async projections_8_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from index 'Companies/ByContact' 
select Name, Phone
{CODE-TAB-BLOCK/}
{CODE-TAB:csharp:Index projections_9@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Class projections_9_class@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}

{CODE-TABS/}

{PANEL/}

{PANEL:OfType (As)}

`OfType` or `As` is a client-side projection. You can read more about it [here](../../client-api/session/querying/how-to-project-query-results#oftype-(as)---simple-projection).

{PANEL/}

{NOTE Projected entities (even named types) are not tracked by the session. /}

## Related articles

- [Client API : Session : How to project query results?](../../client-api/session/querying/how-to-project-query-results)
