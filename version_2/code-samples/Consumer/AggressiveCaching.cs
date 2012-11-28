namespace RavenCodeSamples.Consumer
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Raven.Client.Document;

	public class AggressiveCaching
	{
		public class User
		{
			public string Name { get; set; }
		}

		public AggressiveCaching()
		{
			using (var documentStore = new DocumentStore {Url = "http://localhost:8080"})
			{
				documentStore.Initialize();

				#region should_cache_delegate
				documentStore.Conventions.ShouldCacheRequest = url => true;
				#endregion

				#region max_number_of_requests
				documentStore.MaxNumberOfCachedRequests = 2048;
				#endregion

				using (var session = documentStore.OpenSession())
				{
					#region aggressive_cache_load
					using (session.Advanced.DocumentStore.AggressivelyCacheFor(TimeSpan.FromMinutes(5)))
					{
						var user = session.Load<User>("users/1");
					}
					#endregion
				}

				using (var session = documentStore.OpenSession())
				{
					#region aggressive_cache_query
					using (session.Advanced.DocumentStore.AggressivelyCacheFor(TimeSpan.FromMinutes(5)))
					{
						var users = session.Query<User>().ToList();
					}
					#endregion
				}
			}
		}
	}
}
