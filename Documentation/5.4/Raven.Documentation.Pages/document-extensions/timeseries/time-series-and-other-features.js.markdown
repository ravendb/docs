﻿# Time Series and Other Features

---

{NOTE: }

* This page describes how time series interact with various other RavenDB features.  

* Features not listed here either have no special behavior regarding time series,  
  or they have their own pages describing their interaction with time series (such as [indexing](../../document-extensions/timeseries/indexing)).

* In this page:  
  * [General features](../../document-extensions/timeseries/time-series-and-other-features#general-features)  
  * [Smuggler](../../document-extensions/timeseries/time-series-and-other-features#smuggler)  
  * [Ongoing tasks](../../document-extensions/timeseries/time-series-and-other-features#ongoing-tasks)  
  * [Revisions](../../document-extensions/timeseries/time-series-and-other-features#revisions)  

{NOTE/}

---

{PANEL: General features}  

* The Document Session [tracks](../../client-api/session/what-is-a-session-and-how-does-it-work#tracking-changes) changes to time series data.  
* The [Changes API](../../client-api/changes/what-is-changes-api) service is triggered by changes to time series data.  
* Learn about how to **index** time series [here](../../document-extensions/timeseries/indexing).  
* Learn about how to **query** time series data [here](../../document-extensions/timeseries/querying/overview-and-syntax).  
* Learn how to **include** time series with `session.Load()` and in queries [here](../../document-extensions/timeseries/client-api/session/include/overview).  

{PANEL/}

{PANEL: Smuggler}

[Smuggler](../../client-api/smuggler/what-is-smuggler) is a DocumentStore property that can be used to export selected database items to an external file 
or import database items from an existing file into the database.  

To [configure smuggler](../../client-api/smuggler/what-is-smuggler#databasesmugglerexportoptions) to handle time series, 
add the string `TimeSeries` to the `operateOnTypes` array:  

{CODE-BLOCK: javascript}
const options = new DatabaseSmugglerExportOptions();
options.operateOnTypes = ["Documents", "TimeSeries"];
{CODE-BLOCK/}

{PANEL/}

{PANEL: Ongoing tasks}

[Ongoing tasks](../../studio/database/tasks/ongoing-tasks/general-info) are various automatic processes that operate on the database.  
Some of these apply to time series data, while others do not.

#### Tasks that apply to time series

* [External replication](../../server/ongoing-tasks/external-replication) creates a complete copy of a database, including documents and their extensions.  
* [Hub/Sink replication](../../server/ongoing-tasks/hub-sink-replication) allows you to create a live replica of a database or a part of it,  
  including documents' time series, using Hub and Sink tasks.  
* [Backups](../../client-api/operations/maintenance/backup/backup) save the whole database at a certain point in time and can be used to restore the database later.  
  All kinds of backups include time series data: logical-backup and snapshot, full and incremental.
* [RavenDB ETL](../../server/ongoing-tasks/etl/raven#time-series) is a type of task that _extracts_ some portion of the data from a database, _transforms_ it according to a script,  
  and _loads_ it to another RavenDB database on another server.  

#### Tasks that cannot be applied to time series

* [SQL ETL](../../server/ongoing-tasks/etl/basics), another type of ETL that can set a relational database as its target.  
* [Data Subscriptions](../../client-api/data-subscriptions/what-are-data-subscriptions) send data to "worker" clients in batches.

{INFO: }

Support for time series in ETL is planned for one of the next releases.

{INFO/}

{PANEL/}

{PANEL: Revisions}

[Revisions](../../document-extensions/revisions/overview) are old versions of a document.  
They can be created manually or by setting a policy that creates them automatically on selected collections.  

Revisions do not preserve time series data, and editing a time series does not trigger the creation of a new revision as editing a document would. 
This is because time series are designed to accommodate frequent additions of new entries quickly, and creating revisions each time would significantly slow down this process.

However, revisions are triggered / created manually if a _new_ time series is added to the document,  
or an existing time series is deleted. (Remember that a time series is deleted by deleting all of its entries).  

#### The&nbsp;`@timeseries-snapshot`&nbsp;metadata property

While revisions don't contain the time series data themselves, they do include few details about the time series the document had at the time.
These details appear in the `@timeseries-snapshot` property within the document's metadata.
When a revision is viewed in the studio, this metadata property looks like this:  

![NoSQL Database Time Series Feature](images/TSSnapshot.png "NoSQL Database Time Series Feature")

This time series snapshot property can also be accessed by loading a revision in the client.  
This is the general JSON format of the time series snapshot:  

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
[Revisions](../../document-extensions/revisions/client-api/overview)  

### Server  
[External replication](../../server/ongoing-tasks/external-replication)  
[Hub/Sink replication](../../server/ongoing-tasks/hub-sink-replication)  
[Backup](../../client-api/operations/maintenance/backup/backup)  
[ETL](../../server/ongoing-tasks/etl/basics)  

### Studio  
[Ongoing tasks](../../studio/database/tasks/ongoing-tasks/general-info)  
