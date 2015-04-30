namespace Raven.Documentation.Samples.FileSystem.ClientApi
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Abstractions.FileSystem;
	using Client.FileSystem;
	using Client.FileSystem.Shard;
	using Json.Linq;

	public class Sharding
	{

		public async Task Foo()
		{
			#region sharding_1
			var shards = new Dictionary<string, IAsyncFilesCommands>
			{
				{"europe", new AsyncFilesServerClient("http://localhost:8080", "NorthwindFS")},
				{"asia", new AsyncFilesServerClient("http://localhost:8081", "NorthwindFS")},
			};

			var shardStrategy = new ShardStrategy(shards)
			{
				/*
				ShardAccessStrategy = ...
				ShardResolutionStrategy = ...
				ModifyFileName = ...
				*/
			};

			var shardedCommands = new AsyncShardedFilesServerClient(shardStrategy);
			#endregion

			#region file_operations
			string fileName = await shardedCommands.UploadAsync("test.bin", new RavenJObject()
			{
				{
					"Owner", "Admin"
				}
			}, new MemoryStream()); // will return either /europe/test.bin or /asia/test.bin name

			// you need to pass the returned file name here to let the client know on which shard the file exists
			using (var content = await shardedCommands.DownloadAsync(fileName)) 
			{
				
			}

			string renamed = await shardedCommands.RenameAsync(fileName, "new.bin");

			await shardedCommands.DeleteAsync(renamed);

			#endregion

			#region search_browse_operations
			FileHeader[] fileHeaders = await shardedCommands.BrowseAsync();

			SearchResults searchResults = await shardedCommands.SearchAsync("__fileName:test*");
			#endregion

			#region custom_shard_res_strategy_2
			var strategy = new ShardStrategy(shards);

			strategy.ShardResolutionStrategy = new RegionMetadataBasedResolutionStrategy(shards.Keys.ToList(), strategy.ModifyFileName, strategy.Conventions);

			var client = new AsyncShardedFilesServerClient(strategy);
			#endregion
		}

		#region custom_shard_res_strategy_1
		public class RegionMetadataBasedResolutionStrategy : IShardResolutionStrategy
		{
			private int counter;
			private readonly IList<string> shardIds;
			private readonly ShardStrategy.ModifyFileNameFunc modifyFileName;
			private readonly FilesConvention conventions;

			public RegionMetadataBasedResolutionStrategy(IList<string> shardIds, ShardStrategy.ModifyFileNameFunc modifyFileName, FilesConvention conventions)
			{
				this.shardIds = shardIds;
				this.modifyFileName = modifyFileName;
				this.conventions = conventions;
			}

			public ShardResolutionResult GetShardIdForUpload(string filename, RavenJObject metadata)
			{
				var shardId = GenerateShardIdFor(filename, metadata);

				return new ShardResolutionResult
				{
					ShardId = shardId,
					NewFileName = modifyFileName(conventions, shardId, filename)
				};
			}

			public string GetShardIdFromFileName(string filename)
			{
				if (filename.StartsWith("/"))
					filename = filename.TrimStart(new[] { '/' });
				var start = filename.IndexOf(conventions.IdentityPartsSeparator, StringComparison.OrdinalIgnoreCase);
				if (start == -1)
					throw new InvalidDataException("file name does not have the required file name");

				var maybeShardId = filename.Substring(0, start);

				if (shardIds.Any(x => string.Equals(maybeShardId, x, StringComparison.OrdinalIgnoreCase)))
					return maybeShardId;

				throw new InvalidDataException("could not find a shard with the id: " + maybeShardId);
			}

			public string GenerateShardIdFor(string filename, RavenJObject metadata)
			{
				// choose shard based on the region
				var region = metadata.Value<string>("Region");

				string shardId = null;

				if (string.IsNullOrEmpty(region) == false)
					shardId = shardIds.FirstOrDefault(x => x.Equals(region, StringComparison.OrdinalIgnoreCase));

				return shardId ?? shardIds[Interlocked.Increment(ref counter) % shardIds.Count];
			}

			public IList<string> PotentialShardsFor(ShardRequestData requestData)
			{
				// for future use
				throw new NotImplementedException();
			}
		}
		#endregion
	}
}