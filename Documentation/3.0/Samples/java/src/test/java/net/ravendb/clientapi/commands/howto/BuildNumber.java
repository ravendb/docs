package net.ravendb.clientapi.commands.howto;

import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class BuildNumber {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region build_number_1
    BuildNumber getBuildNumber();
    //endregion
  }

  @SuppressWarnings("unused")
  public BuildNumber() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region build_number_2
      net.ravendb.abstractions.data.BuildNumber buildNumber =
         store.getDatabaseCommands().getGlobalAdmin().getBuildNumber();
      //endregion
    }
  }
}
