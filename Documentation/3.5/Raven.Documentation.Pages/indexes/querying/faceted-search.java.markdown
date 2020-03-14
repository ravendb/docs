# Faceted Search

When displaying a large amount of data, often paging is used to make viewing the data manageable. However it's also useful to give some context of the entire data-set and a easy way to drill-down into particular categories. The common approach to doing this is "faceted search", as shown in the image below. __Note__ how the count of each category within the current search is across the top.

![Facets](images\CNET_faceted_search_2.jpg)

<br />
To achieve this in RavenDB, let's say you have a document like this:

{CODE:java camera@samples/Camera.java /}

## Step 1

Create an index to work against, this can be setup like so:

{CODE:java step_2@Indexes\Querying\FacetedSearch.java /}

## Step 2

Next you need to setup your facet definitions:

{CODE:java step_1@Indexes\Querying\FacetedSearch.java /}

This tells RavenDB that you would like to get the following facets:

* For the **Manufacturer** field look at the documents and return a count for each unique Term found
* For the **Cost** field, return the count of the following ranges:
 * Cost <= 200.0
 * 200.0 <= Cost <= 400.0
 * 400.0 <= Cost <= 600.0
 * 600.0 <= Cost <= 800.0
 * Cost >= 800.0
* For the **Megapixels** field, return the count of the following ranges:
 * Megapixels <= 3.0
 * 3.0 <= Megapixels <= 7.0
 * 7.0 <= Megapixels <= 10.0
 * Megapixels >= 10.0

## Step 3

Finally you can write the following code and you get back the data below:

{CODE-TABS}
{CODE-TAB:java:Query step_3_0@Indexes\Querying\FacetedSearch.java /}
{CODE-TAB:java:DocumentQuery step_3_1@Indexes\Querying\FacetedSearch.java /}
{CODE-TAB:java:Commands step_3_2@Indexes\Querying\FacetedSearch.java /}
{CODE-TAB:java:Index step_1@Indexes\Querying\FacetedSearch.java /}
{CODE-TABS/}

The data below represents the sample faceted data that satisfies above query:

{CODE-BLOCK:json}
{
   Manufacturer: [
      {
         Range: 'canon',
         Count: 42
      },
      {
         Range: 'jessops',
         Count: 50
      },
      {
         Range: 'nikon',
         Count: 46
      },
      {
         Range: 'phillips',
         Count: 44
      },
      {
         Range: 'sony',
         Count: 35
      }
   ],
   Cost_Range: [
      {
         Range: '[NULL TO Dx200.0]',
         Count: 115
      },
      {
         Range: '[Dx200.0 TO Dx400.0]',
         Count: 102
      }
   ],
   Megapixels_Range: [
      {
         Range: '[NULL TO Dx3.0]',
         Count: 42
      },
      {
         Range: '[Dx3.0 TO Dx7.0]',
         Count: 79
      },
      {
         Range: '[Dx7.0 TO Dx10.0]',
         Count: 82
      },
      {
         Range: '[Dx10.0 TO NULL]',
         Count: 14
      }
   ]
}
{CODE-BLOCK/}

### Storing facets

Alternatively, if you do not have to change your facets dynamically, you can store your facets as `FacetSetup` document and pass the document Id instead of the list each time:

{CODE:java step_4_0@Indexes\Querying\FacetedSearch.java /}

{CODE-TABS}
{CODE-TAB:java:Query step_4_1@Indexes\Querying\FacetedSearch.java /}
{CODE-TAB:java:DocumentQuery step_4_2@Indexes\Querying\FacetedSearch.java /}
{CODE-TAB:java:Commands step_4_3@Indexes\Querying\FacetedSearch.java /}
{CODE-TAB:java:Index step_1@Indexes\Querying\FacetedSearch.java /}
{CODE-TABS/}

### Stale results

The faceted search does not take into account a staleness of an index. You can't wait for non stale results by customizing your query with one of `waitForNonStaleResultsXXX` method.

## Related articles

- [Querying : Dynamic aggregation](../../indexes/querying/dynamic-aggregation)
- [Client API : Session : How to perform a faceted search?](../../client-api/session/querying/how-to-perform-a-faceted-search)
