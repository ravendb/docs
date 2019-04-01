# Querying: Spatial

To perform a spatial search, you can use the `Spatial` method which contains a full spectrum of spatial capabilities. You can check the detailed Client API reference for this method [here](../../client-api/session/querying/how-to-query-a-spatial-index).

## Radius Search

The most basic usage and probably most common one is to search for all points or shapes within provided distance from the given center point. To perform this search use the `WithinRadius` method.

{CODE-TABS}
{CODE-TAB:csharp:Query spatial_1_0@Indexes\Querying\Spatial.cs /}
{CODE-TAB:csharp:DocumentQuery spatial_1_1@Indexes\Querying\Spatial.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Events
where spatial.within(spatial.point(Latitude, Longitude), spatial.circle(500, 30, 30))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Advanced Search

The most advanced (and low-level) method available is `RelatesToShape`

{CODE-TABS}
{CODE-TAB:csharp:Query spatial_2_0@Indexes\Querying\Spatial.cs /}
{CODE-TAB:csharp:DocumentQuery spatial_2_1@Indexes\Querying\Spatial.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Events
where spatial.within(spatial.point(Latitude, Longitude), spatial.wkt('Circle(30 30 d=500.0000)'))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

Where the shape is in [WKT](https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry) format and the relation is one of `Within`, `Contains`, `Disjoint`, `Intersects`. The above example will yield the same results as the example from the `Radius Search` section.

## Static Indexes

All of the above examples are using the dynamic querying capabilities of RavenDB and will create automatic indexes to retrieve their results. However, spatial queries can also be performed against static indexes, and this is done in a very similar way.

{CODE-TABS}
{CODE-TAB:csharp:Query spatial_3_0@Indexes\Querying\Spatial.cs /}
{CODE-TAB:csharp:DocumentQuery spatial_3_1@Indexes\Querying\Spatial.cs /}
{CODE-TAB:csharp:Index spatial_3_2@Indexes\Querying\Spatial.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Events/ByCoordinates'
where spatial.within(Coordinates, spatial.circle(500, 30, 30))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{INFO If you want to know how to setup and customize a spatial field in static index please refer to [this](../../indexes/indexing-spatial-data) article. /}

## Ordering

In order to sort the results by distance, please use the `OrderByDistance` or `OrderByDistanceDescending` methods. You can read more about them [here](../../client-api/session/querying/how-to-query-a-spatial-index).

## Remarks

{INFO Distance in RavenDB by default is measured in **kilometers**. /}

## Related Articles

### Indexes

- [Indexing Spatial Data](../../indexes/indexing-spatial-data)

### Client API

- [How to Query a Spatial Index](../../client-api/session/querying/how-to-query-a-spatial-index)
