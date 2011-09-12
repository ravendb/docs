# Defining a static index

To define a new index manually, you need to create an `IndexDefinition` object and pass it to the database. Once notified of the new index, the RavenDB server will execute a background indexing task to build the index. Once indexing is started (even before the indexing is completed), the index can be queried, and it will be constantly updated when additions or edits will occur.

## The IndexDefinition class

An index definition is composed of an index name, Map/Reduce functions, an optional TransformResults function and several other indexing options:

{CODE index_definition@Consumer\StaticIndexes.cs /}

Every index is required to have a name and a Map function. The Map function is the way for us to tell RavenDB how to find the data we are interested in, and what fields we are going to be searching on. The Map function is written in Linq, just like you'd write a simple query

The Reduce function is optional, and is written and executed just like the Map function, but this time on the results of the Map function. This is actually a second indexing pass, which allows us to perform aggregation operations quite cheaply, directly from the index.

{INFO To better understand the operations of the Map/Reduce functions, it is recommended that you read the Map/Reduce chapter in the Theory section. /}

The third function, `TransformResults`, is of a feature called Live Projections, which is discussed later in this chapter.

The remaining properties are useful for leveraging the full power of Lucene by customizing the indexes even further. We will discuss their use in depth later in this chapter.

## Creating a new index

Once we figured out how our index should look like, we can go ahead and send the index definition we built to the server, so the indexing process can start. The most straight-forward way to do so is through the `PutIndex` function, available from the `DatabaseCommands` object:

{CODE static_indexes2@Consumer\StaticIndexes.cs /}

{INFO The `DatabaseCommands` object is available from both the session object _and_ the `IDocumentStore` object. /}

An alternative approach is to create a class which is derived from `AbstractIndexCreationTask<T>`, and populating the required fields in its constructor. This is very useful when you have many indexes to maintain, so you can keep each index in its own class, and pass them all in one line of code (assuming they all reside in the same assembly), like so:

    Raven.Client.Indexes.IndexCreation.CreateIndexes(typeof(MyIndexClass).Assembly, documentStore);

The recommended way of creating static indexes is by using `AbstractIndexCreationTask<T>`.

## Putting indexes into practice

Let's assume we have a blog with many posts, each filed under several tags, and now we want to know how many posts each tag has. Since creating indexes from code through the client API requires having strongly typed objects, we will need to create a helper entity for the Reduce operation:

{CODE blogpost_mapreduce_classes@Common.cs /}

Then we can create a new Map/Reduce index using the following code:

{CODE static_indexes3@Consumer\StaticIndexes.cs /}

{NOTE Notice the use the generic `IndexDefinitionBuilder` class. It builds an `IndexDefinition` object for you based on the Linq queries you specified. If needed you can pass an `IndexDefinition` object with the Map/Reduce functions as strings. /}

Alternatively, we can use the exact same logic in the form of `AbstractIndexCreationTask<T>`:

{CODE static_indexes4@Consumer\StaticIndexes.cs /}

And trigger its execution on startup by calling:

{CODE static_indexes5@Consumer\StaticIndexes.cs /}

Either way, querying the indexes is the same, using the index name. Here's how we would go about finding the count of posts tagged under the "RavenDB" tag:

{CODE static_indexes6@Consumer\StaticIndexes.cs /}