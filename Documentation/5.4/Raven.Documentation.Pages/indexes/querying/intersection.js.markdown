# Querying: Intersection
---

{NOTE: }

* To allow users to `intersect` queries on the server-side and return only documents 
  that match **all** the provided sub-queries, we introduced the query intersection feature.

* In this page:
  * [Intersection](../../indexes/querying/intersection#intersection)

{NOTE/}

---

{PANEL: Intersection}

Let's consider a case where we have a T-Shirt class:

{CODE:nodejs intersection_1@indexes\querying\intersection.js /}

We will fill our database with few records:

{CODE:nodejs intersection_3@indexes\querying\intersection.js /}

Now we want to return all the T-shirts that are manufactured by `Raven` and contain both 
`Small Blue` and `Large Gray` types.

To do this, we need to use the `intersect` query method:

{CODE-TABS}
{CODE-TAB:nodejs:Query intersection_4@indexes\querying\intersection.js /}
{CODE-TAB:nodejs:Index intersection_2@indexes\querying\intersection.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'TShirts/ByManufacturerColorSizeAndReleaseYear' 
where intersect(manufacturer = 'Raven', color = 'Blue' and size = 'Small', color = 'Gray' and size = 'Large') 
{CODE-TAB-BLOCK/}
{CODE-TABS/}

The above query will return `tshirts/1` and `tshirts/4` as a result.  
The document `tshirts/2` will not be included because it is not manufactured by `Raven`, 
and `tshirts/3` is not available in `Small Blue` so it does not match **all** the sub-queries. 

{PANEL/}

## Related Articles

### Client API

- [How to Use Intersect](../../client-api/session/querying/how-to-use-intersect)
