
## Attachments

While the general advice is to store files on a dedicated server or in the cloud whenever possible, some scenarios require storing large blobs in the database itself.

If large chunks of data are part of our entities, we would most probably take a performance hit quite quickly: Whenever a document containing this entity is requested, pulled for indexing or updated, the whole object graph would have to be loaded and processed.

This is where Attachments come in. They offer the luxury of efficiently attaching even a large blob to a RavenDB document. Attachments in RavenDB can have metadata, are replicated between nodes, can be cascade deleted on document deletions and are HTTP cachable.

Even better: with attachments you can load large chunks of data concurrently to loading the entities, so data is displayed without much delay and doesn't suffer from the size of the data attached to it.

### Retrieving attachments

Attachments are handled outside of the Unit of Work, and as such they are not tracked for changes, are not transactional and don't require a session. They are accessed via the `DatabaseCommands` object, which is accessible via both the document store object and the session object:

{CODE retrieving_attachment@ClientApi/Attachments.cs /}

As you can see, loading an attachment from RavenDB is very simple. Each attachment has its own unique key, and all it takes is passing that key to get an Attachment object. In that object you'll find five properties:

* **Func\<Stream\> Data** - the delegate that returns stream of actual data,
* **RavenJObject Metadata** - a dictionary object with the attachment's metadata,
* **Guid Etag** - short for entity tag, a sequential Guid that is being updated every time the stored attachment changes,
* **int Size** - the size of the attachment,
* **string Key** - the attachment name.

### Storing and updating attachments

Adding an attachment uses the exact same process. You call `DatabaseCommands.PutAttachment` with the key to store the attachment at, the actual data and a `RavenJObject` containing metadata key/value pairs:

{CODE putting_attachment@ClientApi/Attachments.cs /}

The second argument, which we passed `null` to, is the Etag. When set, it makes sure the data we send is not persisted using this key unless the current attachment's Etag has that value set. This ensures you don't overwrite an attachment by accident. Set it to `null` whenever putting a new attachment, or when you want to forcibly overwrite existing one.

### Deleting attachments

As you have guessed, this is a one-liner too:

{CODE deleting_attachment@ClientApi/Attachments.cs /}

Same as with updating an attachment, you can specify an Etag to make sure the correct attachment is being removed, and you are not deleting one that has been recently updated by someone else.

### Attachment metadata

#### Retrieve

If you need to grab just metadata of an attachment you can use HEAD method:

{CODE retrieving_attachment_metadata@ClientApi/Attachments.cs /}

In result you will get a filled attachment object except from the Data property. In that case an attempt to use attachment's data will throw an exception.

You are also able to get metadata of all attachments which names start with a specified prefix. You just need to pass the prefix and paging parameters.

{CODE retrieving_attachment_headers_with_prefix@ClientApi/Attachments.cs /}

#### Update

The Client API offers an easy way to update attachment metadata:

{CODE updating_attachment_metadata@ClientApi/Attachments.cs /}

The first parameters is a name of an attachment. The next one is an ETag number that you can provide to be sure that you do an update to a concrete version of an attachment, otherwise pass `null`. If the specified ETag does not match with the existing value in a database then the RavenDB server will return a conflict error. The last argument is new metadata. 

Note that the update is an overriding operation. Take care of sending even those values that you didn't changed it you don't want to loose them.
