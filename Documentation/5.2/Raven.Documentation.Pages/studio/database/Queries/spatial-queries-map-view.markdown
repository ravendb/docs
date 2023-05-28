# Spatial Queries Map View
---

{NOTE: }

* [Spatial queries](../../../client-api/session/querying/how-to-make-a-spatial-query) retrieve documents using geographical criteria,  
  provided that the documents contain spatial data (latitude & longitude).

* The spatial query defines geographic regions (circles or polygons)  
  and queries for documents that have some relation to those regions.

* When executing a dynamic spatial query in the Studio, in addition to the regular document results,   
  a **Spatial Map** view will be displayed, showing the search results on a global map. 

    {INFO: }
    The spatial map view is added only for a [dynamic](../../../client-api/session/querying/how-to-query#dynamicQuery) spatial query.  
    It is not added when querying a spatial index.  
    {INFO/}

* In this page:  
  * [Spatial data in documents](../../../studio/database/queries/spatial-queries-map-view#spatial-data-in-documents)  
  * [Running a dynamic spatial query](../../../studio/database/queries/spatial-queries-map-view#running-a-dynamic-spatial-query)  
  * [Spatial map view](../../../studio/database/queries/spatial-queries-map-view#spatial-map-view)  
  * [Examples](../../../studio/database/queries/spatial-queries-map-view#examples)  
     * [Circular region example](../../../studio/database/queries/spatial-queries-map-view#circular-region-example)  
     * [Polygonal region example](../../../studio/database/queries/spatial-queries-map-view#polygonal-region-example)  
     * [Mixed shapes intersection example](../../../studio/database/queries/spatial-queries-map-view#mixed-shapes-intersection-example)  
{NOTE/}

---

{PANEL: Spatial data in documents}

* Documents that contain geographic data (latitude & longitude) can be queried for using a spatial query.  
  
* For example, the following Employee document contains fields:  
  `Address.Location.Latitude` & `Address.Location.Longitude` and can be searched for by this spatial data.

* Note:  
  Using "Latitude" and "Longitude" as the document fields' names is Not mandatory.  
  Any custom name can be assigned to those coordinate fields.

![Figure 1. Spatial Data](images/spatial-map-view-query-1.png "Figure 1. A document containing spatial data")

{PANEL/}

---

{PANEL: Running a dynamic spatial query}

![Figure 2. The Query Section](images/spatial-map-view-query-2.png "Figure 2. The query section")

* **1**. **Indexes**  
  Open the **Indexes** section.  
* **2**. **Query**  
  Choose **Query**.  

---

![Figure 3. Running a Query](images/spatial-map-view-query-3.png "Figure 3. Running the query")

* **1**. **Query box**  
  Type your query in this area. The query is written in [RQL](../../../client-api/session/querying/what-is-rql).

{CODE-BLOCK: csharp}
// Specify the collection queried
from Employees

// Make a spatial query
where spatial.within(

    // Use 'spatial.point' to specify the document field names containing the spatial data.
    // The latitude field is always passed as the first param.
    spatial.point(Address.Location.Latitude, Address.Location.Longitude),

    // Specify a geographical area.
    // This query will search for documents whose latitude & longitude are within this circle.
    spatial.circle(20,47.623473, -122.3060097, 'miles')
)
{CODE-BLOCK/}

* **2**. **Run query**  
  Click this button to execute the query.  
  The query results will include two tabs:  
  * Results tab - listing all resulting documents in textual form  
  * Spatial map tab - showing results on the global map   

---

![Figure 4. Textual Results View](images/spatial-map-view-query-4.png "Figure 4. Textual results view")

{PANEL/}

{PANEL: Spatial map view}

![Figure 5. Spatial Map View](images/spatial-map-view-query-5.png "Figure 5. Spatial map view")

* **1**. **Spatial map**  
  If there are any resulting documents that match the spatial query, a __Spatial Map tab__ is added to the results view.  
  Click the tab to view the resulting documents in their geographical locations on the map.  

* **2**. **Expand results**  
  Click to expand the spatial map.  

---

![Figure 6. Zoom and Drag](images/spatial-map-view-query-6.png "Figure 6. Zoom and drag")

* **1**. **Zoom control**  
  Click **+**/**-** or roll your mouse wheel to zoom in and out.  
* **2**. **Map**  
  Click anywhere in the map area and drag to move the map.  

---

![Figure 7. Region and Points](images/spatial-map-view-query-7.png "Figure 7. Region and points")

![Figure 8. Same Points, Zoomed In](images/spatial-map-view-query-8.png "Figure 8. Same points, zoomed in")

* **1**. **Region**  
  The region (a circle in this case) that was defined in the query.

* **2**. **Location markers**

  * **Red markers**  
    A red marker locates a single document result.  
    Hovering over the marker pops up the document's name.  
    ![Figure 9. Hovering over a Red Point](images/spatial-map-view-query-9.png "Figure 9. Hovering over a Red Point")  
    Clicking the marker pops up the document's contents.  
    ![Figure 10. Clicking a Red Point](images/spatial-map-view-query-10.png "Figure 10. Clicking a Red Point")  

  * **Green markers**  
    A green marker gathers two or more neighboring red markers (documents).  
    Zooming in will reveal the individual locations.  
    The number on the green marker represents the number of documents gathered by this marker.  
     * If a green marker gathers at least three document locations,  
       hovering over it would display a crude scheme of these locations on the map.  
       ![Figure 11. Hovering over a Green Point](images/spatial-map-view-query-11.png "Figure 11. Hovering over a Green Point")  
     * Clicking a green marker zooms in to reveal the locations it gathers.  

---

![Figure 12. Viewing-Options Tooltip](images/spatial-map-view-query-12.png "Figure 12. Viewing-options tooltip")

* **1**. **Viewing-options toggler**  
  Hovering over the toggler will open the viewing options.  
* **2**. **Viewing Options**  
  The map view options.  
* **3**. **Map view**  
  Select **Streets** or **Topography** map.  
* **4**. **Point fields**  
  When checked, the documents that contain these fields will be visible as marker points on the map.  
* **5**. **Regions**  
  Show or hide circular/polygonal search regions.  

{PANEL/}

{PANEL: Examples}

{NOTE: }

### Circular region example

---

The following query locates companies within **two separate circular regions**.  

{CODE-BLOCK: csharp}
from Companies 
where 
spatial.within(
    spatial.point(Address.Location.Latitude, Address.Location.Longitude),
    spatial.circle(200, 45.5137863, -122.675375, 'miles')
    )
or
spatial.within(
    spatial.point(Address.Location.Latitude, Address.Location.Longitude),
    spatial.circle(200, 37.7774357, -122.418, 'miles')
    )
{CODE-BLOCK/}

![Figure 13. Multiple Regions](images/spatial-map-view-query-13.png "Figure 13. Multiple Regions")

{INFO: }

* **Circular region syntax**  
  A circular region can be defined using two different syntaxes, 
  **spatial.circle** and **spatial.[wkt](https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry)**.  
  {CODE-TABS}
  {CODE-TAB:csharp:spatial.circle spatial.circle@Studio\Database\Queries\Queries.cs /}
  {CODE-TAB:csharp:spatial.wkt spatial.wkt@Studio\Database\Queries\Queries.cs /}
  {CODE-TABS/}
  The **search coordinates** are provided in **a different order** for the two syntaxes.  
  For `spatial.circle`, provide the Latitude first and the Longitude second.  
  For `spatial.wkt`, provide the Longitude first and the Latitude second.  

* **Region color**  
  When multiple regions are defined, they are given different colors in the spatial map view.

{INFO/}

{NOTE/}

{NOTE: }

### Polygonal region example

---

The following query searches for companies within the boundaries of a **polygonal** region.  

* The polygon's coordinates must be provided in [counterclockwise](../../../indexes/querying/spatial#advanced-search) order.  // todo.. link..
* The first and last coordinates must mark the same location to form a closed region.  
* You can use [tools like this one](https://www.keene.edu/campus/maps/tool/) 
  to draw a polygon on the world map and copy the coordinates to your query.  

{CODE-BLOCK: csharp}
from companies 
where 
spatial.within(
    spatial.point(Address.Location.Latitude, Address.Location.Longitude), 
    spatial.wkt('POLYGON ((
        -118.6527948 32.7114894, 
        -95.8040242 37.5929338, 
        -102.8344151 53.3349629, 
        -127.5286633 48.3485664, 
        -129.4620208 38.0786067, 
        -118.7406746 32.7853769, 
        -118.6527948 32.7114894))')
)
{CODE-BLOCK/}

![Figure 14. Polygon](images/spatial-map-view-query-14.png "Figure 14. Polygon")

{NOTE/}

{NOTE: }

### Mixed shapes intersection example

---

This query searches for companies at the intersection of a **circular region** and a **polygonal region**.  
Though additional companies are located in each region, only companies located in both regions are retrieved.  

{CODE-BLOCK: csharp}
from Companies 
where 
spatial.within(
    spatial.point(Address.Location.Latitude, Address.Location.Longitude),
    spatial.wkt('CIRCLE(-119.5 45.5137863 d=400)')
)
and 
spatial.within(
    spatial.point(Address.Location.Latitude, Address.Location.Longitude), 
    spatial.wkt('POLYGON ((
        -119.7105226 47.1000662,
        -117.0712682 40.3896178,
        -110.7439164 34.3929116,
        -97.9134529 38.0071749,
        -98.1770925 45.2197803,
        -119.7105226 47.1000662))')
)
{CODE-BLOCK/}

![Figure 15. Multiple Shapes Intersection](images/spatial-map-view-query-15.png "Figure 15. Multiple shapes intersection")

{NOTE/}

{PANEL/}

## Related Articles

### Queries
- [Querying: Spatial](../../../indexes/querying/spatial)  

### Indexes
- [Indexing Spatial Data](../../../indexes/indexing-spatial-data)  

### Client API
- [How to Query a Spatial Index](../../../client-api/session/querying/how-to-query-a-spatial-index)  

### Additional Pages
- [Polyline Mapping Tool](https://www.keene.edu/campus/maps/tool/)
