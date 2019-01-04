package net.ravendb.ClientApi.Session.Querying.DocumentQuery;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.session.IDocumentQuery;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.List;

public class WhatIsDocumentQuery {

    private class Employee {
    }

    private class MyCustomIndex extends AbstractIndexCreationTask {
    }

    private interface IFoo {
        //region document_query_1
        <T> IDocumentQuery<T> documentQuery(Class<T> clazz);

        <T> IDocumentQuery<T> documentQuery(Class<T> clazz, String indexName, String collectionName, boolean isMapReduce);
        //endregion
    }

    public WhatIsDocumentQuery() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region document_query_2
                // load all entities from 'Employees' collection
                List<Employee> employees = session
                    .advanced()
                    .documentQuery(Employee.class)
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region document_query_3
                // load all entities from 'Employees' collection
                // where firstName equals 'Robert'
                List<Employee> employees = session
                    .advanced()
                    .documentQuery(Employee.class)
                    .whereEquals("firstName", "Robert")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region document_query_4
                // load all entities from 'Employees' collection
                // where firstName equals 'Robert'
                // using 'My/Custom/Index'
                List<Employee> employees = session
                    .advanced()
                    .documentQuery(Employee.class, "My/Custom/Index", null, false)
                    .whereEquals("firstName", "Robert")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region document_query_5
                // load all entities from 'Employees' collection
                // where firstName equals 'Robert'
                // using 'My/Custom/Index'
                List<Employee> employees = session
                    .advanced()
                    .documentQuery(Employee.class, MyCustomIndex.class)
                    .whereEquals("firstName", "Robert")
                    .toList();
                //endregion
            }
        }
    }
}
