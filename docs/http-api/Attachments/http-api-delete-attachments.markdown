#HTTP API - Attachment Operations - DELETE

Perform a DELETE request to delete the attachment specified by the URL:

    > curl -X DELETE http://localhost:8080/static/users/ayende.jpg

For a successful delete, RavenDB will respond with an HTTP response code 204 No Content:

    "HTTP/1.1 204 No Content"

The only way a delete can fail is if [the etag doesn't match](http://ravendb.net/docs/http-api/http-api-comcurrency), even if the attachment doesn't exist, a delete will still respond with a successful status code.