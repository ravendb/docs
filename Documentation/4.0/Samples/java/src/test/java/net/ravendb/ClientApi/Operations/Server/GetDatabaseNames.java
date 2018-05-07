package net.ravendb.ClientApi.Operations.Server;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.serverwide.operations.GetDatabaseNamesOperation;

public class GetDatabaseNames {


    private interface IFoo {
        /*
        //region get_db_names_interface
        public GetDatabaseNamesOperation(int start, int pageSize)
        //endregion
        */
    }

    public GetDatabaseNames() {
        try (IDocumentStore store = new DocumentStore()) {
            //region get_db_names_sample
            GetDatabaseNamesOperation operation = new GetDatabaseNamesOperation(0, 25);
            String[] databaseNames = store.maintenance().server().send(operation);
            //endregion
        }
    }
}
