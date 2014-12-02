package net.ravendb.clientapi.commands.howto;

import static org.junit.Assert.assertEquals;
import net.ravendb.abstractions.json.linq.RavenJObject;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class DisableCaching {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region disable_caching_1
    public AutoCloseable disableAllCaching();
    //endregion
  }

  public DisableCaching() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region disable_caching_2
      store.getDatabaseCommands().put("employees/1", null, new RavenJObject(), new RavenJObject());
      store.getDatabaseCommands().put("employees/2", null, new RavenJObject(), new RavenJObject());

      store.getDatabaseCommands().get("employees/1"); // Response: '200 OK'
      assertEquals(0, store.getJsonRequestFactory().getNumOfCachedRequests()); // not read from cache
      assertEquals(1, store.getJsonRequestFactory().getCurrentCacheSize()); // employees/1 in cache

      store.getDatabaseCommands().get("employees/1"); // Response: '304 Not Modified'
      assertEquals(1, store.getJsonRequestFactory().getNumOfCachedRequests()); // read from cache
      assertEquals(1, store.getJsonRequestFactory().getCurrentCacheSize()); // employees/1 in cache

      store.getDatabaseCommands().get("employees/1"); // Response: '304 Not Modified'
      assertEquals(2, store.getJsonRequestFactory().getNumOfCachedRequests()); // read from cache
      assertEquals(1, store.getJsonRequestFactory().getCurrentCacheSize()); // employees/1 in cache

      try (AutoCloseable disableCache = store.getDatabaseCommands().disableAllCaching()) {
        store.getDatabaseCommands().get("employees/2"); // Response: '200 OK'
        assertEquals(2, store.getJsonRequestFactory().getNumOfCachedRequests()); // not read from cache
        assertEquals(2, store.getJsonRequestFactory().getCurrentCacheSize()); // employees/1 and employees/2 in cache
        store.getDatabaseCommands().get("employees/2"); // Response: '200 OK'
        assertEquals(2, store.getJsonRequestFactory().getNumOfCachedRequests()); // not read from cache
        assertEquals(2, store.getJsonRequestFactory().getCurrentCacheSize()); // employees/1 and employees/2 in cache
      }

      store.getDatabaseCommands().get("employees/2"); // Response: '304 Not Modified'
      assertEquals(3, store.getJsonRequestFactory().getNumOfCachedRequests()); // read from cache
      assertEquals(2, store.getJsonRequestFactory().getCurrentCacheSize()); // employees/1 and employees/2 in cache
      //endregion
    }
  }
}
