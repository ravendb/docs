# Session : How to Enable Optimistic Concurrency

RavenDB defines some simple rules to determine how to handle concurrent or near requests from the HTTP API against the same document. 

## Leveraging Change-Vectors

Every document in RavenDB has a corresponding change vector. This change vector is updated by RavenDB every time the document is changed.

When you want to update a document, you can specify the change vector of the document you were working with in the `If-Match` of the header. For example, using the cURL utility to build the request: 

{CODE-START:plain /}
curl --header "If-Match:A:2-2698NqDtYk67V0Q6otr4pg" -X PUT http://localhost:8080/databases/documentation/docs?id=bobs_address -d "{ FirstName : 'Bob', LastName: 'Smith', Address: '5 Elm St.' }"
{CODE-END /}

If the change vector specified in the header matches the current change vector of the document in RavenDB, then this update will go through successfully.

If the document was updated by someone else before you, then the change vector's won't match and RavenDB will return a conflict error: 

{CODE-START:plain /}
HTTP/1.1 409 Conflict
{
	"url":"/databases/documentation/docs",
	"ExpectedChangeVector":"A:2-2698NqDtYk67V0Q6otr4pg",
	"ActualChangeVector":"A:3-2698NqDtYk67V0Q6otr4pg",
	"error":"Document bobs_address has change vector A:3-2698NqDtYk67V0Q6otr4pg, but Put was called with change vector A:2-2698NqDtYk67V0Q6otr4pg. Optimistic concurrency violation, transaction will be aborted."
}
{CODE-END /}

At this point, the update has been rejected. Nothing on the server has changed. It's up to you to determine how to respond to this conflict.

Change vectors are used for the following commands:

* PUT
* PATCH
* DELETE

When using `If-Match`, the change vector from the client is compared to the current change vector, and if the change vector does not match, a 409 Conflict will be generated.


## Last One in Wins

When the change vector is not specified in the header for a given request, the last request processed for that URL wins.

## Related Articles

- [Glosarry : Change Vector](../../glossary/change-vector)
