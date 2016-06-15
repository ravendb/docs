package net.ravendb.clientapi.commands.querying;

import net.ravendb.abstractions.data.SuggestionQuery;
import net.ravendb.abstractions.data.SuggestionQueryResult;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class HowToWorkWithSuggestionQuery {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region suggest_1
    public SuggestionQueryResult suggest(String index, SuggestionQuery suggestionQuery);
    //endregion
  }

  public HowToWorkWithSuggestionQuery() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region suggest_2
      // Get suggestions for 'johne' using 'FullName' field in 'Users/ByFullName' index
      SuggestionQuery suggestionQuery = new SuggestionQuery();
      suggestionQuery.setField("FullName");
      suggestionQuery.setTerm("johne");
      suggestionQuery.setMaxSuggestions(10);
      SuggestionQueryResult result = store.getDatabaseCommands().suggest("Users/ByFullName", suggestionQuery);

      System.out.println("Did you mean?");

      for (String suggestion: result.getSuggestions()) {
        System.out.println("\t" + suggestion);
      }
      // Did you mean?
      //      john
      //      jones
      //      johnson
      //endregion
    }
  }
}
