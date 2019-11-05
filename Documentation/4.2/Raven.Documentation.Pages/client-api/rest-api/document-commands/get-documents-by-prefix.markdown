# Get Documents by Prefix

---

{NOTE: }  

* Use this endpoint with the **`GET`** method to retrieve documents from the database by a common prefix in their document IDs:  
`<server URL>/databases/<database name>/docs?startsWith=<document ID prefix>`  

* Query parameters can be used to filter and page the results.  

* In this page:  
  * [Basic Example](../../../client-api/rest-api/document-commands/get-documents-by-prefix#basic-example)  
  * [Request Format](../../../client-api/rest-api/document-commands/get-documents-by-prefix#request-format)  
  * [Response Format](../../../client-api/rest-api/document-commands/get-documents-by-prefix#response-format)  
  * [More Examples](../../../client-api/rest-api/document-commands/get-documents-by-prefix#more-examples)  
      * [Get Using `matches`](../../../client-api/rest-api/document-commands/get-documents-by-prefix#get-using)  
      * [Get Using `matches` and `exclude`](../../../client-api/rest-api/document-commands/get-documents-by-prefix#get-usingand)  
      * [Get Using `startAfter`](../../../client-api/rest-api/document-commands/get-documents-by-prefix#get-using-1)  
      * [Page Results](../../../client-api/rest-api/document-commands/get-documents-by-prefix#page-results)  
      * [Get Document Metadata Only](../../../client-api/rest-api/document-commands/get-documents-by-prefix#get-document-metadata-only)
{NOTE/}  

---

{PANEL:Basic Example}

This is a cURL request to a database named "Example" on our [playground server](http://live-test.ravendb.net), to retrieve all documents 
whose IDs begin with the prefix "ship":  

{CODE-BLOCK: bash}
curl -X GET "http://live-test.ravendb.net/databases/Example/docs?startsWith=ship"
{CODE-BLOCK/}

Response:  

{CODE-BLOCK: http}
HTTP/1.1 200 OK
Server: nginx
Date: Tue, 10 Sep 2019 15:25:34 GMT
Content-Type: application/json; charset=utf-8
Transfer-Encoding: chunked
Connection: keep-alive
Content-Encoding: gzip
ETag: "A:2137-pIhs+72n6USJoZ5XIvTHvQ"
Vary: Accept-Encoding
Raven-Server-Version: 4.2.4.42

{
    "Results": [
        {
            "Name": "Speedy Express",
            "Phone": "(503) 555-9831",
            "@metadata": {
                "@collection": "Shippers",
                "@change-vector": "A:349-k50KTOC5G0mfVXKjomTNFQ",
                "@id": "shippers/1-A",
                "@last-modified": "2018-07-27T12:11:53.0317375Z"
            }
        },
        {
            "Name": "United Package",
            "Phone": "(503) 555-3199",
            "@metadata": {
                "@collection": "Shippers",
                "@change-vector": "A:351-k50KTOC5G0mfVXKjomTNFQ",
                "@id": "shippers/2-A",
                "@last-modified": "2018-07-27T12:11:53.0317596Z"
            }
        },
        {
            "Name": "Federal Shipping",
            "Phone": "(503) 555-9931",
            "@metadata": {
                "@collection": "Shippers",
                "@change-vector": "A:353-k50KTOC5G0mfVXKjomTNFQ",
                "@id": "shippers/3-A",
                "@last-modified": "2018-07-27T12:11:53.0317858Z"
            }
        }
    ]
}
{CODE-BLOCK/}

{PANEL/}

{PANEL: Request Format}

This is the general form of a cURL request that uses all parameters:  

{CODE-BLOCK: batch}
curl -X GET "<server URL>/databases/<database name>/docs?
            startsWith=<prefix>
            &matches=<suffix>|<suffix>|...
            &exclude=<suffix>|<suffix>|...
            &startAfter=<document ID>
            &start=<integer>
            &pageSize=<integer>
            &metadata=<boolean>"
--header If-None-Match: <hash>
{CODE-BLOCK/}
Linebreaks are added for clarity.  
<br/>
####Query String Parameters

| Parameter | Description | Required  |
| - | - | - |
| **startsWith** | Retrieve all documents whose IDs begin with this string. If the value of this parameter is left empty, all documents in the database are retrieved. | Yes |
| **matches** | Retrieve documents whose IDs are exactly `<startsWith>`+`<matches>`. Accepts multiple values separated by a pipe character: ' \| ' . Use `?` to represent any single character, and `*` to represent any string. | No |
| **exclude** | _Exclude_ documents whose IDs are exactly `<startsWith>`+`<exclude>`. Accepts multiple values separated by a pipe character: ' \| ' . Use `?` to represent any single character, and `*` to represent any string. | No |
| **startAfter** | Retrieve only the results after the first document ID that begins with this prefix. | No |
| **start** | Number of results to skip. | No |
| **pageSize** | Maximum number of results to retrieve. | No |
| **metadataOnly** | Set this parameter to `true` to retrieve only the document metadata for each result. | No |

####Headers

| Header | Description | Required |
| - | - | - |
| **If-None-Match** | This header takes a hash representing the previous results of an **identical** request. The hash is found in the response header `ETag`. If the results were not modified since the previous request, the server responds with http status code `304` and the requested documents are retrieved from a local cache rather than over the network. | No |

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
| **ETag** | Hash representing the state of these results. If another, **identical** request is made, this hash can be sent in the `If-None-Match` header to check whether the retrieved documents have been modified since the last response. If none were modified. |
| **Raven-Server-Version** | Version of RavenDB that the responding server is running |

#### Body

Retrieved documents are sorted in ascending [lexical order](https://en.wikipedia.org/wiki/Lexicographical_order) of their document IDs. A retrieved document is identical in 
contents and format to the document stored in the server - unless the `metadataOnly` parameter is set to `true`.  

This is the general JSON format of the response body:  

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
Linebreaks are added for clarity.  
{CODE-BLOCK/}

{PANEL/}

{PANEL: More Examples}

[About Northwind](../../../getting-started/about-examples), the database used in our examples.

In this section:  

* [Get Using `matches`](../../../client-api/rest-api/document-commands/get-documents-by-prefix#get-using)  
* [Get Using `matches` and `exclude`](../../../client-api/rest-api/document-commands/get-documents-by-prefix#get-usingand)  
* [Get Using `startAfter`](../../../client-api/rest-api/document-commands/get-documents-by-prefix#get-using-1)  
* [Page Results](../../../client-api/rest-api/document-commands/get-documents-by-prefix#page-results)  
* [Get Document Metadata Only](../../../client-api/rest-api/document-commands/get-documents-by-prefix#get-document-metadata-only)

---

### Get Using&nbsp;`matches`

cURL request:  

{CODE-BLOCK: bash}
curl -X GET "http://live-test.ravendb.net/databases/Example/docs?
            startsWith=shipp
            &matches=ers/3-A|ers/1-A"
{CODE-BLOCK/}
Linebreaks are added for clarity.  

Response:  

{CODE-BLOCK: Http}
HTTP/1.1 200 OK
Server: nginx
Date: Thu, 12 Sep 2019 10:57:58 GMT
Content-Type: application/json; charset=utf-8
Transfer-Encoding: chunked
Connection: keep-alive
Content-Encoding: gzip
ETag: "A:5972-k50KTOC5G0mfVXKjomTNFQ"
Vary: Accept-Encoding
Raven-Server-Version: 4.2.4.42

{
    "Results": [
        {
            "Name": "Speedy Express",
            "Phone": "(503) 555-9831",
            "@metadata": {
                "@collection": "Shippers",
                "@change-vector": "A:349-k50KTOC5G0mfVXKjomTNFQ",
                "@id": "shippers/1-A",
                "@last-modified": "2018-07-27T12:11:53.0317375Z"
            }
        },
        {
            "Name": "Federal Shipping",
            "Phone": "(503) 555-9931",
            "@metadata": {
                "@collection": "Shippers",
                "@change-vector": "A:353-k50KTOC5G0mfVXKjomTNFQ",
                "@id": "shippers/3-A",
                "@last-modified": "2018-07-27T12:11:53.0317858Z"
            }
        }
    ]
}
{CODE-BLOCK/}

---

### Get Using&nbsp;`matches`&nbsp;and&nbsp;`exclude`

cURL request:  

{CODE-BLOCK: bash}
curl -X GET "http://live-test.ravendb.net/databases/Example/docs?
            startsWith=shipp
            &matches=ers/3-A|ers/1-A
            &exclude=ers/3-A"
{CODE-BLOCK/}
Linebreaks are added for clarity.  

Response:  

{CODE-BLOCK: Http}
HTTP/1.1 200 OK
Server: nginx
Date: Thu, 12 Sep 2019 12:24:50 GMT
Content-Type: application/json; charset=utf-8
Transfer-Encoding: chunked
Connection: keep-alive
Content-Encoding: gzip
ETag: "A:5972-k50KTOC5G0mfVXKjomTNFQ"
Vary: Accept-Encoding
Raven-Server-Version: 4.2.4.42

{
    "Results": [
        {
            "Name": "Speedy Express",
            "Phone": "(503) 555-9831",
            "@metadata": {
                "@collection": "Shippers",
                "@change-vector": "A:349-k50KTOC5G0mfVXKjomTNFQ",
                "@id": "shippers/1-A",
                "@last-modified": "2018-07-27T12:11:53.0317375Z"
            }
        }
    ]
}
{CODE-BLOCK/}

---

### Get Using&nbsp;`startAfter`

cURL request:  

{CODE-BLOCK: bash}
curl -X GET "http://live-test.ravendb.net/databases/Example/docs?
            startsWith=shipp
            startAfter=shippers/1-A"
{CODE-BLOCK/}
Linebreaks are added for clarity.  

Response:  

{CODE-BLOCK: Http}
HTTP/1.1 200 OK
Server: nginx
Date: Thu, 12 Sep 2019 12:37:39 GMT
Content-Type: application/json; charset=utf-8
Transfer-Encoding: chunked
Connection: keep-alive
Content-Encoding: gzip
ETag: "A:5972-k50KTOC5G0mfVXKjomTNFQ"
Vary: Accept-Encoding
Raven-Server-Version: 4.2.4.42

{
    "Results": [
        {
            "Name": "United Package",
            "Phone": "(503) 555-3199",
            "@metadata": {
                "@collection": "Shippers",
                "@change-vector": "A:351-k50KTOC5G0mfVXKjomTNFQ",
                "@id": "shippers/2-A",
                "@last-modified": "2018-07-27T12:11:53.0317596Z"
            }
        },
        {
            "Name": "Federal Shipping",
            "Phone": "(503) 555-9931",
            "@metadata": {
                "@collection": "Shippers",
                "@change-vector": "A:353-k50KTOC5G0mfVXKjomTNFQ",
                "@id": "shippers/3-A",
                "@last-modified": "2018-07-27T12:11:53.0317858Z"
            }
        }
    ]
}
{CODE-BLOCK/}

---

### Page Results

cURL request:  

{CODE-BLOCK: bash}
curl -X GET "http://live-test.ravendb.net/databases/Example/docs?
            startsWith=product
            &start=50
            &pageSize=2"
{CODE-BLOCK/}
Linebreaks are added for clarity.  

Response:  

{CODE-BLOCK: Http}
HTTP/1.1 200 OK
Server: nginx
Date: Thu, 12 Sep 2019 13:17:44 GMT
Content-Type: application/json; charset=utf-8
Transfer-Encoding: chunked
Connection: keep-alive
Content-Encoding: gzip
ETag: "A:5972-k50KTOC5G0mfVXKjomTNFQ"
Vary: Accept-Encoding
Raven-Server-Version: 4.2.4.42

{
    "Results": [
        {
            "Name": "Pâté chinois",
            "Supplier": "suppliers/25-A",
            "Category": "categories/6-A",
            "QuantityPerUnit": "24 boxes x 2 pies",
            "PricePerUnit": 24.0000,
            "UnitsInStock": 25,
            "UnitsOnOrder": 115,
            "Discontinued": false,
            "ReorderLevel": 20,
            "@metadata": {
                "@collection": "Products",
                "@change-vector": "A:8170-k50KTOC5G0mfVXKjomTNFQ, A:1887-0N64iiIdYUKcO+yq1V0cPA, A:6214-xwmnvG1KBkSNXfl7/0yJ1A",
                "@id": "products/55-A",
                "@last-modified": "2018-07-27T12:11:53.0303784Z"
            }
        },
        {
            "Name": "Gnocchi di nonna Alice",
            "Supplier": "suppliers/26-A",
            "Category": "categories/5-A",
            "QuantityPerUnit": "24 - 250 g pkgs.",
            "PricePerUnit": 38.0000,
            "UnitsInStock": 26,
            "UnitsOnOrder": 21,
            "Discontinued": false,
            "ReorderLevel": 30,
            "@metadata": {
                "@collection": "Products",
                "@change-vector": "A:8172-k50KTOC5G0mfVXKjomTNFQ, A:1887-0N64iiIdYUKcO+yq1V0cPA, A:6214-xwmnvG1KBkSNXfl7/0yJ1A",
                "@id": "products/56-A",
                "@last-modified": "2018-07-27T12:11:53.0304385Z"
            }
        }
    ]
}
{CODE-BLOCK/}

Note that the document ID numbers are 55 and 56 rather than the expected 51 and 52 because results are sorted in lexical order.

---

### Get Document Metadata Only

cURL request:  

{CODE-BLOCK: bash}
curl -X GET "http://live-test.ravendb.net/databases/Example/docs?
            startsWith=regio
            &metadataOnly=true"
{CODE-BLOCK/}
Linebreaks are added for clarity.  

Response:  

{CODE-BLOCK: Http}
HTTP/1.1 200 OK
Server: nginx
Date: Thu, 12 Sep 2019 13:44:16 GMT
Content-Type: application/json; charset=utf-8
Transfer-Encoding: chunked
Connection: keep-alive
Content-Encoding: gzip
ETag: "A:5972-k50KTOC5G0mfVXKjomTNFQ"
Vary: Accept-Encoding
Raven-Server-Version: 4.2.4.42

{
    "Results": [
        {
            "@metadata": {
                "@collection": "Regions",
                "@change-vector": "A:9948-k50KTOC5G0mfVXKjomTNFQ, A:1887-0N64iiIdYUKcO+yq1V0cPA, A:6214-xwmnvG1KBkSNXfl7/0yJ1A",
                "@id": "regions/1-A",
                "@last-modified": "2018-07-27T12:11:53.2016685Z"
            }
        },
        {
            "@metadata": {
                "@collection": "Regions",
                "@change-vector": "A:9954-k50KTOC5G0mfVXKjomTNFQ, A:1887-0N64iiIdYUKcO+yq1V0cPA, A:6214-xwmnvG1KBkSNXfl7/0yJ1A",
                "@id": "regions/2-A",
                "@last-modified": "2018-07-27T12:11:53.2021826Z"
            }
        },
        {
            "@metadata": {
                "@collection": "Regions",
                "@change-vector": "A:9950-k50KTOC5G0mfVXKjomTNFQ, A:1887-0N64iiIdYUKcO+yq1V0cPA, A:6214-xwmnvG1KBkSNXfl7/0yJ1A",
                "@id": "regions/3-A",
                "@last-modified": "2018-07-27T12:11:53.2018086Z"
            }
        },
        {
            "@metadata": {
                "@collection": "Regions",
                "@change-vector": "A:9952-k50KTOC5G0mfVXKjomTNFQ, A:1887-0N64iiIdYUKcO+yq1V0cPA, A:6214-xwmnvG1KBkSNXfl7/0yJ1A",
                "@id": "regions/4-A",
                "@last-modified": "2018-07-27T12:11:53.2019223Z"
            }
        }
    ]
}
{CODE-BLOCK/}

{PANEL/}

## Related Articles  

### Getting Started  

- [About Examples](../../../getting-started/about-examples)  

### Client API  

- [Get All Documents](../../../client-api/rest-api/document-commands/get-all-documents)  
- [Get Documents by ID](../../../client-api/rest-api/document-commands/get-documents-by-id)  
- [Put Documents](../../../client-api/rest-api/document-commands/put-documents)  
- [Delete Document](../../../client-api/rest-api/document-commands/delete-document)  
- [Batch Commands](../../../client-api/rest-api/document-commands/batch-commands)  
