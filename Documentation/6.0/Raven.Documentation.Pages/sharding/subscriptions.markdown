﻿# Sharding: Subscriptions
---

{NOTE: }

* From a user's point of view, [Data Subscriptions](../client-api/data-subscriptions/what-are-data-subscriptions) 
  are created and [consumed](../client-api/data-subscriptions/consumption/how-to-consume-data-subscription) 
  in the exact same way when the database is sharded and when it is not.  
  
* Old clients are supported and can continue communicating with RavenDB 
  without knowing whether it is sharded or not.  
  Subscription workers of a non-sharded database are not required to change 
  anything to connect to a sharded database.  

* Data subscriptions in a sharded database are managed by orchestrators 
  that serve the workers, and shards that serve the orchestrators.  

* In this page:  
  * [Data Subscriptions in a Sharded Database](../sharding/subscriptions#data-subscriptions-in-a-sharded-database)  
  * [Unsupported Features](../sharding/subscriptions#unsupported-features)  

{NOTE/}

---

{PANEL: Data Subscriptions in a Sharded Database}

To allow data subscriptions in a sharded database:  

* From a user's point of view, creating a data subscription is 
  done once, [just like it is done](../client-api/data-subscriptions/creation/how-to-create-data-subscription) 
  under a non-sharded database.  
* Behind the scenes, though, the [orchestrator](../sharding/overview#client-server-communication) 
  that was appointed to handle this client and received its 
  subscription request uses the subscription's settings and 
  creates a data subscription with each [shard](../sharding/overview#shards).  
* Each shard independently organizes available data in batches 
  and sends the batches to the orchestrator in response to data 
  requests.  
* The orchestrator unifies the data sent by shards, handles 
  documents (e.g. by adding included documents even if the 
  original document arrives from one shard and the included 
  document from another), and arranges them in new consumable 
  batches.  
* The orchestrator keeps track of the worker's progress.  
  When the worker consumes its current delivery and requests 
  another, the orchestrator sends it the next available batch.  

Distributing the subscriptions between all shards this way 
allows RavenDB to serve its workers efficiently no matter 
how large the overall database gets.  

---

![Subscription](images/subscriptions.png "Subscription")

1. **Data Subscription Worker**  
2. **Worker Subscription and Data Requests**  
    * The worker subscribes with the orchestrator.  
      The worker is unaware that the destination database is sharded, 
      no special syntax or preparation is needed.  
    * The worker informs the orchestrator when all data was consumed.  
3. **Orchestrator**  
    * A subscription is created with the orchestrator.  
    * The orchestrator registers which data has been consumed by the worker 
      and which is still to be delivered.  
4. **Orchestrator Subscription and Data Requests**  
    * The orchestrator subscribes with all shards.  
    * The orchestrator informs all shards when all data was consumed.  
5. **Shard**  
    * A subscription is created with the shard.  
    * Relevant data is organized in consumable batches.  
      The shard registers which data has been consumed and which is still 
      to be delivered.  
6. **Data Delivery to Orchestrator**  
   When the orchestrator informs the shard that all data has been consumed 
   and the shard has an available batch, the batch is delivered to the orchestrator.  
6. **Data Delivery to Worker**  
   When the worker informs the orchestrator that all data has been consumed 
   and the orchestrator has an available batch, the batch is delivered to the worker.  

{PANEL/}

{PANEL: Unsupported Features}

Revisions features that are not supported yet under a sharded database:  

* [Concurrent Subscriptions](../client-api/data-subscriptions/concurrent-subscriptions)  
  Allowing multiple workers to connect a common subscription simultaneously.  
* [Data Subscriptions Revisions Support](../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning)  
  Subscribing to document revisions.  
* [SubscriptionCreationOptions.ChangeVector](../client-api/data-subscriptions/creation/api-overview#subscriptioncreationoptions<t>)  
  Providing an arbitrary change vector from which the subscription would start processing 
  is currently not supported.  
  {NOTE: }
  Setting `ChangeVector` to one of the following special values **is** supported:  
  `"LastDocument"` (the latest change vector on the database)  
  `"BeginningOfTime"` (the earliest change vector on the database)  
  `"DoNotChange"` (keep current subscription change vector)  
  {NOTE/}


{PANEL/}

## Related articles

**Data Subscriptions**  
[Data Subscriptions](../client-api/data-subscriptions/what-are-data-subscriptions)  
[Creating Subscription](../client-api/data-subscriptions/creation/how-to-create-data-subscription)  
[Consuming Data](../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)  
[Concurrent Subscriptions](../client-api/data-subscriptions/concurrent-subscriptions)  

**Sharding**  
[Orchestrator](../sharding/overview#client-server-communication)  
[Shard](../sharding/overview#shards)  
