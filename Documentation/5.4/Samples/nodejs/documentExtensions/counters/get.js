import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function getCounters() {
    {
        //region getCounter
        // Open a session
        const session = documentStore.openSession();        
       
        // Pass a document ID to the countersFor constructor 
        const documentCounters = session.countersFor("products/1-A");

        // Call `get` to retrieve a Counter's value
        const daysLeft = await documentCounters.get("DaysLeftForSale");

        console.log("Days Left For Sale: " + daysLeft);
        //endregion
    }
    {
        //region getAllCounters
        // Open a session
        const session = documentStore.openSession();

        // Pass a document ID to the countersFor constructor 
        const documentCounters = session.countersFor("products/1-A");

        // Call `getAll` to retrieve all of the document's Counters' names and values
        const allCounters = await documentCounters.getAll();

        for (var counter in allCounters) {
            console.log("counter name: " + counter + ", counter value: " + allCounters[counter]);
        }
        //endregion
    }
}

//region syntax_1
get(counter);
//endregion

//region syntax_2
getAll(counters);
//endregion
