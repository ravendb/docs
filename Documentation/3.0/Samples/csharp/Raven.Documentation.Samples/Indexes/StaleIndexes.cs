using System;
using System.Linq;

using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Linq;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
	public class StaleIndexes
	{
		public void SimpleStaleChecks()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region stale1
					RavenQueryStatistics stats;
					var results = session.Query<Product>()
						.Statistics(out stats)
						.Where(x => x.PricePerUser > 10)
						.ToList();

					if (stats.IsStale)
					{
						// Results are known to be stale
					}
					#endregion
				}

				using (var session = store.OpenSession())
				{

					#region stale2
					RavenQueryStatistics stats;
					var results = session.Query<Product>()
						.Statistics(out stats)
						.Where(x => x.PricePerUser > 10)
						.Customize(x => x.WaitForNonStaleResultsAsOfNow(TimeSpan.FromSeconds(5)))
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region stale3
					RavenQueryStatistics stats;
					var results = session.Query<Product>()
						.Statistics(out stats)
						.Where(x => x.PricePerUser > 10)
						.Customize(x => x.WaitForNonStaleResultsAsOf(new DateTime(2014, 5, 1, 10, 0, 0, 0)))
						.ToArray();
					#endregion
				}

				#region stale4
				store.Conventions.DefaultQueryingConsistency = ConsistencyOptions.AlwaysWaitForNonStaleResultsAsOfLastWrite;
				#endregion
			}
		}
	}
}
