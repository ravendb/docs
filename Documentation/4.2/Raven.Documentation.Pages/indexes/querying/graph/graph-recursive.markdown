#Graph API : Recursive Queries

### What are Recursive Queries?
Recursive graph queries allows for conditional graph traversal as part of sub-graph pattern matching. This can be useful when we look for a pattern that is can be of unknown length.

### Syntax

| Parameters | Type | Description |
| ------------- | ------------- | ----- |
| **Min** | int | Minimum hops between vertices when recursively traversing the graph |
| **Max** | int | Maximum hops between vertices when recursively traversing the graph |
| **RecursiveMatchType** | string | If multiple paths are possible, how should we choose which paths to return as results? It is set to 'Lazy' by default. |

{NOTE:RecursiveMatchType values}
If only one path is possible between two vertices, **RecursiveMatchType** value will affect nothing.
{NOTE/}

The following **RecursiveMatchType** values are possible:

| Match Type | What it does |
| ---------- | ------------ |
| Lazy | Return the **_first_ path** matched by query conditions |
| Longest | Out of all types matched by query conditions, return the **_longest_ path** |
| Shortest | Out of all types matched by query conditions, return the **_shortest_ path** |
| All | Return **_all_ distinct paths** that are matched by query conditions |

### Example I
Assuming a Northwind database. 
The following query would fetch the management chain of each employees. Notice the use of **recursive** block over **ReportsTo** edge. This allows traversal down the reporting chain to fetch all the managers in the "command" chain. 
Also, note that **chainOfManagers** would return an ordered array of IDs - which are a path traversed by the recursive query between the initial vertex to the final vertex of the path.
{CODE-BLOCK:sql}
match (Employees as employee)-recursive as chainOfManagers { [ReportsTo] }->(Employees as manager)
{CODE-BLOCK/}

### Example II
Assuming a Northwind database. 
The following query would retrieve the management chain of each employees when there is a minimum of two managers above an employee.
{CODE-BLOCK:sql}
match (Employees as employee)-recursive as chainOfManagers (2,3) { [ReportsTo] }->(Employees as manager)
{CODE-BLOCK/}

### Example III
Assuming a Northwind database. 
The following query would retrieve the management chain of each employees that is the longest out of all possible.
{CODE-BLOCK:sql}
match (Employees as employee)-recursive as chainOfManagers (1,8,'longest') { [ReportsTo] }->(Employees as manager)
{CODE-BLOCK/}

### Example IV
Assuming a Northwind database.
It is possible to include sub-pattern to traverse recursively. The following query would retrieve manager documents in a path, essentially traversing not on edges, but on **edge => vertex** pattern.
{CODE-BLOCK:sql}
match (Employees as employee)-recursive as chainOfManagers { [ReportsTo]->(Employees as manager) }
{CODE-BLOCK/}
