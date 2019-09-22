# Send Multiple Commands in a Batch

---

{NOTE: }

* Use this endpoint with the **`POST`** method to send multiple commands in one request:  
`http://<server URL>/databases/<database name>/bulk_docs`  

* All the commands in the batch will either succeed or fail as a **single transaction**. Changes will not be visible until the entire batch completes.  

* Options are available to make the server wait for indexing and replication to complete before returning.  

* In this page:  
  * [Basic Example](../../../client-api/rest-api/document-commands/batch-commands#basic-example)  
  * [Request Format](../../../client-api/rest-api/document-commands/batch-commands#request-format)  
  * [Response Format](../../../client-api/rest-api/document-commands/batch-commands#response-format)  
  * [More Examples](../../../client-api/rest-api/document-commands/batch-commands#more-examples)  
{NOTE/}

---

{PANEL: Basic Example}

This is a cURL request to our [playground server](http://live-test.ravendb.net) to a database named "Example".  
It batches two commands:  

1. Upload a new document called "person/1".  
2. Execute a [patch](../../../client-api/operations/patching/single-document) on that same document.  

{CODE-BLOCK: bash}
curl -X POST http://live-test.ravendb.net/databases/Example/bulk_docs \
-H 'Content-Type: application/json' \
-d '{
    "Commands": [
        {
            "Id": "person/1",
            "ChangeVector": null,
            "Document": {
                "Name": "John Smith"
            },
            "Type": "PUT"
        },
        {
            "Id": "person/1",
            "ChangeVector": null,
            "Patch": {
                "Script": "this.Name = 'Jane Doe';",
                "Values": {}
            },
            "Type": "PATCH"
        }
    ]
}'
{CODE-BLOCK/}

Linebreaks are added for clarity.  

Response:  

{CODE-BLOCK: http}
HTTP/1.1 201 Created
Server: nginx
Date: Sun, 15 Sep 2019 14:12:30 GMT
Content-Type: application/json; charset=utf-8
Transfer-Encoding: chunked
Connection: keep-alive
Content-Encoding: gzip
Vary: Accept-Encoding
Raven-Server-Version: 4.2.4.42

{
    "Results": [
        {
            "Type": "PUT",
            "@id": "person/1",
            "@collection": "@empty",
            "@change-vector": "A:1-urx5nDNUT06FCpCon1wCyA",
            "@last-modified": "2019-09-15T14:12:30.0425811"
        },
        {
            "Id": "person/1",
            "ChangeVector": "A:2-urx5nDNUT06FCpCon1wCyA",
            "LastModified": "2019-09-15T14:12:30.0495095",
            "Type": "PATCH",
            "PatchStatus": "Patched",
            "Debug": null
        }
    ]
}
{CODE-BLOCK/}

{PANEL/}

{PANEL: Request Format}

This is the general form of a cURL request for a batch that does _not_ include a put attachment command (see the Put Attachment Command format 
[below](../../../client-api/rest-api/document-commands/batch-commands#put-attachment-command)):  

{CODE-BLOCK: bash}
curl -X POST http<server URL>/databases/<database name>/bulk_docs?<batch options> \
-H 'Content-Type: application/json' \
-d '{
    "Commands": [
        { },
        ...
    ]
}'
{CODE-BLOCK/}

Linebreaks are added for clarity.  

#### Header

The header `Content-Type` is required and takes one of two values:  

* `application/json` - if the batch _does not_ include a Put Attachment Command.  
* `multipart/mixed; boundary=<separator>` - if the batch [_does_](../../../client-api/rest-api/document-commands/batch-commands#put-attachment-command) include a put attachment command. 
The `separator` is used to demarcate the attachment streams.  

The following commands can be sent using the batch command:  

* [Put Document Command](../../../client-api/rest-api/document-commands/batch-commands#put-document-command)  
* [Patch Document Command](../../../client-api/rest-api/document-commands/batch-commands#patch-document-command)  
* [Delete Document Command](../../../client-api/rest-api/document-commands/batch-commands#delete-document-command)  
* [Delete by Prefix Command](../../../client-api/rest-api/document-commands/batch-commands#delete-by-prefix-command)  
* [Put Attachment Command](../../../client-api/rest-api/document-commands/batch-commands#put-attachment-command)  
* [Delete Attachment Command](../../../client-api/rest-api/document-commands/batch-commands#delete-attachment-command)  

The query string takes [batch options](../../../client-api/rest-api/document-commands/batch-commands#batch-options), which can make the server wait for 
indexing and replication to finish before responding.  

---

### Put Document Command

Upload a new document or update an existing document.  

Format within the `Commands` array in the [request body](../../../client-api/rest-api/document-commands/batch-commands#request-format):  

{CODE-BLOCK: JavaScript}
{
    "Id": "<document ID>",
    "ChangeVector": "<expected change vector>",
    "Document": {
        <document content>
    },
    "Type": "PUT",
    "ForceRevisionCreationStrategy": "<none / before>"
}
{CODE-BLOCK/}

| Parameter | Description | Required |
| - | - | - |
| **Id** | ID of document to create or update | Required |
| **ChangeVector** | If updating an existing document - that document's expected change vector. If it does not match the server-side change vector a concurrency exception is thrown. | Optional |
| **Document** | JSON Document to create, or to replace existing document | Required |
| **Type** | Set to `PUT` | Required |
| **ForceRevisionCreationStrategy** | Can be set to either `none` or `before` | Optional |

---

### Patch Document Command

Update a document. A [patch](../../../client-api/operations/patching/single-document) is executed on the server side and does not involve loading the 
document, thus avoiding the cost of sending the entire document in a round trip over the network.  

Format within the `Commands` array in the [request body](../../../client-api/rest-api/document-commands/batch-commands#request-format):  

{CODE-BLOCK: JavaScript}
{
    "Id": "<document ID>",
    "ChangeVector": "<expected change vector>",
    "Patch": {
        "Script": "<javascript code using $<argument name> >",
        "Values": {
            "<argument name>": "<value>",
            ...
        }
    },
    "PatchIfMissing": {
        "Script": "<javascript code>",
        "Values": {
            <arguments>
        }
    },
    "Type": "PATCH"
}
{CODE-BLOCK/}

| Parameter | Description | Required |
| - | - | - |
| **Id** | ID of document on which to execute the patch | Required |
| **ChangeVector** | The document's expected change vector. If it does not match the server-side change vector a concurrency exception is thrown. | Optional |
| **Patch** | Contains a script that modifies the specified document. [Details below](../../../client-api/rest-api/document-commands/batch-commands#patch-request). | Required |
| **PatchIfMissing** | Contains an alternative script to be executed if no document with the given ID is found. This will create a new document with the given ID. [Details below](../../../client-api/rest-api/document-commands/batch-commands#patch-request). | Optional |
| **Type** | Set to `PATCH` | Required |

#### Patch Request

Using scripts with arguments allows RavenDB to cache scripts and boost performance.

| Sub-Parameter | Description | Required |
| - | - | - |
| **Script** | Commands in javascript to perform on the document. Use arguments from `Values` with a `$` prefix, i.e. `$<argument name>`. | Required |
| **Values** | Arguments that can be used in the script. | Optional |

---

### Delete Document Command

Delete a document by its ID.  

Format within the `Commands` array in the [request body](../../../client-api/rest-api/document-commands/batch-commands#request-format):  

{CODE-BLOCK: JavaScript}
{
    "Id": "<document ID>",
    "ChangeVector": "<expected change vector>",
    "Type": "DELETE"
}
{CODE-BLOCK/}

| Parameter | Description | Required |
| - | - | - |
| **Id** | ID of document to delete (only one can be deleted per command) | Required |
| **ChangeVector** | The document's expected change vector. If it does not match the server-side change vector a concurrency exception is thrown. | Optional |
| **Type** | Set to `DELETE` | Required |

---

### Delete by Prefix Command

Delete all documents whose IDs begin with a certain prefix.  

Format within the `Commands` array in the [request body](../../../client-api/rest-api/document-commands/batch-commands#request-format):  

{CODE-BLOCK: JavaScript}
{
    "Id": "<document ID prefix>",
    "IdPrefixed": true,
    "Type": "DELETE"
}
{CODE-BLOCK/}

| Parameter | Description | Required |
| - | - | - |
| **Id** | A prefix of some document IDs. Documents whose IDs begin with this prefix will be deleted | Required |
| **IdPrefixed** | Set to `true`. Distinguishes this as a delete by prefix command rather than the [delete by document ID command](../../../client-api/rest-api/document-commands/batch-commands#delete-command). | Required |
| **Type** | Set to `DELETE` | Required |

---

### Put Attachment Command

Add an [attachment](../../../client-api/session/attachments/what-are-attachments) to a document, or update an existing attachment.  

If a batch contains a Put Attachment Command, the cURL format of the request is slightly different from a batch that doesn't.  
The `Content-Type` header takes `multipart/mixed; boundary="<separator>"` instead of the default `application/json`.  
The body contains the `Commands` array followed by each of the attachments, passed in the form of binary streams. The attachment streams come in the 
same order as their respective Put Attachment Commands within the `Commands` array. The `separator` demarcates these sections.  

The general form of a cURL request:  

{CODE-BLOCK: bash}
curl -X POST http://<server URL>/databases/<database name>/bulk_docs \
-H Content-Type: multipart/mixed; boundary="<separator>" \
-d
--<separator>
{
    "Commands":[ 
        {
            "Id": "<document ID>",
            "Name": "<attachment name>",
            "ContentType": "<attachment MIME type>"
            "ChangeVector": "<expected change vector>",
            "Type": "AttachmentPUT"
        },
        ...
    ]
}
--<separator>
Command-Type: AttachmentStream

<binary stream>
--<separator>
...
--<separator>--
{CODE-BLOCK/}

| Parameter | Description | Required |
| - | - | - |
| **boundary** | A `separator`, a string that does not appear within the contents of the body | Required |
| **Id** | Document ID | Required |
| **Name** | Name of attachment to create or update | Required |
| **ContentType** | Mime type of the attachment |
| **ChangeVector** | The document's expected change vector. If it does not match the server-side change vector a concurrency exception is thrown. | Optional |
| **Type** | Set to `AttachmentPUT` | Required |


---

### Delete Attachment Command

Delete an attachment in a certain document.  

Format within the `Commands` array in the [request body](../../../client-api/rest-api/document-commands/batch-commands#request-format):  

{CODE-BLOCK: JavaScript}
{
    "Id": "<document ID>",
    "Name": "<attachment name>",
    "ChangeVector": "<expected change vector>",
    "Type": "AttachmentDELETE"
}
{CODE-BLOCK/}

| Parameter | Description | Required |
| - | - | - |
| **Id** | ID of document for which to delete the attachment | Required |
| Name | Name of the attachment to delete | Required |
| **ChangeVector** | The document's expected change vector. If it does not match the server-side change vector a concurrency exception is thrown. | Optional |
| **Type** | Set to `AttachmentDELETE` | Required |

---

### Batch Options

These options make the server wait until indexing or replication have completed before responding. If these have not completed before a specified 
amount of time has passed, the server can either respond as normal, or throw an exception.  

{CODE-BLOCK: bash}
curl -X POST 'http<server URL>/databases/<database name>/bulk_docs?<option>=<value>&<option>=<value>&... \
-H Content-Type \
-d '{ }'
{CODE-BLOCK/}

#### Index Options

| Query Parameter | Type | Description |
| - | - | - |
| **waitForIndexesTimeout** | `TimeSpan` | The amount of time to wait for indexing to complete. [Format of `TimeSpan`](https://docs.microsoft.com/en-us/dotnet/api/system.timespan). |
| **waitForIndexThrow** | `boolean` | Set to `true` to throw an exception if the the indexing doesn't complete before `waitForIndexesTimeout`.<br/>Set to `false` to recieve the normal response body. |
| **waitForSpecificIndex** | `string[]` | If this parameter is used, wait only for the listed indexes to finish updating, rather than all indexes. |

#### Replication Options

| Query Parameter | Type | Description |
| - | - | - |
| **waitForReplicasTimeout** | `TimeSpan` | The amount of time to wait for replication to complete. [Format of `TimeSpan`](https://docs.microsoft.com/en-us/dotnet/api/system.timespan). |
| **throwOnTimeoutInWaitForReplicas** | `boolean` | Set this parameter to `true` to throw an exception if the the replication doesn't complete before `waitForReplicasTimeout`.<br/>Set to `false` to recieve the normal response body. |
| **numberOfReplicasToWaitFor** | `int` | The number of replicas that should be made before `waitForReplicasTimeout`. Set this parameter to `majority` to wait until the data has been replicated to a majority of the nodes in the database group. Default = `1`. |

{PANEL/}

{PANEL: Response Format}

### Http Status Codes

| Code | Description |
| - | - |
| `201` | The transaction was successfully completed. |
| `408` | The time specified by the options `waitForIndexThrow` or `waitForReplicasTimeout` passed before indexing or replication completed respectively, and an exception was thrown. This only happens if `throwOnTimeoutInWaitForReplicas` or `waitForIndexThrow` are set to `true`. |
| `409` | The specified change vector did not match the server-side change vector, or a change vector was specified for a document that does not exist. A concurrency exception is thrown. |
| `500` | Invalid request, such as a put attachment command for a document that does not exist. |

### Response Body

{CODE-BLOCK: JavaScript}
{
    "Results":[
        { },
        ...
    ]
}
{CODE-BLOCK/}

* Format within the `Results` array in the response body:  
  * [Put Document Command](../../../client-api/rest-api/document-commands/batch-commands#put-document-command-1)  
  * [Patch Document Command](../../../client-api/rest-api/document-commands/batch-commands#patch-document-command-1)  
  * [Delete Document Command](../../../client-api/rest-api/document-commands/batch-commands#delete-document-command-1)  
  * [Delete by Prefix Command](../../../client-api/rest-api/document-commands/batch-commands#delete-by-prefix-command-1)  
  * [Put Attachment Command](../../../client-api/rest-api/document-commands/batch-commands#put-attachment-command-1)  
  * [Delete Attachment Command](../../../client-api/rest-api/document-commands/batch-commands#delete-attachment-command-1)  

---

### Put Document Command

{CODE-BLOCK: JavaScript}
{
    "Type": "PUT",
    "@id": "<document ID>",
    "@collection": "<collection name>",
    "@change-vector": "<current change vector>",
    "@last-modified": "<date and time UTC>"
}
{CODE-BLOCK/}

| Parameter | Description |
| - | - | - |
| **Type** | Same as the `Type` of the command sent - in this case `PUT`. |
| **@id** | The ID of the document that has been created or deleted. |
| **@collection** | Name of the [collection](../../../client-api/faq/what-is-a-collection) that contains the document. If none was specified, the collection will be `@empty`. |
| **@change-vector** | The document's change vector after the command was executed. |

---

### Patch Document Command

{CODE-BLOCK: JavaScript}
{
    "@id": "<document ID>",
    "@change-vector": "<current change vector>",
    "@last-modified": "<date and time UTC>",
    "Type": "PATCH",
    "PatchStatus": "<patch status>",
    "Debug": null
}
{CODE-BLOCK/}

| Parameter | Description |
| - | - | - |
| **@id** | The ID of the document that has been created or deleted. |
| **@change-vector** | The document's change vector after the command was executed. Returns `null` if the command did not result in a modification to the document. |
| **@last-modified** | Date and time (UTC) of the most recent modification made to the document. |
| **Type** | Same as the `Type` of the command sent - in this case `PATCH`. |
| **PatchStatus** | See [below](../../../client-api/rest-api/document-commands/batch-commands#patchstatus) |
| **Debug** | Should always return `null` in the context of batch commands. |

---

#### PatchStatus

| Status | Description |
| - | - |
| **DocumentDoesNotExist** | No document with the specified ID exists. This will only be returned if no `PatchIfMissing` script was given. |
| **Created** | No document with the specified ID existed, so a new document was created with that ID and `PatchIfMissing` was applied. |
| **Patched** | The specified document was successfully patched. |
| **Skipped** | Should not appear in the context of batch commands. |
| **NotModified** | Patch was successful but did not result in a modification to the document. |

---

### Delete Document Command

{CODE-BLOCK: JavaScript}
{
    "Id": "<document ID>",
    "Type": "DELETE",
    "Deleted": <boolean>
}
{CODE-BLOCK/}

| Parameter | Description |
| - | - | - |
| **Id** | The ID of the document that has been created or deleted. |
| **Type** | Same as the `Type` of the command sent - in this case `DELETE`. |
| **Deleted** | `true` if the document was successfully deleted, `false` if not (for instance, because the specified document did not exist). |

---

### Delete by Prefix Command

{CODE-BLOCK: JavaScript}
{
    "Id": "<prefix>",
    "Type": "DELETE",
    "Deleted": <boolean>
}
{CODE-BLOCK/}

| Parameter | Description |
| - | - | - |
| **Id** | The document ID prefix of the documents which were deleted. |
| **Type** | Same as the `Type` of the command sent - in this case `DELETE`. |
| **Deleted** | `true` if the documents were successfully deleted, `false` if not (for instance, because no documents with the specified prefix exist). |

---

### Put Attachment Command

{CODE-BLOCK: JavaScript}
{
    "Id": "<document ID>",
    "Type": "AttachmentPUT",
    "Name": "<attachment name>",
    "ChangeVector": "<attachment change vector>",
    "Hash": "<Hash>",
    "ContentType": "<MIME type>",
    "Size": <attachment size in bytes>,
    "DocumentChangeVector": "<current change vector>"
}
{CODE-BLOCK/}

| Parameter | Description |
| - | - | - |
| **Id** | The document ID prefix of the documents which were deleted. |
| **Type** | Same as the `Type` of the command sent - in this case `AttachmentPUT`. |
| **Name** | Name of the attachment that was created or updated. |
| **ChangeVector** | A change vector specific to the _attachment_, distinct from the usual document change vector. Use this change vector in requests to update this attachment. |
| **Hash** | Hash representing the attachment. |
| **ContentType** | MIME type of the attachment. |
| **Size** | Size of the attachment in bytes. |
| **DocumentChangeVector** | The document's change vector after the command was executed. |

---

### Delete Attachment Command

{CODE-BLOCK: JavaScript}
{
    "Type": "AttachmentDELETE",
    "@id": "<document ID>",
    "Name": "<attachment name",
    "DocumentChangeVector": "<current change vector>"
}
{CODE-BLOCK/}

| Parameter | Description |
| - | - | - |
| **Type** | Same as the `Type` of the command sent - in this case `AttachmentDELETE`. |
| **@id** | The ID of the documents that was deleted. |
| **Name** | Name of the attachment that was deleted. |
| **DocumentChangeVector** | The document's change vector after the command was executed. |

{PANEL/}

{PANEL: More Examples}

* In this section:  
  * [Put Document Command](../../../client-api/rest-api/document-commands/batch-commands#put-document-command-2)  
  * [Patch Document Command](../../../client-api/rest-api/document-commands/batch-commands#patch-document-command-2)  
  * [Delete Document Command](../../../client-api/rest-api/document-commands/batch-commands#delete-document-command-2)  
  * [Delete by Prefix Command](../../../client-api/rest-api/document-commands/batch-commands#delete-by-prefix-command-2)  
  * [Put Attachment Command](../../../client-api/rest-api/document-commands/batch-commands#put-attachment-command-2)  
  * [Delete Attachment Command](../../../client-api/rest-api/document-commands/batch-commands#delete-attachment-command-2)  

---

###Put Document Command

Request:

{CODE-BLOCK: bash}
curl -X POST http://live-test.ravendb.net/databases/Example/bulk_docs \
-H Content-Type: application/json \
-d '{
    "Commands": [
        {
            "Id": "person/1",
            "ChangeVector": null,
            "Document": {
                "Name": "John Smith"
            },
            "Type": "PUT"
        }
    ]
}'
{CODE-BLOCK/}

Response:

{CODE-BLOCK: http}
HTTP/1.1 201 Created
Server:"nginx"
Date:"Wed, 18 Sep 2019 16:14:20 GMT"
Content-Type:"application/json; charset=utf-8"
Transfer-Encoding:"chunked"
Connection:"keep-alive"
Content-Encoding:"gzip"
Vary:"Accept-Encoding"
Raven-Server-Version:"4.2.4.42"

{
    "Results": [
        {
            "Type": "PUT",
            "@id": "person/1",
            "@collection": "@empty",
            "@change-vector": "A:5951-pITDlhlRaEeJh16dDBREzg, A:1887-0N64iiIdYUKcO+yq1V0cPA, A:6214-xwmnvG1KBkSNXfl7/0yJ1A",
            "@last-modified": "2019-09-18T16:14:20.5759532"
        }
    ]
}
{CODE-BLOCK/}

---

###Patch Document Command

Request:

{CODE-BLOCK: bash}
curl -X POST http://live-test.ravendb.net/databases/Example/bulk_docs \
-H Content-Type: application/json \
-d '{
    "Commands": [
        {
            "Id": "person/1",
            "ChangeVector": null,
            "Patch": {
                "Script": "this.Name = 'Jane Doe';",
                "Values": {}
            },
            "Type": "PATCH"
        }
    ]
}'
{CODE-BLOCK/}

Response:

{CODE-BLOCK: http}
HTTP/1.1 201 Created
Server:"nginx"
Date:"Wed, 18 Sep 2019 16:18:13 GMT"
Content-Type:"application/json; charset=utf-8"
Transfer-Encoding:"chunked"
Connection:"keep-alive"
Content-Encoding:"gzip"
Vary:"Accept-Encoding"
Raven-Server-Version:"4.2.4.42"

{
    "Results": [
        {
            "Id": "person/1",
            "ChangeVector": "A:5952-pITDlhlRaEeJh16dDBREzg, A:1887-0N64iiIdYUKcO+yq1V0cPA, A:6214-xwmnvG1KBkSNXfl7/0yJ1A",
            "LastModified": "2019-09-18T16:18:13.5745560",
            "Type": "PATCH",
            "PatchStatus": "Patched",
            "Debug": null
        }
    ]
}
{CODE-BLOCK/}

---

### Delete Document Command

Request:

{CODE-BLOCK: bash}
curl -X POST http://live-test.ravendb.net/databases/Example/bulk_docs \
-H Content-Type: application/json \
-d '{
    "Commands": [
        {
            "Id": "employees/1-A",
            "ChangeVector": null,
            "Type": "DELETE"
        }
	]
}'
{CODE-BLOCK/}

Response:

{CODE-BLOCK: http}
HTTP/1.1 201 Created
Server:"nginx"
Date:"Wed, 18 Sep 2019 16:30:15 GMT"
Content-Type:"application/json; charset=utf-8"
Transfer-Encoding:"chunked"
Connection:"keep-alive"
Content-Encoding:"gzip"
Vary:"Accept-Encoding"
Raven-Server-Version:"4.2.4.42"

{
    "Results": [
        {
            "Id": "employees/1-A",
            "Type": "DELETE",
            "Deleted": true,
            "ChangeVector": null
        }
    ]
}
{CODE-BLOCK/}

---

### Delete by Prefix Command

Request:

{CODE-BLOCK: bash}
curl -X POST http://live-test.ravendb.net/databases/Example/bulk_docs \
-H Content-Type: application/json \
-d '{
    "Commands": [
        {
            "Id": "employ",
            "ChangeVector": null,
            "IdPrefixed": true,
            "Type": "DELETE"
        }
	]
}'

{CODE-BLOCK/}

Response:

{CODE-BLOCK: http}
HTTP/1.1 201 Created
Server:"nginx"
Date:"Wed, 18 Sep 2019 16:32:16 GMT"
Content-Type:"application/json; charset=utf-8"
Transfer-Encoding:"chunked"
Connection:"keep-alive"
Content-Encoding:"gzip"
Vary:"Accept-Encoding"
Raven-Server-Version:"4.2.4.42"

{
    "Results": [
        {
            "Id": "employ",
            "Type": "DELETE",
            "Deleted": true
        }
    ]
}
{CODE-BLOCK/}

---

### Put Attachment Command

Request:

{CODE-BLOCK: bash}
curl -X POST http://live-test.ravendb.net/databases/Example/bulk_docs \
-H Content-Type: multipart/mixed; boundary="some_boundary" \
-d '
--some_boundary

{
	"Commands": [
		{
			"Id":"shippers/1-A",
			"Name":"some_file",
			"ContentType":"text"
			"Type":"AttachmentPUT",
		}
	]
}

--some_boundary
Command-Type: AttachmentStream

12345
--some_boundary--
'

{CODE-BLOCK/}

Response:

{CODE-BLOCK: http}
HTTP/1.1 201 Created
Server:"nginx"
Date:"Wed, 18 Sep 2019 16:40:43 GMT"
Content-Type:"application/json; charset=utf-8"
Transfer-Encoding:"chunked"
Connection:"keep-alive"
Content-Encoding:"gzip"
Vary:"Accept-Encoding"
Raven-Server-Version:"4.2.4.42"

{
    "Results": [
        {
            "Id": "shippers/1-A",
            "Type": "AttachmentPUT",
            "Name": "some_file",
            "ChangeVector": "A:5973-pITDlhlRaEeJh16dDBREzg, A:1887-0N64iiIdYUKcO+yq1V0cPA, A:6214-xwmnvG1KBkSNXfl7/0yJ1A",
            "Hash": "DHnN2gtPymAUoaFxtgjxfU83O8fxGHw8+H/P+kkPxjg=",
            "ContentType": "text",
            "Size": 5,
            "DocumentChangeVector": "A:5974-pITDlhlRaEeJh16dDBREzg, A:1887-0N64iiIdYUKcO+yq1V0cPA, A:6214-xwmnvG1KBkSNXfl7/0yJ1A"
        }
    ]
}
{CODE-BLOCK/}

---

### Delete Attachment Command

Request:

{CODE-BLOCK: bash}
curl -X POST http://live-test.ravendb.net/databases/Example/bulk_docs \
-H 'Content-Type: application/json' \
-d '{
    "Commands": [
        {
            "Id": "categories/2-A",
            "Name": "image.jpg",
            "ChangeVector": null,
            "Type": "AttachmentDELETE"
        }
	]
}
'

{CODE-BLOCK/}

Response:

{CODE-BLOCK: http}
HTTP/1.1 201 Created
Server:"nginx"
Date:"Wed, 18 Sep 2019 16:44:40 GMT"
Content-Type:"application/json; charset=utf-8"
Transfer-Encoding:"chunked"
Connection:"keep-alive"
Content-Encoding:"gzip"
Vary:"Accept-Encoding"
Raven-Server-Version:"4.2.4.42"

{
    "Results": [
        {
            "Type": "AttachmentDELETE",
            "@id": "categories/2-A",
            "Name": "image.jpg",
            "DocumentChangeVector": "A:5979-pITDlhlRaEeJh16dDBREzg, A:1887-0N64iiIdYUKcO+yq1V0cPA, A:6214-xwmnvG1KBkSNXfl7/0yJ1A"
        }
    ]
}
{CODE-BLOCK/}

{PANEL/}

## Related articles  

### Client API  

- [Get Documents by ID](../../../client-api/rest-api/document-commands/get-documents-by-id)  
- [Get Documents by Prefix](../../../client-api/rest-api/document-commands/get-documents-by-prefix)  
- [Put Documents](../../../client-api/rest-api/document-commands/put-documents)  
- [Delete Document](../../../client-api/rest-api/document-commands/delete-document)  
- [How to Perform Single Document Patch Operations](../../../client-api/operations/patching/single-document)  
- [What is a Collection](../../../client-api/faq/what-is-a-collection)  

### Attachments  

- [What are Attachments](../../../client-api/session/attachments/what-are-attachments)  
