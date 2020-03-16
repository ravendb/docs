# Faceted Search

When displaying a large amount of data, often paging is used to make viewing the data manageable. However it's also useful to give some context of the entire data-set and a easy way to drill-down into particular categories. The common approach to doing this is "faceted search", as shown in the image below. __Note__ how the count of each category within the current search is across the top.

![Facets](images\CNET_faceted_search_2.jpg)

To achieve this in RavenDB, let's say you have a document like this:

{CODE-START:json /}
{ 
    DateOfListing: "2000-09-01T00:00:00.0000000+01:00" 
    Manufacturer: "Jessops" 
    Model: "blah" 
    Cost: 717.502206059872 
    Zoom: 9 
    Megapixels: 10.4508949012733 
    ImageStabiliser: false 
}
{CODE-END /}

## Step 1

You need to setup your facet definitions and store them in RavenDB as a document, like so:

{CODE step_1@ClientApi\FacetedSearch.cs /}

This tells RavenDB that you would like to get the following facets.

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

## Step 2

Next you need to create an index to work against, this can be setup like so:

{CODE step_2@ClientApi\FacetedSearch.cs /}

## Step 3

Finally you can write the following code and you get back the data below.

{CODE step_3@ClientApi\FacetedSearch.cs /}

This is equivalent to hitting the following Url:

{CODE-START:plain /}
http://localhost:8080/facets/CameraCost?facetDoc=facets/CameraFacets&query=Cost_Range:[Dx100 TO Dx300.0]
{CODE-END /}

{NOTE The data returned represents the count of the faceted data that satisfies the query `Where(x => x.Cost >= 100 && x.Cost <= 300 )` /}

{CODE-START:json /}
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
{CODE-END /}

###Stale results

The faceted search does not take into account a stealeness of an index. You can't wait for non stale results by customize you query by one of `WaitForNonStaleResultsXXX` method.