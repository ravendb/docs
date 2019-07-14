# Map-Reduce Visualizer
---

{NOTE: }

* The **MapReduce visualizer** allows inspecting the internal structure of a Map-Reduce index 
  by graphing the relations between the documents and the Map-Reduce results.  

* In this page:
  * [Map-Reduce Visualizer Usage Flow](../../../studio/database/indexes/map-reduce-visualizer#map-reduce-visualizer-usage-flow)
  * [Map-Reduce Visualizer for Big Data](../../../studio/database/indexes/map-reduce-visualizer#map-reduce-visualizer-for-big-data)

{NOTE/}

---

{PANEL: Map-Reduce Visualizer Usage Flow}

* **1.** Select a Map-Reduce index and the documents to view

![Figure 1. Select an Index](images/map-reduce-visualizer-1.png "Figure-1: Select Map-Reduce Index and Document IDs")

{PANEL/}

{PANEL: }

* **2**. View the graphical representation of the documents and their related 'group-by' reduced key field from the index

![Figure 2. View relation](images/map-reduce-visualizer-2.png "Figure-2: Graphical Representation of the Documents and the Reduced Key")

  * In the above example, the selected documents `orders/1-A, orders/27-A, orders/829-A` 
    are related to the key: `companies/85-A`.  
    i.e. All 3 documents have the value **'companies/85-A'** in their **'Company'** field in the document body.  

  * Document `ordes/100-A` has the value `companies/21-A` in its 'Company' field.
{PANEL/}

{PANEL: }

* **3**. View the detailed view for the key reduced results

![Figure 3. Key details](images/map-reduce-visualizer-3.png "Figure-3: Detailed View of the Key Reduced Results")

  1. The **total** number of orders documents made that contain the key 'companies/85-A' - **which is 6**  
     and the **total** orders amount made by company `companies/85-A' - **which is 2,272.75**

  2. A detailed info of these aggregated values showing per document selected.  
     Note: per each document, the OrdesCount is **'1'**, as each company makes one order.  
{PANEL/}

{PANEL: }

* **4**. View details of all entries

![Figure 4. All Entry details](images/map-reduce-visualizer-4.png "Figure-4: Detailed View of All Entries")

  * A detailed view of **all** the entries in the index that have 'companies/85-A' for their key (the 'Company' field).  
    Only the selected documents are listed explicitly in the Source Document column.    
{PANEL/}

{PANEL: Map-Reduce Visualizer for Big Data}

* **5**. Visualizer results for big data

![Figure 5. Big Data Tree](images/map-reduce-visualizer-5.png "Figure-5: Results for Big Data")

  * Once past a certain size, RavenDB starts processing the Map-Reduce entries in a **treelike** fashion.
{PANEL/}

{PANEL: }

* **6**. Visualizer details for big data

![Figure 6. Big Data Details](images/map-reduce-visualizer-6.png "Figure-6: Details for Big Data")

  * When there is a lot of data for a particular key, RavenDB will **segment the data**,  
    so that a minimal number of aggregation operations is required. 

  * For example, consider an update to document `orders/77-A` in the above image.  
    First, the Map function is run on the updated document, giving a Map entry to write to **page #1047**.  
    Then, the Reduce function is run on this page, giving the final tally for page #1047.  
    We’ll then recurse upward, toward **page #1391**, where the Reduce is run again.  
{PANEL/}

## Related Articles

### Indexes
- [Map-Reduce Indexes](../../../indexes/map-reduce-indexes)

### Studio
- [Create Map-Reduce Index](../../../studio/database/indexes/create-map-reduce-index)
