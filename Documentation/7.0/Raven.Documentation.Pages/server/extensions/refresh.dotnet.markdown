# Document Refresh
---

{NOTE: }

* The Refresh feature increments a document's [change vector](../../server/clustering/replication/change-vector), 
  triggering its re-indexation as well as other features that react to document updates.  

* Refresh is scheduled using the `@refresh` flag in a document's [metadata](../../client-api/session/how-to/get-and-modify-entity-metadata).  

* In this page:  
  * [Overview](../../server/extensions/refresh#overview)  
  * [Examples](../../server/extensions/refresh#examples)
  * [Syntax](../../server/extensions/refresh#syntax)
  * [Configure from Studio](../../server/extensions/refresh#configure-from-studio)


{NOTE/}

---

{PANEL: Overview}

* To set a document refresh time, add the document's `@metadata` a 
  `@refresh` property with the designated refresh time as a value.  
  Set the time in **UTC** format, not local time, e.g. -  
  **"@refresh": "2025-04-22T08:00:00.0000000Z"**  
  {WARNING: }
  Metadata properties starting with `@` are for internal RavenDB usage only.  
  Do _not_ use the metadata `@refresh` property for any other purpose than 
  scheduling a document's refresh time for the built-in refresh feature.  
  {WARNING/}

* This will cause the document to refresh **only once**.  
  When the refresh operation takes place, it will also remove the `@refresh` property from the document.  
     {INFO: The exact refresh time is determined by:}

     1. The refresh time value set for the `@refresh` property.  
     2. The way you set the [Refresh Configuration](../../server/extensions/refresh#syntax), 
        including -  
         - The interval by which the server refreshes documents (set by default to 60 seconds).  
         - The way you set **maximal items to process**, potentially limiting the number 
           of documents that RavenDB is allowed to delete each time the refresh feature is invoked.  

     {INFO/}

* Refreshing a document causes its [change vector](../../server/clustering/replication/change-vector) 
  to increment the same way it would after any other kind of update to the document.  
  This triggers any features that react to document updating, including but not limited to:  
   - Re-indexing of the document by indexes that cover it  
   - [Replication](../../server/ongoing-tasks/external-replication), 
     [Subscriptions](../../client-api/data-subscriptions/what-are-data-subscriptions), 
     and [ETL](../../server/ongoing-tasks/etl/basics) triggering  
   - Creation of a [document revision](../../document-extensions/revisions/overview)  

{PANEL/}

{PANEL: Examples}

#### Example I  

How to set refresh configuration for a database:  

{CODE refresh_0@Server/Extensions/DocumentRefresh.cs /}  

This activates document refreshing and sets the interval at 5 minutes.  
<br/>
#### Example II  

How to set a document to refresh 1 hour from now:  

{CODE refresh_1@Server/Extensions/DocumentRefresh.cs /}  

{PANEL/}

{PANEL: Syntax}

To activate and/or configure document refreshing, send the server a 
`RefreshConfiguration` object using the `ConfigureRefreshOperation` operation.  

---

#### `RefreshConfiguration`

{CODE refresh_config@Server/Extensions/DocumentRefresh.cs /}

| Parameter | Type | Description |
| - | - | - |
| **Disabled** | `bool` | If `true`, refreshing documents is disabled for the entire database.<BR>Default: `true` |
| **RefreshFrequencyInSec** | `long?` | Determines how often (in seconds) the server processes documents that need to be refreshed.<BR>Default: `60` |
| **MaxItemsToProcess** | `long?` | Determines the maximal number of documents the feature is allowed to refresh in one run. |

{PANEL/}

{PANEL: Configure from Studio}

Alternatively, document refreshing can also be configured via Studio, under **Settings > Document Refresh**.

![NoSQL DB Server - Document Refresh](images/StudioRefresh.png "Document Refresh Settings")

{PANEL/}

## Related Articles

### Client API

- [How to Get and Modify Metadata](../../client-api/session/how-to/get-and-modify-entity-metadata)  

### Server

- [What is a Change Vector](../../server/clustering/replication/change-vector)  
