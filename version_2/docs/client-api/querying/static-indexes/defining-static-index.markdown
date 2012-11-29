# Defining a static index

As we have seen, defining a static index allows for more sophisticated queries and is also likely to reduce staleness for some scenarios, and this is generally preferred over relying on dynamic indexes.

To define a new index manually, you need to create an `IndexDefinition` object and pass it to the database. Once notified of the new index, the RavenDB server will execute a background indexing task to build the index. An index can be queried immediately after the indexing process has started, but until the process is finished the query results will be marked as stale. The index will be constantly updated when additions or edits occur.

## The IndexDefinition class

An index definition is composed of an index name, Map/Reduce functions, an optional TransformResults function and several other indexing options. The structure of the internal `IndexDefinition` class is shown here:

{CODE index_definition@ClientApi\Querying\StaticIndexes\DefiningStaticIndex.cs /}

Every index is required to have a name and a Map function. The Map function is the way for us to tell RavenDB how to find the data we are interested in, and what fields we are going to be searching on. The Map function is written in Linq, just like you'd write a simple query

The Reduce function is optional, and is written and executed just like the Map function, but this time on the results of the Map function. This is actually a second indexing pass, which allows us to perform aggregation operations quite cheaply, directly from the index.

{INFO To better understand the operations of the Map/Reduce functions, it is recommended that you read the Map/Reduce chapter in the Theory section. /}

The third function, `TransformResults`, is of a feature called Live Projections, which is discussed later in this chapter.

The remaining properties are useful for leveraging the full power of Lucene by customizing the indexes even further. We will discuss them in depth later in this chapter.

## Creating a new index

Once we figured out how our index should look like, we can go ahead and tell the server to create it, so the indexing process can start. The most straight-forward way to do that is through the `PutIndex` function, available from the `DatabaseCommands` object:

{CODE static_indexes2@ClientApi\Querying\StaticIndexes\DefiningStaticIndex.cs /}

{NOTE Note the use the generic `IndexDefinitionBuilder` class. It builds an `IndexDefinition` object for you based on the Linq queries you specified. If needed you can pass an `IndexDefinition` object with the Map/Reduce functions as strings. /}

A cleaner way to do this, which is also the recommended one, is to create an index class inheriting  from `AbstractIndexCreationTask<T>`, and name it after the indexing operation it does. In the constructor of that class you have access to all the the index properties, which you can change to match your.

Telling the server to create the actual index is done by adding the following call on application startup. This one liner will submit all `AbstractIndexCreationTask<T>` classes for creation as indexes on the server (existing indexes will remain untouched):

{CODE static_indexes5@ClientApi\Querying\StaticIndexes\DefiningStaticIndex.cs /}

With this approach each index can be in its own file, what makes it much easier to work with in case you have a lot of them.

## Putting indexes into practice

Let's assume we have a blog with many posts, each filed under several tags, and we wish to know how many posts are under each of the tags. One way to do this would be as follows:

{CODE static_indexes3@ClientApi\Querying\StaticIndexes\DefiningStaticIndex.cs /}

Where `BlogTagPostsCount` is declared like this:

{CODE blogpost_mapreduce_classes@Common.cs /}

A better way of doing this is by creating an index class, and using `IndexCreation.CreateIndexes` to submit it to the server:

{CODE static_indexes4@ClientApi\Querying\StaticIndexes\DefiningStaticIndex.cs /}

Either way, querying the indexes is the same. You can either let RavenDB decide which index to use, or instruct it to use a specific index by explicitly specifying the index name while querying. Here's how we would go about finding the count of posts tagged under the "RavenDB" tag:

{CODE static_indexes6@ClientApi\Querying\StaticIndexes\DefiningStaticIndex.cs /}