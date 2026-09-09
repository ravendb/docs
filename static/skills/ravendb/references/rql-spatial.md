# RQL — spatial

Geospatial filtering and distance sorting over point/shape fields. Dynamic spatial queries auto-create a spatial auto-index; for heavy use define a static spatial index and query `from index '…'`. Version-correct details: ravendb.net/docs.

## Shape & point constructors
| Constructor | Meaning |
|---|---|
| `spatial.point(lat, lng)` | a point — argument order is **latitude, longitude** |
| `spatial.circle(radius, lat, lng [, units])` | a circle; `units` = `kilometers` (default) or `miles` |
| `spatial.wkt('POLYGON((…))')` | any shape from Well-Known Text |

## Relation predicates (WHERE)
`spatial.within(field, shape)` · `spatial.contains(field, shape)` · `spatial.intersects(field, shape)` · `spatial.disjoint(field, shape)`

```
# within a radius
from Restaurants
where spatial.within(spatial.point(Latitude, Longitude),
                     spatial.circle(5, 40.7128, -74.0060))          # 5 km around the point

# within a radius in miles
where spatial.within(spatial.point(Latitude, Longitude),
                     spatial.circle(5, 40.7128, -74.0060, 'miles'))

# within an arbitrary polygon
where spatial.within(spatial.point(Lat, Lng),
                     spatial.wkt('POLYGON((-74.1 40.6, -73.9 40.6, -73.9 40.8, -74.1 40.8, -74.1 40.6))'))
```

## Sort by distance
```
from Events
where spatial.within(Coordinates, spatial.circle(20, 47.623473, -122.3060097))
order by spatial.distance(Coordinates, spatial.point(47.623473, -122.3060097))
```
- `spatial.distance(field, point [, roundFactor])` — ascending = closest first. The optional third argument ROUNDS the distance (grouping nearby results), it is NOT units — units follow the shape/index config, default **kilometers**. Combine with `asc`/`desc` and other sort keys.

## Notes & gotchas
- Argument order is **lat, lng** for `spatial.point`/`spatial.circle` — but **WKT is `x y` = lng lat** (see the POLYGON above). Swapping either is the classic mistake.
- The distance value is in kilometers unless the filtering shape specified `'miles'`.
- Point fields must be indexable as spatial; a dynamic query builds a spatial auto-index on first use. Define a static spatial index for repeated/large workloads.
- `spatial.contains`/`disjoint`/`intersects` are documented primarily via the client on some versions — confirm exact behavior at ravendb.net/docs.
