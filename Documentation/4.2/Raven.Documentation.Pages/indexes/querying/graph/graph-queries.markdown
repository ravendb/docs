# Querying: Graph Queries

Graph queries are a new type of queries that allows you to query your documents as if they were a graph.  
The nodes which we query are your regular documents and the edges are just any string property that holds the id of another document.  

### How to graph query
For the following examples we will use the `Northwind` database, you can read how to generate it [here](../../../studio/database/tasks/create-sample-data).  
Lets start with the simplest of queries match an order:  

{CODE-BLOCK:plain}
match (Orders as o)
{CODE-BLOCK/}  

![Figure 1. Simple graph query result](images/simple_graph_query.JPG "Simple graph query result")

As we can see the results are simmilar to the following `RQL` query

{CODE-BLOCK:plain}
from Orders as o
{CODE-BLOCK/} 

Lets add an edge to the query  

{CODE-BLOCK:plain}
match (Orders as o)-[Employee as edge]->(Employees as e)
{CODE-BLOCK/}  

![Figure 2. Simple pattern query result](images/basic_pattern_query.JPG "Simple pattern query result")

As we can see the result of the above query is a tuple of an order as `o`, employee as `e` and the employee id as `edge`.  

The result is simmilar to the following `RQL` query but not exactly:  

{CODE-BLOCK:plain}
from Orders as o 
select o, o.Employee
include o.Employee
{CODE-BLOCK/} 

The above query will return the order, emploee id but the employee document is included and not part of the result.  
The fact that the emploee is part of the result means that we can actually query it too. 

Lets query the employee:  

{CODE-BLOCK:plain}
match (Orders as o)-[Employee as edge]->(Employees as e where HiredAt.Year = 1993)
{CODE-BLOCK/} 

![Figure 3. Filter destination pattern query result](images/filtering_destination_pattern.JPG "Filter destination pattern query result")

As you can see there are now `265` result rather than `830` as before since we filtered all tuples where their employee didn't match the query.  

We can concatenate more edges to the query, lets fetch the employee boss too:  

{CODE-BLOCK:plain}
match (Orders as o)-[Employee as _ ]->(Employees as e where HiredAt.Year = 1993)-[ReportsTo as __ ]->(Employees as boss)
{CODE-BLOCK/} 

![Figure 4. Concatenate pattern query result](images/concatenate_pattern_query.JPG "Concatenate pattern query result")

As you can see the query yields the order as `o` the employee as `e` and the employee boss as `boss`.  
You can also notice that the edge disappeared that is because we used the `as _` and `as __` aliases which means don't fetch.   

Now lets add projection to the query to make it leaner:  
{CODE-BLOCK:plain}
match (Orders as o)-[Employee as _ ]->(Employees as e where HiredAt.Year = 1993)-[ReportsTo as __ ]->(Employees as boss)
select 
{
    OrderId: id(o), 
    "Employee Name": e.FirstName+ " " + e.LastName, 
    "Boss Name": boss.FirstName+ " " + boss.LastName
}
{CODE-BLOCK/} 

![Figure 5. Projecting graph query results](images/projecting_graph_query.JPG "Projecting graph query results")

As you can see we can mix regular `RQL` clauses with the graph query to get complex queries answered.

Lets dive a little deeper into the moving parts of a graph queries.  


### Nodes
Nodes are documents, you select nodes using regular [RQL](../what-is-rql) queries inside a with clause.  

{CODE-BLOCK:plain}
with {from Orders} as o
{CODE-BLOCK/}

In the above with clause we choose nodes from the Orders collection and we will later refer to them as `o`.  

### Edges
Edges are properties of documents they could be:  

- Properties that are a document key.  
{CODE-BLOCK:plain}
{
    ...
    "User": "users/1-A"
    ...
}
{CODE-BLOCK/}  
- For properties that contain nested objects where that nested object has a property that is a document key, the nested object as a whole is considered an edge in this case.  
{CODE-BLOCK:plain}
{
    ...
    "User": 
    {
        ...
        "UserId":"users/1-A"
        ...
    }
    ...
}
{CODE-BLOCK/} 
- Properties that are an array of document keys, each key is a diffrent edge in this case.  
{CODE-BLOCK:plain}
{
    ...
    "Users": ["users/1-A", "users/3-A", ...,"users/283-A"]
    ...
}
{CODE-BLOCK/}  
- Properties that are an array of nested objects that contains a property that is a document key, each object is a diffrent edge in this case.  
{CODE-BLOCK:plain}
{
    ...
    "Users":  [ 
    {
        ...
        "UserId":"users/1-A"
        ...
    },
    {
        ...
        "UserId":"users/3-A"
        ...
    },
    ...
    ,
    {
        ...
        "UserId":"users/283-A"
        ...
    }
    ]
    ...
}
{CODE-BLOCK/}  

You choose edges using `with edges` clause e.g.  
`with edges (Lines) { where Discount >= 0.25 select Product } as cheap`  
In the clause above we will choose the `Lines` property as our edge, since we select the `Product` property of an edge we can tell it is a nested object.  
We filter edges using a regular [where clause](../what-is-rql#where) and then we will refer to the edges using the `cheap` alias.  

### Matching
All graph quries will end with a match clause that describes the pattern of nodes and edges that needs to match inorder to return a result.  

{CODE-BLOCK:plain}
with {from Orders} as o
with edges (Lines) { where Discount >= 0.25 select Product } as cheap
with {from Products} as p
match (o)-[cheap]->(p)
{CODE-BLOCK/}

The above graph query over the `Northwind` database would return the tuples (o, p, cheap).  

- o is an order document  
- cheap is an order line with discount greater or equal 25%  
- p is the product refered to by the cheap order line  

### Implicit with clauses

There is actually no need to define the `with` and `with edges` clauses for the above query, you can write a graph query like so:  

{CODE-BLOCK:plain}
match (Orders as o)-[Lines as cheap where Discount >= 0.25 select Product]->(Products as p)
{CODE-BLOCK/}

Note that the above query yields the same result, on the server side this query will actually be translated to:  

{CODE-BLOCK:plain}
WITH {
    FROM Orders
} AS o
WITH {
    FROM Products
} AS p
WITH EDGES (Lines) {
    WHERE Discount >= 0.25
} AS cheap
MATCH (o)-[cheap]->(p)
{CODE-BLOCK/}
