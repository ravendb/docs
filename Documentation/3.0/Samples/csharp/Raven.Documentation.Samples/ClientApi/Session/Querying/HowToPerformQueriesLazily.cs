using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
	public class HowToPerformQueriesLazily
	{
		private interface IFoo
		{
			/*
			#region lazy_1
			Lazy<IEnumerable<T>> Lazily<T>(
				this IQueryable<T> source) { ... }

			Lazy<IEnumerable<T>> Lazily<T>(
				this IQueryable<T> source,
				Action<IEnumerable<T>> onEval) { ... }

			Lazy<Task<IEnumerable<T>>> LazilyAsync<T>(
				this IQueryable<T> source) { ... }

			Lazy<Task<IEnumerable<T>>> LazilyAsync<T>(
				this IQueryable<T> source,
				Action<IEnumerable<T>> onEval) { ... }
			#endregion
			*/

			/*
			#region lazy_4
			Lazy<int> CountLazily<T>(
				this IRavenQueryable<T> source) { ... }
			#endregion
			*/

			/*
			#region lazy_6
			Lazy<SuggestionQueryResult> SuggestLazy(
				this IQueryable queryable) { ... }

			Lazy<SuggestionQueryResult> SuggestLazy(
				this IQueryable queryable,
				SuggestionQuery query) { ... }
			#endregion
			*/

			/*
			#region lazy_8
			Lazy<FacetResults> ToFacetsLazy<T>(
				this IQueryable<T> queryable,
				string facetSetupDoc,
				int start = 0,
				int? pageSize = null) { ... }

			Lazy<FacetResults> ToFacetsLazy<T>(
				this IQueryable<T> queryable,
				IEnumerable<Facet> facets,
				int start = 0,
				int? pageSize = null) { ... }
			#endregion
			*/
		}

		public HowToPerformQueriesLazily()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region lazy_2
					Lazy<IEnumerable<Employee>> employeesLazy = session
						.Query<Employee>()
						.Where(x => x.FirstName == "Robert")
						.Lazily();

					IEnumerable<Employee> employees = employeesLazy.Value; // query will be executed here
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region lazy_5
					Lazy<int> countLazy = session
						.Query<Employee>()
						.Where(x => x.FirstName == "Robert")
						.CountLazily();

					int count = countLazy.Value; // query will be executed here
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region lazy_7
					Lazy<SuggestionQueryResult> suggestLazy = session
						.Query<Employee>()
						.SuggestLazy();

					SuggestionQueryResult suggest = suggestLazy.Value; // query will be executed here
					string[] suggestions = suggest.Suggestions;
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region lazy_9
					Lazy<FacetResults> facetsLazy = session
						.Query<Camera>("Camera/Costs")
						.ToFacetsLazy("facets/CameraFacets");

					FacetResults facets = facetsLazy.Value; // query will be executed here
					Dictionary<string, FacetResult> results = facets.Results;
					#endregion
				}
			}
		}

		private async Task LazilyAsync()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region lazy_3
					Lazy<Task<IEnumerable<Employee>>> employeesLazy = session
						.Query<Employee>()
						.Where(x => x.FirstName == "Robert")
						.LazilyAsync();

					IEnumerable<Employee> employees = await employeesLazy.Value; // query will be executed here
					#endregion
				}
			}
		}
	}
}