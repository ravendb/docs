namespace Raven.Documentation.CodeSamples.ClientApi.HowTo
{
	using System;
	using System.Linq;
	using Client.Document;
	using Orders;

	public class SetupAggressiveCaching
	{
		public SetupAggressiveCaching()
		{
			using (var documentStore = new DocumentStore { Url = "http://localhost:8080" })
			{
				documentStore.Initialize();

				#region should_cache_delegate
				documentStore.Conventions.ShouldCacheRequest = url => true;
				#endregion

				#region max_number_of_requests
				documentStore.MaxNumberOfCachedRequests = 2048;
				#endregion

				#region disable_changes_tracking
				documentStore.Conventions.ShouldAggressiveCacheTrackChanges = false;
				#endregion

				using (var session = documentStore.OpenSession())
				{
					#region aggressive_cache_load
					using (session.Advanced.DocumentStore.AggressivelyCacheFor(TimeSpan.FromMinutes(5)))
					{
						var user = session.Load<Order>("orders/1");
					}
					#endregion
				}

				using (var session = documentStore.OpenSession())
				{
					#region aggressive_cache_query
					using (session.Advanced.DocumentStore.AggressivelyCacheFor(TimeSpan.FromMinutes(5)))
					{
						var users = session.Query<Order>().ToList();
					}
					#endregion

					#region aggressive_cache_for_one_day_1
					using (session.Advanced.DocumentStore.AggressivelyCache()) { }
					#endregion

					#region aggressive_cache_for_one_day_2
					using (session.Advanced.DocumentStore.AggressivelyCacheFor(TimeSpan.FromDays(1))) { }
					#endregion
				}

				#region should_save_changes_force_aggressive_cache_check_convention
				documentStore.Conventions.ShouldSaveChangesForceAggressiveCacheCheck = true;
				#endregion
			}
		}
	}
}