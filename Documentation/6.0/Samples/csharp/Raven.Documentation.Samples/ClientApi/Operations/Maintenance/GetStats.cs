using Raven.Client.Documents;
using Raven.Client.Documents.Operations;

namespace Raven.Documentation.Samples.ClientApi.Operations.Maintenance
{
    public class GetStats
    {
        public GetStats()
        {
            using (var store = new DocumentStore())
            {
                #region stats_1
                // Pass an instance of class `GetCollectionStatisticsOperation` to the store 
                CollectionStatistics stats =
                    store.Maintenance.Send(new GetCollectionStatisticsOperation());
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region stats_2
                // Pass an instance of class `GetDetailedCollectionStatisticsOperation` to the store 
                DetailedCollectionStatistics stats =
                    store.Maintenance.Send(new GetDetailedCollectionStatisticsOperation());
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region stats_3
                // Pass an instance of class `GetStatisticsOperation` to the store 
                DatabaseStatistics stats =
                    store.Maintenance.Send(new GetStatisticsOperation());
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region stats_4
                // Pass an instance of class `GetDetailedStatisticsOperation` to the store 
                DetailedDatabaseStatistics stats =
                    store.Maintenance.Send(new GetDetailedStatisticsOperation());
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region stats_5
                // Get stats for 'AnotherDatabase':
                DatabaseStatistics stats =
                    store.Maintenance.ForDatabase("AnotherDatabase").Send(new GetStatisticsOperation());
                #endregion
            }
        }
    }
  
    /*
    #region stats_1_results
    // Collection stats results:
    public class CollectionStatistics
    {
        // Total # of documents in all collections
        public long CountOfDocuments { get; set; }
        // Total # of conflicts
        public long CountOfConflicts { get; set; }
        // Total # of documents per collection
        public Dictionary<string, long> Collections { get; set; }
    }
    #endregion
    */
    
    /*
    #region stats_2_results
    // Detailed collection stats results:
    public class DetailedCollectionStatistics
    {
        // Total # of documents in all collections
        public long CountOfDocuments { get; set; }
        // Total # of conflicts
        public long CountOfConflicts { get; set; }
        // Collection details per collection
        public Dictionary<string, CollectionDetails> Collections { get; set; }
    }
    
    // Details per collection
    public class CollectionDetails
    {        
        public string Name { get; set; }
        public long CountOfDocuments { get; set; }
        public Size Size { get; set; }
        public Size DocumentsSize { get; set; }
        public Size TombstonesSize { get; set; }
        public Size RevisionsSize { get; set; }
    }
    #endregion
    */
    
    /*
    #region stats_3_results
    // Database stats results:
    public class DatabaseStatistics
    {
        public long? LastDocEtag { get; set; }      // Last document etag in database
        public long? LastDatabaseEtag { get; set; } // Last database etag
        
        public int CountOfIndexes { get; set; }     // Total # of indexes in database
        public long CountOfDocuments { get; set; }  // Total # of documents in database
        public long CountOfRevisionDocuments { get; set; }  // Total # of revision documents in database
        public long CountOfDocumentsConflicts { get; set; } // Total # of documents conflicts in database
        public long CountOfTombstones { get; set; }         // Total # of tombstones in database
        public long CountOfConflicts { get; set; }          // Total # of conflicts in database
        public long CountOfAttachments { get; set; }        // Total # of attachments in database
        public long CountOfUniqueAttachments { get; set; }  // Total # of unique attachments in database
        public long CountOfCounterEntries { get; set; }     // Total # of counter-group entries in database
        public long CountOfTimeSeriesSegments { get; set; } // Total # of time-series segments in database
        
        // List of stale index names in database
        public string[] StaleIndexes => Indexes?.Where(x => x.IsStale).Select(x => x.Name).ToArray();
        // Statistics for each index in database
        public IndexInformation[] Indexes { get; set; }
        
        public string DatabaseChangeVector { get; set; } // Global change vector of the database
        public string DatabaseId { get; set; }           // Database identifier
        public bool Is64Bit { get; set; }                // Indicates if process is 64-bit
        public string Pager { get; set; }                // Component handling the memory-mapped files
        public DateTime? LastIndexingTime { get; set; }  // Last time of indexing an item
        public Size SizeOnDisk { get; set; }             // Database size on disk
        public Size TempBuffersSizeOnDisk { get; set; }  // Temp buffers size on disk
        public int NumberOfTransactionMergerQueueOperations { get; set; }
    }
    #endregion
    */
    
    /*
    #region stats_4_results
    // Detailed database stats results:
    public class DetailedDatabaseStatistics : DatabaseStatistics
    {
        // Total # of identities in database
        public long CountOfIdentities { get; set; }
        // Total # of compare-exchange items in database
        public long CountOfCompareExchange { get; set; }
        // Total # of cmpXchg tombstones in database
        public long CountOfCompareExchangeTombstones { get; set; }
        // Total # of TS deleted ranges values in database
        public long CountOfTimeSeriesDeletedRanges { get; set; }
    }
    #endregion
    */
}
