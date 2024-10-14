# Include Query Timings

---

{NOTE: }

* When making a query,  
  you can request to get detailed stats of the time spent by RavenDB on each part of the query.  
  E.g. duration of search, loading documents, transforming results, total duration, etc.

* By default, the timings stats are Not included in the query results, to avoid the measuring overhead.

* **To include the query timings** in the query results:  
  add a call to `Timings()` in your query code, or add `include timings()` to an RQL query.  
  See examples below.  

* In this page:
    * [Include timings in a query](../../../../client-api/session/querying/debugging/query-timings#include-timings-in-a-query)
    * [View timings](../../../../client-api/session/querying/debugging/query-timings#view-timings)
    * [Syntax](../../../../client-api/session/querying/debugging/query-timings#syntax)  

{NOTE/}

---

{PANEL: Include timings in a query}

{CODE-TABS}
{CODE-TAB:csharp:Query-sync timing_1@ClientApi\Session\Querying\Debugging\IncludeQueryTimings.cs /}
{CODE-TAB:csharp:Query-async timing_3@ClientApi\Session\Querying\Debugging\IncludeQueryTimings.cs /}
{CODE-TAB:csharp:DocumentQuery-sync timing_2@ClientApi\Session\Querying\Debugging\IncludeQueryTimings.cs /}
{CODE-TAB:csharp:DocumentQuery-async timing_4@ClientApi\Session\Querying\Debugging\IncludeQueryTimings.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where search(Name, "Syrup") or search(Name, "Lager")
include timings()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: View timings}

* The detailed timings can be viewed from Studio's [Query view](../../../../studio/database/queries/query-view).

* Running an RQL query with `include timings()` will show an additional **Timings Tab**  
  with a graphical representation of the time spent in each query part.   

![Figure 1. Include timings graphical results](images/include-timings.png "Include timings results")

{PANEL/}

{PANEL: Syntax}

{CODE syntax@ClientApi\Session\Querying\Debugging\IncludeQueryTimings.cs /}

| Parameter   | Type           | Description                                                 |
|-------------|----------------|-------------------------------------------------------------|
| ****timings** | `QueryTimings` | An _out_ param that will be filled with the timings results |

| `QueryTimings`   |                                     |                                                   |
|------------------|-------------------------------------|---------------------------------------------------|
| **DurationInMs** | `long`                              | Total duration                                    |
| **Timings**      | `IDictionary<string, QueryTimings>` | Dictionary with `QueryTimings` info per time part |

{PANEL/}
