# Commands: Documents: Get

---
{NOTE: }  

* Use this endpoint with the `GET` method to retrieve documents from the database:  
`http://[server URL]/databases/[database name]/docs`
* There are two kinds of query strings that determine which documents will be retrieved:
  * A list of specific document IDs.
  * A prefix shared by a set of document IDs.

* In this page:    
  * [Get Documents by ID](../../commands/documents/get#get-documents-by-id)
      * [Request Format](../../commands/documents/get#request-format)
      * [Response Format](../../commands/documents/get#response-format)
      * [Example](../../commands/documents/get#example)
  * [Get Documents by startsWith](../../commands/documents/get#get-documents-by-startswith)
      * [Request Format](../../commands/documents/get#request-format-1)
      * [Response Format](../../commands/documents/get#response-format-1)
      * [Example](../../commands/documents/get#example-1)
{NOTE/}  

---

{PANEL: Get Documents by ID}

This query string specifies document IDs, counters, and related documents.

###Request Format

{CODE-BLOCK: bash}
curl \
    'http://[server URL]/databases/[database name]/docs?
                                                        id=[document ID]&id=[document ID]
                                                        &include=[field name]
                                                        &counter=[counter name]
                                                        &metadata=[boolean]' \
    -X GET \
    --header 'If-None-Match: [expected change vector]''
{CODE-BLOCK/}

| Query Parameters | Description | Required |
| - | - | - |
| **id** | ID of a document to retrieve.<br/>If no IDs are specified, all the documents in the database are retrieved in descending order of their [change vectors](../../../server/clustering/replication/change-vector). | Required |
| **include** | A field in the retrieved document(s) which contains the ID of another, 'related' document.<br/>These related documents are also retrieved. [See: How to Handle Document Relationships](../../../client-api/how-to/handle-document-relationships#includes). | Optional |
| **counter** | Name of a [counter](../../../client-api/session/counters/overview) to retrieve. Set this parameter to "@all_counters" to retrieve all counters (counters of _related_ documents are not retrieved). | Optional |
| **metadataOnly** | Set this parameter to "true" to retrieve only document metadata (_related_ documents are always retrieved with their complete contents). | Optional |

| Header | Description | Required |
| - | - | - |
| **If-None-Match** | The documents' expected [change vector](../../../server/clustering/replication/change-vector). If these change vectors do not match the server-side change vectors, a concurrency exception is thrown. | Optional |

###Response Format

| HTTP Status Code | Description |
| ----------- | - |
| `200` | Results were successfully retrieved |
| `304` | All requested document(s) are already cached locally, and the expected change vector matches the server side change vector. No documents were retrieved. |
| `404` | No documents with the specified IDs were found |

The response body in JSON format:

{CODE-BLOCK: JavaScript}
{
    "Results": [ 
        { 
            "[field]":"[value]",
            ...
            "@metadata":{
                ...
            }
        },
        { ... },
        ...
    ],
    "Includes":
        "[document ID]": {
            ...
        },
        "[document ID]": { ... },
        ...
    }
    "CounterIncludes": {
        "[document ID]": [
            {
                "DocumentId": "[document ID]",
                "CounterName": "[counter name]",
                "TotalValue": [integer value]
            },
            ...
        ],
        "[document ID]": [
            ...
        ],
        ...
    }
}
{CODE-BLOCK/}

###Example

An example request sent to the RavenDB playground server:

{CODE-BLOCK: bash}
curl \
    'http://live-test.ravendb.net/databases/Demo/docs?
                                                        id=orders/1-A
                                                        &id=orders/2-A
                                                        &include=Company
                                                        &counter=@all_counters
                                                        &metadata=true' \
    -X GET \
{CODE-BLOCK/}

The response:

{CODE-BLOCK: JSON}
HTTP/1.1 200
status: 200
Server: nginx
Date: Tue, 27 Aug 2019 09:53:17 GMT
Content-Type: application/json; charset=utf-8
Transfer-Encoding: chunked
Connection: keep-alive
Content-Encoding: gzip
ETag: "Hash-pTi+pLiV7/DeFimBXomeIghe0WBdY6fvBwLAby6GQ4s="
Vary: Accept-Encoding
Raven-Server-Version: 4.2.3.42

{
    "Results": [
        {
            "@metadata": {
                "@collection": "Orders",
                "@change-vector": "A:417-7pP/RxYrFEWb+RGV730VaA",
                "@flags": "HasRevisions",
                "@id": "orders/1-A",
                "@last-modified": "2018-07-27T12:11:53.0447651Z"
            }
        },
        {
            "@metadata": {
                "@attachments": [
                    {
                        "Name": "photo.jpg",
                        "Hash": "NV4XfGnTtz6l2mAJBQV8t6JpeeVy2TNyBiGwEZaPaAg=",
                        "ContentType": "image/jpeg",
                        "Size": 13441
                    }
                ],
                "@collection": "Employees",
                "@change-vector": "A:2113-7pP/RxYrFEWb+RGV730VaA",
                "@flags": "HasAttachments",
                "@id": "employees/1-A",
                "@last-modified": "2018-07-27T12:23:25.3556270Z"
            }
        }
    ],
    "Includes": {
        "companies/85-A": {
            "ExternalId": "VINET",
            "Name": "Vins et alcools Chevalier",
            "Contact": {
                "Name": "Paul Henriot",
                "Title": "Accounting Manager"
            },
            "Address": {
                "Line1": "59 rue de l'Abbaye",
                "Line2": null,
                "City": "Reims",
                "Region": null,
                "PostalCode": "51100",
                "Country": "France",
                "Location": {
                    "Latitude": 49.255958199999988,
                    "Longitude": 4.1547448
                }
            },
            "Phone": "26.47.15.10",
            "Fax": "26.47.15.11",
            "@metadata": {
                "@collection": "Companies",
                "@change-vector": "A:171-7pP/RxYrFEWb+RGV730VaA",
                "@id": "companies/85-A",
                "@last-modified": "2018-07-27T12:11:53.0258706Z"
            }
        }
    },
    "CounterIncludes": {
        "orders/1-A": [],
        "employees/1-A": [
                    {
                "DocumentId": "employees/1-A",
                "CounterName": "NumOrders",
                "TotalValue": 123
            }
        ]
    }
}
{CODE-BLOCK/}

{PANEL/}

{PANEL: Get Documents by startsWith}

This query string identifies document IDs based on substrings.

###Request Format

{CODE-BLOCK: bash}
curl \
    'http://[server URL]/databases/[database name]/docs?
                                                        startsWith=[prefix]
                                                        &matches=[suffix]|[suffix]
                                                        &exclude=[suffix]|[suffix]
                                                        &startAfter=[document ID]
                                                        &start=[integer]
                                                        &pageSize=[integer]
                                                        &metadata=[boolean]' \
    -X GET \
    --header 'If-None-Match: [expected change vector]'
{CODE-BLOCK/}

| Query Parameters | Description | Required |
| - | - | - |
| **startsWith** | Retrieve all documents whose IDs begin with this prefix | Required |
| **matches** | Retrieve documents whose ID exactly matches `startsWith` + `matches`. Accepts multiple values separated by a pipe ('\|'). Use `?` to represent any single character, and `*` to represent any substring | Optional |
| **exclude** | _Exclude_ documents whose ID exactly matches `startsWith` + `exclude`. Accepts multiple values separated by a pipe: ('\|'). Use `?` to represent any single character, and `*` to represent any substring | Optional |
| **startAfter** | With results sorted in alphabetical order, retrieve only the results following the document ID that begins with this prefix | Optional |
| **start** | Number of results to skip, with results sorted in alphabetical order | Optional |
| **pageSize** | Total number of results to retrieve | Optional |
| **metadataOnly** | Set this parameter to "true" to retrieve only document metadata (_related_ documents are always retrieved with their complete contents). | Optional |

| Header | Description | Required |
| - | - | - |
| **If-None-Match** | The documents' expected [change vectors](../../../server/clustering/replication/change-vector). If these change vectors do not match the server-side change vectors, a concurrency exception is thrown. | Optional |

###Response Format

| HTTP Status Code | Description |
| ----------- | - |
| `200` | Results were successfully retrieved |
| `304` | All requested document(s) are already cached locally, and the expected change vector matches the server side change vector. No documents were retrieved. |
| `404` | No documents with the specified IDs were found |

The response body in JSON format:

{CODE-BLOCK: JavaScript}
{
    "Results": [ 
        { 
            "[field]":"[value]",
            ...
            "@metadata":{
                ...
            }
        },
        { ... },
        ...
    ]
}
{CODE-BLOCK/}

###Example

An example request sent to the RavenDB playground server:

{CODE-BLOCK: bash}
curl \
    'http://live-test.ravendb.net/databases/Demo/docs?
                                                        startsWith=employe
                                                        &matches=es/1-A|es/2-A|es/3-A|es/4-A|es/5-A|es/6-A|es/7-A|es/8-A|
                                                        &exclude=es/5-A|es/6-A
                                                        &startAfter=employees/1-A
                                                        &start=1
                                                        &pageSize=3
                                                        &metadata=true' \
    -X GET \
{CODE-BLOCK/}

The response:

{CODE-BLOCK: JSON}
HTTP/1.1 200
status: 200
Server: nginx
Date: Tue, 27 Aug 2019 10:00:02 GMT
Content-Type: application/json; charset=utf-8
Transfer-Encoding: chunked
Connection: keep-alive
Content-Encoding: gzip
ETag: "Hash-pTi+pLiV7/DeFimBXomeIghe0WBdY6fvBwLAby6GQ4s="
Vary: Accept-Encoding
Raven-Server-Version: 4.2.3.42

{
    "Results": [
        {
            "@metadata": {
                "@attachments": [
                    {
                        "Name": "photo.jpg",
                        "Hash": "z3QaQ8oet5WDQ0WHDu4LgWv9LJPqUR+2aDjU1sbGXQA=",
                        "ContentType": "image/jpeg",
                        "Size": 17741
                    }
                ],
                "@collection": "Employees",
                "@change-vector": "A:2119-7pP/RxYrFEWb+RGV730VaA",
                "@flags": "HasAttachments",
                "@id": "employees/3-A",
                "@last-modified": "2018-07-27T12:24:21.7053605Z"
            }
        },
        {
            "@metadata": {
                "@attachments": [
                    {
                        "Name": "photo.jpg",
                        "Hash": "6VYrILPe+PvqQOUxRUEzqVM+5kBndU67o+MxnOam378=",
                        "ContentType": "image/jpeg",
                        "Size": 26439
                    }
                ],
                "@collection": "Employees",
                "@change-vector": "A:2122-7pP/RxYrFEWb+RGV730VaA",
                "@flags": "HasAttachments",
                "@id": "employees/4-A",
                "@last-modified": "2018-07-27T12:24:41.9236835Z"
            }
        },
        {
            "@metadata": {
                "@attachments": [
                    {
                        "Name": "photo.jpg",
                        "Hash": "97S5UrejdZqHfel4i+/ts5orhNlp92DItxOUVow0maI=",
                        "ContentType": "image/jpeg",
                        "Size": 17908
                    }
                ],
                "@collection": "Employees",
                "@change-vector": "A:2131-7pP/RxYrFEWb+RGV730VaA",
                "@flags": "HasAttachments",
                "@id": "employees/7-A",
                "@last-modified": "2018-07-27T12:26:10.2204935Z"
            }
        }
    ]
}
{CODE-BLOCK/}

{PANEL/}

## Related Articles

### Client API 

- [Put Documents](../../../client-api/commands/documents/put)  
- [Delete Documents](../../../client-api/commands/documents/delete)
- [How to Send Multiple Commands Using a Batch](../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)
- [Counters: Overview](../../../client-api/session/counters/overview)
- [How to Handle Document Relationships](../../../client-api/how-to/handle-document-relationships#includes)

### Server

- [Change Vector](../../../server/clustering/replication/change-vector)
