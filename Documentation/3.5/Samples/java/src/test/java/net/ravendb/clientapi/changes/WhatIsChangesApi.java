package net.ravendb.clientapi.changes;

import java.io.Closeable;

import net.ravendb.abstractions.closure.Action1;
import net.ravendb.abstractions.data.DocumentChangeNotification;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.changes.IDatabaseChanges;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.utils.Observers;


public class WhatIsChangesApi {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region changes_1
    public IDatabaseChanges changes();

    public IDatabaseChanges changes(String database);
    //endregion
  }

  public WhatIsChangesApi() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region changes_2
      Closeable subscription = store
        .changes()
        .forAllDocuments()
        .subscribe(Observers.create(new Action1<DocumentChangeNotification>() {
          @Override
          public void apply(DocumentChangeNotification change) {
            System.out.println(change.getType() + " on document " + change.getId());
          }
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
