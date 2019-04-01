# Glossary: SynchronizationDetails

RavenFS client API class contains info about an file synchronization that is in progress.

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **FileName** | string | The file name |
| **FileETag** | Etag | The file `Etag` |
| **DestinationUrl** | string | The destination file system URL |
| **Type** | SynchronizationType | The type of performed synchronization: ContentUpdate, MetadataUpdate, Rename or Delete |
