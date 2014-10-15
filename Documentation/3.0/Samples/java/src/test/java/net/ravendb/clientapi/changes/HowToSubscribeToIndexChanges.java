package net.ravendb.clientapi.changes;

import java.io.Closeable;

import net.ravendb.abstractions.closure.Action1;
import net.ravendb.abstractions.data.IndexChangeNotification;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.changes.IObservable;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.utils.Observers;


public class HowToSubscribeToIndexChanges {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region index_changes_1
    public IObservable<IndexChangeNotification> forIndex(String indexName);
    //endregion

    //region index_changes_3
    public IObservable<IndexChangeNotification> forAllIndexes();
    //endregion
  }

  @SuppressWarnings("unused")
  public HowToSubscribeToIndexChanges() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region index_changes_2
      Closeable subscription = store.
         changes().
         forIndex("Orders/All")
         .subscribe(Observers.create(new Action1<IndexChangeNotification>() {
          @Override
          public void apply(IndexChangeNotification change) {
            switch (change.getType()) {
              case INDEX_ADDED:
                // do something
                break;
              case INDEX_DEMOTED_TO_ABANDONED:
                // do something
                break;
              case INDEX_DEMOTED_TO_DISABLED:
                // do something
                break;
              case INDEX_DEMOTED_TO_IDLE:
                // do something
                break;
              case INDEX_MARKED_AS_ERRORED:
                // do something
                break;
              case INDEX_PROMOTED_FROM_IDLE:
                // do something
                break;
              case INDEX_REMOVED:
                // do something
                break;
              case MAP_COMPLETED:
                // do something
                break;
              case REDUCE_COMPLETED:
                // do something
                break;
              case REMOVE_FROM_INDEX:
                // do something
                break;
              default:
                break;
            }
          }
        }));
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region index_changes_4
      Closeable subscription = store.
        changes().
        forAllIndexes()
        .subscribe(Observers.create(new Action1<IndexChangeNotification>() {
         @Override
         public void apply(IndexChangeNotification change) {
           System.out.println(change.getType() + " on index " + change.getName());
         }
       }));
      //endregion
    }
  }
}
