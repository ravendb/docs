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

{CODE:php intersection_1@Indexes\Querying\Intersection.php /}

We will fill our database with a few records:

{CODE:php intersection_3@Indexes\Querying\Intersection.php /}

Now we can use the `intersect` method to return all the T-shirts that are 
manufactured by `Raven` and contain both `Small Blue` and `Large Gray` types.

{CODE-TABS}
{CODE-TAB:php:Query intersection_4@Indexes\Querying\Intersection.php /}
{CODE-TAB:php:Index intersection_2@Indexes\Querying\Intersection.php /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'TShirts/ByManufacturerColorSizeAndReleaseYear' 
where intersect(Manufacturer = 'Raven', Color = 'Blue' and Size = 'Small', Color = 'Gray' and Size = 'Large') 
{CODE-TAB-BLOCK/}
{CODE-TABS/}

The above query will return `tshirts/1` and `tshirts/4`, that match **all** sub-queries.  
`tshirts/2` will not be included in the results because it is not manufactured by `Raven`, 
and `tshirts/3` will not be included because it is not available in `Small Blue`.  

{PANEL/}

## Related Articles

### Client API

- [How to Use Intersect](../../client-api/session/querying/how-to-use-intersect)
