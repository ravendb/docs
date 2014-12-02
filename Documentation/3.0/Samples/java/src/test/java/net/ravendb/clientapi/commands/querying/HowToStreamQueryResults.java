package net.ravendb.clientapi.commands.querying;

import net.ravendb.abstractions.basic.CloseableIterator;
import net.ravendb.abstractions.basic.Reference;
import net.ravendb.abstractions.data.IndexQuery;
import net.ravendb.abstractions.data.QueryHeaderInformation;
import net.ravendb.abstractions.json.linq.RavenJObject;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class HowToStreamQueryResults {

  @SuppressWarnings("unused")
  private interface IFoo {

    //region stream_query_1
    public CloseableIterator<RavenJObject> streamQuery(String index, IndexQuery query, Reference<QueryHeaderInformation> queryHeaderInfo);
    //endregion
  }

  @SuppressWarnings("unused")
  public HowToStreamQueryResults() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region stream_query_2
      Reference<QueryHeaderInformation> queryHeaderInfoRef = new Reference<>();
      try (CloseableIterator<RavenJObject> iterator = store.getDatabaseCommands().streamQuery("Orders/Totals", new IndexQuery("Company:companies/1"), queryHeaderInfoRef)) {

        while (iterator.hasNext()) {
          RavenJObject order = iterator.next();
        }
      }


      //endregion
    }
  }
}
