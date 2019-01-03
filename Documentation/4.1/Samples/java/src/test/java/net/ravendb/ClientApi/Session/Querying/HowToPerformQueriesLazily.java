package net.ravendb.ClientApi.Session.Querying;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.Lazy;
import net.ravendb.client.documents.queries.Query;
import net.ravendb.client.documents.queries.facets.FacetResult;
import net.ravendb.client.documents.queries.suggestions.SuggestionResult;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.List;
import java.util.Map;
import java.util.function.Consumer;

public class HowToPerformQueriesLazily {

    private interface IFoo<T> {
        //region lazy_1
        Lazy<List<T>> lazily();

        Lazy<List<T>> lazily(Consumer<List<T>> onEval);
        //endregion

        //region lazy_4
        Lazy<Integer> countLazily();
        //endregion

        //region lazy_6
        Lazy<Map<String, SuggestionResult>> executeLazy();

        Lazy<Map<String, SuggestionResult>> executeLazy(Consumer<Map<String, SuggestionResult>> onEval);
        //endregion
    }

    private interface IFoo2<T> {
        //region lazy_8
        Lazy<Map<String, FacetResult>> executeLazy();

        Lazy<Map<String, FacetResult>> executeLazy(Consumer<Map<String, FacetResult>> onEval);
        //endregion
    }

    private static class Employee {

    }

    private static class Camera  {

    }

    public HowToPerformQueriesLazily() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region lazy_2
                Lazy<List<Employee>> employeesLazy = session
                    .query(Employee.class)
                    .whereEquals("firstName", "Robert")
                    .lazily();

                List<Employee> employees = employeesLazy.getValue(); // query will be executed here
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region lazy_5
                Lazy<Integer> countLazy = session
                    .query(Employee.class)
                    .whereEquals("firstName", "Robert")
                    .countLazily();

                Integer count = countLazy.getValue(); // query will be executed here
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region lazy_7
                Lazy<Map<String, SuggestionResult>> suggestLazy = session
                    .query(Employee.class, Query.index("Employees_ByFullName"))
                    .suggestUsing(builder -> builder.byField("fullName", "johne"))
                    .executeLazy();

                Map<String, SuggestionResult> suggest = suggestLazy.getValue(); // query will be executed here
                List<String> suggestions = suggest.get("fullName").getSuggestions();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region lazy_9
                Lazy<Map<String, FacetResult>> facetsLazy = session
                    .query(Camera.class, Query.index("Camera/Costs"))
                    .aggregateUsing("facets/CameraFacets")
                    .executeLazy();

                Map<String, FacetResult> facets = facetsLazy.getValue(); // query will be executed here
                FacetResult results = facets.get("manufacturer");
                //endregion
            }
        }
    }
}
