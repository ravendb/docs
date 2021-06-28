# Get All Documents

---

{NOTE: }  

* Use this endpoint with the **`GET`** method to retrieve all documents from the database:  
`<server URL>/databases/<database name>/docs`  

* Query parameters can be used to page the results.  

* In this page:  
  * [Basic Example](../../../client-api/rest-api/document-commands/get-all-documents#basic-example)  
  * [Request Format](../../../client-api/rest-api/document-commands/get-all-documents#request-format)  
  * [Response Format](../../../client-api/rest-api/document-commands/get-all-documents#response-format)  
  * [Query Parameter Examples](../../../client-api/rest-api/document-commands/get-all-documents#query-parameter-examples)  
      * [start](../../../client-api/rest-api/document-commands/get-all-documents#start)  
      * [pageSize](../../../client-api/rest-api/document-commands/get-all-documents#pagesize)
      * [metadataOnly](../../../client-api/rest-api/document-commands/get-all-documents#metadataonly)  
{NOTE/}  

---

{PANEL:Basic Example}

This is a cURL request to a database named "Example" on our [playground server](http://live-test.ravendb.net). Paging 
through all of the documents in the database, the request skips the first 9 documents and retrieves the next 2.  

{CODE-BLOCK: bash}
curl -X GET "http://live-test.ravendb.net/databases/Example/docs?start=9&pageSize=2"
{CODE-BLOCK/}

Response:  

{CODE-BLOCK: http}
HTTP/1.1 200 OK
Server: nginx 
Date: Thu, 10 Oct 2019 12:00:40 GMT 
Content-Type: application/json; charset=utf-8 
Transfer-Encoding: chunked 
Connection: keep-alive 
Content-Encoding: gzip 
ETag: "A:2134-W33iO0zJC0qZKWh6fjnp6A, A:1887-0N64iiIdYUKcO+yq1V0cPA, A:6214-xwmnvG1KBkSNXfl7/0yJ1A" 
Vary: Accept-Encoding 
Raven-Server-Version: 4.2.4.42 

{
    "Results": [
        {
            "Name": "Seafood",
            "Description": "Seaweed and fish",
            "@metadata": {
                "@attachments": [
                    {
                        "Name": "image.jpg",
                        "Hash": "GWdpGVCWyLsrtNdA5AOee0QOZFG6rKIqCosZZN5WnCA=",
                        "ContentType": "image/jpeg",
                        "Size": 33396
                    }
                ],
                "@collection": "Categories",
                "@change-vector": "A:2107-W33iO0zJC0qZKWh6fjnp6A",
                "@flags": "HasAttachments",
                "@id": "categories/8-A",
                "@last-modified": "2018-07-27T12:21:39.1315788Z"
            }
        },
        {
            "Name": "Produce",
            "Description": "Dried fruit and bean curd",
            "@metadata": {
                "@attachments": [
                    {
                        "Name": "image.jpg",
                        "Hash": "asY7yUHhdgaVoKhivgua0OUSJKXqNDa3Z1uLP9XAocM=",
                        "ContentType": "image/jpeg",
                        "Size": 61749
                    }
                ],
                "@collection": "Categories",
                "@change-vector": "A:2104-W33iO0zJC0qZKWh6fjnp6A",
                "@flags": "HasAttachments",
                "@id": "categories/7-A",
                "@last-modified": "2018-07-27T12:21:11.2283909Z"
            }
        }
    ]
}
{CODE-BLOCK/}

{PANEL/}

{PANEL: Request Format}

This is the general format of a cURL request that uses all query string parameters:  

{CODE-BLOCK: batch}
curl -X GET "<server URL>/databases/<database name>/docs?
            &start=<integer>
            &pageSize=<integer>
            &metadataOnly=<boolean>"
--header "If-None-Match: <hash>"
{CODE-BLOCK/}
Linebreaks are added for clarity.  
<br/>
####Query String Parameters

| Parameter | Description | Required  |
| - | - | - |
| **start** | Number of results to skip. | No |
| **pageSize** | Maximum number of results to retrieve. | No |
| **metadataOnly** | Set this parameter to `true` to retrieve only the document metadata from each result. | No |

####Headers

| Header | Description | Required |
| - | - | - |
| **If-None-Match** | This header takes a hash representing the previous results of an **identical** request. The hash is found in the response header `ETag`. If the results were not modified since the previous request, the server responds with http status code `304`, and the requested documents are retrieved from a local cache rather than over the network. | No |

{PANEL/}

{PANEL:Response Format}

#### Http Status Codes

| Code | Description |
| ----------- | - |
| `200` | Results were successfully retrieved |
| `304` | In response to an `If-None-Match` check: none of the requested documents were modified since they were last loaded, so they were not retrieved from the server. |

#### Headers

| Header | Description |
| - | - |
| **Content-Type** | MIME media type and character encoding. This should always be: `application/json; charset=utf-8` |
| **ETag** | Hash representing the state of these results. If another, **identical** request is made, this hash can be sent in the `If-None-Match` header to check whether the retrieved documents have been modified since the last response. |
| **Raven-Server-Version** | Version of RavenDB that the responding server is running |

#### Body

Retrieved documents are sorted in descending order of their [change vectors](../../../server/clustering/replication/change-vector). 
A retrieved document is identical in contents and format to the document stored in the server - unless the `metadataOnly` 
parameter is set to `true`.  

This is the general format of the JSON response body:  

{CODE-BLOCK: javascript}
{
    "Results": [
        {
            "<field>":"<value>",
            ...
            "@metadata":{
                ...
            }
        },
        { <document contents> },
        ...
    ]
}
{CODE-BLOCK/}
Linebreaks are added for clarity.  

{PANEL/}

{PANEL: Query Parameter Examples}

[About Northwind](../../../start/about-examples), the database used in our examples.

In this section:  

* [start](../../../client-api/rest-api/document-commands/get-all-documents#start)  
* [pageSize](../../../client-api/rest-api/document-commands/get-all-documents#pagesize)  
* [metadataOnly](../../../client-api/rest-api/document-commands/get-all-documents#metadataonly)  

---

### start

Skip first 1,057 documents, and retrieve the rest (our version of Northwind contains 1,059 documents).  
cURL request:  

{CODE-BLOCK: bash}
curl -X GET "http://live-test.ravendb.net/databases/Example/docs?start=1057"
{CODE-BLOCK/}

Response:  

{CODE-BLOCK: Http}
HTTP/1.1 200 OK
Server: nginx
Date: Thu, 10 Oct 2019 16:30:37 GMT
Content-Type: application/json; charset=utf-8
Transfer-Encoding: chunked
Connection: keep-alive
Content-Encoding: gzip
ETag: "A:2134-W33iO0zJC0qZKWh6fjnp6A, A:1887-0N64iiIdYUKcO+yq1V0cPA, A:6214-xwmnvG1KBkSNXfl7/0yJ1A"
Vary: Accept-Encoding
Raven-Server-Version: 4.2.4.42

{
    "Results": [
        {
            "ExternalId": "ALFKI",
            "Name": "Alfreds Futterkiste",
            "Contact": {
                "Name": "Maria Anders",
                "Title": "Sales Representative"
            },
            "Address": {
                "Line1": "Obere Str. 57",
                "Line2": null,
                "City": "Berlin",
                "Region": null,
                "PostalCode": "12209",
                "Country": "Germany",
                "Location": {
                    "Latitude": 53.24939,
                    "Longitude": 14.43286
                }
            },
            "Phone": "030-0074321",
            "Fax": "030-0076545",
            "@metadata": {
                "@collection": "Companies",
                "@change-vector": "A:3-W33iO0zJC0qZKWh6fjnp6A",
                "@id": "companies/1-A",
                "@last-modified": "2018-07-27T12:11:53.0182893Z"
            }
        },
        {
            "Max": 8,
            "@metadata": {
                "@collection": "@hilo",
                "@change-vector": "A:1-W33iO0zJC0qZKWh6fjnp6A",
                "@id": "Raven/Hilo/categories",
                "@last-modified": "2018-07-27T12:11:53.0145929Z"
            }
        }
    ]
}
{CODE-BLOCK/}

---

### pageSize

Retrieve the first document.  
cURL request:  

{CODE-BLOCK: bash}
curl -X GET "http://live-test.ravendb.net/databases/Example/docs?pageSize=1"
{CODE-BLOCK/}

Response:  

{CODE-BLOCK: Http}
HTTP/1.1 200 OK
Server: nginx
Date: Thu, 10 Oct 2019 16:33:31 GMT
Content-Type: application/json; charset=utf-8
Transfer-Encoding: chunked
Connection: keep-alive
Content-Encoding: gzip
ETag: "A:2134-W33iO0zJC0qZKWh6fjnp6A, A:1887-0N64iiIdYUKcO+yq1V0cPA, A:6214-xwmnvG1KBkSNXfl7/0yJ1A"
Vary: Accept-Encoding
Raven-Server-Version: 4.2.4.42

{
    "Results": [
        {
            "LastName": "Callahan",
            "FirstName": "Laura",
            "Title": "Inside Sales Coordinator",
            "Address": {
                "Line1": "4726 - 11th Ave. N.E.",
                "Line2": null,
                "City": "Seattle",
                "Region": "WA",
                "PostalCode": "98105",
                "Country": "USA",
                "Location": {
                    "Latitude": 47.664164199999988,
                    "Longitude": -122.3160148
                }
            },
            "HiredAt": "1994-03-05T00:00:00.0000000",
            "Birthday": "1958-01-09T00:00:00.0000000",
            "HomePhone": "(206) 555-1189",
            "Extension": "2344",
            "ReportsTo": "employees/2-A",
            "Notes": [
                "Laura received a BA in psychology from the University of Washington.  She has also completed a course in business French.  She reads and writes French."
            ],
            "Territories": [
                "19428",
                "44122",
                "45839",
                "53404"
            ],
            "@metadata": {
                "@attachments": [
                    {
                        "Name": "photo.jpg",
                        "Hash": "8dte+O8Ds9RJx8dKruWurqapAojM/ZxjHBMst9wm5sI=",
                        "ContentType": "image/jpeg",
                        "Size": 14446
                    }
                ],
                "@collection": "Employees",
                "@change-vector": "A:2134-W33iO0zJC0qZKWh6fjnp6A",
                "@flags": "HasAttachments",
                "@id": "employees/8-A",
                "@last-modified": "2018-07-27T12:26:25.0179915Z"
            }
        }
    ]
}
{CODE-BLOCK/}

---

### metadataOnly

Skip first 123 documents, take the next 5, and retrieve only the metadata of each document.  
cURL request:  

{CODE-BLOCK: bash}
curl -X GET "http://live-test.ravendb.net/databases/Example/docs?
                start=123
                &pageSize=5
                &metadataOnly=true"
{CODE-BLOCK/}
Linebreaks are added for clarity.  

Response:  

{CODE-BLOCK: Http}
HTTP/1.1 200 OK
Server: nginx
Date: Thu, 10 Oct 2019 16:50:00 GMT
Content-Type: application/json; charset=utf-8
Connection: keep-alive
ETag: "A:2134-W33iO0zJC0qZKWh6fjnp6A, A:1887-0N64iiIdYUKcO+yq1V0cPA, A:6214-xwmnvG1KBkSNXfl7/0yJ1A"
Vary: Accept-Encoding
Raven-Server-Version: 4.2.4.42
Content-Length: 918

{
    "Results": [
        {
            "@metadata": {
                "@collection": "Orders",
                "@change-vector": "A:1871-W33iO0zJC0qZKWh6fjnp6A",
                "@flags": "HasRevisions",
                "@id": "orders/728-A",
                "@last-modified": "2018-07-27T12:11:53.1753957Z"
            }
        },
        {
            "@metadata": {
                "@collection": "Orders",
                "@change-vector": "A:1869-W33iO0zJC0qZKWh6fjnp6A",
                "@flags": "HasRevisions",
                "@id": "orders/727-A",
                "@last-modified": "2018-07-27T12:11:53.1751418Z"
            }
        },
        {
            "@metadata": {
                "@collection": "Orders",
                "@change-vector": "A:1867-W33iO0zJC0qZKWh6fjnp6A",
                "@flags": "HasRevisions",
                "@id": "orders/726-A",
                "@last-modified": "2018-07-27T12:11:53.1749721Z"
            }
        },
        {
            "@metadata": {
                "@collection": "Orders",
                "@change-vector": "A:1865-W33iO0zJC0qZKWh6fjnp6A",
                "@flags": "HasRevisions",
                "@id": "orders/725-A",
                "@last-modified": "2018-07-27T12:11:53.1747646Z"
            }
        },
        {
            "@metadata": {
                "@collection": "Orders",
                "@change-vector": "A:1863-W33iO0zJC0qZKWh6fjnp6A",
                "@flags": "HasRevisions",
                "@id": "orders/724-A",
                "@last-modified": "2018-07-27T12:11:53.1745710Z"
            }
        }
    ]
}
{CODE-BLOCK/}

{PANEL/}

## Related Articles  

### Getting Started  

- [About Examples](../../../start/about-examples)  
<br/>
### Client API  

##### Commands

- [Documents: Get](../../../client-api/commands/documents/get)

##### REST API

- [Get Documents by ID](../../../client-api/rest-api/document-commands/get-documents-by-id)  
- [Get Documents by Prefix](../../../client-api/rest-api/document-commands/get-documents-by-prefix)  
- [Put a Document](../../../client-api/rest-api/document-commands/put-documents)  
- [Delete a Document](../../../client-api/rest-api/document-commands/delete-document)  
- [Batch Commands](../../../client-api/rest-api/document-commands/batch-commands)  
<br/>
### Server  

- [Change Vector](../../../server/clustering/replication/change-vector)  
