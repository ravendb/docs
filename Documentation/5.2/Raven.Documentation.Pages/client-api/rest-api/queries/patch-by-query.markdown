# Patch By Query

---

{NOTE: }

* Use this endpoint with the **`PATCH`** method to update all documents that satisfy a query:  
`<server URL>/databases/<database name>/queries`  

* [Patching](../../../client-api/operations/patching/set-based) occurs on the server side.  

* In this page:  
  * [Example](../../../client-api/rest-api/queries/patch-by-query#example)  
  * [Request Format](../../../client-api/rest-api/queries/patch-by-query#request-format)  
  * [Response Format](../../../client-api/rest-api/queries/patch-by-query#response-format)  

{NOTE/}

---

{PANEL: Example}

This cURL request sends a query with an `update` clause to a database named "Example" on our 
[playground server](http://live-test.ravendb.net). The results of this query will each be modified on the server side.  

{CODE-BLOCK: bash}
curl -X PATCH "http://live-test.ravendb.net/databases/Example/queries"
-d "{ \"Query\": { \"Query\": \"from Employees as E update{ E.FirstName = 'Bob' }\" } }"
{CODE-BLOCK/}
Linebreaks are added for clarity.  

Response:  

{CODE-BLOCK: http}
HTTP/1.1 200 OK
Server: nginx
Date: Sun, 24 Nov 2019 12:24:51 GMT
Content-Type: application/json; charset=utf-8
Transfer-Encoding: chunked
Connection: keep-alive
Content-Encoding: gzip
Vary: Accept-Encoding
Raven-Server-Version: 4.2.5.42
Request-Time: 5

{
    "OperationId": 42,
    "OperationNodeTag": "A"
}
{CODE-BLOCK/}

{PANEL/}

{PANEL: Request Format}

This the general format of a cURL request that uses all query string parameters:

{CODE-BLOCK: batch}
curl -X GET "<server URL>/databases/<database name>/docs?
            allowStale=<boolean>
            &staleTimeout=<TimeSpan>
            &maxOpsPerSec=<integer>"
-d "{ }"
{CODE-BLOCK/}

#### Query String Parameters

| Option | Description |
| - | - |
| **allowStale** | If the query is on an index (rather than a collection), this determines whether to patch results from a [stale index](../../../indexes/stale-indexes). If set to `false` and the specified index is stale, an exception is thrown. Default: `false`. |
| **staleTimeout** | If `allowStale` is set to `false`, this parameter sets the amount of time to wait for the index not to be stale. If the time runs out, an exception is thrown. The value is of type [TimeSpan](https://docs.microsoft.com/en-us/dotnet/api/system.timespan). Default: `null` - if the index is stale the exception is thrown immediately. |
| **maxOpsPerSec** | The maximum number of patches per second the server can perform in the background. Default: no limit. |

#### Body

This is the general format of the request body:

{CODE-BLOCK: powershell}
-d "{
    \"Query\": {
        \"Query\": \"<RQL query referencing $<name>>\",
        \"QueryParameters\": {
            \"<name>\":\"<parameter>\",
            ...
        }
    }
}"
{CODE-BLOCK/}
Depending on the shell you're using to run cURL, you will probably need to escape all 
double quotes within the request body using a backslash: `"` -> `\"`.  

| Parameter | Description |
| - | - |
| **Query** | A query in [RQL](../../../client-api/session/querying/what-is-rql). You can insert parameters from the `QueryParameters` object with `$<parameter name>` |
| **QueryParameters** | A list of values that can be used in the query, such as strings, ints, or documents IDs. Inputs from your users should always be passed as query parameters to avoid SQL injection attacks, and in general it's best practice to pass all your right-hand operands as parameters. |
{PANEL/}

{PANEL: Response Format}

#### Http Status Codes

| Code | Description |
| - | - |
| `200` | The request was valid. This includes the case where the query found 0 results, or the specified index does not exist, etc. |
| `400` | Bad request |
| `500` | Server-side exception |

#### Body

{CODE-BLOCK: javascript}
{
    "OperationId": <int>,
    "OperationNodeTag": "<cluster node tag>"
}
{CODE-BLOCK/}

| Field | Description |
| - | - |
| **OperationId** | Increments each time the server recieves a new Operation to execute, such as `DeleteByQuery` or `PatchByQuery` |
| **OperationNodeTag** | The tag of the Cluster Node that first received the Patch by Query Operation. Values are `A` to `Z`. See [Cluster Topology](../../../server/clustering/rachis/cluster-topology). |
{PANEL/}

## Related articles  

### Client API  

##### Operations  

- [Set Based Patching of Documents](../../../client-api/operations/patching/set-based)  

##### REST API  

- [Query the Database](../../../client-api/rest-api/queries/query-the-database)  
- [Delete by Query](../../../client-api/rest-api/queries/common/delete-by-query)  
<br/>
### Indexes  

- [Stale Indexes](../../../indexes/stale-indexes)  
<br/>
### Server  

- [Cluster Topology](../../../server/clustering/rachis/cluster-topology)  
