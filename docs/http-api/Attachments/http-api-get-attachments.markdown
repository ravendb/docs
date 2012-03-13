#HTTP API - Attachments Operations - GET
Raven supports the concept of attachments. Attachments are binary data that are stored in the database and can be retrieved by a key.
Retrieving an attachment is done by performing an HTTP GET on the following URL:

    > curl -X GET http://localhost:8080/static/{attachment key}

For example, the following request:

    > curl -X GET http://localhost:8080/static/users/ayende.jpg

Will retrieve an attachment whose key is "users/ayende.jpg", the response to the request is the exact byte stream that was stored in a previous [PUT]()//"TODO: link to put attachments" request.