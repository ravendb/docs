# Session: Querying: How to Use Intersect

To return only documents that match **all** provided sub-queries, use the `intersect` method which enables RavenDB to perform server-side intersection queries.

## Syntax

{CODE:java intersect_1@ClientApi\Session\Querying\HowToUseIntersect.java /}

## Example

{CODE-TABS}
{CODE-TAB:java:Java intersect_2@ClientApi\Session\Querying\HowToUseIntersect.java /}
{CODE-TAB:java:Index intersection_2@Indexes\Querying\Intersection.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'TShirts/ByManufacturerColorSizeAndReleaseYear' 
where intersect(Manufacturer = 'Raven', Color = 'Blue' and Size = 'Small', Color = 'Gray' and Size = 'Large')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Related articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)

### Querying

- [Intersection](../../../indexes/querying/intersection)
