package net.ravendb.ClientApi.Changes;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.changes.IDatabaseChanges;
import net.ravendb.client.documents.changes.Observers;

public class WhatIsChangesApi {

    private interface IFoo {
        //region changes_1
        IDatabaseChanges changes();

        IDatabaseChanges changes(String database);
        //endregion
    }

    public WhatIsChangesApi() {
        try (IDocumentStore store = new DocumentStore()) {
            //region changes_2
            IDatabaseChanges subscription = store.changes();

            subscription.ensureConnectedNow();

            subscription.forAllDocuments().subscribe(Observers.create(change -> {
                System.out.println(change.getType() + " on document " + change.getId());
            }));

            try {
                // application code here
            } finally {
                if (subscription != null) {
                    subscription.close();
                }
            }
            //endregion
        }
    }
}
