# Session: Querying: How to Perform a Faceted (Aggregated) Search

To execute facet (aggregation) query via the session `query` method, use `aggregate_by`, 
`aggregate_by_facets`, or `aggregate_using`.  
This will scope you to the aggregation query builder, where you'll be allowed to define 
single or multiple facets for the query using a straightforward and fluent API.

## Syntax

{CODE:python facet_1@ClientApi\Session\Querying\HowToPerformFacetedSearch.py /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **builder_or_facet** | `Union[Callable[[FacetBuilder], None]` | **Builder** with a fluent API that constructs a `FacetBase` instance<br>**-or-**<br>**FacetBase** implementation defining the scope of the facet and its options (either `Facet` or `RangeFacet`) |
| **facets** | `List[FacetBase]` | Items containing `FacetBase` implementations |
| **facet_setup_document_id** | `str` | ID of a document containing `FacetSetup` |

### Facet & RangeFacet

{INFO:Facet vs RangeFacet}
`RangeFacet` allows you to split the results of the calculations into several ranges, in contrast to `Facet` where whole spectrum of results will be used to generate a single outcome.
{INFO/}

{CODE-TABS}
{CODE-TAB:python:Facet facet_7_3@ClientApi\Session\Querying\HowToPerformFacetedSearch.py /}
{CODE-TAB:python:RangeFacet facet_7_4@ClientApi\Session\Querying\HowToPerformFacetedSearch.py /}
{CODE-TAB:python:FacetAggregation facet_7_5@ClientApi\Session\Querying\HowToPerformFacetedSearch.py /}
{CODE-TABS/}

### Builder

{CODE:python facet_7_1@ClientApi\Session\Querying\HowToPerformFacetedSearch.py /}

| Parameters | | |
| ------------- | ------------- | ----- |
| ***ranges** | `RangeBuilder` | A list of aggregated ranges |
| **field_name** | `str` | Points to the index field that should be used for operation (`by_ranges`, `by_field`) or to document field that should be used for aggregation (`sum_on`, `min_on`, `max_on`, `average_on`) |
| **display_name** | `str` | If set, results of a facet will be returned under this name |
| **options** | `FacetOptions` | Non-default options that should be used for operation |
| **path** | `str` | Points to the index field that should be used for operation (`by_ranges`, `by_field`) or to document field that should be used for aggregation (`sum_on`, `min_on`, `max_on`, `average_on`) |

### Options

{CODE:python facet_7_2@ClientApi\Session\Querying\HowToPerformFacetedSearch.py /}

| Options | | |
| ------------- | ------------- | ----- |
| **term_sort_mode** | `FacetTermSortMode` | Indicates how terms should be sorted (`VALUE_ASC`, `VALUE_DESC`, `COUNT_ASC`, `COUNT_DESC`) |
| **include_remaining_terms** | `bool` | Indicates if remaining terms should be included in results |
| **start** | `Union[None, int]` | Used to skip given number of facet results in the outcome |
| **page_size** | `int` | Used to limit facet results to the given value |

## Example I

{CODE-TABS}
{CODE-TAB:python:Python facet_2_1@ClientApi\Session\Querying\HowToPerformFacetedSearch.py /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Camera/Costs' 
select 
facet(manufacturer), 
facet(cost < 200, cost >= 200 AND cost < 400, cost >= 400 AND cost < 600, cost >= 600 AND cost < 800, cost >= 800),
facet(megapixels < 3, megapixels >= 3 AND megapixels < 7, megapixels >= 7 AND megapixels < 10, megapixels >= 10)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Example II

{CODE-TABS}
{CODE-TAB:python:Python facet_3_1@ClientApi\Session\Querying\HowToPerformFacetedSearch.py /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Camera/Costs' 
select 
facet(manufacturer), 
facet(cost < 200, cost >= 200 AND cost < 400, cost >= 400 AND cost < 600, cost >= 600 AND cost < 800, cost >= 800),
facet(megapixels < 3, megapixels >= 3 AND megapixels < 7, megapixels >= 7 AND megapixels < 10, megapixels >= 10)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Example III

{CODE-TABS}
{CODE-TAB:python:Python facet_4_1@ClientApi\Session\Querying\HowToPerformFacetedSearch.py /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Camera/Costs' 
select facet(id('facets/CameraFacets'))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

---

{INFO: }
`aggregate_by` only supports aggregation by a single field.  
If you want to aggregate by multiple fields, emit a single field that contains all values. 
{INFO/}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)

### Indexes

- [Faceted Search](../../../indexes/querying/faceted-search) 
