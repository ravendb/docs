package net.ravendb.ClientApi.Changes;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.changes.CounterChange;
import net.ravendb.client.documents.changes.IChangesObservable;
import net.ravendb.client.documents.changes.Observers;

public class HowToSubscribeToCounterChanges {

    private interface IFoo {
        //region counter_changes_1
        IChangesObservable<CounterChange> forCounter(String counterName);
        //endregion

        //region counter_changes_2
        IChangesObservable<CounterChange> forCounterOfDocument(String documentId, String counterName);
        //endregion

        //region counter_changes_3
        IChangesObservable<CounterChange> forCountersOfDocument(String documentId);
        //endregion

        //region counter_changes_4
        IChangesObservable<CounterChange> forAllCounters();
        //endregion
    }

    public HowToSubscribeToCounterChanges() {
        try (IDocumentStore store = new DocumentStore()) {
            //region counter_changes_1_1
            store
                .changes()
                .forCounter("likes")
                .subscribe(Observers.create(change -> {
                    switch (change.getType()) {
                        case INCREMENT:
                            // do something ...
                            break;
                    }
                }));
            //endregion
        }

        try (IDocumentStore store = new DocumentStore()) {
            //region counter_changes_2_1
            store
                .changes()
                .forCounterOfDocument("companies/1-A", "likes")
                .subscribe(Observers.create(change -> {
                    switch (change.getType()) {
                        case INCREMENT:
                            // do something
                            break;
                    }
                }));
            //endregion
        }

        try (IDocumentStore store = new DocumentStore()) {
            //region counter_changes_3_1
            store
                .changes()
                .forCountersOfDocument("companies/1-A")
                .subscribe(Observers.create(change -> {
                    switch (change.getType()) {
                        case INCREMENT:
                            // do something ...
                            break;
                    }
                }));
            //endregion
        }

        try (IDocumentStore store = new DocumentStore()) {
            //region counter_changes_4_1
            store
                .changes()
                .forAllCounters()
                .subscribe(Observers.create(change -> {
                    switch (change.getType()) {
                        case INCREMENT:
                            // do something ...
                            break;
                    }
                }));
            //endregion
        }
    }
}
