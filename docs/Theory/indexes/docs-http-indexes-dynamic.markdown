# Dynamic Indexes

Rather than define indexes up front, RavenDB can analyse a query and create a temporary index on the fly, which will be persisted for some amount of time before then being disposed of.

Further uses of similar queries (using the same parameters) will result in the temporary index being re-used, and if used enough in a given period of time the temporary index will be promoted and made permanent.

All temporary indexes except those which have been made permanent will be cleared on start-up.

{NOTE When a dynamic index is first created, results are likely to be stale, subsequent calls can be used to alleviate this, but this may result in sup optimal application performance. /}

The following examples assume the following document structure is in place:

{CODE-START:json /}
    {
        Id: "blogs/1"
        Title: "one",
        Content: "I like ravens",
        Category: "Ravens",
        Tags:
        {
            Name: "birds"
        }
    }
    
    {
        Id: "blogs/2"
        Title: "two",
        Content: "",
        Category: "Ravens",
        Tags:
        {
           Name: "birds"
        }
    }
    
    {
        Id: "blogs/2"
        Title: "two",
        Content: "I like rhinos",
        Category: "Rhinos",
        Tags:
        {
           Name: "mammals"
        }
    }
{CODE-END /}

##Querying simple properties

We can perform a query for all the documents with a category of “Ravens” by making a call to the ordinary indexes endpoint like so:

{CODE-START:plain /}
    > curl -X GET http://localhost:8080/indexes/dynamic?query=Category:Ravens
{CODE-END /}

This call returns the same structure as ordinary indexes, and accepts all of the usual arguments.

Multiple properties can be queried like so:

{CODE-START:plain /}
    > curl -X GET http://localhost:8080/indexes/dynamic?query=Category:Ravens AND Title:one
{CODE-END /}

##Querying simple properties

Unlike when querying an index which is flattened, dynamic indexes take in the full path to the properties being compared

{CODE-START:plain /}
    > curl -X GET http://localhost:8080/indexes/dynamic?query=Content.Length:[00000000 TO NULL]
{CODE-END /}

##Querying collection properties

Special syntax is used to query collections, instead of a period to denote property access, a comma is used to indicate an array is being accessed.

{CODE-START:plain /}
    > curl -X GET http://localhost:8080/indexes/dynamic?query=Tags,Name:mammals
{CODE-END /}

This is equivalent to “find all the blogs with the tag of mammals.