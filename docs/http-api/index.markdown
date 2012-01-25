# HTTP API

Every document, index and attachment in RavenDB is addressable by a URL on the server, following a simple format.

For example, documents are addressable by document id, under the docs area:

    http://localhost:8080/docs/{document_id}
    
Attachments are addressable by the attachment path, under the static area:

    http://localhost:8080/static/{attachment_path}

Indexes are addressable by index id, under the indexes area:

    http://localhost:8080/indexes/{index_id}

Using Bundles it is possible to add custom HTTP endpoints, and indeed some of the official Bundles shipping with RavenDB do.