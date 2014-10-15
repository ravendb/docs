package net.ravendb.clientapi.listeners;

import java.util.Arrays;
import java.util.List;

import net.ravendb.abstractions.json.linq.RavenJObject;


public class Store {
  //region document_store_interface
  public interface IDocumentStoreListener {
    /**
     * Invoked before the store request is sent to the server.
     * @param key The key.
     * @param entityInstance The entity instance.
     * @param metadata The metadata.
     * @param original The original document that was loaded from the server
     * @return
     *
     * Whatever the entity instance was modified and requires us re-serialize it.
     * Returning true would force re-serialization of the entity, returning false would
     * mean that any changes to the entityInstance would be ignored in the current SaveChanges call.
     */
    boolean beforeStore(String key, Object entityInstance, RavenJObject metadata, RavenJObject original);

    /**
     * Invoked after the store request is sent to the server.
     * @param key The key.
     * @param entityInstance The entity instance.
     * @param metadata The metadata
     */
    void afterStore(String key, Object entityInstance, RavenJObject metadata);
  }
  //endregion

  //region document_store_example
  public static class FilterForbiddenKeysDocumentListener implements IDocumentStoreListener {
    private static final List<String> FORBIDDEN_KEYS = Arrays.asList("system");

    @Override
    public boolean beforeStore(String key, Object entityInstance, RavenJObject metadata, RavenJObject original) {
      return FORBIDDEN_KEYS.contains(key.toLowerCase());
    }

    @Override
    public void afterStore(String key, Object entityInstance, RavenJObject metadata) {
      //empty by design
    }
  }
  //endregion
}
