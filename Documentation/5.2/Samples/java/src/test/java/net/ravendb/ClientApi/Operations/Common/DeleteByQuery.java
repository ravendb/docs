package net.ravendb.ClientApi.Operations;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.operations.DeleteByQueryOperation;
import net.ravendb.client.documents.operations.Operation;
import net.ravendb.client.documents.operations.OperationIdResult;
import net.ravendb.client.documents.queries.IndexQuery;
import net.ravendb.client.documents.queries.QueryOperationOptions;

public class DeleteByQuery {

    private static class Person_ByAge extends AbstractIndexCreationTask {

    }

    private static class Person {

    }

    private interface IFoo {
            /*
            //region delete_by_query
            public DeleteByQueryOperation(IndexQuery queryToDelete)

            public DeleteByQueryOperation(IndexQuery queryToDelete, QueryOperationOptions options)
            //endregion
            */
    }

    public DeleteByQuery() {
        try (IDocumentStore store = new DocumentStore()) {
            //region delete_by_query1
            // remove all documents from the server where Name == Bob using Person/ByName index
            store
                .operations()
                .send(new DeleteByQueryOperation(new IndexQuery("from Persons where name = 'Bob'")));
            //endregion

            //region delete_by_query2
            // remove all documents from the server where Age > 35 using Person/ByAge index
            store
                .operations()
                .send(new DeleteByQueryOperation(new IndexQuery("from 'Person/ByAge' where age < 35")));
            //endregion

            //region delete_by_query3
            // delete multiple docs with specific ids in a single run without loading them into the session
            Operation operation = store
                .operations()
                .sendAsync(new DeleteByQueryOperation(new IndexQuery(
                    "from People u where id(u) in ('people/1-A', 'people/3-A')"
                )));
            //endregion
        }

        try (IDocumentStore store = new DocumentStore()) {
            //region delete_by_query_wait_for_completion
            // remove all document from server where Name == Bob and Age >= 29 using People collection
            Operation operation = store.operations()
                .sendAsync(new DeleteByQueryOperation(new IndexQuery(
                    "from People where Name = 'Bob' and Age >= 29"
                )));

            operation.waitForCompletion();
            //endregion
        }
    }
}
