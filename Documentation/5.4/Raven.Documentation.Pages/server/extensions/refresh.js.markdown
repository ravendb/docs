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

To refresh a document, add a `@refresh` flag to the document's metadata specifying datetime in UTC format.  
This indicates when the document should be refreshed.  

This will cause the document to refresh **_only once_**! The refresh operation removes 
the `@refresh` flag.  

The exact time that the document refreshes is not determined by the value of `@refresh` 
- rather, the server refreshes documents at regular intervals determined by the Refresh 
Configuration. The default interval is 60 seconds.  

Refreshing a document causes its [change vector](../../server/clustering/replication/change-vector) 
to increment the same way it would after any other kind of update to the document. 
This triggers any features that react to documents updating, including but not limited 
to:  

* The document is re-indexed by any indexes that cover it.  
* [Replication](../../server/ongoing-tasks/external-replication), 
  [Subscriptions](../../client-api/data-subscriptions/what-are-data-subscriptions), 
  and [ETL](../../server/ongoing-tasks/etl/basics) are triggered.
* A [revision](../../document-extensions/revisions/overview) of the document is created.  

{PANEL/}

{PANEL: Examples}

#### Setting the refresh configuration for a database:

To activate and/or configure document refreshing, the server needs to be sent a configuration object using the `ConfigureRefreshOperation` operation.  

{CODE:nodejs configure_refresh@server/extensions/documentRefresh.js /}

---

#### Set a document to refresh 1 hour from now:

{CODE:nodejs refresh_example@server/extensions/documentRefresh.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax_1@server/extensions/documentRefresh.js /}

[//]: # (| Parameter                 | Type     | Description                                                                                   |)
[//]: # (|---------------------------|----------|-----------------------------------------------------------------------------------------------|)
[//]: # (| __configuration__         | `object` | Refresh configuration that will be set on the server &#40;for the database&#41;                       |)
[//]: # (| __disabled__              | `object` | If set to true, document refreshing is disabled for the entire database. Default: `true`      |)
[//]: # (| __refreshFrequencyInSec__ | `number` | Determines how often the server checks for documents that need to be refreshed. Default: `60` |)

{CODE:nodejs syntax_2@server/extensions/documentRefresh.js /}

| Parameter                 | Type      | Description                                                                                                              |
|---------------------------|-----------|--------------------------------------------------------------------------------------------------------------------------|
| __disabled__              | `boolean` | `true` - document refreshing is disabled for the entire database (Default).<br>`false` - document refreshing is enabled. |
| __refreshFrequencyInSec__ | `number`  | Set how often the server checks for documents that need to be refreshed.<br>Default: `60`                                |

{PANEL/}

{PANEL: Configure from Studio}

Alternatively, document refreshing can also be configured in the studio, under __Settings > Document Refresh__.

![NoSQL DB Server - Document Refresh](images/StudioRefresh.png "Document Refresh Settings")

{PANEL/}

## Related Articles

### Client API

- [How to Get and Modify Metadata](../../client-api/session/how-to/get-and-modify-entity-metadata)  

### Server

- [What is a Change Vector](../../server/clustering/replication/change-vector)  
