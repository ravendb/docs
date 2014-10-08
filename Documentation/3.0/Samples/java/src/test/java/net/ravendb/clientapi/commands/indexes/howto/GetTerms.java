package net.ravendb.clientapi.commands.indexes.howto;

import java.util.List;

import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class GetTerms {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region get_terms_1
    public List<String> getTerms(String index, String field, String fromValue, int pageSize);
    //endregion
  }

  public GetTerms() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region get_terms_2
      store.getDatabaseCommands().getTerms("Orders/Totals", "Company", null, 128);
      //endregion
    }
  }
}
