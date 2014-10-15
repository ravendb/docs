package net.ravendb.clientapi.listeners;

import net.ravendb.client.IDocumentQueryCustomization;


public class Query {
  //region document_query_listener
  public interface IDocumentQueryListener {
    /**
     * Allow to customize a query globally
     * @param queryCustomization
     */
    void beforeQueryExecuted(IDocumentQueryCustomization queryCustomization);
  }
  //endregion

  //region document_query_example
  public class DisableCachingQueryListener implements IDocumentQueryListener {
    @Override
    public void beforeQueryExecuted(IDocumentQueryCustomization queryCustomization) {
      queryCustomization.noCaching();
    }
  }
  //endregion
}
