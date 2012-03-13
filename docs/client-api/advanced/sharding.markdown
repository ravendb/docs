# Client side sharding

RavenDB has a native sharding support. [Sharding](http://en.wikipedia.org/wiki/Shard_(database_architecture)) is a way to split your data across servers, so each server hold just a portion of your data. This is required in situations that you have a lot of data and you prefer to handle.

Let's say that we have to handle in our applications a lot of companies across the globe. A good chose will be to store the companies on dedicated shard which would be depends on the company's region. For example, the companies in Asia will be stored on one shard, the companies in the Middle East will be stored on a different dedicated shard and the companies from America would be stored on a third shard.

Here is the `company` entity, which has the Region peroperty.

{CODE company@Consumer\Sharding.cs /}

In order to achieve this, you need to use the ShardedDocumentStore which is implements the IDocumentStore interface and it's API looks like the DocumentStore one. 

In order to create a ShardedDocumentStore you should supply two parameters: a instance of ShardStrategy and a dictionary with the shards to operate on. The keys and values in the dictionary are the shard ID and a DocumentStore instance that point out to that shard.

The ShardStrategy tells to the ShardedDocumentStore how to interact with the shards. You the following properties there:

- ShardResolutionStrategy: an instance that implements the `IShardResolutionStrategy` interface, which decides which shards should be contact in order to complete an database operation.
- ShardAccessStrategy: an instance that implements the `IShardAccessStrategy` interface, which decides how to contact them. There is already built-in implementation of this interface which are `SequentialShardAccessStrategy` and `ParallelShardAccessStrategy` which let you access the shards in sequential or parallel manner respectively.
- MergeQueryResults: a delegate that let you decide how to marge query result from a few shards. There is a default implementation for this which just merges the result as they came back with not a particular order.

So in order to use sharding, you must at least have a custom implementation of `IShardResolutionStrategy` which has the following methods:

- GenerateShardIdFor: here you can decide which shard should be used in order to store a specific entity.
- MetadataShardIdFor: here you can decide which shard should be used in order to store the metadata documents (like the HiLo documents) for a specific entity.
- PotentialShardsFor: here you can decide which shards should be contact in order to complete a query operation. You can decide this based on the available parameters which can be the DocumentKey, EntityType and the Query.
 
Here is an example how to use the ShardedDocumentStore:

{CODE intro@Consumer\Sharding.cs /}

In the above example we're storing each of the companies on a different dedicated shard, and then querying the companies from all of the shards.

Let see the implementation of `IShardResolutionStrategy`:

{CODE IShardResolutionStrategy@Consumer\Sharding.cs /}

Sharding is a great feature to have and with RavenDB you get this supported natively.