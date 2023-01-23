package net.ravendb.ClientApi.Operations.Indexes;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.IndexDefinition;
import net.ravendb.client.documents.operations.indexes.GetIndexNamesOperation;
import net.ravendb.client.documents.operations.indexes.GetIndexOperation;
import net.ravendb.client.documents.operations.indexes.GetIndexesOperation;

public class Get {

    private interface IFoo {
        /*
        //region get_1_0
        public GetIndexOperation(String indexName)
        //endregion

        //region get_2_0
        public GetIndexesOperation(int start, int pageSize)
        //endregion

        //region get_3_0
        public GetIndexNamesOperation(int start, int pageSize)
        //endregion
        */
    }

    public Get() {
        try (IDocumentStore store = new DocumentStore()) {
            //region get_1_1
            IndexDefinition index
                = store.maintenance()
                    .send(new GetIndexOperation("Orders/Totals"));

            //endregion

            //region get_2_1
            IndexDefinition[] indexes
                = store.maintenance()
                    .send(new GetIndexesOperation(0, 10));
            //endregion

            //region get_3_1
            String[] indexNames
                = store.maintenance()
                    .send(new GetIndexNamesOperation(0, 10));
            //endregion
        }
    }
}
