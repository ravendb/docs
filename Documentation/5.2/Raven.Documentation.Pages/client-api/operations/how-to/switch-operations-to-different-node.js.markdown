# Switch Operations to a Different Node

---

{NOTE: }

* By default, when working with multiple nodes,  
  all client requests will access the server node that is defined by the client configuration.  
  (Learn more in: [readBalanceBehavior](../../../client-api/configuration/load-balance-and-failover) & [loadBalanceBehavior](../../../client-api/session/configuration/use-session-context-for-load-balancing)).

* __Maintenance server operations__ can be executed on a specific node by using the `forNode` method.  
  (An exception is thrown if that node is not available).

* In this page:
    * [Maintenance server operations - forNode](../../../client-api/operations/how-to/switch-operations-to-different-node#maintenance-server-operations---fornode)
{NOTE/}

---

{PANEL: Maintenance server operations - forNode}

* For reference, all maintenance server operations are listed [here](../../../client-api/operations/what-are-operations#the-following-server-wide-operations-are-available).

{CODE:nodejs for_node_1@ClientApi\Operations\HowTo\switchOperationsToDifferentNode.js /}

__Syntax__:

{CODE:nodejs syntax_1@ClientApi\Operations\HowTo\switchOperationsToDifferentNode.js /}

| Parameters | Type | Description |
| - | - | - |
| **nodeTag** | string | The tag of the node to operate on |

| Return Value | |
| - | - |
| `Promise<ServerOperationExecutor>` | A promise that returns a new instance of Server Operation Executor<br>scoped to the requested node |

{PANEL/}

## Related Articles

### Operations

- [What are Operations](../../../client-api/operations/what-are-operations)
