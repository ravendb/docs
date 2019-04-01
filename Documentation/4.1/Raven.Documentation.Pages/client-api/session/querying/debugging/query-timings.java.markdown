# Session: Querying: Debugging: How to include Query Timings

By default, detailed timings (duration of Lucene search, loading documents, transforming results) in queries are turned off, this is due to small overhead that calculation of such timings produces.

## Syntax

{CODE:java timing_1@ClientApi\Session\Debugging\IncludeQueryTimings.java /}

## Example

{CODE-TABS}
{CODE-TAB:java:Java timing_2@ClientApi\Session\Debugging\IncludeQueryTimings.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Products 
where search(Name, 'Syrup')
include timings()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

<hr />

When you run the query with `include timings()` in Studio, extra tab appears. 

![Figure 1. Include timings graphical results](images/include-timings-1.png "Include timings results")
