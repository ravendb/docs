# Attachments: Put

**PutAttachment** is used to insert or update an attachment in a database.

## Syntax

{CODE:java put_1@ClientApi\Commands\Attachments\Put.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **key** | String | unique key under which attachment will be stored |
| **etag** | Etag | current attachment etag, used for concurrency checks (`null` to skip check)  |
| **data** | InputStream | attachment data |
| **metadata** | RavenJObject | attachment metadata |

## Example

{CODE:java put_2@ClientApi\Commands\Attachments\Put.java /}

## Related articles

- [How to **update** attachment **metadata** only?](../../../client-api/commands/attachments/how-to/update-attachment-metadata-only)  
- [GetAttachment](../../../client-api/commands/attachments/get)  
- [DeleteAttachment](../../../client-api/commands/attachments/delete)  
