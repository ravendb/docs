package net.ravendb.ClientApi.Operations;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.operations.identities.GetIdentitiesOperation;

import java.util.Map;

public class Identities {

    private interface IFoo {
        /*
        //region sample_1
        public GetIdentitiesOperation()
        //endregion
         */
    }

    public Identities() {
        try (IDocumentStore store = new DocumentStore()) {
            //region sample_2
            Map<String, Long> identities
                = store.maintenance().send(new GetIdentitiesOperation());
            //endregion
        }
    }
}
