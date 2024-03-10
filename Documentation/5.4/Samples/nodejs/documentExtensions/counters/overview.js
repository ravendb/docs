import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function countersOverview() {
    {
        //region overview_1
        // Use countersFor without loading a document:
        // ===========================================
        
        // Open a session
        const session = documentStore.openSession();        
       
        // Pass an explicit document ID to the countersFor constructor 
        const documentCounters = session.countersFor("products/1-A");

        // Use `CountersFor` methods to manage the document's Counters
        documentCounters.delete("ProductLikes"); // Delete the "ProductLikes" Counter
        documentCounters.increment("ProductModified", 15); // Add 15 to Counter "ProductModified"
        const counter = await documentCounters.get("DaysLeftForSale"); // Get value for "DaysLeftForSale"

        // Save changes
        await session.saveChanges();
        //endregion
    }
    {
        //region overview_2
        // Use countersFor by passing it a document entity:
        // ================================================

        // Open a session
        const session = documentStore.openSession();

        // Load a document
        const product = await session.load("products/1-A");

        // Pass the entity returned from session.load as a param.
        const documentCounters = session.countersFor(product);

        // Use `countersFor` methods to manage the document's Counters
        documentCounters.delete("ProductLikes"); // Delete the "ProductLikes" Counter
        documentCounters.increment("ProductModified", 15); // Add 15 to Counter "ProductModified"
        const counter = await documentCounters.get("DaysLeftForSale"); // Get value for "DaysLeftForSale"

        // Save changes
        await session.saveChanges();
        //endregion
    }
}
