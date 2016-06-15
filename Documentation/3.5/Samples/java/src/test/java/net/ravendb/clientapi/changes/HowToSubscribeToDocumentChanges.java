package net.ravendb.clientapi.changes;

import java.io.Closeable;

import net.ravendb.abstractions.closure.Action1;
import net.ravendb.abstractions.data.DocumentChangeNotification;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.changes.IObservable;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.utils.Observers;
import net.ravendb.samples.northwind.Employee;


public class HowToSubscribeToDocumentChanges {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region document_changes_1
    public IObservable<DocumentChangeNotification> forDocument(String docId);
    //endregion

    //region document_changes_3
    public IObservable<DocumentChangeNotification> forDocumentsInCollection(String collectionName);

    public IObservable<DocumentChangeNotification> forDocumentsInCollection(Class<?> clazz);
    //endregion

    //region document_changes_6
    public IObservable<DocumentChangeNotification> forDocumentsOfType(String typeName);

    public IObservable<DocumentChangeNotification> forDocumentsOfType(Class<?> clazz);
    //endregion

    //region document_changes_9
    public IObservable<DocumentChangeNotification> forDocumentsStartingWith(String docIdPrefix);
    //endregion

    //region document_changes_1_1
    public IObservable<DocumentChangeNotification> forAllDocuments();
    //endregion
  }

  @SuppressWarnings("unused")
  public HowToSubscribeToDocumentChanges() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region document_changes_2
      Closeable subscription = store
        .changes()
        .forDocument("employees/1")
        .subscribe(Observers.create(new Action1<DocumentChangeNotification>() {
          @Override
          public void apply(DocumentChangeNotification change) {
            switch (change.getType()) {
              case PUT:
                // do something
                break;
              case DELETE:
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
      //region document_changes_4
      Closeable subscription = store
        .changes()
        .forDocumentsInCollection(Employee.class)
        .subscribe(Observers.create(new Action1<DocumentChangeNotification>() {
          @Override
          public void apply(DocumentChangeNotification change) {
            System.out.println(change.getType() + " on document " + change.getId());
          }
        }));
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region document_changes_5
      String collectionName = store.getConventions().getTypeTagName(Employee.class);
      Closeable subscription = store
        .changes()
        .forDocumentsInCollection(collectionName)
        .subscribe(Observers.create(new Action1<DocumentChangeNotification>() {
          @Override
          public void apply(DocumentChangeNotification change) {
            System.out.println(change.getType() + " on document " + change.getId());
          }
        }));
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region document_changes_7
      Closeable subscription = store
        .changes()
        .forDocumentsOfType(Employee.class)
        .subscribe(Observers.create(new Action1<DocumentChangeNotification>() {
          @Override
          public void apply(DocumentChangeNotification change) {
            System.out.println(change.getType() + " on document " + change.getId());
          }
        }));
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region document_changes_8
      String typeName = store.getConventions().getFindJavaClassName().find(Employee.class);
      Closeable subscription = store
        .changes()
        .forDocumentsOfType(typeName)
        .subscribe(Observers.create(new Action1<DocumentChangeNotification>() {
          @Override
          public void apply(DocumentChangeNotification change) {
            System.out.println(change.getType() + " on document " + change.getId());
          }
        }));
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region document_changes_1_0
      Closeable subscription = store
        .changes()
        .forDocumentsStartingWith("employees/1") // employees/1, employees/10, employees/11, etc.
        .subscribe(Observers.create(new Action1<DocumentChangeNotification>() {
          @Override
          public void apply(DocumentChangeNotification change) {
            System.out.println(change.getType() + " on document " + change.getId());
          }
        }));
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region document_changes_1_2
      Closeable subscription = store
        .changes()
        .forAllDocuments() // employees/1, orders/1, customers/1, etc.
        .subscribe(Observers.create(new Action1<DocumentChangeNotification>() {
          @Override
          public void apply(DocumentChangeNotification change) {
            System.out.println(change.getType() + " on document " + change.getId());
          }
        }));
      //endregion
    }
  }
}
