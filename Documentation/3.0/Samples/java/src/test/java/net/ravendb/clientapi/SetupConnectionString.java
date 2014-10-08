package net.ravendb.clientapi;

import net.ravendb.client.document.DocumentStore;

public class SetupConnectionString {
  public SetupConnectionString() throws Exception {

    //region connection_string_1
    try (DocumentStore store = new DocumentStore()) {
      store.parseConnectionString("Url = http://localhost:8080");
      store.initialize();
    }
    //endregion

    /*
    //region connection_string_2
    Url = http://ravendb.mydomain.com
        // connect to a remote RavenDB instance at ravendb.mydomain.com, to the default database
    Url = http://ravendb.mydomain.com;Database=Northwind
        // connect to a remote RavenDB instance at ravendb.mydomain.com, to the Northwind database there
    //endregion
    */
  }

}
