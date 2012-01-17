# Deleting existing documents

Once a valid reference to a document has been retrieved, the document can be deleted with a call to `Delete` in the following manner:

{CODE deleting_document_1@Intro\BasicOperations.cs /}

Once again, this results in an HTTP communication as shown below:

    POST /bulk_docs HTTP/1.1
    Accept-Encoding: deflate,gzip
    Content-Type: application/json; charset=utf-8
    Host: 127.0.0.1:8081
    Content-Length: 49
    Expect: 100-continue

    [
      {
        "Key": "blogs/1",
        "Etag": null,
        "Method": "DELETE"
      }
    ]
	
Deletes are final and cannot be rolled back, once committed.

If you have the id of a document, and you don't want to load it just in order to delete it, you can perform a direct delete using the `DatabaseCommands` property available in the `Advanced` section of the session API.

{CODE deleting_document_2@Intro\BasicOperations.cs /}

This results in an HTTP communication as shown below:

    DELETE /docs/posts/1234 HTTP/1.1
    Host: 127.0.0.1:8081
    
    
Unlike session operations, `DatabaseCommands` operations execute immediately, and do not wait for `SaveChanges` to be called.