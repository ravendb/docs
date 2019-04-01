# Glossary: FileHeader

RavenFS client API class contains info about a file.

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Name** | string | File name |
| **Metadata** | RavenJObject | File metadata |
| **OriginalMetadata** | RavenJObject | File metadata used by the session mechanism to track changes |
| **Etag** | Etag | File `Etag` |
| **TotalSize** | long | Total file size |
| **UploadedSize** | long | Number of uploaded bytes |
| **LastModified** | DateTimeOffset | Date of last file modification |
| **CreationDate** | DateTimeOffset | File creation date |
| **FullPath** | string | Full file path |
| **Extension** | string | File extension |
| **Directory** | string | Directory path |
| **IsTombstone** | bool | Indicates if a file has `Raven-Delete-Marker` in metadata |
