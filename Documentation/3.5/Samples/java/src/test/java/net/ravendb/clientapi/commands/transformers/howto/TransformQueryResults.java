package net.ravendb.clientapi.commands.transformers.howto;

import net.ravendb.abstractions.basic.CloseableIterator;
import net.ravendb.abstractions.basic.Reference;
import net.ravendb.abstractions.data.IndexQuery;
import net.ravendb.abstractions.data.MoreLikeThisQuery;
import net.ravendb.abstractions.data.MultiLoadResult;
import net.ravendb.abstractions.data.QueryHeaderInformation;
import net.ravendb.abstractions.data.QueryResult;
import net.ravendb.abstractions.json.linq.RavenJObject;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class TransformQueryResults {
  @SuppressWarnings("unused")
  public TransformQueryResults() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region query_transformer_1
      // query for all orders with 'Company' equal to 'companies/1' using 'Orders/Totals' index
      // and transform results using 'Order/Statistics' transformer
      IndexQuery indexQuery = new IndexQuery();
      indexQuery.setQuery("Company:companies/1");
      indexQuery.setResultsTransformer("Order/Statistics");
      QueryResult result = store.getDatabaseCommands().query("Orders/Totals", indexQuery);
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region query_transformer_2
      // query for all orders with 'Company' equal to 'companies/1' using 'Orders/Totals' index
      // and transform results using 'Order/Statistics' transformer
      // stream the results
      Reference<QueryHeaderInformation> queryHeaderInfoRef = new Reference<QueryHeaderInformation>();
      IndexQuery indexQuery = new IndexQuery();
      indexQuery.setQuery("Company:companies/1");
      indexQuery.setResultsTransformer("Order/Statistics");
      CloseableIterator<RavenJObject> result = store.getDatabaseCommands().streamQuery("Orders/Totals", indexQuery, queryHeaderInfoRef);
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region query_transformer_3
      // Search for similar documents to 'articles/1'
      // using 'Articles/MoreLikeThis' index, search only field 'Body'
      // and transform results using 'Articles/NoComments' transformer
      MoreLikeThisQuery moreLikeThisQuery = new MoreLikeThisQuery();
      moreLikeThisQuery.setIndexName("Articles/MoreLikeThis");
      moreLikeThisQuery.setDocumentId("articles/1");
      moreLikeThisQuery.setFields(new String[] { "Body" });
      moreLikeThisQuery.setResultsTransformer("Articles/NoComments");
      MultiLoadResult result = store.getDatabaseCommands().moreLikeThis(moreLikeThisQuery);
      //endregion
    }
  }
}
