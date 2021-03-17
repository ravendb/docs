# Spatial Queries Map View
---

{NOTE: }

* [Spatial Queries](../../../../indexes/querying/spatial) 
  locate documents by spatial data (geographic location).  

* When a spatial query is executed using Studio, a **Spatial Map** view 
  displays the search results in graphical form.  

    {INFO: }
    The spatial map view is added only for **dynamic** spatial queries.  
    It is not added when spatial queries are executed over indexes.  
    {INFO/}

* A spatial query can define geographic regions shaped like circles or polygons and relate the search to these regions.  
  These regions can be displayed in the spatial map view, in addition to the document results.  

* In this page:  
  * [Spatial Data in Documents](../../../../studio/database/indexes/querying/spatial-data-in-documents)  
  * [Running a Dynamic Spatial Query](../../../../studio/database/indexes/querying/spatial-queries-map-view#running-a-dynamic-spatial-query)  
  * [Spatial Map View](../../../../studio/database/indexes/querying/spatial-queries-map-view#spatial-map-view)  
  * [Additional Examples](../../../../studio/database/indexes/querying/spatial-queries-map-view#additional-examples)  
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
* **2**. **Play Button**  
  Click this button to execute the query.  
  The above query searches for documents whose Latitude and Longitude are within the specified circle.  

---

![Figure 4. Textual Results View](images/spatial-map-view-query-4.png "Figure 4. Textual Results View")

* **1**. **Results**  
  The **Results** tab lists the results in textual form.  

{PANEL/}

{PANEL: Spatial Map View}

![Figure 5. Spatial Map View](images/spatial-map-view-query-5.png "Figure 5. Spatial Map View")

* **1**. **Spatial Map**  
  If there are any resulting documents that match the spatial query, a **Spatial Map** tab is added 
  to the results view.  
  Click the **Spatial Map** tab to view the resulting documents in their geographical locations on the map.  
* **2**. **Expand Results**  
  Click to expand the spatial map.  

---

![Figure 6. Zoom and Drag](images/spatial-map-view-query-6.png "Figure 6. Zoom and Drag")

In the **Spatial Map** view:  

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
  Show or Hide circle/polygon search regions.  

{PANEL/}

{PANEL: Additional Examples}

The following query locates companies in **two separate circular regions**.  
Each region is given its own color (up to 5 colors) in the spatial map view.  
![Figure 13. Multiple Regions](images/spatial-map-view-query-13.png "Figure 13. Multiple Regions")

---

The following query searches for companies within the boundaries of a **polygon**-shape region.  

* Polygon coordinates must be provided in a [counterclockwise](../../../../indexes/querying/spatial#advanced-search) order.  
* The first and last coordinates mark the same location to form a closed area.  
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

![Figure 14. Polygon](images/spatial-map-view-query-14.png "Figure 143. Polygon")

{PANEL/}

## Related Articles

### Queries
- [Querying: Spatial](../../../../indexes/querying/spatial)  

### Indexes
- [Indexing Spatial Data](../../../../indexes/indexing-spatial-data)  

### Client API
- [How to Query a Spatial Index](../../../../client-api/session/querying/how-to-query-a-spatial-index)  

###Additional Pages
- [Polyline Mapping Tool](https://www.keene.edu/campus/maps/tool/)
