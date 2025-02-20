# Query by Facets
---

{NOTE: }

* A **Faceted Search** provides an efficient way to explore and navigate through large datasets or search results.
 
* Multiple filters (facets) are applied to narrow down the search results according to different attributes or categories.

![Facets](images\CNET_faceted_search.jpg)

---

* In this page
   * [Define an index](../../indexes/querying/faceted-search#define-an-index)
   * [Facets - Basics](../../indexes/querying/faceted-search#facets---basics)
   * [Facets - Options](../../indexes/querying/faceted-search#facets---options)
   * [Facets - Aggregations](../../indexes/querying/faceted-search#facets---aggregations)
   * [Storing facets definition in a document](../../indexes/querying/faceted-search#storing-facets-definition-in-a-document)
   * [Syntax](../../indexes/querying/faceted-search#syntax)
   
{NOTE/}

---

{PANEL: Define an index}

* To make a faceted search, **a static-index must be defined** for the fields you want to query and apply facets on.

* The examples in this article will be based on the following Class, Index, and Sample Data:

{CODE-TABS}
{CODE-TAB:php:Class camera_class@Indexes\Querying\FacetedSearch.php /}
{CODE-TAB:php:Index camera_index@Indexes\Querying\FacetedSearch.php /}
{CODE-TAB:php:Sample_data camera_sample_data@Indexes\Querying\FacetedSearch.php /}
{CODE-TABS/}

{PANEL/}

{PANEL: Facets - Basics}

#### Facets definition:

* Define a list of facets to aggregate the data by.

* There are two **Facet types**:
    * `Facet` - returns a count for each unique term found in the specified index-field.
    * `RangeFacet` - returns a count per range within the specified index-field.
  
{CODE:php facets_1@Indexes\Querying\FacetedSearch.php /}

---

#### Query the index for facets results:

* Query the index to get the aggregated facets information.  

* Either:  

  * Pass the facets definition from above directly to the query  
  
  * Or - construct a facet using a builder with the Fluent API option, as shown below.

{CODE-TABS}
{CODE-TAB:php:Query facets_2@Indexes\Querying\FacetedSearch.php /}
{CODE-TAB:php:Query_FluentAPI facets_5@Indexes\Querying\FacetedSearch.php /}
{CODE-TAB:php:RawQuery facets_2_rawQuery@Indexes\Querying\FacetedSearch.php /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Cameras/ByFeatures"
select
    facet(Brand) as "Camera Brand",
    facet(Price < 200.0,
          Price >= 200.0 and Price < 400.0,
          Price >= 400.0 and Price < 600.0,
          Price >= 600.0 and Price < 800.0,
          Price >= 800.0) as "Camera Price"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

---

#### Query results:

* **Query results** are Not the collection documents, they are of type:  
  `Dict[str, FacetResult]` which is the facets results per index-field specified.  

* Using the sample data from this article, the resulting aggregations will be:

{CODE:php facets_6@Indexes\Querying\FacetedSearch.php /}
{CODE:php facets_7@Indexes\Querying\FacetedSearch.php /}

{INFO: }

**Query further**:

* Typically, after presenting users with the initial facets results which show the available options,  
  users can select specific categories to explore further.

* For example, if the user selects Fuji and Nikon,  
  then your next query can include a filter to focus only on those selected brands.

{CODE:php facets_8@Indexes\Querying\FacetedSearch.php /}

{INFO/}

{PANEL/}

{PANEL: Facets - Options}

#### Facets definition:

* **Options** are available only for the `Facet` type.  

* Available options:  
    * `Start` - The position from which to send items (how many to skip).  
    * `PageSize` - Number of items to return.  
    * `IncludeRemainingTerms` - Show summary of items that didn't make it into the requested PageSize.  
    * `TermSortMode` - Set the sort order on the resulting items. 

{CODE:php facets_9@Indexes\Querying\FacetedSearch.php /}

---

#### Query the index for facets results:

{CODE-TABS}
{CODE-TAB:php:Query facets_10@Indexes\Querying\FacetedSearch.php /}
{CODE-TAB:php:Query_FluentAPI facets_13@Indexes\Querying\FacetedSearch.php /}
{CODE-TAB:php:RawQuery facets_10_rawQuery@Indexes\Querying\FacetedSearch.php /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Cameras/ByFeatures"
select facet(Brand, $p0)
{"p0": { "TermSortMode": "CountDesc", "PageSize": 3 }}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

---

#### Query results:

{CODE:php facets_14@Indexes\Querying\FacetedSearch.php /}
{CODE:php facets_15@Indexes\Querying\FacetedSearch.php /}

{PANEL/}

{PANEL: Facets - Aggregations}

#### Facets definition:

* Aggregation of data is available for an index-field per unique Facet or Range item.  
  For example:  
  * Get the total number of UnitsInStock per Brand  
  * Get the highest MegaPixels value for documents that cost between 200 & 400   

* The following aggregation operations are available:  
  * sum
  * average
  * min
  * max

* Multiple operations can be added on each facet, for multiple fields.

{CODE:php facets_16@Indexes\Querying\FacetedSearch.php /}

---

#### Query the index for facets results:

{CODE-TABS}
{CODE-TAB:php:Query facets_17@Indexes\Querying\FacetedSearch.php /}
{CODE-TAB:php:Query_FluentAPI facets_20@Indexes\Querying\FacetedSearch.php /}
{CODE-TAB:php:RawQuery facets_17_rawQuery@Indexes\Querying\FacetedSearch.php /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Cameras/ByFeatures"
select
    facet(Brand,
          sum(UnitsInStock),
          avg(Price),
          min(Price),
          max(MegaPixels),
          max(MaxFocalLength)),
    facet(Price < $p0,
          Price >= $p1 and Price < $p2,
          Price >= $p3 and Price < $p4,
          Price >= $p5 and Price < $p6,
          Price >= $p7,
          sum(UnitsInStock),
          avg(Price),
          min(Price),
          max(MegaPixels),
          max(MaxFocalLength))
{"p0":200.0,"p1":200.0,"p2":400.0,"p3":400.0,"p4":600.0,"p5":600.0,"p6":800.0,"p7":800.0}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

---

#### Query results:

{CODE:php facets_21@Indexes\Querying\FacetedSearch.php /}
{CODE:php facets_22@Indexes\Querying\FacetedSearch.php /}

{PANEL/}

{PANEL: Storing facets definition in a document}

#### Define and store facets in a document:

* The facets definitions can be stored in a document.

* That document can then be used by a faceted search query.

{CODE:php facets_23@Indexes\Querying\FacetedSearch.php /}

---

#### Query using facets from document:

{CODE-TABS}
{CODE-TAB:php:Query facets_24@Indexes\Querying\FacetedSearch.php /}
{CODE-TAB:php:RawQuery facets_24_rawQuery@Indexes\Querying\FacetedSearch.php /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Cameras/ByFeatures"
select facet(id("customDocumentID"))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE:php syntax_1@Indexes\Querying\FacetedSearch.php /}

| Parameter                    | Type                       | Description           |
|------------------------------|----------------------------|-----------------------|
| **builderOrFacets** | `Callable` **or** `FacetBase` **or** `FacetBaseArray` **or** `array` | Builder with a fluent API that constructs a `FacetBase` implementation instance **or** `FacetBase` implementation instance |
| **facets** | `FacetBaseArray` **or** `array` | A list of `FacetBase` implementations instances. |
| **facetSetupDocumentId**  | `string ` | ID of a document containing `FacetSetup` |

{CODE-TABS}
{CODE-TAB:php:Facet syntax_2@Indexes\Querying\FacetedSearch.php /}
{CODE-TAB:php:RangeFacet syntax_3@Indexes\Querying\FacetedSearch.php /}
{CODE-TAB:php:FacetBase syntax_4@Indexes\Querying\FacetedSearch.php /}
{CODE-TAB:php:FacetAggregation syntax_5@Indexes\Querying\FacetedSearch.php /}
{CODE-TABS/}

**Fluent API builder methods**:

{CODE:php syntax_6@Indexes\Querying\FacetedSearch.php /}

| Parameter        | Type                        | Description |
|------------------|-----------------------------|-------------|
| **range** | `RangeBuilder` | A range of indexes |
| **ranges** | `RangeBuilder` | Multiple index ranges (at least one), separated by `,` |
| **fieldName** | `string` | The index-field to use for the facet |
| **path** | `string` | Points to the index-field to use for the facet (`ByRanges`, `ByField`) or for the aggregation (`SUM_ON`, `MIN_ON`, `MAX_ON`, `AVERAGE_ON`) |
| **displayName** | `string` | If set, results of a facet will be returned under this name |
| **options** | `FacetOptions` | Non-default options to use in the facet definition |



**Options**:

{CODE:php syntax_7@Indexes\Querying\FacetedSearch.php /}

| Option                    | Type                | Description                                                                                                 |
|---------------------------|---------------------|-------------------------------------------------------------------------------------------------------------|
| **termSortMode** | `FacetTermSortMode` | Set the sort order on the resulting items<br>(`VALUE_ASC` (Default), `VALUE_DESC`, `COUNT_ASC`, `COUNT_DESC`) |
| **start** | `int` | The position from which to send items (how many to skip) |
| **pageSize** | `int` | Number of items to return |
| **includeRemainingTerms** | `bool` | Indicates if remaining terms that didn't make it into the requested PageSize should be included in results<br>Default value: `False` |

{PANEL/}

## Related Articles

### Client API

- [Query Overview](../../client-api/session/querying/how-to-query)
