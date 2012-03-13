using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Shard;

namespace RavenCodeSamples.Consumer
{
	public class Sharding
	{
		public Sharding()
		{
			#region intro
			var shards = new Dictionary<string, IDocumentStore>
			             	{
			             		{"Asia", new DocumentStore {Url = "http://localhost:8080"}},
			             		{"Middle East", new DocumentStore {Url = "http://localhost:8081"}},
			             		{"America", new DocumentStore {Url = "http://localhost:8082"}},
			             	};

			var shardStrategy = new ShardStrategy
			                    	{
			                    		ShardAccessStrategy = new ParallelShardAccessStrategy(),
			                    		ShardResolutionStrategy = new ShardResolutionByRegion(),
			                    	};

			using (var documentStore = new ShardedDocumentStore(shardStrategy, shards).Initialize())
			using (var session = documentStore.OpenSession())
			{
				//store 3 items in the 3 shards
				session.Store(new Company {Name = "Company 1", Region = "Asia"});
				session.Store(new Company {Name = "Company 2", Region = "Middle East"});
				session.Store(new Company {Name = "Company 3", Region = "America"});
				session.SaveChanges();

				//get all, should automagically retrieve from each shard
				var allCompanies = session.Query<Company>()
					.Customize(x => x.WaitForNonStaleResultsAsOfNow()).ToArray();

				foreach (var company in allCompanies)
					Console.WriteLine(company.Name);
			}
			#endregion
		}

		#region company
		public class Company
		{
			public string Id { get; set; }
			public string Name { get; set; }
			public string Region { get; set; }
		}
		#endregion

		#region IShardResolutionStrategy
		public class ShardResolutionByRegion : IShardResolutionStrategy
		{
			public string GenerateShardIdFor(object entity)
			{
				var company = entity as Company;
				if (company != null)
				{
					return company.Region;
				}
				return null;
			}

			public string MetadataShardIdFor(object entity)
			{
				// We can select one of the shards to hold the metadata entities like the HiLo document for all of the shards:
				return "Asia";

				// Or we can store the metadata on each of the shads itself, so each shards will have its own HiLo document:
				var company = entity as Company;
				if (company != null)
				{
					return company.Region;
				}
				return null;
			}

			public IList<string> PotentialShardsFor(ShardRequestData requestData)
			{
				if (requestData.EntityType == typeof (Company))
				{
					// You can try to limit the potential shards based on the query
				}
				return null;
			}
		}
		#endregion
	}
}