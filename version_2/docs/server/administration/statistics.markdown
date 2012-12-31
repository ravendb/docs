﻿#Statistics

##Server statistics

One of the options available for the RavenDB administrators is a capability of retrieving database statistics for the server. The statistics are available at `/admin/stats` endpoint.

{CODE-START:json /}
   > curl -X GET "http://localhost:8080/admin/stats"
{CODE-END /}

Document with following format is retrieved:

{CODE-START:json /}
	{
		"TotalNumberOfRequests": 1573,
		"Uptime": "00:03:08.5002420",
		"LoadedDatabases": [
			{
				"Name": "System",
				"LastActivity": "2012-12-14T13:21:03.0212875Z",
				"Size": 1056828,
				"HumaneSize": "1.01 MBytes",
				"CountOfDocuments": 1,
				"RequestsPerSecond": 0.0,
				"ConcurrentRequests": 1.0
			},
			...
			{
				"Name": "ExampleDB"
				...
			}
		]
	}
{CODE-END /}

where    

* **TotalNumberOfRequests** - number of requests that have been executed against the server   
* **Uptime** - uptime of a server       
* **LoadedDatabases** - list of current active databases containing:    
   * **Name** - database name   
   * **LastActivity** - database last activity time   
   * **Size** - database size in bytes      
   * **HumaneSize** - database size in a more readable format. This value will be in KBytes, MBytes or GBytes depends on the actual **Size**      
   * **CountOfDocuments** - number of documents in database       
   * **RequestsPerSecond** - number of request per second     
   * **ConcurrentRequests** - number of concurrent requests           

##Database statistics

To obtain database statistics one must use `/stats` endpoint.

{CODE-START:json /}
   > curl -X GET "http://localhost:8080/stats" //statistics for 'system' database
   > curl -X GET "http://localhost:8080/databases/ExampleDB/stats" //statistics for 'ExampleDB' database
{CODE-END /}

Executing one of the above actions will end up in getting a document in the following format:

{CODE-START:json /}
	{
		"LastDocEtag": "00000000-0000-0600-0000-000000000001",
		"LastAttachmentEtag": "00000000-0000-0000-0000-000000000000",
		"CountOfIndexes": 1,
		"ApproximateTaskCount": 0,
		"CountOfDocuments": 2,
		"StaleIndexes": [
		],
		"CurrentNumberOfItemsToIndexInSingleBatch": 512,
		"CurrentNumberOfItemsToReduceInSingleBatch": 256,
		"Memory": {
			"DatabaseCacheSizeInMB": 0.25,
			"DatabaseTransactionVersionSizeInMB": 0.19,
			"ManagedMemorySizeInMB": 11.68,
			"TotalProcessMemorySizeInMB": 143.35,
			"MemoryThatIsNotAccountedFor": 131.23
		},
		"Indexes": [
			{
				"Name": "Raven/DocumentsByEntityName",
				"IndexingAttempts": 1,
				"IndexingSuccesses": 1,
				"IndexingErrors": 0,
				"LastIndexedEtag": "00000000-0000-0600-0000-000000000001",
				"LastIndexedTimestamp": "2012-12-17T12:07:55.2670000",
				"LastQueryTimestamp": null,
				"TouchCount": 0,
				"ReduceIndexingAttempts": null,
				"ReduceIndexingSuccesses": null,
				"ReduceIndexingErrors": null,
				"LastReducedEtag": null,
				"LastReducedTimestamp": null,
				"Performance": [
					{
						"Operation": "Index",
						"OutputCount": 1,
						"InputCount": 1,
						"Duration": "00:00:00.0221447",
						"Started": "2012-12-17T12:07:55.2715387Z",
						"DurationMilliseconds": 22.14
					}
				]
			}
		],
		"Errors": [
		],
		"Triggers": [
			{
				"Type": "Put",
				"Name": "Raven.Database.Plugins.Builtins.InvalidDocumentNames"
			},
			{
				"Type": "Put",
				"Name": "Raven.Database.Plugins.Builtins.Tenants.ModifiedTenantDatabase"
			},
			{
				"Type": "Put",
				"Name": "Raven.Database.Server.Security.Triggers.WindowsAuthPutTrigger"
			},
			{
				"Type": "Delete",
				"Name": "Raven.Database.Plugins.Builtins.Tenants.DeletedTenantDatabase"
			},
			{
				"Type": "Delete",
				"Name": "Raven.Database.Server.Security.Triggers.WindowsAuthDeleteTrigger"
			},
			{
				"Type": "Delete",
				"Name": "Raven.Database.Plugins.Builtins.Tenants.RemoveTenantDatabase"
			},
			{
				"Type": "Read",
				"Name": "Raven.Database.Plugins.Builtins.FilterRavenInternalDocumentsReadTrigger"
			}
		],
		"Extensions": [
			{
				"Name": "IStartupTask",
				"Installed": [
					{
						"Name": "RemoveBackupDocumentStartupTask",
						"Assembly": "Raven.Database"
					},
					{
						"Name": "CleanupOldDynamicIndexes",
						"Assembly": "Raven.Database"
					},
					{
						"Name": "CreateFolderIcon",
						"Assembly": "Raven.Database"
					},
					{
						"Name": "DeleteRemovedIndexes",
						"Assembly": "Raven.Database"
					},
					{
						"Name": "DeleteTemporaryIndexes",
						"Assembly": "Raven.Database"
					}
				]
			},
			{
				"Name": "AbstractReadTrigger",
				"Installed": [
					{
						"Name": "FilterRavenInternalDocumentsReadTrigger",
						"Assembly": "Raven.Database"
					}
				]
			},
			{
				"Name": "AbstractDeleteTrigger",
				"Installed": [
					{
						"Name": "DeletedTenantDatabase",
						"Assembly": "Raven.Database"
					},
					{
						"Name": "WindowsAuthDeleteTrigger",
						"Assembly": "Raven.Database"
					},
					{
						"Name": "RemoveTenantDatabase",
						"Assembly": "Raven.Database"
					}
				]
			},
			{
				"Name": "AbstractPutTrigger",
				"Installed": [
					{
						"Name": "InvalidDocumentNames",
						"Assembly": "Raven.Database"
					},
					{
						"Name": "ModifiedTenantDatabase",
						"Assembly": "Raven.Database"
					},
					{
						"Name": "WindowsAuthPutTrigger",
						"Assembly": "Raven.Database"
					}
				]
			},
			{
				"Name": "AbstractDynamicCompilationExtension",
				"Installed": [
					{
						"Name": "SpatialDynamicCompilationExtension",
						"Assembly": "Raven.Database"
					}
				]
			}
		],
		"ActualIndexingBatchSize": [
			{
				"Size": 1,
				"Timestamp": "2012-12-17T12:07:55.2705358Z"
			}
		],
		"Prefetches": [
		]
	}

{CODE-END /}

where

* **LastDocEtag** - last added document Etag   
* **LastAttachmentEtag** - last added attachment Etag   
* **CountOfIndexes** - number of indexes in database   
* **ApproximateTaskCount** - approximate number of current database tasks   
* **CountOfDocuments** - number of documents in database   
* **StaleIndexes** - index names of stale indexes   
* **CurrentNumberOfItemsToIndexInSingleBatch** - initial value is 512 for 64-bit systems and 256 for 32-bit. Depending on the load can be auto-adjusted. Used by database indexer.   
* **CurrentNumberOfItemsToReduceInSingleBatch** - initial value is 512 for 64-bit systems and 256 for 32-bit. Depending on the load can be auto-adjusted. Used by database reducer.   
* **Memory**    
   * **DatabaseCacheSizeInMB** - size of database cache (in MB)   
   * **DatabaseTransactionVersionSizeInMB** - current size (in MB) of version store (in memory modified data)     
   * **ManagedMemorySizeInMB** - best available approximation of the number of MB currently allocated in managed memory    
   * **TotalProcessMemorySizeInMB** - amount of private memory (in MB) allocated for the server   
   * **MemoryThatIsNotAccountedFor** - amount of memory that is not accounted for: (TotalProcessMemorySizeInMB - (DatabaseCacheSizeInMB + DatabaseTransactionVersionSizeInMB + ManagedMemorySizeInMB))   
* **Indexes**    
   * **Name** - index name
   * **IndexingAttempts** - number of indexing attempts    
   * **IndexingSuccesses** - number of indexing successes   
   * **IndexingErrors** - number of indexing errors  
   * **LastIndexedEtag** - GUID representing last indexed Etag  
   * **LastIndexedTimestamp** - last indexing timestamp  
   * **LastQueryTimestamp** - last query timestamp 
   * **TouchCount** - number of index touches used to calculate index Etag properly,   
   * **ReduceIndexingAttempts** - number of reduce attempts. Null if not applicable.   
   * **ReduceIndexingSuccesses** - number of reduce successes. Null if not applicable.   
   * **ReduceIndexingErrors** - number of reduce errors. Null if not applicable.   
   * **LastReducedEtag** - GUID representing last reduced Etag. Null if not applicable.     
   * **LastReducedTimestamp** - last reduce timestamp       
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
* **Errors**
   * **Index** - name of index that caused error    
   * **Error** - error message    
   * **Timestamp** - error timestamp   
   * **Document** - key of document that caused error   
* **Triggers**
   * **Type** - type of trigger:    
      * `Read`   
      * `Put`   
      * `Index Update`    
      * `Delete`        
   * **Name** - trigger name     
* **Extensions**   
   * **Name** - extension type:
      * `IStartupTask`   
      * `AbstractReadTrigger`   
      * `AbstractDeleteTrigger`   
      * `AbstractPutTrigger`   
      * `AbstractDocumentCodec`    
      * `AbstractIndexCodec`   
      * `AbstractDynamicCompilationExtension`   
      * `AbstractIndexQueryTrigger`    
      * `AbstractIndexUpdateTrigger`    
      * `AbstractAnalyzerGenerator`    
      * `AbstractAttachmentDeleteTrigger`    
      * `AbstractAttachmentPutTrigger`     
      * `AbstractAttachmentReadTrigger`    
      * `AbstractBackgroundTask`    
      * `IAlterConfiguration`    
   * **Installed** - list of installed extensions of that type  
      * **Name** - installed extension name    
      * **Assembly** - assembly of the installed extension     
* **ActualIndexingBatchSize**   
   * **Size** - indexing batch size   
   * **Timestamp** - batch size report timestamp        
* **Prefetches** - prefetched indexing statistics        
   * **Timestamp** - prefetching start time     
   * **Duration** - prefetching duration      
   * **Size** - number of documents prefetched      
   * **Retries** - number of prefetching retries      

