#HTTP API - Single Document Operations - PUT
Perform a PUT request to /docs/{document_id} to create the specified document with the given document id: 

    > curl -X PUT http://localhost:8080/docs/bobs_address -d "{ FirstName: 'Bob', LastName: 'Smith', Address: '5 Elm St' }"

For a successful request, RavenDB will respond with the id it generated and an HTTP 201 Created response code: 

    HTTP/1.1 201 Created
  
    {"Key":"bobs_address","ETag":"179048f3-4c71-11df-8ec2-001fd08ec235"}

It is important to note that a PUT in RavenDB will always create the specified document at the request URL, if necessary overwriting what was there before.

A PUT request to /docs without specifying the document id in the URL is an invalid request and RavenDB will return a HTTP 400 Bad Request response code. 