# Filter Query Results
---

{NOTE: }

* One of the most basic functionalities of querying is the ability to filter out data and return records that match a given condition.

* The following examples demonstrate how to add simple conditions to a query:
  * [Where - equals](../../indexes/querying/filtering#where---equals)
  * [Where - numeric property](../../indexes/querying/filtering#where---numeric-property)
  * [Where - nested property](../../indexes/querying/filtering#where---nested-property)
  * [Where - multiple values](../../indexes/querying/filtering#where---multiple-values)
  * [Where - in](../../indexes/querying/filtering#where---in)
  * [Where - containsAny](../../indexes/querying/filtering#where---containsany)
  * [Where - containsAll](../../indexes/querying/filtering#where---containsall)
  * [Where - startsWith](../../indexes/querying/filtering#where---startswith)
  * [Where - endsWith](../../indexes/querying/filtering#where---endswith)
  * [Where - exists](../../indexes/querying/filtering#where---exists)
  * [Where - filter by ID](../../indexes/querying/filtering#where---filter-by-id)

{NOTE/}

---

{PANEL: Where - equals}

{CODE-TABS}
{CODE-TAB:nodejs:Query filter_1@indexes\querying\filtering.js /}
{CODE-TAB:nodejs:Index index_1@indexes\querying\filtering.js  /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByFirstAndLastName"
where FirstName == "Robert" and LastName == "King"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Where - numeric Property}

{CODE-TABS}
{CODE-TAB:nodejs:Query filter_2@indexes\querying\filtering.js /}
{CODE-TAB:nodejs:Index index_2@indexes\querying\filtering.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Products/ByUnitsInStock"
where UnitsInStock > 20
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CODE-TABS}
{CODE-TAB:nodejs:Query filter_2_1@indexes\querying\filtering.js /}
{CODE-TAB:nodejs:Index index_2@indexes\querying\filtering.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Products/ByUnitsInStock"
where UnitsInStock < 20
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Where - nested property}

{CODE-TABS}
{CODE-TAB:nodejs:Query filter_3@indexes\querying\filtering.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Orders"
where ShipTo.City == "Albuquerque"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Where - multiple values}

{CODE-TABS}
{CODE-TAB:nodejs:Query filter_4@indexes\querying\filtering.js /}
{CODE-TAB:nodejs:Index index_3@indexes\querying\filtering.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Orders/ByProductNamesPerOrderLine"
where ProductNames == "Teatime Chocolate Biscuits"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Where - in}

Use `whereIn` when you want to filter by a single value out of multiple given values.  

{CODE-TABS}
{CODE-TAB:nodejs:Query filter_5@indexes\querying\filtering.js /}
{CODE-TAB:nodejs:Index index_1@indexes\querying\filtering.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByFirstAndLastName"
where FirstName in ("Robert", "Nancy")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Where - containsAny}

Use `containsAny` to check if an enumeration contains any of the values from the specified list.

{CODE-TABS}
{CODE-TAB:nodejs:Query filter_6@indexes\querying\filtering.js /}
{CODE-TAB:nodejs:Index index_4@indexes\querying\filtering.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Orders/ByProductNames"
where ProductNames in ("ravioli", "coffee")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Where - containsAll}

Use `containsAll` to check if an enumeration contains all of the values from the specified list.

{CODE-TABS}
{CODE-TAB:nodejs:Query filter_7@indexes\querying\filtering.js /}
{CODE-TAB:nodejs:Index index_4@indexes\querying\filtering.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Orders/ByProductNames"
where ProductNames all in ("ravioli", "pepper")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Where - startsWith}

{CODE-TABS}
{CODE-TAB:nodejs:Query filter_8@indexes\querying\filtering.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products" 
where startsWith(Name, "ch")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Where - endsWith}

{CODE-TABS}
{CODE-TAB:nodejs:Query filter_9@indexes\querying\filtering.js /}
{CODE-TAB-BLOCK:sql:RQL}
from Products 
where endsWith(Name, 'ra')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Where - exists}

* To find all documents in a collection that have a specified field,  
  see [How to Filter by Field Presence](../../client-api/session/querying/how-to-filter-by-field).
  
* To find all documents in a collection that don't have a specified field,  
  see [How to Filter by Non-Existing Field](../../client-api/session/querying/how-to-filter-by-non-existing-field).

{PANEL/}

{PANEL: Where - filter by ID}

* Once the property used in the `whereEquals` clause is recognized as an identity property of a given entity type,  
  and there aren't any other fields involved in the query predicate, then this query is considered a "Collection Query".

* Such collection queries that ask about documents with given IDs, or where identifiers start with a given prefix
  and don't require any additional handling like ordering, full-text searching, etc, are handled directly by the storage engine.

* This means that querying by ID doesn't create an auto-index and has no extra cost.  
  In terms of efficiency, it is the same as loading documents with [`session.load`](../../client-api/session/loading-entities) usage.

{CODE-TABS}
{CODE-TAB:nodejs:Query filter_10@indexes\querying\filtering.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Orders"
where id() == "orders/1-A"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CODE-TABS}
{CODE-TAB:nodejs:Query filter_11@indexes\querying\filtering.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Orders"
where startsWith(id(), "orders/1")
{CODE-TAB-BLOCK/}
{CODE-TABS/}
{PANEL/}

## Related Articles

### Client API

- [Query Overview](../../client-api/session/querying/how-to-query)

### Indexes

- [Indexing Basics](../../indexes/indexing-basics)

### Querying

- [Query an Index](../../indexes/querying/query-index)
- [Paging](../../indexes/querying/paging)
- [Sorting](../../indexes/querying/sorting)
