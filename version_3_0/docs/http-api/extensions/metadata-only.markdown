#Metadata only

Every RavenDB's response that includes documents together with metadata can be cut off to metadata only. If this is your intention add *?metadata-only=true* parameter to the query string.
In result only document metadata will be contained in the HTTP response to a request. Below there are a few simple use cases:

{CODE-START:json /}
   > curl -X GET http://localhost:8080/docs/?metadata-only=true
{CODE-END /}

{CODE-START:json /}
   > curl -X GET "http://localhost:8080/queries?id=albums/661&id=albums/629&metadata-only=true"
{CODE-END /}

{CODE-START:json /}
   > curl -X GET "http://localhost:8080/indexes/AlbumsByGenre?query=Genre:genres/1&metadata-only=true"
{CODE-END /}