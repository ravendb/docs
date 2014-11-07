# Dynamic Indexes

Rather than define indexes up front, RavenDB can analyze a query and create an auto index on the fly. Further uses of similar queries (using the same parameters) will result in the auto index being re-used.

Automatic indexes have their age, and that is tracked internally by RavenDB. If an automatic index isn't being used, it will become idle and eventually abandoned (see [index priorities](../../server/administration/index-administration#index-prioritization)). If it is a very young index, we will decide it was a temporary index after all, and remove it from the system completely.


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
&nbsp;
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
&nbsp;
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

## Querying simple properties

We can perform a query for all the documents with a category of “Ravens” by making a call to the ordinary indexes endpoint like so:

{CODE-START:plain /}
    curl -X GET http://localhost:8080/indexes/dynamic?query=Category:Ravens
{CODE-END /}

This call returns the same structure as ordinary indexes, and accepts all of the usual arguments.

Multiple properties can be queried like so:

{CODE-START:plain /}
    curl -X GET http://localhost:8080/indexes/dynamic?query=Category:Ravens AND Title:one
{CODE-END /}

## Querying simple properties

Unlike when querying an index which is flattened, dynamic indexes take in the full path to the properties being compared

{CODE-START:plain /}
    curl -X GET http://localhost:8080/indexes/dynamic?query=Content.Length:[00000000 TO NULL]
{CODE-END /}

##Querying collection properties

Special syntax is used to query collections, instead of a period to denote property access, a comma is used to indicate an array is being accessed.

{CODE-START:plain /}
    curl -X GET http://localhost:8080/indexes/dynamic?query=Tags,Name:mammals
{CODE-END /}

This is equivalent to "find all the blogs with the tag of mammals".

## More querying syntax

Let's assume the following document is stored in RavenDB:

{CODE-START:json /}
{
    "Title": "RavenDB Indexing",
    "Author": { "Id": "users/ayende", "Name": "Ayende" },
    "Tags": ["Indexing", "AdHoc"],
    "Images": [
        { "Url": "/static/ayende-on-beach.jpg", "Title": "Ayende's on the Beach" },
        { "Url": "/static/arava.jpg", "Title": "Arava with a bone" },
    ]
}
{CODE-END /}

We will show the appropriate Linq query and the actual Lucene query generated for each example:

    // Simple property - linq
    from doc in docs
    where doc.Title == "RavenDB Indexing"
    select doc;

    // Simple property - Lucene
    Title:"RavenDB Indexing"

    // Nested property - linq
    from doc in docs
    where doc.Author.Name == "Ayende"
    select doc;

    // Nested property - lucene
    Author.Name:Ayende

    // Primitive list - linq
    from doc in docs
    where doc.Tags.Any( tag => tag == "Indexing")
    select doc;

    // Primitive list - lucene

    Tags,:Indexing

    // List of objects - linq
    from doc in docs
    where doc.Images.Any( img => img.Url == "/static/arava.jpg")
    select doc;

    // List of objects - lucene

    Images,Url:"/static/arava.jpg"

For querying into nested objects, we use the dot operator, just like in C#. For querying into collections, we use the comma operator.
