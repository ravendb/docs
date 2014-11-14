package net.ravendb.clientapi.configuration.conventions;

import net.ravendb.client.document.DocumentConvention;
import net.ravendb.client.document.DocumentStore;


public class WhatAreConventions {
  public WhatAreConventions() {
    DocumentStore store = new DocumentStore();
    //region conventions_1
    DocumentConvention conventions = store.getConventions();
    //endregion
  }
}
