# Time-Series and Smuggler
---

{NOTE: }

* This section describes the relationships between Time Series and the Smuggler interface.  

* In this page:  
  * []()  
  * []()  
{NOTE/}

---

{PANEL: }

###Time Series and Smuggler  

[Smuggler](../../../client-api/smuggler/what-is-smuggler) is a DocumentStore property, that can be used 
to [export](../../../client-api/smuggler/what-is-smuggler#databasesmugglerexportoptions) chosen 
database items to an external file or to [import](../../../client-api/smuggler/what-is-smuggler#databasesmugglerimportoptions) 
database items from an existing file into the database.  

* **Transferred Time Series Values**  
  [how does smuggler transfer the series' values]  
* To make Smuggler handle time series, include `DatabaseItemType.TimeSeries` in `OperateOnTypes`'s list of entities to import or export.  
  [sample here]  

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
