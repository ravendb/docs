package net.ravendb.clientapi.session.querying;

import net.ravendb.abstractions.data.StringDistanceTypes;
import net.ravendb.abstractions.data.SuggestionQuery;
import net.ravendb.abstractions.data.SuggestionQueryResult;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.indexes.AbstractIndexCreationTask;
import net.ravendb.samples.northwind.Employee;


public class HowToWorkWithSuggestions {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region suggest_1
    public SuggestionQueryResult suggest();

    public SuggestionQueryResult suggest(SuggestionQuery query);
    //endregion
  }

  private class Employees_ByFullName extends AbstractIndexCreationTask {
    // empty
  }

  @SuppressWarnings("unused")
  public HowToWorkWithSuggestions() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region suggest_2
        SuggestionQuery suggestionQuery = new SuggestionQuery("FullName", "johne");
        suggestionQuery.setAccuracy(0.4f);
        suggestionQuery.setMaxSuggestions(5);
        suggestionQuery.setDistance(StringDistanceTypes.JARO_WINKLER);
        suggestionQuery.setPopularity(true);
        SuggestionQueryResult suggestions = session
          .query(Employee.class, Employees_ByFullName.class)
          .suggest();
        //endregion
      }
    }
  }
}
