// -----------------------------------------------------------------------
//  <copyright file="ScriptedIndexResults.cs" company="Hibernating Rhinos LTD">
//      Copyright (c) Hibernating Rhinos LTD. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using System.Linq;

using Raven.Abstractions.Data;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Documentation.CodeSamples.Orders;
using Raven.Documentation.Samples.Server.Bundles.Foo1;

namespace Raven.Documentation.Samples.Server.Bundles
{
	namespace Foo1
	{
		#region index_def
		public class Orders_ByCompany : AbstractIndexCreationTask<Order, Orders_ByCompany.Result>
		{
			public class Result
			{
				public string Company { get; set; }

				public int Count { get; set; }

				public decimal Total { get; set; }
			}

			public Orders_ByCompany()
			{
				Map = orders => from order in orders
								select new
								{
									order.Company,
									Count = 1,
									Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount))
								};

				Reduce = results => from result in results
									group result by result.Company into g
									select new
									{
										Company = g.Key,
										Count = g.Sum(x => x.Count),
										Total = g.Sum(x => x.Total)
									};
			}
		}
		#endregion
	}

	namespace Foo2
	{
		#region index_def_2
		public class Orders_ByCompany : AbstractScriptedIndexCreationTask<Order, Orders_ByCompany.Result>
		{
			public class Result
			{
				public string Company { get; set; }

				public int Count { get; set; }

				public decimal Total { get; set; }
			}

			public Orders_ByCompany()
			{
				Map = orders => from order in orders
								select new
								{
									order.Company,
									Count = 1,
									Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount))
								};

				Reduce = results => from result in results
									group result by result.Company into g
									select new
									{
										Company = g.Key,
										Count = g.Sum(x => x.Count),
										Total = g.Sum(x => x.Total)
									};

				IndexScript = @"
							var company = LoadDocument(key);
							if(company == null)
									return;
							company.Orders = { Count: this.Count, Total: this.Total };
							PutDocument(this.Company, company);
						";

				DeleteScript = @"
							var company = LoadDocument(key);
							if(company == null)
									return;
							delete company.Orders;
							PutDocument(key, company);
						";
			}
		}
		#endregion
	}

	public class ScriptedIndexResults_Sample
	{
		public void Sample()
		{
			using (var store = new DocumentStore())
			{
				#region activate_bundle
				store
					.DatabaseCommands
					.GlobalAdmin
					.CreateDatabase(
						new DatabaseDocument
							{
								Id = "Northwind",
								Settings =
									{
										{ "Raven/ActiveBundles", "ScriptedIndexResults" }
									}
							});
				#endregion

				#region setup_doc
				using (var session = store.OpenSession())
				{
					session.Store(new Abstractions.Data.ScriptedIndexResults
									  {
										  Id = Abstractions.Data.ScriptedIndexResults.IdPrefix + "IndexName",
										  IndexScript = @"", // index script
										  DeleteScript = @"" // delete script body
									  });

					session.SaveChanges();
				}
				#endregion

				#region sample_setup_doc
				using (var session = store.OpenSession())
				{
					session.Store(new Abstractions.Data.ScriptedIndexResults
									  {
										  Id = Abstractions.Data.ScriptedIndexResults.IdPrefix + new Orders_ByCompany().IndexName,
										  IndexScript = @"
							var company = LoadDocument(key);
							if(company == null)
									return;
							company.Orders = { Count: this.Count, Total: this.Total };
							PutDocument(this.Company, company);
						",
										  DeleteScript = @"
							var company = LoadDocument(key);
							if(company == null)
									return;
							delete company.Orders;
							PutDocument(key, company);
						"
									  });
				}
				#endregion
			}
		}
	}

	#region activate_bundle
	public class ScriptedIndexResults
	{
		public const string IdPrefix = "Raven/ScriptedIndexResults/";

		public string Id { get; set; }
		public string IndexScript { get; set; }
		public string DeleteScript { get; set; }
	}
	#endregion
}