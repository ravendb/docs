using System.Linq;
using Raven.Client;
using Raven.Client.Linq;
using RavenDBSamples.BaseForSamples;

namespace RavenDBSamples.BasicOperations
{
	public class BasicQueryOperations : SampleBase
	{
		public void QuarySample()
		{
			using (var session = DocumentStore.OpenSession())
			{
				//Get all blog posts from a category
				var results1 = (
					               from blog in session.Query<BlogPost>()
					               where blog.Category == "RavenDB"
					               select blog
				               ).ToList();

				//Get all blog posts that has at least 10 comments
				var results2 = session.Query<BlogPost>()
				                      .Where(x => x.Comments.Length >= 10)
				                      .ToList();

				//Get all the Ids of blog posts from a category with projection
				var results3 = from blog in session.Query<BlogPost>()
								   where blog.Category == "RavenDB"
								   select new { blog.Id }; // This is the projection

				//Get all blog posts that has at least 10 comments sorted by number of comments
				var results4 = session.Query<BlogPost>()
									  .Where(x => x.Comments.Length >= 10)
									  .OrderBy(post => post.Comments.Count())
									  .ToList();

				//Get all blog posts that has at least 10 comments sorted by number of comments decending
				var results5 = session.Query<BlogPost>()
									  .Where(x => x.Comments.Length >= 10)
									  .OrderByDescending(post => post.Comments.Count())
									  .ToList();
			}
		}

		public void Paging()
		{
			using (var session = DocumentStore.OpenSession())
			{
				// Assuming a page size of 10, this is how will retrieve the 3rd page:
				var results = session.Query<BlogPost>()
				                     .Skip(20) // skip 2 pages worth of posts
				                     .Take(10) // Take posts in the page size
				                     .ToList(); // execute the query
			}

			//Paging with regards to skipped results
			using (var session = DocumentStore.OpenSession())
			{
				RavenQueryStatistics stats;

				// get the first page
				var results = session.Query<BlogPost>()
					.Statistics(out stats)
					.Skip(0 * 10) // retrieve results for the first page
					.Take(10) // page size is 10
					.Where(x => x.Category == "RavenDB")
					.Distinct()
					.ToList();
				var totalResutls = stats.TotalResults;
				var skippedResults = stats.SkippedResults;

				// get the second page
				results = session.Query<BlogPost>()
					.Statistics(out stats)
					.Skip((1 * 10) + skippedResults) // retrieve results for the second page, taking into account skipped results
					.Take(10) // page size is 10
					.Where(x => x.Category == "RavenDB")
					.Distinct()
					.ToList();
			}
		}
	}
}
