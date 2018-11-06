# Querying : Graph Queries

Graph queries are a new type of queries that allows you to query your documents as if they were a graph.
The nodes which we query are your regular documents and the edges are just plain properties of your documents.

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
    User: Users/1-A
    ...
}
{CODE-BLOCK/}  
- Properties that are nested objects that have a property that is a document key, the nested object is considered an edge in this case.  
{CODE-BLOCK:plain}
{
    ...
    User: 
    {
        ...
        UserId:Users/1-A
        ...
    }
    ...
}
{CODE-BLOCK/} 
- Properties that are an array of document keys, each key is a diffrent edge in this case.  
{CODE-BLOCK:plain}
{
    ...
    Users: [Users/1-A, Users/3-A, ...,Users/283-A]
    ...
}
{CODE-BLOCK/}  
- Properties that are an array of nested objects that contains a property that is a document key, each object is a diffrent edge in this case.  
{CODE-BLOCK:plain}
{
    ...
    Users:  [ 
    {
        ...
        UserId:Users/1-A
        ...
    },
    {
        ...
        UserId:Users/3-A
        ...
    },
    ...
    ,
    {
        ...
        UserId:Users/283-A
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
