package net.ravendb.ClientApi.Session.Querying;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.queries.suggestions.*;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.Map;
import java.util.function.Consumer;

public class HowToWorkWithSuggestions {

    private class Foo {

        //region suggest_7
        private int pageSize = 15;

        private StringDistanceTypes distance = StringDistanceTypes.LEVENSHTEIN;

        private Float accuracy = 0.5f;

        private SuggestionSortMode sortMode = SuggestionSortMode.POPULARITY;

        // getters and setters for fields listed above
        //endregion
    }

    private interface IFoo<T> {
        //region suggest_1
        ISuggestionDocumentQuery<T> suggestUsing(SuggestionBase suggestion);

        ISuggestionDocumentQuery<T> suggestUsing(Consumer<ISuggestionBuilder<T>> builder);
        //endregion

        //region suggest_2
        ISuggestionOperations<T> byField(String fieldName, String term);

        ISuggestionOperations<T> byField(String fieldName, String[] terms);

        ISuggestionOperations<T> withOptions(SuggestionOptions options);
        //endregion
    }

    private static class Employees_ByFullName extends AbstractIndexCreationTask {

    }

    private static class Employee {

    }

    public void sample() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region suggest_5
                SuggestionOptions options = new SuggestionOptions();
                options.setAccuracy(0.4f);
                options.setPageSize(5);
                options.setDistance(StringDistanceTypes.JARO_WINKLER);
                options.setSortMode(SuggestionSortMode.POPULARITY);

                Map<String, SuggestionResult> suggestions = session
                    .query(Employee.class, Employees_ByFullName.class)
                    .suggestUsing(builder ->
                        builder.byField("fullName", "johne")
                            .withOptions(options))
                    .execute();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region suggest_8
                SuggestionWithTerm suggestionWithTerm = new SuggestionWithTerm("fullName");
                suggestionWithTerm.setTerm("johne");

                Map<String, SuggestionResult> suggestions = session
                    .query(Employee.class, Employees_ByFullName.class)
                    .suggestUsing(suggestionWithTerm)
                    .execute();
                //endregion
            }
        }
    }
}
