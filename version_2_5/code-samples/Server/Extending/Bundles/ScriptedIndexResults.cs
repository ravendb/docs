// -----------------------------------------------------------------------
//  <copyright file="ScriptedIndexResults.cs" company="Hibernating Rhinos LTD">
//      Copyright (c) Hibernating Rhinos LTD. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Raven.Abstractions.Data;
using Raven.Client.Extensions;
using Raven.Client.Indexes;

namespace RavenCodeSamples.Server.Extending.Bundles
{
	public class Order
	{
		public Company Company { get; set; }
		public LineItem[] Lines { get; set; }
	}

	public class OrderResult
	{
		public string Company { get; set; }
		public int Count { get; set; }
		public decimal Total { get; set; }
	}

	#region index_def
	public class OrdersByCompany : AbstractIndexCreationTask<Order, OrderResult>
	{
		public OrdersByCompany()
		{
			Map = orders => from order in orders
			                select new
				                {
					                order.Company, 
									Count = 1, 
									Total = order.Lines.Sum(l => (l.Quantity * l.Price))
				                };

			Reduce = results => from result in results
			                    group result by result.Company
			                    into g
			                    select new
				                    {
					                    Company = g.Key,
					                    Count = g.Sum(x => x.Count),
					                    Total = g.Sum(x => x.Total)
				                    };
		}
	}
	#endregion

	public class ScriptedIndexResults_Sample : CodeSampleBase
	{
		 public void Sample()
		 {
			 using (var store = NewDocumentStore())
			 {
				 #region activate_bundle
				 store.DatabaseCommands.CreateDatabase(new DatabaseDocument
				 {
					 Id = "Northwind",
					 Settings =
							 {
								 {"Raven/ActiveBundles", "ScriptedIndexResults"}
							 }
				 });
				 #endregion

				 #region setup_doc
				 using (var session = store.OpenSession())
				 {
					 session.Store(new ScriptedIndexResults
					 {
						 Id = ScriptedIndexResults.IdPrefix + "IndexName",
						 IndexScript = @"", // index script
						 DeleteScript = @"" // delete script body
					 });

					 session.SaveChanges();
				 }
				 #endregion

				 #region sample_setup_doc
				 using (var session = store.OpenSession())
				 {
					session.Store(new ScriptedIndexResults
					{
						Id = ScriptedIndexResults.IdPrefix + new OrdersByCompany().IndexName,
						IndexScript = @"
							var company = LoadDocument(this.Company);
							if(company == null)
									return;
							company.Orders = { Count: this.Count, Total: this.Total };
							PutDocument(this.Company, company);
						",
						DeleteScript = @"
							var company = LoadDocument(this.Company);
							if(company == null)
									return;
							delete company.Orders;
							PutDocument(this.Company, company);
						"
					});
				 }
				 #endregion
			 }
		 }
	}
}

namespace Sample
{
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