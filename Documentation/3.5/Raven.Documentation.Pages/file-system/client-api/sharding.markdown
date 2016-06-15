#Sharding

The file system client offers the sharding support. The basic principles remain the same like for sharding documents. If you aren't familiar with
the sharding concept, please read [this](../../server/scaling-out/sharding/how-to-setup-sharding) article first.

##Sharded client creation

Three steps are necessary in order to create the RavenFS client with the sharding support:

1. First you need to specify URLs of servers and the names of file systems that you want to shard on.
2. Next you have to create `ShardStrategy` (you can use its default behavior or overwrite such options as: `ShardAccessStrategy`, `ShardResolutionStrategy` and `ModifyFileName`).
3. Create the instance of `AsyncShardedFilesServerClient` object and pass the configured sharding strategy.

{CODE sharding_1@FileSystem\ClientApi\Sharding.cs /}

##Usage

The `AsyncShardedFilesServerClient` is the sharding equivalent of [IAsyncFilesCommands](./commands/what-are-commands) for file management and searching functionalities. 

{NOTE: File names in the sharded environment}
The sharding strategy relays on the names of shards specified during the setup. In order to properly work with files you need to take into account
that the upload operation returns the new name of a file, created according to the mentioned `ModifyFileName` function. Its default formula is:
`(convention, shardId, fileName) => convention.IdentityPartsSeparator + shardId + convention.IdentityPartsSeparator + fileName`. It
means that the file `doc.txt` stored on the shard named `europe` will obtain the following name: `/europe/doc.txt`.
{NOTE/}

{PANEL:File operations}

The below examples present the basic usage of CURD methods.

{CODE file_operations@FileSystem\ClientApi\Sharding.cs /}

{PANEL/}

{PANEL:Browsing / searching files}

The file browsing and searching looks exactly the same like for non-sharded environment. The results from the sharded file systems are merged according to the 
`ApplyAsync` method of `IShardAccessStrategy`. The default implementation is `SequentialShardAccessStrategy` that combines results in the sequential order
according to the list of shards passes to the `ShardStrategy` object.

{CODE search_browse_operations@FileSystem\ClientApi\Sharding.cs /}

{PANEL/}


##Custom IShardResolutionStrategy

The default implementation of the `IShardResolutionStrategy` alternately uploads files to shards. However you can overwrite that and
for instance use metadata to select the appropriate shard server.

###RegionMetadataBasedResolutionStrategy

The actual decision is made in `GenerateShardIdFor` method.

{CODE custom_shard_res_strategy_1@FileSystem\ClientApi\Sharding.cs /}

Use the following code for initialization with this strategy:

{CODE custom_shard_res_strategy_2@FileSystem\ClientApi\Sharding.cs /}
