using System;
using System.Linq;
using Raven.Client.Linq;

namespace RavenCodeSamples.Consumer
{
	public class Stale : CodeSampleBase
	{
		public void SimpleStaleChecks()
		{
			using (var documentStore = NewDocumentStore())
			{
				using (var session = documentStore.OpenSession())
				{
					#region stale1

					RavenQueryStatistics stats;
					var results = session.Query<Product>()
						.Statistics(out stats)
						.Where(x => x.Price > 10)
						.ToArray();

					if (stats.IsStale)
					{
						// Results are known to be stale
					}

					#endregion
				}

				using (var session = documentStore.OpenSession())
				{

					#region stale2

					RavenQueryStatistics stats;
					var results = session.Query<Product>()
						.Statistics(out stats)
						.Where(x => x.Price > 10)
						.Customize(x => x.WaitForNonStaleResults(TimeSpan.FromSeconds(5)))
						.ToArray();

					#endregion
				}

				using (var session = documentStore.OpenSession())
				{

					#region stale3

					RavenQueryStatistics stats;
					var results = session.Query<Product>()
						.Statistics(out stats)
						.Where(x => x.Price > 10)
						.Customize(x => x.WaitForNonStaleResultsAsOf(new DateTime(2011, 5, 1, 10, 0, 0, 0)))
						.ToArray();

					#endregion
				}
			}
		}
	}
}
