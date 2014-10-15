package net.ravendb.clientapi.changes;

import java.io.Closeable;

import net.ravendb.abstractions.closure.Action1;
import net.ravendb.abstractions.data.ReplicationConflictNotification;
import net.ravendb.abstractions.data.ReplicationConflictTypes;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.changes.IObservable;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.utils.Observers;


public class HowToSubscribeToReplicationConflictsChanges {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region replication_conflicts_changes_1
    public IObservable<ReplicationConflictNotification> forAllReplicationConflicts();
    //endregion
  }

  @SuppressWarnings("unused")
  public HowToSubscribeToReplicationConflictsChanges() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region replication_conflicts_changes_2
      Closeable subscription = store
        .changes()
        .forAllReplicationConflicts()
        .subscribe(Observers.create(new Action1<ReplicationConflictNotification>() {
          @Override
          public void apply(ReplicationConflictNotification conflict) {
            if (ReplicationConflictTypes.DOCUMENT_REPLICATION_CONFLICT.equals(conflict.getItemType())) {
              System.out.println("Conflict detected for " + conflict.getId());
            }
          }
        }));
      //endregion
    }
  }
}
