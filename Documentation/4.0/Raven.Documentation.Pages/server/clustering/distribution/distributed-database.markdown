# Distributed Database

In RavenDB every replica is a full copy of the same database.

## Database Node

The node on which a specific database resides.

## Database Group

The collection of database nodes of the same database.

---
### Failover
When the client is initialized it gets a list of nodes that are in the [Database Group](../../glossary/database-group), so when one of the nodes is down (or there are network issues), the client will automatically and transparently try contacting other nodes.  
If a topology is changed after the client was initialized, the client would have old topology etag, and this would make the client fetch the updated topology from the server.
How the failover nodes are selected, depends on the configuration of *read balance behavior*, which can be configured either in the [Studio](../../studio/server/client-configuration) or in the client.  

### Load-balance
The load-balance behavior can be one of three types:  

  * None - No load balance behavior at all.  
  * Round Robin - Each request from the client will address another node.  
  * Fastest Node - Each request will go to the fastest node. The fastest node will be recalculated.  