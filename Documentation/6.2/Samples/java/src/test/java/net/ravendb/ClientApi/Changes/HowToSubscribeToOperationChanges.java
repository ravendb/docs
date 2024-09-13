package net.ravendb.ClientApi.changes;

import com.fasterxml.jackson.databind.node.ObjectNode;
import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.changes.IChangesObservable;
import net.ravendb.client.documents.changes.Observers;
import net.ravendb.client.documents.changes.OperationStatusChange;
import net.ravendb.client.primitives.CleanCloseable;

public class HowToSubscribeToOperationChanges {

    private interface IFoo {
        //region operation_changes_1
        IChangesObservable<OperationStatusChange> forOperationId(long operationId);
        //endregion

        //region operation_changes_3
        IChangesObservable<OperationStatusChange> forAllOperations();
        //endregion
    }

    public HowToSubscribeToOperationChanges() {
        long operationId = 7;
        try (IDocumentStore store = new DocumentStore()) {
            //region operation_changes_2
            CleanCloseable subscription = store
                .changes()
                .forOperationId(operationId)
                .subscribe(Observers.create(change -> {
                    ObjectNode operationState = change.getState();

                    // do something
                }));
            //endregion
        }

        try (IDocumentStore store = new DocumentStore()) {
            //region operation_changes_4
            CleanCloseable subscription = store
                .changes()
                .forAllOperations()
                .subscribe(Observers.create(change -> {
                    System.out.println("Operation #" + change.getOperationId());
                }));
            //endregion
        }

    }
}
