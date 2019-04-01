#Graph API: Recursive Queries

### What are Recursive Queries and why would we want to use them?
We have seen in [this](#) article how we can execute graph queries to find matching sub-graph patterns. This is useful when we are looking for specific patterns in a graph.
But what if we don't know the exact pattern that we look for?

Consider the following graph that models employees, their managers and reporting hierarchy:
![Chain of management graph](images/ChainOfManagementGraph.png)


If we want to know which is the direct manager of each employee, we would simply issue a regular pattern match query. 
Now, if we would like to fetch all managers ranking above a certain employee (managers of managers and so on), 
we would use a ```recursive``` query, and such 'chain' of vertices that point to each other we would call a _**path**_.

#### Simple example
The following query would fetch all manager IDs ranking above of ```"employee/6"```:
{CODE-BLOCK:sql}
match (Employees as employee where id() = 'employees/6-A') //starting node of the recursion
    -recursive as chainOfManagers //here we specify that we want to recursive over a pattern
        { [ReportsTo as reportsTo] } //recurse over those vertices. 
            ->(Employees as manager) //recursion 'destination' node
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
It is possible there would be multiple paths in the query results. If only one path is retrieved, ```RecursiveMatchType``` value will cause the same query results for all values.
{NOTE/}

{NOTE: Recursive query examples }

#### Sample dataset
For the following examples we will be using RavenDB Sample data set - the Northwind database. You can read about it and how to set it up [here](../../../studio/database/tasks/create-sample-data).

#### The examples
In all of the example queries we will be using JavaScript projection in the ```select``` clause. As in regular graph queries, this is not required, because recursive queries are essentially a special type of a graph query.
In general, recursive queries have the same syntax as regular graph queries, with the exception of ```recursive``` clause and it's scope.

  * [Example I](../../../indexes/querying/graph/graph-recursive#example-i---basic-recursive-query) - in this example we show the most basic form of a recursive query.
  * [Example II](../../../indexes/querying/graph/graph-recursive#example-ii---traversal-limits) - in this example we show how we can limit traversal of a recursive query.
  * [Example III](../../../indexes/querying/graph/graph-recursive#example-iii---specifying-recursive-match-type) - in this example we show how we can specify recursive match strategy type.
  * [Example IV](../../../indexes/querying/graph/graph-recursive#example-iv---including-both-nodes-and-edges-in-recursion-pattern) - in this example we show how we can include both nodes and edges in the recursion pattern.
  * [Example V](../../../indexes/querying/graph/graph-recursive#example-v---) - in this example we show how the usage of ```all``` matching strategy would return all possible paths.
  
{NOTE/}


{PANEL: Example I - Basic Recursive Query}

The following query would fetch the management chain of each employees. Notice the use of ```recursive``` block over ```ReportsTo``` edge. This allows traversal down the reporting chain to fetch all the managers in the "command" chain. 
Also, note that ```chainOfManagers``` would return an ordered array of IDs - which are a path traversed by the recursive query between the initial node to the final node of the path.
{CODE-BLOCK:sql}
match (Employees as employee)-recursive as chainOfManagers { [ReportsTo as reportsTo] }->(Employees as manager)
select 
{
    EmployeeName : employee.FirstName, 
    ReportsTo: chainOfManagers.map(x => x.reportsTo).join(' >> '), 
    ManagerName: manager.FirstName
}
{CODE-BLOCK/}
This query would yield the following results.  Note how ```chainOfManagers``` yields array of IDs traversed as part of recursive query.
![Example I query results.](images/recursive_query_results_1.png)
{PANEL/}

{PANEL: Example II - Traversal limits}

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
This query would yield the following results.  Notice how the addition of ```(2,3)``` to the ```recursive``` definition in the query limits the results to only those that have between two and three hops in the path.
![Example II query results.](images/recursive_query_results_2.png)
{PANEL/}


{PANEL: Example III - Specifying recursive match type}

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
This query would yield the following results. The amount of results is the same as in _Example I_, but since we specified ```longest``` in ```recursive``` definition, when there is a longer path available, the query chooses the longest path available.
![Example III query results.](images/recursive_query_results_3.png)

{PANEL/}

{PANEL: Example IV - Including both nodes and edges in recursion pattern}

What if we want to retrieve not only the _IDs_ of documents in traversal path, but vertices that appear along the path as well? Then we would expand the scope of ```recursive``` statement so it contains the node as well, as can be seen in the following query.
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
As we can see, expanding the ```recursive``` scope to a ```manager``` node allows them to be included in retrieved path. In those results we retrieved names of managers in a chain of management without doing any additional 'load document' operations.
![Example IV query results.](images/recursive_query_results_4.png)
{PANEL/}

{PANEL: Example V - 'all' as a special recursive match strategy}
For many use-cases it is enough to determine whether a path exists or not, or to fetch one valid path. But what if we want to fetch all possible paths between two nodes?
For such use-cases, we would use ```all``` matching strategy.
Consider the following graph data:
![Example V - graph with multiple path.](images/MultiplePossibleTraversalPaths.png)

The query itself:
{CODE-BLOCK:sql}
match(Dogs as Buddy where id() = 'dogs/1') 
    -recursive as RecursiveLikes (all) 
        { [Likes as PathElement] } -> 
            (Dogs as TraversalDestination)
select 
{
    IdOfTraversalStart : id(Buddy), 
    LikesPath : RecursiveLikes.map(x => x.PathElement).join(' >> '), 
    IdOfTraversalEnd : id(TraversalDestination)
}
{CODE-BLOCK/}
When executed, the following results would be displayed:
![Example V query results.](images/recursive_query_results_5.png)

We are seeing two different paths, since there are two paths possible for ```"dogs/1"``` node.

{NOTE: Bug in the implementation}
This is a [bug](https://issues.hibernatingrhinos.com/issue/RavenDB-12263) that is yet to be fixed, since the results for this query should be two pathes : ```"dogs/3 >> dogs/5 >> dogs/6"``` and ```"dogs/2 >> dogs/4 >> dogs/6"```
{NOTE/}

{PANEL/}
