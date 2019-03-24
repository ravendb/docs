package net.ravendb.ClientApi.Operations;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.operations.etl.ResetEtlOperation;

public class ResetEtl {

    private interface IFoo {
        /*
        //region reset_etl_1
        public ResetEtlOperation(String configurationName, String transformationName);
        //endregion
        */
    }

    public ResetEtl() {
        try (IDocumentStore store = new DocumentStore()) {
            //region reset_etl_2
            ResetEtlOperation resetEtlOperation = new ResetEtlOperation("OrdersExport", "script1");
            store.maintenance().send(resetEtlOperation);
            //endregion
        }
    }
}
