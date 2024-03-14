package net.ravendb.ClientApi.Session.Querying;

import com.fasterxml.jackson.databind.node.ObjectNode;
import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.queries.IndexQuery;
import net.ravendb.client.documents.queries.ProjectionBehavior;
import net.ravendb.client.documents.queries.QueryResult;
import net.ravendb.client.documents.session.IDocumentQuery;
import net.ravendb.client.documents.session.IDocumentQueryCustomization;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.primitives.Reference;

import java.time.Duration;
import java.util.List;
import java.util.concurrent.atomic.AtomicReference;
import java.util.function.Consumer;

public class HowToCustomize {

    private interface IFoo<T> {
        //region customize_1_0
        IDocumentQueryCustomization addBeforeQueryExecutedListener(Consumer<IndexQuery> action);
        IDocumentQueryCustomization removeBeforeQueryExecutedListener(Consumer<IndexQuery> action);
        //endregion

        //region customize_1_0_0
        IDocumentQueryCustomization addAfterQueryExecutedListener(Consumer<QueryResult> action);
        IDocumentQueryCustomization removeAfterQueryExecutedListener(Consumer<QueryResult> action);
        //endregion


        //region customize_1_0_1
        IDocumentQueryCustomization addAfterStreamExecutedListener(Consumer<ObjectNode> action);
        IDocumentQueryCustomization removeAfterStreamExecutedListener(Consumer<ObjectNode> action);
        //endregion


        //region customize_2_0
        IDocumentQueryCustomization noCaching();
        //endregion

        //region customize_3_0
        IDocumentQueryCustomization noTracking();
        //endregion

        //region customize_4_0
        IDocumentQueryCustomization randomOrdering();

        IDocumentQueryCustomization randomOrdering(String seed);
        //endregion

        //region customize_8_0
        IDocumentQueryCustomization waitForNonStaleResults();

        IDocumentQueryCustomization waitForNonStaleResults(Duration waitTimeout);
        //endregion

        //region projectionbehavior
        IDocumentQueryCustomization projection(ProjectionBehavior projectionBehavior);

        public enum ProjectionBehavior {
            DEFAULT,
            FROM_INDEX,
            FROM_INDEX_OR_THROW,
            FROM_DOCUMENT,
            FROM_DOCUMENT_OR_THROW
        }
        //endregion
    }

    private static class Employee {
    }

    public HowToCustomize() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region customize_1_1
                session.advanced().addBeforeQueryListener(
                    (sender, event) -> event.getQueryCustomization().addBeforeQueryExecutedListener(
                        // set 'pageSize' to 10
                        q -> q.setPageSize(10)));

                session.query(Employee.class).toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region customize_1_1_0
                AtomicReference<Duration> queryDuration = new AtomicReference<>();

                session.query(Employee.class)
                    .addAfterQueryExecutedListener(result -> {
                        queryDuration.set(Duration.ofMillis(result.getDurationInMs()));
                    })
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region customize_1_1_1
                Reference<Long> totalStreamedResultsSize = new Reference<Long>(0L);

                session.query(Employee.class)
                    .addAfterStreamExecutedListener(result -> {
                        totalStreamedResultsSize.value += result.size();
                    })
                    .toList();

                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region customize_2_1
                session.advanced().addBeforeQueryListener(
                    ((sender, event) -> event.getQueryCustomization().noCaching()));

                List<Employee> results = session.query(Employee.class)
                    .whereEquals("FirstName", "Robert")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region customize_3_1
                session.advanced().addBeforeQueryListener(
                    ((sender, event) -> event.getQueryCustomization().noTracking()));

                List<Employee> results = session.query(Employee.class)
                    .whereEquals("FirstName", "Robert")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region customize_4_1
                session.advanced().addBeforeQueryListener(
                    (sender, event) -> event.getQueryCustomization().randomOrdering());

                //result will be ordered randomly each time
                List<Employee> results = session.query(Employee.class)
                    .whereEquals("FirstName", "Robert")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region customize_8_1
                session.advanced().addBeforeQueryListener(
                    (sender, event) -> event.getQueryCustomization().waitForNonStaleResults());

                List<Employee> results = session.query(Employee.class)
                    .whereEquals("FirstName", "Robert")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {

                //region projectionbehavior_query
                session.advanced().addBeforeQueryListener((sender, event)
                    -> event.getQueryCustomization().projection(ProjectionBehavior.DEFAULT));

                List<Employee> results = session.query(Employee.class)
                    .selectFields(Employee.class,"name")
                    .toList();
                //endregion

                session.saveChanges();
            }

        }
    }
}
