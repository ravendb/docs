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
{CODE-TAB:nodejs:Class camera_class@Indexes\Querying\facetedSearch.js /}
{CODE-TAB:nodejs:Index camera_index@Indexes\Querying\facetedSearch.js /}
{CODE-TAB:nodejs:Sample_data camera_sample_data@Indexes\Querying\facetedSearch.js /}
{CODE-TABS/}

{PANEL/}

{PANEL: Facets - Basics}

{NOTE: }

**Facets definition**:

---

* Define a list of facets by which to aggregate the data.

* There are two **Facet types**:
    * `Facet` - returns a count for each unique term found in the specified index-field.
    * `RangeFacet` - returns a count per range within the specified index-field.
  
{CODE:nodejs facets_1@Indexes\Querying\facetedSearch.js /}

{NOTE/}

{NOTE: }

**Query the index for facets results**:

---

* Query the index to get the aggregated facets information.  

* Either:  

  * Pass the facets definition from above directly to the query  
  
  * Or - construct a facet using a builder with the Fluent API option, as shown below.

{CODE-TABS}
{CODE-TAB:nodejs:Query facets_2@Indexes\Querying\facetedSearch.js /}
{CODE-TAB:nodejs:Query_FluentAPI facets_3@Indexes\Querying\facetedSearch.js /}
{CODE-TAB:nodejs:RawQuery facets_2_rawQuery@Indexes\Querying\facetedSearch.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Cameras/ByFeatures"
select
    facet(brand) as "Camera Brand",
    facet(price < 200,
          price >= 200 and price < 400,
          price >= 400 and price < 600,
          price >= 600 and price < 800,
          price >= 800) as "Camera Price"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Query results**:

---

* **Query results** are Not the collection documents, they are of type `FacetResultObject`  
  which is the facets results per index-field specified.  

* Using the sample data from this article, the resulting aggregations will be:

{CODE:nodejs facets_4@Indexes\Querying\facetedSearch.js /}
{CODE:nodejs facets_5@Indexes\Querying\facetedSearch.js /}

{NOTE/}

{INFO: }

**Query further**:

---

* Typically, after presenting users with the initial facets results which show the available options,  
  users can select specific categories to explore further.

* For example, if the user selects Fuji and Nikon,  
  then your next query can include a filter to focus only on those selected brands.

{CODE:nodejs facets_6@Indexes\Querying\facetedSearch.js /}

{INFO/}

{PANEL/}

{PANEL: Facets - Options}

{NOTE: }

**Facets definition**:

---

* **Options** are available only for the `Facet` type.  

* Available options:  

    * `start` - The position from which to send items (how many to skip).  
    * `pageSize` - Number of items to return.  
    * `includeRemainingTerms` - Show summary of items that didn't make it into the requested pageSize.  
    * `termSortMode` - Set the sort order on the resulting items. 

{CODE:nodejs facets_7@Indexes\Querying\facetedSearch.js /}

{NOTE/}

{NOTE: }

**Query the index for facets results**:

---

{CODE-TABS}
{CODE-TAB:nodejs:Query facets_8@Indexes\Querying\facetedSearch.js /}
{CODE-TAB:nodejs:Query_FluentAPI facets_9@Indexes\Querying\facetedSearch.js /}
{CODE-TAB:nodejs:RawQuery facets_8_rawQuery@Indexes\Querying\facetedSearch.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Cameras/ByFeatures"
select facet(brand, $p0)
{"p0": { "termSortMode": "CountDesc", "pageSize": 3 }}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Query results**:

---

{CODE:nodejs facets_10@Indexes\Querying\facetedSearch.js /}
{CODE:nodejs facets_11@Indexes\Querying\facetedSearch.js /}

{NOTE/}

{PANEL/}

{PANEL: Facets - Aggregations}

{NOTE: }

**Facets definition**:

---

* Aggregation of data is available for an index-field per unique Facet or Range item.  
  For example:  
  * Get the total number of unitsInStock per Brand  
  * Get the highest megaPixels value for documents that cost between 200 & 400   

* The following aggregation operations are available:  
  * Sum
  * Average
  * Min
  * Max

* Multiple operations can be added on each facet, for multiple fields.

{CODE:nodejs facets_12@Indexes\Querying\facetedSearch.js /}

{NOTE/}

{NOTE: }

**Query the index for facets results**:

---

{CODE-TABS}
{CODE-TAB:nodejs:Query facets_13@Indexes\Querying\facetedSearch.js /}
{CODE-TAB:nodejs:Query_FluentAPI facets_14@Indexes\Querying\facetedSearch.js /}
{CODE-TAB:nodejs:RawQuery facets_13_rawQuery@Indexes\Querying\facetedSearch.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Cameras/ByFeatures"
select
    facet(brand,
          sum(unitsInStock),
          avg(price),
          min(price),
          max(megaPixels),
          max(maxFocalLength)),
    facet(price < 200,
          price >= 200 and price < 400,
          price >= 400 and price < 600,
          price >= 600 and price < 800,
          price >= 800,
          sum(unitsInStock),
          avg(price),
          min(price),
          max(megaPixels),
          max(maxFocalLength))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Query results**:

---

{CODE:nodejs facets_15@Indexes\Querying\facetedSearch.js /}
{CODE:nodejs facets_16@Indexes\Querying\facetedSearch.js /}

{NOTE/}

{PANEL/}

{PANEL: Storing facets definition in a document}



{NOTE: }

**Define and store facets in a document**:

---

* The facets definitions can be stored in a document.

* That document can then be used by a faceted search query.

{CODE:nodejs facets_17@Indexes\Querying\facetedSearch.js /}

{NOTE/}

{NOTE: }

**Query using facets from document**:

---

{CODE-TABS}
{CODE-TAB:nodejs:Query facets_18@Indexes\Querying\facetedSearch.js /}
{CODE-TAB:nodejs:RawQuery facets_18_rawQuery@Indexes\Querying\facetedSearch.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Cameras/ByFeatures"
select facet(id("customDocumentID"))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax_1@Indexes\Querying\facetedSearch.js /}

| Parameter                | Type                       | Description                                                                                       |
|--------------------------|----------------------------|---------------------------------------------------------------------------------------------------|
| **facet**                | `FacetBase`                | `FacetBase` implementation defining the facet and its options.<br>Either `Facet` or `RangeFacet`. |
| **...facet**             | `FacetBase[]`              | List containing `FacetBase` implementations.                                                      |
| **action** / **builder** | `(builder) => void`        | Builder with a fluent API that constructs a `FacetBase` instance.                                 |
| **facetSetupDocumentId** | `string`                   | ID of a document containing `FacetSetup`.                                                         |

{CODE-TABS}
{CODE-TAB:nodejs:Facet syntax_2@Indexes\Querying\facetedSearch.js /}
{CODE-TAB:nodejs:RangeFacet syntax_3@Indexes\Querying\facetedSearch.js /}
{CODE-TAB:nodejs:FacetBase syntax_4@Indexes\Querying\facetedSearch.js /}
{CODE-TABS/}

**builder methods**:

{CODE:nodejs syntax_5@Indexes\Querying\facetedSearch.js /}

| Parameter       | Type           | Description                                                                                                                  |
|-----------------|----------------|------------------------------------------------------------------------------------------------------------------------------|
| **fieldName**   | `string`       | The index-field to use for the facet                                                                                         |
| **path**        | `string`       | The index-field to use for the facet (`byRanges`, `byField`) or for the aggregation (`sumOn`, `minOn`, `maxOn`, `averageOn`) |
| **displayName** | `string`       | If set, results of a facet will be returned under this name                                                                  |
| **options**     | `FacetOptions` | Non-default options to use in the facet definition                                                                           |

**Options**:

{CODE:nodejs syntax_6@Indexes\Querying\facetedSearch.js /}

| Option                    | Type                | Description                                                                                                 |
|---------------------------|---------------------|-------------------------------------------------------------------------------------------------------------|
| **termSortMode**          | `FacetTermSortMode` | Set the sort order on the resulting items<br>(`ValueAsc` (Default), `ValueDesc`, `CountAsc`, `CountDesc`)   |
| **start**                 | `number`            | The position from which to send items (how many to skip)                                                    |
| **pageSize**              | `number`            | Number of items to return                                                                                   |
| **includeRemainingTerms** | `boolean`           | Indicates if remaining terms that didn't make it into the requested PageSize should be included in results  |

{PANEL/}

## Related Articles

### Client API

- [Query Overview](../../client-api/session/querying/how-to-query)
