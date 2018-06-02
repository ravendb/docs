package net.ravendb.ClientApi.changes;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.changes.DocumentChange;
import net.ravendb.client.documents.changes.IChangesObservable;
import net.ravendb.client.documents.changes.Observers;
import net.ravendb.client.primitives.CleanCloseable;

public class HowToSubscribeToDocumentChanges {

    private interface IFoo {
        //region document_changes_1
        IChangesObservable<DocumentChange> forDocument(String docId);
        //endregion

        //region document_changes_3
        IChangesObservable<DocumentChange> forDocumentsInCollection(String collectionName);

        IChangesObservable<DocumentChange> forDocumentsInCollection(Class<?> clazz);
        //endregion

        //region document_changes_6
        IChangesObservable<DocumentChange> forDocumentsOfType(String typeName);

        IChangesObservable<DocumentChange> forDocumentsOfType(Class<?> clazz);
        //endregion

        //region document_changes_9
        IChangesObservable<DocumentChange> forDocumentsStartingWith(String docIdPrefix);
        //endregion

        //region document_changes_1_1
        IChangesObservable<DocumentChange> forAllDocuments();
        //endregion
    }

    private static class Employee {

    }

    public HowToSubscribeToDocumentChanges() {
        try (IDocumentStore store = new DocumentStore()) {
            //region document_changes_2
            CleanCloseable subscription = store.changes()
                .forDocument("employees/1")
                .subscribe(Observers.create(change -> {
                    switch (change.getType()) {
                        case PUT:
                            // do something
                            break;
                        case DELETE:
                            // do something
                            break;
                    }
                }));
            //endregion
        }

        try (IDocumentStore store = new DocumentStore()) {
            //region document_changes_4
            CleanCloseable subscription = store
                .changes()
                .forDocumentsInCollection(Employee.class)
                .subscribe(Observers.create(change -> {
                    System.out.println(change.getType() + " on document " + change.getId());
                }));
            //endregion
        }

        try (IDocumentStore store = new DocumentStore()) {
            //region document_changes_5
            String collectionName = store.getConventions().getFindCollectionName().apply(Employee.class);
            CleanCloseable subscription = store
                .changes()
                .forDocumentsInCollection(collectionName)
                .subscribe(Observers.create(change -> {
                    System.out.println(change.getType() + " on document " + change.getId());
                }));
            //endregion
        }

        try (IDocumentStore store = new DocumentStore()) {
            //region document_changes_7
            CleanCloseable subscription = store
                .changes()
                .forDocumentsOfType(Employee.class)
                .subscribe(Observers.create(change -> {
                    System.out.println(change.getType() + " on document " + change.getId());
                }));
            //endregion
        }

        try (IDocumentStore store = new DocumentStore()) {
            //region document_changes_8
            String className = store.getConventions().getFindJavaClassName().apply(Employee.class);
            CleanCloseable subscription = store
                .changes()
                .forDocumentsOfType(className)
                .subscribe(Observers.create(change -> {
                    System.out.println(change.getType() + " on document " + change.getId());
                }));
            //endregion
        }

        try (IDocumentStore store = new DocumentStore()) {
            //region document_changes_1_0
            CleanCloseable subscription = store
                .changes()
                .forDocumentsStartingWith("employees/1") // employees/1, employees/10, employees/11, etc.
                .subscribe(Observers.create(change -> {
                    System.out.println(change.getType() + " on document " + change.getId());
                }));
            //endregion
        }

        try (IDocumentStore store = new DocumentStore()) {
            //region document_changes_1_2
            CleanCloseable subscription = store
                .changes()
                .forAllDocuments()
                .subscribe(Observers.create(change -> {
                    System.out.println(change.getType() + " on document " + change.getId());
                }));
            //endregion
        }
    }
}
