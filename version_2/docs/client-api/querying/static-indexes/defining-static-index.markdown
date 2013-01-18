# Defining a static index

As we have seen, defining a static index allows for more sophisticated queries and is also likely to reduce staleness for some scenarios, and this is generally preferred over relying on dynamic indexes.

To define a new index manually, you need to create an `IndexDefinition` object and pass it to the database. Once notified of the new index, the RavenDB server will execute a background indexing task to build the index. An index can be queried immediately after the indexing process has started, but until the process is finished the query results will be marked as stale. The index will be constantly updated when additions or edits occur.

## The IndexDefinition class

An index definition is composed of an index name, Map/Reduce functions, an optional TransformResults function and several other indexing options. The structure of the internal `IndexDefinition` class is shown here:

{CODE index_definition@ClientApi\Querying\StaticIndexes\DefiningStaticIndex.cs /}

###Map

Every index is required to have a name and at least one map function. The map functions are the way for us to tell RavenDB how to filter out the data we are interested in (fields we are going to be searching on). 
They are written in Linq, just like you'd write a simple query. If you index definition contains only maps then according to their definitions, values selected from documents that belongs to the specified collection 
will be stored inside a persistent Lucene index. The map function does not change any document values, just indicates which ones should be taken into account to index. This way we will be able to
perform low cost queries over the stored values. 

If you also define a reduce function (described below) then your index is going to be considered as map/reduce. Then steps of the index processing are different than for a simple index (map-only).
The map function that is executed over the set of documents, the same like previously, filters data of documents and produces an output called *mapped results*. This one is stored internally in RavenDB 
and it becomes an input for the reduce function. 

{NOTE The map functions defined in multi map index must return identical types. /}

###Reduce

The reduce function is an optional, written and executed just like the map function, but this time against the mapped results. In accordance with its name it takes the output of map functions
and reduces the values. This is a second pass of the map/reduce index processing. The reduce function aggregates the incoming data set based on a *reduce key* (given in `group by` clause)
and performs an actual operation over the groupped items. The values are calculated and then the actual results are stored into Lucene index. 

{NOTE The reduce function must be able to process the map function output as well as its own output. This is required because reduce may be applied recursively to its own output. This means is that the output of map and reduce functions must be the same type. /}

###TransformResults

The third function, `TransformResults`, is a part of a feature called Live Projections. It has no effect on values that are stored into Lucene index but when it was applied then a processing of a query will take additional step on a server. 
Before the RavenDB will send the query results to the client it will transform them according to the transform results definition. Then only the projected document values will be sent. More details about Live Projections you will find [here](live-projections).

###Index options

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

### Using `AsDocument` and `MetadataFor`

To support more complex scenarios where one wants to use document metadata or document itself in index, the `AsDocument` and `MetadataFor` methos have been introduced.

First method can be used to create an index that contains **everything** from the given document and allows one to search over all of the values of all the properties of the provided entity.

{CODE defining_static_index_1@ClientApi\Querying\StaticIndexes\DefiningStaticIndex.cs /}

{CODE defining_static_index_2@ClientApi\Querying\StaticIndexes\DefiningStaticIndex.cs /}

{NOTE Indexing all entity properties will result in larger index sizes, decreased performance and may lead to poor search relevancy, because whole document is used as an index entry. Use with caution.  /}

Second method allows us to use document metadata inside the index. For example:   

{CODE defining_static_index_3@ClientApi\Querying\StaticIndexes\DefiningStaticIndex.cs /}

{CODE defining_static_index_4@ClientApi\Querying\StaticIndexes\DefiningStaticIndex.cs /}