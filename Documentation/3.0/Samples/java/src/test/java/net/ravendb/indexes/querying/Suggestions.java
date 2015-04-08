package net.ravendb.indexes.querying;

import net.ravendb.abstractions.data.StringDistanceTypes;
import net.ravendb.abstractions.data.SuggestionQuery;
import net.ravendb.abstractions.data.SuggestionQueryResult;
import net.ravendb.abstractions.indexing.FieldIndexing;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.indexes.AbstractIndexCreationTask;
import net.ravendb.client.linq.IRavenQueryable;
import net.ravendb.samples.northwind.Product;
import net.ravendb.samples.northwind.QProduct;


public class Suggestions {

  //region suggestions_1
  public static class Products_ByName extends AbstractIndexCreationTask {
    public Products_ByName() {
      QProduct p = QProduct.product;
      map =
       " from product in docs.Products " +
       " select new                    " +
       "   {                           " +
       "       product.Name            " +
       "   }; ";

      index(p.name, FieldIndexing.ANALYZED); // (optional) splitting name into multiple tokens
      suggestion(p.name); // configuring suggestions
    }
  }
  //endregion

  @SuppressWarnings("boxing")
  public Suggestions() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region suggestions_2
        QProduct p = QProduct.product;
        IRavenQueryable<Product> query = session
          .query(Product.class, Products_ByName.class)
          .where(p.name.eq("chaig"));

        Product product = query.firstOrDefault();
        //endregion

        //region suggestions_3
        if (product == null) {
          SuggestionQueryResult suggestionResult = query.suggest();
          System.out.println("Did you mean?");
          for (String suggestion : suggestionResult.getSuggestions()) {
            System.out.println("\t" + suggestion);
          }
        }
        //endregion

        //region query_suggestion_with_options
        SuggestionQuery suggestionQuery = new SuggestionQuery();
        suggestionQuery.setField("Name");
        suggestionQuery.setTerm("chaig");
        suggestionQuery.setAccuracy(0.4f);
        suggestionQuery.setMaxSuggestions(5);
        suggestionQuery.setDistance(StringDistanceTypes.JARO_WINKLER);
        suggestionQuery.setPopularity(true);
        session
          .query(Product.class, Products_ByName.class)
          .suggest(suggestionQuery);
        //endregion

      }
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region document_store_suggestion
      SuggestionQuery suggestionQuery = new SuggestionQuery();
      suggestionQuery.setField("Name");
      suggestionQuery.setTerm("chaig");
      suggestionQuery.setAccuracy(0.4f);
      suggestionQuery.setMaxSuggestions(5);
      suggestionQuery.setDistance(StringDistanceTypes.JARO_WINKLER);
      suggestionQuery.setPopularity(true);
      store
        .getDatabaseCommands()
        .suggest("Products/ByName", suggestionQuery);
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region query_suggestion_over_multiple_words
      SuggestionQuery suggestionQuery = new SuggestionQuery();
      suggestionQuery.setField("Name");
      suggestionQuery.setTerm("<<chaig tof>>");
      suggestionQuery.setAccuracy(0.4f);
      suggestionQuery.setMaxSuggestions(5);
      suggestionQuery.setDistance(StringDistanceTypes.JARO_WINKLER);
      suggestionQuery.setPopularity(true);
      SuggestionQueryResult resultsByMultipleWords = store
        .getDatabaseCommands()
        .suggest("Products/ByName", suggestionQuery);

      System.out.println("Did you mean?");
      for (String suggestion : resultsByMultipleWords.getSuggestions()) {
        System.out.println("\t" + suggestion);
      }
      //endregion
    }
  }
}
