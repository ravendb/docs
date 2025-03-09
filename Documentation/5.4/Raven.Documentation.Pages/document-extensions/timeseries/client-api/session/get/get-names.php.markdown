# Get Time Series Names
---

{NOTE: }

* Use `advanced.getTimeSeriesFor` to get the names of all time series for the specified entity.

* In this page:   
  * [`getTimeSeriesFor` usage](../../../../../document-extensions/timeseries/client-api/session/get/get-names#gettimeseriesfor-usage)
  * [Example](../../../../../document-extensions/timeseries/client-api/session/get/get-names#example)  
  * [Syntax](../../../../../document-extensions/timeseries/client-api/session/get/get-names#syntax)

{NOTE/}

---

{PANEL: `getTimeSeriesFor` usage}

**Flow**:  

* Open a session.
* Load an entity to the session either using [session.load](../../../../../client-api/session/loading-entities#load) 
  or by querying for the document via [session.query](../../../../../client-api/session/querying/how-to-query).  
  In both cases, the resulting entity will be tracked by the session.
* Call `advanced.getTimeSeriesFor`, pass the tracked entity.

**Note**:  

* If the entity is Not tracked by the session, an `ArgumentException` exception is thrown.

{PANEL/}

{PANEL: Example}

{CODE:php timeseries_region_Retrieve-TimeSeries-Names@DocumentExtensions\TimeSeries\TimeSeriesTests.php /}  

{PANEL/}

{PANEL: Syntax}

{CODE-BLOCK:php}
def get_time_series_for(self, entity: object) -> List[str]:
    ...
{CODE-BLOCK/}
 
| Parameter    | Type  | Description                                         |
|--------------|-------|-----------------------------------------------------|
| **entity** | `object` | The entity whose time series names you want to get |

| Return value   |                                                                                                   |
|----------------|---------------------------------------------------------------------------------------------------|
| `List[str]` | A list of names of all the time series associated with the entity, sorted alphabetically by the name |

{PANEL/}

## Related articles

**Client API**  
[Time Series API Overview](../../../../../document-extensions/timeseries/client-api/overview)  

**Studio Articles**  
[Studio Time Series Management](../../../../../studio/database/document-extensions/time-series)  

**Querying and Indexing**  
[Time Series Querying](../../../../../document-extensions/timeseries/querying/overview-and-syntax)  
[Time Series Indexing](../../../../../document-extensions/timeseries/indexing)  

**Policies**  
[Time Series Rollup and Retention](../../../../../document-extensions/timeseries/rollup-and-retention)  
