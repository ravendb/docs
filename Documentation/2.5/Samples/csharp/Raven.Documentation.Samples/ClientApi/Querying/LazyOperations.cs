namespace RavenCodeSamples.ClientApi.Querying
{
	using System;
	using System.Collections.Generic;

	using Raven.Abstractions.Data;
	#region using
	using Raven.Client;
	#endregion
	using Raven.Client.Linq;

	public class LazyOperations : CodeSampleBase
	{
		private class Camera
		{
			public decimal Cost { get; set; }
		}

		#region lazy_operations_loading_6
		private class User
		{
			public string Id { get; set; }

			public string Name { get; set; }

			public string CityId { get; set; }
		}

		private class City
		{
			public string Id { get; set; }

			public string Name { get; set; }
		}

		#endregion

		public void Sample()
		{
			using (var store = NewDocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region lazy_operations_querying_1
					IEnumerable<User> users = session
						.Query<User>()
						.Where(x => x.Name == "john");

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region lazy_operations_querying_2
					Lazy<IEnumerable<User>> lazyUsers = session
						.Query<User>()
						.Where(x => x.Name == "John")
						.Lazily();

					#endregion

					#region lazy_operations_querying_3
					IEnumerable<User> users = lazyUsers.Value;

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region lazy_operations_querying_4
					IEnumerable<User> users = null;
					IEnumerable<City> cities = null;

					session
						.Query<User>()
						.Where(x => x.Name == "John")
						.Lazily(x => users = x);

					session
						.Query<City>()
						.Where(x => x.Name == "New York")
						.Lazily(x => cities = x);

					session.Advanced.Eagerly.ExecuteAllPendingLazyOperations();

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region lazy_operations_querying_5
					Lazy<IEnumerable<User>> users = session.Advanced
						.LuceneQuery<User>()
						.WhereEquals("Name", "John")
						.Lazily();

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region lazy_operations_loading_1
					User user = session.Load<User>("users/1");

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region lazy_operations_loading_2
					Lazy<User> lazyUser = session.Advanced.Lazily.Load<User>("users/1");

					#endregion

					#region lazy_operations_loading_3
					User user = lazyUser.Value;

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region lazy_operations_loading_4
					User user = null;

					session.Advanced.Lazily.Load<User>("users/1", x => user = x);
					session.Advanced.Eagerly.ExecuteAllPendingLazyOperations();

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region lazy_operations_loading_5
					var users = session.Advanced.Lazily.LoadStartingWith<User>("users/1");

					#endregion
				}

				#region lazy_operations_loading_7
				using (var session = store.OpenSession())
				{
					var user = new User
								   {
									   Id = "users/1",
									   Name = "John",
									   CityId = "cities/1"
								   };

					var city = new City
								   {
									   Id = "cities/1",
									   Name = "New York"
								   };

					session.Store(user);
					session.Store(city);
					session.SaveChanges();
				}

				#endregion

				using (var session = store.OpenSession())
				{
					#region lazy_operations_loading_8
					var lazyUser = session.Advanced.Lazily
						.Include("CityId")
						.Load<User>("users/1");

					var user = lazyUser.Value;
					var isCityLoaded = session.Advanced.IsLoaded("cities/1"); // will be true

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region lazy_operations_facets_1
					Lazy<FacetResults> lazyFacetResults = session
						.Query<Camera>("CameraCost")
						.Where(x => x.Cost >= 100 && x.Cost <= 300)
						.ToFacetsLazy("facets/CameraFacets");

					FacetResults facetResults = lazyFacetResults.Value;

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region lazy_operations_suggest_1
					Lazy<SuggestionQueryResult> lazySuggestionResult = session
						.Query<User>()
						.Where(x => x.Name == "John")
						.SuggestLazy();

					SuggestionQueryResult suggestionResult = lazySuggestionResult.Value;

					#endregion
				}
			}
		}
	}
}
