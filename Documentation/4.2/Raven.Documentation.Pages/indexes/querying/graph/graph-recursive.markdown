#Graph API : Recursive Queries

### What are Recursive Queries and why would we want to use them?
We have seen in [this](#) article how we can execute graph queries to find matching sub-graph patterns. This is useful when we are looking for specific patterns in a graph.
But what if we don't know the exact pattern that we look for?

Consider the following graph that models employees, their managers and reporting hierarchy:
![Chain of management graph](images/ChainOfManagementGraph.png)


If we want to know which is the direct manager of each employee, we would simply issue a regular pattern match query. 
Now, if we would like to fetch all managers ranking above a certain employee (managers of managers and so on), 
we would use a **recursive** query, and such 'chain' of vertices that point to each other we would call a _**path**_.

#### Simple example
The following query would fetch all manager IDs ranking above of _"employee/6"_:
{CODE-BLOCK:sql}
match (Employees as employee where id() = 'employees/6-A') //starting vertex of the recursion
    -recursive as chainOfManagers //here we specify that we want to recursive over a pattern
        { [ReportsTo as reportsTo] } //recurse over those vertices. 
            ->(Employees as manager) //recursion 'destination' vertex
select { managerIdsAsPath : chainOfManagers.map(x => x.reportsTo) }
{CODE-BLOCK/}

We call such array that is gathered while the recursive query traverses the graph as **path**, the same path we defined above. 
For the example data seen above in the chart, such query would traverse the following part of the graph:
![Chain of management graph - recursive traversal](images/ChainOfManagementGraph_recursive_traversal.png)

### Syntax of a 'recursive' clause

| Parameters | Type | Description |
| ------------- | ------------- | ----- |
| **Min** | int | Minimum hops between vertices when recursively traversing the graph. If traversal didn't reach the minimum amount of hops, the result would be discarded. |
| **Max** | int | Maximum hops between vertices when recursively traversing the graph. If traversal reaches the maximum specified amount of hops, it will stop. |
| **RecursiveMatchType** | string | If multiple paths are possible, how should we choose which paths to return as results? It is set to 'Lazy' by default. |

The following **RecursiveMatchType** values are possible:

| Match Type | What it does |
| ---------- | ------------ |
| Lazy | Return the **_first_ path** matched by query conditions |
| Longest | Out of all types matched by query conditions, return the **_longest_ path** |
| Shortest | Out of all types matched by query conditions, return the **_shortest_ path** |
| All | Return **_all_ distinct paths** that are matched by query conditions |

{NOTE:RecursiveMatchType values}
It is possible there would be multiple paths in the query results. If only one path is retrieved, **RecursiveMatchType** value will cause the same query results for all values.
{NOTE/}

## Examples

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
match (Employees as employee)-recursive as chainOfManagers(1,longest) { [ReportsTo as reportsTo] }->(Employees as manager)
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

What if we want to retrieve not only the _IDs_ of documents in traversal path, but vertices that appear along the path as well? Then we would expand the scope of **recursive** statement so it contains the vertex as well, as can be seen in the following query.
{CODE-BLOCK:sql}
//since the match expression here is complex, 
//we simply break it into multiple rows - to increase readability
match (Employees as employee)-
        recursive as chainOfManagers (longest) //recursion definition
                { [ReportsTo as reportsTo]->(Employees as manager) } //pattern to 'recurse' on
select 
{
    EmployeeName : employee.FirstName, 
    ReportsTo: chainOfManagers.map(x => x.reportsTo).join(' >> '), 
    managementChain: chainOfManagers.map(x => x.manager.FirstName).join(' >> ')
}
{CODE-BLOCK/}
As we can see, expanding the **recursive** scope to a **manager** vertex allows them to be included in retrieved path. In those results we retrieved names of managers in a chain of management without doing any additional 'load document' operations.
![Example IV query results.](images/recursive_query_results_4.png)
