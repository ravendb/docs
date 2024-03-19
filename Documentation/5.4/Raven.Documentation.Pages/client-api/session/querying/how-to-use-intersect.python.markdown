# Using Intersect

To return only documents that match **all** provided sub-queries, use the `Intersect` extension which enables RavenDB to perform server-side intersection queries.

## Syntax

{CODE:python intersect_1@ClientApi\Session\Querying\HowToUseIntersect.py /}

## Example

{CODE-TABS}
{CODE-TAB:python:Query intersect_2@ClientApi\Session\Querying\HowToUseIntersect.py /}
{CODE-TAB:python:Index intersection_2@Indexes\Querying\Intersection.py /}
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
