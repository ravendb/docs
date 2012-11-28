# HTTP API - Simple Indexes
Indexes enable advanced fast and complex queries of JSON documents already in RavenDB. Indexes are expressed using LINQ and are composed of up to two parts, a map and a reduce function.

This section of the documentation focuses on map only or simple indexes. This type of index functions much like an index in a relational database. It offers fast access to documents in RavenDB by a property or properties other then their unique id.

## Creating a Simple Index
Imagine we had a document per user in our address book and wanted to find any users who live in Maryland. We have five users, so five documents:

{CODE-START:json /}
    http://localhost:8080/docs/bob
    {
       "Name": "Bob",
       "HomeState": "Maryland",
       "ObjectType": "User"
    }
    
    http://localhost:8080/docs/sarah
    {
       "Name": "Sarah",
       "HomeState": "Illinois",
       "ObjectType": "User"
    }
    
    http://localhost:8080/docs/paul
    {
       "Name": "Paul",
       "HomeState": "Maryland",
       "ObjectType": "User"
    }
    
    http://localhost:8080/docs/mary
    {
       "Name": "Mary",
       "HomeState": "Maryland",
       "ObjectType": "User"
    }
    
    http://localhost:8080/docs/george
    {
       "Name": "George",
       "HomeState": "California",
       "ObjectType": "User"
    }
{CODE-END /}
    
If you're following along with curl:

{CODE-START:json /}    
    > curl -X PUT http://localhost:8080/docs/bob -d "{ Name: 'Bob', HomeState: 'Maryland', ObjectType: 'User' }"
    > curl -X PUT http://localhost:8080/docs/sarah -d "{ Name: 'Sarah', HomeState: 'Illinois', ObjectType: 'User' }"
    > curl -X PUT http://localhost:8080/docs/paul -d "{ Name: 'Paul', HomeState: 'Maryland', ObjectType: 'User' }"
    > curl -X PUT http://localhost:8080/docs/mary -d "{ Name: 'Mary', HomeState: 'Maryland', ObjectType: 'User' }"
    > curl -X PUT http://localhost:8080/docs/george -d "{ Name: 'George', HomeState: 'California', ObjectType: 'User' }"
{CODE-END /}

To create an index to retrieve these documents by the HomeState property, make a PUT request to \indexes\{index_id}:

{CODE-START:json /}
    > curl -X PUT http://localhost:8080/indexes/usersByHomeState 
              -d "{ Map:'from doc in docs\r\nwhere doc.ObjectType==\"User\"\r\nselect new { doc.HomeState }' }"
{CODE-END /}

On a successful index create, RavenDB will respond with a HTTP 201 Created response code, and a JSON acknowledgment of the index just created:

{CODE-START:json /}
    HTTP/1.1 201 Created

    {"index":"usersByHomeState"}
{CODE-END /}

##Querying a Simple Index

Perform a GET request to the URL of an index to retrieve all the documents in that index:

{CODE-START:plain /}
    > curl -X GET http://localhost:8080/indexes/usersByHomeState?query=HomeState%3AMaryland
{CODE-END /}

RavenDB will respond with a result set that includes all the matching records, plus some other useful information:

{CODE-START:json /}
    {"Results":
    [{"Name":"Mary","HomeState":"Maryland","ObjectType":"User",
               "@metadata":{"Content-Type":"application/x-www-form-urlencoded","@id":"mary","@etag":"25ff8144-36f8-11df-858f-001de034b773"}},
    {"Name":"Paul","HomeState":"Maryland","ObjectType":"User",
               "@metadata":{"Content-Type":"application/x-www-form-urlencoded","@id":"paul","@etag":"25ff8145-36f8-11df-858f-001de034b773"}},
    {"Name":"Bob","HomeState":"Maryland","ObjectType":"User",
               "@metadata":{"Content-Type":"application/x-www-form-urlencoded","@id":"bob","@etag":"25ff8147-36f8-11df-858f-001de034b773"}}],
     "IsStale":false,"TotalResults":3}
{CODE-END /}

In the result set, "TotalResults" is a count of the matching records.

"IsStale" is a boolean indicator of whether or not this index (and results) are up to date. When an index is first created or when new documents are added that could be part of the index, RavenDB runs a background process to update the index. If this process is running while an index query is issued, then the last known results will be returned, but clearly marked as stale with "IsStale" set to true.

## Paging Results

RavenDB allows you to control how many documents are returned from an index query by using the query string arguments "start" and "pageSize". "pageSize" specifies how many records to return, "start" is the starting position within all results to return "pageSize" records from. So with our Maryland users, we could break up the results as follows:

{CODE-START:json /}
    > curl -X GET "http://localhost:8080/indexes/usersByHomeState?query=HomeState%3AMaryland&amp;start=0&amp;pageSize=2"
    
    {"Results":
    [{"Name":"Mary","HomeState":"Maryland","ObjectType":"User",
                "@metadata":{"Content-Type":"application/x-www-form-urlencoded","@id":"mary","@etag":"25ff8144-36f8-11df-858f-001de034b773"}},
    {"Name":"Paul","HomeState":"Maryland","ObjectType":"User",
                "@metadata":{"Content-Type":"application/x-www-form-urlencoded","@id":"paul","@etag":"25ff8145-36f8-11df-858f-001de034b773"}}],
     "IsStale":false,"TotalResults":3}
    
    > curl -X GET "http://localhost:8080/indexes/usersByHomeState?query=HomeState%3AMaryland&amp;start=2&amp;pageSize=2"
    
    {"Results":
    [{"Name":"Bob","HomeState":"Maryland","ObjectType":"User",
                "@metadata":{"Content-Type":"application/x-www-form-urlencoded","@id":"bob","@etag":"25ff8147-36f8-11df-858f-001de034b773"}}],
      "IsStale":false,"TotalResults":3}
{CODE-END /}

##Deleting an Index

To delete an index, use the following syntax:

{CODE-START:plain /}
    > curl -X DELETE http://localhost:8080/indexes/usersByHomeState
{CODE-END /}

RavenDB will respond with an HTTP 204 No Content response code for a successful delete.