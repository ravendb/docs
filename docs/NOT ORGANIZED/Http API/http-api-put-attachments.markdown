#HTTP API - Attachment Operations - PUT

Perform a PUT request to /static/{attachment key} to create the specified attachment at the given URL:

    > curl -X PUT http://localhost:8080/static/users/ayende.jpg -d "[.. binary image data ...]"

For a successful request, RavenDB will respond with the id it generated and an HTTP 201 Created response code:

    HTTP/1.1 201 Created
    Location: /static/users/ayende.jpg

It is important to note that a PUT in RavenDB will always create the specified attachment at the request URL, if necessary overwriting what was there before.

While putting an attachment, it is possible to store metadata about it using HTTP Headers. The following standard HTTP headers will be stored and sent back when the attachment is next retrieved from Raven.

* Allow
* Content-Disposition
* Content-Encoding
* Content-Language
* Content-Location
* Content-MD5
* Content-Range
* Content-Type
* Expires
* Last-Modified

In addition to that, any custom HTTP header will also be stored and sent back to the client on GET requests for the attachment.