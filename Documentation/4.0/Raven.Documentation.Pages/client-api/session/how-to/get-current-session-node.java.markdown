# Session : How to Get the Current Session Node

When working in a RavenDB cluster, a database can reside on multiple nodes. When the client needs to send a request to the server, it can have several nodes to choose from.

The choice of the node depends on the value of `ReadBalanceBehavior`, which is taken from the current **conventions** (read more about it [here](../../../client-api/configuration/cluster)).

In order to find out what is the current node that the session sends its requests to, use the `getCurrentSessionNode` method  from the `advanced` session operations.

## Syntax

{CODE:java current_session_node_1@ClientApi\Session\HowTo\GetCurrentSessionNode.java /}

### Return Value

The return value of `getCurrentSessionNode` is a **ServerNode** object
{CODE:java current_session_node_2@ClientApi\Session\HowTo\GetCurrentSessionNode.java /}

## Example

{CODE:java current_session_node_3@ClientApi\Session\HowTo\GetCurrentSessionNode.java /}
