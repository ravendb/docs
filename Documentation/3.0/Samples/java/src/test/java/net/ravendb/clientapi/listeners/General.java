package net.ravendb.clientapi.listeners;

import net.ravendb.abstractions.json.linq.RavenJObject;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentSessionListeners;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.listeners.IDocumentDeleteListener;
import net.ravendb.client.listeners.IDocumentStoreListener;


public class General {
  public static class SampleDocumentStoreListener implements IDocumentStoreListener {
    @Override
    public boolean beforeStore(String key, Object entityInstance, RavenJObject metadata, RavenJObject original) {
      return false;
    }
    @Override
    public void afterStore(String key, Object entityInstance, RavenJObject metadata) {
      // empty
    }
  }

  public static class SampleDocumentDeleteListener implements IDocumentDeleteListener {
    @Override
    public void beforeDelete(String key, Object entityInstance, RavenJObject metadata) {
      // empty
    }
  }

  public General() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region register_listener
      store.getListeners().registerListener(new SampleDocumentStoreListener());
      //endregion

      //region set_listeners
      DocumentSessionListeners listeners = new DocumentSessionListeners();
      listeners.registerListener(new SampleDocumentDeleteListener());
      listeners.registerListener(new SampleDocumentStoreListener());
      store.setListeners(listeners);
      //endregion
    }
  }
}
