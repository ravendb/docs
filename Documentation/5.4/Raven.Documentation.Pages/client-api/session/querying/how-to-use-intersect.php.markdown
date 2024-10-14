# Using Intersect

To return only documents that match **all** provided sub-queries, use the 
`intersect` extension to allow RavenDB to perform server-side intersection queries.

## Syntax

{CODE:php intersect_1@ClientApi\Session\Querying\HowToUseIntersect.php /}

## Example

{CODE-TABS}
{CODE-TAB:php:Sync intersect_2@ClientApi\Session\Querying\HowToUseIntersect.php /}
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
