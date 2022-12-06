import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function getStats() {
    {
        //region stats_1
        // Pass an instance of class `GetCollectionStatisticsOperation` to the store 
        const stats = await store.maintenance.send(new GetCollectionStatisticsOperation());
        //endregion
    }
    {
        //region stats_2
        // Pass an instance of class `GetDetailedCollectionStatisticsOperation` to the store 
        const stats = await store.maintenance.send(new GetDetailedCollectionStatisticsOperation());
        //endregion
    }
    {
        //region stats_3
        // Pass an instance of class `GetStatisticsOperation` to the store 
        const stats = await store.maintenance.send(new GetStatisticsOperation());
        //endregion
    }
    {
        //region stats_4
        // Pass an instance of class `GetDetailedStatisticsOperation` to the store 
        const stats = await store.maintenance.send(new GetDetailedStatisticsOperation());
        //endregion
    }
}

//region stats_1_results
// Collection stats results:
interface CollectionStatistics {
    // Total # of documents in all collections
    countOfDocuments;
    // Total # of conflicts
    countOfConflicts;
    // Dictionary with total # of documents per collection
    collections;
}
//endregion

//region stats_2_results
// Detailed collection stats results:
interface DetailedCollectionStatistics {
    // Total # of documents in all collections
    countOfDocuments;
    // Total # of conflicts
    countOfConflicts;
    // Dictionary with collection details per collection
    collections;
}

// Details per collection
interface CollectionDetails {
    name;
    countOfDocuments;
    size;
    documentsSize;
    tombstonesSize;
    revisionsSize;
}
//endregion

//region stats_3_results
// Database stats results:
interface DatabaseStatistics {
    lastDocEtag;      // Last document etag in database
    lastDatabaseEtag; // Last database etag
    
    countOfIndexes;            // Total # of indexes in database
    countOfDocuments;          // Total # of documents in database
    countOfRevisionDocuments;  // Total # of revision documents in database
    countOfDocumentsConflicts; // Total # of documents conflicts in database 
    countOfTombstones;         // Total # of tombstones in database
    countOfConflicts;          // Total # of conflicts in database
    countOfAttachments;        // Total # of attachments in database
    countOfUniqueAttachments;  // Total # of unique attachments in database
    countOfCounterEntries;     // Total # of counter-group entries in database
    countOfTimeSeriesSegments; // Total # of time-series segments in database
    
    indexes; // Statistics for each index in database (array of IndexInformation) 

    databaseChangeVector;  // Global change vector of the database
    databaseId;            // Database identifier
    is64Bit;               // Indicates if process is 64-bit 
    pager;                 // Component handling the memory-mapped files
    lastIndexingTime;      // Last time of indexing an item
    sizeOnDisk;            // Database size on disk
    tempBuffersSizeOnDisk; // Temp buffers size on disk
    numberOfTransactionMergerQueueOperations;
}
//endregion

//region stats_4_results
// Detailed database stats results:
interface DetailedDatabaseStatistics extends DatabaseStatistics {
    // Total # of identities in database
    countOfIdentities;
    // Total # of compare-exchange items in database
    countOfCompareExchange;
    // Total # of cmpXchg tombstones in database
    countOfCompareExchangeTombstones;
    // Total # of TS deleted ranges values in database
    countOfTimeSeriesDeletedRanges;
}
//endregion
