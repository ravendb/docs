// -----------------------------------------------------------------------
//  <copyright file="DynamicReporting.cs" company="Hibernating Rhinos LTD">
//      Copyright (c) Hibernating Rhinos LTD. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using Raven.Client;
using System.Linq;

namespace RavenCodeSamples.Studio
{
	public class DynamicReporting : CodeSampleBase
	{
		class Order
		{
			public string Company { get; set; }
			public string Employee { get; set; }
			public double Total { get; set; }
		}

		public DynamicReporting()
		{
			using (var store = NewDocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region query
					session.Query<Order>("Orders/Total")
					  .Where(x => x.Company == "companies/1")
					  .AggregateBy(x => x.Employee)
					  .SumOn(x => x.Total)
					  .ToList();
					#endregion
				}
			}
		}
	}
}