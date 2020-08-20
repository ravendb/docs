# Querying: Faceted (Aggregation) Search

When displaying a large amount of data, paging is often used to make viewing the data manageable. It's also useful to give some context of the entire data-set and a easy way to drill-down into particular categories. The common approach to doing this is a "faceted search", as shown in the image below. __Note__ how the count of each category within the current search is across the top.

![Facets](images\CNET_faceted_search_2.jpg)

<br />
Let's start with defining a document like this:

{CODE camera@Faceted.cs /}

## Step 1

Create an index to work against. 

{CODE step_2@Indexes\Querying\FacetedSearch.cs /}

## Step 2

Setup your facet definitions:

{CODE step_1@Indexes\Querying\FacetedSearch.cs /}

This tells RavenDB that you would like to get the following facets:

* For the **Manufacturer** field, look at the documents and return a count for each unique Term found.

* For the **Cost** field, return the count of the following ranges:

 * Cost < 200.0
 * 200.0 <= Cost < 400.0
 * 400.0 <= Cost < 600.0
 * 600.0 <= Cost < 800.0
 * Cost >= 800.0
* For the **Megapixels** field, return the count of the following ranges:
 * Megapixels <= 3.0
 * 3.0 <= Megapixels < 7.0
 * 7.0 <= Megapixels < 10.0
 * Megapixels >= 10.0

## Step 3

You can write the following code to get back the data below:

{CODE-TABS}
{CODE-TAB:csharp:Query step_3_0@Indexes\Querying\FacetedSearch.cs /}
{CODE-TAB:csharp:DocumentQuery step_3_1@Indexes\Querying\FacetedSearch.cs /}
{CODE-TAB:csharp:Facets step_1@Indexes\Querying\FacetedSearch.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Cameras/ByManufacturerModelCostDateOfListingAndMegapixels' 
where Cost between 100 and 300
select facet(Manufacturer), 
       facet(Cost <= 200, 
             Cost between 200 and 400, 
             Cost between 400 and 600, Cost between 600 and 800, 
             Cost >= 800), 
       facet(Megapixels <= 3, 
             Megapixels between 3 and 7, 
             Megapixels between 7 and 10, 
             Megapixels >= 10)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

This data represents the sample faceted data that satisfies the above query:

{CODE-BLOCK:json}
[
    {
        "Name": "Manufacturer",
        "Values": [
            {
                "Count": 1,
                "Range": "canon"
            },
            {
                "Count": 2,
                "Range": "jessops"
            },
            {
                "Count": 1,
                "Range": "nikon"
            },
            {
                "Count": 1,
                "Range": "phillips"
            },
            {
                "Count": 3,
                "Range": "sony"
            }
        ]
    },
    {
        "Name": "Cost",
        "Values": [
            {
                "Count": 6,
                "Range": "Cost <= 200"
            },
            {
                "Count": 2,
                "Range": "Cost between 200 and 400"
            },
            {
                "Count": 0,
                "Range": "Cost between 400 and 600"
            },
            {
                "Count": 0,
                "Range": "Cost between 600 and 800"
            },
            {
                "Count": 0,
                "Range": "Cost >= 800"
            }
        ]
    },
    {
        "Name": "Megapixels",
        "Values": [
            {
                "Count": 0,
                "Range": "Megapixels <= 3"
            },
            {
                "Count": 6,
                "Range": "Megapixels between 3 and 7"
            },
            {
                "Count": 1,
                "Range": "Megapixels between 7 and 10"
            },
            {
                "Count": 1,
                "Range": "Megapixels >= 10"
            }
        ]
    }
]
{CODE-BLOCK/}

### Storing Facets

If you do not have to change your facets dynamically, you can store your facets as a `FacetSetup` document and pass the document ID instead of the list each time:

{CODE step_4_0@Indexes\Querying\FacetedSearch.cs /}

{CODE-TABS}
{CODE-TAB:csharp:Query step_4_1@Indexes\Querying\FacetedSearch.cs /}
{CODE-TAB:csharp:DocumentQuery step_4_2@Indexes\Querying\FacetedSearch.cs /}
{CODE-TAB:csharp:Facets step_1@Indexes\Querying\FacetedSearch.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Cameras/ByManufacturerModelCostDateOfListingAndMegapixels' 
where Cost between 100 and 300
select facet(id('facets/CameraFacets'))
{CODE-TAB-BLOCK/}
{CODE-TABS/}
<br/>
### Stale Results

The faceted search does not take into account a staleness of an index. You can wait for non stale results by customizing your query with the `WaitForNonStaleResults` method.

### Fluent API

As an alternative for creating a list of facets and passing it to the `AggregateBy` method, RavenDB also exposes a dynamic API where you can create your facets using a builder. You can read more about those methods in our dedicated Client API article [here](../../client-api/session/querying/how-to-perform-a-faceted-search).

## Related Articles

### Client API

- [How to Perform a Faceted Search](../../client-api/session/querying/how-to-perform-a-faceted-search)
