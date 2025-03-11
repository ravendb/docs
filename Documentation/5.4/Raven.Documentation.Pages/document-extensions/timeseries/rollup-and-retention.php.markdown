# Time Series Rollups and Retention
---

{NOTE: }

Many time series applications produce massive amounts of data at a steady rate.  
**Time Series Policies** help you manage your data in two ways:  

* Creating **Rollups**:  
  Summarizing time series data by aggregating it into the form of a new, lower-resolution time series.

* Limiting **Retention**:  
  Controlling the duration for which time series data is kept before deletion.
 
* In this page:  
  * [Time series policies](../../document-extensions/timeseries/rollup-and-retention#time-series-policies)  
  * [Examples](../../document-extensions/timeseries/rollup-and-retention#examples)  
      * [Create time series policy](../../document-extensions/timeseries/rollup-and-retention#create-time-series-policies)  
      * [Retrieve rollup data](../../document-extensions/timeseries/rollup-and-retention#retrieve-rollup-data)
  * [Syntax](../../document-extensions/timeseries/rollup-and-retention#syntax)

{NOTE/}

---

{PANEL: Time series policies}

#### What are rollups?

A rollup is a time series that summarizes the data from another time series, 
with each rollup entry representing a specific time frame in the original time series.  
Each rollup entry contains 6 values that aggregate the data from all the entries in the original time frame:  

* `First` - the value of the first entry in the frame.  
* `Last` - the value of the last entry.  
* `Min` - the smallest value.  
* `Max` - the largest value.  
* `Sum` - the sum of all the values in the frame.  
* `Count` - the total number of entries in the frame.  

This results in a much more compact time series that still contains useful information about the original time series (also called "raw" time series).

#### Rollup policies:

Rollup time series are created automatically according to rollup policies that can be defined from Studio or client code.  

* A rollup policy applies to all time series of every document in the given collection. 

* Each collection can be configured to have multiple policies which are applied sequentially:
  * The raw time series is first rolled up using the policy with the shortest aggregation frame.
  * Subsequently, the resulting rollup time series is further aggregated using the policy with the next shortest aggregation frame,  
    and so on.

[Querying with group-by](../../document-extensions/timeseries/querying/aggregation-and-projections#examples) 
will transparently traverse over the rollups to retrieve the relevant results.  

Let's look at an example of rollup data:  

!["Rollup time series entries"](images/rollup-1.png "A rollup time series' entries")

**1) Name:**  
The name of a rollup time series has this format: `<name of raw time series>@<name of time series policy>`  
It is a combination of the name of the raw time series and the name of the time series policy separated by `@`.  
In the image above these are "HeartRates" and "byHour" respectively.  
For this reason, neither a time series name nor a policy name can have the character `@` in it.

**2) Timestamp:**  
The aggregation frame always begins at a round number of one of these time units: a second, minute, hour, day, week, month, or year. 
So the frame includes all entries starting at a round number of time units, and ending at a round number *minus one millisecond* 
(since milliseconds are the minimal resolution in RavenDB time series). 
The timestamp for a rollup entry is the beginning of the frame it represents.  

For example, if the aggregation frame is three days, a frame will start and end at a time stamps like:  
`2020-01-01 00:00:00` - `2020-01-03 23:59:59.999`.

**3) Values:**  
Each group of six values represents one value from the original entries. 
If the raw time series has `n` values per entry, the rollup time series will have `6 * n` per entry: 
the first six summarize the first raw value, the next six summarize the next raw value, and so on. 
The aggregated values have the names: `"First (<name of raw value>)", "Last (<name of raw value>)", ...` respectively.  

Because time series entries are limited to 32 values, rollups are limited to the first five values of an original time series entry, or 30 aggregate values.  

{PANEL/}

{PANEL: Examples}

#### Create time series policies:

{CODE:php rollup_and_retention_0@DocumentExtensions\TimeSeries\RollupAndRetention.php /}

---

#### Retrieve rollup data:

* Retrieving entries from a rollup time series is similar to getting the raw time series data.

* Learn more about using `timeSeriesFor.get` in [Get time series entries](../../document-extensions/timeseries/client-api/session/get/get-entries).

{CODE:php rollup_and_retention_1@DocumentExtensions\TimeSeries\RollupAndRetention.php /}

{PANEL/}

{PANEL: Syntax}

### The time series policies

* `rawPolicy`  
  * Used to define the retention time of the raw time series. 
  * Only one such policy per collection can be defined.
  * Does not perform aggregation.

* Rollup policy:  
  * Used to define the aggregation time frame and retention time for the rollup time series.
  * Multiple policies can be defined per collection.

{CODE-BLOCK: python}
class RawTimeSeriesPolicy(TimeSeriesPolicy):    
    def __init__(self, retention_time: TimeValue = TimeValue.MAX_VALUE()):
        ...

class TimeSeriesPolicy:
    def __init__(
        self,
        name: Optional[str] = None,
        aggregation_time: Optional[TimeValue] = None,
        retention_time: TimeValue = TimeValue.MAX_VALUE(),
    ):
        ...
{CODE-BLOCK/}

| Property             | Type | Description |
|----------------------|------|-------------|
| **name** (Optional)  | `str` | This string is used to create the name of the rollup time series.<br/>`name` is added to the raw time series name - with `@` as a separator,<br>e.g.: `<name of raw time series>@<name of time series policy>` |
| **retention_time** | `TimeValue` | Time series entries older than this time value (see `TimeValue` below) are automatically deleted. |
| **aggregation_time** (Optional) | `TimeValue` |The time series data being rolled up is divided into parts of this length of time, rounded to nearest time units. Each part is aggregated into an entry of the rollup time series. |

{CODE-BLOCK: python}
class TimeValue:
    def __init__(self, value: int, unit: TimeValueUnit):
        self.value = value
        self.unit = unit

   @classmethod
   def of_seconds(cls, seconds: int) -> TimeValue:
       return cls(seconds, TimeValueUnit.SECOND)

   @classmethod
   def of_minutes(cls, minutes: int) -> TimeValue:
       return cls(minutes * 60, TimeValueUnit.SECOND)

   @classmethod
   def of_hours(cls, hours: int) -> TimeValue:
       return cls(hours * 3600, TimeValueUnit.SECOND)

   @classmethod
   def of_days(cls, days: int) -> TimeValue:
       return cls(days * cls.SECONDS_PER_DAY, TimeValueUnit.SECOND)

   @classmethod
   def of_months(cls, months: int) -> TimeValue:
       return cls(months, TimeValueUnit.MONTH)

   @classmethod
   def of_years(cls, years: int) -> TimeValue:
       return cls(12 * years, TimeValueUnit.MONTH)
{CODE-BLOCK/}

Each of the above `TimeValue` methods returns a `TimeValue` object representing a whole number of the specified time units. 
These methods are used to define the aggregation and retention spans om time series policies.  

---

### The time series configuration object

{CODE-BLOCK: python}
class TimeSeriesConfiguration:
    def __init__(self):
        self.collections: Dict[str, TimeSeriesCollectionConfiguration] = {}
        self.policy_check_frequency: Optional[datetime.timedelta] = None
        self.named_values: Optional[Dict[str, Dict[str, List[str]]]] = None

class TimeSeriesCollectionConfiguration:
    def __init__(
        self,
        disabled: Optional[bool] = False,
        policies: Optional[List[TimeSeriesPolicy]] = None,
        raw_policy: Optional[RawTimeSeriesPolicy] = RawTimeSeriesPolicy.DEFAULT_POLICY(),
    ):
        self.disabled = disabled
        self.policies = policies
        self.raw_policy = raw_policy
{CODE-BLOCK/}

| Property        | Type | Description |
|-----------------|------|-------------|
| **collections** | `Dict[str, TimeSeriesCollectionConfiguration]` | Populate this `Dictionary` with the collection names and their corresponding `TimeSeriesCollectionConfiguration` objects. |
| **disabled** (Optional) | `bool` | If set to `true`, rollup processes will stop, and time series data will not be deleted by retention policies. |
| **policies** (Optional) | `List[TimeSeriesPolicy]` | Populate this `List` with your rollup policies. |
| **raw_policy** (Optional) | `RawTimeSeriesPolicy` | The `RawTimeSeriesPolicy`, the retention policy for the raw time series. |

---

### The time series configuration operation

{CODE-BLOCK: python}
class ConfigureTimeSeriesOperation(MaintenanceOperation[ConfigureTimeSeriesOperationResult])
{CODE-BLOCK/}

Learn more about operations in: [What are operations](../../client-api/operations/what-are-operations).  

---

### Time series entries

Time series entries are of one of the following classes:  

{CODE-BLOCK: python}
class TimeSeriesEntry:
  def __init__(
   self, timestamp: datetime.datetime = None, tag: str = None, values: List[int] = None, rollup: bool = None
  ):
   self.timestamp = timestamp
   self.tag = tag
   self.values = values
   self.rollup = rollup

class TypedTimeSeriesEntry(Generic[_T_TSBindable]):
  def __init__(
   self,
   timestamp: datetime.datetime = None,
   tag: str = None,
   values: List[int] = None,
   is_rollup: bool = None,
   value: _T_TSBindable = None,
  ):
   self.timestamp = timestamp
   self.tag = tag
   self.values = values
   self.is_rollup = is_rollup
   self.value = value


class TypedTimeSeriesRollupEntry(Generic[_T_Values]):
  def __init__(self, object_type: Type[_T_Values], timestamp: datetime.datetime):
   self._object_type = object_type
   self.tag: Optional[str] = None
   self.rollup = True
   self.timestamp = timestamp

   self._first: Optional[_T_Values] = None
   self._last: Optional[_T_Values] = None
   self._max: Optional[_T_Values] = None
   self._min: Optional[_T_Values] = None
   self._sum: Optional[_T_Values] = None
   self._count: Optional[_T_Values] = None
   self._average: Optional[_T_Values] = None
{CODE-BLOCK/}

{PANEL/}

## Related articles  
###Studio  
- [Time Series Interface in Studio](../../studio/database/document-extensions/time-series)

###Time Series  
- [Time Series Overview](../../document-extensions/timeseries/overview)  
- [API Overview](../../document-extensions/timeseries/client-api/overview)  

###Client-API  
- [What Are Operations?](../../client-api/operations/what-are-operations)

