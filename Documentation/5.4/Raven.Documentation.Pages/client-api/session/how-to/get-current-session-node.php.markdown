# Session: How to Get the Current Session Node

When working in a RavenDB cluster, a database can reside on multiple nodes.  
When the client needs to send a request to the server, it can have several nodes to choose from.

The client uses [this logic](../../../client-api/configuration/load-balance/overview#client-logic-for-choosing-a-node) 
to determine which node to send the request to.  
Learn more about load balancing the client requests in this [overview](../../../client-api/configuration/load-balance/overview).

Use the `getCurrentSessionNode` method from the `advanced` session operations 
to find out what is the current node that the session sends its requests to.

## Syntax

{CODE:php current_session_node_1@ClientApi\Session\HowTo\GetCurrentSessionNode.php  /}

### Return Value

The return value of `getCurrentSessionNode` is a `ServerNode` object
{CODE:php current_session_node_2@ClientApi\Session\HowTo\GetCurrentSessionNode.php  /}

## Example

{CODE:php current_session_node_3@ClientApi\Session\HowTo\GetCurrentSessionNode.php  /}

## Related articles

### Client API

- [How a Client Integrates with Replication and the Cluster](../../../client-api/cluster/how-client-integrates-with-replication-and-cluster)
- [Cluster Node Health Check](../../../client-api/cluster/health-check)

### Clustering

- [Overview](../../../server/clustering/overview)
