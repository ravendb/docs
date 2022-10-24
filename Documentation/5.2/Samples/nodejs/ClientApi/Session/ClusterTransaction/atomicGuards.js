import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function examples() {
    {
        //region atomic-guards-enabled
        let user = {
            firstName: 'John',
            lastName: 'Doe'
        };
        
        // Open a cluster-wide session
        const session = documentStore.openSession({
            transactionMode: "ClusterWide"
        });

        await session.store(user, "users/johndoe");
        await session.saveChanges();
        // An atomic-guard is now automatically created for the new document "users/johndoe".
        
        // Open two more cluster-wide sessions
        const session1 = documentStore.openSession({
            transactionMode: "ClusterWide"
        });        
        const session2 = documentStore.openSession({
            transactionMode: "ClusterWide"
        });

        // The two sessions will load the same document at the same time
        const loadedUser1  = await session1.load("users/johndoe");
        loadedUser1.name = "jindoe";

        const loadedUser2  = await session2.load("users/johndoe");
        loadedUser2.name = "jandoe";
        
        // session1 will save changes first, which triggers a change in the 
        // version number of the associated atomic-guard.
        await session1.saveChanges();
        
        // session2.saveChanges() will be rejected with a ClusterTransactionConcurrencyException
        // since session1 already changed the atomic-guard version,
        // and session2 saveChanges uses the document version that it had when it loaded the document.
        await session2.saveChanges();        
        //endregion

        //region atomic-guards-disabled

        // Open a cluster-wide session
        const session = documentStore.openSession({
            transactionMode: "ClusterWide",
            // Disable atomic-guards
            disableAtomicDocumentWritesInClusterWideTransaction: true
        });

        await session.store(user, "users/johndoe");
        
        // No atomic-guard will be created upon saveChanges
        await session.saveChanges();
        //endregion
    }
}
