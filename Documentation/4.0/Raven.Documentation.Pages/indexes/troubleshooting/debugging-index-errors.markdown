# Indexes: Debugging Index Errors

Indexes in RavenDB are user provided LINQ queries running on top of dynamic JSON data model. There is a wide space for errors here, either because of malformed index definition or missing / corrupt data on the JSON document itself.

## Index Compilation Errors

An index definition such as the following one will fail:

{CODE-BLOCK:json}
{
    "Name": "Posts_TitleLength",
    "Maps" : [
        "from doc in docs where doc.Type == 'posts' select new { doc.Title.Length }"
    ]
}
{CODE-BLOCK/}

The error is the use of single quotes to enclose a string, something that is not allowed in C#. This will result in the following compilation error:

{CODE-BLOCK:csharp}
IndexCompilationException: Failed to compile index Posts_TitleLength

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Lucene.Net.Documents;
using Raven.Server.Documents.Indexes.Static;
using Raven.Server.Documents.Indexes.Static.Linq;
using Raven.Server.Documents.Indexes.Static.Extensions;

namespace Raven.Server.Documents.Indexes.Static.Generated
{
    public class Index_Posts_TitleLength : StaticIndexBase
    {
        IEnumerable Map_0(IEnumerable<dynamic> docs)
        {
            foreach (var doc in docs)
            {
                if ((doc.Type == 'posts') == false)
                    continue;
                yield return new
                {
                    doc.Title.Length
                }

                ;
            }
        }

        public Index_Posts_TitleLength()
        {
            this.AddMap("@all_docs", this.Map_0);
            this.OutputFields = new string[] { "Length" };
        }
    }
}

(20,34): error CS1012: Too many characters in character literal

{CODE-BLOCK/}

Which clearly indicates that the error is at line 20, column 34: `if ((doc.Type == 'posts') == false)`.

This gives you enough information to figure out what is wrong. Those errors are immediate, and require no further action from the database. The only thing that the user can do is fix the index definition.

{NOTE:Information}

Please note that the index definition differs from the send one, because internally RavenDB is applying a lot of optimizations to the send LINQ function to achieve best performance.

{NOTE/}

## Index Execution Errors

A common case is an index that doesn't take into account that other documents also exists on the server. For example, let us take this index:

{CODE-BLOCK:json}
{
    "Name": "YearOfBirth",
    "Maps" : [
        "from doc in docs select new { YearOfBirth = DateTime.Parse(doc.DateOfBirth).Year }"
    ]
}
{CODE-BLOCK/}

This index makes an assumption that all documents have a `DateOfBirth` property and that the value of this property can be parsed to `DateTime`. A document that doesn't have that property will return `null` when it is accessed, resulting in a `ArgumentNullException` when the index is executed.

Because indexes are updated on a background thread, it is unlikely that users will be aware of those errors.  

RavenDB surfaces index execution errors in two places, the first is the index statistics and index error statistics. Accessible for programmatic access at `/indexes/stats` and `/indexes/errors` or in human readable form at [Studio](../../todo-update-me-later).

{CODE-TABS}
{CODE-TAB-BLOCK:json:Statistics}
{
   "Results":[
      {
         "Name":"TitleLength",
         "MapAttempts":2,
         "MapSuccesses":2,
         "MapErrors":2,
         "ReduceAttempts":null,
         "ReduceSuccesses":null,
         "ReduceErrors":null,
         "MappedPerSecondRate":0.0,
         "ReducedPerSecondRate":0.0,
         "MaxNumberOfOutputsPerDocument":0,
         "Collections":{
            "@all_docs":{
               "LastProcessedDocumentEtag":791,
               "LastProcessedTombstoneEtag":0,
               "DocumentLag":0,
               "TombstoneLag":0
            }
         },
         "LastQueryingTime":"2018-02-26T14:17:08.7454587Z",
         "State":"Error",
         "Priority":"Normal",
         "CreatedTimestamp":"2018-02-26T14:17:08.7092294Z",
         "LastIndexingTime":"2018-02-26T14:17:08.7512648Z",
         "IsStale":true,
         "LockMode":"Unlock",
         "Type":"Map",
         "Status":"Paused",
         "EntriesCount":0,
         "ErrorsCount":2,
         "IsInvalidIndex":true
      }
   ]
}
{CODE-TAB-BLOCK/}
{CODE-TAB-BLOCK:json:Errors}
{
   "Results":[
      {
         "Name":"TitleLength",
         "Errors":[
            {
               "Timestamp":"2018-02-26T14:17:08.7813846Z",
               "Document":"Raven/Hilo/categories",
               "Action":"Map",
               "Error":"Failed to execute mapping function on Raven/Hilo/categories. Exception: System.ArgumentNullException: String reference not set to an instance of a String.\r\nParameter name: s\r\n   at System.DateTimeParse.Parse(String s, DateTimeFormatInfo dtfi, DateTimeStyles styles)\r\n   at CallSite.Target(Closure , CallSite , Type , Object )\r\n   at System.Dynamic.UpdateDelegates.UpdateAndExecute2[T0,T1,TRet](CallSite site, T0 arg0, T1 arg1)\r\n   at Raven.Server.Documents.Indexes.Static.Generated.Index_TitleLength.<Map_0>d__0.MoveNext()\r\n   at Raven.Server.Documents.Indexes.Static.TimeCountingEnumerable.Enumerator.MoveNext() in C:\\Builds\\RavenDB-Stable-4.0\\src\\Raven.Server\\Documents\\Indexes\\Static\\TimeCountingEnumerable.cs:line 41\r\n   at Raven.Server.Documents.Indexes.MapIndexBase`2.HandleMap(LazyStringValue lowerId, IEnumerable mapResults, IndexWriteOperation writer, TransactionOperationContext indexContext, IndexingStatsScope stats) in C:\\Builds\\RavenDB-Stable-4.0\\src\\Raven.Server\\Documents\\Indexes\\MapIndexBase.cs:line 64\r\n   at Raven.Server.Documents.Indexes.Workers.MapDocuments.Execute(DocumentsOperationContext databaseContext, TransactionOperationContext indexContext, Lazy`1 writeOperation, IndexingStatsScope stats, CancellationToken token) in C:\\Builds\\RavenDB-Stable-4.0\\src\\Raven.Server\\Documents\\Indexes\\Workers\\MapDocuments.cs:line 108"
            },
            {
               "Timestamp":"2018-02-26T14:17:08.7958137Z",
               "Document":"companies/1-A",
               "Action":"Map",
               "Error":"Failed to execute mapping function on companies/1-A. Exception: System.ArgumentNullException: String reference not set to an instance of a String.\r\nParameter name: s\r\n   at System.DateTimeParse.Parse(String s, DateTimeFormatInfo dtfi, DateTimeStyles styles)\r\n   at CallSite.Target(Closure , CallSite , Type , Object )\r\n   at Raven.Server.Documents.Indexes.Static.Generated.Index_TitleLength.<Map_0>d__0.MoveNext()\r\n   at Raven.Server.Documents.Indexes.Static.TimeCountingEnumerable.Enumerator.MoveNext() in C:\\Builds\\RavenDB-Stable-4.0\\src\\Raven.Server\\Documents\\Indexes\\Static\\TimeCountingEnumerable.cs:line 41\r\n   at Raven.Server.Documents.Indexes.MapIndexBase`2.HandleMap(LazyStringValue lowerId, IEnumerable mapResults, IndexWriteOperation writer, TransactionOperationContext indexContext, IndexingStatsScope stats) in C:\\Builds\\RavenDB-Stable-4.0\\src\\Raven.Server\\Documents\\Indexes\\MapIndexBase.cs:line 64\r\n   at Raven.Server.Documents.Indexes.Workers.MapDocuments.Execute(DocumentsOperationContext databaseContext, TransactionOperationContext indexContext, Lazy`1 writeOperation, IndexingStatsScope stats, CancellationToken token) in C:\\Builds\\RavenDB-Stable-4.0\\src\\Raven.Server\\Documents\\Indexes\\Workers\\MapDocuments.cs:line 108"
            }
         ]
      }
   ]
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

As you can see, RavenDB surfaces both the fact that the index has encountered, what was the document it errored on, and what that error was. The errors collection contains the last 500 errors that happened on the server per index.

In addition to that, the server logs may contain additional information regarding the error.

## Marking Index as Errored

Furthermore, in order to protect itself from indexes that always fail, RavenDB will mark index as errored if it keeps failing. The actual logic for erroring-out an index is:

* If an index has 15% or more failure rate
* The 15% count is only considered after the first 100 indexing attempts to make sure that have a good determination

A errored index cannot be queried, all queries to a errored index will result in an exception.

The only thing that can be done with a errored index is to either delete it or replace the index definition with one that is resilient to those errors.

## Related articles

### Server

- [Index Administration](../../server/administration/index-administration)

### Client API

- [How to Get Index Errors](../../client-api/operations/maintenance/indexes/get-index-errors)
- [How to Reset Index](../../client-api/operations/maintenance/indexes/reset-index)
