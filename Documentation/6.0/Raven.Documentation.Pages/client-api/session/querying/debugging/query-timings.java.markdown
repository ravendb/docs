# Include Query Timings

---

{NOTE: }

* When making a query,  
  you can request to get detailed stats of the time spent by RavenDB on each part of the query.  
  E.g. duration of search, loading documents, transforming results, total duration, etc.

* By default, the timings stats are Not included in the query results, to avoid the measuring overhead.

* __To include the query timings__ in the query results:  
  add a call to `timings()` in your query code, or add `include timings()` to an RQL query.  
  See examples below.

* In this page:
    * [Include timings in a query](../../../../client-api/session/querying/debugging/query-timings#include-timings-in-a-query)
    * [View timings](../../../../client-api/session/querying/debugging/query-timings#view-timings)
    * [Syntax](../../../../client-api/session/querying/debugging/query-timings#syntax)
    * [Timings in a sharded database](../../../../client-api/session/querying/debugging/query-timings#timings-in-a-sharded-database)

{NOTE/}

---

{PANEL: Include timings in a query}

{CODE-TABS}
{CODE-TAB:java:Query timing_2@ClientApi\Session\Querying\Debugging\IncludeQueryTimings.java /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where search(Name, "Syrup") or search(Name, "Lager")
include timings()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: View timings}

* The detailed timings can be viewed from the [Query view](../../../../studio/database/queries/query-view) in the Studio.

* Running an RQL query with `include timings()` will show an additional __Timings Tab__  
  with a graphical representation of the time spent in each query part.

![Figure 1. Include timings graphical results](images/include-timings.png "Include timings results")

{PANEL/}

{PANEL: Syntax}

{CODE:java timing_1@ClientApi\Session\Querying\Debugging\IncludeQueryTimings.java /}

The `QueryTimings` object will be filled with the timings when the query returns.  

| `QueryTimings`   |                             |                                                   |
|------------------|-----------------------------|---------------------------------------------------|
| __durationInMs__ | `long`                   | Total duration                                    |
| __timings__      | `Map<String, QueryTimings>` | Dictionary with `QueryTimings` info per time part |

{PANEL/}

{PANEL: Timings in a sharded database}

* In a sharded database, timings for each part are provided __per shard__.

* Learn more in [timings in a sharded database](../../../../sharding/querying#timing-queries).

{PANEL/}
