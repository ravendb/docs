package net.ravendb.ClientApi.Operations;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.IndexErrors;
import net.ravendb.client.documents.indexes.IndexingError;
import net.ravendb.client.documents.operations.indexes.GetIndexErrorsOperation;

import java.util.Date;

public class IndexError {

    private interface IFoo {
        /*
        //region errors_1
        public GetIndexErrorsOperation()

        public GetIndexErrorsOperation(String[] indexNames)
        //endregion
        */
    }

    private class Foo {
        //region errors_2
        public class IndexErrors {
            private String name;
            private IndexingError[] errors;

            public IndexErrors() {
                errors = new IndexingError[0];
            }

            public String getName() {
                return name;
            }

            public void setName(String name) {
                this.name = name;
            }

            public IndexingError[] getErrors() {
                return errors;
            }

            public void setErrors(IndexingError[] errors) {
                this.errors = errors;
            }
        }
        //endregion

        //region errors_3

        public class IndexingError {

            private String error;
            private Date timestamp;
            private String document;
            private String action;

            public String getError() {
                return error;
            }

            public void setError(String error) {
                this.error = error;
            }

            public Date getTimestamp() {
                return timestamp;
            }

            public void setTimestamp(Date timestamp) {
                this.timestamp = timestamp;
            }

            public String getDocument() {
                return document;
            }

            public void setDocument(String document) {
                this.document = document;
            }

            public String getAction() {
                return action;
            }

            public void setAction(String action) {
                this.action = action;
            }
        }
        //endregion
    }

    public IndexError() {
        try (IDocumentStore store = new DocumentStore()) {
            {
                //region errors_4
                // gets errors for all indexes
                IndexErrors[] indexErrors
                    = store.maintenance().send(new GetIndexErrorsOperation());
                //endregion
            }

            {
                //region errors_5
                // gets errors only for 'Orders/Totals' index
                IndexErrors[] indexErrors
                    = store.maintenance()
                        .send(new GetIndexErrorsOperation(new String[]{"Orders/Totals"}));
                //endregion
            }
        }
    }
}
