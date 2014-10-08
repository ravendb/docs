package net.ravendb.clientapi.commands.querying;

import java.util.Collection;
import java.util.List;

import net.ravendb.abstractions.data.IndexQuery;
import net.ravendb.abstractions.data.QueryResult;
import net.ravendb.abstractions.json.linq.RavenJObject;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class HowToQueryDatabase {

  @SuppressWarnings("unused")
  private interface IFoo {

    //region query_database_1
    public QueryResult query(String index, IndexQuery query);

    public QueryResult query(String index, IndexQuery query, String[] includes);

    public QueryResult query(String index, IndexQuery query, String[] includes, boolean metadataOnly);

    public QueryResult query(String index, IndexQuery query, String[] includes, boolean metadataOnly, boolean indexEntriesOnly);
    //endregion
  }

  @SuppressWarnings("unused")
  public HowToQueryDatabase() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      QueryResult result;
      //region query_database_2
      result = store.getDatabaseCommands().query("Orders/Totals", new IndexQuery("Company:companies/1"));

      List<RavenJObject> users = result.getResults(); // documents resulting from this query - orders
      //endregion

      //region query_database_3
      result = store.getDatabaseCommands().query("Orders/Totals", new IndexQuery(), new String[] { "Company", "Employee" });

      Collection<RavenJObject> referencedDocs = result.getIncludes(); // included documents - companies and employees
      //endregion
    }
  }
}
