# Indexes: Indexing Spatial Data

To support the ability to retrieve the data based on spatial coordinates, the spatial search has been introduced.

{INFO This article describes how to setup a spatial field in static index. If you are interested in an automatic approach, please visit relevant spatial querying article that can be found [here](../indexes/querying/spatial). /}

## Creating Indexes

To take an advantage of the spatial search, first we need to create an index with a spatial field. To mark field as the spatial field, we need to use the `CreateSpatialField` method:

{CODE:java spatial_search_0@Indexes\SpatialIndexes.java /}

Where:   
     
*	**lat/lng** are latitude/longitude coordinates   
*	**shapeWKT** is a shape in the [WKT](https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry) format    

### Example

{CODE-TABS}
{CODE-TAB:java:Coordinates spatial_search_1@Indexes\SpatialIndexes.java /}
{CODE-TAB:java:WKT spatial_search_2@Indexes\SpatialIndexes.java /}
{CODE-TAB:java:JavaScript spatial_search_1@Indexes\JavaScript.java /}
{CODE-TABS/}

### Options

RavenDB supports both the `Geography` and `Cartesian` systems and multiple strategies for each one of them.

{CODE:java spatial_search_enhancements_3@Indexes\SpatialIndexes.java /}

{CODE-TABS}
{CODE-TAB:java:GeographySpatialOptionsFactory spatial_search_enhancements_4@Indexes\SpatialIndexes.java /}
{CODE-TAB:java:CartesianSpatialOptionsFactory spatial_search_enhancements_5@Indexes\SpatialIndexes.java /}
{CODE-TABS/}

### Changing Default Behavior

By default, if no action is taken, the `GeohashPrefixTree` strategy is used with `GeohashLevel` set to **9**. This behavior can be changed by using the `spatial()` method from `AbstractIndexCreationTask`

{CODE:java spatial_search_3@Indexes\SpatialIndexes.java /}

## Spatial search strategies

{PANEL:GeohashPrefixTree}
Geohash is a latitude/longitude representation system that describes earth as a grid with 32 cells, assigning an alphanumeric character to each grid cell. Each grid cell is further divided into 32 smaller chunks, and each chunk has an alphanumeric character assigned as well, and so on.

E.g. The location of 'New York' in the United States is represented by the following geohash: [DR5REGY6R](http://geohash.org/dr5regy6r) and it represents the `40.7144 -74.0060` coordinates. Removing characters from the end of geohash will decrease the precision level.

More information about geohash uses, decoding algorithm and limitations can be found [here](https://en.wikipedia.org/wiki/Geohash).
{PANEL/}

{PANEL:QuadPrefixTree}
QuadTree represents the earth as a grid with exactly four cells and similarly to geohash, each grid cell (sometimes called a bucket) has a letter assigned, and is divided further into 4 more cells and so on.

More information about QuadTree can be found [here](https://en.wikipedia.org/wiki/Quadtree).
{PANEL/}

{PANEL:BoundingBox}
More information about BoundingBox can be found [here](https://en.wikipedia.org/wiki/Minimum_bounding_rectangle).
{PANEL/}

{WARNING `GeohashPrefixTree` is a default `SpatialSearchStrategy`. Doing any changes to the strategy after an index has been created will trigger the re-indexation process. /}

### Precision

By default, the precision level (`maxTreeLevel`) for GeohashPrefixTree is set to **9** and for QuadPrefixTree the value is **23**. This means that the coordinates are represented by a 9 or 23 character string. The difference exists because the `QuadTree` representation would be much less precise if the level would be the same.

{PANEL:Geohash precision values}
Source: [http://unterbahn.com](http://unterbahn.com/2009/11/metric-dimensions-of-geohash-partitions-at-the-equator/)

| Level | E-W Distance at Equator | N-S Distance at Equator |
|:----- |:------------------------|:------------------------|
| 12    | ~3.7cm                  | ~1.8cm                  |
| 11    | ~14.9cm                 | ~14.9cm                 |
| 10    | ~1.19m                  | ~0.60m                  |
| **9** | **~4.78m**              | **~4.78m**              |
| 8     | ~38.2m                  | ~19.1m                  |
| 7     | ~152.8m                 | ~152.8m                 |
| 6     | ~1.2km                  | ~0.61km                 |
| 5     | ~4.9km                  | ~4.9km                  |
| 4     | ~39km                   | ~19.6km                 |
| 3     | ~157km                  | ~157km                  |
| 2     | ~1252km                 | ~626km                  |
| 1     | ~5018km                 | ~5018km                 |

{PANEL/}

{PANEL:Quadtree precision values}

| Level | Distance at Equator |
|:-------|:-------------------|
| 30     | ~4cm               |
| 29     | ~7cm               |
| 28     | ~15cm              |
| 27     | ~30cm              |
| 26     | ~60cm              |
| 25     | ~1.19m             |
| 24     | ~2.39m             |
| **23** | **~4.78m**         |
| 22     | ~9.56m             |
| 21     | ~19.11m            |
| 20     | ~38.23m            |
| 19     | ~76.23m            |
| 18     | ~152.92m           |
| 17     | ~305.84m           |
| 16     | ~611.67m           |
| 15     | ~1.22km            |
| 14     | ~2.45km            |
| 13     | ~4.89km            |
| 12     | ~9.79km            |
| 11     | ~19.57km           |
| 10     | ~39.15km           |
| 9      | ~78.29km           |
| 8      | ~156.58km          |
| 7      | ~313.12km          |
| 6      | ~625.85km          |
| 5      | ~1249km            |
| 4      | ~2473km            |
| 3      | ~4755km            |
| 2      | ~7996km            |
| 1      | ~15992km           |

{PANEL/}

## Remarks

{INFO You can read more about **spatial search** in a **dedicated querying article** available [here](../indexes/querying/spatial). /}

{WARNING Distance by default is measured in **kilometers**. /}

## Related Articles

### Querying

- [Spatial](../indexes/querying/spatial)

### Client API

- [How to Query a Spatial Index](../client-api/session/querying/how-to-query-a-spatial-index)
