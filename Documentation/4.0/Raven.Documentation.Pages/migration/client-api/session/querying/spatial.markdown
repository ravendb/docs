# Migration: How to Migrate Spatial from 3.x

Spatial functionality has been merged into [RQL](../../../../indexes/querying/what-is-rql). To reflect that change, the Client API has integrated this feature into the `session.Query` and `session.Advanced.DocumentQuery`. The following migration samples will focus on the `session.Query` - the most common and recommended way of interaction with querying capabilities on RavenDB.

## Namespaces

| 3.x |
|:---:|
| {CODE spatial_1@Migration\ClientApi\Session\Querying\Spatial.cs /} |

| 4.0 |
|:---:|
| {CODE spatial_2@Migration\ClientApi\Session\Querying\Spatial.cs /} |

## Example I - Index

The following changes have been applied:

1. All spatial fields must be created using the `CreateSpatialField` method
2. No support for GeoJSON and other non-standard formats
3. No support for spatial clustering

| 3.x |
|:---:|
| {CODE spatial_index_1@Migration\ClientApi\Session\Querying\Spatial.cs /} |

| 4.0 |
|:---:|
| {CODE spatial_index_2@Migration\ClientApi\Session\Querying\Spatial.cs /} |

## Example II - RelatesToShape

| 3.x |
|:---:|
| {CODE spatial_3@Migration\ClientApi\Session\Querying\Spatial.cs /} |

| 4.0 |
|:---:|
| {CODE spatial_4@Migration\ClientApi\Session\Querying\Spatial.cs /} |

## Example III - WithinRadiusOf

| 3.x |
|:---:|
| {CODE spatial_5@Migration\ClientApi\Session\Querying\Spatial.cs /} |

| 4.0 |
|:---:|
| {CODE spatial_6@Migration\ClientApi\Session\Querying\Spatial.cs /} |

## Example IV - SortByDistance

| 3.x |
|:---:|
| {CODE spatial_7@Migration\ClientApi\Session\Querying\Spatial.cs /} |

| 4.0 |
|:---:|
| {CODE spatial_8@Migration\ClientApi\Session\Querying\Spatial.cs /} |

## Remarks

{INFO You can read more about Spatial in our dedicated [Client API article](../../../../client-api/session/querying/how-to-query-a-spatial-index) or our [Indexing article](../../../../indexes/indexing-spatial-data). /}
