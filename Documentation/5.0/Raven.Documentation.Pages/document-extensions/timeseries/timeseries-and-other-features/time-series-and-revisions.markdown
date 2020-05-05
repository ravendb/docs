# Time-Series and Revisions
---

{NOTE: }

* This section describes the relationships between Time Series and Ravedb's Revisions feature.  
   * [How time series are supported by revisions.]  
   * [How time series trigger revisions execution.]  

* In this page:  
  * []()  
  * []()  
{NOTE/}

---

{PANEL: }

###Time Series and Revisions  

A [document revision](../../../client-api/session/revisions/what-are-revisions) stores all the document's time series' names, 
but only selected timepoints [start and end timeponits?]  
 
* **Stored Time Series Values**  
  [specify how values are stored here]  

* Revisions-creation can be initiated by time series **name** modification.  
   * When the Revisions feature is enabled, the creation or deletion of a time series initiates the creation of a new document revision.  
   * Time series **value** modifications do **not** cause the creation of new revisions.  

{PANEL/}

## Related articles
**Studio Articles**:  
[Studio Time Series Management]()  

**Client-API - Session Articles**:  
[Time Series Overview]()  
[Creating and Modifying Time Series]()  
[Deleting Time Series]()  
[Retrieving Time Series Values]()  
[Time Series and Other Features]()  

**Client-API - Operations Articles**:  
[Time Series Operations]()  
