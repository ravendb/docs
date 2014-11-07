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
			#region store
			var shards = new Dictionary<string, IDocumentStore>
			             	{
			             		{"Asia", new DocumentStore {Url = "http://localhost:8080"}},
			             		{"Middle East", new DocumentStore {Url = "http://localhost:8081"}},
			             		{"America", new DocumentStore {Url = "http://localhost:8082"}},
			             	};

			var shardStrategy = new ShardStrategy(shards)
				.ShardingOn<Company>(company => company.Region)
				.ShardingOn<Invoice>(x => x.CompanyId);

			var documentStore = new ShardedDocumentStore(shardStrategy).Initialize();
			#endregion

			#region SaveEntities
			using (var session = documentStore.OpenSession())
			{
				var asian = new Company { Name = "Company 1", Region = "Asia" };
				session.Store(asian);
				var middleEastern = new Company { Name = "Company 2", Region = "Middle-East" };
				session.Store(middleEastern);
				var american = new Company { Name = "Company 3", Region = "America" };
				session.Store(american);

				session.Store(new Invoice { CompanyId = american.Id, Amount = 3, IssuedAt = DateTime.Today.AddDays(-1) });
				session.Store(new Invoice { CompanyId = asian.Id, Amount = 5, IssuedAt = DateTime.Today.AddDays(-1) });
				session.Store(new Invoice { CompanyId = middleEastern.Id, Amount = 12, IssuedAt = DateTime.Today });
				session.SaveChanges();
			}
			#endregion

			#region Query
			using (var session = documentStore.OpenSession())
			{
				//get all, should automagically retrieve from each shard
				var allCompanies = session.Query<Company>()
					.Customize(x => x.WaitForNonStaleResultsAsOfNow())
					.Where(company => company.Region == "Asia")
					.ToArray();

				foreach (var company in allCompanies)
					Console.WriteLine(company.Name);
			}
			#endregion

			documentStore.Dispose();
		}

		#region entities
		public class Company
		{
			public string Id { get; set; }
			public string Name { get; set; }
			public string Region { get; set; }
		}

		public class Invoice
		{
			public string Id { get; set; }
			public string CompanyId { get; set; }
			public decimal Amount { get; set; }
			public DateTime IssuedAt { get; set; }
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