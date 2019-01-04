package net.ravendb.Indexes.Querying;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.indexes.FieldIndexing;
import net.ravendb.client.documents.queries.suggestions.StringDistanceTypes;
import net.ravendb.client.documents.queries.suggestions.SuggestionOptions;
import net.ravendb.client.documents.queries.suggestions.SuggestionResult;
import net.ravendb.client.documents.queries.suggestions.SuggestionSortMode;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.Map;

public class Suggestions {

    //region suggestions_1
    public class Products_ByName extends AbstractIndexCreationTask {
        public Products_ByName() {
            map = "from product in docs.Products " +
                "select new " +
                "{ " +
                "  product.Name " +
                "} ";

            index("Name", FieldIndexing.SEARCH); // (optional) splitting name into multiple tokens
            suggestion("Name");// configuring suggestions
        }
    }
    //endregion

    private class Product {

    }

    public Suggestions() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region suggestions_2
                Product product = session
                    .query(Product.class, Products_ByName.class)
                    .search("Name", "chaig")
                    .firstOrDefault();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region suggestions_3
                Map<String, SuggestionResult> suggestionResult = session
                    .query(Product.class, Products_ByName.class)
                    .suggestUsing(builder -> builder.byField("Name", "chaig"))
                    .execute();

                System.out.println("Did you mean?");

                for (String suggestion : suggestionResult.get("Name").getSuggestions()) {
                    System.out.println("\t" + suggestion);
                }
                //endregion
            }
        }

        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region query_suggestion_over_multiple_words
                SuggestionOptions options = new SuggestionOptions();
                options.setAccuracy(0.4f);
                options.setPageSize(5);
                options.setDistance(StringDistanceTypes.JARO_WINKLER);
                options.setSortMode(SuggestionSortMode.POPULARITY);

                Map<String, SuggestionResult> resultsByMultipleWords = session
                    .query(Product.class, Products_ByName.class)
                    .suggestUsing(builder ->
                        builder.byField("Name", new String[]{"chaig", "tof"})
                            .withOptions(options))
                    .execute();

                System.out.println("Did you mean?");

                for (String suggestion : resultsByMultipleWords.get("Name").getSuggestions()) {
                    System.out.println("\t" + suggestion);
                }
                //endregion
            }
        }
    }
}
