# Commands: Documents: How to get document metadata only?

**Head** is used to retrieve document metadata from a database.


### Syntax

{CODE-BLOCK:json}
curl \ 
	http://{serverName}/databases/{databaseName}/docs/{key} \
	-X HEAD 

{CODE-BLOCK/}

### Request

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **key** | Yes | key of a document to get metadata for |


### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Header | Description |
| -------- | - |
| anyHeader | Metadata of document is returned as response headers |

<hr />

### Example I

Gets metadata of `employees/1`.

{CODE-BLOCK:json}
curl -X HEAD "http://localhost:8080/databases/NorthWind/docs/employees/1" 
< HTTP/1.1 200 OK
< ETag: "01000000-0000-0001-0000-00000000006B"
< Raven-Entity-Name: Employees
< Raven-Clr-Type: Orders.Employee, Northwind
< Non-Authoritative-Information: False
< __document_id: employees/1
< Raven-Last-Modified: 2014-11-06T12:47:28.9789141Z
{CODE-BLOCK/}


## Related articles

- [Get](../../../../client-api/commands/documents/get)  
- [Put](../../../../client-api/commands/documents/put)  
- [Delete](../../../../client-api/commands/documents/delete)  
- [Revisions and concurrency with ETags](../../../../client-api/concurrency/revisions-and-concurrency-with-etags)   
