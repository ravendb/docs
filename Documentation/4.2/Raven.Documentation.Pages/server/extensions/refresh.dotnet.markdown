# Document Refresh
---

{NOTE: }

* The Refresh feature causes a document's [change vector](../../server/clustering/replication/change-vector) 
to increment. This causes it to be re-indexed, as well as activates other features 
that react to document updates.  

* Refresh is scheduled using the `@refresh` flag in a document's [metadata](../../client-api/session/how-to/get-and-modify-entity-metadata).  

* In this page:  
  * [Overview](../../server/extensions/refresh#overview)  
  * [Syntax](../../server/extensions/refresh#syntax)  
  * [Examples](../../server/extensions/refresh#examples)  

{NOTE/}

---

{PANEL: Overview}

To refresh a document, add a `@refresh` flag to a document's metadata with a value of 
type `DateTime` (this must be in UTC format, i.e. `DateTime.UtcNow`). This specifies 
when the document should be refreshed.  

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
* A [revision](../../server/extensions/revisions) of the document is created.  

{PANEL/}

{PANEL: Syntax}

To activate and/or configure document refreshing, the server needs to be sent a 
`RefreshConfiguration` object using a `ConfigureRefreshOperation` operation.  
<br/>
#### RefreshConfiguration Object

{CODE refresh_config@Server/Extensions/DocumentRefresh.cs /}

| Parameter | Type | Description |
| - | - | - |
| **Disabled** | `bool` | If set to true, document refreshing is disabled for the entire database. Default: `true` |
| **RefreshFrequencyInSec** | `long` | Determines how often the server checks for documents that need to be refreshed. Default: `60` |
<br/>
#### Studio

Alternatively, document refreshing can also be configured in the studio, under **Settings > Document Refresh**.

![](images/StudioRefresh.png)

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

## Related Articles

### Client API

- [How to Get and Modify Metadata](../../client-api/session/how-to/get-and-modify-entity-metadata)  

### Server

- [What is a Change Vector](../../server/clustering/replication/change-vector)  
- [Ongoing Tasks: General Info](../../server/ongoing-tasks/general-info)  
