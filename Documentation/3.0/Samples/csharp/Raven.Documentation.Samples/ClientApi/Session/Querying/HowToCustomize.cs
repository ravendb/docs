using System;
using System.Collections.Generic;
using System.Linq;

using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
	public class HowToCustomize
	{
		private class Employees_NoLastName : AbstractTransformerCreationTask<Employee>
		{
		}

		private interface IFoo
		{
			#region customize_1_0
			IDocumentQueryCustomization BeforeQueryExecution(Action<IndexQuery> action);
			#endregion

			#region customize_2_0
			IDocumentQueryCustomization NoCaching();
			#endregion

			#region customize_3_0
			IDocumentQueryCustomization NoTracking();
			#endregion

			#region customize_4_0
			IDocumentQueryCustomization RandomOrdering();

			IDocumentQueryCustomization RandomOrdering(string seed);
			#endregion

			#region customize_5_0
			IDocumentQueryCustomization
				SetAllowMultipleIndexEntriesForSameDocumentToResultTransformer(bool val);
			#endregion

			#region customize_6_0
			IDocumentQueryCustomization ShowTimings();
			#endregion

			#region customize_7_0
			IDocumentQueryCustomization TransformResults(
				Func<IndexQuery, IEnumerable<object>, IEnumerable<object>> resultsTransformer);
			#endregion

			#region customize_8_0
			IDocumentQueryCustomization WaitForNonStaleResults();

			IDocumentQueryCustomization WaitForNonStaleResults(TimeSpan waitTimeout);
			#endregion

			#region customize_9_0
			IDocumentQueryCustomization WaitForNonStaleResultsAsOf(DateTime cutOff);

			IDocumentQueryCustomization WaitForNonStaleResultsAsOf(
				DateTime cutOff,
				TimeSpan waitTimeout);

			IDocumentQueryCustomization WaitForNonStaleResultsAsOf(Etag cutOffEtag);

			IDocumentQueryCustomization WaitForNonStaleResultsAsOf(
				Etag cutOffEtag,
				TimeSpan waitTimeout);
			#endregion

			#region customize_10_0
			IDocumentQueryCustomization WaitForNonStaleResultsAsOfLastWrite();

			IDocumentQueryCustomization WaitForNonStaleResultsAsOfLastWrite(TimeSpan waitTimeout);
			#endregion

			#region customize_11_0
			IDocumentQueryCustomization WaitForNonStaleResultsAsOfNow();

			IDocumentQueryCustomization WaitForNonStaleResultsAsOfNow(TimeSpan waitTimeout);
			#endregion
		}

		public HowToCustomize()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region customize_1_1
					// set 'PageSize' to 10
					var results = session.Query<Employee>()
						.Customize(x => x.BeforeQueryExecution(query => query.PageSize = 10))
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region customize_2_1
					var results = session.Query<Employee>()
						.Customize(x => x.NoCaching())
						.Where(x => x.FirstName == "Robert")
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region customize_3_1
					var results = session.Query<Employee>()
						.Customize(x => x.NoTracking())
						.Where(x => x.FirstName == "Robert")
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region customize_4_1
					// results will be ordered randomly each time
					var results = session.Query<Employee>()
						.Customize(x => x.RandomOrdering())
						.Where(x => x.FirstName == "Robert")
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region customize_5_1
					var results = session.Query<Employee>()
						.Customize(x => x.SetAllowMultipleIndexEntriesForSameDocumentToResultTransformer(true))
						.TransformWith<Employees_NoLastName, Employee>()
						.Where(x => x.FirstName == "Robert")
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region customize_6_1
					RavenQueryStatistics stats;
					var results = session.Query<Employee>()
						.Customize(x => x.ShowTimings())
						.Statistics(out stats)
						.Where(x => x.FirstName == "Robert")
						.ToList();

					var timings = stats.TimingsInMilliseconds;
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region customize_7_1
					// filter-out all results where 'LastName' equals 'Doe' on client-side
					var results = session.Query<Employee>()
						.Customize(x => x.TransformResults((indexQuery, queryResults) => queryResults.Cast<Employee>().Where(q => q.LastName != "Doe")))
						.Where(x => x.FirstName == "Robert")
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region customize_8_1
					var results = session.Query<Employee>()
						.Customize(x => x.WaitForNonStaleResults())
						.Where(x => x.FirstName == "Robert")
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region customize_9_1
					// results will be considered non-stale
					// if last indexed document modified date
					// will be greater than 1 minute ago
					var results = session.Query<Employee>()
						.Customize(x => x.WaitForNonStaleResultsAsOf(DateTime.Now.AddMinutes(-1)))
						.Where(x => x.FirstName == "Robert")
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region customize_10_1
					var results = session.Query<Employee>()
						.Customize(x => x.WaitForNonStaleResultsAsOfLastWrite())
						.Where(x => x.FirstName == "Robert")
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region customize_11_1
					var results = session.Query<Employee>()
						.Customize(x => x.WaitForNonStaleResultsAsOfNow())
						.Where(x => x.FirstName == "Robert")
						.ToList();
					#endregion
				}
			}
		}
	}
}