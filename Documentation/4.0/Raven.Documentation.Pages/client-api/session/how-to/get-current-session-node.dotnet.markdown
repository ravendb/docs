# Session : How to Get the Current Session Node

When working in a RavenDB cluster, a database can reside on multiple nodes. When the client needs to send a request to the server, it can have several nodes to choose from.

The choice of the node depends on the value of `ReadBalanceBehavior`, which is taken from the current **conventions** (read more about it [here](../../../client-api/configuration/cluster)).

In order to find out what is the current node that the session sends its requests to, use the `GetCurrentSessionNode` method  from the `Advanced` session operations.

## Syntax

{CODE current_session_node_1@ClientApi\Session\HowTo\GetCurrentSessionNode.cs /}

### Return Value

The return value of `GetCurrentSessionNode` is a **ServerNode** object
{CODE current_session_node_2@ClientApi\Session\HowTo\GetCurrentSessionNode.cs /}

## Example

{CODE current_session_node_3@ClientApi\Session\HowTo\GetCurrentSessionNode.cs /}
