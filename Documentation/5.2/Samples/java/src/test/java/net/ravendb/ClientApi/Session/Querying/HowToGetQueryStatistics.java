package net.ravendb.ClientApi.Session.Querying;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentQuery;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.documents.session.QueryStatistics;
import net.ravendb.client.primitives.Reference;

import java.util.List;

public class HowToGetQueryStatistics {
    private interface IFoo<T> {
        //region stats_1
        IDocumentQuery<T> statistics(Reference<QueryStatistics> stats);
        //endregion
    }

    public HowToGetQueryStatistics() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region stats_2
                Reference<QueryStatistics> stats = new Reference<>();

                List<Employee> employees = session.query(Employee.class)
                    .whereEquals("FirstName", "Robert")
                    .statistics(stats)
                    .toList();

                int totalResults = stats.value.getTotalResults();
                long durationInMs = stats.value.getDurationInMs();
                //endregion
            }
        }
    }

    private static class Employee {

    }
}
