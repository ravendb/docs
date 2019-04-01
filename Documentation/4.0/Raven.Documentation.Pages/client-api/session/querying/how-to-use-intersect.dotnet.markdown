# Session: Querying: How to Use Intersect

To return only documents that match **all** provided sub-queries, use the `Intersect` extension which enables RavenDB to perform server-side intersection queries.

## Syntax

{CODE intersect_1@ClientApi\Session\Querying\HowToUseIntersect.cs /}

## Example

{CODE-TABS}
{CODE-TAB:csharp:Sync intersect_2@ClientApi\Session\Querying\HowToUseIntersect.cs /}
{CODE-TAB:csharp:Async intersect_3@ClientApi\Session\Querying\HowToUseIntersect.cs /}
{CODE-TAB:csharp:Index intersection_2@Indexes\Querying\Intersection.cs /}
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
