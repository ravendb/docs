import { DocumentStore } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

{
    let docId, collectionName, clazz, docIdPrefix;
    //region document_changes_1
    store.changes().forDocument(docId);
    //endregion

    //region document_changes_3
    store.changes().forDocumentsInCollection(collectionName);
    store.changes().forDocumentsInCollection(clazz);
    //endregion

    //region document_changes_9
    store.changes().forDocumentsStartingWith(docIdPrefix);
    //endregion

    //region document_changes_1_1
    store.changes().forAllDocuments();
    //endregion
}

class Employee {}

async function examples() {
    {
        //region document_changes_2
        store.changes().forDocument("employees/1")
            .on("error", err => {
                //handle error 
            })
            .on("data", change => {
                switch (change.type) {
                    case "Put":
                        // do something
                        break;
                    case "Delete":
                        // do something
                        break;
                }
            });
        //endregion
    }

        //region document_changes_4
        store.changes().forDocumentsInCollection(Employee)
            .on("data", change => {
                console.log(change.type + " on document " + change.id);
            });
        //endregion

        //region document_changes_5
        const collectionName = store.conventions.getCollectionNameForType(Employee);
        store.changes()
            .forDocumentsInCollection(collectionName)
            .on("data", change => {
                console.log(change.type + " on document " + change.id);
            });
        //endregion

        //region document_changes_1_0
        store.changes()
            .forDocumentsStartingWith("employees/1") // employees/1, employees/10, employees/11, etc.
            .on("data", change => {
                console.log(change.type + " on document " + change.id);
            });
        //endregion

        //region document_changes_1_2
        store.changes().forAllDocuments()
            .on("data", change => {
                console.log(change.type + " on document " + change.id);
            });
        //endregion
}
