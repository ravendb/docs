package net.ravendb.clientapi.commands.documents.howto;

import net.ravendb.abstractions.data.JsonDocumentMetadata;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class Head {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region head_1
    public JsonDocumentMetadata head(String key);
    //endregion
  }

  @SuppressWarnings("unused")
  public Head() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region head_2
      JsonDocumentMetadata metadata = store.getDatabaseCommands().head("employees/1"); // null if does not exist
      //endregion
    }
  }
}
