# Glossary: IndexQuery

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Query** | string | The query |
| **QueryParameters** | Parameters (Dictionary&lt;string, object&gt;) | The query parameters |
| **Start** | int | The start of records to read |
| **PageSize** |  int | The page size |
| **WaitForNonStaleResults** | bool | If set to true, the server side will wait until the results are non-stale or until a timeout. |
| **WaitForNonStaleResultsTimeout** | TimeSpan? | The timeout for WaitForNonStaleResults |
| **CutoffEtag** | long? | The cutoff Etag is used to check if the index has already processed a document with the given Etag. |
| **DisableCaching** | bool | Indicates if the query results should be read from the cache (if cached previously), or added to the cache (if there were no cached items prior). |
| **SkipDuplicateChecking** | bool | Allows to skip duplicate checking during queries. |
| **ExplainScores** | bool | When a query result should contain an explanation about how docs are scored against a query. |
	
