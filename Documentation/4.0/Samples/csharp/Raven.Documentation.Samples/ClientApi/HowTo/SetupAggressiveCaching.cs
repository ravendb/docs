﻿using System;
using System.Collections.Generic;
using System.Linq;

using Raven.Client.Document;
using Raven.Client.Documents;
using Raven.Documentation.CodeSamples.Orders;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.HowTo
{
	public class SetupAggressiveCaching
	{
		public SetupAggressiveCaching()
		{
			using (var documentStore = new DocumentStore { Urls = new []{"http://localhost:8080"} })
			{
				documentStore.Initialize();

				#region should_cache_delegate
				documentStore.Conventions.ShouldCacheRequest = url => true;
                #endregion

                #region max_number_of_requests
			    // TODO change this to MaxHttpCacheSize 
                documentStore.Conventions.MaxNumberOfRequestsPerSession = 1;
				#endregion

				#region disable_changes_tracking
				documentStore.Conventions.ShouldAggressiveCacheTrackChanges = false;
				#endregion

				using (var session = documentStore.OpenSession())
				{
					#region aggressive_cache_load
					using (session.Advanced.DocumentStore.AggressivelyCacheFor(TimeSpan.FromMinutes(5)))
					{
						Order user = session.Load<Order>("orders/1");
					}
					#endregion
				}

				using (var session = documentStore.OpenSession())
				{
					#region aggressive_cache_query
					using (session.Advanced.DocumentStore.AggressivelyCacheFor(TimeSpan.FromMinutes(5)))
					{
						List<Order> users = session.Query<Order>().ToList();
					}
					#endregion

					#region aggressive_cache_for_one_day_1
					using (session.Advanced.DocumentStore.AggressivelyCache()) { }
					#endregion

					#region aggressive_cache_for_one_day_2
					using (session.Advanced.DocumentStore.AggressivelyCacheFor(TimeSpan.FromDays(1))) { }
					#endregion
				}

			}
		}
	}
}
