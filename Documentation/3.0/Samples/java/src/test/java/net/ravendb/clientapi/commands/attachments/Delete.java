package net.ravendb.clientapi.commands.attachments;

import net.ravendb.abstractions.data.Etag;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class Delete {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region delete_1
    @Deprecated
    public void deleteAttachment(String key, Etag etag);
    //endregion
  }

  @SuppressWarnings("deprecation")
  public Delete() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region delete_2
      store.getDatabaseCommands().deleteAttachment("albums/holidays/sea.jpg", null);
      //endregion
    }
  }
}
