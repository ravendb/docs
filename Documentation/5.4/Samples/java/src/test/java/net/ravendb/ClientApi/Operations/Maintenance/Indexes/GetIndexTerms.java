package net.ravendb.ClientApi.Operations;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.operations.indexes.GetTermsOperation;

public class GetIndexTerms {

    private interface IFoo {
        /*
        //region get_terms1
        public GetTermsOperation(String indexName, String field, String fromValue)

        public GetTermsOperation(String indexName, String field, String fromValue, Integer pageSize)
        //endregion
        */
    }

    public GetIndexTerms() {
        try (IDocumentStore store = new DocumentStore()) {
            //region get_terms2
            String[] terms = store
                .maintenance()
                .send(
                    new GetTermsOperation("Orders/Totals", "Employee", null));
            //endregion
        }
    }
}
