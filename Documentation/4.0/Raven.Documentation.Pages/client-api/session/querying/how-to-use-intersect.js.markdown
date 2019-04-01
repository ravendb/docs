# Session: Querying: How to Use Intersect

To return only documents that match *all* provided sub-queries, use the `intersect()` method which enables RavenDB to perform server-side intersection queries.

## Syntax

{CODE:nodejs intersect_1@client-api\session\querying\howToUseIntersect.js /}

## Example

{CODE-TABS}
{CODE-TAB:nodejs:Node.js intersect_2@client-api\session\querying\howToUseIntersect.js /}
{CODE-TAB:nodejs:Index intersection_2@indexes\querying\intersection.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'TShirts/ByManufacturerColorSizeAndReleaseYear' 
where intersect(manufacturer = 'Raven', color = 'Blue' and size = 'Small', color = 'Gray' and size = 'Large')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Related articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)

### Querying

- [Intersection](../../../indexes/querying/intersection)
