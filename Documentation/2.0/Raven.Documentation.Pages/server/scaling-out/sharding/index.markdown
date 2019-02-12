# Sharding

RavenDB has native sharding support. [Sharding](https://en.wikipedia.org/wiki/Shard_(database_architecture)) is a way to split your data across servers, so each server hold just a portion of your data. This is required in situations where you need to handle a lot of data.

Let's say in our application we have to handle data from a lot of companies across the globe. One choice would be to store a company's data on a shard which depends on the company's region. For example, the companies located in Asia will be stored on one shard, the companies located in the Middle East will be stored on a different shard and the companies from America would be stored on a third shard.

The idea above is to geo locate the shards near the location where the data is used, so companies in Asia get served from a nearby server and respond more quickly to a user located in Asia. We are also able to reduce the load on each server, because it only handle some part of the data.

RavenDB contains built-in support for sharding. It'll handle all aspects of sharding for you, leaving you with the sole task of defining the shard function (how to actually split the documents among multiple shards).

Here are some entities that can be split to different shards based on their Region: the `Company` and `Invoice` entities.

{CODE entities@Server\ScalingOut\Sharding\Index.cs /}

In order to achieve this, you need to use the ShardedDocumentStore, instead of the usual DocumentStore. Except for the initialization phase, it behaves just like the standard DocumentStore, and you have access to all of the usual API and features of RavenDB.

In order to create a ShardedDocumentStore you should supply an instance of ShardStrategy which in turn takes a dictionary with the shards to operate on. The keys and values in the dictionary are the shard ID and a DocumentStore instance that points to that shard.

The ShardStrategy tells the ShardedDocumentStore how to interact with the shards. This allows you to customize different aspects of the sharding behavior, giving you the option of fine grained control over how RavenDB handles your data:

* ShardAccessStrategy: an instance that implements the `IShardAccessStrategy` interface, which decides how to contact them. There are already built-in implementations of this interface which are `SequentialShardAccessStrategy` and `ParallelShardAccessStrategy` which let you access the shards in sequential or parallel manner respectively. The default value for this is `SequentialShardAccessStrategy`.
* ShardResolutionStrategy: an instance that implements the `IShardResolutionStrategy` interface, which decides which shards should be contacted in order to complete a database operation. The default implementation for this is the `DefaultShardResolutionStrategy` class, which let you to start with sharding fast without the need to implement `IShardResolutionStrategy` yourself.
* MergeQueryResults: a delegate that lets you decide how to merge query results from a few shards. There is a default implementation for this which merges the result as they come back and apply minimal sorting behavior.
* ModifyDocumentId: lets you store the shard id for a document in the document itself. The default implementation is: `(convention, shardId, documentId) => shardId + convention.IdentityPartsSeparator + documentId`.

So in order to use sharding, you can just use the `ShardStrategy` with its default behavior:

{CODE store@Server\ScalingOut\Sharding\Index.cs /}

For complex sharding environments you would probably want to implement the `IShardResolutionStrategy` yourself and set the `ShardResolutionStrategy` property in the `ShardStrategy` to hold your custom implementation instead of the `DefaultShardResolutionStrategy`. The `IShardResolutionStrategy` has the following methods that need to be implemented:

- GenerateShardIdFor: here you can decide which shard should be used in order to store a specific entity.
- MetadataShardIdFor: here you can decide which shard should be used in order to store the metadata documents (like the HiLo documents) for a specific entity.
- PotentialShardsFor: here you can decide which shards should be contacted in order to complete a query operation. You can decide this based on the available parameters which can be the DocumentKey, EntityType and the Query.

By default, if you don't set the `ShardResolutionStrategy` property on the `ShardStrategy` you'll use the `DefaultShardResolutionStrategy`. 

##Default Shard Resolution Strategy

If you're using the `DefaultShardResolutionStrategy` you can just use the `ShardingOn` method on the `ShardStrategy` object, in order to instruct the `DefaultShardResolutionStrategy` which property holds the shards id information for a specific entity.   

{NOTE If used, `DefaultShardResolutionStrategy` `ShardingOn` implementation enforces user to setup it on all entities that are getting stored. /}

{NOTE If `ShardingOn` is not used then `DefaultShardResolutionStrategy` will assign all entities within same session to the same shard. /}

In the code below you can see that the `Company` holds the shard ID in the Region property and the `Invoice` holds the shard id in the CompanyId property. The CompanyId holds the shard id because of the `ModifyDocumentId` convention of the `ShardStrategy`.

{CODE sharding_1@Server\ScalingOut\Sharding\Index.cs /}

Now we can store some data which will be split across different shards:

{CODE SaveEntities@Server\ScalingOut\Sharding\Index.cs /}

In the above example we're storing each of the companies on a different shard, and each of the invoices in the same shard of their company's shard.

Now we can do operation like `Query`, `Load` or `LuceneQuery`, and the actual shards that will be contacted in order to complete that operation will depend on the `IShardResolutionStrategy` implementation.

{CODE Query@Server\ScalingOut\Sharding\Index.cs /}

If you're using the `DefaultShardResolutionStrategy` than in this case it'll make a request just to the "Asia" shard.

In summary, sharding is a great feature to have and with RavenDB you get this supported natively and easily.