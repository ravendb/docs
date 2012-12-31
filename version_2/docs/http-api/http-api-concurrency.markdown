# HTTP API - Revisions and Concurrency with E-Tags
RavenDB defines some simple rules to determine how to handle concurrent or near requests from the HTTP API against the same document. 

## Leveraging E-Tags

Every document in RavenDB has a corresponding e-tag (entity tag). This e-tag is updated by RavenDB every time the document is changed.

When you want to update a document, you can specify the e-tag of the document you were working with in the "If-Match" element of the header. For example, using the cURL utility to build the request: 

{CODE-START:plain /}
    > curl --header "If-Match:dd62a2e0-2744-11df-a9ff-001c251ced36"  
         -X PUT http://localhost:8080/docs/bobs_address -d "{ FirstName : 'Bob', LastName: 'Smith', Address: '5 Elm St.' }"
{CODE-END /}

If the e-tag specified in the header matches the current e-tag of the document in RavenDB, then this update will go through successfully.

If the document was updated by someone else before you, then the e-tag's won't match and RavenDB will return a conflict error: 

{CODE-START:plain /}
    HTTP/1.1 409 Conflict  
    
    {"url":"/docs/bobs_address","actualETag":"dd62a2e0-2744-11df-a9ff-001c251ced36","expectedETag":"ac6ca153-2745-11df-a9ff-001c251ced36",  
         "error":"PUT attempted on document 'bobs_address' using a non current etag"}
{CODE-END /}

At this point, the update has been rejected. Nothing on the server has changed. It's up to you to determine how to respond to this conflict.

Etags are used for the following commands:

* PUT
* PATCH
* DELETE

In each case, before any operation is made, the etag from the client is compared to the current etag, and if the etag does not match, a 409 Conflict will be generated.

## Last One in Wins

When the e-tag is not specified in the header for a given request, the last request processed for that URL wins.

It's recommended that you always specify an e-tag for an update request to ensure that updates are processed as you expect.

Unless your problem space demands it, it's generally not useful to specify an e-tag for deletes.