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
{CODE-TAB:python:Class camera_class@Indexes\Querying\FacetedSearch.py /}
{CODE-TAB:python:Index camera_index@Indexes\Querying\FacetedSearch.py /}
{CODE-TAB:python:Sample_data camera_sample_data@Indexes\Querying\FacetedSearch.py /}
{CODE-TABS/}

{PANEL/}

{PANEL: Facets - Basics}

#### Facets definition:

* Define a list of facets by which to aggregate the data.

* There are two **Facet types**:
    * `Facet` - returns a count for each unique term found in the specified index-field.
    * `RangeFacet` - returns a count per range within the specified index-field.
  
{CODE:python facets_1@Indexes\Querying\FacetedSearch.py /}

---

#### Query the index for facets results:

* Query the index to get the aggregated facets information.  

* Either:  

  * Pass the facets definition from above directly to the query  
  
  * Or - construct a facet using a builder with the Fluent API option, as shown below.

{CODE-TABS}
{CODE-TAB:python:Query facets_2@Indexes\Querying\FacetedSearch.py /}
{CODE-TAB:python:Query_FluentAPI facets_5@Indexes\Querying\FacetedSearch.py /}
{CODE-TAB:python:RawQuery facets_2_rawQuery@Indexes\Querying\FacetedSearch.py /}
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

{CODE:python facets_6@Indexes\Querying\FacetedSearch.py /}
{CODE:python facets_7@Indexes\Querying\FacetedSearch.py /}

{INFO: }

**Query further**:

* Typically, after presenting users with the initial facets results which show the available options,  
  users can select specific categories to explore further.

* For example, if the user selects Fuji and Nikon,  
  then your next query can include a filter to focus only on those selected brands.

{CODE:python facets_8@Indexes\Querying\FacetedSearch.py /}

{INFO/}

{PANEL/}

{PANEL: Facets - Options}

#### Facets definition:

* **Options** are available only for the `Facet` type.  

* Available options:  

    * `start` - The position from which to send items (how many to skip).  
    * `page_size` - Number of items to return.  
    * `include_remaining_terms` - Show summary of items that didn't make it into the requested PageSize.  
    * `term_sort_mode` - Set the sort order on the resulting items. 

{CODE:python facets_9@Indexes\Querying\FacetedSearch.py /}

---

#### Query the index for facets results:

{CODE-TABS}
{CODE-TAB:python:Query facets_10@Indexes\Querying\FacetedSearch.py /}
{CODE-TAB:python:Query_FluentAPI facets_13@Indexes\Querying\FacetedSearch.py /}
{CODE-TAB:python:RawQuery facets_10_rawQuery@Indexes\Querying\FacetedSearch.py /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Cameras/ByFeatures"
select facet(Brand, $p0)
{"p0": { "TermSortMode": "CountDesc", "PageSize": 3 }}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

---

#### Query results:

{CODE:python facets_14@Indexes\Querying\FacetedSearch.py /}
{CODE:python facets_15@Indexes\Querying\FacetedSearch.py /}

{PANEL/}

{PANEL: Facets - Aggregations}

#### Facets definition:

* Aggregation of data is available for an index-field per unique Facet or Range item.  
  For example:  
  * Get the total number of UnitsInStock per Brand  
  * Get the highest MegaPixels value for documents that cost between 200 & 400   

* The following aggregation operations are available:  
  * Sum
  * Average
  * Min
  * Max

* Multiple operations can be added on each facet, for multiple fields.

{CODE:python facets_16@Indexes\Querying\FacetedSearch.py /}

---

#### Query the index for facets results:

{CODE-TABS}
{CODE-TAB:python:Query facets_17@Indexes\Querying\FacetedSearch.py /}
{CODE-TAB:python:Query_FluentAPI facets_20@Indexes\Querying\FacetedSearch.py /}
{CODE-TAB:python:RawQuery facets_17_rawQuery@Indexes\Querying\FacetedSearch.py /}
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

{CODE:python facets_21@Indexes\Querying\FacetedSearch.py /}
{CODE:python facets_22@Indexes\Querying\FacetedSearch.py /}

{PANEL/}

{PANEL: Storing facets definition in a document}

#### Define and store facets in a document:

* The facets definitions can be stored in a document.

* That document can then be used by a faceted search query.

{CODE:python facets_23@Indexes\Querying\FacetedSearch.py /}

---

#### Query using facets from document:

{CODE-TABS}
{CODE-TAB:python:Query facets_24@Indexes\Querying\FacetedSearch.py /}
{CODE-TAB:python:RawQuery facets_24_rawQuery@Indexes\Querying\FacetedSearch.py /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Cameras/ByFeatures"
select facet(id("customDocumentID"))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE:python syntax_1@Indexes\Querying\FacetedSearch.py /}

| Parameter                    | Type                       | Description           |
|------------------------------|----------------------------|-----------------------|
| **builder_or_facet** (Union) | `Callable[[FacetBuilder]`<br>**or**<br>`FacetBase` | Builder with a fluent API that constructs a `FacetBase` implementation instance<br>**or**<br>`FacetBase` implementation instance |
| **facets** | `List[FacetBase]` | A list of `FacetBase` implementations instances. |
| **facet_setup_document_id**  | `str` | ID of a document containing `FacetSetup` |

{CODE-TABS}
{CODE-TAB:python:Facet syntax_2@Indexes\Querying\FacetedSearch.py /}
{CODE-TAB:python:RangeFacet syntax_3@Indexes\Querying\FacetedSearch.py /}
{CODE-TAB:python:FacetBase syntax_4@Indexes\Querying\FacetedSearch.py /}
{CODE-TAB:python:FacetAggregation syntax_5@Indexes\Querying\FacetedSearch.py /}
{CODE-TABS/}

**Fluent API builder methods**:

{CODE:python syntax_6@Indexes\Querying\FacetedSearch.py /}

| Parameter        | Type                        | Description |
|------------------|-----------------------------|-------------|
| **range_**       | `RangeBuilder` | A range of indexes |
| **\*ranges**     | `RangeBuilder` | Multiple index ranges (at least one), separated by `,` |
| **field_name**   | `str` | The index-field to use for the facet |
| **path**         | `str` | Points to the index-field to use for the facet (`ByRanges`, `ByField`) or for the aggregation (`SUM_ON`, `MIN_ON`, `MAX_ON`, `AVERAGE_ON`) |
| **display_name** | `str` | If set, results of a facet will be returned under this name |
| **options**      | `FacetOptions` | Non-default options to use in the facet definition |



**Options**:

{CODE:python syntax_7@Indexes\Querying\FacetedSearch.py /}

| Option                    | Type                | Description                                                                                                 |
|---------------------------|---------------------|-------------------------------------------------------------------------------------------------------------|
| **term_sort_mode**          | `FacetTermSortMode` | Set the sort order on the resulting items<br>(`VALUE_ASC` (Default), `VALUE_DESC`, `COUNT_ASC`, `COUNT_DESC`) |
| **start**                 | `int` | The position from which to send items (how many to skip) |
| **page_size**              | `int` | Number of items to return |
| **include_remaining_terms** | `bool` | Indicates if remaining terms that didn't make it into the requested PageSize should be included in results<br>Default value: `False` |

{PANEL/}

## Related Articles

### Client API

- [Query Overview](../../client-api/session/querying/how-to-query)
