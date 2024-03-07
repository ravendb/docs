import { DocumentStore } from "ravendb";
import assert from "assert";

async function disableCaching() {
    
    {
        const documentStore = new DocumentStore();

        //region disable_caching
        // Define the session's options object
        const sessionOptions: SessionOptions = {
            noCaching: true // Disable caching
        };

        // Open the session, pass the options object 
        const session = store.openSession(sessionOptions);
        
        // The session will not cache any HTTP response data from the server
        //endregion
    }
    
}

