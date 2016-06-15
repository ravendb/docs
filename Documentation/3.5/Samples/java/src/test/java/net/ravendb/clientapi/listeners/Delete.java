package net.ravendb.clientapi.listeners;

import org.apache.commons.lang.NotImplementedException;

import net.ravendb.abstractions.json.linq.RavenJObject;

public class Delete {
  //region document_delete_interface
  public interface IDocumentDeleteListener {

    /**
     * Invoked before the delete request is sent to the server.
     * @param key The key.
     * @param entityInstance The entity instance.
     * @param metadata The metadata.
     */
    public void beforeDelete(String key, Object entityInstance, RavenJObject metadata);
  }
  //endregion

  //region document_delete_example
  public static class PreventDeleteListener implements IDocumentDeleteListener {
    @Override
    public void beforeDelete(String key, Object entityInstance, RavenJObject metadata) {
      throw new NotImplementedException();
    }
  }
  //endregion
}
