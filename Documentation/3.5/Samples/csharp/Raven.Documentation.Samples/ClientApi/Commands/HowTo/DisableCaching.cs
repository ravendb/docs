using System;

using Raven.Client.Document;
using Raven.Json.Linq;

using Xunit;

namespace Raven.Documentation.Samples.ClientApi.Commands.HowTo
{
	public class DisableCaching
	{
		private interface IFoo
		{
			#region disable_caching_1
			IDisposable DisableAllCaching();
			#endregion
		}

		public DisableCaching()
		{
			using (var store = new DocumentStore())
			{
                #region disable_caching_2
                store.DatabaseCommands.Put("employees/1", null, new RavenJObject(), new RavenJObject());
                store.DatabaseCommands.Put("employees/2", null, new RavenJObject(), new RavenJObject());

                store.DatabaseCommands.Get("employees/1"); // Response: '200 OK'
                Assert.Equal(0, store.JsonRequestFactory.NumberOfCachedRequests); // not read from cache
                Assert.Equal(1, store.JsonRequestFactory.CurrentCacheSize); // employees/1 in cache

                store.DatabaseCommands.Get("employees/1"); // Response: '304 Not Modified'
                Assert.Equal(1, store.JsonRequestFactory.NumberOfCachedRequests); // read from cache
                Assert.Equal(1, store.JsonRequestFactory.CurrentCacheSize); // employees/1 in cache

                store.DatabaseCommands.Get("employees/1"); // Response: '304 Not Modified'
                Assert.Equal(2, store.JsonRequestFactory.NumberOfCachedRequests); // read from cache
                Assert.Equal(1, store.JsonRequestFactory.CurrentCacheSize); // employees/1 in cache

                // disable read from cache however can write to cache
                using (store.DatabaseCommands.DisableAllCaching())
                {
                    store.DatabaseCommands.Get("employees/2"); // Response: '200 OK'
                    Assert.Equal(2, store.JsonRequestFactory.NumberOfCachedRequests); // not read from cache
                    Assert.Equal(2, store.JsonRequestFactory.CurrentCacheSize); // employees/1 and employees/2 in cache
                    store.DatabaseCommands.Get("employees/2"); // Response: '200 OK'
                    Assert.Equal(2, store.JsonRequestFactory.NumberOfCachedRequests); // not read from cache
                    Assert.Equal(2, store.JsonRequestFactory.CurrentCacheSize); // employees/1 and employees/2 in cache
                }

                store.DatabaseCommands.Get("employees/2"); // Response: '304 Not Modified'
                Assert.Equal(3, store.JsonRequestFactory.NumberOfCachedRequests); // read from cache
                Assert.Equal(2, store.JsonRequestFactory.CurrentCacheSize); // employees/1 and employees/2 in cache
                #endregion
			}
		}
	}
}