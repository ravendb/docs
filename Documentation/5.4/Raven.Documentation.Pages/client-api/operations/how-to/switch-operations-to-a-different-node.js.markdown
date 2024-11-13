# Switch Operations to a Different Node

---

{NOTE: }

* By default, when working with multiple nodes,  
  all client requests will access the server node that is defined by the client configuration.  
  (Learn more in: [Load balancing client requests](../../../client-api/configuration/load-balance/overview)).

* However, **server maintenance operations** can be executed on a specific node by using the `forNode` method.  
  (An exception is thrown if that node is not available).

* In this page:
    * [Server maintenance operations - forNode](../../../client-api/operations/how-to/switch-operations-to-a-different-node#server-maintenance-operations---fornode)
{NOTE/}

---

{PANEL: Server maintenance operations - forNode}

* For reference, all server maintenance operations are listed [here](../../../client-api/operations/what-are-operations#server-maintenance-operations).

{CODE:nodejs for_node_1@client-api\Operations\HowTo\switchOperationsToDifferentNode.js /}

**Syntax**:

{CODE:nodejs syntax_1@client-api\Operations\HowTo\switchOperationsToDifferentNode.js /}

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
