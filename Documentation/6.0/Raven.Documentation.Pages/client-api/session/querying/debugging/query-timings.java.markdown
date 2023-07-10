# Include Query Timings

---

{NOTE: }

* When running a query, you can request for statistics regarding 
  the time it takes to perform the whole query as well as parts 
  of it like the duration of document search and loading.  
* By default, this feature is **disabled**.  
  To **enable** it, add `include timings()` to an RQL query or call 
  `.timings()` from your code.  

* In this page:  
    * [Including Timings In a Query](../../../../client-api/session/querying/debugging/query-timings#including-timings-in-a-query)  
        * [Syntax](../../../../client-api/session/querying/debugging/query-timings#syntax)  
        * [Example](../../../../client-api/session/querying/debugging/query-timings#example)
    * [Viewing Timings](../../../../client-api/session/querying/debugging/query-timings#viewing-timings)  

{NOTE/}

---

{PANEL: Including Timings In a Query}

## Syntax

{CODE:java timing_1@ClientApi\Session\Querying\Debugging\IncludeQueryTimings.java /}

The `QueryTimings` object will be filled with the timings when the query returns.  

| `QueryTimings` | | |
| - | - | - |
| __durationInMs__ | `number` | Total duration |
| __timings__ | `Record<string, QueryTimings>` | Dictionary with `QueryTimings` info per time part |

## Example

{CODE-TABS}
{CODE-TAB:java:Java timing_2@ClientApi\Session\Querying\Debugging\IncludeQueryTimings.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Products 
where search(Name, 'Syrup')
include timings()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Viewing Timings}

* Timings can be viewed using Studio's [Query view](../../../../studio/database/queries/query-view).  
* If `include Timings()` is added to an RQL query Studio will display an additional **Timings** tab 
  with a graphical representation of the time it took to perform each query part.   

![Figure 1. Include timings graphical results](images/include-timings.png "Include timings results")

{NOTE: }
In a sharded database, timings are provided per shard.  
Read more about it [here](../../../../sharding/querying#timing-queries).  
{NOTE/}

{PANEL/}
