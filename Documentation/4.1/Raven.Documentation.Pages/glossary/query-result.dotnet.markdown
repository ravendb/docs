# Glossary: QueryResult

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Results** | BlittableJsonReaderArray | The documents resulting from this query. |
| **Includes** | BlittableJsonReaderObject | The documents included in the result. |
| **IsStale** | bool | The value indicating whether the index is stale. |
| **IndexTimestamp** | DateTime | The last time the index was updated. |
| **TotalResults** | int | The total results for this query. |
| **SkippedResults** | int | The skipped results. |
| **IndexName** | string | The index used to answer this query. |
| **ResultEtag** | long |  The ETag value for this index current state, which includes what we docs we indexed, what document were deleted, etc. |
| **LastQueryTime** | DateTime | The timestamp of the last time the index was queried. |
| **DurationInMs** | long | The duration of actually executing the query server side. |
| **ResultSize** | long | The size of the response which was sent from the server. This value is the _uncompressed_ size.  |
| **NodeTag** | string | Tag of a cluster node which responded to the query.  |
