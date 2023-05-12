import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function documentsCompression() {
    {
        //region compress_all
        // Compression is configured by setting the database record 

        // Retrieve the database record
        const dbrecord = await store.maintenance.server.send(new GetDatabaseRecordOperation(store.database));

        // Set compression on ALL collections
        dbrecord.documentsCompression.compressAllCollections = true;

        // Update the the database record
        await store.maintenance.server.send(new UpdateDatabaseOperation(dbrecord, dbrecord.etag));
        //endregion
    }
    {
        //region compress_specific
        // Retrieve the database record
        const dbrecord = store.maintenance.server.send(new GetDatabaseRecordOperation(store.database));
        
        // Turn on compression for specific collections
        // Turn off compression for all revisions, on all collections
        dbrecord.documentsCompression = {
            collections: ["Orders", "Employees"], 
            compressRevisions: false
        };

        // Update the the database record
        store.maintenance.server.send(new UpdateDatabaseOperation(dbrecord, dbrecord.etag));
        //endregion
    }
}

{
    //region syntax
    // The documentsCompression object
    {
        collections;            // string[], List of collections to compress 
        compressRevisions;      // boolean, set to true to compress revisions 
        compressAllCollections; // boolean, set to true to compress all collections
    }
    //endregion
}
