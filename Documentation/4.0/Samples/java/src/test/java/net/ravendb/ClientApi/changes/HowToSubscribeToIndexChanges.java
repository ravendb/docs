package net.ravendb.ClientApi.changes;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.changes.IChangesObservable;
import net.ravendb.client.documents.changes.IndexChange;
import net.ravendb.client.documents.changes.Observers;
import net.ravendb.client.primitives.CleanCloseable;

public class HowToSubscribeToIndexChanges {

    private interface IFoo {
        //region index_changes_1
        IChangesObservable<IndexChange> forIndex(String indexName);
        //endregion

        //region index_changes_3
        IChangesObservable<IndexChange> forAllIndexes();
        //endregion
    }

    public HowToSubscribeToIndexChanges() {
        try (IDocumentStore store = new DocumentStore()) {
            //region index_changes_2
            CleanCloseable subscription = store
                .changes()
                .forIndex("Orders/All")
                .subscribe(Observers.create(change -> {
                    switch (change.getType()) {
                        case NONE:
                            // do something
                            break;
                        case BATCH_COMPLETED:
                            // do something
                            break;
                        case INDEX_ADDED:
                            // do something
                            break;
                        case INDEX_REMOVED:
                            // do something
                            break;
                        case INDEX_DEMOTED_TO_IDLE:
                            // do something
                            break;
                        case INDEX_PROMOTED_FROM_IDLE:
                            // do something
                            break;
                        case INDEX_DEMOTED_TO_DISABLED:
                            // do something
                            break;
                        case INDEX_MARKED_AS_ERRORED:
                            // do something
                            break;
                        case SIDE_BY_SIDE_REPLACE:
                            // do something
                            break;
                        case RENAMED:
                            // do something
                            break;
                        case INDEX_PAUSED:
                            // do something
                            break;
                        case LOCK_MODE_CHANGED:
                            // do something
                            break;
                        case PRIORITY_CHANGED:
                            // do something
                            break;
                        default:
                            throw new IllegalArgumentException();
                    }
                }));
            //endregion
        }

        try (IDocumentStore store = new DocumentStore()) {
            //region index_changes_4
            CleanCloseable subscription = store
                .changes()
                .forAllIndexes()
                .subscribe(Observers.create(change -> {
                    System.out.println(change.getType() + " on index " + change.getName());
                }));
            //endregion
        }
    }
}
