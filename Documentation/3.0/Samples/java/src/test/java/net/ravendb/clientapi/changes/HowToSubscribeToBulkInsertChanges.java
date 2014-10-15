package net.ravendb.clientapi.changes;

import java.io.Closeable;
import java.util.UUID;

import net.ravendb.abstractions.closure.Action1;
import net.ravendb.abstractions.data.BulkInsertChangeNotification;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.changes.IObservable;
import net.ravendb.client.document.BulkInsertOperation;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.utils.Observers;
import net.ravendb.samples.northwind.Employee;


public class HowToSubscribeToBulkInsertChanges {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region bulk_insert_changes_1
    public IObservable<BulkInsertChangeNotification> forBulkInsert(UUID operationId);
    //endregion
  }

  public HowToSubscribeToBulkInsertChanges() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region bulk_insert_changes_2
      try (BulkInsertOperation bulkInsert = store.bulkInsert()) {
        Closeable subscription = store
          .changes()
          .forBulkInsert(bulkInsert.getOperationId())
          .subscribe(Observers.create(new Action1<BulkInsertChangeNotification>() {
            @Override
            public void apply(BulkInsertChangeNotification change) {
              switch (change.getType()) {
                case BULK_INSERT_STARTED:
                  // do something
                  break;
                case BULK_INSERT_ENDED:
                  // do something
                  break;
                case BULK_INSERT_ERROR:
                  // do something
                  break;
                default:
                  break;
              }
            }
          }));

        try {
          for (int i = 0; i < 1000 * 1000; i++) {
            Employee employee = new Employee();
            employee.setFirstName("FirstName #" + i);
            employee.setLastName("LastName #" + i);
            bulkInsert.store(employee);
          }
        } finally {
          if (subscription != null) {
            subscription.close();
          }
        }
      }
      //endregion
    }
  }
}
