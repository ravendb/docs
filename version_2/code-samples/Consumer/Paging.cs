using Raven.Client;
using Raven.Client.Linq;
using System.Linq;

namespace RavenCodeSamples.Consumer
{
	class Paging : CodeSampleBase
	{
		public void SimplePaging()
		{
			using (var store = NewDocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region paging1

					// Assuming a page size of 10, this is how will retrieve the 3rd page:
					var results = session.Query<BlogPost>()
						.Skip(20) // skip 2 pages worth of posts
						.Take(10) // Take posts in the page size
						.ToArray() // execute the query
						;

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region paging2

					RavenQueryStatistics stats;
					var results = session.Query<BlogPost>()
						.Statistics(out stats)
						.Where(x => x.Category == "RavenDB")
						.Take(10)
						.ToArray();
					var totalResutls = stats.TotalResults;

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region paging3

					RavenQueryStatistics stats;

					// get the first page
					var results = session.Query<BlogPost>()
						.Statistics(out stats)
						.Skip(0 * 10) // retrieve results for the first page
						.Take(10) // page size is 10
						.Where(x => x.Category == "RavenDB")
						.Distinct()
						.ToArray();
					var totalResutls = stats.TotalResults;
					var skippedResults = stats.SkippedResults;

					// get the second page
					results = session.Query<BlogPost>()
						.Statistics(out stats)
						.Skip((1 * 10) + skippedResults) // retrieve results for the second page, taking into account skipped results
						.Take(10) // page size is 10
						.Where(x => x.Category == "RavenDB")
						.Distinct()
						.ToArray();

					// and so on...

					#endregion
				}
			}
		}
	}
}
