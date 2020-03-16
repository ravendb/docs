# Basic query support in RavenDB

Once data has been stored in RavenDB, we have seen how it can be retrieved by id, updated and deleted. The next useful operation is the ability to query based on some aspect of the documents that have been stored. 

For example, we might wish to ask for all the blog entries that belong to a certain category like so:

{CODE basic_querying_1@Intro\BasicOperations.cs /}

Or, using a different syntax, to find all blog posts that have at least 10 comments:

{CODE basic_querying_2@Intro\BasicOperations.cs /}

That Just Works(tm) and gives us all the blog posts matching the criteria we have specified.

As usual, let's have a look at the HTTP communication for the first operation:

	GET /indexes/dynamic/BlogPosts?query=Category:RavenDB&start=0&pageSize=128 HTTP/1.1
	Accept-Encoding: deflate,gzip
	Content-Type: application/json; charset=utf-8
	Host: 127.0.0.1:8081

{NOTE Notice that a page size of 128 was passed along, although none was specified. This is RavenDB's "Safe by default" feature kicking in /}

The important part to notice in this query is that we are querying the "BlogPosts" collection, for the property "Category" with the value of "RavenDB".

When we query RavenDB we first need to specify which collection we are interested on querying. Once we did that (in the first code snippet above - by specifying `session.Query<`**BlogPost**`>`), we can use the various available modifiers and clauses to filter the results as much as we'd like.

RavenDB supports many querying types, ranging from simple value comparisons to geo-spatial queries and even full blown full-text search queries.

In the next chapters we will be discussing querying in more depth - from syntax, through all available querying options, to how RavenDB actually allows efficient data querying, and how you take all that to your advantage when designing your own complex data structure.