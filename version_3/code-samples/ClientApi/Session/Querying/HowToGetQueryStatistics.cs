using System.Linq;

using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Linq;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.CodeSamples.ClientApi.Session.Querying
{
	public class HowToGetQueryStatistics
	{
		private interface IFoo<TResult>
		{
			#region stats_1
			IRavenQueryable<TResult> Statistics(out RavenQueryStatistics stats);
			#endregion
		}

		public HowToGetQueryStatistics()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region stats_2
					RavenQueryStatistics stats;
					var people = session.Query<Employee>()
						.Where(x => x.FirstName == "Robert")
						.Statistics(out stats)
						.ToList();

					var totalResults = stats.TotalResults;
					var durationMilliseconds = stats.DurationMilliseconds;
					#endregion
				}
			}
		}
	}
}