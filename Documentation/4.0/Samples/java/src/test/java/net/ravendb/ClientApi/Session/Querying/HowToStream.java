package net.ravendb.ClientApi.Session.Querying;

import net.ravendb.Indexes.Querying.Sorting;
import net.ravendb.client.documents.CloseableIterator;
import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.commands.StreamResult;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.session.IDocumentQuery;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.documents.session.IRawDocumentQuery;
import net.ravendb.client.documents.session.StreamQueryStatistics;
import net.ravendb.client.primitives.Reference;

public class HowToStream {

    private interface IFoo {
        //region stream_1
        <T> CloseableIterator<StreamResult<T>> stream(IDocumentQuery<T> query);

        <T> CloseableIterator<StreamResult<T>> stream(IDocumentQuery<T> query, Reference<StreamQueryStatistics> streamQueryStats);

        <T> CloseableIterator<StreamResult<T>> stream(IRawDocumentQuery<T> query);

        <T> CloseableIterator<StreamResult<T>> stream(IRawDocumentQuery<T> query, Reference<StreamQueryStatistics> streamQueryStats);
        //endregion
    }

    public void examples() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region stream_2
                IDocumentQuery<Employee> query = session
                    .query(Employee.class, Employees_ByFirstName.class)
                    .whereEquals("firstName", "Robert");

                CloseableIterator<StreamResult<Employee>> results = session.advanced().stream(query);

                while (results.hasNext()) {
                    StreamResult<Employee> employee = results.next();
                }
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region stream_3
                IDocumentQuery<Employee> query = session
                    .advanced()
                    .documentQuery(Employee.class)
                    .whereEquals("firstName", "Robert");

                Reference<StreamQueryStatistics> streamQueryStatsRef = new Reference<>();
                CloseableIterator<StreamResult<Employee>> results = session.advanced().stream(query, streamQueryStatsRef);

                while (results.hasNext()) {
                    StreamResult<Employee> employee = results.next();
                }
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region stream_4
                IRawDocumentQuery<Employee> query = session.advanced()
                    .rawQuery(Employee.class, "from Employees where firstName = 'Robert'");

                CloseableIterator<StreamResult<Employee>> results = session.advanced().stream(query);

                while (results.hasNext()) {
                    StreamResult<Employee> employee = results.next();
                }
                //endregion
            }
        }
    }

    private static class Employee {

    }

    public static class Employees_ByFirstName extends AbstractIndexCreationTask {
        public Employees_ByFirstName() {
            map = "from employee in docs.Employees " +
                "select new { " +
                "   employee.firstName " +
                "} ";
        }
    }
}
