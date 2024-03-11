import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function deleteCounter() {
    {
        //region delete
        // Open a session
        const session = documentStore.openSession();
       
        // Pass a document ID to the countersFor constructor 
        const documentCounters = session.countersFor("products/1-A");
        
        // Delete the "ProductLikes" Counter
        documentCounters.delete("ProductLikes");

        // The 'Delete' is executed upon calling saveChanges
        await session.saveChanges();
        //endregion
    }
}

//region syntax
delete(counter);
//endregion
