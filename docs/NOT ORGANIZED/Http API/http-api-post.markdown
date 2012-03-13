#HTTP API - Single Document Operations - POST
Perform a POST request to the /docs area to create the specified document and allow RavenDB to assign a unique id to it: 

    > curl -X POST http://localhost:8080/docs -d "{ FirstName: 'Bob', LastName: 'Smith', Address: '5 Elm St' }"

For a successful request, RavenDB will respond with the id it generated and an HTTP 201 Created response code: 

    HTTP/1.1 201 Created  

    {"Key":"5ecec911-4c71-11df-8ec2-001fd08ec235","ETag":"5ecec912-4c71-11df-8ec2-001fd08ec235"}

It is important to note that a repeated POST request for the same document will create that document in a new place, with a new id each time.

A POST to a document URL is an invalid request and RavenDB will return a HTTP 400 Bad Request response code.