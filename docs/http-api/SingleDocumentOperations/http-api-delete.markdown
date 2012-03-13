#HTTP API - Single Document Operations - DELETE
Perform a DELETE request to delete the JSON document specified by the URL: 

    > curl -X DELETE http://localhost:8080/docs/bobs_address

For a successful delete, RavenDB will respond with an HTTP response code 204 No Content: 

    "HTTP/1.1 204 No Content"

The only way a delete can fail is if [the etag doesn't match]()//"TODO:link to concurrency". If the document doesn't exist, a delete will still respond with a successful status code. 

**Hard vs. Soft Deletes**  
Deleting a document through the HTTP API is not reversible. In database terms, it is a "hard" delete.

An alternative approach is to mark a document with a deleted flag and then ignore documents like this in your business logic.

This approach, a "soft" delete, preserves the data intact in RavenDB and can be useful for auditing or undoing a user's actions.

The right approach for you will depend on the problem space that you are modeling. 