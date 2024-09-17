# Events and Snapshots
---

{NOTE: }

* Akka.Persistence provides two primary methods to persist actor state: **Event sourcing** and **Snapshots**.

* With event sourcing, each state change is stored as a separate event, creating a sequence of events that represents the actor’s history. 
  Snapshots, on the other hand, capture the actor’s state at specific points in time.

* Upon actor restart, both events and snapshots can be replayed to restore the actor's internal state,  
  with snapshots allowing for quicker recovery by avoiding the need to replay all past events.

* The stored events can be queried via Akka's query interface.  
  Learn more about that in [Queries](../../integrations/akka.net-persistence/queries).

* To learn how to configure the events journal and the snapshot-store via the _Akka.Persistence.RavenDB_ plugin, 
  see [Integrating with Akka.NET persistence](../../integrations/akka.net-persistence/integrating-with-akka-persistence).

* In this page:
   * [Storing events](../../integrations/akka.net-persistence/events-and-snapshots#storing-events)
   * [Storing snapshots](../../integrations/akka.net-persistence/events-and-snapshots#storing-snapshots)
   * [Storing guidelines](../../integrations/akka.net-persistence/events-and-snapshots#storing-guidelines)
   * [Global consistency](../../integrations/akka.net-persistence/events-and-snapshots#global-consistency)
   * [Sample application](../../integrations/akka.net-persistence/events-and-snapshots#sample-application)

{NOTE/}

---

{PANEL: Storing events}

**Events**:  
Persistent actors can write messages, called events, into the configured RavenDB database,  
which serves as the events journal.

**The Events collection**:  
Each event is stored as a document in the `Events` collection in append-only mode.  

**The Event document**:  
Each event document includes the following fields, among others:  

  * `id` - The event document id, composed of `<Events/persistentId/sequenceNr-with-leading-zeros>`
  * `payload` - The actual message content or event data.  
  * `persistentId` - The unique identifier of the actor that persisted this event.
  * `sequenceNr` - The sequence number for the event, indicating its position in the sequence of events for a particular actor.  
                   Serves as a unique, gap-less identifier that helps maintain the correct order and consistency of the actor's state.

**Replaying events**:  
Maintaining the event documents in chronological order (based on the `sequenceNr` field)  
enables retrieval and replay in the correct sequence when an actor restarts.

{PANEL/}

{PANEL: Storing snapshots}

**Snapshots**:  

  * Snapshots capture the current state of an actor at a specific point in time,  
    representing all the data the actor has accumulated or processed up to that moment.
 
  * Persistent actors can store these snapshots in the configured RavenDB database,  
    which serves as the snapshot-store.

  * After a snapshot is successfully persisted, events can be deleted from the events journal to free up space.  

**The Snapshots collection**:  
Each snapshot is stored as a document in the `Snapshots` collection in append-only mode.  

**The Snapshot document**:  
Each snapshot document includes the following fields, among others:

  * `id` - The snapshot document id, composed of `<Snapshots/persistentId/sequenceNr-with-leading-zeros>`  
  * `payload` - The actor's state at the time the snapshot was taken.  
  * `persistentId` - The unique identifier of the actor that created the snapshot.
  * `sequenceNr` - The sequence number indicating the position of the snapshot in the sequence of events.  
                   Serves as a unique, gap-less identifier that helps maintain the correct order and consistency of the actor's state.

**Replaying snapshots**:  

  * When an actor restarts, instead of replaying the entire event history from the events journal,  
    which can be inefficient as this journal grows, the actor's state can be restored from a snapshot  
    and then replay only the events that occurred after that snapshot.

  * Replaying snapshots significantly accelerates recovery, reduces network transmission,  
    and lowers both actor event replay time and CPU usage.

{PANEL/}

{PANEL: Storing guidelines}

{INFO: }

* The RavenDB plugin designates the Events and Snapshots collections for storing Akka’s data.  
  While it’s technically possible to store documents from other sources in these collections,  
  you shouldn't do so.
 
* The Events and Snapshots collections should be reserved exclusively for Akka’s storage needs.  
  It is recommended to place these collections in a separate, designated database.

{INFO/}

{PANEL/}

{PANEL: Global consistency}

**The consistency requirement**:  

  * Consistency refers to the property that ensures data is uniform and accurate across all database replicas at a given point in time.
    In a distributed system, Akka.NET Persistence relies on consistency to accurately restore an actor’s state from its events during recovery, 
    regardless of which node is contacted.

  * Events must be applied (replayed) in the exact order they were generated,  
    so consistency is crucial to ensure that no events are missed or processed out of order.

**Cluster-wide transactions**:  

  * RavenDB is a distributed database, allowing writes, reads, and queries to target different nodes across the cluster.  

  * To prioritize consistency over availability, the RavenDB plugin uses a [cluster-wide transaction](../../server/clustering/cluster-transactions) for storing events and snapshot documents. 
    This ensures that persisted data is consistently applied across all database instances in the cluster, preventing conflicts and guaranteeing that restoring to the latest state reflects the correct event sequence, as required by Akka.

  * Note that cluster consensus is required for a cluster-wide transaction to execute. 
    This means that a majority of nodes in the [database group](../../studio/database/settings/manage-database-group) must be up and connected in order to persist new events & snapshots.

**Atomic-guards usage**:  

  * As with every document created using a cluster-wide transaction in RavenDB, 
    the server creates an [Atomic-Guard](../../client-api/session/cluster-transaction/atomic-guards) for each event or snapshot document that is stored to prevent concurrent modifications.

  * The atomic-guard is particularly beneficial in scenarios where an actor recovers its events and snapshots from a node that failed, 
    came back up, but has not yet received the complete replication information from the other nodes in the database group. 
    In such cases, the actor’s state might not be fully up-to-date.

  * If the actor attempts to write a new event using a _sequenceNr_ that already exists, the Atomic-Guard will prevent this action from succeeding. 
    Upon this failure, the actor will restart itself. If, by that time, the node has received all the missing information, the actor will now recover with a fully updated state.

{PANEL/}

{PANEL: Sample application}

The following is a sample application that stores events and snapshots in a RavenDB database.  

{CODE-TABS}
{CODE-TAB:csharp:Main main@Integrations\AkkaPersistence\AkkaPersistenceSampleApp.cs /}
{CODE-TAB:csharp:SalesActor sales_actor@Integrations\AkkaPersistence\AkkaPersistenceSampleApp.cs /}
{CODE-TAB:csharp:SalesSimulatorActor sales_simulator_actor@Integrations\AkkaPersistence\AkkaPersistenceSampleApp.cs /}
{CODE-TAB:csharp:Classes classes@Integrations\AkkaPersistence\AkkaPersistenceSampleApp.cs /}
{CODE-TABS/}

---

The documents created in the Events and Snapshots collections are visible in the Documents View in the Studio:

#### The Events collection

![The events collection](images/the-events-collection.png "The Events collection")

1. The Events collection.
2. The event document ID in the format: `<Events/persistentId/sequenceNr-with-leading-zeros>`
3. The unique ID of the actor that persisted these events.
4. The unique sequence number of the event.
5. The data that was stored for the event.

![The payload](images/the-payload.png "The event payload")

The data stored for each event is an instance of the `Sale` class, containing `Price` and `Brand` fields.

---

#### The Snapshots collection

![The snapshots collection](images/the-snapshots-collection.png "The Snapshots collection")

1. The Snapshots collection.
2. The snapshot document ID in the format: `<Snapshots/persistentId/sequenceNr-with-leading-zeros>`
3. The unique ID of the actor that persisted this snapshot. 
4. The sequence number of the event after which the snapshot was stored, `5` in this case.
5. The data stored in this snapshot represents the actor's state immediately after event 5 was stored.  
   In this example, it reflects the **accumulated sales profit** made after the first 5 sale events.  

{PANEL/}

## Related Articles

### Integrations  

[Integrating with Akka.Persistence](../../integrations/akka.net-persistence/integrating-with-akka-persistence)  
[Queries](../../integrations/akka.net-persistence/queries)
