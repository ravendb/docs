# Spatial Queries Map View
---

{NOTE: }

* [Spatial Queries](../../../../indexes/querying/spatial) 
  search for documents (e.g. employee profiles) by their geographic location.  
  Locations are indicated by Latitude and Longitude coordinates.  

* When a spatial query is executed using Studio, a Spatial Map view 
  displays the search results in graphical form.  

* A spatial query can define regions, e.g. a 300-square-miles square, 
  and relate the search to these regions.  
  You can, for instance, look for rice providers located within 
  (or out of) a region you defined.  
   * You can shape regions as circles or polygons.  
   * Regions can be included in the Spatial Map view.  

* In this page:  
  * [Running a Query](../../../../studio/database/indexes/querying/spatial-queries-map-view#running-a-query)  
  * [Spatial Map View](../../../../studio/database/indexes/querying/spatial-queries-map-view#spatial-map-view)  
  * [Additional Examples](../../../../studio/database/indexes/querying/spatial-queries-map-view#additional-examples)  
{NOTE/}

---

{PANEL: Running a Query}

![Figure 1. The Query Section](images/spatial-map-view-query-1.png "Figure 1. The Query Section")

**1**. Open the **Indexes** section.  
**2**. Choose **Query**.  

---

![Figure 2. Running a Query](images/spatial-map-view-query-2.png "Figure 2. Running a Query")

**1**. Type your query in the query box.  
**2**. Click the Play button to execute the query.  

{PANEL/}

{PANEL: Spatial Map View}

![Figure 3. Textual Results View](images/spatial-map-view-query-3.png "Figure 3. Textual Results View")

**1**. As for all queries (spatial or not), the **Results** tab lists the results in textual form.  

---

![Figure 4. Spatial Map View](images/spatial-map-view-query-4.png "Figure 4. Spatial Map View")

**1**. If the results include spatial data, a **Spatial Map** tab is displayed.  
**2**. Click **Expand Results** to enlarge the map view.  

---

![Figure 5. Zoom and Drag](images/spatial-map-view-query-5.png "Figure 5. Zoom and Drag")

In the **Spatial Map** view:  

**1**. Click **+**/**-** or **roll your mouse wheel** to zoom in or out.  
**2**. Click and drag anywhere in the map to move the entire map.  

---

![Figure 6. Region and Points](images/spatial-map-view-query-6.png "Figure 6. Region and Points")

![Figure 7. Same Points, Zoomed In](images/spatial-map-view-query-7.png "Figure 7. Same Points, Zoomed In")

**1**. **Region**  
  The region (in this case a circle) that was defined in the query.  

**2**. **Points**  

  * **Red Points**  
    A red point marks the location of a single search result in its precise 
    longitude and latitude.  

     * Hovering over the point pops up the document's name.  
       ![Figure 8. Hovering over a Red Point](images/spatial-map-view-query-8.png "Figure 8. Hovering over a Red Point")

     * Clicking the point pops up the document's contents.  
       ![Figure 9. Clicking a Red Point](images/spatial-map-view-query-9.png "Figure 9. Clicking a Red Point")

  * **Green Points**  
    A green point gathers two or more neighboring locations (red points).  
    The number in its center specifies the number of locations gathered in it.  

     * If a green point gathers three individual locations or more, hovering over it 
       would draw them on the map.  
       ![Figure 10. Hovering over a Green Point](images/spatial-map-view-query-10.png "Figure 10. Hovering over a Green Point")

     * Clicking a green point zooms in to show the locations it gathers.  

---

![Figure 11. Viewing-Options Tooltip](images/spatial-map-view-query-11.png "Figure 11. Viewing-Options Tooltip")

**1**. Hover over the button to reveal a tooltip of viewing-options.  
**2**. Choose a **Streets** or a **Topography** map view.  
**3**. Choose whether to point search-results locations on the map.  
**4**. A list of the queried field names the spatial data was fetched from.  
**5**. Show or Hide search regions.  

{PANEL/}

{PANEL: Additional Examples}

The following query locates companies in **two separate circular regions**.  
Note that each region is given its own color in the spatial map view to 
differentiate it from others (though colors will start repeating themselves 
if more than 5 regions are displayed).  
![Figure 12. Multiple Regions](images/spatial-map-view-query-12.png "Figure 12. Multiple Regions")

---

The following query searches for companies within the boundaries of a **polygon**.  

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

Note that the coordinates must advance counterclockwise, and the shape must 
close, i.e. the same pair of coordinates starts the polygon and ends it.  
![Figure 13. Polygon](images/spatial-map-view-query-13.png "Figure 13. Polygon")

{PANEL/}

## Related Articles

### Queries
- [Querying: Spatial](../../../../indexes/querying/spatial)  

### Indexes
- [Indexing Spatial Data](../../../../indexes/indexing-spatial-data)  

### Client API
- [How to Query a Spatial Index](../../../../client-api/session/querying/how-to-query-a-spatial-index)  

