using System.Linq;
using System.Threading.Tasks;

using Raven.Client;
using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.ClientApi.Session.Querying
{
	public class HowToPerformQueriesLazily
	{
		private interface IFoo
		{
			/*
			#region lazy_1
			public static Lazy<IEnumerable<T>> Lazily<T>(
				this IQueryable<T> source) { ... }

			public static Lazy<IEnumerable<T>> Lazily<T>(
				this IQueryable<T> source,
				Action<IEnumerable<T>> onEval) { ... }

			public static Lazy<Task<IEnumerable<T>>> LazilyAsync<T>(
				this IQueryable<T> source) { ... }

			public static Lazy<Task<IEnumerable<T>>> LazilyAsync<T>(
				this IQueryable<T> source,
				Action<IEnumerable<T>> onEval) { ... }
			#endregion
			*/

			/*
			#region lazy_4
			public static Lazy<int> CountLazily<T>(
				this IRavenQueryable<T> source) { ... }
			#endregion
			*/

			/*
			#region lazy_6
			public static Lazy<SuggestionQueryResult> SuggestLazy(
				this IQueryable queryable) { ... }

			public static Lazy<SuggestionQueryResult> SuggestLazy(
				this IQueryable queryable,
				SuggestionQuery query) { ... }
			#endregion
			*/

			/*
			#region lazy_8
			public static Lazy<FacetResults> ToFacetsLazy<T>(
				this IQueryable<T> queryable,
				string facetSetupDoc,
				int start = 0,
				int? pageSize = null) { ... }

			public static Lazy<FacetResults> ToFacetsLazy<T>(
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
					var peopleLazy = session
						.Query<Person>()
						.Where(x => x.FirstName == "John")
						.Lazily();

					var people = peopleLazy.Value; // query will be executed here
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region lazy_5
					var countLazy = session
						.Query<Person>()
						.Where(x => x.FirstName == "John")
						.CountLazily();

					var count = countLazy.Value; // query will be executed here
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region lazy_7
					var suggestLazy = session
						.Query<Person>()
						.SuggestLazy();

					var suggest = suggestLazy.Value; // query will be executed here
					var suggestions = suggest.Suggestions;
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region lazy_9
					var facetsLazy = session
						.Query<Camera>("Camera/Costs")
						.ToFacetsLazy("facets/CameraFacets");

					var facets = facetsLazy.Value; // query will be executed here
					var results = facets.Results;
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
					var peopleLazy = session
						.Query<Person>()
						.Where(x => x.FirstName == "John")
						.LazilyAsync();

					var people = await peopleLazy.Value; // query will be executed here
					#endregion
				}
			}
		}
	}
}