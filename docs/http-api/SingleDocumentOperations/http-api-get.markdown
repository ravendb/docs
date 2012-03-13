#HTTP API - Single Document Operations - GET
Perform a GET request to read a JSON document from its URL: 

    > curl -X GET http://localhost:8080/docs/bobs_address

Assuming there is a document with an id of "bobs_address", RavenDB will respond with the contents of that document and an HTTP 200 OK response code: 

    HTTP/1.1 200 OK 
 
    {  
      "FirstName": "Bob",  
      "LastName": "Smith",  
      "Address": "5 Elm St."  
    }

If the URL specified does not point to a valid document, RavenDB follows HTTP conventions and responds with: 

    HTTP/1.1 404 Not Found