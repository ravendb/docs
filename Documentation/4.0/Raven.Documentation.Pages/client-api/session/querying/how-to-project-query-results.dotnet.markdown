# Session : Querying : How to project query results?

Instead of pulling full documents in query results you can just grab some pieces of data from documents. Moreover, you can transform the projected
results. The projections are defined in LINQ with the usage of:

- [Select](../../../client-api/session/querying/how-to-project-query-results#select)
- [ProjectFromIndexFieldsInto](../../../client-api/session/querying/how-to-project-query-results#projectfromindexfieldsinto)
- [OfType (As)](../../../client-api/session/querying/how-to-project-query-results#oftype-(as)---simple-projection)

{PANEL:Select}

The most common way to perform a query with projection is to use `Select` method. You can specify what fields from a document you want to retrieve or even provide complex expression.

### Example I - Projecting only some fields of document

{CODE-TABS}
{CODE-TAB:csharp:Sync projections_1@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Async projections_1_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from Companies
select Name, Address.City as City, Address.Country as Country
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example II - Projecting arrays and objects

{CODE-TABS}
{CODE-TAB:csharp:Sync projections_2@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Async projections_2_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from Orders
select ShipTo, Lines[].ProductName as Products
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example III - Projection with expression

{CODE-TABS}
{CODE-TAB:csharp:Sync projections_3@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Async projections_3_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from Employees as e
select {
    FullName : e.FirstName + " " + e.LastName
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example IV - Projection with calculation

{CODE-TABS}
{CODE-TAB:csharp:Sync projections_4@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Async projections_4_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from Orders as o
select {
    Total : o.Lines.reduce(
        (acc , l) => acc += l.PricePerUnit * l.Quantity, 0)
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example V - Projection using loaded document

{CODE-TABS}
{CODE-TAB:csharp:Sync projections_5@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Async projections_5_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from Orders as o
where Company = 'companies /1'
load o.Company as c
select {
	CompanyName: c.Name,
	ShippedAt: o.ShippedAt
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example VI - Projection with dates

{CODE-TABS}
{CODE-TAB:csharp:Sync projections_6@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Async projections_6_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from Employees as e 
select { 
    DayOfBirth : new Date(Date.parse(e.Birthday)).getDate(), 
    MonthOfBirth : new Date(Date.parse(e.Birthday)).getMonth() + 1,
    Age : new Date().getFullYear() - new Date(Date.parse(e.Birthday)).getFullYear() 
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example VII - Projection with raw JavaScript code

{CODE-TABS}
{CODE-TAB:csharp:Sync projections_7@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Async projections_7_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from Employees as e 
select {
    Date : new Date(Date.parse(r.Birthday)), 
    Name : e.Name.substr(0,3)
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL:ProjectFromIndexFieldsInto}

This extension method retrieves all public fields and properties of the type given in generic and use them to perform projection to the requested type.
The query results will be created directly from stored fields of the index (it needs to be marked in the index definition).
If the necessary fields aren't stored then documents will be retrieved from the storage in order to return projections.

### Example

{CODE-TABS}
{CODE-TAB:csharp:Sync projections_8@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Async projections_8_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from index 'Companies/ByContact' 
select Name, Phone
{CODE-TAB-BLOCK/}
{CODE-TAB:csharp:Index projections_9@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL:OfType (As) - simple projection}

`OfType` or `As` is a client-side projection. The easiest explanation of how it works is: take results that server returned and map them to given type. This may become useful when querying index that contains fields that are not available in mapped type.

### Example

{CODE-TABS}
{CODE-TAB:csharp:Sync projections_10@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Async projections_10_async@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TAB:csharp:Index projections_11@ClientApi\Session\Querying\HowToProjectQueryResults.cs /}
{CODE-TABS/}

{PANEL/}

{NOTE Projected entities (even named types) are not tracked by session. /}
