# Attachments: How to Update Only Attachment Metadata

**UpdateAttachmentMetadata** is used to update an attachment's metadata in a database.

## Syntax

{CODE update_1@ClientApi\Commands\Attachments\HowTo\Update.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **key** | string | key under which attachment is stored |
| **etag** | Etag | current attachment Etag, used for concurrency checks (`null` to skip check) |
| **metadata** | RavenJObject | attachment metadata |

## Example

{CODE update_2@ClientApi\Commands\Attachments\HowTo\Update.cs /}

## Related Articles

- [How to **get** attachment **metadata** only?](../../../../client-api/commands/attachments/how-to/get-attachment-metadata-only)  
- [PutAttachment](../../../../client-api/commands/attachments/put)  
