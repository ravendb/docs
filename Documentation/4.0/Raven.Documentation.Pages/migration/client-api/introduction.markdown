# Introduction to 3.x to 4.0 migration

Raven.Client 4.0 is a major upgrade. It's no backward compatible and the API has a lot of breaking changes.

This section discusses the changes that you need to be aware of when migrating your application using 3.x DLLs and recommended actions you need to take.

### Internalized Json.NET usage

Raven.Client doesn't use customized Json.NET any longer. Replace all references to RavenDB Json.NET, like `Raven.Imports.Newtonsoft`, with `Newtonsoft.Json` reference. 
Please use latest version of Json.NET for good performance.

### RavenJObject 

`RavenJObject` is no longer available. Instead, documents are kept internally in blittable format - native, efficient way to represent JSON. You might encounter it when dealing 
with low-level stuff in the client as `BlittableJsonReaderObject` or `BlittableJsonReaderArray`.

### Etags

Documents doesn't have etags. Since documents can be stored in a cluster environment, that is on different nodes, they use a change vector to reflect changes on particular cluster nodes.
The change vector is composed of list of node IDs and a sequential number (per node). The sequential number, still named as the etag, is represeted by `long`.

### Identifiers

The entity identifier can only be `string`. Value types identifiers (`int`, `Guid` etc.) are no longer supported.

### Metadata

Document metadata is no longer sent via HTTP headers. It's part of JSON document under `@metdata` property.

List of changed metadata properties:

* renamed:
  * `Raven-Entity-Name` -> `@collection`
  * `Last-Modified` -> `@last-modified`
  * `__document_id` -> `@id`

* deleted:
  * `Raven-Last-Modified`
  * `ETag`

* new:
  * `@change-vector`
  * `@flags`
