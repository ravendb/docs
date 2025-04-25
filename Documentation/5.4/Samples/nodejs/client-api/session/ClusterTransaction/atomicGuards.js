import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function examples() {
    {
        //region atomic_guards_enabled
        const user = {
            firstName: "John",
            lastName: "Doe"
        };

        // Open a cluster-wide session:
        const session = documentStore.openSession({
            transactionMode: "ClusterWide"
        });

        await session.store(user, "users/johndoe");
        await session.saveChanges();
        // An atomic-guard is now automatically created for the new document "users/johndoe".

        // Open two concurrent cluster-wide sessions:
        const session1 = documentStore.openSession({
            transactionMode: "ClusterWide"
        });
        const session2 = documentStore.openSession({
            transactionMode: "ClusterWide"
        });

        // Both sessions load the same document:
        const loadedUser1 = await session1.load("users/johndoe");
        loadedUser1.name = "jindoe";

        const loadedUser2 = await session2.load("users/johndoe");
        loadedUser2.name = "jandoe";

        // session1 saves its changes first —
        // this increments the Raft index of the associated atomic guard.
        await session1.saveChanges();

        // session2 tries to save using an outdated atomic guard version
        // and fails with a ConcurrencyException.
        await session2.saveChanges();
        //endregion
    }
    {
        //region atomic_guards_disabled
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
    {
        //region load_before_storing
        const session = documentStore.openSession({
            // Open a cluster-wide session
            transactionMode: "ClusterWide"
        });

        // Load the user document BEFORE creating or updating
        const user = await session.load("users/johndoe");
        
        if (!user) {
            // Document doesn't exist => create a new document
            const newUser = {
                name: "John Doe",
                // ... initialize other properties
            };
            
            // Store the new user document in the session
            await session.store(newUser, "users/johndoe");
            
        } else {
            // Document exists => apply your modifications
            user.name = "New name";
            // ... make any other updates

            // No need to call store() again
            // RavenDB tracks changes on loaded entities
        }

        // Commit your changes
        await session.saveChanges();
        //endregion
    }
}
