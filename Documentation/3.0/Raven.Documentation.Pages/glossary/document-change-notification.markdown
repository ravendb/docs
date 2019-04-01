# Glossary: DocumentChangeNotification

### General

This class extends `EventArgs`.

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Type** | [DocumentChangeTypes](../glossary/document-change-notification#documentchangetypes-enum-flags) | Document change type enum |
| **Id** | string | Document identifier |
| **CollectionName** | string | Document's collection name |
| **TypeName** | string | Type name |
| **Etag** | Etag | Etag |
| **Message** | string | Notification payload |

<hr />

# DocumentChangeTypes (enum flags)

### Members

| Name | Value |
| ---- | ----- |
| **None** | `0` |
| **Put** | `1` |
| **Delete** | `2` |
| **BulkInsertStarted** | `4` |
| **BulkInsertEnded** | `8` |
| **BulkInsertError** | `16` |
| **Common** | `Put & Delete` |

