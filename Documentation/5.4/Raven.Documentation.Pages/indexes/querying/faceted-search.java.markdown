# Querying: Faceted (Aggregation) Search

When displaying a large amount of data, paging is often used to make viewing the data manageable. 
It's also useful to give some context of the entire data-set and a easy way to drill-down into 
particular categories. The common approach to doing this is a "faceted search", as shown in the 
image below. **Note** how the count of each category within the current search is across the top.

![Facets](images\CNET_faceted_search.jpg)

<br />
Let's start with defining a document like this:

{CODE:java camera@Indexes\Querying\Faceted.java /}

## Step 1

Create an index to work against. 

{CODE:java step_2@Indexes\Querying\FacetedSearch.java /}

## Step 2

Setup your facet definitions:

{CODE:java step_1@Indexes\Querying\FacetedSearch.java /}

This tells RavenDB that you would like to get the following facets:

* For the **manufacturer** field, look at the documents and return a count for each unique Term found.

* For the **cost** field, return the count of the following ranges:

 * cost < 200.0
 * 200.0 <= cost < 400.0
 * 400.0 <= cost < 600.0
 * 600.0 <= cost < 800.0
 * cost >= 800.0
* For the **megapixels** field, return the count of the following ranges:
 * megapixels <= 3.0
 * 3.0 <= megapixels < 7.0
 * 7.0 <= megapixels < 10.0
 * megapixels >= 10.0

## Step 3

You can write the following code to get back the data below:

{CODE-TABS}
{CODE-TAB:java:Query step_3_0@Indexes\Querying\FacetedSearch.java /}
{CODE-TAB:java:Facets step_1@Indexes\Querying\FacetedSearch.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Cameras/ByManufacturerModelCostDateOfListingAndMegapixels' 
where cost between 100 and 300
select facet(manufacturer), facet(cost <= 200, cost between 200 and 400, cost between 400 and 600, cost between 600 and 800, cost >= 800), facet(megapixels <= 3, megapixels between 3 and 7, megapixels between 7 and 10, megapixels >= 10)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

This data represents the sample faceted data that satisfies the above query:

{CODE-BLOCK:json}
[
    {
        "Name": "manufacturer",
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
        "Name": "cost",
        "Values": [
            {
                "Count": 6,
                "Range": "cost <= 200"
            },
            {
                "Count": 2,
                "Range": "cost between 200 and 400"
            },
            {
                "Count": 0,
                "Range": "cost between 400 and 600"
            },
            {
                "Count": 0,
                "Range": "cost between 600 and 800"
            },
            {
                "Count": 0,
                "Range": "cost >= 800"
            }
        ]
    },
    {
        "Name": "megapixels",
        "Values": [
            {
                "Count": 0,
                "Range": "megapixels <= 3"
            },
            {
                "Count": 6,
                "Range": "megapixels between 3 and 7"
            },
            {
                "Count": 1,
                "Range": "megapixels between 7 and 10"
            },
            {
                "Count": 1,
                "Range": "megapixels >= 10"
            }
        ]
    }
]
{CODE-BLOCK/}

### Storing Facets

If you do not have to change your facets dynamically, you can store your facets as a `FacetSetup` document and pass the document ID instead of the list each time:

{CODE:java step_4_0@Indexes\Querying\FacetedSearch.java /}

{CODE-TABS}
{CODE-TAB:java:Query step_4_1@Indexes\Querying\FacetedSearch.java /}
{CODE-TAB:java:Facets step_1@Indexes\Querying\FacetedSearch.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Cameras/ByManufacturerModelCostDateOfListingAndMegapixels' 
where cost between 100 and 300
select facet(id('facets/CameraFacets'))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Stale Results

The faceted search does not take into account a staleness of an index. You can wait for non stale results by customizing your query with the `waitForNonStaleResults` method.

### Fluent API

As an alternative for creating a list of facets and passing it to the `aggregateBy` method, RavenDB also exposes a dynamic API where you can create your facets using a builder. You can read more about those methods in our dedicated Client API article [here](../../client-api/session/querying/how-to-perform-a-faceted-search).

## Related Articles

### Client API

- [How to Perform a Faceted Search](../../client-api/session/querying/how-to-perform-a-faceted-search)
