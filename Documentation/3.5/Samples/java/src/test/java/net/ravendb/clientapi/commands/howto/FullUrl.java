package net.ravendb.clientapi.commands.howto;

import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class FullUrl {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region full_url_1
    public String urlFor(String documentKey);
    //endregion
  }

  @SuppressWarnings("unused")
  public FullUrl() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region full_url_2
      // http://localhost:8080/databases/Northwind/docs/employees/1
      String url = store.getDatabaseCommands().urlFor("employees/1");
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region full_url_3
      //  // http://localhost:8080/docs/employees/1
      String url = store.getDatabaseCommands().forSystemDatabase().urlFor("employees/1");
      //endregion
    }
  }
}
