# Session: Querying: How to Project Query Results

Instead of pulling full documents in query results you can just grab some pieces of data from documents. You can also transform the projected
results. The projections are defined with the usage of:

- [SelectFields](../../../client-api/session/querying/how-to-project-query-results#selectfields)
- [OfType](../../../client-api/session/querying/how-to-project-query-results#oftype---simple-projection)

{PANEL:SelectFields}

The most common way to perform a query with projection is to use the `selectFields` method. You can specify what fields from a document you want to retrieve.

### Example I - Projecting Individual Fields of the Document

{CODE-TABS}
{CODE-TAB:java:Java projections_1@ClientApi\Session\Querying\HowToProjectQueryResults.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Companies
select Name, Address.City as City, Address.Country as Country
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example II - Projecting Arrays and Objects

{CODE-TABS}
{CODE-TAB:java:Java projections_2@ClientApi\Session\Querying\HowToProjectQueryResults.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders
select ShipTo, Lines[].ProductName as Products
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example III - Projection with Expression

{CODE-TABS}
{CODE-TAB:java:Java projections_3@ClientApi\Session\Querying\HowToProjectQueryResults.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Employees as e
select {
    FullName : e.FirstName + " " + e.LastName
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example IV - Projection with `declared function`

{CODE-TABS}
{CODE-TAB:java:Java projections_12@ClientApi\Session\Querying\HowToProjectQueryResults.java /}
{CODE-TAB-BLOCK:sql:RQL}
declare function output(e) {
	var format = function(p){ return p.FirstName + " " + p.LastName; };
	return { FullName : format(e) };
}
from Employees as e select output(e)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example V - Projection with Calculation

{CODE-TABS}
{CODE-TAB:java:Java projections_4@ClientApi\Session\Querying\HowToProjectQueryResults.java /}
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
{CODE-TAB:java:Java projections_5@ClientApi\Session\Querying\HowToProjectQueryResults.java /}
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
{CODE-TAB:java:Java projections_6@ClientApi\Session\Querying\HowToProjectQueryResults.java /}
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
{CODE-TAB:java:Java projections_7@ClientApi\Session\Querying\HowToProjectQueryResults.java /}
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
{CODE-TAB:java:Java projections_13@ClientApi\Session\Querying\HowToProjectQueryResults.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Employees as e 
select {
     Name : e.FirstName, 
     Metadata : getMetadata(e)
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

This method overload retrieves all public fields of the class given in generic and uses them to perform projection to the requested type.
You can use this method instead of using `selectFields` together with all fields of the projection class.

### Example X

{CODE-TABS}
{CODE-TAB:java:Java projections_8@ClientApi\Session\Querying\HowToProjectQueryResults.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Companies/ByContact' 
select Name, Phone
{CODE-TAB-BLOCK/}
{CODE-TAB:java:Index projections_9_0@ClientApi\Session\Querying\HowToProjectQueryResults.java /}
{CODE-TAB:java:Class projections_9_1@ClientApi\Session\Querying\HowToProjectQueryResults.java /}

{CODE-TABS/}

{PANEL/}

{PANEL:OfType - simple projection}

`ofType` is a client-side projection. The easiest explanation of how it works is to take the results that the server returns and map them to given type. This may become useful when querying an index that contains fields that are not available in mapped type.

### Example

{CODE:java projections_10@ClientApi\Session\Querying\HowToProjectQueryResults.java /}

{PANEL/}

{NOTE Projected entities (even named types) are not tracked by the session. /}

{NOTE If the projected fields are stored inside the index itself (`FieldStorage.YES` in the index definition), then the query results will be created directly from there instead of retrieving documents in order to project. /}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)
- [How to Stream Query Results](../../../client-api/session/querying/how-to-stream-query-results)

### Querying

- [Basics](../../../indexes/querying/basics)
- [Projections](../../../indexes/querying/projections)

### Server

- [JavaScript Engine](../../../server/kb/javascript-engine)  
