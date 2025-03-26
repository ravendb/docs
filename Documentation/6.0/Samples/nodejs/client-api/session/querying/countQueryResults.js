import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function countQueryResults() {

    {
        //region count
        const numberOfOrders = await session
            .query({ collection: "Orders" })
            .whereEquals("ShipTo.Country", "UK")
             // Call 'count' to get the number of results
            .count();
        
        // The query returns the NUMBER of orders shipped to UK
        //endregion
    }
}
