# Cluster : How a Speed Test Works

In RavenDB ClientAPI, if the _Read balance behavior_ is configured for the _Fastest node_ , under certain conditions the ClientAPI would execute a speed test for each node, so the fastest test would be selected.

### When Does it Trigger?

Once a client configuration is updated on a server, the next response from the server would include the following header: `Refresh-Client-Configuration`. 

When a client sees such a header for the first time, it will probe all nodes when the next read request will happen and store the fastest found.

After the first probe for speed, the client will repeat the speed test at most once per minute on a read request.

## Related articles

### Cluster

- [Clustering Overview](../../server/clustering/overview)
- [How a Client Integrates with Replication and the Cluster](../../client-api/cluster/how-client-integrates-with-replication-and-cluster)
- [Cluster Node Health Check](../../client-api/cluster/health-check)

### Configuration

- [Cluster](../../client-api/configuration/cluster)
