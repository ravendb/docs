# Migration: How to Migrate Facets and Dynamic Aggregation from 3.x

Facets and Dynamic Aggregation have been merged into a single feature and are now a part of [RQL](../../../../indexes/querying/what-is-rql).

{PANEL:Facets}

Facets are now divided into two types:

- `Facet` where the whole spectrum of results will generate a single outcome
- `RangeFacet` gives you the ability to split the whole spectrum of results into smaller ranges

The `FacetSetup` document also now splits the facets into two properties. One containing `Facets`, the other containing `RangeFacets`.

### Example

| 3.x |
|:---:|
| {CODE facets_1_0@Migration\ClientApi\Session\Querying\Facets.cs /} |

| 4.0 |
|:---:|
| {CODE facets_1_1@Migration\ClientApi\Session\Querying\Facets.cs /} |

---

{PANEL/}

{PANEL:Querying}

All of the method were substituted with `AggregateBy` and `AggregateUsing`.

### Example I

| 3.x |
|:---:|
| {CODE facets_2_0@Migration\ClientApi\Session\Querying\Facets.cs /} |

| 4.0 |
|:---:|
| {CODE facets_2_1@Migration\ClientApi\Session\Querying\Facets.cs /} |

### Example II

| 3.x |
|:---:|
| {CODE facets_3_0@Migration\ClientApi\Session\Querying\Facets.cs /} |

| 4.0 |
|:---:|
| {CODE facets_3_1@Migration\ClientApi\Session\Querying\Facets.cs /} |

{PANEL/}

## Remarks

{INFO You can read more about Facets in our dedicated [Querying article](../../../../indexes/querying/faceted-search), our [Indexing article](../../../../indexes/querying/faceted-search) or [Client API article](../../../../client-api/session/querying/how-to-query-a-spatial-index). /}
