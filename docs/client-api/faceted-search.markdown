# Faceted Search

When displaying a large amount of data, often paging is used to make viewing the data manageable. However it's also useful to give some context of the entire data-set and a easy way to drill-down into particular categories. The common approach to doing this is "faceted search", as shown in the image below. __Note__ how the count of each category within the current search is displayed across the top.

![Facets](images\CNET_faceted_search_2.jpg)

To achieve this in RavenDB, lets say you have a document like this:

{CODE-START:json /}
{ 
    DateOfListing: "2000-09-01T00:00:00.0000000+01:00" 
    Manufacturer: "Jessops" 
    Model: "blah" 
    Cost: 717.50 
    Zoom: 9 
    Megapixels: 10.45 
    ImageStabiliser: false 
}
{CODE-END /}

## Step 1

You need to setup your facet definitions and store them in RavenDB as a document, like so:

{CODE-START:csharp /}
_facets = new List<Facet>
{
	new Facet<Test> {Name = x => x.Manufacturer},
	new Facet<Test>
	{  
		Name = x => x.Cost,
		Ranges =
		{
			x => x.Cost < 200m,
			x => x.Cost > 200m && x.Cost < 400m,
			x => x.Cost > 400m && x.Cost < 600m,
			x => x.Cost > 600m && x.Cost < 800m,
			x => x.Cost > 800m
		}
	},
	new Facet<Test>
	{  
		Name = x => x.MegaPixels,
		Ranges = 
		{
			x => x.MegaPixels < 3.0m,
			x => x.MegaPixels > 3.0m && x.MegaPixels < 7.0m, 
			x => x.MegaPixels > 7.0m && x.MegaPixels < 10.0m, 
			x => x.MegaPixels > 10.0m
		}
	}
};
                        
session.Store(new FacetSetup { Id = "facets/CameraFacets", Facets = _facets });
{CODE-END /}

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

{CODE-START:csharp /}
store.DatabaseCommands.PutIndex("CameraCost",
                            new IndexDefinition
                            {
                                Map = @"from camera in docs 
                                    select new 
                                    { 
                                        camera.Manufacturer, 
                                        camera.Model, 
                                        camera.Cost,
                                        camera.DateOfListing,
                                        camera.Megapixels
                                    }"
                            });
{CODE-END /}

## Step 3

Finally you can write the following code and you get back the data below.

{CODE-START:csharp /}
var facetResults = s.Query<Camera>("CameraCost") 
                        .Where(x => x.Cost >= 100 && x.Cost <= 300 ) 
                        .ToFacets("facets/CameraFacets");
{CODE-END /}

This is equivalent to hitting the following Url:

    http://localhost:8080/facets/CameraCost?facetDoc=facets/CameraFacets&query=Cost_Range:[Dx100 TO Dx300.0]

__NOTE:__ the data returned represents the faceted results for documents that satisfy the query `Where(x => x.Cost >= 100 && x.Cost <= 300 )`

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