# Spatial Queries Map View
---

{NOTE: }

* [Spatial Queries](../../../indexes/querying/spatial) 
  locate documents by spatial data (geographic location).  

* When a spatial query is executed using Studio, a **Spatial Map** view 
  displays the search results in graphical form.  

    {INFO: }
    The spatial map view is added only for [dynamic](../../../client-api/session/querying/what-is-rql#dynamic-and-indexed-queries) 
    spatial queries.  
    It is not added when spatial queries are executed over indexes.  
    {INFO/}

* A spatial query can define geographic regions shaped like circles or polygons and relate the search to these regions.  
  These regions can be displayed in the spatial map view, in addition to the document results.  

* In this page:  
  * [Spatial Data in Documents](../../../studio/database/queries/spatial-queries-map-view#spatial-data-in-documents)  
  * [Running a Dynamic Spatial Query](../../../studio/database/queries/spatial-queries-map-view#running-a-dynamic-spatial-query)  
  * [Spatial Map View](../../../studio/database/queries/spatial-queries-map-view#spatial-map-view)  
  * [Examples](../../../studio/database/queries/spatial-queries-map-view#examples)  
     * [Circular Region Example](../../../studio/database/queries/spatial-queries-map-view#circular-region-example)  
     * [Polygonal Region Example](../../../studio/database/queries/spatial-queries-map-view#polygonal-region-example)  
     * [Mixed Shapes Intersection Example](../../../studio/database/queries/spatial-queries-map-view#mixed-shapes-intersection-example)  
{NOTE/}

---


{PANEL: Spatial Data in Documents}

Spatial Queries locate documents by geographic location, indicated by Latitude and Longitude coordinates.  
An employee profile, for example, may include and be searched by spatial data.  

![Figure 1. Spatlal Data](images/spatial-map-view-query-1.png "Figure 1. Spatlal Data")

{INFO: }
You can name Coordinates' fields freely (not necessarily by the names "Latitude" and "Longitude").  
{INFO/}

{PANEL/}

---

{PANEL: Running a Dynamic Spatial Query}

![Figure 2. The Query Section](images/spatial-map-view-query-2.png "Figure 2. The Query Section")

* **1**. **Indexes**  
  Open the **Indexes** section.  
* **2**. **Query**  
  Choose **Query**.  

---

![Figure 3. Running a Query](images/spatial-map-view-query-3.png "Figure 3. Running a Query")

* **1**. **Query Box**  
  Type your query in this area.
{CODE-BLOCK:JSON}
from Employees
where spatial.within(
    spatial.point(Address.Location.Latitude, Address.Location.Longitude),
    spatial.circle(20,47.623473, -122.3060097, 'miles')
    )
{CODE-BLOCK/}

     {INFO: }
     Use `spatial.point` to specify the document field names containing the spatial data.  
     The Latitude field is always chosen first, and the Longitude field second.  
     {INFO/}

* **2**. **Play Button**  
  Click this button to execute the query.  
  The above query searches for documents whose Latitude and Longitude are within [the specified circle](../../../studio/database/queries/spatial-queries-map-view#circular-region-example).  

---

![Figure 4. Textual Results View](images/spatial-map-view-query-4.png "Figure 4. Textual Results View")

{PANEL/}

{PANEL: Spatial Map View}

![Figure 5. Spatial Map View](images/spatial-map-view-query-5.png "Figure 5. Spatial Map View")

* **1**. **Spatial Map**  
  If there are any resulting documents that match the spatial query, a **Spatial Map** tab is added 
  to the results view.  
  Click the tab to view the resulting documents in their geographical locations on the map.  
* **2**. **Expand Results**  
  Click to expand the spatial map.  

---

![Figure 6. Zoom and Drag](images/spatial-map-view-query-6.png "Figure 6. Zoom and Drag")

* **1**. **Zoom Control**  
  Click **+**/**-** or **roll your mouse wheel** to zoom in and out.  
* **2**. **Map**  
  Click anywhere in the map area and drag to move the map.  

---

![Figure 7. Region and Points](images/spatial-map-view-query-7.png "Figure 7. Region and Points")

![Figure 8. Same Points, Zoomed In](images/spatial-map-view-query-8.png "Figure 8. Same Points, Zoomed In")

* **1**. **Region**  
  The region (a circle in this case) that was defined in the query.  
* **2**. **Location Markers**  
  * **Red Markers**  
    A red marker locates a single document result.  
    Hovering over the marker pops up the document's name.  
    ![Figure 9. Hovering over a Red Point](images/spatial-map-view-query-9.png "Figure 9. Hovering over a Red Point")  
    Clicking the marker pops up the document's contents.  
    ![Figure 10. Clicking a Red Point](images/spatial-map-view-query-10.png "Figure 10. Clicking a Red Point")  

  * **Green Markers**  
    A green marker gathers two or more neighboring red markers (documents).  
    Zooming in will reveal the individual locations.  
    The number on the green marker represents the number of documents gathered by this marker.  
     * If a green marker gathers at least three document locations, hovering over it 
       would display a crude scheme of these locations on the map.  
       ![Figure 11. Hovering over a Green Point](images/spatial-map-view-query-11.png "Figure 11. Hovering over a Green Point")  
     * Clicking a green marker zooms in to reveal the locations it gathers.  

---

![Figure 12. Viewing-Options Tooltip](images/spatial-map-view-query-12.png "Figure 12. Viewing-Options Tooltip")

* **1**. **Viewing-Options Toggler**  
  Hovering over the toggler will open the viewing options.  
* **2**. **Viewing Options**  
  The map view options.  
* **3**. **Map View**  
  Select **Streets** or **Topography** map.  
* **4**. **Point Fields**  
  When checked, the documents that contain these fields will be visible as marker points on the map.  
* **5**. **Regions**  
  Show or Hide circular/polygonal search regions.  

{PANEL/}

{PANEL: Examples}

## Circular Region Example

The following query locates companies in **two separate circular regions**.  

{CODE-BLOCK:JSON}
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

* **Circular Region Syntax**  
  A circular region can be defined using two different syntaxes, 
  **spatial.circle** and **spatial.[wkt](https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry)**.  
  {CODE-TABS}
  {CODE-TAB:csharp:spatial.circle spatial.circle@Studio\Database\Queries\Queries.cs /}
  {CODE-TAB:csharp:spatial.wkt spatial.wkt@Studio\Database\Queries\Queries.cs /}
  {CODE-TABS/}
  The **search coordinates** are provided in **a different order** for the two syntaxes.  
  For `spatial.circle`, provide the Latitude first and the Longitude second.  
  For `spatial.wkt`, provide the Longitude first and the Latitude second.  

* **Region Color**  
  When multiple regions are defined, they are given different colors in the spatial map view.  

{INFO/}

## Polygonal Region Example

The following query searches for companies within the boundaries of a **polygonal** (polygon-shaped) region.  

* The polygon's coordinates must be provided in a [counterclockwise](../../../indexes/querying/spatial#advanced-search) order.  
* The first and last coordinates must mark the same location to form a closed region.  
* You can use [tools like this one](https://www.keene.edu/campus/maps/tool/) 
  to draw a polygon on the world map and copy the coordinates to your query.  

{CODE-BLOCK:JSON}
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
    -118.6527948 32.7114894))'))
{CODE-BLOCK/}

![Figure 14. Polygon](images/spatial-map-view-query-14.png "Figure 14. Polygon")

## Mixed Shapes Intersection Example

This query searches for companies at the intersection of 
a **circular region** and a **polygonal region**.  
Though additional companies are located in each region, 
only companies located in both regions are retrieved.  

{CODE-BLOCK:JSON}
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
    -119.7105226 47.1000662))'))
{CODE-BLOCK/}

![Figure 15. Multiple Shapes Intersection](images/spatial-map-view-query-15.png "Figure 15. Multiple Shapes Intersection")

{PANEL/}

## Related Articles

### Queries
- [Querying: Spatial](../../../indexes/querying/spatial)  

### Indexes
- [Indexing Spatial Data](../../../indexes/indexing-spatial-data)  

### Client API
- [How to Query a Spatial Index](../../../client-api/session/querying/how-to-query-a-spatial-index)  

###Additional Pages
- [Polyline Mapping Tool](https://www.keene.edu/campus/maps/tool/)
