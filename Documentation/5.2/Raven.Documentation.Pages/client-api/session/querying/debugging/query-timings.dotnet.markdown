# Include Query Timings

---

{NOTE: }

* When making a query, you can request to get detailed stats of the time spent by RaveDB on each part of the query. 
  i.e. duration of the search, loading documents, transforming results, etc.
 
* By default, getting the timings in queries is turned off.

* __To get the query timings__ include a call to `Timings`.

* In this page:
    * [Include timings in query](../../../../client-api/session/querying/debugging/query-timings#include-timings-in-query)
    * [View timings](../../../../client-api/session/querying/debugging/query-timings#view-timings)
    * [Syntax](../../../../client-api/session/querying/debugging/query-timings#syntax)  
{NOTE/}

---

{PANEL: Include timings in query}

{CODE-TABS}
{CODE-TAB:csharp:Query-sync timing_1@ClientApi\Session\Querying\Debugging\IncludeQueryTimings.cs /}
{CODE-TAB:csharp:Query-async timing_3@ClientApi\Session\Querying\Debugging\IncludeQueryTimings.cs /}
{CODE-TAB:csharp:DocumentQuery-sync timing_2@ClientApi\Session\Querying\Debugging\IncludeQueryTimings.cs /}
{CODE-TAB:csharp:DocumentQuery-async timing_4@ClientApi\Session\Querying\Debugging\IncludeQueryTimings.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Products
where search(Name, 'Syrup') or search(Name, 'Lager')
include timings()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: View timings}

* The detailed timings can be viewed from the __Query view__ in the Studio.  
* Running a query with `include Timings()` will show an additional __Timings Tab__  
  with a graphical representation of the time spent in each query part.   

![Figure 1. Include timings graphical results](images/include-timings.png "Include timings results")

{PANEL/}

{PANEL: Syntax}

{CODE syntax@ClientApi\Session\Querying\Debugging\IncludeQueryTimings.cs /}

| Parameters | Data type | Description |
| - | - | - |
| __timings__ | `QueryTimings` | An _out_ param that will be filled with the timings results |

| `QueryTimings` | | |
| - | - | - |
| __DurationInMs__ | `long` | Total duration |
| __Timings__ | `IDictionary<string, QueryTimings>` | Dictionary with `QueryTimings` info per time part |

{PANEL/}
