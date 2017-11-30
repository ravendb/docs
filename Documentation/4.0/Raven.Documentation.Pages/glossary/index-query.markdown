# Glossary : IndexQuery

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Query** | string | The query |
| **QueryParameters** | Parameters (Dictionary&lt;string, object&gt;) | The query parameters |
| **Start** | int | The start of records to read |
| **PageSize** |  int | The page size |
| **WaitForNonStaleResults** | bool | If set to true the server side will wait until result are non stale or until timeout |
| **WaitForNonStaleResultsTimeout** | TimeSpan? | The timeout for WaitForNonStaleResults |
| **CutoffEtag** | long? | Cutoff etag is used to check if the index has already process a document with the given etag. |
| **DisableCaching** | bool | Indicates if query results should be read from cache (if cached previously) or added to cache (if there were no cached items prior) |
| **SkipDuplicateChecking** | bool | Allow to skip duplicate checking during queries |
| **ExplainScores** | bool | Whatever a query result should contains an explanation about how docs scored against query |
	
