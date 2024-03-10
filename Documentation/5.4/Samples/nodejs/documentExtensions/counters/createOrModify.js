import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function countersOverview() {
    {
        //region create_or_modify        
        // Open a session
        const session = documentStore.openSession();        
       
        // Pass a document ID to the countersFor constructor 
        const documentCounters = session.countersFor("products/1-A");

        // Use `countersFor.increment`:
        // ============================

        // Increase "ProductLikes" by 1, or create it if doesn't exist with a value of 1
        documentCounters.increment("ProductLikes", 1); 
        
        // Increase "ProductPageViews" by 15, or create it if doesn't exist with a value of 15
        documentCounters.increment("ProductPageViews", 15);
        
        // Decrease "DaysLeftForSale" by 10, or create it if doesn't exist with a value of -10
        documentCounters.increment("DaysLeftForSale", -10);

        // Save changes
        await session.saveChanges();
        //endregion
    }
}

//region syntax
increment(counter);
increment(counter, delta);
//endregion
