# Administration : Statistics

## Server statistics

One of the options available for the RavenDB administrators is a capability of retrieving database statistics for the server. The statistics are available at `/admin/stats` endpoint or by Client API (details [here](../../client-api/commands/how-to/get-database-and-server-statistics)).

{CODE-START:json /}
   > curl -X GET "http://localhost:8080/admin/stats"
{CODE-END /}

Document with following format is retrieved:

{CODE-START:json /}
    {
      "ServerName": null,
      "TotalNumberOfRequests": 212,
      "Uptime": "00:04:37.5452852",
      "Memory": {
        "DatabaseCacheSizeInMB": 0.00,
        "ManagedMemorySizeInMB": 568.88,
        "TotalProcessMemorySizeInMB": 838.39
      },
      "LoadedDatabases": [
        {
          "Name": "Northwind",
          "LastActivity": "2014-08-07T08:17:43.7246662Z",
          "TransactionalStorageAllocatedSize": 6356992,
          "TransactionalStorageAllocatedSizeHumaneSize": "6.06 MBytes",
          "TransactionalStorageUsedSize": 4509696,
          "TransactionalStorageUsedSizeHumaneSize": "4.3 MBytes",
          "IndexStorageSize": 307512,
          "IndexStorageHumaneSize": "300.3 KBytes",
          "TotalDatabaseSize": 6664504,
          "TotalDatabaseHumaneSize": "6.36 MBytes",
          "CountOfDocuments": 1059,
          "CountOfAttachments": 0,
          "DatabaseTransactionVersionSizeInMB": 0.00,
          "Metrics": {
            "DocsWritesPerSecond": 0.0,
            "IndexedPerSecond": 0.0,
            "ReducedPerSecond": 0.0,
            "RequestsPerSecond": 0.0,
            "Requests": {
              "Count": 23,
              "MeanRate": 0.096,
              "OneMinuteRate": 0.048,
              "FiveMinuteRate": 0.771,
              "FifteenMinuteRate": 1.253
            },
            "RequestsDuration": {
              "Counter": 23,
              "Max": 2224.0,
              "Min": 0.0,
              "Mean": 184.95652173913044,
              "Stdev": 534.17859443353609,
              "Percentiles": {
                "50%": 3.0,
                "75%": 28.0,
                "95%": 2054.9999999999977,
                "99%": 2224.0,
                "99.9%": 2224.0,
                "99.99%": 2224.0
              }
            },
            "StaleIndexMaps": {
              "Counter": 20,
              "Max": 4.0,
              "Min": 0.0,
              "Mean": 0.4,
              "Stdev": 1.2311740225021848,
              "Percentiles": {
                "50%": 0.0,
                "75%": 0.0,
                "95%": 4.0,
                "99%": 4.0,
                "99.9%": 4.0,
                "99.99%": 4.0
              }
            },
            "StaleIndexReduces": {
              "Counter": 20,
              "Max": 2.0,
              "Min": 0.0,
              "Mean": 0.2,
              "Stdev": 0.52314836378059693,
              "Percentiles": {
                "50%": 0.0,
                "75%": 0.0,
                "95%": 1.9499999999999993,
                "99%": 2.0,
                "99.9%": 2.0,
                "99.99%": 2.0
              }
            },
            "Gauges": {
              "Raven.Database.Indexing.IndexBatchSizeAutoTuner": {
                "MaxNumberOfItems": "131072",
                "CurrentNumberOfItems": "1024",
                "InitialNumberOfItems": "512"
              },
              "Raven.Database.Indexing.WorkContext": {
                "RunningQueriesCount": "1"
              },
              "Raven.Database.Indexing.ReduceBatchSizeAutoTuner": {
                "InitialNumberOfItems": "256",
                "MaxNumberOfItems": "65536",
                "CurrentNumberOfItems": "1024"
              }
            },
            "ReplicationBatchSizeMeter": {},
            "ReplicationDurationMeter": {},
            "ReplicationBatchSizeHistogram": {},
            "ReplicationDurationHistogram": {}
          }
        }
      ],
      "LoadedFileSystems": []
    }
{CODE-END /}

where    

* **TotalNumberOfRequests** - number of requests that have been executed against the server   
* **Uptime** - uptime of a server      
* **Memory** - memory information   
    * **DatabaseCacheSizeInMB** - size of cache
    * **ManagedMemorySizeInMB** - size of managed memory taken by server   
    * **TotalProcessMemorySizeInMB** - total size of memory taken by server    
* **LoadedDatabases** - list of current active databases containing:    
   * **Name** - database name   
   * **LastActivity** - database last activity time   
   * **TransactionalStorageAllocatedSize** - number of bytes allocated by data storage engine
   * **TransactionalStorageAllocatedSizeHumaneSize** - number of bytes allocated by data storage engine in a more readable form
   * **TransactionalStorageUsedSize** - number of bytes used by data storage engine
   * **TransactionalStorageUsedSizeHumaneSize** - number of bytes used by data storage engine in a more readable form
   * **IndexStorageSize** - number of bytes taken by index storage
   * **IndexStorageHumaneSize** - number of bytes taken by index storage in a more readable form
   * **TotalDatabaseSize** - total number of bytes taken by both data and index storages
   * **TotalDatabaseHumaneSize** - total number of bytes taken by both data and index storages in a more readable form
   * **CountOfDocuments** - number of documents in database
   * **CountOfAttachments** - number of attachments in database
* **Metrics**
    * **DocsWritesPerSecond** - number of document writes per second
    * **IndexedPerSecond** - number of indexed documents per second
    * **ReducedPerSecond** - number of reductions per second
    * **RequestsPerSecond** - number of requests per second
    * **Requests** - detailed request statistics
    * **RequestsDuration** - detailed request duration statistics
    * ...many more

## Database statistics

To obtain database statistics one must use `/stats` endpoint or access them by Client API (details [here](../../client-api/commands/how-to/get-database-and-server-statistics)).

{CODE-START:json /}
   > curl -X GET "http://localhost:8080/stats" //statistics for 'system' database
   > curl -X GET "http://localhost:8080/databases/Northwind/stats" //statistics for 'Northwind' database
{CODE-END /}

Executing one of the above actions will end up in getting a document in the following format:

{CODE-START:json /}
    {
      "LastDocEtag": "01000000-0000-0001-0000-000000000423",
      "LastAttachmentEtag": "00000000-0000-0000-0000-000000000000",
      "CountOfIndexes": 4,
      "InMemoryIndexingQueueSize": 0,
      "ApproximateTaskCount": 0,
      "CountOfDocuments": 1059,
      "CountOfAttachments": 0,
      "StaleIndexes": [],
      "CurrentNumberOfItemsToIndexInSingleBatch": 512,
      "CurrentNumberOfItemsToReduceInSingleBatch": 256,
      "DatabaseTransactionVersionSizeInMB": 0.00,
      "Indexes": [
        {
          "Id": 1,
          "PublicName": "Raven/DocumentsByEntityName",
          "IndexingAttempts": 1051,
          "IndexingSuccesses": 1051,
          "IndexingErrors": 0,
          "LastIndexedEtag": "01000000-0000-0001-0000-000000000423",
          "LastIndexedTimestamp": "2014-08-07T08:16:11.0641827Z",
          "LastQueryTimestamp": "2014-08-07T08:17:22.0162553Z",
          "TouchCount": 0,
          "Priority": "Normal",
          "ReduceIndexingAttempts": null,
          "ReduceIndexingSuccesses": null,
          "ReduceIndexingErrors": null,
          "LastReducedEtag": null,
          "LastReducedTimestamp": null,
          "CreatedTimestamp": "2014-08-07T08:15:59.1742983Z",
          "LastIndexingTime": "2014-08-07T08:16:12.2413413Z",
          "IsOnRam": "false",
          "LockMode": "Unlock",
          "ForEntityName": [],
          "Performance": [],
          "DocsCount": 1051
        },
        {
          "Id": 2,
          "PublicName": "Orders/ByCompany",
          "IndexingAttempts": 830,
          "IndexingSuccesses": 830,
          "IndexingErrors": 0,
          "LastIndexedEtag": "01000000-0000-0001-0000-000000000423",
          "LastIndexedTimestamp": "2014-08-07T08:16:11.0641827Z",
          "LastQueryTimestamp": null,
          "TouchCount": 0,
          "Priority": "Normal",
          "ReduceIndexingAttempts": 169,
          "ReduceIndexingSuccesses": 169,
          "ReduceIndexingErrors": 0,
          "LastReducedEtag": "06000000-0000-0001-0000-0000000000BE",
          "LastReducedTimestamp": "2014-08-07T08:16:12.3323461Z",
          "CreatedTimestamp": "2014-08-07T08:16:10.4691466Z",
          "LastIndexingTime": "2014-08-07T08:16:12.3373439Z",
          "IsOnRam": "false",
          "LockMode": "Unlock",
          "ForEntityName": [
            "Orders"
          ],
          "Performance": [],
          "DocsCount": 89
        },
        {
          "Id": 3,
          "PublicName": "Orders/Totals",
          ...
        },
        {
          "Id": 4,
          "PublicName": "Product/Sales",
          ...
        }
      ],
      "Errors": [],
      "Triggers": null,
      "Extensions": null,
      "IndexingBatchInfo": [],
      "Prefetches": [],
      "DatabaseId": "b7746a76-1c84-483d-a219-cc154108bb2b",
      "SupportsDtc": false
    }
{CODE-END /}

where

* **LastDocEtag** - last added document Etag   
* **LastAttachmentEtag** - last added attachment Etag   
* **CountOfIndexes** - number of indexes in database   
* **ApproximateTaskCount** - approximate number of current database tasks   
* **CountOfDocuments** - number of documents in database   
* **CountOfAttachments** - number of attachments in database
* **StaleIndexes** - index names of stale indexes   
* **CurrentNumberOfItemsToIndexInSingleBatch** - initial value is 512 for 64-bit systems and 256 for 32-bit. Depending on the load can be auto-adjusted. Used by database indexer.   
* **CurrentNumberOfItemsToReduceInSingleBatch** - initial value is 512 for 64-bit systems and 256 for 32-bit. Depending on the load can be auto-adjusted. Used by database reducer.     
* **Indexes**    
   * **Id** - index identifier
   * **PublicName** - index name
   * **IndexingAttempts** - number of indexing attempts    
   * **IndexingSuccesses** - number of indexing successes   
   * **IndexingErrors** - number of indexing errors  
   * **LastIndexedEtag** - GUID representing last indexed Etag  
   * **LastIndexedTimestamp** - last indexing timestamp  
   * **LastQueryTimestamp** - last query timestamp 
   * **TouchCount** - number of index touches used to calculate index Etag properly,  
   * **Priority** - controls how much indexing processing resources index can consume. More information [here](../../server/administration/index-administration#index-prioritization).
   * **ReduceIndexingAttempts** - number of reduce attempts. Null if not applicable.   
   * **ReduceIndexingSuccesses** - number of reduce successes. Null if not applicable.   
   * **ReduceIndexingErrors** - number of reduce errors. Null if not applicable.   
   * **LastReducedEtag** - GUID representing last reduced Etag. Null if not applicable.     
   * **LastReducedTimestamp** - last reduce timestamp       
   * **CreatedTimestamp** - indicates when index was created
   * **IsOnRam** - indicates if index is stored only in memory (new and small indexes are stored in memory at first)
   * **LockMode** - indicates what is the current lock mode for index. More information [here](../../server/administration/index-administration#index-locking).
   * **Performance** - index performance information      
      * **Operation** - operation type:
         * `Map` or `Reduce Level level_number` for Map-Reduce indexes
         * `Index` for Map-only indexes       
      * **OutputCount** - number of documents indexed      
      * **InputCount**   
         * for `Map` and `Index` operations this is a number of documents sent for processing   
         * for `Reduce Level level_number` operation this is a number of documents that came from `Map` operation.   
      * **Duration** - operation duration      
      * **Started** - operation start time    
      * **DurationInMilliseconds** - duration in milliseconds     
    * **DocsCount** - number of indexes documents
* **Errors**
   * **Index** - name of index that caused error    
   * **Error** - error message    
   * **Timestamp** - error timestamp   
   * **Document** - key of document that caused error     
* **IndexingBatchInfo**   
   * **TotalDocumentCount** - number of documents in batch   
   * **TotalDocumentSize** - size of documents in batch
   * **Timestamp** - batch size report timestamp        
* **Prefetches** - prefetched indexing statistics        
   * **Timestamp** - prefetching start time     
   * **Duration** - prefetching duration      
   * **Size** - number of documents prefetched      
   * **Retries** - number of prefetching retries  
* **DatabaseId** - unique Id for database
* **SupportsDtc** - indicates if database (transactional storage) supports DTC transactions    

#### Related articles

TODO

