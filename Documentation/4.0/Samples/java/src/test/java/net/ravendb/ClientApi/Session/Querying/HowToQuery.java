package net.ravendb.ClientApi.Session.Querying;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.queries.Query;
import net.ravendb.client.documents.session.IDocumentQuery;
import net.ravendb.client.documents.session.IDocumentSession;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;

import static net.ravendb.client.documents.queries.Query.index;

public class HowToQuery {

    private static class Employees_ByName extends AbstractIndexCreationTask {

    }

    private static class Employee {

    }

    private interface IFoo {
        //region query_1_0
        <T> IDocumentQuery<T> query(Class<T> clazz);

        <T> IDocumentQuery<T> query(Class<T> clazz, Query collectionOrIndexName);

        <T, TIndex extends AbstractIndexCreationTask> IDocumentQuery<T> query(Class<T> clazz, Class<TIndex> indexClazz);
        //endregion
    }

    public HowToQuery() {

        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region query_1_1
                // load all entities from 'Employees' collection
                List<Employee> employees = session.query(Employee.class)
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region query_1_2
                // load all entities from 'Employees' collection
                // where FirstName equals 'Robert'
                List<Employee> employees = session.query(Employee.class)
                    .whereEquals("FirstName", "Robert")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region query_1_4
                // load all entities from 'Employees' collection
                // where firstName equals 'Robert'
                // using 'Employees/ByName' index
                List<Employee> employees = session.query(Employee.class, index("Employees/ByName"))
                    .whereEquals("FirstName", "Robert")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region query_1_5
                // load all entities from 'Employees' collection
                // where firstName equals 'Robert'
                // using 'Employees/ByName' index
                List<Employee> employees = session.query(Employee.class, Employees_ByName.class)
                    .whereEquals("FirstName", "Robert")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region query_1_6
                // load all employees hired between
                // 1/1/2002 and 12/31/2002
                List<Employee> employees = session.advanced().documentQuery(Employee.class)
                    .whereBetween("HiredAt",
                        LocalDate.of(2002, 1, 1), LocalDate.of(2002, 12, 31))
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region query_1_7
                // load all entities from 'Employees' collection
                // where FirstName equals 'Robert'
                List<Employee> employees = session.advanced()
                    .rawQuery(Employee.class,
                        "from Employees where FirstName = 'Robert'")
                    .toList();
                //endregion
            }

        }

    }
}
