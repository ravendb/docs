# Client side sharding

RavenDB has a native sharding support. [Sharding](http://en.wikipedia.org/wiki/Shard_(database_architecture)) is a way to split your data across servers, so each server hold just a portion of your data. This is required in situations that you have a lot of data and you prefer to handle.

Let's say that we have to handle in our applications a lot of companies across the globe. A good chose will be to store the companies on dedicated shard which would be depends on the company's region. For example, the companies in Asia will be stored on one shard, the companies in the Middle East will be stored on a different dedicated shard and the companies from America would be stored on a third shard.

We geo locate the shards near the location where they are used, so companies in Asia get served from a nearby server and respond more quickly to user. We are also able to reduce the load on each server, because it only handle some part of the data.

RavenDB contains builtin support for sharding. It'll handle all aspects of sharding for you, leaving you with the sole task of defining the shard function (how to actually split the documents among multiple shards).

Here are some entities that can be splitted to a different shards based on their Region: the `Company` and `Invoice` entities.

{CODE entities@Consumer\Sharding.cs /}

In order to achieve this, you need to use the ShardedDocumentStore, instead of the usual DocumentStore. Except for the initialization phase, it behaves just like the standard DocumentStore, and you have access to all of the usual API and features of RavenDB.

In order to create a ShardedDocumentStore you should supply an instance of ShardStrategy which in turn takes a dictionary with the shards to operate on. The keys and values in the dictionary are the shard ID and a DocumentStore instance that point out to that shard.

The ShardStrategy tells to the ShardedDocumentStore how to interact with the shards. This allows you to customize different aspects of the sharding behavior, giving you the option of fine grained control over how RavenDB handles your data:

* ShardAccessStrategy: an instance that implements the `IShardAccessStrategy` interface, which decides how to contact them. There are already built-in implementations of this interface which are `SequentialShardAccessStrategy` and `ParallelShardAccessStrategy` which let you access the shards in sequential or parallel manner respectively. The default value for this is `SequentialShardAccessStrategy`.
* ShardResolutionStrategy: an instance that implements the `IShardResolutionStrategy` interface, which decides which shards should be contact in order to complete an database operation. The default implementation for this is the `DefaultShardResolutionStrategy` class, which let you to start with sharding fast without the need to implement `IShardResolutionStrategy` by yourself.
* MergeQueryResults: a delegate that let you decide how to marge query result from a few shards. There is a default implementation for this which merges the result as they came back and apply minimal sorting behavior.
* ModifyDocumentId: let you store the shard id for a document in the document it. The default implementation is: `(convention, shardId, documentId) => shardId + convention.IdentityPartsSeparator + documentId`.

So in order to use sharding, you can just use the `ShardStrategy` with its default behaviour:

{CODE store@Consumer\Sharding.cs /}

For a complex sharding environments you would probably want to implement the `IShardResolutionStrategy` yourself and set the `ShardResolutionStrategy` property in the `ShardStrategy` to hold your custome implementation instead of the `DefaultShardResolutionStrategy`. The `IShardResolutionStrategy` has the following methods that need to be implemented:

- GenerateShardIdFor: here you can decide which shard should be used in order to store a specific entity.
- MetadataShardIdFor: here you can decide which shard should be used in order to store the metadata documents (like the HiLo documents) for a specific entity.
- PotentialShardsFor: here you can decide which shards should be contacted in order to complete a query operation. You can decide this based on the available parameters which can be the DocumentKey, EntityType and the Query.

By default, if you don't set the `ShardResolutionStrategy` property on the `ShardStrategy` you'll use the `DefaultShardResolutionStrategy`. 

If you're using the `DefaultShardResolutionStrategy` you can just use the `ShardingOn` method on the `ShardStrategy` object, in order to instruct the `DefaultShardResolutionStrategy` what is property holds the shards id information for a specify entity.
In the above code you can see that the `company` holds the shard ID in the Region property and the `invoce` holds the shard id in the CompanyId property. The CompanyId holds the shard id because of the `ModifyDocumentId` convention of the `ShardStrategy`.

Now we can store some data which will be splitted across different shards:

{CODE SaveEntities@Consumer\Sharding.cs /}

In the above example we're storing each of the companies on a different shard, and each of the invoices in the same shard of their dompany's shard.

Now we can do operation like `Query`, `Load` or `LuceneQuery`, and the actuall shards that will be contacted in order to complete that opertion will be depnad on the `IShardResolutionStrategy` implementation.

{CODE Query@Consumer\Sharding.cs /}

If you're using the `DefaultShardResolutionStrategy` than in this case it'll make a request just to the "Asia" shard.

In summary, sharding is a great feature to have and with RavenDB you get this supported natively and easily.