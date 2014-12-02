package net.ravendb.clientapi.commands.indexes.howto;

import net.ravendb.abstractions.indexing.IndexMergeResults;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class GetIndexMergeSuggestions {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region merge_suggestions_1
    public IndexMergeResults getIndexMergeSuggestions();
    //endregion
  }

  @SuppressWarnings("unused")
  public GetIndexMergeSuggestions() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region merge_suggestions_2
      IndexMergeResults suggestions = store.getDatabaseCommands().getIndexMergeSuggestions();
      //endregion
    }
  }
}
