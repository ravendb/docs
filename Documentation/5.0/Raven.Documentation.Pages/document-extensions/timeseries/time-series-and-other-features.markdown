# Time Series and Other Features

---

{NOTE: }

* This page describes how time series interact with various other RavenDB 
features.  

* Features that are not listed here either have no special behavior in 
regard to time series, or they have their own pages describing how they 
interact with time series (such as [indexing](../../document-extensions/timeseries/indexing)).

* In this page:  
  * [General Features](../../document-extensions/timeseries/time-series-and-other-features#general-features)  
  * [Smuggler](../../document-extensions/timeseries/time-series-and-other-features#smuggler)  
  * [Ongoing Tasks](../../document-extensions/timeseries/time-series-and-other-features#ongoing-tasks)  
  * [Revisions](../../document-extensions/timeseries/time-series-and-other-features#revisions)  

{NOTE/}

---

{PANEL: General Features}  

* The Document Session [tracks](../../client-api/session/what-is-a-session-and-how-does-it-work#tracking-changes) 
changes to time series data.  
* The [Changes API](../../client-api/changes/what-is-changes-api) service 
is triggered by changes to time series data.  
* Learn about how to **index** time series [here](../../document-extensions/timeseries/indexing).  
* Learn about how to **query** time series data [here](../../document-extensions/timeseries/querying/overview-and-syntax).  
* Learn how to **include** time series with `session.Load()` and in queries 
[here](../../document-extensions/timeseries/client-api/session/include/overview).  
{PANEL/}

{PANEL: Smuggler}
[Smuggler](../../client-api/smuggler/what-is-smuggler) is a DocumentStore 
property that can be used to export chosen database items to an external 
file or to import database items from an existing file into the database.  

To [configure smuggler](../../client-api/smuggler/what-is-smuggler#databasesmugglerexportoptions) 
to handle time series, add the parameter `DatabaseItemType.TimeSeries` 
to the `OperateOnTypes` object.  

{CODE-BLOCK: csharp}
OperateOnTypes = DatabaseItemType.Documents
                 | DatabaseItemType.TimeSeries
{CODE-BLOCK/}
{PANEL/}

{PANEL: Ongoing Tasks}
"[Ongoing tasks](../../studio/database/tasks/ongoing-tasks/general-info)" are 
various automatic processes that operate on an entire database. Some of these 
apply to time series data and some do not.  
<br/>

#### Tasks that apply to time series

* [External replication](../../server/ongoing-tasks/external-replication) 
creates a complete copy of a database, including documents and their extensions.  
* [Pull replication](../../server/ongoing-tasks/pull-replication) is similar 
to external replication except that the process is initiated by the 
destination server rather than the source server.  
* [Backup](../../client-api/operations/maintenance/backup/backup)s save the 
whole database at a certain point in time and can be used to restore the 
database later. All kinds of backups include time series data: "logical"-backup 
and snapshot, full and incremental.  

#### Cannot be applied to time series

* [ETL](../../server/ongoing-tasks/etl/basics) is a type of task that _extracts_ 
some portion of the data from a database, _transforms_ it according to a script, 
and _loads_ it to some external target. Neither the **RavenDB ETL** nor the 
**SQL ETL** tasks can access or operate on time series data.  
* [Data Subscriptions](../../client-api/data-subscriptions/what-are-data-subscriptions) 
send data to "worker" clients in batches.

{INFO: }
Support for time series in ETL is planned for one of the next releases.
{INFO/}

{PANEL/}

{PANEL: Revisions}

[Revisions](../../client-api/session/revisions/what-are-revisions) are old 
versions of a document. They can be saved manually or by setting a policy for 
a collection that creates them automatically.  

Revisions do not preserve time series data, and editing a time series does 
not trigger the creation of a new revision the way editing a document would. 
This is because time series are designed so for new entries to be added 
quickly and often, and creating revisions each time would significantly slow 
this process down.  

However, revisions _can_ be triggered / created manually if a _new_ time series 
is added to the document, or an existing time series is deleted. (Remember that 
a time series is deleted by deleting all of its entries).  
</br>

#### The &nbsp; `@timeseries-snapshot`

While revisions don't contain the time series data themselves, they do contain 
few details about the time series the document had at the time. These details 
appear as though they are part of the document's metadata, but it's not really 
part of the document. When a revision is viewed in the studio, this 
"time series snapshot" looks like this:  

![](images/TSSnapshot.png "NoSQL Database Time Series Feature")

This time series snapshot can also be accessed by loading a revision in the 
client. This is the general JSON format of the time series snapshot:  

{CODE-BLOCK: json}
"@metadata": {
    ...
    "@timeseries-snapshot": {
        "<the name of a time series>": {
            "Count": <the number of entries>,
            "Start": "<timestamp of first entry>",
            "End": "<timestamp of last entry>"
        },
        "<the name of the next time series>": { ... }
    }
{CODE-BLOCK/}
{PANEL/}

## Related articles  

### Time Series  
[Time Series Overview](../../document-extensions/timeseries/overview)  
[Include Time Series](../../document-extensions/timeseries/client-api/session/include/overview)  
[Time Series Indexing](../../document-extensions/timeseries/indexing)  
[Querying Time Series](../../document-extensions/timeseries/querying/overview-and-syntax)  

### Client-API  
[Session Entity Tracking](../../client-api/session/what-is-a-session-and-how-does-it-work#tracking-changes)  
[Changes API](../../client-api/changes/what-is-changes-api)  
[Smuggler](../../client-api/smuggler/what-is-smuggler)  
[Data Subscriptions](../../client-api/data-subscriptions/what-are-data-subscriptions)  
[Revisions](../../client-api/session/revisions/what-are-revisions)  

### Server  
[External replication](../../server/ongoing-tasks/external-replication)  
[Pull replication](../../server/ongoing-tasks/pull-replication)  
[Backup](../../client-api/operations/maintenance/backup/backup)  
[ETL](../../server/ongoing-tasks/etl/basics)  

### Studio  
[Ongoing tasks](../../studio/database/tasks/ongoing-tasks/general-info)  
