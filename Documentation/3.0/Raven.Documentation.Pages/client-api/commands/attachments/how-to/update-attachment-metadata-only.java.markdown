# Attachments: How to update an attachment metadata only?

**UpdateAttachmentMetadata** is used to update an attachment metadata in a database.

## Syntax

{CODE:java update_1@ClientApi\Commands\Attachments\HowTo\Update.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **key** | String | key under which attachment is stored |
| **etag** | Etag | current attachment etag, used for concurrency checks (`null` to skip check) |
| **metadata** | RavenJObject | attachment metadata |

## Example

{CODE:java update_2@ClientApi\Commands\Attachments\HowTo\Update.java /}

## Related articles

- [How to **get** attachment **metadata** only?](../../../../client-api/commands/attachments/how-to/get-attachment-metadata-only)  
- [PutAttachment](../../../../client-api/commands/attachments/put)  
