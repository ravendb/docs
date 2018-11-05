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

{NOTE: Source data for example queries}
For the following samples we will be using RavenDB Sample data set - the Northwind database. You can read about it and how to set it up [here](../../../studio/database/tasks/create-sample-data).
{NOTE/}


### Example I

The following query would fetch the management chain of each employees. Notice the use of **recursive** block over **ReportsTo** edge. This allows traversal down the reporting chain to fetch all the managers in the "command" chain. 
Also, note that **chainOfManagers** would return an ordered array of IDs - which are a path traversed by the recursive query between the initial vertex to the final vertex of the path.
{CODE-BLOCK:sql}
match (Employees as employee)-recursive as chainOfManagers { [ReportsTo as reportsTo] }->(Employees as manager)
select 
{
    EmployeeName : employee.FirstName, 
    ReportsTo: chainOfManagers.map(x => x.reportsTo).join(' >> '), 
    ManagerName: manager.FirstName
}
{CODE-BLOCK/}
This query would yield the following results.  Note how **chainOfManagers** yields array of IDs traversed as part of recursive query.
![Example I query results.](images/recursive_query_results_1.png)



### Example II

The following query would retrieve the management chain of each employees when there is a minimum of two managers and maximum of three managers above an employee.
{CODE-BLOCK:sql}
match (Employees as employee)-recursive as chainOfManagers(2,3) { [ReportsTo as reportsTo] }->(Employees as manager)
select 
{
    EmployeeName : employee.FirstName, 
    ReportsTo: chainOfManagers.map(x => x.reportsTo).join(' >> '), 
    ManagerName: manager.FirstName
}
{CODE-BLOCK/}
This query would yield the following results.  Notice how the addition of **(2,3)** to the **recursive** definition in the query limits the results to only those that have between two and three hops in the path.
![Example II query results.](images/recursive_query_results_2.png)

{NOTE: Recursive queries that match multiple possible paths}

{NOTE/}

### Example III

The following query would retrieve the management chain of each employees that is the longest out of all possible.
{CODE-BLOCK:sql}
match (Employees as employee)-recursive as chainOfManagers(1,3,longest) { [ReportsTo as reportsTo] }->(Employees as manager)
select 
{
    EmployeeName : employee.FirstName, 
    ReportsTo: chainOfManagers.map(x => x.reportsTo).join(' >> '), 
    ManagerName: manager.FirstName
}
{CODE-BLOCK/}
This query would yield the following results. The amount of results is the same as in _Example I_, but since we specified **longest** in **recursive** definition, when there is a longer path available, the query chooses the longest path available.
![Example III query results.](images/recursive_query_results_3.png)

### Example IV

It is possible to include sub-pattern to traverse recursively. The following query would retrieve manager documents in a path, essentially traversing not on edges, but on **edge => vertex** pattern.
{CODE-BLOCK:sql}
match (Employees as employee)-recursive as chainOfManagers (longest) { [ReportsTo as reportsTo]->(Employees as manager) }
select 
{
    EmployeeName : employee.FirstName, 
    ReportsTo: chainOfManagers.map(x => x.reportsTo).join(' >> '), 
    managementChain: chainOfManagers.map(x => x.manager.FirstName).join(' >> ')
}
{CODE-BLOCK/}
As we can see, expanding the **recursive** scope to a **manager** vertex allows them to be included in retrieved path. 
![Example IV query results.](images/recursive_query_results_4.png)
