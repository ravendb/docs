package net.ravendb.ClientApi.Operations;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;

import net.ravendb.client.documents.operations.CollectionStatistics;
import net.ravendb.client.documents.operations.GetCollectionStatisticsOperation;

import net.ravendb.client.documents.operations.DetailedCollectionStatistics;
import net.ravendb.client.documents.operations.GetDetailedCollectionStatisticsOperation;

import net.ravendb.client.documents.operations.DatabaseStatistics;
import net.ravendb.client.documents.operations.GetStatisticsOperation;

import net.ravendb.client.documents.operations.DetailedDatabaseStatistics;
import net.ravendb.client.documents.operations.GetDetailedStatisticsOperation;

public class GetStats {

    public DetailedStatistics() {
    
        try (IDocumentStore store = new DocumentStore()) {
            //region stats_1
            // Pass an instance of class `GetCollectionStatisticsOperation` to the store 
            CollectionStatistics stats =
                store.maintenance().send(new GetCollectionStatisticsOperation());
            //endregion
        }
        
        try (IDocumentStore store = new DocumentStore()) {
            //region stats_2
            // Pass an instance of class `GetDetailedCollectionStatisticsOperation` to the store 
            DetailedCollectionStatistics stats =
                store.maintenance().send(new GetDetailedCollectionStatisticsOperation());
            //endregion
        }
        
        try (IDocumentStore store = new DocumentStore()) {
            //region stats_3
            // Pass an instance of class `GetStatisticsOperation` to the store 
            DatabaseStatistics stats =
                store.maintenance().send(new GetStatisticsOperation());
            //endregion
        }
    
        try (IDocumentStore store = new DocumentStore()) {
            //region stats_4
            // Pass an instance of class `GetDetailedStatisticsOperation` to the store 
            DetailedDatabaseStatistics stats =
                store.maintenance().send(new GetDetailedStatisticsOperation());
            //endregion
        }
        
        try (IDocumentStore store = new DocumentStore()) {
            //region stats_5
            // Get stats for 'AnotherDatabase':
            DatabaseStatistics stats =
                store.maintenance().forDatabase("AnotherDatabase").send(new GetStatisticsOperation());
            //endregion
        }
    }
    
    /*
    //region stats_1_results
    public class CollectionStatistics {
    // Collection stats results:
        // Total # of documents in all collections
        int CountOfDocuments;
        // Total # of conflicts
        int CountOfConflicts;
        // Total # of documents per collection
        Map<String, Long> Collections;
    }
    //endregion
    */
    
    /*
    //region stats_2_results
    // Detailed collection stats results:
    public class DetailedCollectionStatistics  {
        // Total # of documents in all collections
        long CountOfDocuments;
        // Total # of conflicts
        long CountOfConflicts;
        // Collection details per collection
        Map<String, CollectionDetails> Collections;
    }
    
    // Details per collection
    public class CollectionDetails {
        String Name;
        long CountOfDocuments;
        Size Size;
        Size DocumentsSize;
        Size TombstonesSize;
        Size RevisionsSize;
    }
    //endregion
    */
    
    /*
    //region stats_3_results
    // Database stats results:
    public class DatabaseStatistics {
        Long LastDocEtag;       // Last document etag in database
        Long LastDatabaseEtag;  // Last database etag
        
        int CountOfIndexes;     // Total # of indexes in database
        long CountOfDocuments;  // Total # of documents in database
        long CountOfRevisionDocuments;  // Total # of revision documents in database
        long CountOfDocumentsConflicts; // Total # of documents conflicts in database
        long CountOfTombstones;         // Total # of tombstones in database
        long CountOfConflicts;          // Total # of conflicts in database
        long CountOfAttachments;        // Total # of attachments in database
        long CountOfUniqueAttachments;  // Total # of unique attachments in database
        long CountOfCounterEntries;     // Total # of counter-group entries in database
        long CountOfTimeSeriesSegments; // Total # of time-series segments in database
    
        IndexInformation[] Indexes;     // Statistics for each index in database
    
        String DatabaseChangeVector;    // Global change vector of the database
        String DatabaseId;              // Database identifier
        boolean Is64Bit;                // Indicates if process is 64-bit
        String Pager;                   // Component handling the memory-mapped files
        Date LastIndexingTime;          // Last time of indexing an item
        Size SizeOnDisk;                // Database size on disk
        Size TempBuffersSizeOnDisk;     // Temp buffers size on disk
        int NumberOfTransactionMergerQueueOperations;
    }
    //endregion
    */
    
    /*
    //region stats_4_results
    // Detailed database stats results:
    public class DetailedDatabaseStatistics extends DatabaseStatistics {
        // Total # of identities in database
        long CountOfIdentities;
        // Total # of compare-exchange items in database
        long CountOfCompareExchange;
        // Total # of cmpXchg tombstones in database
        long CountOfCompareExchangeTombstones;
        // Total # of TS deleted ranges values in database
        long CountOfTimeSeriesDeletedRanges;
    }
    //endregion
    */
}
