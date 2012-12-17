#Statistics

One of the options available for the RavenDB administators is a capability of retrieving database statistics for the server. The statistics are available at `/admin/stats` endpoint.

{CODE-START:json /}
   > curl -X GET "http://localhost:8080/admin/stats"
{CODE-END /}

Document with following format is retreived:

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