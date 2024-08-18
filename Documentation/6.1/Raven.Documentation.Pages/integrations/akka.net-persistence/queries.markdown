# Queries
---

{NOTE: }


* Akka.Persistence.Query comes with several stream-based query interfaces for querying persisted data.  
  This interface abstracts the underlying database, allowing your application to switch persistence providers without requiring changes to the query code.
 
* The RavenDB persistence plugin fully supports all Akka's query interfaces.  
  Just include `Akka.Persistence.RavenDb.Query` in your application.

* view queries in traffic watch under 'streams' todo..
 
* In this page:
  * [Interface types](../../integrations/akka.net-persistence/queries#interface-types)
  * [Supported interfaces](../../integrations/akka.net-persistence/queries#supported-interfaces)
     * [IPersistenceIdsQuery & ICurrentPersistenceIdsQuery](../../integrations/akka.net-persistence/queries#ipersistenceidsquery--icurrentpersistenceidsquery)
     * [IEventsByPersistenceIdQuery & ICurrentEventsByPersistenceIdQuery](../../integrations/akka.net-persistence/queries#ieventsbypersistenceidquery--icurrenteventsbypersistenceidquery)
     * [IEventsByTagQuery & ICurrentEventsByTagQuery](../../integrations/akka.net-persistence/queries#ieventsbytagquery--icurrenteventsbytagquery)
     * [IAllEventsQuery & ICurrentAllEventsQuery](../../integrations/akka.net-persistence/queries#ialleventsquery--icurrentalleventsquery)
  * [Indexes](../../integrations/akka.net-persistence/queries#indexes)

{NOTE/}

---

{PANEL: Interface types}

Each query interface comes in two forms to allow flexible querying based on whether you need  
real-time updates (continuous) or a snapshot of the current state (current).

* **Continuous Query** (e.g., `EventsByPersistenceId`):  
  This type of query continuously streams data as it is persisted.  
  It starts from a specified offset (or from the beginning if no offset is provided)  
  and keeps the stream open to deliver new data as it is added.

* **Current Query** (e.g., `CurrentEventsByPersistenceId`):    
  This type of query retrieves only the data available up to the point of the query.  
  Once all current data is fetched, the stream is completed.  
  Data that is persisted after the query is completed is Not included in the stream.

{PANEL/}

{PANEL: Supported interfaces}

{NOTE: }

#### IPersistenceIdsQuery & ICurrentPersistenceIdsQuery

---

Use these methods to retrieve the PersistenceIds of ALL actors that have persisted events to the journal store:

`PersistenceIds()`  
The stream does Not complete when it reaches the end of the PersistenceIds list that currently exists in the journal store.
Instead, it continues to push new PersistenceIds as they are added.

`CurrentPersistenceIds()`  
The stream is completed immediately when it reaches the end of the result set.  
PersistenceIds that are created after the query is completed are Not included in the stream.

{CODE:csharp query_1@Integrations\AkkaPersistence\AkkaPersistenceSampleApp.cs /}

**Syntax**:

{CODE:csharp syntax_1@Integrations\AkkaPersistence\queries.cs /}
{CODE:csharp syntax_1_current@Integrations\AkkaPersistence\queries.cs /}

{NOTE/}

{NOTE: }

#### IEventsByPersistenceIdQuery & ICurrentEventsByPersistenceIdQuery

---

* Use the methods below to retrieve events that have been persisted by a specific actor.
 
* The returned event stream is ordered by the sequence numbers of the events.

`EventsByPersistenceId()`  
The stream does Not complete when it reaches the end of the currently stored events.  
Instead, it continues to push new events as they are persisted.

`CurrentEventsByPersistenceId()`  
The stream is completed immediately when it reaches the end of the result set.  
Events that are stored after the query is completed are Not included in the event stream.

{CODE:csharp query_2@Integrations\AkkaPersistence\AkkaPersistenceSampleApp.cs /}

**Syntax**:

{CODE:csharp syntax_2@Integrations\AkkaPersistence\queries.cs /}
{CODE:csharp syntax_2_current@Integrations\AkkaPersistence\queries.cs /}

| Parameter           | Type     | Description                                                                                                 |
|---------------------|----------|-------------------------------------------------------------------------------------------------------------|
| **persistenceId**   | `string` | The actor's persistence ID for which to retrieve events.                                                    |
| **fromSequenceNr**  | `long`   | Retrieve events from this sequenceNr.                                                                       |
| **toSequenceNr**    | `long`   | Retrieve events up to this sequenceNr.<br>Use `0L` and `long.MaxValue` respectively to retrieve all events. |

{NOTE/}

{NOTE: }

#### IEventsByTagQuery & ICurrentEventsByTagQuery

---

* In Akka.Persistence, you can add one or more string **tags** to events.  

* Use the methods below to retrieve events that have a specific tag.  
  The query will be applied to all events persisted by all actors.  
  Results will include events with the specified tag, regardless of the PersistenceId they are associated with.

* You can specify the change-vector of an event document as the **offset** to determine where in the event stream you want to start querying.
  * In RavenDB, a [change-vector](../../server/clustering/replication/change-vector) is a unique identifier that represents the version of a document (an event in this case)  
    across different nodes in a distributed database.  
  * The change-vector of a document can be obtained from the Properties pane in the [Document View](../../studio/database/documents/document-view#the-document-view) in the Studio.  

* The returned event stream is ordered by the change-vector value of the event documents.

`EventsByTagQuery()`  
The stream does Not complete when it reaches the end of the currently stored events.  
Instead, it continues to push new events as they are persisted.

`CurrentEventsByTagQuery()`  
The stream is completed immediately when it reaches the end of the result set.  
Events that are stored after the query is completed are Not included in the event stream.

{CODE:csharp query_3@Integrations\AkkaPersistence\AkkaPersistenceSampleApp.cs /}

**Syntax**:

{CODE:csharp syntax_3@Integrations\AkkaPersistence\queries.cs /}
{CODE:csharp syntax_3_current@Integrations\AkkaPersistence\queries.cs /}

| Parameter   | Type                 | Description                                                           |
|-------------|----------------------|-----------------------------------------------------------------------|
| **tag**     | `string`             | Retrieve only events that contain this tag.                           |
| **offset**  | `null`               | Retrieve all events from the beginning, no offset is applied.         |
| **offset**  | `Offset.NoOffset`    | Retrieve all events from the beginning, no offset is applied.         |
| **offset**  | `Offset.Sequence(0)` | Retrieve all events from the beginning, no offset is applied.         |
| **offset**  | `ChangeVectorOffset` | Provide a change-vector to retrieve events starting after this point. |

Note:  
`Offset.TimeBasedUuid` is not supported.  
`Offset.Sequence(x)` where x is > 0 is not supported.  

{NOTE/}

{NOTE: }

#### IAllEventsQuery & ICurrentAllEventsQuery

---

* Use the methods below to retrieve all events regardless of which PersistenceId they are associated with.

* The returned event stream is ordered by the change-vector value of the event documents.

`AllEvents()`  
The stream does Not complete when it reaches the end of the currently stored events.  
Instead, it continues to push new events as they are persisted.

`CurrentAllEvents()`  
The stream is completed immediately when it reaches the end of the result set.  
Events that are stored after the query is completed are Not included in the event stream.

{CODE:csharp query_4@Integrations\AkkaPersistence\AkkaPersistenceSampleApp.cs /}

**Syntax**:

{CODE:csharp syntax_4@Integrations\AkkaPersistence\queries.cs /}
{CODE:csharp syntax_4_current@Integrations\AkkaPersistence\queries.cs /}

The available options for the `offset` parameter are the same as those listed  
for the `EventsByTag` & `CurrentEventsByTag` methods above.

{NOTE/}

{PANEL/}

{PANEL: Indexes}

* To support these queries, the RavenDB plugin automatically creates internal static-indexes,  
  optimized for fast data retrieval.

* The indexes created are:

    * `ActorsByChangeVector`
    * `EventsByTagAndChangeVector`

{PANEL/}

## Related Articles

**Integrations**:  

[Integrating with Akka.Persistence](../../integrations/akka.net-persistence/integrating-with-akka-persistence)  
[Events and Snapshots](../../integrations/akka.net-persistence/events-and-snapshots)  
