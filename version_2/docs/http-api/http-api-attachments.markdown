#HTTP API - Attachment Operations

Raven supports the concept of attachments. The attachments are binary data that are stored in the database and can be retrieved by a key.

## PUT

Perform a PUT request to /static/{attachment key} to create the specified attachment at the given URL:

{CODE-START:plain /}
    > curl -X PUT http://localhost:8080/static/users/ayende.jpg -d "[.. binary image data ...]"
{CODE-END /}

For a successful request, RavenDB will respond with the id it generated and an HTTP 201 Created response code:

{CODE-START:plain /}
    HTTP/1.1 201 Created
    Location: /static/users/ayende.jpg
	ETag: "00000000-0000-1e00-0000-000000000002"
{CODE-END /}

It is important to note that a PUT in RavenDB will always create the specified attachment at the request URL, if necessary overwriting what was there before.

While putting an attachment, it is possible to store metadata about it using HTTP Headers. By default a standard `Content-Type` HTTP header is always stored, however any custom header that you defined will be saved as well. When the attachment is next retrieved by GET request, its metadata are sent back to the client.

## GET
Retrieving an attachment is done by performing an HTTP GET on the following URL:

{CODE-START:plain /}
    > curl -X GET http://localhost:8080/static/{attachment key}
{CODE-END /}

For example, the following request:

{CODE-START:plain /}
    > curl -X GET http://localhost:8080/static/users/ayende.jpg
{CODE-END /}

Will retrieve an attachment whose key is "users/ayende.jpg", the response to the request is the exact byte stream that was stored in a previous [PUT](http://ravendb.net/docs/http-api/attachments/http-api-put-attachments?version=2.0) request.

## DELETE

Perform a DELETE request to delete the attachment specified by the URL:

{CODE-START:plain /}
    > curl -X DELETE http://localhost:8080/static/users/ayende.jpg
{CODE-END /}

For a successful delete, RavenDB will respond with an HTTP response code 204 No Content:

{CODE-START:plain /}
    "HTTP/1.1 204 No Content"
{CODE-END /}

The only way a delete can fail is if [the etag doesn't match](http://ravendb.net/docs/http-api/http-api-concurrency?version=2.0), even if the attachment doesn't exist, a delete will still respond with a successful status code.

##HEAD

To retrieve only metadata of an attachment execute a HEAD request:

{CODE-START:plain /}
	> curl -X HEAD -I http://localhost:8080/static/users/ayende.jpg
{CODE-END /}

For a successful delete, RavenDB will respond with an HTTP response code 200 OK. Metadata will be contained in response headers.

##POST

Perform a POST request to update attachment metadata:

{CODE-START:plain /}
	> curl -X POST --header "Content-Length:0" --header "Author:Ayende" http://localhost:8080/static/users/ayende.jpg
{CODE-END /}

All custom headers that you sent will be associated with an attachment and replace the existing ones.
 
{INFO Note that you have to set `Content-Length` to 0. /}