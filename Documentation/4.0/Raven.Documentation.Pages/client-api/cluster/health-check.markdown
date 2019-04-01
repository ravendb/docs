# Cluster: Cluster Node Health Check

A health check sends an HTTP request to the `/databases/[Database Name]/stats` endpoint. 
If the request is successful, it will reset node failure counters which will cause the client to try sending operations to that specific node again.

### When Does it Trigger?

Any time a low-level [operation](../operations/what-are-operations) fails to connect to a node, the client spawns a health check thread for that particular node. 
The thread will periodically ping the not responding server until it gets a proper response.
The frequency of pinging the non-responsive server will start from 100ms and will gradually increase until it reaches 5sec intervals.

## Related articles

### Cluster

- [Clustering Overview](../../server/clustering/overview)
- [How a Client Integrates with Replication and the Cluster](../../client-api/cluster/how-client-integrates-with-replication-and-cluster)
- [Client Speed Test](../../client-api/cluster/speed-test)

### Configuration

- [Load Balance & Failover](../../client-api/configuration/load-balance-and-failover)
