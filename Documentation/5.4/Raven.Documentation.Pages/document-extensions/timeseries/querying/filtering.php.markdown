# Filtering Time Series Queries

---

{NOTE: }

* In addition to limiting time series query results by specifying the [range of entries](../../../document-extensions/timeseries/querying/choosing-query-range) to retrieve,
  you can filter the time series entries by their **values**, **tag**, or by the contents of a **document referenced in the tag**.

* In this page:  
  * [Filter by value](../../../document-extensions/timeseries/querying/filtering#filter-by-value)
  * [Filter by tag](../../../document-extensions/timeseries/querying/filtering#filter-by-tag)
  * [Filter by referenced document](../../../document-extensions/timeseries/querying/filtering#filter-by-referenced-document)

{NOTE/}

---

{PANEL: Filter by value}

* A time series entry can have up to 32 [values](../../../document-extensions/timeseries/overview#values).

* A time series query can filter entries based on these values.  

{CODE-TABS}
{CODE-TAB:php:Query filter_entries_1@DocumentExtensions\TimeSeries\FilterTimeSeriesQuery.php /}
{CODE-TAB:php:DocumentQuery filter_entries_2@DocumentExtensions\TimeSeries\FilterTimeSeriesQuery.php /}
{CODE-TAB:php:RawQuery filter_entries_3@DocumentExtensions\TimeSeries\FilterTimeSeriesQuery.php /}
{CODE-TAB-BLOCK:sql:RQL}
from Employees 
select timeseries (
    from HeartRates
    between "2020-05-17T00:00:00.0000000"
    and "2020-05-17T00:10:00.0000000"
    // Use the "where Value" clause to filter entries by the value
    where Value > 75
)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Filter by tag}

* A time series entry can have an optional [tag](../../../document-extensions/timeseries/overview#tags).

* A time series query can filter entries based on this tag.  

{CODE-TABS}
{CODE-TAB:php:Query filter_entries_4@DocumentExtensions\TimeSeries\FilterTimeSeriesQuery.php /}
{CODE-TAB:php:DocumentQuery filter_entries_5@DocumentExtensions\TimeSeries\FilterTimeSeriesQuery.php /}
{CODE-TAB:php:RawQuery filter_entries_6@DocumentExtensions\TimeSeries\FilterTimeSeriesQuery.php /}
{CODE-TAB-BLOCK:sql:RQL}
from Employees
select timeseries (
    from HeartRates
    between "2020-05-17T00:00:00.0000000"
    and "2020-05-17T00:10:00.0000000"
    // Use the "where Tag" clause to filter entries by the tag string content
    where Tag == "watches/fitbit"
)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CODE-TABS}
{CODE-TAB:php:Query filter_entries_7@DocumentExtensions\TimeSeries\FilterTimeSeriesQuery.php /}
{CODE-TAB:php:DocumentQuery filter_entries_8@DocumentExtensions\TimeSeries\FilterTimeSeriesQuery.php /}
{CODE-TAB:php:RawQuery filter_entries_9@DocumentExtensions\TimeSeries\FilterTimeSeriesQuery.php /}
{CODE-TAB-BLOCK:sql:RQL}
from Employees  
select timeseries (
    from HeartRates
    between "2020-05-17T00:00:00.0000000"
    and "2020-05-17T00:10:00.0000000"
    // Use the "where Tag in" clause to filter by various tag options
    where Tag in ("watches/apple", "watches/samsung", "watches/xiaomi")
)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Filter by referenced document}

* A time series entry's [tag](../../../document-extensions/timeseries/overview#tags) can contain the **ID of a document**.

* A time series query can filter entries based on the contents of this referenced document.  
  The referenced document is loaded, and entries are filtered by its properties.

{CODE-TABS}
{CODE-TAB:php:Query filter_entries_10@DocumentExtensions\TimeSeries\FilterTimeSeriesQuery.php /}
{CODE-TAB:php:DocumentQuery filter_entries_11@DocumentExtensions\TimeSeries\FilterTimeSeriesQuery.php /}
{CODE-TAB:php:RawQuery filter_entries_12@DocumentExtensions\TimeSeries\FilterTimeSeriesQuery.php /}
{CODE-TAB-BLOCK:sql:RQL}
from Companies
where Address.Country == "USA"
select timeseries (
    from StockPrices
    // Use 'load Tag' to load the employee document referenced in the tag
    load Tag as employeeDoc
    // Use 'where <property>' to filter entries by the properties of the loaded document
    where employeeDoc.Title == "Sales Manager"
)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

## Related articles

**Time Series Overview**  
[Time Series Overview](../../../document-extensions/timeseries/overview)  

**Studio Articles**  
[Studio Time Series Management](../../../studio/database/document-extensions/time-series)  

**Time Series Indexing**  
[Time Series Indexing](../../../document-extensions/timeseries/indexing)  

**Time Series Queries**  
[Range Selection](../../../document-extensions/timeseries/querying/choosing-query-range)  
[Aggregation and Projection](../../../document-extensions/timeseries/querying/aggregation-and-projections)  
[Indexed Time Series Queries](../../../document-extensions/timeseries/querying/using-indexes)  

**Policies**  
[Time Series Rollup and Retention](../../../document-extensions/timeseries/rollup-and-retention)  
