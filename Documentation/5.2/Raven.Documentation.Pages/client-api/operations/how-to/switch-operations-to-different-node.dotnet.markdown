# Switch Operations to a Different Node

---

{NOTE: }

* By default, when working with multiple nodes,  
  all client requests will access the server node that is defined by the client configuration.  
  (Learn more in: [ReadBalanceBehavior](../../../client-api/configuration/load-balance-and-failover) & [LoadBalanceBehavior](../../../client-api/session/configuration/use-session-context-for-load-balancing)).

* __Maintenance server operations__ can be executed on a specific node by using the `ForNode` method.  
  (An exception is thrown if that node is not available).

* In this page:
    * [Maintenance server operations - ForNode](../../../client-api/operations/how-to/switch-operations-to-different-node#maintenance-server-operations---fornode)
{NOTE/}

---

{PANEL: Maintenance server operations - ForNode}

* For reference, all maintenance server operations are listed [here](../../../client-api/operations/what-are-operations#the-following-server-wide-operations-are-available).

{CODE for_node_1@ClientApi\Operations\HowTo\SwitchOperationsToDifferentNode.cs /}

__Syntax__:

{CODE syntax_1@ClientApi\Operations\HowTo\SwitchOperationsToDifferentNode.cs /}

| Parameters | Type | Description |
| - | - | - |
| **nodeTag** | string | The tag of the node to operate on |

| Return Value | |
| - | - |
| `ServerOperationExecutor` | New instance of Server Operation Executor that is scoped to the requested node |

{PANEL/}

## Related Articles

### Operations

- [What are Operations](../../../client-api/operations/what-are-operations)
